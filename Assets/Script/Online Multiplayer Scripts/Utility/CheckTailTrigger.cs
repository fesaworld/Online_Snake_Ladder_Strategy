using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class CheckTailTrigger : MonoBehaviourPun
{
    public static bool TailChecked = false;
    public static bool TriggerByTail = false;
    public static string AttachedItemName;
    public static string AlertMessage;
    GameplayManager gameplayManager;

    void Start() {
        gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();
    }

    private void OnTriggerEnter2D(Collider2D objectCollidedByItemTail)
    {
        string objectHitByItem = objectCollidedByItemTail.transform.tag;
        if(CardManager.ClickMousePlaceItem == true)
        {
            switch(objectHitByItem)
            {
                case "Barrier":
                    //SoundManager.PlaySoundEffect("WrongPlace");
                    AlertMessage = "ujung item keluar papan permainan!";
                    displayTailCollideObjectAlert(AlertMessage, 2f);
                    break;
                case "kepala":
                    //SoundManager.PlaySoundEffect("WrongPlace");
                    AlertMessage = "ujung item menyentuh objek lain!";
                    displayTailCollideObjectAlert(AlertMessage, 2f);
                    break;
                case "ekor":
                    //SoundManager.PlaySoundEffect("WrongPlace");
                    AlertMessage = "ujung item menyentuh objek lain!";
                    displayTailCollideObjectAlert(AlertMessage, 2f);
                    break;
                case "Player":
                    //SoundManager.PlaySoundEffect("WrongPlace");
                    AlertMessage = "ujung item menyentuh pion pemain!";
                    displayTailCollideObjectAlert(AlertMessage, 2f);
                    break;
                case "Petak-100":
                    //SoundManager.PlaySoundEffect("WrongPlace");
                    AlertMessage = "ujung item menyentuh petak 100!";
                    displayTailCollideObjectAlert(AlertMessage, 2f);
                    break;
                default:
                    break;
            }
        }
        TailChecked = true;
    }

    void displayTailCollideObjectAlert(string message, float duration)
    {
        StartCoroutine(gameplayManager.DisplayNotificationInfo(message, duration, false));
        AttachedItemName = this.gameObject.transform.parent.gameObject.name;
        TriggerByTail = true;
    }
}