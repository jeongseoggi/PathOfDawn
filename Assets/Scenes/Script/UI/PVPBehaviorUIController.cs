using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPBehaviorUIController : MonoBehaviourPunCallbacks
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
        UIManager.instance.pVpBehaviorUIController = this;

        gameObject.SetActive(false);

        atkBtn.onClick.AddListener(() => { curPressBtn = Btn_Type.Attack; });
        atkBtn.onClick.AddListener(() =>
        {
            if (PhotonView.Find(PVPBattleManager.instance.curTurnCharacterID).GetComponent<Playerable>() != null)
                damage = PhotonView.Find(PVPBattleManager.instance.curTurnCharacterID).GetComponent<Playerable>().skill[0].GetComponent<Skill>().skillDamage;
        });
        Init(atkBtn);

        skillBtn.onClick.AddListener(() => { curPressBtn = Btn_Type.Skill; });
        skillBtn.onClick.AddListener(() =>
        {
            if (PhotonView.Find(PVPBattleManager.instance.curTurnCharacterID).GetComponent<Playerable>() != null)
                damage = PhotonView.Find(PVPBattleManager.instance.curTurnCharacterID).GetComponent<Playerable>().skill[1].GetComponent<Skill>().skillDamage;
        });
        Init(skillBtn);

        ultimateBtn.onClick.AddListener(() => { curPressBtn = Btn_Type.Ultimate; });
        ultimateBtn.onClick.AddListener(() =>
        {
            if (PhotonView.Find(PVPBattleManager.instance.curTurnCharacterID).GetComponent<Playerable>() != null)
                damage = PhotonView.Find(PVPBattleManager.instance.curTurnCharacterID).GetComponent<Playerable>().skill[2].GetComponent<Skill>().skillDamage;
        });
        Init(ultimateBtn);

        btnDic.Add(Btn_Type.Attack, ATTACK_TYPE.NORMAL);
        btnDic.Add(Btn_Type.Skill, ATTACK_TYPE.SKILLATK);
        btnDic.Add(Btn_Type.Ultimate, ATTACK_TYPE.ULTIMATEATK);
    }

    public override void OnEnable()
    {
        if(PVPBattleManager.instance.curTurnCharacterID != 0)
        {
            if (PhotonView.Find(PVPBattleManager.instance.curTurnCharacterID).GetComponent<Playerable>() != null)
            {
                curTurnPlayerableCharacter = PhotonView.Find(PVPBattleManager.instance.curTurnCharacterID).GetComponent<Playerable>();
                atkBtn.GetComponent<Image>().sprite = curTurnPlayerableCharacter.skillSprites[0];
                skillBtn.GetComponent<Image>().sprite = curTurnPlayerableCharacter.skillSprites[1];
                ultimateBtn.GetComponent<Image>().sprite = curTurnPlayerableCharacter.skillSprites[2];
            }
        }
    }

    void Init(Button button)
    {
        button.onClick.AddListener(() => { SetBehavior(PhotonView.Find(PVPBattleManager.instance.curTurnCharacterID).GetComponent<Playerable>()); });
    }

    public void SetBehavior(Playerable playerable)
    {
        playerable.Atk_type = btnDic[curPressBtn];
        playerable.sm.SetState(STATE.BATTLE);
        PVPBattleManager.instance.BattleDamage(damage, btnDic[curPressBtn]);
        gameObject.SetActive(false);

        PVPBattleManager.instance.NextOrder();
    }
}
