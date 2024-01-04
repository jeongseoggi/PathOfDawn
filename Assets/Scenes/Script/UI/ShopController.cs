using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public List<Playerable> playerables;
    public Playerable curSelectCharacter;

    BtnPressGrow btnPressGrow;

    public BtnImageViewController deckImage;
    public BtnImageViewController infoImage;

    public GameObject moneyEffect;

    public void Start()
    {
        UIManager.instance.shopController = this;
        btnPressGrow = GetComponent<BtnPressGrow>();
    }

    public void SetBtnImage()
    {
        deckImage.SetImage();
        infoImage.SetImage();
    }

    public void CharacterSelect(int index)
    {
        if(curSelectCharacter != null
          && curSelectCharacter.player_type == playerables[index].player_type)
            curSelectCharacter = null;
        else
            curSelectCharacter = playerables[index];

        btnPressGrow.SetBtnGrow(index);
    }

    public void Buy()
    {
        if (curSelectCharacter == null)
            return;

        moneyEffect.SetActive(true);
        User.instance.AddCharacter(curSelectCharacter);
        UIManager.instance.SetBtn();

        SetBtnImage();
    }
}
