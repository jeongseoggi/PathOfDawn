using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SungMi
{
    public class StartGame : MonoBehaviour
    {
        public Animator ani;
        private void Start()
        {
            ani = GetComponent<Animator>();
        }
    }
}