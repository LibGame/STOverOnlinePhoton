using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cell : MonoBehaviour
{
    public Renderer renderer;
    public int PlayerIndexOwner { get; private set; }
    private void Awake()
    {
        PlayerIndexOwner = -1;
    }



    [PunRPC]
    public void ChangeColor(int colorIndex, int index , Color32 color)
    {
        PlayerIndexOwner = index;
        renderer.material.color = color;
    }
}
