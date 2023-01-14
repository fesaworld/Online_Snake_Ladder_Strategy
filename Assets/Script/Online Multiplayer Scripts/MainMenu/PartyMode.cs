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

public class PartyMode : MonoBehaviourPunCallbacks 
{
    SceneLoader sceneLoader;

    public InputField partyCodeField;
    public static string partyCode;
    public static bool isReconRoom = false;
    public static bool onMaster = false;

    void Start() {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        if (isReconRoom == true)
        {
            sceneLoader.NowLoading();
        }
    }

    public void OnClick_CreatePartyButton() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        sceneLoader.LoadScene("CreatePartyMode", SoundManager.sfxLength);
    }

    public void OnClick_JoinButton()
    {
        partyCode = partyCodeField.text;

        if (partyCode != "")
        {
            joinRoom(partyCode);
            sceneLoader.NowLoading();
        }
        else{
            return;
        }
    }

    public void BackButton() 
    {
        //SoundManager.PlaySoundEffect("ButtonClick");
        sceneLoader.LoadScene("GameMode", SoundManager.sfxLength);
    }

    public void joinRoom(string roomCode)
    {
        PhotonNetwork.JoinRoom(roomCode);
        //Debug.Log("join room name : " + roomCode);
        
    }

    public override void OnJoinedRoom()
    {
        //sceneLoader.LoadingOver();
        int infoRoom = PhotonNetwork.CurrentRoom.MaxPlayers;
        //Debug.Log("Jumlah Player : " + infoRoom);
        switch (infoRoom)
        {
            case 2:
                Rooms.expectedMaxPlayer = 2;
                sceneLoader.LoadScene("RoomLobby2Players");
                break;
            case 3:
                Rooms.expectedMaxPlayer = 3;
                sceneLoader.LoadScene("RoomLobby3Players");
                break;
            case 4:
                Rooms.expectedMaxPlayer = 4;
                sceneLoader.LoadScene("RoomLobby4Players");
                break;
        }
        //Debug.Log("join room successfull");
        //isReconRoom = false;
    }

    public override void OnConnectedToMaster()
    {
        onMaster = true;
        if (isReconRoom == true && onMaster == true)
        {
            joinRoom(partyCode);
            isReconRoom = false;
            onMaster = false;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        sceneLoader.LoadingOver();
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = "Room Not Found";
        ConnectionLostAlert.DisconnectMessages = message;
        SceneManager.LoadScene("ConnLostAlert");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = cause.ToString();
        SceneManager.LoadScene("ConnLostAlert");
    }
}
