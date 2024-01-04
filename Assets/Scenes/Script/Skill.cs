using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    protected GameObject parent;
    protected Vector3 prevPos;
    public float skillDamage;
    public float skillPercent;

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