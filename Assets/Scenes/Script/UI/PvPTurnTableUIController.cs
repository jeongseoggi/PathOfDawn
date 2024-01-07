using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PvPTurnTableUIController : MonoBehaviourPunCallbacks
{
    public List<GameObject> images;
    public List<Vector3> imagesPos;
    public Vector3[] imgPrevPos;
    public PVPBattleManager pb;

    public Queue<IEnumerator> onDieChangeCoQueue;


    private void Start()
    {
        UIManager.instance.pvpTurnTableUIController = this;
        imagesPos = new List<Vector3>();
        imgPrevPos = new Vector3[6];

        gameObject.SetActive(false);
        onDieChangeCoQueue = new Queue<IEnumerator>();
    }


    IEnumerator QueueControlCo()
    {
        while (true)
        {
            if (onDieChangeCoQueue.Count > 0)
            {
                yield return StartCoroutine(onDieChangeCoQueue.Dequeue());

            }
            yield return null;
        }
    }

    [PunRPC]
    public void Init()
    {
        StartCoroutine(QueueControlCo());
        for (int i = 0; i < pb.battleList.Count; i++)
        {
            images.Add(transform.GetChild(i).gameObject);
            images[i].GetComponent<TurnIcon>().owner = PhotonView.Find(pb.battleList[i]).GetComponent<Playerable>();
            if (imgPrevPos[pb.battleList.Count - 1] == Vector3.zero)
                imgPrevPos[i] = images[i].transform.position;
        }

        for (int i = 0; i < pb.battleList.Count; i++)
        {
            int index = i;
            imagesPos.Add(imgPrevPos[i]);
            images[i].gameObject.SetActive(true);
            images[i].transform.GetChild(0).GetComponent<Image>().sprite = PhotonView.Find(pb.battleList[i]).GetComponent<Playerable>().character_image;
            PhotonView.Find(pb.battleList[index]).GetComponent<Playerable>().OnDie += () =>
            {
                if (pb.battleList.Count > 0)
                    photonView.RPC("OnDieTurnTable", RpcTarget.All, PhotonView.Find(pb.battleList[index]).GetComponent<PhotonView>().ViewID);
            };
        }
    }
    [PunRPC]
    public void MoveTurnTable()
    {
        GameObject c = images[0].GetComponentInParent<Transform>().gameObject;
        images.RemoveAt(0);
        images.Add(c);

        int index = 0;
        foreach (GameObject t in images)
        {
            StartCoroutine(ChangeCO(index, t));
            index++;
            if (index >= imagesPos.Count)
                break;
        }
    }

    [PunRPC]
    public void OnDieTurnTable(int id)
    {
        Playerable diePlayer = PhotonView.Find(id).GetComponent<Playerable>();
        int removeIndex = 0;
        foreach (GameObject go in images)
        {
            if (go.GetComponent<TurnIcon>().owner == diePlayer)
            {
                go.SetActive(false);
                images.RemoveAt(removeIndex); //°¡Á¤ 3
                onDieChangeCoQueue.Enqueue(ChangeCO(removeIndex, go));
                break;
            }
            removeIndex++;
        }
    }

    IEnumerator ChangeCO(int i, GameObject image)
    {
        while (Vector3.Distance(image.transform.position, imagesPos[i]) >= 0.1f)
        {
            image.transform.position
               = Vector3.Lerp(image.transform.position, imagesPos[i], Time.deltaTime * 5.0f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Clear()
    {
        images.Clear();
        imagesPos.Clear();
        onDieChangeCoQueue.Clear();
    }

    public override void OnDisable()
    {
        if (images.Count > 0)
        {
            int index = 0;
            foreach (Vector3 v in imgPrevPos)
            {
                transform.GetChild(index).transform.position = imgPrevPos[index];
                index++;
            }
        }
    }
}