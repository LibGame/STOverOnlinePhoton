using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TakeBlock : MonoBehaviour
{
    public Player Player;
    public PhotonView PhotonView;

    void Update()
    {
        if (!PhotonView.IsMine) return;

        Vector2Int pos = Utilites.Instance.GetPositionOnBlock(transform.position);
        if (Utilites.Instance.CheckBlockOwner(pos, Player.PlayerIndex))
        {
            if (Utilites.Instance.CheckBlockOwner(pos, -1))
            {
                Utilites.Instance.ChangeOwnerBlock(pos);
            }
            GameController.Instance.CallChangeColorCell(pos.x, pos.y, Player.PlayerIndex, Player.PlayerColor);
            GameController.Instance.CallIncreasePlayerScore(Player.PlayerIndex);
        }
    }


}
