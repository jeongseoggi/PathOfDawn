using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PVPBattleManager : Singleton<PVPBattleManager>, IPunObservable
{
    [SerializeField]
    Transform[] masterPlayerPos;
    [SerializeField]
    Transform[] otherPlayerPos;

    public List<int> battleList;

    public Dictionary<int, Playerable> masterDic;
    public Dictionary<int, Playerable> clientDic;

    public Camera mainCam;
    public LayerMask layerMask;

    public GameObject targetSelectEffect;
    public int curTurnCharacterID;
    public int targetCharacter;
    public string myName;
    public int TargetCharacter
    {
        get => targetCharacter;
        set
        {
            targetCharacter = value;

            if (targetCharacter == 0)
                return;

            if (!PhotonView.Find(targetCharacter).GetComponent<PhotonView>().IsMine)
            {
                UIManager.instance.pVpBehaviorUIController.gameObject.SetActive(true);
                targetSelectEffect.transform.position = PhotonView.Find(targetCharacter).GetComponent<Playerable>().gameObject.transform.position;
                targetSelectEffect.SetActive(true);
            }
            else
            {
                UIManager.instance.pVpBehaviorUIController.gameObject.SetActive(false);
            }
        }
    }

    public int battleCount = 0;
    void Start()
    {
        myName = PhotonNetwork.IsMasterClient ? PhotonNetwork.MasterClient.NickName : PhotonNetwork.CurrentRoom.Players[2].NickName;
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
            foreach(GameObject skill in clonePlayer.GetComponent<Character>().skill)
                skill.GetComponent<Skill>().battle_type = BATTLE_TYPE.PVP;
            Transform[] playerPos = PhotonNetwork.IsMasterClient ? masterPlayerPos : otherPlayerPos;
            clonePlayer.transform.position = playerPos[i].position;
            if (PhotonNetwork.IsMasterClient)
            {
                clonePlayer.transform.LookAt(otherPlayerPos[i]);
            }
            else
            {
                clonePlayer.transform.LookAt(masterPlayerPos[i]);
            }

            photonView.RPC("AddList", RpcTarget.All, clonePlayer.GetComponent<PhotonView>().ViewID, i);
        }
    }

    [PunRPC]
    public void AddList(int id, int index)
    {
        battleList.Add(id);
        if(id > 1000 && id < 2000)
        {
            masterDic.Add(id, PhotonView.Find(id).GetComponent<Playerable>());
        }
        else 
            clientDic.Add(id, PhotonView.Find(id).GetComponent<Playerable>());
    }

    [PunRPC]
    public void Copy()
    {
        foreach (KeyValuePair<int, Playerable> kv in masterDic)
        {
            Debug.Log("마스터" + masterDic[kv.Key].Level);
            Debug.Log(kv.Key);
        }
        foreach (KeyValuePair<int, Playerable> kv in clientDic)
        {
            Debug.Log("클라 : " + clientDic[kv.Key].Level);
            Debug.Log(kv.Key);
        }

        for (int i = 0; i < battleList.Count; i++)
        {
            Debug.Log(i);
            if (battleList[i] > 1000 && battleList[i] < 2000)
            {
                PhotonView.Find(battleList[i]).GetComponent<Playerable>().DeepCopy(masterDic[battleList[i]]);
            }
            else
            {
                Debug.Log("else" + i);
                PhotonView.Find(battleList[i]).GetComponent<Playerable>().DeepCopy(clientDic[battleList[i]]);
            }
        }
    }

    [PunRPC]
    public void BattleStart()
    {
        Debug.Log(clientDic.Count);
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
        if (Input.GetMouseButtonDown(0) && PhotonView.Find(curTurnCharacterID).Owner.NickName.Equals(myName))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log(hit.collider.name);
                if (!hit.collider.GetComponent<PhotonView>().IsMine)
                    TargetCharacter = hit.collider.GetComponent<PhotonView>().ViewID;
            }
        }
    }

    [PunRPC]
    public void BattleSet()
    {
        UIManager.instance.pvpTurnTableUIController.gameObject.SetActive(true);
        UIManager.instance.pvpTurnTableUIController.Init();
    }

    public void BattleDamage(float damage, ATTACK_TYPE atkType)
    {
        if (TargetCharacter == 0)
            return;

        if (atkType == ATTACK_TYPE.NORMAL || atkType == ATTACK_TYPE.SKILLATK)
        {
            int dodgeRandomValue = UnityEngine.Random.Range(0, 100);
            if (dodgeRandomValue <= PhotonView.Find(TargetCharacter).GetComponent<Playerable>().Dodge)
                return;

            PhotonView.Find(TargetCharacter).GetComponent<Playerable>().TakeDamage(damage);
            return;
        }
        else if (atkType == ATTACK_TYPE.ULTIMATEATK)
        {
            Dictionary<int, Playerable> tagetDic = PhotonNetwork.IsMasterClient ? masterDic : clientDic;
            if (PhotonView.Find(TargetCharacter).GetComponent<Playerable>().TryGetComponent<Playerable>(out var playerable))
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


    [PunRPC]
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
        UIManager.instance.pvpTurnTableUIController.MoveTurnTable();
    }

    IEnumerator WaitCam()
    {
        yield return new WaitForSeconds(3);
        photonView.RPC("Copy", RpcTarget.All);
        yield return new WaitForSeconds(4);
        photonView.RPC("Sort", RpcTarget.All);
        yield return new WaitForSeconds(2);
        if (photonView.IsMine)
            photonView.RPC("BattleSet", RpcTarget.AllBuffered);
        BattleStart();

        yield return null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(TargetCharacter);
        else
            TargetCharacter = (int)stream.ReceiveNext();
    }
}
