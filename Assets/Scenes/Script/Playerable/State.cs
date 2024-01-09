using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public enum STATE //상태 타입
{
    IDLE,
    BATTLE,
    HIT,
    MOVE
}

public enum ATTACK_TYPE //공격 타입
{
    NORMAL,
    SKILLATK,
    ULTIMATEATK
}


//상태 머신 인터페이스
public interface IStateMachine
{
    State CurState { get; }
    Animator Animator { get; }
    object GetOwner();
    void SetState(STATE state);
}

public abstract class State
{
    public IStateMachine sm;
    public virtual void Init(IStateMachine sm)
    {
        this.sm = sm;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

//플레이어 상태 추상 클래스
public abstract class PlayerState : State
{
    public Playerable player;
}

//몬스터 상태 추상 클래스
public abstract class MonsterState : State
{
    public Monster monster;
}


//플레이어 대기 상태
public class PlayerIdleState : PlayerState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        player = (Playerable)this.sm.GetOwner();
    }
    public override void Enter() { }
    public override void Exit() { }
    public override void Update() { }
  
}

//플레이어 전투 상태
public class PlayerBattleState : PlayerState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        player = (Playerable)this.sm.GetOwner();
    }
    public override void Enter() { }
    public override void Exit() { }
    public override void Update() { }
}

//플레이어 피격 상태
public class PlayerHitState : PlayerState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        player = (Playerable)this.sm.GetOwner();
    }
    public override void Enter() { }
    public override void Update()
    {
        sm.Animator.SetBool("Hit", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("Hit", false);
    }
}

//플레이어 움직이는 상태
public class PlayerMoveState : PlayerState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        player = (Playerable)this.sm.GetOwner();
    }
    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}


//플레이어블 캐릭터의 공격타입(기본)
public class PlayerAttack : PlayerBattleState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
    }
    public override void Enter()
    {
        //플레이어블 캐릭터의 기본 공격 이펙트가의 게임 오브젝트가 먼저 꺼져있는지 확인
        if (!player.skill[0].gameObject.activeSelf)
        {
            //해당 플레이어블 캐릭터가 가지고 있는 스킬의 해당하는 타입에 따라 공격
            player.skill[0].GetComponent<Skill>().Cast();
            player.skill[0].gameObject.SetActive(true); //해당 게임 오브젝트를 활성화
            if (player.skill[0].GetComponent<Skill>().battle_type == BATTLE_TYPE.PVE)
                BattleManager.instance.TargetCharacter.GetComponent<Character>().Hp -= player.skill[1].GetComponent<Skill>().skillDamage;
            else
                PVPBattleManager.instance.TargetCharacter.GetComponent<Character>().Hp -= player.skill[1].GetComponent<Skill>().skillDamage;
        }
    }

    public override void Update()
    {
        sm.Animator.SetBool("Attack", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("Attack", false);

    }
}

//플레어블 캐릭터의 공격 타입(스킬)
public class PlayerSkillAttack : PlayerBattleState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
    }
    public override void Enter()
    {
        //이펙트 상태 체크
        if (!player.skill[1].gameObject.activeSelf)
        {
            //스킬 타입에 따라 공격 함수 실행
            player.skill[1].GetComponent<Skill>().Cast();
            player.skill[1].gameObject.SetActive(true);
            if(player.skill[1].GetComponent<Skill>().battle_type == BATTLE_TYPE.PVE)
                BattleManager.instance.TargetCharacter.GetComponent<Character>().Hp -= player.skill[1].GetComponent<Skill>().skillDamage;
            else
                PVPBattleManager.instance.TargetCharacter.GetComponent<Character>().Hp -= player.skill[1].GetComponent<Skill>().skillDamage;
        }
    }

    public override void Update()
    {
        sm.Animator.SetBool("SkillAttack", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("SkillAttack", false);
    }
}

//플레어어블 캐릭터의 공격 타입(궁극기)
public class PlayerUltimateAttack : PlayerBattleState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
    }
    public override void Enter()
    {
        if (!player.skill[2].gameObject.activeSelf)
        {
            player.skill[2].GetComponent<Skill>().Cast();
            player.skill[2].gameObject.SetActive(true);
        }
    }

    public override void Update()
    {
        sm.Animator.SetBool("UltimateAttack", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("UltimateAttack", false);
    }
}

public class MonsterIdleState : MonsterState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = (Monster)this.sm.GetOwner();
    }
    public override void Enter()
    {

    }
    public override void Update()
    {

    }

    public override void Exit()
    {
    }

}
public class MonsterBattleState : MonsterState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = (Monster)this.sm.GetOwner();
    }
    public override void Enter()
    {

    }
    public override void Update()
    {

    }

    public override void Exit()
    {

    }

}
public class MonsterAttack : MonsterBattleState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = (Monster)this.sm.GetOwner();
    }
    public override void Enter()
    {
        if (!monster.skill[0].gameObject.activeSelf)
        {
            monster.skill[0].GetComponent<Skill>().Cast();
            monster.skill[0].gameObject.SetActive(true);
            BattleManager.instance.TargetCharacter.GetComponent<Character>().Hp -= monster.skill[0].GetComponent<Skill>().skillDamage;
        }
    }
    public override void Update()
    {
        sm.Animator.SetBool("Attack", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("Attack", false);
    }
}
public class MonsterSkillAttack : MonsterBattleState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = (Monster)this.sm.GetOwner();
    }
    public override void Enter()
    {
        if (!monster.skill[1].gameObject.activeSelf)
        {
            monster.skill[1].GetComponent<Skill>().Cast();
            monster.skill[1].gameObject.SetActive(true);
            if (monster.skill[1].GetComponent<Skill>().skillDamage != 0)
            {
                BattleManager.instance.TargetCharacter.GetComponent<Character>().Hp -= monster.skill[1].GetComponent<Skill>().skillDamage;
            }
        }
    }

    public override void Update()
    { 
        sm.Animator.SetBool("SkillAtk", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("SkillAtk", false);
    }
}
public class MonsterUltimateAtk : MonsterBattleState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = (Monster)this.sm.GetOwner();
    }
    public override void Enter()
    {

        if (!monster.skill[2].gameObject.activeSelf)
        {
            monster.skill[2].GetComponent<Skill>().Cast();
            monster.skill[2].gameObject.SetActive(true);
        }
    }

    public override void Update()
    {
        sm.Animator.SetBool("UltimateAtk", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("UltimateAtk", false);
    }
}
public class MonsterHitState : MonsterState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = (Monster)this.sm.GetOwner();
    }
    public override void Enter()
    {
    }
    public override void Update()
    {

    }

    public override void Exit()
    {

    }

}

public class StateMachine<T> : IStateMachine where T : Character //T의 스코프를 Character로 좁혀줌 T에는 Character만 들어올 수 있음
{
    public Animator animator = null;
    T owner;
    State curState;
    int stateEnumint;
    public State CurState
    {
        get { return curState; }
    }
    public Animator Animator
    {
        get { return animator; }
    }
    public Dictionary<STATE, State> stateDic = new Dictionary<STATE, State>();
    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    public void SetAnimator(in Animator animator)
    {
        this.animator = animator;
    }

    public object GetOwner()
    {
        return owner;
    }

    public void SetState(STATE state)
    {
        curState?.Exit(); //현재 상태가 null이 아니면 Exit()
        curState = stateDic[state];
        curState.Enter(); //상태 진입

        //애니메이션 제어를 int형으로 받아서 관리하기 편하게 하기 위해 현재 상태 타입(enum)을 int형으로 캐스팅
        stateEnumint = (int)stateDic.FirstOrDefault(stateenum => stateenum.Value == curState).Key;
        owner.GetAnimator().SetInteger("State", stateEnumint); //캐릭터의 애니메이션 변경
    }

    public void AddState(STATE state_type, State state)
    {
        if (stateDic.ContainsKey(state_type)) //키 중복 방지
            return;
        state.Init(this);
        stateDic.Add(state_type, state);
    }

    public void Update()
    {
        curState.Update();
    }
}
