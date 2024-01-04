using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Cinemachine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;
    private int monsterCount;
    public int MonsterCount
    {
        get => monsterCount;
        set
        {
            monsterCount = value;
            if (monsterCount <= 0)
                BattleEnd();
        }
    }
    int userCount;
    public int UserCount
    {
        get => userCount;
        set
        {
            userCount = value;
            if (userCount <= 0)
            {
                isLose = true;
            }
        }
    }

    public MonsterManager monsterManager;
    User user;

    public List<Playerable> tempPlayer = new List<Playerable>();

    public List<Character> BattleCharacterList => battleCharacterList;
    [SerializeField] List<Character> battleCharacterList = new List<Character>();

    public List<Character> CurMonsterList => curMonsterList;
    List<Character> curMonsterList = new List<Character>();

    public Character CurTurnCharacter
    {
        get => curTurnCharacter;
        set
        {
            curTurnCharacter = value;
            if(curTurnCharacter == null)
                return;

            if (curTurnCharacter.TryGetComponent<Playerable>(out var p))
                isPlayerTurn = true;
            else
            {
                targetSelectEffect.SetActive(false);
                isPlayerTurn = false;

                Debug.Log("MonsterTurn");
                Invoke("MonsterTurnStart", 3.0f);
            }
        }
    }
    [SerializeField] Character curTurnCharacter;
    public Character TargetCharacter
    {
        get => targetCharacter;

        set
        {
            targetCharacter = value;

            if(targetCharacter == null)
                return;

            if(targetCharacter.TryGetComponent<Playerable>(out var playerable))
            {
                UIManager.instance.behaviorUIController.gameObject.SetActive(false);
            }
            else
            {
                UIManager.instance.behaviorUIController.gameObject.SetActive(true);
                targetSelectEffect.transform.position = targetCharacter.transform.position;
                targetSelectEffect.SetActive(true);
            }
        }
}
    [SerializeField] Character targetCharacter;

    public EventManager eM;
    public Camera mainCam;

    bool isPlayerTurn;
    int index;

    public GameObject targetSelectEffect;

    public bool isLose;
    float loseTimer;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        index = 0;

        GameManager.instance.battleManager = this;
        user = User.instance;

        isLose = false;
        loseTimer = 0;


        foreach (Character playerable in user.Deck)
        {
            tempPlayer.Add(playerable.GetComponent<Playerable>());
            playerable.OnDie += () => { UserCount--; };
            playerable.OnDie += () => { user.Deck.Remove((Playerable)playerable); };
        }
    }

    public void BattleStart()
    {
        SetBattleCharacterList();
        UIManager.instance.playerableUIController.gameObject.SetActive(true);
        CurTurnCharacter = battleCharacterList[0];

        index = 0;
    }

    public void BattleEnd()
    {
        battleCharacterList.Clear();
        curMonsterList.Clear();
        CurTurnCharacter = null;
        TargetCharacter = null;
        monsterManager.round++;

        UIManager.instance.ExitBattleUI();
        targetSelectEffect.SetActive(false);

        eM.buffSelectStart();



        //eM.battleEnd();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
            isLose = true;

        if (isLose)
            loseTimer += Time.deltaTime;

        if (loseTimer >= 4.0f)
            BadEnd();

        if(Input.GetMouseButtonDown(0) && isPlayerTurn)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, float.MaxValue, 1<<6))
            {
                TargetCharacter = hit.collider.TryGetComponent<Monster>(out var monster) ? monster : null;
            }
        }
    }

    private void MonsterTurnStart()
    {
        if (curTurnCharacter.TryGetComponent<Monster>(out var monster))
        {
            TargetCharacter = monster.SetTarget(user.Deck);
            monster.SelectSkill();
            monster.sm.SetState(STATE.BATTLE);
            BattleDamage(monster.Atk, monster.Atk_type);
            NextOrder();
        }
    }

    public void BattleDamage(float damage, ATTACK_TYPE atkType)
    {
        if(TargetCharacter == null)
            return;

        if(atkType == ATTACK_TYPE.NORMAL || atkType == ATTACK_TYPE.SKILLATK)
        {
            int dodgeRandomValue = UnityEngine.Random.Range(0, 100);
            if (dodgeRandomValue <= TargetCharacter.Dodge)
                return;

            TargetCharacter.TakeDamage(damage);
            return;
        }
        else if(atkType == ATTACK_TYPE.ULTIMATEATK)
        {
            if(curTurnCharacter.TryGetComponent<Playerable>(out var playerable))
            {
                List<Monster> copyList = new List<Monster>();
                foreach (Monster mon in curMonsterList)
                {
                    copyList.Add(mon);
                }

                foreach (Monster mon in copyList)
                    mon?.TakeDamage(damage);
            }
            else
            {
                List<Playerable> copyList = new List<Playerable>();
                foreach(Playerable p in user.Deck)
                {
                    copyList.Add(p);
                }
                
                foreach (Playerable target in copyList)
                {
                    if(target.isDie == false)
                        target?.TakeDamage(damage);
                }
            }
        }
    }

    public void NextOrder()
    {
        if (battleCharacterList.Count <= 0)
            return;

        index++;
        if (index >= battleCharacterList.Count)
            index = 0;


        if(battleCharacterList[index].isDie == true)
        {
            NextOrder();
            return;
        }
        CurTurnCharacter = battleCharacterList[index];

        StartCoroutine(WaitSeCo());
    }
    public void StageEnd()
    {
        int listIndex = 0;
        UIManager.instance.ExitBattleUI();
        targetSelectEffect.SetActive(false);

        foreach (Playerable p in user.Deck)
            p.OnDie = null;

        foreach (Playerable p in user.Deck)
            p.ReturnPool();

        foreach(Playerable player in user.Deck)
        {
            ReturnState(tempPlayer[listIndex], player);
            listIndex++;
        }

        GameManager.instance.LoadScene("MainScene");
        UIManager.instance.MainSceneUI(true);
    }

    //스테이지 종료 후 이전 스탯으로 돌아가야함
    public void ReturnState(Playerable tempPlayer, Playerable curPlayer)
    {
        curPlayer.Def = tempPlayer.Def;
        curPlayer.Atk = tempPlayer.Atk;
        curPlayer.Speed = tempPlayer.Speed;
    }

    public void TurnStart()
    {
        if (curMonsterList == null)
            return;

        foreach (Character c in user.Deck)
            battleCharacterList.Add(c);

        foreach (Character c in curMonsterList)
            battleCharacterList.Add(c);

        battleCharacterList.Sort();

        UIManager.instance.turnTableUIController.Init();
    }

    void SetBattleCharacterList()
    {
        curMonsterList = monsterManager.GetCurrentRoundMonsters();

        MonsterCount = curMonsterList.Count;
        UserCount = user.Deck.Count;

        foreach (Character mon in curMonsterList)
            mon.OnDie += () => { MonsterCount--; };

        TurnStart();
    }

    IEnumerator WaitSeCo()
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.instance.turnTableUIController.MoveTurnTable();
    }


    public void BadEnd()
    {
        UIManager.instance.ExitBattleUI();
        targetSelectEffect.SetActive(false);

        foreach (Playerable p in user.Deck)
            p.OnDie = null;

        foreach (Playerable p in user.Deck)
            p.ReturnPool();

        GameManager.instance.LoadScene("MainScene");
        UIManager.instance.MainSceneUI(true);
    }
}
