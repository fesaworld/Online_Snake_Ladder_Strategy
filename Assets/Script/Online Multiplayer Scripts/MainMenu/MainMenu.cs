using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class MainMenu : MonoBehaviourPunCallbacks
{
    SceneLoader sceneLoader;

    // void Update() {
    //     StartCoroutine(connectedStatusMainMenu());
    // }

    void Start() {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    public void OnClick_PlayButton() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        //sceneLoader.LoadScene("SelectMode", SoundManager.sfxLength);
        sceneLoader.LoadScene("GameMode", SoundManager.sfxLength);
    }

    public void OnClick_InstructionButton() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        sceneLoader.LoadScene("Tutorial", SoundManager.sfxLength);
    }

    public void OnClick_ExitButton() {
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        //SoundManager.PlaySoundEffect("ButtonClick");
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
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
