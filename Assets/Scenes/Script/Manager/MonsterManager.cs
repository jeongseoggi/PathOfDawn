using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public int round;
    const int lastRound = 3;

    [SerializeField] List<Character> firstRoundMonsters;
    [SerializeField] List<Character> secondRoundMonsters;
    [SerializeField] List<Character> thirdRoundMonsters;

    List<Character>[] roundMonsters;

    public MonsterUIController monsterUIController;

    public void Start()
    {
        round = 0;
        Init();
    }

    private void Init()
    {
        roundMonsters = new List<Character>[lastRound];

        roundMonsters[0] = firstRoundMonsters;
        roundMonsters[1] = secondRoundMonsters;
        roundMonsters[2] = thirdRoundMonsters;
    }

    public List<Character> GetCurrentRoundMonsters()
    {
        foreach(Monster m in roundMonsters[round])
            m.gameObject.SetActive(true);

        for(int i = 0; i < roundMonsters[round].Count; i++)
        {
            monsterUIController.monsterUISlots[i].Init((Monster)roundMonsters[round][i]);
        }
        // 몬스터 UI 초기화

        UIManager.instance.monsterUIController.gameObject.SetActive(true);
        UIManager.instance.turnTableUIController.gameObject.SetActive(true);

        return roundMonsters[round];
    }
}
