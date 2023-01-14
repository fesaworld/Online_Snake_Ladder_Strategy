using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class OldLobby : MonoBehaviourPunCallbacks
{
    public GameObject player1SpawnLocation, player2SpawnLocation, player3SpawnLocation, player4SpawnLocation, rollBtn, disableRollBtn, playBtn, disablePlayBtn;

    public static int giliranP1, giliranP2, giliranP3, giliranP4;

    public Text TextGiliran1, TextGiliran2, TextGiliran3, TextGiliran4;

    GameObject player1, player2, player3, player4;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Instantiating Player 1");
            player1 = PhotonNetwork.Instantiate("Player/PlayerPrefab", player1SpawnLocation.transform.position, player1SpawnLocation.transform.rotation, 0);


            //Disini masih ada bug
            if(giliranP1 > 0) {
                rollBtn.SetActive(false);
                disableRollBtn.SetActive(true);
                playBtn.SetActive(true);
                disablePlayBtn.SetActive(false);
            } else if(giliranP1 == 0) {
                rollBtn.SetActive(true);
                disableRollBtn.SetActive(false);
                playBtn.SetActive(false);
                disablePlayBtn.SetActive(true);
            }
        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount.Equals(2))
        {
            Debug.Log("Instantiating Player 2");
            player2 = PhotonNetwork.Instantiate("Player/PlayerPrefab", player2SpawnLocation.transform.position, player2SpawnLocation.transform.rotation, 0);
        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount.Equals(3))
        {
            Debug.Log("Instantiating Player 3");
            player3 = PhotonNetwork.Instantiate("Player/PlayerPrefab", player3SpawnLocation.transform.position, player3SpawnLocation.transform.rotation, 0);
        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount.Equals(4))
        {
            Debug.Log("Instantiating Player 4");
            player4 = PhotonNetwork.Instantiate("Player/PlayerPrefab", player4SpawnLocation.transform.position, player4SpawnLocation.transform.rotation, 0);
        }
    }

    List<int> turnNumberAvailable = new List<int>(new int[]{1,2,3,4});
    public void Roll_Giliran() {
        giliranP1 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(giliranP1);

        giliranP2 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(giliranP2);

        giliranP3 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(giliranP3);

        giliranP4 = turnNumberAvailable[Random.Range(0, turnNumberAvailable.Count)];
        turnNumberAvailable.Remove(giliranP4);

        TextGiliran1.text = "Giliran : " + giliranP1.ToString();
        TextGiliran2.text = "Giliran : " + giliranP2.ToString();
        TextGiliran3.text = "Giliran : " + giliranP3.ToString();
        TextGiliran4.text = "Giliran : " + giliranP4.ToString();

        // rollBtn.SetActive(false);
        // disableRollBtn.SetActive(true);
        // playBtn.SetActive(true);
        // disablePlayBtn.SetActive(false);
    }

    public void Mulai() {
        SceneManager.LoadScene("NewGame");
    }
}
