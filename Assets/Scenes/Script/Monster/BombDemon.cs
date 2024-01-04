using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BombDemon : Monster
{
    private new void Awake()
    {
        Job = "ÆøÅº:µ¥¸ó";
        Level = 0;
        Hp = 30;
        MaxHp = 30;
        Atk = 15;
        Def = 5;
        Dodge = 10;
        Mp = 0;
        MaxMp = 100;
        Speed = 5;
        Aggro = 1;
        MonRecMp = 3;

        base.Awake();
    }

    public override void MonAddValue(int lv)
    {
        Level = lv;
        MaxHp += lv * 10;
        Hp = MaxHp;
        Atk += lv * 5;
        Def += lv * 1;
    }

    new void Update()
    {
        base.Update();
    }
}