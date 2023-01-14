using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Card_Level3 : MonoBehaviourPunCallbacks
{
    CardManager cardManager;
    GameplayManager gameplayManager;

    // Start is called before the first frame update
    void Start()
    {
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();

        cardManager.SetCardSpriteAndInteractable("3", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(PointBar.PlayerPointValue >= 13) {
            cardManager.SetCardSpriteAndInteractable("3", true);
        } else {
            cardManager.SetCardSpriteAndInteractable("3", false);
        }
    }
}
