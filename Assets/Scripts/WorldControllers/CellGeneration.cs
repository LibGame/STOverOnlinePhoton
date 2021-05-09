using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CellGeneration : MonoBehaviour
{
    [SerializeField] private GameObject CellPrefab;


    public void GenerateCell()
    {
        for(int x = 0; x < GlobalSettings.Instance.WorldSettings.HorizontalSize; x++)
        {
            for (int y = 0; y < GlobalSettings.Instance.WorldSettings.HorizontalSize; y++)
            {
                var cell = Instantiate(CellPrefab, new Vector3(x, 0, y), Quaternion.identity);
                GameController.Instance.Cells[x, y] = cell.GetComponent<Cell>();
            }
        }
    }
}
