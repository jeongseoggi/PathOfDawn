using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviourPun, IComparable<Character>
{
    [SerializeField] protected string job;       //�̸�
    [SerializeField] protected float hp;          //ü��
    [SerializeField] protected float maxHp;       //�ִ�ü��
    [SerializeField] protected float atk;         //���ݷ�
    [SerializeField] protected float def;         //����
    [SerializeField] protected int dodge;         //ȸ�Ƿ�
    [SerializeField] protected float mp;          //����
    [SerializeField] protected float maxMp;       //�ִ븶��
    [SerializeField] protected int speed;         //�ӵ�
    [SerializeField] protected int aggro;         //��׷� ��ġ
    [SerializeField] protected int turnCount = 1; //�� ī��Ʈ
    [SerializeField] protected int level;
    public bool isDie = false;

    public GameObject impact;
    public Sprite character_image; //ĳ���͵��� �̹���
    public GameObject[] skill;
    public Animator ani;
    public Action OnDie;


    public string Job { get { return job; } set { job = value; } }
    public virtual float Hp { get; set; }   //hp ������Ƽ
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }                         //maxHp ������Ƽ
    public virtual float Atk { get { return atk; } set { atk = value; } }                                //���ݷ� ������Ƽ
    public float Def { get { return def; } set { def = value; } }                               //���� ������Ƽ
    public int Dodge { get { return dodge; } set { dodge = value; } }                          //ȸ�Ƿ� ������Ƽ
    public float Mp { get { return mp; } set { mp = value; if (mp > maxMp) { mp = maxMp; } } }   //���� ������Ƽ
    public float MaxMp { get { return maxMp; } set { maxMp = value; } }                         //maxMp ������Ƽ
    public int Speed { get { return speed; } set { speed = value; } }                           //�ӵ� ������Ƽ
    public int Aggro { get { return aggro; } set { aggro = value; } }                            //��� ������Ƽ
    public int Level { get { return level; } set { level = value; } }
    public int CompareTo(Character other) //���ǵ忡 ���� ����(���ǵ尡 ���� ��)
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