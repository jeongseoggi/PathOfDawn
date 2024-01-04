using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class TurnTableUIController : MonoBehaviour
{
    public List<GameObject> images;
    public List<Vector3> imagesPos;
    public Vector3[] imgPrevPos;
 
    public Queue<IEnumerator> onDieChangeCoQueue;

    private void Start()
    {
        UIManager.instance.turnTableUIController = this;
        imagesPos = new List<Vector3>();
        imgPrevPos = new Vector3[6];

        gameObject.SetActive(false);
        onDieChangeCoQueue = new Queue<IEnumerator>();
    }


    IEnumerator QueueControlCo()
    {
        while (true)
        {
            if (onDieChangeCoQueue.Count > 0)
            {
                yield return StartCoroutine(onDieChangeCoQueue.Dequeue()); //Dequeue로 빠진 코루틴이 실행이 끝나면 다음 코루틴 실행

            }
            yield return null;
        }
    }

    public void Init()
    {
        StartCoroutine(QueueControlCo());
        for (int i = 0; i < BattleManager.instance.BattleCharacterList.Count; i++)
        {
            int index = i;
            images.Add(transform.GetChild(i).gameObject); //이 오브젝트 자식들을 List에 넣어줌
            images[i].GetComponent<TurnIcon>().owner = BattleManager.instance.BattleCharacterList[i]; //각 이미지 슬롯에 주인을 정해줌
            if (imgPrevPos[BattleManager.instance.BattleCharacterList.Count - 1] == Vector3.zero) //이미지 포지션 배열의 마자막이 Vector3의 zero 즉, 포지션이 정해지지 않았으면
                imgPrevPos[i] = images[i].transform.position; //이미지 포지션을 같은 인덱스 번호의 이미지 슬롯의 포지션으로 포지션 저장

            imagesPos.Add(imgPrevPos[i]); //List에 저장
            images[i].gameObject.SetActive(true);
            images[i].transform.GetChild(0).GetComponent<Image>().sprite = BattleManager.instance.BattleCharacterList[i].character_image; //이미지 슬롯에 캐릭터의 이미지 띄워주기
            BattleManager.instance.BattleCharacterList[index].OnDie += () => //캐릭터가 죽었을 때 엮어주기
            {
                if (BattleManager.instance.BattleCharacterList.Count > 0)
                    OnDieTurnTable(BattleManager.instance.BattleCharacterList[index]);
            };

        }
    }
    public void MoveTurnTable() // 턴 이동 함수
    {
        GameObject c = images[0].GetComponentInParent<Transform>().gameObject;
        //List의 맨 앞 지우고 맨 뒤로 넣어주기 ex) 1->2->3 ---> 2->3->1
        images.RemoveAt(0); 
        images.Add(c);

        int index = 0;
        foreach (GameObject t in images)
        {
            StartCoroutine(ChangeCO(index, t)); //이미지 슬롯을 Lerp를 통해 이동 시켜주는 코루틴
            index++;
            if (index >= imagesPos.Count)
                break;
        }
    }


    public void OnDieTurnTable(Character c) //캐릭터가 죽었을 때 호출되는 함수
    {
        int removeIndex = 0;
        foreach (GameObject go in images)
        {
            if (go.GetComponent<TurnIcon>().owner == c) //반복하여 이미지 슬롯 주인 찾기(죽은 캐릭터)
            {
                go.SetActive(false);
                images.RemoveAt(removeIndex);
                onDieChangeCoQueue.Enqueue(ChangeCO(removeIndex, go)); //코루틴 큐에 넣어주기
                break;
            }
            removeIndex++;
        }
    }

    IEnumerator ChangeCO(int i, GameObject image)
    {
        while (Vector3.Distance(image.transform.position, imagesPos[i]) >= 0.1f) //해당 이미지를 해당 포지션으로 이동할 때 까지 반복
        {
            image.transform.position
               = Vector3.Lerp(image.transform.position, imagesPos[i], Time.deltaTime * 5.0f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Clear() //스테이지가 끝난 후 비워줘야 함
    {
        images.Clear();
        imagesPos.Clear();
        onDieChangeCoQueue.Clear();
    }

    private void OnDisable()
    {
        if (images.Count > 0)
        {
            int index = 0;
            foreach (Vector3 v in imgPrevPos)
            {
                transform.GetChild(index).transform.position = imgPrevPos[index];
                index++;
            }
        }
    }
}