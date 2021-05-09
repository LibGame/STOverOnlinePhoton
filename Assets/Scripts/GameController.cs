using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using Photon.Realtime;

public enum PhotonEventCodes
{
    ColorNextPlayer = 0,
    UpdateSend = 1
}

public class GameController : MonoBehaviour, IPunObservable
{
    public PhotonView PhotonView;
    public static GameController Instance;
    public CellGeneration CellGeneration;
    public PlayerSpawner PlayerSpawner;
    public Cell[,] Cells;
    public List<Player> Players = new List<Player>();
    //public List<Color> PlayerByColor = new List<Color>();
    public List<Color32> ColorByIndex = new List<Color32>();
    public Dictionary<int, object> PlayersPerID = new Dictionary<int, object>();
    public int ColorLastIndex;
    public Color ColorNextPlayer;
    public Text Text;
    private ExitGames.Client.Photon.Hashtable data = new ExitGames.Client.Photon.Hashtable();

    public void Awake()
    {

        Instance = GetComponent<GameController>();

        PhotonPeer.RegisterType(typeof(Player), 255, DataNetwork.SerializePlayer, DataNetwork.DeserializePlayer);
        PhotonPeer.RegisterType(typeof(Color32), (byte)'C', SerializeColor, DeserializeColor);
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
 
        ColorByIndex.Add(Color.magenta);
        ColorByIndex.Add(Color.yellow);
        ColorByIndex.Add(Color.blue);
        ColorByIndex.Add(Color.red);
        if (!PhotonNetwork.IsMasterClient)
        {
            ColorByIndex = UnPackArrayColor((string[])PhotonNetwork.CurrentRoom.CustomProperties["ColorByIndex"]);
        }
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public string[] PackArrayColor(List<Color32> array)
    {
        string[] exitArray = new string[array.Count - 1];
        for (int i = 0; i < array.Count - 1; i++)
        {
            string code = "";

            code += array[i].r.ToString() + "/";
            code += array[i].g.ToString() + "/";
            code += array[i].b.ToString() + "/";
            exitArray[i] = code;
        }

        return exitArray;
    }

    public List<Color32> UnPackArrayColor(string[] array)
    {
        List<Color32> exitArray = new List<Color32>();
        for(int i = 0; i < array.Length; i++)
        {
            char[] ar = array[i].ToCharArray();
            string q = "";
            List<int> cArr = new List<int>();

            for (int k = 0; k < ar.Length; k++)
            {
                if(ar[k].ToString() != "/")
                {
                    q += ar[k].ToString();
                }
                else
                {
                    cArr.Add(int.Parse(q));
                    q = "";
                }
            }

            exitArray.Add(new Color32((byte)cArr[0], (byte)cArr[1], (byte)cArr[2],255));
            cArr.RemoveRange(0, cArr.Count - 1);
        }

 
        return exitArray;
    }

    public void OnEvent(EventData photonEvent)
    {
        PhotonEventCodes code = (PhotonEventCodes)photonEvent.Code;

        if (code == PhotonEventCodes.ColorNextPlayer)
        {
            object[] data = (object[])photonEvent.CustomData;

            if(data.Length == 1)
            {
                ColorLastIndex = (int)data[0];
            }
        }
    }

    public Color32 TakeFromListLikeQueue(List<Color32> list)
    {
        var obj = list[list.Count -  1];
        list.RemoveAt(list.Count - 1);
        return obj;
    }

    public void Start()
    {
        Cells = new Cell[GlobalSettings.Instance.WorldSettings.HorizontalSize, GlobalSettings.Instance.WorldSettings.VerticalSize];

        CellGeneration.GenerateCell();
        PlayerSpawner.SpawnPlayer();
    }

    public void Update()
    {
        Text.text = ColorLastIndex.ToString();
    }


    public Color32 GetColorFromArrayQueue()
    {
        Color32 c = ColorByIndex[ColorByIndex.Count - 1];
        ColorByIndex.RemoveAt(ColorByIndex.Count - 1);
        return c;
    }

    public void AddInHashTable(string name,object type)
    {
        ExitGames.Client.Photon.Hashtable data = new ExitGames.Client.Photon.Hashtable();
        data.Add(name, type);
        PhotonNetwork.CurrentRoom.SetCustomProperties(data);
    }

    public void GetFromHashTable(string name)
    {
        int lastIndex = (int)PhotonNetwork.CurrentRoom.CustomProperties["ColorLastIndex"];
        ExitGames.Client.Photon.Hashtable data = new ExitGames.Client.Photon.Hashtable();
    }

    public static object DeserializeColor(byte[] data)
    {
        Color32 result = new Color32();
        result.r = (byte)System.BitConverter.ToInt32(data,0);
        result.g = (byte)System.BitConverter.ToInt32(data, 4);
        result.b = (byte)System.BitConverter.ToInt32(data, 8);
        result.a = (byte)System.BitConverter.ToInt32(data, 12);
        return result;
    }

    public static byte[] SerializeColor(object obj)
    {
        Color32 color = (Color32)obj;
        byte[] result = new byte[16];

        System.BitConverter.GetBytes(color.r).CopyTo(result, 0);
        System.BitConverter.GetBytes(color.g).CopyTo(result, 4);
        System.BitConverter.GetBytes(color.b).CopyTo(result, 8);
        System.BitConverter.GetBytes(color.a).CopyTo(result, 12);

        return result;
    }


    public void SendColorNextPlayer()
    {
        ColorLastIndex++;
        object[] datas = new object[] { ColorLastIndex };
        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others
        };

        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.ColorNextPlayer, datas, options, SendOptions.SendReliable);
    }


    public void CallChangeColorCell(int x, int y, int index , Color32 color)
    {
        PhotonView.RPC(nameof(ChangeColorCell), RpcTarget.All, x, y, index , color);
    }

    [PunRPC]
    public void ChangeColorCell(int x , int y , int index, Color32 color)
    {
        Cells[x, y].ChangeColor(index, index, color);
    }

    public void CallIncreasePlayerScore(int playerIndex)
    {
        PhotonView.RPC(nameof(IncreasePlayerScore), RpcTarget.All, playerIndex);
    }

    public void CallDecreasePlayerScore(int playerIndex)
    {
        PhotonView.RPC(nameof(IncreasePlayerScore), RpcTarget.All, playerIndex);
    }

    [PunRPC]
    private void IncreasePlayerScore(int playerIndex)
    {
        Players[playerIndex].Score++;
        UIGame.Instance.DisplayPlayerScore(Players[playerIndex].Score);
    }

    [PunRPC]
    private void DecreasePlayerScore(int playerIndex)
    {
        Players[playerIndex].Score--;
        UIGame.Instance.DisplayPlayerScore(Players[playerIndex].Score);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(PlayersPerID);
            //stream.SendNext(ColorByIndex.ToArray());
        }
        else
        {
            PlayersPerID = (Dictionary<int, object>)stream.ReceiveNext();
            //ColorByIndex = new List<Color>((Color[])stream.ReceiveNext());
        }
    }


}
