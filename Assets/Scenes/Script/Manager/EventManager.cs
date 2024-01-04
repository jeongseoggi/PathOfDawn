using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SungMi;

public class EventManager : MonoBehaviour
{
    public CamManager cM;
    public PointMove pointMove;

    public Transform curPlayerTransform;
    public Transform[] eventPositions;

    public Dictionary<Transform, Action> eventDic = new Dictionary<Transform, Action>();

    public Action playerMove;
    public Action playerWait;

    public Action buffSelectStart;

    public Action battleStart;
    public Action battleEnd;
}
