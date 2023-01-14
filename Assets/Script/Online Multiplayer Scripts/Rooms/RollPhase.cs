using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Threading.Tasks;

public class RollPhase : MonoBehaviourPun
{
    public static int result_RollP1, result_RollP2, result_RollP3, result_RollP4;
    public static bool isRollPhaseDone = false;
    public Text TurnTextPlayer1, TurnTextPlayer2, TurnTextPlayer3, TurnTextPlayer4;
    List<int> turnNumberAvailable = new List<int>();

    void Start()
    {
        if (Rooms.expectedMaxPlayer.Equals(2))
        {
            turnNumberAvailable = new List<int>(new int[] { 1, 2 });
        }
        else if (Rooms.expectedMaxPlayer.Equals(3))
        {
            turnNumberAvailable = new List<int>(new int[] { 1, 2, 3 });
        }
        else if (Rooms.expectedMaxPlayer.Equals(4))
        {
            turnNumberAvailable = new List<int>(new int[] { 1, 2, 3, 4 });
        }
    }

    public void OnClick_RollButton()
    {
        //SoundManager.PlaySoundEffect("ButtonClick");
        RollingPlayerTurn();
    }

    void RollingPlayerTurn()
    {
        if (Rooms.expectedMaxPlayer.Equals(2))
        {
            rollingTurn2PlayerMode();
        }
        else if (Rooms.expectedMaxPlayer.Equals(3))
        {
            rollingTurn3PlayerMode();
        }
        else if (Rooms.expectedMaxPlayer.Equals(4))
        {
            rollingTurn4PlayerMode();
        }

        isRollPhaseDone = true;

        object[] rollDoneStateData = new object[] { isRollPhaseDone };
        PhotonNetwork.RaiseEvent(RaiseEventCode.ROLL_PHASE_DONE, rollDoneStateData, RaiseEventOptions.Default, SendOptions.SendReliable);
        //Debug.LogError("isi dari roll phase : " + isRollPhaseDone);
    }

    void rollingTurn2PlayerMode()
    {
        result_RollP1 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(result_RollP1);

        result_RollP2 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(result_RollP2);

        GetMyTurnValue(result_RollP1, result_RollP2, result_RollP3, result_RollP4);

        object[] playerTurnDatas = new object[] { result_RollP1, result_RollP2 };

        PhotonNetwork.RaiseEvent(RaiseEventCode.ROLL_PLAYER_TURN, playerTurnDatas, RaiseEventOptions.Default, SendOptions.SendReliable);

        TurnTextPlayer1.text = "Giliran: " + result_RollP1;
        TurnTextPlayer2.text = "Giliran: " + result_RollP2;
    }

    void rollingTurn3PlayerMode()
    {
        result_RollP1 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(result_RollP1);

        result_RollP2 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(result_RollP2);

        result_RollP3 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(result_RollP3);

        GetMyTurnValue(result_RollP1, result_RollP2, result_RollP3);

        object[] playerTurnDatas = new object[] { result_RollP1, result_RollP2, result_RollP3 };

        PhotonNetwork.RaiseEvent(RaiseEventCode.ROLL_PLAYER_TURN, playerTurnDatas, RaiseEventOptions.Default, SendOptions.SendReliable);

        TurnTextPlayer1.text = "Giliran: " + result_RollP1;
        TurnTextPlayer2.text = "Giliran: " + result_RollP2;
        TurnTextPlayer3.text = "Giliran: " + result_RollP3;
    }

    void rollingTurn4PlayerMode()
    {
        result_RollP1 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(result_RollP1);

        result_RollP2 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(result_RollP2);

        result_RollP3 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(result_RollP3);

        result_RollP4 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(result_RollP4);

        GetMyTurnValue(result_RollP1, result_RollP2, result_RollP3, result_RollP4);

        object[] playerTurnDatas = new object[] { result_RollP1, result_RollP2, result_RollP3, result_RollP4 };

        PhotonNetwork.RaiseEvent(RaiseEventCode.ROLL_PLAYER_TURN, playerTurnDatas, RaiseEventOptions.Default, SendOptions.SendReliable);

        TurnTextPlayer1.text = "Giliran: " + result_RollP1;
        TurnTextPlayer2.text = "Giliran: " + result_RollP2;
        TurnTextPlayer3.text = "Giliran: " + result_RollP3;
        TurnTextPlayer4.text = "Giliran: " + result_RollP4;
    }

    private void GetMyTurnValue(int turnP1, int turnP2)
    {
        if (PlayerScript.PlayerNumber.Equals(1))
        {
            PlayerScript.PlayerTurn = turnP1;
            savePlayerNicknameToConstant(PlayerScript.PlayerNickname, turnP1);
        }
        else if (PlayerScript.PlayerNumber.Equals(2))
        {
            PlayerScript.PlayerTurn = turnP2;
            savePlayerNicknameToConstant(PlayerScript.PlayerNickname, turnP2);
        }
    }

    private void GetMyTurnValue(int turnP1, int turnP2, int turnP3)
    {
        if (PlayerScript.PlayerNumber.Equals(1))
        {
            PlayerScript.PlayerTurn = turnP1;
            savePlayerNicknameToConstant(PlayerScript.PlayerNickname, turnP1);
        }
        else if (PlayerScript.PlayerNumber.Equals(2))
        {
            PlayerScript.PlayerTurn = turnP2;
            savePlayerNicknameToConstant(PlayerScript.PlayerNickname, turnP2);
        }
        else if (PlayerScript.PlayerNumber.Equals(3))
        {
            PlayerScript.PlayerTurn = turnP3;
            savePlayerNicknameToConstant(PlayerScript.PlayerNickname, turnP3);
        }
    }

    private void GetMyTurnValue(int turnP1, int turnP2, int turnP3, int turnP4)
    {
        if (PlayerScript.PlayerNumber.Equals(1))
        {
            PlayerScript.PlayerTurn = turnP1;
            savePlayerNicknameToConstant(PlayerScript.PlayerNickname, turnP1);
        }
        else if (PlayerScript.PlayerNumber.Equals(2))
        {
            PlayerScript.PlayerTurn = turnP2;
            savePlayerNicknameToConstant(PlayerScript.PlayerNickname, turnP2);
        }
        else if (PlayerScript.PlayerNumber.Equals(3))
        {
            PlayerScript.PlayerTurn = turnP3;
            savePlayerNicknameToConstant(PlayerScript.PlayerNickname, turnP3);
        }
        else if (PlayerScript.PlayerNumber.Equals(4))
        {
            PlayerScript.PlayerTurn = turnP4;
            savePlayerNicknameToConstant(PlayerScript.PlayerNickname, turnP4);
        }
    }

    void savePlayerNicknameToConstant(string nickname, int turnNumber)
    {
        if (turnNumber.Equals(1))
        {
            Constant.FirstTurnPlayerNickname = nickname;
            sendNicknameDataToAnotherPlayer(Constant.FirstTurnPlayerNickname, 1);
        }
        else if (turnNumber.Equals(2))
        {
            Constant.SecondTurnPlayerNickname = nickname;
            sendNicknameDataToAnotherPlayer(Constant.SecondTurnPlayerNickname, 2);
        }
        else if (turnNumber.Equals(3))
        {
            Constant.ThirdTurnPlayerNickname = nickname;
            sendNicknameDataToAnotherPlayer(Constant.ThirdTurnPlayerNickname, 3);
        }
        else if (turnNumber.Equals(4))
        {
            Constant.FourthTurnPlayerNickname = nickname;
            sendNicknameDataToAnotherPlayer(Constant.FourthTurnPlayerNickname, 4);
        }
    }

    void sendNicknameDataToAnotherPlayer(string nickname, int turnNumber)
    {
        object[] playerNicknameData = new object[] { nickname, turnNumber };

        PhotonNetwork.RaiseEvent(RaiseEventCode.SEND_PLAYER_NICKNAME_TO_OTHERS, playerNicknameData, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    public void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == RaiseEventCode.ROLL_PLAYER_TURN)
        {
            object[] playerTurnDatas = (object[])obj.CustomData;
            int result_RollP1_ForOtherScreen, result_RollP2_ForOtherScreen, result_RollP3_ForOtherScreen, result_RollP4_ForOtherScreen;

            if (Rooms.expectedMaxPlayer == 2)
            {
                result_RollP1_ForOtherScreen = (int)playerTurnDatas[0];
                result_RollP2_ForOtherScreen = (int)playerTurnDatas[1];

                GetMyTurnValue(result_RollP1_ForOtherScreen, result_RollP2_ForOtherScreen);

                TurnTextPlayer1.text = "Giliran: " + result_RollP1_ForOtherScreen;
                TurnTextPlayer2.text = "Giliran: " + result_RollP2_ForOtherScreen;
            }
            else if (Rooms.expectedMaxPlayer == 3)
            {
                result_RollP1_ForOtherScreen = (int)playerTurnDatas[0];
                result_RollP2_ForOtherScreen = (int)playerTurnDatas[1];
                result_RollP3_ForOtherScreen = (int)playerTurnDatas[2];

                GetMyTurnValue(result_RollP1_ForOtherScreen, result_RollP2_ForOtherScreen, result_RollP3_ForOtherScreen);

                TurnTextPlayer1.text = "Giliran: " + result_RollP1_ForOtherScreen;
                TurnTextPlayer2.text = "Giliran: " + result_RollP2_ForOtherScreen;
                TurnTextPlayer3.text = "Giliran: " + result_RollP3_ForOtherScreen;
            }
            else if (Rooms.expectedMaxPlayer == 4)
            {
                result_RollP1_ForOtherScreen = (int)playerTurnDatas[0];
                result_RollP2_ForOtherScreen = (int)playerTurnDatas[1];
                result_RollP3_ForOtherScreen = (int)playerTurnDatas[2];
                result_RollP4_ForOtherScreen = (int)playerTurnDatas[3];

                GetMyTurnValue(result_RollP1_ForOtherScreen, result_RollP2_ForOtherScreen, result_RollP3_ForOtherScreen, result_RollP4_ForOtherScreen);

                TurnTextPlayer1.text = "Giliran: " + result_RollP1_ForOtherScreen;
                TurnTextPlayer2.text = "Giliran: " + result_RollP2_ForOtherScreen;
                TurnTextPlayer3.text = "Giliran: " + result_RollP3_ForOtherScreen;
                TurnTextPlayer4.text = "Giliran: " + result_RollP4_ForOtherScreen;
            }
        }
        else if (obj.Code == RaiseEventCode.SEND_PLAYER_NICKNAME_TO_OTHERS)
        {
            object[] playerNicknameData = (object[])obj.CustomData;

            string nickname = (string)playerNicknameData[0];
            int turnNumber = (int)playerNicknameData[1];

            if (turnNumber.Equals(1))
            {
                Constant.FirstTurnPlayerNickname = nickname;
            }
            else if (turnNumber.Equals(2))
            {
                Constant.SecondTurnPlayerNickname = nickname;
            }
            else if (turnNumber.Equals(3))
            {
                Constant.ThirdTurnPlayerNickname = nickname;
            }
            else if (turnNumber.Equals(4))
            {
                Constant.FourthTurnPlayerNickname = nickname;
            }
        }
    }
}