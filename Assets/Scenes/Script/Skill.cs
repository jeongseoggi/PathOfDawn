using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BATTLE_TYPE
{
    PVE,
    PVP
}


public class Skill : MonoBehaviour
{
    protected GameObject parent;
    protected Vector3 prevPos;
    public float skillDamage;
    public float skillPercent;
    public BATTLE_TYPE battle_type = BATTLE_TYPE.PVE;
    public Playerable playerable;

    protected void Awake()
    {
        gameObject.SetActive(false);
        parent = transform.parent.gameObject;
        prevPos = transform.position;
    }

    private void Update()
    {

    }

    public virtual void Cast()
    {

    }
}