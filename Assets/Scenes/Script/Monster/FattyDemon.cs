using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FattyDemon : Monster
{
    public Transform hadTrasform;

    private new void Awake()
    {
        Job = "패티:데몬";
        Level = 0;
        Hp = 40;
        MaxHp = 40;
        Atk = 20;
        Def = 5;
        Dodge = 10;
        Mp = 0;
        MaxMp = 100;
        Speed = 8;
        Aggro = 1;
        MonRecMp = 3;

        base.Awake();
    }

    public override void MonAddValue(int lv)
    {
        Level = lv;
        MaxHp += lv * 20;
        Hp = MaxHp;
        Atk += lv * 4;
        Def += lv * 3;
    }

    new void Update()
    {
        base.Update();
        TransInfo();
    }

    protected void TransInfo()
    {
        skill[1].transform.position = hadTrasform.position;
        skill[1].transform.rotation = hadTrasform.rotation * Quaternion.Euler(-90, 0, 0);
        skill[2].transform.position = hadTrasform.position;
        skill[2].transform.rotation = hadTrasform.rotation * Quaternion.Euler(-90, 0, 0);
    }
}