using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUISlot : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;

    public TextMeshProUGUI levelText;

    public Image characterIcon;

    public void Init(Monster mon)
    {
        mon.myUi = this;
        hpBar.fillAmount = mon.Hp / mon.MaxHp;
        mpBar.fillAmount = mon.Mp / mon.MaxMp;

        levelText.text = mon.Level.ToString();

        characterIcon.sprite = mon.character_image;
    }


    public void SetHpBar(Monster mon)
    {
        hpBar.fillAmount = mon.Hp / mon.MaxHp;
    }
}
