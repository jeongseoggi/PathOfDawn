using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TARGET_TYPE
{
    None,
    SINGLE,
    FULL
}


public class TargetSkill : Skill
{
    public TARGET_TYPE type;
    public GameObject tarImpact;
    public Transform prevImpactPos;
    bool isEnd;

    private new void Awake()
    {
        base.Awake();
        if(tarImpact != null)
        {
            prevImpactPos = tarImpact.transform.parent;
            tarImpact.SetActive(false);
        }
    }

    public void Update()
    {
        isEnd = GetComponent<ParticleSystem>().isStopped;
        if (isEnd)
        {
            transform.position = prevPos;
            if(tarImpact != null)
                tarImpact.transform.position = parent.transform.position;
        }
    }

    public override void Cast()
    {
        if(type == TARGET_TYPE.SINGLE)
        {
            if (tarImpact != null)
            {
                BattleManager.instance.TargetCharacter.GetComponent<Character>().impact = tarImpact;
                tarImpact.transform.position = BattleManager.instance.TargetCharacter.transform.position;
            }
            else
                transform.position = BattleManager.instance.TargetCharacter.transform.position;
        }
        if(type == TARGET_TYPE.FULL)
        {
            //USER.DECk[1]여기에 파티클 포지션 변경
        }
    }
}
