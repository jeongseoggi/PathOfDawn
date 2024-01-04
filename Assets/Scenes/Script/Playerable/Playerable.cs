using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//플레이어블 캐릭터의 직업 타입
public enum PLAYER_TYPE
{
    WARRIOR,
    ARCHER,
    WIZARD,
    HEALER,
    BUFFER
}

//코루틴에서 캐릭터가 애니메이션이 끝날 때 까지 대기하는 CustomYieldInstruction
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
    //Hp 프로퍼티
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

    //공격 타입에 따른 상태 전이를 위해 프로퍼티 설정
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

    //공격 타입에 따른 Battle상태를 저장하기 위해 딕셔너리 사용
    public Dictionary<ATTACK_TYPE, PlayerBattleState> atkDic = new Dictionary<ATTACK_TYPE, PlayerBattleState>();
    //현재 공격 타입 상태
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

        //딕셔너리 저장
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
                Job = "전사";
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
                Job = "궁수";
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
                Job = "마법사";
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
                Job = "버퍼";
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
                Job = "힐러";
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
    
    //플레이어블 캐릭터의 애니메이션이 끝날 때 까지 대기하는 코루틴
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

    //플레이어블 캐릭터가 파괴되지 않고 오브젝트가 꺼진 상태에서 User의 하위 자식으로 들어가게 하기 위한 함수
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