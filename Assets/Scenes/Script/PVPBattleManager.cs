using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PVPBattleManager : Singleton<PVPBattleManager>
{
    public static List<Playerable> ownerList = new List<Playerable>();

    
    [SerializeField]
    Transform[] masterPlayerPos;
    [SerializeField]
    Transform[] otherPlayerPos;

    public List<Playerable> ownerPlayerableList;
    public List<Playerable> otherPlayerableList;
    public List<Playerable> battleList;

    void Start()
    {
        ownerPlayerableList = new List<Playerable>();
        otherPlayerableList = new List<Playerable>();
        battleList = new List<Playerable>();
        Init();
        StartCoroutine(WaitCam());
    }

    void Init()
    {
        for (int i = 0; i < User.instance.Deck.Count; i++)
        {
            string[] cloneStr = User.instance.Deck[i].gameObject.name.Split("(");
            GameObject clonePlayer = PhotonNetwork.Instantiate(cloneStr[0], transform.position, Quaternion.identity);
            clonePlayer.GetComponent<Character>().DeepCopy(User.instance.Deck[i]);

            Transform[] playerPos = PhotonNetwork.IsMasterClient ? masterPlayerPos : otherPlayerPos;
            clonePlayer.transform.position = playerPos[i].position;
            if (PhotonNetwork.IsMasterClient)
                clonePlayer.transform.LookAt(otherPlayerPos[i]);
            else
                clonePlayer.transform.LookAt(masterPlayerPos[i]);

            ownerList.Add(clonePlayer.GetComponent<Playerable>());
        }
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
        foreach (Playerable player in ownerList)
        {
            battleList.Add(player);
        }
        battleList.Sort();
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
