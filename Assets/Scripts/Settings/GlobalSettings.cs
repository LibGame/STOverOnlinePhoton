using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings Instance;

    public WorldSettings WorldSettings;

    public void Awake()
    {
        Instance = GetComponent<GlobalSettings>();
    }
}
