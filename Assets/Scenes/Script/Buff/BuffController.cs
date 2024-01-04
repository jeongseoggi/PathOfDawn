using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 종류 9 개
// { 기본 공격력 증가, 스킬 공격력 증가, 방어력 증가, 우선순위 증가, 랜덤 적 1명 스턴,
//   지속 치유, 마나 재생, 최대 Hp 증가, 골드 획득량 증가 (방어력 감소) }


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
