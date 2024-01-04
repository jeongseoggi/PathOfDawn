using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Monster : Character
{
    public State Monster_State => sm.CurState;
 
    Coroutine coroutine;
    MonsterBattleState curAtk;

    public MonsterUISlot myUi;

    public Dictionary<ATTACK_TYPE, MonsterBattleState> atkDic = new Dictionary<ATTACK_TYPE, MonsterBattleState>();
    public override float Hp
    {
        get => hp;

        set
        {
            hp = value;
            myUi?.SetHpBar(this);
            if (hp > maxHp)
            {
                hp = maxHp;
            }

            if (hp <= 0 && isDie == false)
            {
                OnDie();
            }
        }

    }

    public float monRecMp;
    public float MonRecMp
    {
        get { return monRecMp; }
        set { monRecMp = value; }
    }

    Rigidbody rb;
    public StateMachine<Monster> sm;

    public ATTACK_TYPE atk_type;

    public ATTACK_TYPE Atk_type
    {
        get => atk_type;
        set
        {
            atk_type = value;
            curAtk = atkDic[atk_type];
            sm.stateDic[STATE.BATTLE] = curAtk;
        }
    }
    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        sm = new StateMachine<Monster>(this);
        sm.SetAnimator(ani);
        sm.AddState(STATE.IDLE, new MonsterIdleState());
        sm.AddState(STATE.BATTLE, new MonsterBattleState());
        sm.AddState(STATE.HIT, new MonsterHitState());

        AddAttackStrategy(ATTACK_TYPE.NORMAL, new MonsterAttack());
        AddAttackStrategy(ATTACK_TYPE.SKILLATK, new MonsterSkillAttack());
        AddAttackStrategy(ATTACK_TYPE.ULTIMATEATK, new MonsterUltimateAtk());


        sm.SetState(STATE.IDLE);
        Invoke("Spawn", 1);

        //skill[0]?.SetActive(false);
        //skill[1]?.SetActive(false);
        //skill[2]?.SetActive(false);

        OnDie += () => { Invoke("Die", 2.5f); };
        OnDie += () => { isDie = true; };
        MonAddValue(level);
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    void AddAttackStrategy(ATTACK_TYPE aty, MonsterBattleState pbs)
    {
        curAtk = pbs;
        curAtk.Init(sm);
        atkDic.Add(aty, pbs);
    }

    protected void Update()
    {
        if (Monster_State is MonsterBattleState)
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(WaitCo());
            }
        }
        sm.Update();
    }

    public Character SetTarget(List<Playerable> targetList)
    {
        int totalAggro = 0;
        int curAggro = 0;

        foreach (Playerable target in targetList)
            totalAggro += target.Aggro;

        int randomValue = UnityEngine.Random.Range(0, totalAggro);

        foreach (Playerable target in targetList)
        {
            curAggro += target.Aggro;
            if(randomValue <= curAggro)
                return target;
        }
        return null;
    }

    public virtual IEnumerator WaitCo()
    {
        //yield return new WaitForSeconds(0.75f);
        yield return new WaitForAnimationClip(ani);
        sm.SetState(STATE.IDLE);
        coroutine = null;
    }

    public virtual void MonAddValue(int lv)
    {

    }

    public virtual void Spawn()
    {
        ani.SetTrigger("Spawn") ;
    }

    //public virtual void GetHit()
    //{

    //}
    //public virtual void Normal()
    //{
    //    ani.SetBool("Normal", true);

    //}
    //public virtual void SkillAtk()
    //{
    //    ani.SetBool("SkillAtk", true);
    //}
    //public virtual void UltimateAtk()
    //{
    //    ani.SetBool("UltimateAtk", true);
    //}

    public virtual void SelectSkill()
    {
        int enumLenght = (int)ATTACK_TYPE.ULTIMATEATK + 1;
        int random = UnityEngine.Random.Range(0, enumLenght);

        switch (random)
        {
            case 0:
                Atk_type = ATTACK_TYPE.NORMAL;
                break;
            case 1:
                Atk_type = ATTACK_TYPE.SKILLATK;
                break;
            case 2:
                Atk_type = ATTACK_TYPE.ULTIMATEATK;
                break;
        }
    }

    public override void TakeDamage(float damage)
    {
        float resultDamage = damage - Def;
        if (resultDamage <= 0)
            resultDamage = 0;

        sm.SetState(STATE.HIT);
        StartCoroutine(WaitCo());
        Hp -= damage;
    }
}