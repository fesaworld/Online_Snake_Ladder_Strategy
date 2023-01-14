using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class Item : MonoBehaviourPunCallbacks
{
    public string Owner;
    public int Age;
    public int BoardPosition;
    public Vector3 DefaultPosition;
    public Card.CardType Type;
    public Transform Destination;
    public GameObject ItemObject;

    public void Spawn(Vector2 position)
    {
        this.ItemObject.transform.position = position;
        this.Destination = this.ItemObject.transform.Find("ekor");
    }

    public void AgeUpdate() {
        if(this.Age < 3)
        {
            this.Age += 1;
        }
        
        // if(this.Age == 3) {
        //     if(Type == Card.CardType.Snake || Type == Card.CardType.Snake2Head)
        //         SoundManager.PlaySoundEffect("SnakeDied");
        //     else
        //         SoundManager.PlaySoundEffect("LadderDestroyed");

        //     if(this.Owner == PlayerScript.PlayerNickname)
        //         PhotonNetwork.Destroy(this.ItemObject);
        // }
    }
}
