using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySeting : MonoBehaviour
{
    public LobbyPos[] robbyPosArray;

    public void SetLobby()
    {
        for(int i = 0; i < User.instance.Deck.Count; i++)
            robbyPosArray[i].SetObject(User.instance.Deck[i].player_type);

        for(int i = User.instance.Deck.Count; i < robbyPosArray.Length; i++)
            robbyPosArray[i].InitObject();
    }
}
