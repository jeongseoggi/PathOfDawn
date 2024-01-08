using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerableUIController : MonoBehaviourPunCallbacks
{
    public PlayerableUISlot[] playerableUISlots;
    public bool isMaster;
    private void Start()
    {

        UIManager.instance.playerableUIController = this;

        for (int i = 0; i < User.instance.Deck.Count; i++)
            playerableUISlots[i].Init(User.instance.Deck[i]);

        gameObject.SetActive(false);
    }
}
