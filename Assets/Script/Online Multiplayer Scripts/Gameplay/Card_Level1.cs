using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Card_Level1 : MonoBehaviourPunCallbacks
{
    CardManager cardManager;
    GameplayManager gameplayManager;

    // Start is called before the first frame update
    void Start()
    {
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();

        cardManager.SetCardSpriteAndInteractable("1", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(PointBar.PlayerPointValue >= 7) {
            cardManager.SetCardSpriteAndInteractable("1", true);
        } else {
            cardManager.SetCardSpriteAndInteractable("1", false);
        }
    }
}
