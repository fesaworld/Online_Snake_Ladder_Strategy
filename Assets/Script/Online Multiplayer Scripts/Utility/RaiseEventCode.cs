using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseEventCode : MonoBehaviour
{
    public const byte ALL_PLAYERS_IN_ROOM = 1;
    public const byte ENTER_THE_GAME = 2;
    public const byte ROLL_PLAYER_TURN = 3;
    public const byte CURRENT_PLAYER_END_TURN = 4;
    public const byte SEND_PLAYER_NICKNAME_TO_OTHERS = 5;
    public const byte SHOW_BOUGHT_CARD_NOTIFICATION = 6;
    public const byte SEND_PILE_OF_CARDS_DATA = 7;
    public const byte SEND_ATTACHED_ITEM_DATA = 8;
    public const byte ITEM_USED_BY_PLAYER = 9;
    public const byte PLAYER_SELL_CARD = 10;
    public const byte PLAYER_ATTACH_ITEM_ON_BOARD = 11;
    public const byte PLAYER_STOP_AT_LAST_SQUARE = 12;
    public const byte ITEM_DESTROYED_AFTER_THREE_TURN = 13;
    public const byte ROLL_PHASE_DONE = 14;
    public const byte HOST_READY = 15;
    public const byte OTHER_PLAYER_READY = 16;
}
