using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using ExitGames.Client.Photon;

[System.Serializable]
public class CardDataSerialization
{
    public static readonly byte[] memCard = new byte[3 * 4];

    public static short SerializeCard(StreamBuffer outStream, object cardObjData) {
        Card cardData = (Card)cardObjData;

        int cardValue = cardData.Value;
        int cardTypeID = 0;
        int cardLevelID = 0;

        if(cardData.Type.ToString() == "Snake") {
            cardTypeID = 1;
        } else if(cardData.Type.ToString() == "Snake2Head") {
            cardTypeID = 2;
        } else if(cardData.Type.ToString() == "Ladder") {
            cardTypeID = 3;
        } else if(cardData.Type.ToString() == "Ladder2Tail") {
            cardTypeID = 4;
        }

        if(cardData.Level == Card.CardLevel.One) {
            cardLevelID = 1;
        } else if(cardData.Level == Card.CardLevel.Two) {
            cardLevelID = 2;
        } else if(cardData.Level == Card.CardLevel.Three) {
            cardLevelID = 3;
        }

        lock(memCard) {
            byte[] bytes = memCard;
            int off = 0;
            Protocol.Serialize(cardValue, bytes, ref off);
            Protocol.Serialize(cardTypeID, bytes, ref off);
            Protocol.Serialize(cardLevelID, bytes, ref off);
            outStream.Write(bytes, 0, 3 * 4);
        }

        return 3 * 4;
    }

    public static object DeserializeCard(StreamBuffer inStream, short length) {
        int cardValue, cardTypeID, cardLevelID;
        Card.CardLevel cardLevel = Card.CardLevel.None;
        Card.CardType cardType = Card.CardType.None;

        lock(memCard) {
            inStream.Read(memCard, 0, 3 * 4);
            int off = 0;
            Protocol.Deserialize(out cardValue, memCard, ref off);
            Protocol.Deserialize(out cardTypeID, memCard, ref off);
            Protocol.Deserialize(out cardLevelID, memCard, ref off);
        }

        if(cardTypeID == 1) {
            cardType = Card.CardType.Snake;
        } else if(cardTypeID == 2) {
            cardType = Card.CardType.Snake2Head;
        } else if(cardTypeID == 3) {
            cardType = Card.CardType.Ladder;
        } else if(cardTypeID == 4) {
            cardType = Card.CardType.Ladder2Tail;
        }

        if(cardLevelID == 1) {
            cardLevel = Card.CardLevel.One;
        } else if(cardLevelID == 2) {
            cardLevel = Card.CardLevel.Two;
        } else if(cardLevelID == 3) {
            cardLevel = Card.CardLevel.Three;
        }

        Card boughtCard = new Card(cardValue, cardLevel, cardType);

        return boughtCard;
        
    }
}
