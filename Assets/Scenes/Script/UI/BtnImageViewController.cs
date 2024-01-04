using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnImageViewController : MonoBehaviour
{
    public Image[] btnImages;

    private void Start()
    {
        SetImage();
    }

    public void SetImage()
    {
        for(int i = 0; i < User.instance.MyCharacters.Count; i++)
            btnImages[i].sprite = User.instance.MyCharacters[i].character_image;
    }
}
