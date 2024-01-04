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
            jobText.text = "직업 : ";
            levelText.text = "레벨 : ";
            hpText.text = "체력 : ";
            mpText.text = "마나 : ";
            atkText.text = "공격력 : ";
            defText.text = "방어력 : ";
            dodgeText.text = "회피 : ";
            speedText.text = "속도 : ";
            aggroText.text = "어그로 : ";
            return;
        }

        jobText.text = "직업 : " + p.Job;
        levelText.text = "레벨 : " + p.Level;
        hpText.text = "체력 : " + p.Hp;
        mpText.text = "마나 : " + p.Mp;
        atkText.text = "공격력 : " + p.Atk;
        defText.text = "방어력 : " + p.Def;
        dodgeText.text = "회피 : " + p.Dodge;
        speedText.text = "속도 : " + p.Speed;
        aggroText.text = "어그로 : " + p.Aggro;
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
