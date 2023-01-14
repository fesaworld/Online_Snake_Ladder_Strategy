using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;

    public void NowLoading() {
        LoadingScreen.SetActive(true);
    }

    public void LoadingOver() {
        LoadingScreen.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(loadAsyncWithoutSfx(sceneName));
    }
    
    public void LoadScene(string sceneName, float sfxLength)
    {
        StartCoroutine(loadAsyncWithSfx(sceneName, sfxLength));
    }

    IEnumerator loadAsyncWithoutSfx(string sceneName)
    {
        NowLoading();
        PhotonNetwork.LoadLevel(sceneName);

        if(PhotonNetwork.LevelLoadingProgress == 0.9f) 
        {
            LoadingOver();
        }

        yield return null;
    }

    IEnumerator loadAsyncWithSfx(string sceneName, float sfxLength)
    {
        yield return new WaitForSeconds(sfxLength);
        NowLoading();
        PhotonNetwork.LoadLevel(sceneName);
        if(PhotonNetwork.LevelLoadingProgress == 0.9f)
        {
            LoadingOver();
        }
    }
}
