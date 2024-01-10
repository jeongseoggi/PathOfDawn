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

    BtnPressGrow btnPressGrow;
    BtnImageViewController btnImageViewController;

    public GameObject infoFrame;

    private void Start()
    {
        UIManager.instance.infoUIController = this;

        btnPressGrow = GetComponent<BtnPressGrow>();
        btnImageViewController = GetComponent<BtnImageViewController>();

        showObject = infoStage.GetComponentsInChildren<Playerable>();

        foreach (Playerable p in showObject)
            p.gameObject.SetActive(false);
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
        infoFrame.GetComponent<Info>().Init(selectCharacter);
        //SetInfoText(selectCharacter);
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
