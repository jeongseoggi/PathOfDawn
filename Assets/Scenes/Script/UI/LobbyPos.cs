using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPos : MonoBehaviour
{
    public Playerable[] selectObjects;

    public GameObject selectEffect;

    void Start()
    {
        selectObjects = GetComponentsInChildren<Playerable>();
        InitObject();
    }

    public void InitObject()
    {
        foreach (Playerable p in selectObjects)
        {
            p.gameObject.SetActive(false);
            selectEffect.SetActive(false);
        }
    }

    public void SetObject(PLAYER_TYPE type)
    {
        foreach(Playerable p in selectObjects)
        {
            if (p.player_type == type)
            {
                p.gameObject.SetActive(true);
                selectEffect.SetActive(true);
            }
            else
                p.gameObject.SetActive(false);
        }
    }
}
