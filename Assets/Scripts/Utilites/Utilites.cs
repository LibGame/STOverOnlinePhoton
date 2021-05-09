using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilites : MonoBehaviour
{
    public static Utilites Instance;
    public void Awake()
    {
        Instance = GetComponent<Utilites>();
    }

    public Vector2Int GetPositionOnBlock(Vector3 pos)
    {
        return new Vector2Int(System.Convert.ToInt32(pos.x), System.Convert.ToInt32(pos.z));
    }

    public bool CheckBlockOwner(Vector2Int pos , int index)
    {
        if (GameController.Instance.Cells[pos.x, pos.y].PlayerIndexOwner == index)
        {
            return false;
        }
        return true;
    }

    public void ChangeOwnerBlock(Vector2Int pos)
    {
        GameController.Instance.CallDecreasePlayerScore(GameController.Instance.Cells[pos.x, pos.y].PlayerIndexOwner);
    }
}
