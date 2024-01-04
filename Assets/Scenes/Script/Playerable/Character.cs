using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviourPun, IComparable<Character>
{
    [SerializeField] protected string job;       //이름
    [SerializeField] protected float hp;          //체력
    [SerializeField] protected float maxHp;       //최대체력
    [SerializeField] protected float atk;         //공격력
    [SerializeField] protected float def;         //방어력
    [SerializeField] protected int dodge;         //회피력
    [SerializeField] protected float mp;          //마나
    [SerializeField] protected float maxMp;       //최대마나
    [SerializeField] protected int speed;         //속도
    [SerializeField] protected int aggro;         //어그로 수치
    [SerializeField] protected int turnCount = 1; //턴 카운트
    [SerializeField] protected int level;
    public bool isDie = false;

    public GameObject impact;
    public Sprite character_image; //캐릭터들의 이미지
    public GameObject[] skill;
    public Animator ani;
    public Action OnDie;


    public string Job { get { return job; } set { job = value; } }
    public virtual float Hp { get; set; }   //hp 프로퍼티
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }                         //maxHp 프로퍼티
    public virtual float Atk { get { return atk; } set { atk = value; } }                                //공격력 프로퍼티
    public float Def { get { return def; } set { def = value; } }                               //방어력 프로퍼티
    public int Dodge { get { return dodge; } set { dodge = value; } }                          //회피력 프로퍼티
    public float Mp { get { return mp; } set { mp = value; if (mp > maxMp) { mp = maxMp; } } }   //마나 프로퍼티
    public float MaxMp { get { return maxMp; } set { maxMp = value; } }                         //maxMp 프로퍼티
    public int Speed { get { return speed; } set { speed = value; } }                           //속도 프로퍼티
    public int Aggro { get { return aggro; } set { aggro = value; } }                            //어그 프로퍼티
    public int Level { get { return level; } set { level = value; } }
    public int CompareTo(Character other) //스피드에 따른 정렬(스피드가 높은 순)
    {
        if (speed < other.speed)
            return 1;
        else if (speed > other.speed)
            return -1;
        else
            return 0;
        //return speed.CompareTo(other.speed);
    }

    public virtual Animator GetAnimator()
    {
        return ani;
    }

    public virtual void TakeDamage(float damage)
    {

    }

    public void DeepCopy(Character original)
    {
        job = original.job;
        hp = original.hp;
        maxHp = original.maxHp;
        atk = original.atk;
        def = original.def;
        dodge = original.dodge;
        mp = original.mp;
        maxMp = original.maxMp;
        speed = original.speed;
        aggro = original.aggro;
        level = original.level;
    }
}