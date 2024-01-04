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
    public bool isRunCo = false;

    public int callIndex = 0;
    public int dieCount;
    public int CallIndex
    {
        get => callIndex;
        set
        {
            callIndex = value;
        }
    }

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
                isRunCo = true;
                Debug.Log(onDieChangeCoQueue.Count);
                yield return StartCoroutine(onDieChangeCoQueue.Dequeue());

            }
            yield return null;
            isRunCo = false;
        }
    }

    public void Init()
    {
        StartCoroutine(QueueControlCo());
        for (int i = 0; i < BattleManager.instance.BattleCharacterList.Count; i++)
        {
            images.Add(transform.GetChild(i).gameObject);
            images[i].GetComponent<TurnIcon>().owner = BattleManager.instance.BattleCharacterList[i];
            if (imgPrevPos[BattleManager.instance.BattleCharacterList.Count - 1] == Vector3.zero)
                imgPrevPos[i] = images[i].transform.position;
        }

        for (int i = 0; i < BattleManager.instance.BattleCharacterList.Count; i++)
        {
            int index = i;
            imagesPos.Add(imgPrevPos[i]);
            images[i].gameObject.SetActive(true);
            images[i].transform.GetChild(0).GetComponent<Image>().sprite = BattleManager.instance.BattleCharacterList[i].character_image;
            BattleManager.instance.BattleCharacterList[index].OnDie += () =>
            {
                if (BattleManager.instance.BattleCharacterList.Count > 0)
                    OnDieTurnTable(BattleManager.instance.BattleCharacterList[index]);
            };
        }
    }
    public void MoveTurnTable()
    {
        Debug.Log("무브함수 호출");
        //CallIndex++;
        GameObject c = images[0].GetComponentInParent<Transform>().gameObject;
        images.RemoveAt(0);
        images.Add(c);

        int index = 0;
        foreach (GameObject t in images)
        {
            //onDieChangeCoQueue.Enqueue(ChangeCO(index, t));
            StartCoroutine(ChangeCO(index, t));
            index++;
            if (index >= imagesPos.Count)
                break;
        }
    }


    public void OnDieTurnTable(Character c)
    {
        int removeIndex = 0;
        foreach (GameObject go in images)
        {
            if (go.GetComponent<TurnIcon>().owner == c)
            {
                go.SetActive(false);
                images.RemoveAt(removeIndex); //가정 3
                onDieChangeCoQueue.Enqueue(ChangeCO(removeIndex, go));
                break;
            }
            removeIndex++;
        }
    }

    //public void OnDieTurnTable(int removeTargetIndex)
    //{
    //    Debug.Log("다이함수 호출");
    //    Debug.Log(removeTargetIndex);

    //    if (BattleManager.instance.MonsterCount <= 0)
    //        return;
    //    if (CallIndex >= images.Count)
    //        CallIndex = 0;

    //    if (removeTargetIndex <= -1)
    //    {
    //        int newIndex = images.Count - (Mathf.Abs(removeTargetIndex));
    //        images[newIndex].gameObject.SetActive(false);
    //        images.RemoveAt(newIndex);
    //        imagesPos.RemoveAt(imagesPos.Count - 1);

    //        for (int i = newIndex; i < images.Count; i++) //A B C D E F ->  B C E F A
    //        {
    //            onDieChangeCoQueue.Enqueue(ChangeCO(i, images[i]));
    //            // StartCoroutine(ChangeCO(i, images[i]));
    //        }
    //    }
    //    else
    //    {
    //        images[removeTargetIndex].gameObject.SetActive(false);
    //        images.RemoveAt(removeTargetIndex);
    //        imagesPos.RemoveAt(imagesPos.Count - 1);
    //        for (int i = removeTargetIndex; i < images.Count; i++)
    //        {
    //            onDieChangeCoQueue.Enqueue(ChangeCO(i, images[i]));
    //            //StartCoroutine(ChangeCO(i, images[i]));
    //        }
    //    }


    //}

    IEnumerator ChangeCO(int i, GameObject image)
    {
        while (Vector3.Distance(image.transform.position, imagesPos[i]) >= 0.1f)
        {
            image.transform.position
               = Vector3.Lerp(image.transform.position, imagesPos[i], Time.deltaTime * 5.0f);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log(i + "Done");
    }

    public void Clear()
    {
        images.Clear();
        imagesPos.Clear();
        onDieChangeCoQueue.Clear();
    }

    private void OnDisable()
    {
        CallIndex = 0;
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