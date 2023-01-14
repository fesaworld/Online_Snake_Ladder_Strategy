using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;
using UnityEngine.EventSystems;

public class CreatePartyMode : MonoBehaviourPunCallbacks
{
    SceneLoader sceneLoader;

    string randomcode;

    void Start() {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    //Methode Generate Random Code
    public void RandomCode()
    {
        System.Random generator = new System.Random();
        randomcode = generator.Next(0, 999999).ToString("D6");

        //randomcode = "123456";
    }

    public void BackButton()
    {
        //SoundManager.PlaySoundEffect("ButtonClick");
        sceneLoader.LoadScene("PartyMode", SoundManager.sfxLength);
    }

    public void SelectModeButtons()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name.ToString();
        switch (buttonName)
        {
            case "2PlayerPartyButton":
                Rooms.expectedMaxPlayer = 2;
                break;
            case "3PlayerPartyButton":
                Rooms.expectedMaxPlayer = 3;
                break;
            case "4PlayerPartyButton":
                Rooms.expectedMaxPlayer = 4;
                break;
        }
        RandomCode();
        CreateRoom(randomcode);
    }

    void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = (byte)Rooms.expectedMaxPlayer;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Created room successfully.. " , this);

        switch (Rooms.expectedMaxPlayer)
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

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        sceneLoader.LoadingOver();
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = "Create Room Failed";
        SceneManager.LoadScene("ConnLostAlert");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        sceneLoader.LoadingOver();
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = "Join Room Failed";
        SceneManager.LoadScene("ConnLostAlert");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = cause.ToString();
        SceneManager.LoadScene("ConnLostAlert");
    }
}
