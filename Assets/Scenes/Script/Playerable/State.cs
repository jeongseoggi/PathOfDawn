using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public enum STATE //���� Ÿ��
{
    IDLE,
    BATTLE,
    HIT,
    MOVE
}

public enum ATTACK_TYPE //���� Ÿ��
{
    NORMAL,
    SKILLATK,
    ULTIMATEATK
}


//���� �ӽ� �������̽�
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

//�÷��̾� ���� �߻� Ŭ����
public abstract class PlayerState : State
{
    public Playerable player;
}

//���� ���� �߻� Ŭ����
public abstract class MonsterState : State
{
    public Monster monster;
}


//�÷��̾� ��� ����
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

//�÷��̾� ���� ����
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

//�÷��̾� �ǰ� ����
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

//�÷��̾� �����̴� ����
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


//�÷��̾�� ĳ������ ����Ÿ��(�⺻)
public class PlayerAttack : PlayerBattleState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
    }
    public override void Enter()
    {
        //�÷��̾�� ĳ������ �⺻ ���� ����Ʈ���� ���� ������Ʈ�� ���� �����ִ��� Ȯ��
        if (!player.skill[0].gameObject.activeSelf)
        {
            //�ش� �÷��̾�� ĳ���Ͱ� ������ �ִ� ��ų�� �ش��ϴ� Ÿ�Կ� ���� ����
            player.skill[0].GetComponent<Skill>().Cast();
            player.skill[0].gameObject.SetActive(true); //�ش� ���� ������Ʈ�� Ȱ��ȭ
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

//�÷���� ĳ������ ���� Ÿ��(��ų)
public class PlayerSkillAttack : PlayerBattleState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
    }
    public override void Enter()
    {
        //����Ʈ ���� üũ
        if (!player.skill[1].gameObject.activeSelf)
        {
            //��ų Ÿ�Կ� ���� ���� �Լ� ����
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

//�÷����� ĳ������ ���� Ÿ��(�ñر�)
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

public class StateMachine<T> : IStateMachine where T : Character //T�� �������� Character�� ������ T���� Character�� ���� �� ����
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
        curState?.Exit(); //���� ���°� null�� �ƴϸ� Exit()
        curState = stateDic[state];
        curState.Enter(); //���� ����

        //�ִϸ��̼� ��� int������ �޾Ƽ� �����ϱ� ���ϰ� �ϱ� ���� ���� ���� Ÿ��(enum)�� int������ ĳ����
        stateEnumint = (int)stateDic.FirstOrDefault(stateenum => stateenum.Value == curState).Key;
        owner.GetAnimator().SetInteger("State", stateEnumint); //ĳ������ �ִϸ��̼� ����
    }

    public void AddState(STATE state_type, State state)
    {
        if (stateDic.ContainsKey(state_type)) //Ű �ߺ� ����
            return;
        state.Init(this);
        stateDic.Add(state_type, state);
    }

    public void Update()
    {
        curState.Update();
    }
}
