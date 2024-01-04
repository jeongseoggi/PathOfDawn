using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SungMi
{
    // �ʿ��� ĳ���Ͱ� Point�� �°� �̵��ϸ�, �Կ��Ǵ� ���� �����ϱ� ����
    public class PointMove : MonoBehaviour
    {
        public GameObject[] movePoint = new GameObject[5];
        public Vector3 destination;    // ���� ���� ��
        public NavMeshAgent agent;
        public int index;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            index = 0;
            agent.destination = movePoint[index].transform.position;
            destination = agent.destination;
        }

        public void NextPosition()
        {
            index++;
            if (index >= movePoint.Length)
                return;

            agent.destination = movePoint[index].transform.position;
            destination = agent.destination;
        }
    }
}