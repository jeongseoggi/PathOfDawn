using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SungMi
{
    // 맵에서 캐릭터가 Point에 맞게 이동하며, 촬영되는 것을 조절하기 위함
    public class PointMove : MonoBehaviour
    {
        public GameObject[] movePoint = new GameObject[5];
        public Vector3 destination;    // 도착 했을 때
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