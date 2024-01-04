using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpDemon : Monster
{
    private new void Awake()
    {
        Job = "임프:데몬";
        Hp = 10;
        MaxHp = 10;
        Atk = 10;
        Def = 0;
        Dodge = 1;
        Mp = 0;
        MaxMp = 100;
        Speed = 3;
        Aggro = 1;
        MonRecMp = 2;

        base.Awake();
    }

    public override void MonAddValue(int lv)
    {
        Level = lv;
        MaxHp += lv * 7;
        Hp = MaxHp;
        Atk += lv * 2;
        Def += lv * 1;
    }

    new void Update()
    {
        base.Update();
    }
}
