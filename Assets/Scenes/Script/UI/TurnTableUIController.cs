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
                yield return StartCoroutine(onDieChangeCoQueue.Dequeue()); //Dequeue�� ���� �ڷ�ƾ�� ������ ������ ���� �ڷ�ƾ ����

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
            images.Add(transform.GetChild(i).gameObject); //�� ������Ʈ �ڽĵ��� List�� �־���
            images[i].GetComponent<TurnIcon>().owner = BattleManager.instance.BattleCharacterList[i]; //�� �̹��� ���Կ� ������ ������
            if (imgPrevPos[BattleManager.instance.BattleCharacterList.Count - 1] == Vector3.zero) //�̹��� ������ �迭�� ���ڸ��� Vector3�� zero ��, �������� �������� �ʾ�����
                imgPrevPos[i] = images[i].transform.position; //�̹��� �������� ���� �ε��� ��ȣ�� �̹��� ������ ���������� ������ ����

            imagesPos.Add(imgPrevPos[i]); //List�� ����
            images[i].gameObject.SetActive(true);
            images[i].transform.GetChild(0).GetComponent<Image>().sprite = BattleManager.instance.BattleCharacterList[i].character_image; //�̹��� ���Կ� ĳ������ �̹��� ����ֱ�
            BattleManager.instance.BattleCharacterList[index].OnDie += () => //ĳ���Ͱ� �׾��� �� �����ֱ�
            {
                if (BattleManager.instance.BattleCharacterList.Count > 0)
                    OnDieTurnTable(BattleManager.instance.BattleCharacterList[index]);
            };

        }
    }
    public void MoveTurnTable() // �� �̵� �Լ�
    {
        GameObject c = images[0].GetComponentInParent<Transform>().gameObject;
        //List�� �� �� ����� �� �ڷ� �־��ֱ� ex) 1->2->3 ---> 2->3->1
        images.RemoveAt(0); 
        images.Add(c);

        int index = 0;
        foreach (GameObject t in images)
        {
            StartCoroutine(ChangeCO(index, t)); //�̹��� ������ Lerp�� ���� �̵� �����ִ� �ڷ�ƾ
            index++;
            if (index >= imagesPos.Count)
                break;
        }
    }


    public void OnDieTurnTable(Character c) //ĳ���Ͱ� �׾��� �� ȣ��Ǵ� �Լ�
    {
        int removeIndex = 0;
        foreach (GameObject go in images)
        {
            if (go.GetComponent<TurnIcon>().owner == c) //�ݺ��Ͽ� �̹��� ���� ���� ã��(���� ĳ����)
            {
                go.SetActive(false);
                images.RemoveAt(removeIndex);
                onDieChangeCoQueue.Enqueue(ChangeCO(removeIndex, go)); //�ڷ�ƾ ť�� �־��ֱ�
                break;
            }
            removeIndex++;
        }
    }

    IEnumerator ChangeCO(int i, GameObject image)
    {
        while (Vector3.Distance(image.transform.position, imagesPos[i]) >= 0.1f) //�ش� �̹����� �ش� ���������� �̵��� �� ���� �ݺ�
        {
            image.transform.position
               = Vector3.Lerp(image.transform.position, imagesPos[i], Time.deltaTime * 5.0f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Clear() //���������� ���� �� ������ ��
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