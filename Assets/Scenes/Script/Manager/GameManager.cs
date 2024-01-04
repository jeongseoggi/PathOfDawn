using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public List<Playerable> initPlayerableList;
    public List<Playerable> curPlayerableList;

    public BattleManager battleManager;
    User user;

    public Action returnLoabby;

    private void Start()
    {
        user = User.instance;
    }

    public void GameStart()
    {
        InitPlayerable();
        UIManager.instance.shopController.SetBtnImage();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void InitPlayerable()
    {
        foreach (Playerable p in initPlayerableList)
        {
            if (p != null)
                user.InitCharacter(p);

            UIManager.instance.SetBtn();
        }
    }
}
