using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossDemon : Monster
{
    private new void Awake()
    {
        Job = "보스:데몬";
        Hp = 100;
        MaxHp = 100;
        Atk = 30;
        Def = 10;
        Dodge = 10;
        Mp = 0;
        MaxMp = 300;
        Speed = 10;
        Aggro = 1;
        MonRecMp = 6;

        base.Awake();
    }

    public override void MonAddValue(int lv)
    {
        Level = lv;
        MaxHp += lv * 50;
        Hp = MaxHp;
        Atk += lv * 10;
        Def += lv * 6;
    }

    new void Update()
    {
        base.Update();
    }
}