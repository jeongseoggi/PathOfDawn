using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cams;
    public CinemachineDollyCart dollyCart;

    public void SetActiveCam(int index, bool isCheck)
    {
        dollyCart.m_Speed = isCheck ? 0 : 3.0f;
        cams[index].gameObject.SetActive(isCheck);
    }
}
