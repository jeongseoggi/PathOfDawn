using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� 9 ��
// { �⺻ ���ݷ� ����, ��ų ���ݷ� ����, ���� ����, �켱���� ����, ���� �� 1�� ����,
//   ���� ġ��, ���� ���, �ִ� Hp ����, ��� ȹ�淮 ���� (���� ����) }


public class BuffController : MonoBehaviour
{
    public Animator animator;
    public Action buffSelectIn;
    public Action buffSelectOut;
    public BuffInfo buff;
    public GameObject buffFrameObj;

    void Start()
    {
        UIManager.instance.buffController = this;
        buffSelectIn += () => { animator.SetBool("FinishBattle", true); };
        buffSelectIn += () => { buff.gameObject.SetActive(true); };
        buffSelectIn += () => { buffFrameObj.SetActive(true); };

        buffSelectOut += () => { animator.SetBool("FinishBattle", false); };
        buffSelectOut += () => { buff.gameObject.SetActive(false); };
        buffSelectOut += () => { BattleManager.instance.eM.battleEnd(); };

        buff.gameObject.SetActive(false);
    }
}
