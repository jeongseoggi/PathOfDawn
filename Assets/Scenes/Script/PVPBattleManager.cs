using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    public List<int> battleList;

    public Dictionary<int, Playerable> masterDic;
    public Dictionary<int, Playerable> clientDic;

    public Camera mainCam;


    public GameObject targetSelectEffect;
    public int curTurnCharacterID;
    public Playerable targetCharacter;
    public Playerable TargetCharacter
    {
        get => targetCharacter;
        set
        {
            targetCharacter = value;

            if (targetCharacter == null)
                return;

            if (!targetCharacter.GetComponent<PhotonView>().IsMine)
            {
                UIManager.instance.pVpBehaviorUIController.gameObject.SetActive(true);
                targetSelectEffect.transform.position = targetCharacter.transform.position;
                targetSelectEffect.SetActive(true);
            }
            else
            {
                UIManager.instance.behaviorUIController.gameObject.SetActive(false);
            }
        }
    }

    public int battleCount = 0;
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
        for (int i = 0; i < User.instance.Deck.Count; i++)
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
            photonView.RPC("AddList", RpcTarget.AllViaServer, clonePlayer.GetComponent<PhotonView>().ViewID, i);
        }
    }

    [PunRPC]
    public void AddList(int id, int index)
    {
        battleList.Add(id);
        if (PhotonNetwork.IsMasterClient)
            masterDic.Add(id, User.instance.Deck[index]);
        else
            clientDic.Add(id, User.instance.Deck[index]);
    }

    [PunRPC]
    public void Copy()
    {
        for (int i = 0; i< battleList.Count; i++)
        {
            if(PhotonView.Find(battleList[i]).IsMine)
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    PhotonView.Find(battleList[i]).GetComponent<Playerable>().DeepCopy(masterDic[battleList[i]]);
                }
                else
                {
                    PhotonView.Find(battleList[i]).GetComponent<Playerable>().DeepCopy(clientDic[battleList[i]]);

                }
            }
        }
    }

    [PunRPC]
    public void BattleStart()
    {
        if (photonView.IsMine)
        {
            UIManager.instance.pVpPlayerableUIController.isMaster = true;
            UIManager.instance.otherPlayerUIController.isMaster = false;
        }
        else
        {
            UIManager.instance.pVpPlayerableUIController.isMaster = false;
            UIManager.instance.otherPlayerUIController.isMaster = true;
        }



        UIManager.instance.pVpPlayerableUIController.gameObject.SetActive(true);
        UIManager.instance.otherPlayerUIController.gameObject.SetActive(true);
        UIManager.instance.pVpPlayerableUIController.Set();
        UIManager.instance.otherPlayerUIController.Set();
        curTurnCharacterID = battleList[0]; 
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
        if (Input.GetMouseButtonDown(0) && PhotonView.Find(battleList[battleCount]).IsMine)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << 10))
            {
                if(!hit.collider.GetComponent<PhotonView>().IsMine)
                    TargetCharacter = hit.collider.GetComponent<Playerable>();
            }
        }
    }

    [PunRPC]
    public void BattleSet()
    {
        UIManager.instance.pvpTurnTableUIController.gameObject.SetActive(true);
        UIManager.instance.pvpTurnTableUIController.GetComponent<PhotonView>().RPC("Init", RpcTarget.All);
        BattleStart();
    }

    public void BattleDamage(float damage, ATTACK_TYPE atkType)
    {
        if (TargetCharacter == null)
            return;

        if (atkType == ATTACK_TYPE.NORMAL || atkType == ATTACK_TYPE.SKILLATK)
        {
            int dodgeRandomValue = UnityEngine.Random.Range(0, 100);
            if (dodgeRandomValue <= TargetCharacter.Dodge)
                return;

            TargetCharacter.TakeDamage(damage);
            return;
        }
        else if (atkType == ATTACK_TYPE.ULTIMATEATK)
        {
            Dictionary<int, Playerable> tagetDic = PhotonNetwork.IsMasterClient ? masterDic : clientDic;
            if (TargetCharacter.TryGetComponent<Playerable>(out var playerable))
            {
                List<Playerable> copyList = new List<Playerable>();
                foreach (KeyValuePair<int, Playerable> kv in tagetDic)
                {
                    copyList.Add(tagetDic[kv.Key]);
                }

                foreach (Playerable player in copyList)
                    player?.TakeDamage(damage);
            }
        }
    }



    public void NextOrder()
    {
        if (battleList.Count <= 0)
            return;

        battleCount++;
        if (battleCount >= battleList.Count)
            battleCount = 0;


        if (PhotonView.Find(battleList[battleCount]).GetComponent<Playerable>().isDie == true)
        {
            NextOrder();
            return;
        }
        curTurnCharacterID = battleList[battleCount];

        StartCoroutine(WaitSeCo());
    }

    IEnumerator WaitSeCo()
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.instance.pvpTurnTableUIController.GetComponent<PhotonView>().RPC("MoveTurnTable", RpcTarget.All);
    }

    IEnumerator WaitCam()
    {
        yield return new WaitForSeconds(3);
        photonView.RPC("Copy", RpcTarget.All);
        yield return new WaitForSeconds(4);
        photonView.RPC("Sort", RpcTarget.All);
        if (photonView.IsMine)
            photonView.RPC("BattleSet", RpcTarget.AllBuffered);
        yield return null;
    }
}
