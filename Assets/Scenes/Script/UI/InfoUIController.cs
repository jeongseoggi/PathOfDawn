using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoUIController : MonoBehaviour
{
    public GameObject infoStage;
    public Playerable[] showObject;

    public Playerable selectCharacter;

    public TextMeshProUGUI jobText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI aggroText;

    BtnPressGrow btnPressGrow;
    BtnImageViewController btnImageViewController;

    private void Start()
    {
        UIManager.instance.infoUIController = this;

        btnPressGrow = GetComponent<BtnPressGrow>();
        btnImageViewController = GetComponent<BtnImageViewController>();

        showObject = infoStage.GetComponentsInChildren<Playerable>();

        foreach (Playerable p in showObject)
            p.gameObject.SetActive(false);
    }

    public void SetInfoText(Playerable p)
    {
        if (p == null)
        {
            jobText.text = "���� : ";
            levelText.text = "���� : ";
            hpText.text = "ü�� : ";
            mpText.text = "���� : ";
            atkText.text = "���ݷ� : ";
            defText.text = "���� : ";
            dodgeText.text = "ȸ�� : ";
            speedText.text = "�ӵ� : ";
            aggroText.text = "��׷� : ";
            return;
        }

        jobText.text = "���� : " + p.Job;
        levelText.text = "���� : " + p.Level;
        hpText.text = "ü�� : " + p.Hp;
        mpText.text = "���� : " + p.Mp;
        atkText.text = "���ݷ� : " + p.Atk;
        defText.text = "���� : " + p.Def;
        dodgeText.text = "ȸ�� : " + p.Dodge;
        speedText.text = "�ӵ� : " + p.Speed;
        aggroText.text = "��׷� : " + p.Aggro;
    }

    public void SelectCharacterType(int num)
    {
        if(num >= User.instance.MyCharacters.Count)
            return;

        if(selectCharacter?.player_type == User.instance.MyCharacters[num].player_type)
            selectCharacter = null;
        else
            selectCharacter = User.instance.MyCharacters[num];

        SetInfoPosObject(selectCharacter);
        btnPressGrow.SetBtnGrow(num);
        SetInfoText(selectCharacter);
    }

    public void SetInfoPosObject(Playerable selectPlayerabl)
    {
        if(selectPlayerabl == null)
        {
            foreach (Playerable p in showObject)
            {
               p.gameObject.SetActive(false);
            }
            return;
        }

        foreach (Playerable p in showObject)
        {
            if(p.player_type == selectPlayerabl.player_type)
                p.gameObject.SetActive(true);
            else
                p.gameObject.SetActive(false);
        }
    }
}
