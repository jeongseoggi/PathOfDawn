using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PVPBattleManager : MonoBehaviourPunCallbacks
{
    public static List<Playerable> ownerList = new List<Playerable>();

    [SerializeField]
    Transform[] masterPlayerPos;
    [SerializeField]
    Transform[] otherPlayerPos;

    public List<Playerable> ownerPlayerableList;
    public List<Playerable> otherPlayerableList;
    public List<int> battleList;

    public Dictionary<int, Playerable> masterDic;
    public Dictionary<int, Playerable> clientDic;
    void Start()
    {
        ownerPlayerableList = new List<Playerable>();
        otherPlayerableList = new List<Playerable>();
        masterDic = new Dictionary<int, Playerable>();
        clientDic = new Dictionary<int, Playerable>();

        battleList = new List<int>();

        if(photonView.IsMine)
            photonView.RPC("Init", RpcTarget.All);

        StartCoroutine(WaitCam());
    }


    [PunRPC]
    void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            string[] cloneStr = User.instance.Deck[i].gameObject.name.Split("(");
            GameObject clonePlayer = PhotonNetwork.Instantiate(cloneStr[0], transform.position, Quaternion.identity);
            ownerPlayerableList.Add(User.instance.Deck[i]);
            Transform[] playerPos = PhotonNetwork.IsMasterClient ? masterPlayerPos : otherPlayerPos;
            clonePlayer.transform.position = playerPos[i].position;
            if (clonePlayer.GetComponent<PhotonView>().IsMine)
            {
                clonePlayer.transform.LookAt(otherPlayerPos[i]);
            }
            else
            {
                clonePlayer.transform.LookAt(masterPlayerPos[i]);
            }
            photonView.RPC("Test", RpcTarget.AllViaServer, clonePlayer.GetComponent<PhotonView>().ViewID, i);
        }
    }

    [PunRPC]
    public void Test(int id, int index)
    {
        battleList.Add(id);
        if (PhotonNetwork.IsMasterClient)
            masterDic.Add(id, User.instance.Deck[index]);
        else
            clientDic.Add(id, User.instance.Deck[index]);
    }

    [PunRPC]
    public void TT()
    {
        for (int i = 0; i< battleList.Count; i++)
        {
            if(PhotonView.Find(battleList[i]).IsMine)
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("ÇöÀç ÀÎµ¦½º" + i + "Æ÷Åæºä ¾ÆÀÌµð" + battleList[i] + "¼ÒÀ¯ÁÖ(t/f) :" + photonView.IsMine);
                    PhotonView.Find(battleList[i]).GetComponent<Playerable>().DeepCopy(masterDic[battleList[i]]);
                }
                else
                {
                    Debug.Log("ÇöÀç ÀÎµ¦½º" + i + "Æ÷Åæºä ¾ÆÀÌµð" + battleList[i] + "¼ÒÀ¯ÁÖ(t/f) :" + photonView.IsMine);
                    PhotonView.Find(battleList[i]).GetComponent<Playerable>().DeepCopy(clientDic[battleList[i]]);
                }
            }
        }
    }

    [PunRPC]
    public void Sort()
    {
        Debug.Log(battleList.Count);
        int rootIndex = 0;
        int index = 0;

        while (rootIndex < battleList.Count)
        {
            if (PhotonView.Find(battleList[rootIndex]).GetComponent<Playerable>().Speed >
                PhotonView.Find(battleList[index]).GetComponent<Playerable>().Speed)
            {
                int temp = battleList[rootIndex];
                battleList[rootIndex] = battleList[index];
                battleList[index] = temp;
            }
            index++;

            if (index == battleList.Count)
            {
                index = 0;
                rootIndex++;
            }

        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.instance.pvpTurnTableUIController.photonView.RPC("MoveTurnTable", RpcTarget.All);
        }
    }




    [PunRPC]
    public void BattleSet()
    {
        UIManager.instance.pvpTurnTableUIController.gameObject.SetActive(true);
        UIManager.instance.pvpTurnTableUIController.GetComponent<PhotonView>().RPC("Init", RpcTarget.All);
    }

    IEnumerator WaitCam()
    {
        yield return new WaitForSeconds(3);
        photonView.RPC("TT", RpcTarget.All);
        yield return new WaitForSeconds(4);
        photonView.RPC("Sort", RpcTarget.All);
        if (photonView.IsMine)
            photonView.RPC("BattleSet", RpcTarget.AllBuffered);
        yield return null;
    }
}
