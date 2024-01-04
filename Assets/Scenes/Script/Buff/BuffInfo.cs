using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffInfo : MonoBehaviour
{
    public BuffController controller;
    public BuffInfo[] buffs;
    
    public void Awake()
    {
        controller = this.GetComponentInParent<BuffController>();
        for(int i = 0; i < buffs.Length; i++)
        {
            buffs[i].gameObject.SetActive(false);
        }
    }

    public void SetBuffSelectZone()
    {
        controller.buffSelectIn();
        RandomItem();
    }

    public virtual void BuffSelect()
    {
        //controller.buffSelectOut();
    }

    public void RandomItem()
    {
        int[] randomItem = new int[3];
        while (true)
        {
            randomItem[0] = Random.Range(0, 3);
            randomItem[1] = Random.Range(0, 3);
            randomItem[2] = Random.Range(0, 3);

            if (randomItem[0] != randomItem[1] && randomItem[1] != randomItem[2]
                && randomItem[0] != randomItem[2])
                break;
        }

        for (int i = 0; i < randomItem.Length; i++)
        {
            BuffInfo ranItem = buffs[randomItem[i]];

            ranItem.gameObject.SetActive(true);
        }
    }
}
