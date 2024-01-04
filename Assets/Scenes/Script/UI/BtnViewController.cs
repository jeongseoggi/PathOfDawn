using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnViewController : MonoBehaviour
{
    public Image[] LockImages => lockImages;
    public Image[] lockImages;

    public void Init()
    {
        UIManager.instance.setBtnDel += SetBtn;
        Debug.Log(gameObject.name + "DEBUG_1");
    }

    public void SetBtn(int index)
    {
        for (int i = 0; i < index; i++)
            lockImages[i]?.gameObject.SetActive(false);
    }
}
