using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnPressGrow : MonoBehaviour
{
    [SerializeField] Image[] growImages;

    public void SetBtnGrow(int index)
    {
        if (index > growImages.Length)
            return;

        for(int i = 0; i < growImages.Length; i++)
        {
            if (i == index)
                growImages[i].gameObject.SetActive(!growImages[i].gameObject.activeSelf);
            else
                growImages[i].gameObject.SetActive(false);
        }
    }
}
