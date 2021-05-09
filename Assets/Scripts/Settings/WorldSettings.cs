using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSettings : MonoBehaviour
{
    [SerializeField] private int horizontalSize;
    [SerializeField] private int verticalSize;

    public int HorizontalSize { get { if (horizontalSize > 0) return horizontalSize; else return 10; } }
    public int VerticalSize { get { if (verticalSize > 0) return verticalSize; else return 10; } }

}
