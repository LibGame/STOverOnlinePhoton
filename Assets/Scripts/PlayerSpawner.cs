using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public CameraController CameraController;
    [SerializeField] private PhotonView photonView;
    public static PlayerSpawner Instance;
    public FloatingJoystick FloatingJoystick;
    public GameObject MainCanvas;
    public GameObject CameraMain;

    public void SpawnPlayer()
    {
        Color32 colorPlayer = GameController.Instance.GetColorFromArrayQueue();
        int xPos = Random.Range(0, GlobalSettings.Instance.WorldSettings.HorizontalSize);
        int zPos = Random.Range(0, GlobalSettings.Instance.WorldSettings.VerticalSize);
        var player = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(xPos, 3, zPos), Quaternion.identity);
        var playerComponent = new Player();
        var playerController = player.GetComponent<PlayerController>();
        playerController.Player = playerComponent;
        playerController.SetParams(CameraMain, MainCanvas, FloatingJoystick);
        int id = GameController.Instance.PlayersPerID.Count;
        
        playerComponent.Speed = 5;
        playerComponent.ColorIndex = GameController.Instance.ColorLastIndex;
        playerComponent.PlayerIndex = id;
        playerComponent.PlayerColor = colorPlayer;
        playerController.CallSetColor(colorPlayer);
        player.GetComponent<TakeBlock>().Player = playerComponent;
        GameController.Instance.Players.Add(playerComponent);
        GameController.Instance.PlayersPerID.Add(id, playerComponent);
        CameraController.Target = player;
        //object[] array = GameController.Instance.ColorByIndex.ToArray();
        GameController.Instance.AddInHashTable("ColorByIndex", GameController.Instance.PackArrayColor(GameController.Instance.ColorByIndex));
    }
}
