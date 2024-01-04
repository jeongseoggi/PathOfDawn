using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUIController : MonoBehaviour
{
   public MonsterUISlot[] monsterUISlots;

    private void Start()
    {
        UIManager.instance.monsterUIController = this;

        gameObject.SetActive(false);
    }
}
