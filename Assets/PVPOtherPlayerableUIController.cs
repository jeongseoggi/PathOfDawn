using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPOtherPlayerableUIController : MonoBehaviourPunCallbacks
{
    public PlayerableUISlot[] playerableUISlots;
    public bool isMaster;
    int i = 0;
    Dictionary<int, Playerable> targetDic;
    private void Start()
    {
        UIManager.instance.otherPlayerUIController = this;
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void Set()
    {
        foreach (KeyValuePair<int, Playerable> kv in PVPBattleManager.instance.clientDic)
        {
            Debug.Log(i);
            playerableUISlots[i].Init(kv.Key);
            i++;
        }
        i = 0;
    }
}
