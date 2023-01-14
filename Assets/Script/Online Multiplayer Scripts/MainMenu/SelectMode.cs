using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class SelectMode : MonoBehaviourPunCallbacks
{
    SceneLoader sceneLoader;

    void Start() {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    public void BackButton() 
    {
        //SoundManager.PlaySoundEffect("ButtonClick");
        sceneLoader.LoadScene("GameMode", SoundManager.sfxLength);
    }
    
    public void SelectModeButtons()
    {
        //SoundManager.PlaySoundEffect("ButtonClick");
        string buttonName = EventSystem.current.currentSelectedGameObject.name.ToString();
        switch(buttonName)
        {
            case "2PlayerButton":
                Rooms.expectedMaxPlayer = 2;
                break;
            case "3PlayerButton":
                Rooms.expectedMaxPlayer = 3;
                break;
            case "4PlayerButton":
                Rooms.expectedMaxPlayer = 4;
                break;
            default:
                Rooms.expectedMaxPlayer = 0;
                break;
        }

        joinRoom();
    }

    void joinRoom()
    {
        sceneLoader.NowLoading();
        PhotonNetwork.JoinRandomRoom(null, Convert.ToByte(Rooms.expectedMaxPlayer));
    }

    void createRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CleanupCacheOnLeave = false;
        roomOptions.MaxPlayers = (byte)Rooms.expectedMaxPlayer;
        
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom() {
        switch(Rooms.expectedMaxPlayer)
        {
            case 2:
                sceneLoader.LoadScene("RoomLobby2Players");
                break;
            case 3:
                sceneLoader.LoadScene("RoomLobby3Players");
                break;
            case 4:
                sceneLoader.LoadScene("RoomLobby4Players");
                break;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        createRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        sceneLoader.LoadingOver();
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = "Create or Join Room Failed";
        SceneManager.LoadScene("ConnLostAlert");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = cause.ToString();
        SceneManager.LoadScene("ConnLostAlert");
    }
}
