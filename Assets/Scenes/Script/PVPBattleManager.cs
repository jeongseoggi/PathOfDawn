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

        photonView.RPC("TT", RpcTarget.AllViaServer);
    }


    [PunRPC]
    void Init()
    {
        for(int i = 0; i < 3; i++)
        {
            string[] cloneStr = User.instance.Deck[i].gameObject.name.Split("(");
            GameObject clonePlayer = PhotonNetwork.Instantiate(cloneStr[0], transform.position, Quaternion.identity);
            Transform[] playerPos = PhotonNetwork.IsMasterClient ? masterPlayerPos : otherPlayerPos;
            clonePlayer.transform.position = playerPos[i].position;
            if (PhotonNetwork.IsMasterClient)
            {
                clonePlayer.transform.LookAt(otherPlayerPos[i]);
                masterDic.Add(clonePlayer.GetComponent<PhotonView>().ViewID, User.instance.Deck[i]);
            }
            else
            {
                clonePlayer.transform.LookAt(masterPlayerPos[i]);
                clientDic.Add(clonePlayer.GetComponent<PhotonView>().ViewID, User.instance.Deck[i]);
            }
            photonView.RPC("Test", RpcTarget.AllViaServer, clonePlayer.GetComponent<PhotonView>().ViewID);
        }

    }
    [PunRPC]
    public void Test(int id)
    {
        battleList.Add(id);
    }

    [PunRPC]
    public void TT()
    {
        Debug.Log(battleList.Count);
        for(int i = 0; i< battleList.Count; i++)
        {
            if (PhotonView.Find(battleList[i]).IsMine)
                PhotonView.Find(battleList[i]).GetComponent<Playerable>().DeepCopy(User.instance.Deck[i]);
        }
    }
   


    [PunRPC]
    public void Sort()
    {
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
        yield return new WaitForSeconds(7);
        if (photonView.IsMine)
            photonView.RPC("BattleSet", RpcTarget.AllBuffered);
        yield return null;
    }
}
