using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class Gameover : MonoBehaviourPunCallbacks
{
    // public static int WinnerPlayerNumber = 0;
    public static string WinnerNickname;
    public Text NotificationText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(showGameoverNotification());
    }

    IEnumerator showGameoverNotification() {
        if(WinnerNickname == PlayerScript.PlayerNickname) {
            NotificationText.text = "You Win!";
        } else {
            NotificationText.text = "You Lose!";
        }

        yield return new WaitForSeconds(5f);
        
        Rooms.isRoomFull = false;
        if(PhotonNetwork.IsMasterClient) {
            PhotonNetwork.OpRemoveCompleteCache();
        }
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom(){
        Debug.Log("I leave the room");
        WinnerNickname = null;
        Constant.FirstTurnPlayerNickname = "Player 1";
        Constant.SecondTurnPlayerNickname = "Player 2";
        Constant.ThirdTurnPlayerNickname = "Player 3";
        Constant.FourthTurnPlayerNickname = "Player 4";
        //PhotonNetwork.CurrentRoom.MaxPlayers = 0;
        PhotonNetwork.LoadLevel("Mainmenu");
    }
}
