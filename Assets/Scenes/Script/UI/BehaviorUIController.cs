using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BehaviorUIController : MonoBehaviour
{
    public enum Btn_Type
    {
        Attack,
        Skill,
        Ultimate
    }

    public Button atkBtn;
    public Button skillBtn;
    public Button ultimateBtn;

    public Dictionary<Btn_Type, ATTACK_TYPE> btnDic = new Dictionary<Btn_Type, ATTACK_TYPE> ();

    public Playerable curTurnPlayerableCharacter;

    Btn_Type curPressBtn;

    float damage;

    private void Start()
    {
        UIManager.instance.behaviorUIController = this;

        gameObject.SetActive(false);

        atkBtn.onClick.AddListener(() => { curPressBtn = Btn_Type.Attack; });
        atkBtn.onClick.AddListener(() =>
        {
            if (BattleManager.instance.CurTurnCharacter != null)
                damage = BattleManager.instance.CurTurnCharacter.skill[0].GetComponent<Skill>().skillDamage;
        });
        Init(atkBtn);

        skillBtn.onClick.AddListener(() => { curPressBtn = Btn_Type.Skill; });
        skillBtn.onClick.AddListener(() =>
        {
            if (BattleManager.instance.CurTurnCharacter != null)
                damage = BattleManager.instance.CurTurnCharacter.skill[1].GetComponent<Skill>().skillDamage;
        });
        Init(skillBtn);

        ultimateBtn.onClick.AddListener(() => { curPressBtn = Btn_Type.Ultimate; });
        ultimateBtn.onClick.AddListener(() =>
        {
            if (BattleManager.instance.CurTurnCharacter != null)
                damage = BattleManager.instance.CurTurnCharacter.skill[2].GetComponent<Skill>().skillDamage;
        });
        Init(ultimateBtn);

        btnDic.Add(Btn_Type.Attack, ATTACK_TYPE.NORMAL);
        btnDic.Add(Btn_Type.Skill, ATTACK_TYPE.SKILLATK);
        btnDic.Add(Btn_Type.Ultimate, ATTACK_TYPE.ULTIMATEATK);
    }

    private void OnEnable()
    {
        if (BattleManager.instance.CurTurnCharacter != null)
        {
            curTurnPlayerableCharacter = BattleManager.instance.CurTurnCharacter.GetComponent<Playerable>();
            atkBtn.GetComponent<Image>().sprite = curTurnPlayerableCharacter.skillSprites[0];
            skillBtn.GetComponent<Image>().sprite = curTurnPlayerableCharacter.skillSprites[1];
            ultimateBtn.GetComponent<Image>().sprite = curTurnPlayerableCharacter.skillSprites[2];
        }
    }

    void Init(Button button)
    {
        button.onClick.AddListener(() => { SetBehavior((Playerable)BattleManager.instance.CurTurnCharacter); });
    }

    public void SetBehavior(Playerable playerable)
    {
        playerable.Atk_type = btnDic[curPressBtn];
        playerable.sm.SetState(STATE.BATTLE);
        BattleManager.instance.BattleDamage(damage, btnDic[curPressBtn]);
        gameObject.SetActive(false);

        BattleManager.instance.NextOrder();
    }
}
