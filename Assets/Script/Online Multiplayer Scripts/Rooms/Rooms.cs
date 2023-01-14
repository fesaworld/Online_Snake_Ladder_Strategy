using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;
using System.Threading.Tasks;

public class Rooms : MonoBehaviourPunCallbacks
{
    public GameObject player1SpawnLocation, player2SpawnLocation, player3SpawnLocation, player4SpawnLocation;
    public Button RollButton, PlayButton, LeaveRoomButton;
    public static bool isRoomFull = false;
    public static int expectedMaxPlayer = 0;
    public static int timeStatus = 0;
    public Text RandomCodeText, PlayerHostText;

    //make me
    public static bool players1 = false;
    public static bool players2 = false;
    public static bool players3 = false;
    public static bool players4 = false;
    public static bool isSpawnOn = false;
    public static bool isPlayerOut = false;
    public static bool isMasterClientOut = false;
    public static bool isRandomPlayerOut = false;
    public static bool getRPC = false;

    public static string playerHost;

    public static int hitungPlayer = 0;
    public static int hitungPlayerTotal = 0;

    GameObject player1Avatar, player2Avatar, player3Avatar, player4Avatar;
    SceneLoader sceneLoader;

    void Start()
    {
        players1 = false;
        players2 = false;
        players3 = false;
        players4 = false;
        isSpawnOn = true;
        isPlayerOut = false;
        isMasterClientOut = false;
        isRandomPlayerOut = false;
        getRPC = false;

        hitungPlayer = 0;
        hitungPlayerTotal = 0;

        RollPhase.isRollPhaseDone = false;
        PlayerScript.PlayerNumber = 0;

        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

        CheckStatus();
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            onPlayerDisconnected("Connection lost", "");
        }
        //else if (PhotonNetwork.MasterClient.ActorNumber != 1)
        //{
        //    onPlayerDisconnected("Masterclient Has Disconnected", "");
        //}
        else if (PhotonNetwork.CurrentRoom.PlayerCount != expectedMaxPlayer && RollPhase.isRollPhaseDone)
        {
            onPlayerDisconnected("Other player leave the room", "");
        }

        checkEnterPlayerInRoomLobby();

        if (PlayerScript.PlayerNumber == 0 && isSpawnOn == true)
        {
            if (!PhotonNetwork.IsMasterClient && players1 == false && players2 == false && players3 == false && players4 == false)
            {
                //Debug.LogError("getRPC = " + getRPC);
                if (!PhotonNetwork.IsMasterClient &&  getRPC == true)
                {
                    //Debug.LogError("masuk kondisi else 1");
                    for (int i = 1; i <= PhotonNetwork.CurrentRoom.PlayerCount; i++)
                    {
                        GameObject.Find("Player" + i + "Avatar(Clone)").SetActive(false);
                    }

                    onPlayerDisconnected("Failed Get Data In Room", PhotonNetwork.CurrentRoom.Name);
                }
                //else
                //{
                //    //Debug.LogError("mulai loading");
                //    //sceneLoader.NowLoading();
                //}
            }
                else
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
                {
                    PhotonNetwork.DestroyAll();
                    PhotonNetwork.DestroyAll(true);
                }

                //Debug.LogError("Masuk Spawn");
                spawnPlayerInRoomLobby();
            }
        }

        //make me
        if (PhotonNetwork.IsMasterClient)
        {
            RollButton.gameObject.SetActive(true);
            RollButton.interactable = false;
            PlayButton.gameObject.SetActive(true);
            PlayButton.interactable = false;

            if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                LeaveRoomButton.interactable = false;
            }
            else
            {
                LeaveRoomButton.interactable = true;
            }
        }
        else
        {
            if (RollPhase.isRollPhaseDone == true)
            {
                LeaveRoomButton.interactable = false;
            }
            else
            {
                LeaveRoomButton.interactable = true;
            }
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == expectedMaxPlayer)
        {
            isRoomFull = true;
        }
        else
        {
            isRoomFull = false;
        }

        allPlayersInRoom(isRoomFull);
        activatePlayButton();
    }

    [PunRPC]
    void SyncronizePlayerData(bool _players1, bool _players2, bool _players3, bool _players4,
        bool _isPlayerOut, string _playerHost, PhotonMessageInfo info)
    {
        getRPC = true;
        //Debug.LogError("selesai loading");
        sceneLoader.LoadingOver();

        players1 = _players1;
        players2 = _players2;
        players3 = _players3;
        players4 = _players4;
        isPlayerOut = _isPlayerOut;
        playerHost = _playerHost;

        //Debug.LogError("isi di sync : player1 : " + players1 + ", player2 : " + players2 + ", player3 : " + players3 + ", player4 : " + players4 + ", Player Out : " + isPlayerOut + ", Master Out : " + isMasterClientOut + ", Random Out : " + isRandomPlayerOut);
        Debug.Log("Penerima ( " + PhotonNetwork.LocalPlayer.NickName + " )" + ", Pengirim: " + info.Sender + ", " + info.photonView + ", " + info.SentServerTime);
        Debug.Log("isi RPC : player1 : " + players1 + ", player2 : " + players2 + ", player3 : " + players3 + ", player4 : " + players4 + ", Player Out : " + isPlayerOut + ", playerHost : " + playerHost);

        if (playerHost == PhotonNetwork.LocalPlayer.NickName)
        {
            PlayerHostText.text = "Yours " + playerHost;
        }
        else
        {
            PlayerHostText.text = playerHost;
        }
    }

    // Make me
    void CheckStatus()
    {
        if (PhotonNetwork.CurrentRoom.IsVisible != true)
        {
            string receivedRandom = PhotonNetwork.CurrentRoom.Name;
            RandomCodeText.text = receivedRandom;
            //Debug.LogError("Received Random Code : " + receivedRandom);
        }
        else
        {
            RandomCodeText.text = "";
            RandomCodeText.enabled = false;
        }        
    }

    //make me
    void checkEnterPlayerInRoomLobby()
    {
        if (hitungPlayer == 0)
        {
            hitungPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
            hitungPlayerTotal++;
        }
        else
        {
            if (hitungPlayer < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                //Debug.LogError("Ada Pemain Masuk");
                hitungPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
                hitungPlayerTotal++;
            }
            else if (hitungPlayer > PhotonNetwork.CurrentRoom.PlayerCount)
            {
                //Debug.LogError("hitung player : " + hitungPlayer);
                //Debug.LogError("player count : " + PhotonNetwork.CurrentRoom.PlayerCount);
                //Debug.LogError("Ada Pemain Keluar");
                hitungPlayer = PhotonNetwork.CurrentRoom.PlayerCount;

                //cek lagi fungsi ini, klo ga dipake ifnya dihapus ajh
                if (PhotonNetwork.IsMasterClient == true && isPlayerOut == true)
                {
                    //Debug.LogError("kondisi player out");
                    isPlayerOut = false;
                    SendData();
                }
            }
        }
    }

    void spawnPlayerInRoomLobby()
    {
        //Debug.LogError("Jumlah pemain sebelum masuk room : " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (players1 == false)
        {
            player1Avatar = PhotonNetwork.Instantiate("Player/Player1Avatar", player1SpawnLocation.transform.position, player1SpawnLocation.transform.rotation, 0);
            PlayerScript.PlayerNumber = 1;
            players1 = true;
            int numberPlayer1 = hitungPlayerTotal;

            //Debug.LogError("kondisi 1 spawn player : " + PlayerScript.PlayerNumber);

            isSpawnOn = false;
        }
        else if (players2 == false)
        {
            player2Avatar = PhotonNetwork.Instantiate("Player/Player2Avatar", player2SpawnLocation.transform.position, player2SpawnLocation.transform.rotation, 0);
            PlayerScript.PlayerNumber = 2;
            players2 = true;
            int numberPlayer2 = hitungPlayerTotal;

            //Debug.LogError("kondisi 2 spawn player : " + PlayerScript.PlayerNumber);

            isSpawnOn = false;
        }
        else if (players3 == false)
        {
            player3Avatar = PhotonNetwork.Instantiate("Player/Player3Avatar", player3SpawnLocation.transform.position, player3SpawnLocation.transform.rotation, 0);
            PlayerScript.PlayerNumber = 3;
            players3 = true;
            int numberPlayer3 = hitungPlayerTotal;

            //Debug.LogError("kondisi 3 spawn player : " + PlayerScript.PlayerNumber);

            isSpawnOn = false;
        }
        else if (players4 == false)
        {
            player4Avatar = PhotonNetwork.Instantiate("Player/Player4Avatar", player4SpawnLocation.transform.position, player4SpawnLocation.transform.rotation, 0);
            PlayerScript.PlayerNumber = 4;
            players4 = true;
            int numberPlayer4 = hitungPlayerTotal;

            //Debug.LogError("kondisi 4 spawn player : " + PlayerScript.PlayerNumber);

            isSpawnOn = false;
        }
        else {
            //Debug.LogError("masuk kondisi else 2");
            onPlayerDisconnected("Failed Get Data In Room", PhotonNetwork.CurrentRoom.Name);

        }
       
        SendData();
    }

    void SendData()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerHost = PhotonNetwork.LocalPlayer.NickName;
        }

        photonView.RPC("SyncronizePlayerData", RpcTarget.AllBufferedViaServer, players1, players2, players3, players4, isPlayerOut, playerHost);
        //Debug.LogError("Sukses terkirim");
        Debug.Log("Terkirim dari ( " + PhotonNetwork.LocalPlayer.NickName + " )" + ", " + "player1 : " + players1 + ", player2 : " + players2 + ", player3 : " + players3 + ", player4 : " + players4 + ", Player Out : " + isPlayerOut + ", PlayerHost : " + playerHost);
    }

    void SendDataOut()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerHost = PhotonNetwork.LocalPlayer.NickName;
        }

        photonView.RPC("SyncronizePlayerData", RpcTarget.AllBufferedViaServer, players1, players2, players3, players4, isPlayerOut, playerHost);
        //Debug.LogError("Sukses terkirim");
        //Debug.LogError("Terkirim dari ( " + PlayerScript.PlayerNumber + " )" + ", " + "player1 : " + players1 + ", player2 : " + players2 + ", player3 : " + players3 + ", player4 : " + players4 + ", Player Out : " + isPlayerOut + ", Master Out : " + isMasterClientOut + ", Random Out : " + isRandomPlayerOut);

        PhotonNetwork.SendAllOutgoingCommands();
    }

    void CheckPlayers()
    {
        //Debug.LogError("Cek IsMasterClient : " + PhotonNetwork.IsMasterClient);
        //Debug.LogError("Cek MasterClient UserId: " + PhotonNetwork.MasterClient.UserId);
        ////Debug.LogError("Cek MasterClient ActorNumber: " + PhotonNetwork.MasterClient.ActorNumber);
        //Debug.LogError("Cek MasterClient NickName: " + PhotonNetwork.MasterClient.NickName);
        ////Debug.LogError("Cek CountOfPlayersOnMaster : " + PhotonNetwork.CountOfPlayersOnMaster);


        ////Debug.LogError("PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount);
        ////Debug.LogError("expectedMaxPlayer : " + expectedMaxPlayer);
        ////Debug.LogError("isRollDone : " + RollPhase.isRollPhaseDone);

        //Debug.LogError("isspawn : " + isSpawnOn);
        //Debug.LogError("is room full : " + isRoomFull);
        //Debug.LogError("jumlah max player : " + expectedMaxPlayer);
        //Debug.LogError("jumlah pemain di room dari cloud: " + PhotonNetwork.CurrentRoom.PlayerCount);
        //Debug.LogError("isRandomPlayerOut : " + isRandomPlayerOut);

        //Debug.LogError("isPlayerOut : " + isPlayerOut);

        Debug.LogError("Players 1 : " + players1);
        Debug.LogError("Players 2 : " + players2);
        Debug.LogError("Players 3 : " + players3);
        Debug.LogError("Players 4 : " + players4);
    }

    public void OnClick_PlayButton()
    {
        StartGame();
    }

    public void OnClick_PlayerButton()
    {
        CheckPlayers();
    }


    public void OnClick_LeaveRoomButton()
    {
        //Debug.LogError("cek player number : " + PlayerScript.PlayerNumber);
        switch (PlayerScript.PlayerNumber)
        {
            case 1:
                //Debug.LogError("Player 1 Keluar");
                players1 = false;
                break;
            case 2:
                //Debug.LogError("Player 2 Keluar");
                players2 = false;
                break;
            case 3:
                //Debug.LogError("Player 3 Keluar");
                players3 = false;
                break;
            case 4:
                //Debug.LogError("Player 4 Keluar");
                players4 = false;
                break;
        }

        if (PhotonNetwork.CurrentRoom.IsVisible == true)
        {
            switch (PlayerScript.PlayerNumber)
            {
                case 1:
                    PhotonNetwork.Destroy(player1Avatar);
                    break;
                case 2:
                    PhotonNetwork.Destroy(player2Avatar);
                    break;
                case 3:
                    PhotonNetwork.Destroy(player3Avatar);
                    break;
                case 4:
                    PhotonNetwork.Destroy(player4Avatar);
                    break;
            }
            ////isRandomPlayerOut = true;
        }
        isPlayerOut = true;
        SendDataOut();

        // StartCoroutine(leaveRoom();
        leaveRoom();
    }

    void leaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        sceneLoader.LoadScene("Mainmenu", SoundManager.sfxLength);
    }

    public void StartGame()
    {
        string sceneName = "Gameplay";
        object[] sceneNameData = new object[] { sceneName };
        PhotonNetwork.RaiseEvent(RaiseEventCode.ENTER_THE_GAME, sceneNameData, RaiseEventOptions.Default, SendOptions.SendReliable);

        RollPhase.isRollPhaseDone = false;
        sceneLoader.LoadScene("Gameplay", SoundManager.sfxLength);

        PhotonPeer.RegisterType(typeof(Card), (byte)'C', CardDataSerialization.SerializeCard, CardDataSerialization.DeserializeCard);
        PhotonPeer.RegisterType(typeof(Item), (byte)'I', ItemDataSerialization.SerializeItem, ItemDataSerialization.DeserializeItem);

        //StartCoroutine(sendDelayStart());
    }

    //IEnumerator sendDelayStart()
    //{
    //    yield return new WaitForSeconds(5f);
    //    Debug.LogError("Mengirim Start Ke pemain Lain");
    //    
    //}

    void activatePlayButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (RollPhase.isRollPhaseDone == true)
            {   
                RollButton.interactable = false;
                PlayButton.interactable = true; 
            }
            else
            {   
                PlayButton.interactable = false; 
            }
        }
    }

    IEnumerator activateLeaveRoomButton()
    {
        yield return new WaitForSeconds(10f);
        LeaveRoomButton.interactable = true;
    }

    void allPlayersInRoom(bool _isRoomFull)
    {
        if (_isRoomFull)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                RollButton.interactable = true;
            }
        }
    }

    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == RaiseEventCode.ALL_PLAYERS_IN_ROOM)
        {
            object[] playerStatusData = (object[])obj.CustomData;
            bool isRoomFull = (bool)playerStatusData[0];

            allPlayersInRoom(isRoomFull);
        }
        else if (obj.Code == RaiseEventCode.ROLL_PHASE_DONE)
        {
            object[] rollStatusData = (object[])obj.CustomData;
            RollPhase.isRollPhaseDone = (bool)rollStatusData[0];

            //Debug.LogError("isi roll phase : " + (bool)rollStatusData[0]);
        }
        else if (obj.Code == RaiseEventCode.ENTER_THE_GAME)
        {
            StartGame();
        }
    }

    void onPlayerDisconnected(string message, string roomCode)
    {
        sceneLoader.LoadingOver();
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = message;
        ConnectionLostAlert.roomCode = roomCode;
        SceneManager.LoadScene("ConnLostAlert");
    }

    private void OnApplicationQuit()
    {
        //Debug.LogError("cek player number : " + PlayerScript.PlayerNumber);
        switch (PlayerScript.PlayerNumber)
        {
            case 1:
                //Debug.LogError("Player 1 Keluar");
                players1 = false;
                break;
            case 2:
                //Debug.LogError("Player 2 Keluar");
                players2 = false;
                break;
            case 3:
                //Debug.LogError("Player 3 Keluar");
                players3 = false;
                break;
            case 4:
                //Debug.LogError("Player 4 Keluar");
                players4 = false;
                break;
        }

        if (PhotonNetwork.CurrentRoom.IsVisible == true)
        {
            switch (PlayerScript.PlayerNumber)
            {
                case 1:
                    PhotonNetwork.Destroy(player1Avatar);
                    break;
                case 2:
                    PhotonNetwork.Destroy(player2Avatar);
                    break;
                case 3:
                    PhotonNetwork.Destroy(player3Avatar);
                    break;
                case 4:
                    PhotonNetwork.Destroy(player4Avatar);
                    break;
            }
            ////isRandomPlayerOut = true;
        }

        isPlayerOut = true;
        SendDataOut();
    }
}
