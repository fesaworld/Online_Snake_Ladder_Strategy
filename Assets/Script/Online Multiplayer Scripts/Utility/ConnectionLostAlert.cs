using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ConnectionLostAlert : MonoBehaviourPunCallbacks
{
    public GameObject ConnLostAlertOnly, ConnLostAlertWithReconnButton, ReconnectAlert;
    public Text ErrorMessageTextReconAlert, ErrorMessageTextAlertOnly, WarningMessageTextAlertOnly, ReconnectResultText;
    public Button OkButton;
    public static string DisconnectedFromScene;
    public static string DisconnectCauses;
    public static string DisconnectMessages;
    public static string roomCode;

    bool isReconnected = false;

    SceneLoader sceneLoader;

    void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

        if (DisconnectedFromScene == "Login" || DisconnectedFromScene == "Mainmenu" || DisconnectedFromScene == "Tutorial" ||
            DisconnectedFromScene == "SelectMode" || DisconnectedFromScene == "PartyMode" || DisconnectedFromScene == "CreatePartyMode")
        {
            if (DisconnectCauses == "Create or Join Room Failed")
            {
                ConnLostAlertOnly.SetActive(true);
                WarningMessageTextAlertOnly.text = "Gagal masuk ke dalam room!\n Silahkan periksa koneksi anda.";
                ErrorMessageTextAlertOnly.text = "Err Message : " + DisconnectCauses;
            }
            else if(DisconnectCauses == "Create Room Failed")
            {
                ConnLostAlertWithReconnButton.SetActive(true);
                WarningMessageTextAlertOnly.text = "Gagal membuat room!\n Silahkan periksa koneksi anda.";
                ErrorMessageTextAlertOnly.text = "Err Message : " + DisconnectCauses;
            }
            else if (DisconnectCauses == "Join Room Failed")
            {
                ConnLostAlertOnly.SetActive(true);
                WarningMessageTextAlertOnly.text = "Gagal masuk kedalam room!\n Silahkan periksa koneksi anda.";
                ErrorMessageTextAlertOnly.text = "Err Message : " + DisconnectCauses;
            }
            else if (DisconnectCauses == "Room Not Found")
            {
                ConnLostAlertOnly.SetActive(true);
                WarningMessageTextAlertOnly.text = DisconnectCauses;
                ErrorMessageTextAlertOnly.text = "Err Message : " + DisconnectMessages;
            }
            else
            {
                ConnLostAlertWithReconnButton.SetActive(true);
                ErrorMessageTextReconAlert.text = "Err Message : " + DisconnectCauses;
                Debug.Log("Connected status : " + PhotonNetwork.IsConnectedAndReady);
            }
        }
        else if (DisconnectedFromScene == "RoomLobby2Players" || DisconnectedFromScene == "RoomLobby3Players" || DisconnectedFromScene == "RoomLobby4Players")
        {
            if (DisconnectCauses == "Failed Get Data In Room")
            {
                ConnLostAlertWithReconnButton.SetActive(true);
                WarningMessageTextAlertOnly.text = "Periksa Koneksi Jaringan Anda!\n Coba Lagi ?";
                ErrorMessageTextReconAlert.text = "Err Message : " + DisconnectCauses;
                Debug.Log("Connected status : " + PhotonNetwork.IsConnectedAndReady);
            }
            else
            {
                ConnLostAlertOnly.SetActive(true);

                if (DisconnectCauses == "Masterclient Has Disconnected")
                {
                    WarningMessageTextAlertOnly.text = "Masterclient keluar dari room\n Permainan dihentikan!";
                }
                else if (DisconnectCauses == "Other player leave the room")
                {
                    WarningMessageTextAlertOnly.text = "Pemain lain keluar room seletelah rool\n Permainan dihentikan!";
                }
                else if (DisconnectCauses == "Other player force exited the app")
                {
                    WarningMessageTextAlertOnly.text = "Pemain lain keluar dari aplikasi\n Permainan dihentikan!";
                }

                ErrorMessageTextAlertOnly.text = "Err Message : " + DisconnectCauses;
                Debug.Log("Connected status : " + PhotonNetwork.IsConnectedAndReady);
            } 
        }
        else
        {
            ConnLostAlertOnly.SetActive(true);

            if (DisconnectCauses == "Other player disconnected")
            {
                WarningMessageTextAlertOnly.text = "Jaringan pemain lain terputus\n Permainan dihentikan!";
            }
            else if (DisconnectCauses == "Other player leave the room")
            {
                WarningMessageTextAlertOnly.text = "Pemain lain keluar dari room\n Permainan dihentikan!";
            }
            else if (DisconnectCauses == "Connection lost" && DisconnectedFromScene == "Gameplay")
            {
                WarningMessageTextAlertOnly.text = "Terjadi Kesalahan Jaringan\n Permainan dihentikan!";
            }

            ErrorMessageTextAlertOnly.text = "Err Message : " + DisconnectCauses;
            Debug.Log("Connected status : " + PhotonNetwork.IsConnectedAndReady);
        }
    }

    public void OKButton()
    {
        Rooms.expectedMaxPlayer = 0;
        Rooms.isRoomFull = false;
        Gameover.WinnerNickname = null;
        Constant.FirstTurnPlayerNickname = "Player 1";
        Constant.SecondTurnPlayerNickname = "Player 2";
        Constant.ThirdTurnPlayerNickname = "Player 3";
        Constant.FourthTurnPlayerNickname = "Player 4";

        if (DisconnectCauses == "Create or Join Room Failed")
        {
            SceneManager.LoadScene("Mainmenu");
        }
        else if (DisconnectCauses == "Room Not Found")
        {
            SceneManager.LoadScene("PartyMode");
        }
        else if (DisconnectCauses == "Masterclient Has Disconnected" || DisconnectCauses == "Other player force exited the app")
        {
            StartCoroutine(leaveRoom());
        }
        else if (PhotonNetwork.IsConnectedAndReady)
        {
            StartCoroutine(leaveRoom());
        }
        else
        {
            SceneManager.LoadScene("Mainmenu");
            StartCoroutine(reconnectToPhotonServer());
        }
    }

    IEnumerator leaveRoom() {
        if (DisconnectCauses == "Failed Get Data In Room")
        {
            PhotonNetwork.LeaveRoom();

            SceneManager.LoadScene("PartyMode");
        }
        else
        {
            PhotonNetwork.LeaveRoom();
            if (PhotonNetwork.CurrentRoom.Name != null)
                yield return null;
            SceneManager.LoadScene("Mainmenu");
        }
    }

    public void ReconnectButton()
    {
        if (DisconnectCauses == "Failed Get Data In Room")
        {
            //Debug.Log("Code room : " + roomCode);
            PartyMode.partyCode = roomCode;
            PartyMode.isReconRoom = true;

            StartCoroutine(leaveRoom());
        }
        else
        {
            ConnLostAlertWithReconnButton.SetActive(false);
            StartCoroutine(reconnectToPhotonServer());
            if (DisconnectedFromScene != "Login")
            {
                SceneManager.LoadScene("Mainmenu");
            }
        }
    }

    public void ExitButton()
    {
        if (DisconnectCauses == "Failed Get Data In Room")
        {
            SceneManager.LoadScene("Mainmenu");
        }
        else 
        {
            Application.Quit();
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene(DisconnectedFromScene);
    }

    IEnumerator reconnectToPhotonServer() {
        PhotonNetwork.Reconnect();
        if(!PhotonNetwork.IsConnectedAndReady)
            yield return null;
        if(ConnLostAlertOnly.activeSelf == false)
        {
            ConnLostAlertWithReconnButton.SetActive(false);
        } 
        else
        {
            ConnLostAlertOnly.SetActive(false);
        }
        ReconnectAlert.SetActive(true);
        ReconnectResultText.text = "Sukses terhubung kembali!";
    }
}
