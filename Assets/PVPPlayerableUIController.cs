using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPPlayerableUIController : MonoBehaviourPunCallbacks
{
    public PlayerableUISlot[] playerableUISlots;
    public bool isMaster;
    private void Start()
    {
        UIManager.instance.pVpPlayerableUIController = this;
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void Set()
    {
        Dictionary<int, Playerable> targetDic = isMaster ? PVPBattleManager.instance.masterDic : PVPBattleManager.instance.clientDic;
        int i = 0;
        foreach (KeyValuePair<int, Playerable> kv in targetDic)
        {
            Debug.Log(i);
            if (i >= playerableUISlots.Length)
                i = 0;
            if (photonView.IsMine)
                playerableUISlots[i].Init(kv.Key);
            i++;
        }
    }
}
