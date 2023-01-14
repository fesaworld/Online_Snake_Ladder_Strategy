using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class GameMode : MonoBehaviourPunCallbacks
{
    SceneLoader sceneLoader;

    void Start() {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    public void OnClick_PartModeButton() {
        sceneLoader.LoadScene("PartyMode", SoundManager.sfxLength);
    }

    public void OnClick_RandomModeButton() {
        sceneLoader.LoadScene("SelectMode", SoundManager.sfxLength);
    }

    public void BackButton() 
    {
        sceneLoader.LoadScene("Mainmenu", SoundManager.sfxLength);
    }

    IEnumerator connectedStatusMainMenu() {
        yield return new WaitForSeconds(5f);
        Debug.Log("Network connected status : " + PhotonNetwork.IsConnectedAndReady);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = cause.ToString();
        SceneManager.LoadScene("ConnLostAlert");
    }
}
