using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPositionSetComponent : MonoBehaviour
{
    public Transform[] positions;

    private void Start()
    {
        for(int i = 0; i < User.instance.Deck.Count; i++)
        {
            User.instance.Deck[i].gameObject.SetActive(true);
            User.instance.Deck[i].gameObject.transform.position = positions[i].position;
            User.instance.Deck[i].gameObject.transform.SetParent(positions[i]);
        }
    }
}
