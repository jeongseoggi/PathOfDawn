using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPPlayerableUIController : MonoBehaviourPunCallbacks
{
    public PlayerableUISlot[] playerableUISlots;
    public bool isMaster;
    int i = 0;
    Dictionary<int, Playerable> targetDic;
    private void Start()
    {
        UIManager.instance.pVpPlayerableUIController = this;
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void Set()
    {
        foreach (KeyValuePair<int, Playerable> kv in PVPBattleManager.instance.masterDic)
        {
            Debug.Log(i);
            playerableUISlots[i].Init(kv.Key);
            i++;
        }
        i = 0;
        Debug.Log("≈ª√‚");
    }
}
