using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;

//UIView ∏Æ∆—≈‰∏µ
[AttributeUsage(AttributeTargets.Field)]
public class UIView : Attribute
{
    string name;

    public string Name => name;

    public UIView(string fieldName)
    {
        name = fieldName;
    }
}

public class UIManager : Singleton<UIManager>
{
    // ∏ﬁ¿Œ æ¿ UI
    public GameObject mainSceneCanvas;
    public GameObject lobbyStage;
    public GameObject infoStage;

    public UICamController uICamController;
    public InfoUIController infoUIController;
    public DeckSelectController deckSelectController;
    public ShopController shopController;

    // πË∆≤ æ¿ UI
    public PlayerableUIController playerableUIController;
    public PVPOtherPlayerableUIController otherPlayerUIController;
    public PVPPlayerableUIController pVpPlayerableUIController;
    public PVPBehaviorUIController pVpBehaviorUIController;
    public MonsterUIController monsterUIController;
    public BehaviorUIController behaviorUIController;
    public TurnTableUIController turnTableUIController;
    public PvPTurnTableUIController pvpTurnTableUIController;
    public BuffController buffController;

    public event Action<int> setBtnDel;
    public BtnViewController[] btnViewControllers;

    private void Start()
    {
        btnViewControllers[0].Init();
        btnViewControllers[1].Init();
    }


    public void SetBtn()
    {
        if (setBtnDel != null)
            setBtnDel(User.instance.MyCharacters.Count);
    }

    public void MainSceneUI(bool isShow)
    {
        mainSceneCanvas.SetActive(isShow);
        lobbyStage.SetActive(isShow);
        infoStage.SetActive(isShow);
    }

    public void ExitBattleUI()
    {
        playerableUIController.gameObject.SetActive(false);
        otherPlayerUIController.gameObject.SetActive(false);
        monsterUIController.gameObject.SetActive(false);
        behaviorUIController.gameObject.SetActive(false);
        turnTableUIController.gameObject.SetActive(false);
        turnTableUIController.Clear();
    }
}
