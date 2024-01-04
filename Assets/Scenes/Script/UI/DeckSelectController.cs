using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckSelectController : MonoBehaviour
{
    public LobbySeting robbySeting;

    private void Start()
    {
        UIManager.instance.deckSelectController = this;
    }

    public void SetDeck(int num)
    {
        User.instance.SetDeck(num);
        robbySeting.SetLobby();
    }
}
