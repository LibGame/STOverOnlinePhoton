using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class DataNetwork : MonoBehaviour
{
    public static byte[] SerializePlayer(object customType)
    {
        var c = (Player)customType;
        byte[] result = new byte[16];

        System.BitConverter.GetBytes(c.PlayerIndex).CopyTo(result, 0);
        System.BitConverter.GetBytes(c.Speed).CopyTo(result, 4);
        System.BitConverter.GetBytes(c.Score).CopyTo(result, 8);
        System.BitConverter.GetBytes(c.ColorIndex).CopyTo(result, 12);

        return result;
    }

    public static object DeserializePlayer(byte[] data)
    {
        var result = new Player();
        result.PlayerIndex = System.BitConverter.ToInt32(data, 0);
        result.Speed = System.BitConverter.ToInt32(data, 4);
        result.Score = System.BitConverter.ToInt32(data, 8);
        result.ColorIndex = System.BitConverter.ToInt32(data, 12);
        return result;
    }
}
