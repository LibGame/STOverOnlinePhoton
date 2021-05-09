using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text LogText;
    public InputField SetName;

    private void Start()
    {

        PhotonNetwork.GameVersion = "1";

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Log("Connect to master");

    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(SetName.text, new Photon.Realtime.RoomOptions { MaxPlayers = 3 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(SetName.text);
    }

    public override void OnJoinedRoom()
    {
        Log("Joined the room");
        PhotonNetwork.NickName = SetName.text;
        PhotonNetwork.LoadLevel("GameScene");
    }

    private void Log(string message)
    {
        Debug.Log(message);
        LogText.text += "\n";
        LogText.text += message;
    }
}
