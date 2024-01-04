using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntDemon : Monster
{
    private new void Awake()
    {
        Job = "그런트:데몬";
        Hp = 20;
        MaxHp = 20;
        Atk = 15;
        Def = 0;
        Dodge = 10;
        Mp = 0;
        MaxMp = 100;
        Speed = 6;
        Aggro = 1;
        MonRecMp = 4;

        base.Awake();
    }

    public override void MonAddValue(int lv)
    {
        Level = lv;
        MaxHp += lv * 20;
        Hp = MaxHp;
        Atk += lv * 4;
        Def += lv * 4;
    }

    new void Update()
    {
        base.Update();
    }
}
