using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Login : MonoBehaviourPunCallbacks
{
    bool isConnecting = true;
    string gameVersion = "1";
    
    [Space(10)]
    [Header("Custom Variables")]
    public InputField playerNameField;

    public Text connectionStatus;

    public GameObject loginButton;

    SceneLoader sceneLoader;

    //Untuk memastikan bahwa scene yang diload semua client sama
    void Awake()
    {
      // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
      PhotonNetwork.AutomaticallySyncScene = false;
    }

    //Method start digunakan untuk mengeksekusi method ConnectToPhoton untuk menghubungkan ke server photon
    void Start()
    {
      sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

      if(!Constant.isGameStarted) {
        sceneLoader.NowLoading();
        Constant.isGameStarted = true;
        PlayerPrefs.DeleteAll();
        ConnectToPhoton();
      } else {
        if(PhotonNetwork.IsConnected) {
          return;
        } else {
          PlayerPrefs.DeleteAll();
          ConnectToPhoton();
        }
      }
    }
    
    //Method ini digunakan untuk menghubungkan ke server photon
    void ConnectToPhoton() {
      connectionStatus.text = "Menghubungkan...";
      PhotonNetwork.GameVersion = gameVersion;
      PhotonNetwork.ConnectUsingSettings();
    }

    //Method ini akan dieksekusi ketika sudah terhubung dengan server
    public override void OnConnected() {
      sceneLoader.LoadingOver();
      isConnecting = false;
    }

    //Method ini akan dieksekusi ketika gagal menghubungkan ke server photon
    public override void OnDisconnected(DisconnectCause cause) {
      sceneLoader.LoadingOver();
      isConnecting = false;

      ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
      ConnectionLostAlert.DisconnectCauses = cause.ToString();
      SceneManager.LoadScene("ConnLostAlert");
    }

    // Method ini digunakan untuk menyimpan nama pemain ke server dan menampilkan menu halaman utama
    public void LoginButton() {
      //SoundManager.PlaySoundEffect("ButtonClick");
      if(playerNameField.text != "") {
        PhotonNetwork.NickName = playerNameField.text;
        sceneLoader.LoadScene("Mainmenu", SoundManager.sfxLength);
      } else {
        return;
      }
    }

    public void TestDisconnectButton() {
      PhotonNetwork.Disconnect();
    }
}
