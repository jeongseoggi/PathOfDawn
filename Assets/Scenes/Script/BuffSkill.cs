using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BUFF_TYPE
{
    ADDATKBUF,
    ADDDEFBUF,
    HEALBUF
}


public class BuffSkill : Skill
{
    public BUFF_TYPE type;

    public new void Awake()
    {
        base.Awake();
    }


    public override void Cast()
    {
        if(type == BUFF_TYPE.ADDATKBUF)
        {
            foreach (Playerable player in User.instance.Deck)
            {
                player.Atk += 3;
            }
        }
        if(type == BUFF_TYPE.ADDDEFBUF)
        {
            foreach (Playerable player in User.instance.Deck)
            {
                player.Def += 2;
            }
        }
        if(type == BUFF_TYPE.HEALBUF)
        {
            foreach(Playerable player in User.instance.Deck)
            {
                player.Hp += 5;
            }
        }
    }
}
