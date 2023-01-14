using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

[System.Serializable]
public class ItemDataSerialization
{
    public static readonly byte[] memItem = new byte[4 * 4];
    public static short SerializeItem(StreamBuffer outStream, object itemObjData) {
        Item itemData = (Item)itemObjData;

        int itemOwnerID = PhotonNetwork.LocalPlayer.ActorNumber;
        int itemAge = itemData.Age;
        int itemBoardPos = itemData.BoardPosition;
        int itemTypeID = 0;

        if(itemData.Type.ToString() == "Snake") {
            itemTypeID = 1;
        } else if(itemData.Type.ToString() == "Snake2Head") {
            itemTypeID = 2;
        } else if(itemData.Type.ToString() == "Ladder") {
            itemTypeID = 3;
        } else if(itemData.Type.ToString() == "Ladder2Tail") {
            itemTypeID = 4;
        }
        
        lock(memItem) {
            byte[] bytes = memItem;
            int off = 0;
            Protocol.Serialize(itemOwnerID, bytes, ref off);
            Protocol.Serialize(itemAge, bytes, ref off);
            Protocol.Serialize(itemBoardPos, bytes, ref off);
            Protocol.Serialize(itemTypeID, bytes, ref off);
            outStream.Write(bytes, 0, 4 * 4);
        }

        return 4 * 4;
    }

    public static object DeserializeItem(StreamBuffer inStream, short length) {
        int itemOwnerID, itemAge, itemBoardPos, itemTypeID;
        Card.CardType itemType = Card.CardType.None;
        
        lock(memItem) {
            inStream.Read(memItem, 0, 4 * 4);
            int off = 0;
            Protocol.Deserialize(out itemOwnerID, memItem, ref off);
            Protocol.Deserialize(out itemAge, memItem, ref off);
            Protocol.Deserialize(out itemBoardPos, memItem, ref off);
            Protocol.Deserialize(out itemTypeID, memItem, ref off);
        }

        if(itemTypeID == 1) {
            itemType = Card.CardType.Snake;
        } else if(itemTypeID == 2) {
            itemType = Card.CardType.Snake2Head;
        } else if(itemTypeID == 3) {
            itemType = Card.CardType.Ladder;
        } else if(itemTypeID == 4) {
            itemType = Card.CardType.Ladder2Tail;
        }

        Item attachedItem = new Item {
            Owner = PhotonNetwork.CurrentRoom.GetPlayer(itemOwnerID).NickName,
            Age = itemAge,
            BoardPosition = itemBoardPos,
            Type = itemType
        };

        return attachedItem;
    }
}
