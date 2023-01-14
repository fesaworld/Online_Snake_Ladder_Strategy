using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class RoomNameBtn : MonoBehaviour
{
    public Text RoomName;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => JoinRoomByName(RoomName.text));
    }

    public void JoinRoomByName(string RoomName) {
        PhotonNetwork.JoinRoom(RoomName);
    }
}
