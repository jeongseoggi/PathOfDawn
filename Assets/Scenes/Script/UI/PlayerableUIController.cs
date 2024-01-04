using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerableUIController : MonoBehaviour
{
    public PlayerableUISlot[] playerableUISlots;

    private void Start()
    {
        UIManager.instance.playerableUIController = this;

        for (int i = 0; i < User.instance.Deck.Count; i++)
            playerableUISlots[i].Init(User.instance.Deck[i]);

        gameObject.SetActive(false);
    }
}
