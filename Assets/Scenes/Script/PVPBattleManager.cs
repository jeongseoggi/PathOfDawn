using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PVPBattleManager : MonoBehaviourPunCallbacks
{
    public static PVPBattleManager instance;
    public int[] ownerList;
    public int[] test;

    [SerializeField]
    Transform[] masterPlayerPos;
    [SerializeField]
    Transform[] otherPlayerPos;

    public List<Playerable> ownerPlayerableList;
    public List<Playerable> otherPlayerableList;
    public List<Playerable> battleList;


    public Dictionary<int, Playerable> dic = new Dictionary<int, Playerable>();
    void Start()
    {
        instance = this;
        ownerList = new int[3];
        test = new int[6];
        ownerPlayerableList = new List<Playerable>();
        otherPlayerableList = new List<Playerable>();
        battleList = new List<Playerable>();
        if (photonView.IsMine)
            photonView.RPC("Init", RpcTarget.All);
        StartCoroutine(WaitCam());
    }


    [PunRPC]
    void Init()
    {
        for (int i = 0; i < PVPManager.instance.battleList.Length; i++)
            test[i] = PVPManager.instance.battleList[i];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.instance.pvpTurnTableUIController.photonView.RPC("MoveTurnTable", RpcTarget.All);
            battleList[2].Hp -= 50;
        }
    }

    [PunRPC]
    public void BattleSet()
    {
        UIManager.instance.pvpTurnTableUIController.gameObject.SetActive(true);
        int index = 0;
        foreach(int e in ownerList)
        {
            test[index] = e;
            index++;
        }

        for(int i = 0; i < dic.Count; i++)
        {
            battleList.Add(dic[ownerList[i]]);
        }



        battleList.Sort();
        UIManager.instance.pvpTurnTableUIController.GetComponent<PhotonView>().RPC("Init", RpcTarget.All);
    }

    IEnumerator WaitCam()
    {
        yield return new WaitForSeconds(10);
        if(photonView.IsMine)
            photonView.RPC("BattleSet", RpcTarget.All);
        yield return null;
    }
}
