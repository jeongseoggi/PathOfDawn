using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//�÷��̾�� ĳ������ ���� Ÿ��
public enum PLAYER_TYPE
{
    WARRIOR,
    ARCHER,
    WIZARD,
    HEALER,
    BUFFER
}

//�ڷ�ƾ���� ĳ���Ͱ� �ִϸ��̼��� ���� �� ���� ����ϴ� CustomYieldInstruction
public class WaitForAnimationClip : CustomYieldInstruction
{
    public Animator anim;
    public WaitForAnimationClip(Animator anim)
    {
        this.anim = anim;
    }
    public override bool keepWaiting
    {
        get
        {
            return anim.GetCurrentAnimatorClipInfo(0).Length < 0.9f;
        }
    }

}

public class Playerable : Character, IPunObservable
{
    //Hp ������Ƽ
    public override float Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if(isDie == false)
                myUi?.SetHpBar(this);

            if (hp <= 0 && isDie == false)
            {
                OnDie();
            }
            if (hp > MaxHp) hp = MaxHp;
        }
    }

    public override float Atk
    {
        get => atk; 
        set
        {
            atk = value;
            foreach (GameObject mySkill in skill)
                mySkill.GetComponent<Skill>().skillDamage = mySkill.GetComponent<Skill>().skillPercent * atk;
        }
    }

    public PlayerableUISlot myUi;
    public Sprite[] skillSprites;

    public StateMachine<Playerable> sm;
    public PLAYER_TYPE player_type;
    public State PlayerState
    {
        get { return sm.CurState; }
        set
        {
            if(sm.CurState is PlayerBattleState)
            {
                coroutine = StartCoroutine(WaitCo());
            }
        }
    }
    public PlayerBattleState battle = null;
    public GameObject target;

    //���� Ÿ�Կ� ���� ���� ���̸� ���� ������Ƽ ����
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
    private ATTACK_TYPE atk_type;
    private Coroutine coroutine;

    //���� Ÿ�Կ� ���� Battle���¸� �����ϱ� ���� ��ųʸ� ���
    public Dictionary<ATTACK_TYPE, PlayerBattleState> atkDic = new Dictionary<ATTACK_TYPE, PlayerBattleState>();
    //���� ���� Ÿ�� ����
    PlayerBattleState curAtk;
    private void Awake()
    {
        Init();

        ani = GetComponent<Animator>();
        sm = new StateMachine<Playerable>(this);
        sm.SetAnimator(ani);

        sm.AddState(STATE.IDLE, new PlayerIdleState());
        sm.AddState(STATE.BATTLE, new PlayerBattleState());
        sm.AddState(STATE.HIT, new PlayerHitState());
        sm.AddState(STATE.MOVE, new PlayerMoveState());

        sm.SetState(STATE.IDLE);

        //��ųʸ� ����
        AddAttackStrategy(ATTACK_TYPE.NORMAL, new PlayerAttack());
        AddAttackStrategy(ATTACK_TYPE.SKILLATK, new PlayerSkillAttack());
        AddAttackStrategy(ATTACK_TYPE.ULTIMATEATK, new PlayerUltimateAttack());

        OnDie += () => { isDie = true; };
        OnDie += () => { ReturnPool(); };
    }

    void AddAttackStrategy(ATTACK_TYPE aty, PlayerBattleState pbs)
    {
        curAtk = pbs;
        curAtk.Init(sm);
        atkDic.Add(aty, pbs);
    }

    public void Init()
    {
        switch (player_type)
        {
            case PLAYER_TYPE.WARRIOR:
                Job = "����";
                MaxHp = 100;
                Hp = 100;
                Atk = 5;
                Def = 10;
                Dodge = 1;
                MaxMp = 50;
                Mp = 50;
                Speed = 5;
                Aggro = 10;
                Level = 1;
                break;
            case PLAYER_TYPE.ARCHER:
                Job = "�ü�";
                MaxHp = 80;
                Hp = 80;
                Atk = 7;
                Def = 5;
                Dodge = 3;
                MaxMp = 70;
                Mp = 70;
                Speed = 7;
                Aggro = 5;
                Level = 1;
                break;
            case PLAYER_TYPE.WIZARD:
                Job = "������";
                MaxHp = 70;
                Hp = 70;
                Atk = 10;
                Def = 3;
                Dodge = 5;
                MaxMp = 100;
                Mp = 100;
                Speed = 10;
                Aggro = 2;
                Level = 1;
                break;
            case PLAYER_TYPE.BUFFER:
                Job = "����";
                MaxHp = 85;
                Hp = 85;
                Atk = 3;
                Def = 5;
                Dodge = 7;
                MaxMp = 100;
                Mp = 100;
                Speed = 8;
                Aggro = 1;
                Level = 1;
                break;
            case PLAYER_TYPE.HEALER:
                Job = "����";
                MaxHp = 90;
                Hp = 90;
                Atk = 3;
                Def = 7;
                Dodge = 8;
                MaxMp = 100;
                Mp = 100;
                Speed = 4;
                Aggro = 2;
                Level = 1;
                break;
        }
    }

    private void Update()
    {
        sm.Update();
    }
    
    //�÷��̾�� ĳ������ �ִϸ��̼��� ���� �� ���� ����ϴ� �ڷ�ƾ
    IEnumerator WaitCo()
    {
        yield return new WaitForAnimationClip(ani);
        sm.SetState(STATE.IDLE);
        coroutine = null;
    }
    public void LevelUp()
    {
        Level++;
        Hp += 15;
        Atk += 3;
        Def += 1;
        Speed += 1;
    }

    //�÷��̾�� ĳ���Ͱ� �ı����� �ʰ� ������Ʈ�� ���� ���¿��� User�� ���� �ڽ����� ���� �ϱ� ���� �Լ�
    public void ReturnPool()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(User.instance.transform);
        Hp = MaxHp;
    }

    public override void TakeDamage(float damage)
    {
        float resultDamage = damage - Def;
        if (resultDamage <= 0)
            resultDamage = 0;

        sm.SetState(STATE.HIT);
        StartCoroutine(WaitCo());
        Hp -= resultDamage;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(Hp);
        if (stream.IsReading)
            Hp = (float)stream.ReceiveNext();
    }
}