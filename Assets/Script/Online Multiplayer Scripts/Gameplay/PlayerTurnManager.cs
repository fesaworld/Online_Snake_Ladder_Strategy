using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;

public class PlayerTurnManager : MonoBehaviourPunCallbacks
{
    public Image TurnInfoPanelRenderer;
    public Sprite PlayerTurnImage, EnemyTurnImage;

    GameplayManager gameplayManager;
    NewDice newDice;
    CardManager cardManager;

    public int currentTurn = 1;

    string message;

    // Start is called before the first frame update
    void Start()
    {
        //gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();
        //cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        //newDice = GameObject.Find("Dice").GetComponent<NewDice>();

        //checkPlayerTurn();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);

        gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        newDice = GameObject.Find("Dice").GetComponent<NewDice>();

        checkPlayerTurn();
    }

    public void OnClick_EndTurnButton() {
        StartCoroutine(EndTurn());
        newDice.GetSixCounts = 0;
    }

    void checkPlayerTurn() {
        if(PlayerScript.PlayerTurn == currentTurn) {
            DisplayTurnInfo(true);
            TurnInfoPanelRenderer.sprite = PlayerTurnImage;
            newDice.RollButton.interactable = true;

            StartCoroutine(gameplayManager.itemOnBoardAgeUpdate());
        } else {
            DisplayTurnInfo(false);
            TurnInfoPanelRenderer.sprite = EnemyTurnImage;
            newDice.RollButton.interactable = false;
        }
    }

    void DisplayTurnInfo(bool isMyTurn) {
        if(isMyTurn) {
            string message = "your turn";
            StartCoroutine(gameplayManager.DisplayNotificationInfo(message, 2f, false));
        } else {
            if(currentTurn.Equals(1)) {
                message = Constant.FirstTurnPlayerNickname + " turn";
            } else if(currentTurn.Equals(2)) {
                message = Constant.SecondTurnPlayerNickname + " turn";
            } else if(currentTurn.Equals(3)) {
                message = Constant.ThirdTurnPlayerNickname + " turn";
            } else if(currentTurn.Equals(4)) {
                message = Constant.FourthTurnPlayerNickname + " turn";
            }
            StartCoroutine(gameplayManager.DisplayNotificationInfo(message, 2f, true));
        }
    }

    IEnumerator EndTurn() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        if(currentTurn < Rooms.expectedMaxPlayer)
        {
            currentTurn += 1;
        }
        else
        {
            currentTurn = 1;
        }

        object[] currentTurnData = new object[] { currentTurn };

        PhotonNetwork.RaiseEvent(RaiseEventCode.CURRENT_PLAYER_END_TURN, currentTurnData, RaiseEventOptions.Default, SendOptions.SendReliable);

        yield return new WaitForSeconds(0.5f);

        checkPlayerTurn();
    }

    new void OnEnable() {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    new void OnDisable() {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    public void NetworkingClient_EventReceived(EventData obj) {
        if(obj.Code == RaiseEventCode.CURRENT_PLAYER_END_TURN) {
            object[] currentTurnData = (object[])obj.CustomData;
            
            currentTurn = (int)currentTurnData[0];
            checkPlayerTurn();
        }
    }
}
