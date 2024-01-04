using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerableUISlot : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI aggroText;

    public Image characterIcon;

    public void Init(Playerable playerable)
    {
        playerable.myUi = this;

        hpBar.fillAmount = playerable.Hp / playerable.MaxHp;
        mpBar.fillAmount = playerable.Mp / playerable.MaxMp;

        levelText.text = playerable.Level.ToString();
        atkText.text = playerable.Atk.ToString();
        defText.text = playerable.Def.ToString();
        dodgeText.text = playerable.Dodge.ToString();
        speedText.text = playerable.Speed.ToString();
        aggroText.text = playerable.Aggro.ToString();

        characterIcon.sprite = playerable.character_image;
    }

    public void SetHpBar(Playerable playerable)
    {
        hpBar.fillAmount = playerable.Hp / playerable.MaxHp;
    }
}
