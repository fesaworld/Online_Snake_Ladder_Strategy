using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Tutorial : MonoBehaviourPunCallbacks
{
    public Image TutorialImages;
    SceneLoader sceneLoader;
    int imagesCount = 13;
    int currentImageIndex = 1;

    void Start() {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    void Update() {
        TutorialImages.sprite = Resources.Load<Sprite>(
            "New Images Asset/TutorialImage/Default/" + currentImageIndex
        );
    }

    public void BackButton() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        sceneLoader.LoadScene("Mainmenu", SoundManager.sfxLength);
        currentImageIndex = 1;
    }

    public void NextButton() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        if(currentImageIndex < 13) {
            currentImageIndex++;
        } else {
            currentImageIndex = 1;
        }
    }

    public void PreviousButton() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        if(currentImageIndex > 1) {
            currentImageIndex--;
        } else {
            currentImageIndex = 13;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = cause.ToString();
        SceneManager.LoadScene("ConnLostAlert");
    }
}
