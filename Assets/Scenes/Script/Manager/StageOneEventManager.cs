using SungMi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOneEventManager : EventManager
{
    public StartGame startGame;

    public GameObject victory;
    public GameObject lose;

    int curPosIndex;
    float timer = 0;
    bool isBattle = false;

    float endingTimer = 0;

    private void Start()
    {
        BattleManager.instance.eM = this;
       
        curPosIndex = 0;

        playerMove += () =>
        {
            foreach (Playerable p in User.instance.Deck)
            {
                p.sm.SetState(STATE.MOVE);
            }
        };
        playerMove += () => { pointMove.NextPosition(); };
        playerMove += () => { curPosIndex++; };

        playerWait += () =>
        {
            foreach (Playerable p in User.instance.Deck)
            {
                if (!(p.sm.CurState is PlayerIdleState))
                {
                    p.sm.SetState(STATE.IDLE);
                }
            }
        };
        playerWait += () =>
        {
            if (timer >= 2.5f)
            {
                cM.SetActiveCam(curPosIndex, false);
                timer = 0;
                playerMove();
            }
            timer += Time.deltaTime;
            startGame.ani.SetBool("GameStart", true);
        };


        battleStart += () => { isBattle = true; };
        battleStart += () => { BattleManager.instance.BattleStart(); };
        battleStart += () =>
        {
            foreach (Playerable p in User.instance.Deck)
                p.sm.SetState(STATE.IDLE);
        };

        battleEnd += () => { isBattle = false; };
        battleEnd += () => { cM.SetActiveCam(curPosIndex, false); };
        battleEnd += () => { playerMove(); };

        buffSelectStart += () => { UIManager.instance.buffController.buff.SetBuffSelectZone(); };

        eventDic.Add(eventPositions[0], playerMove);
        eventDic.Add(eventPositions[1], playerWait);
        eventDic.Add(eventPositions[2], battleStart);
        eventDic.Add(eventPositions[3], battleStart);
        eventDic.Add(eventPositions[4], battleStart);

        StageStart();
    }

    void StageStart()
    {
        playerMove();
    }

    public void Update()
    {
        if(IsArriveCurPosition() && isBattle == false)
        {
            cM.SetActiveCam(curPosIndex, true);
            eventDic[eventPositions[curPosIndex]]();
        }

        if(BattleManager.instance.isLose)
            lose.SetActive(true);
    }

    public bool IsArriveCurPosition()
    {
        if(curPosIndex >= eventPositions.Length)
        {
            endingTimer += Time.deltaTime;
            victory.SetActive(true);

            if (endingTimer >= 5.0f || Input.GetKeyDown(KeyCode.Space))
            {
                endingTimer = 0;
                BattleManager.instance.StageEnd();
            }

            return false;
        }

        Vector3 curPlayerPos = curPlayerTransform.position;
        Vector3 arrivePos = eventPositions[curPosIndex].position;

        return Vector3.Distance(curPlayerPos, arrivePos) <= 3.0f;
    }
}