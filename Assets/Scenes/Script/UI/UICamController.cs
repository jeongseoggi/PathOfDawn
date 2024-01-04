using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICamController : MonoBehaviour
{
    public Transform[] pivots;

    float posZ;

    public int curPivot;
    Stack<int> prevPivotStack;

    private void Start()
    {
        UIManager.instance.uICamController  = this;

        posZ = transform.position.z;
        prevPivotStack = new Stack<int>();

        curPivot = 0;
    }

    public void Back()
    {
        if (prevPivotStack.Count < 1)
            return;

        curPivot = prevPivotStack.Pop();

        Vector3 pos = new Vector3(pivots[curPivot].position.x, pivots[curPivot].position.y, posZ);
        transform.position = pos;
    }

    public void SetCamPostion(int num)
    {
        prevPivotStack.Push(curPivot);
        curPivot = num;

        Vector3 pos = new Vector3(pivots[num].position.x, pivots[num].position.y, posZ);
        transform.position = pos;

        // SoundManager.intance.Play(num);
    }
}
