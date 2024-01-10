using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public TextMeshProUGUI fieldName;
    public TextMeshProUGUI value;
    public void SetValue(string fieldName, object value)
    {
        this.fieldName.text = fieldName + " : " + value.ToString();
    }
}
