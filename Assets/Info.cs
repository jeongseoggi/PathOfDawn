using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Info : MonoBehaviour
{
    List<Slot> slotList = new List<Slot>();
    public GameObject slotPrefab;

    public void Init(Playerable character)
    {
        List<FieldInfo> infos = new List<FieldInfo>();
        Type type = typeof(Playerable);
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        int index = 0;

        foreach (FieldInfo fieldinfo in fieldInfos)
        {
            UIView uiViewAttribute = fieldinfo.GetCustomAttribute<UIView>();
            if (uiViewAttribute != null)
                infos.Add(fieldinfo);
        }

        foreach (FieldInfo fInfo in infos)
        {
            Debug.Log(slotList.Count + "슬롯 리스트 카운트");
            Debug.Log(infos.Count + "인포즈 카운트");
            Slot slot = null;
            if(slotList.Count == infos.Count)
            {
                Debug.Log(index + " " + fInfo.GetValue(character).ToString());
                slotList[index].GetComponent<Slot>().SetValue(fInfo.GetCustomAttribute<UIView>().Name, fInfo.GetValue(character));
                index++;
            }
            else
            {
                slot = Instantiate(slotPrefab, transform).GetComponent<Slot>();
                slotList.Add(slot);
                slot.SetValue(fInfo.GetCustomAttribute<UIView>().Name, fInfo.GetValue(character));
            }
        }
    }

}
