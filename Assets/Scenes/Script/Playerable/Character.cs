using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviourPun, IComparable<Character>
{
    [SerializeField][UIView("����")] protected string job;       //�̸�
    [SerializeField][UIView("ü��")] protected float hp;          //ü��
    [SerializeField] protected float maxHp;       //�ִ�ü��
    [SerializeField][UIView("���ݷ�")] protected float atk;         //���ݷ�
    [SerializeField][UIView("����")] protected float def;         //����
    [SerializeField][UIView("ȸ�Ƿ�")] protected int dodge;         //ȸ�Ƿ�
    [SerializeField][UIView("����")] protected float mp;          //����
    [SerializeField] protected float maxMp;       //�ִ븶��
    [SerializeField][UIView("�ӵ�")] protected int speed;         //�ӵ�
    [SerializeField][UIView("��׷�")] protected int aggro;         //��׷� ��ġ
    [SerializeField] protected int turnCount = 1; //�� ī��Ʈ
    [SerializeField][UIView("����")] protected int level;
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
        Job= original.job;
        Hp = original.hp;
        MaxHp = original.maxHp;
        Atk= original.atk;
        Def = original.def;
        Dodge = original.dodge;
        Mp =  original.mp;
        maxMp = original.maxMp;
        Speed = original.speed;
        Aggro = original.aggro;
        Level = original.level;
    }
}