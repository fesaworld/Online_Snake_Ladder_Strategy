using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class GameplayManager : MonoBehaviourPunCallbacks
{
    public GameObject Player1SpawnLocation, Player2SpawnLocation, PlayerPointbarPrefab, PlayerPointbarSpawnLocation;
    public GameObject BlackScreen, Notification_Panel;
    public Button EndTurnButton;
    public Text NotificationInfo_Text;
    public Text PlayerNicknameText;
    public List<Item> DestroyedItemList;
    public Transform[] waypoints;
    public bool IsMove = false;
    public bool IsBonusTurnNotifDisplayed;
    public bool PawnMoveExceedLastSquare = false;
    public bool PlayerHasReachLastSquare = false;
    public int WaypointsLeft = 100;
    public Item playerHitItem;
    public static bool StillMoveBecauseItem = false;

    GameObject player1Avatar, player2Avatar, player3Avatar, player4Avatar, playerPointbar;
    PlayerScript playerScript;
    NewDice newDice;
    PlayerTurnManager playerTurnManager;
    CardManager cardManager;
    ItemManager itemManager;
    int playerStartWaypoint = 0;
    bool isReadyMovebyItem = false;

    public static bool forUpdate = false;
    public static bool hostDone = false;
    public static bool otherPlayerReady = false;

    void Awake()
    {
        //PhotonPeer.RegisterType(typeof(Card), (byte)'C', CardDataSerialization.SerializeCard, CardDataSerialization.DeserializeCard);
        //PhotonPeer.RegisterType(typeof(Item), (byte)'I', ItemDataSerialization.SerializeItem, ItemDataSerialization.DeserializeItem);
    }

    // Start is called before the first frame update
    void Start()
    {
        forUpdate = false;
        hostDone = false;
        otherPlayerReady = false;

        //NotificationInfo_Text.text = "Asset Loading..";
        //Notification_Panel.SetActive(true);
        //BlackScreen.SetActive(true);

        if (PlayerScript.PlayerNumber != 1)
        {
            StartCoroutine(Timer());
        }

        StartCoroutine(screenLoading()); 



        //spawnPlayer();

        //newDice = GameObject.Find("Dice").GetComponent<NewDice>();

        //playerTurnManager = GameObject.Find("GameManager").GetComponent<PlayerTurnManager>();

        //cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();

        //itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        //cardManager.PlayerPointbarObject = playerPointbar;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError("Cek otherPlayerDone : " + otherPlayerDone);
        //Debug.LogError("Cek hostDone : " + hostDone);
        if ((PhotonNetwork.IsMasterClient == true && otherPlayerReady == true) || (PhotonNetwork.IsMasterClient == false && hostDone == true))
        {
            //Debug.LogError("Cek kondisi 1");

            if (forUpdate == false)
            {
                //Debug.LogError("Cek kondisi 2");
                //Debug.LogError("Cek forUpdate : " + forUpdate);

                forUpdate = true;

                spawnPlayer();

                newDice = GameObject.Find("Dice").GetComponent<NewDice>();

                playerTurnManager = GameObject.Find("GameManager").GetComponent<PlayerTurnManager>();

                cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();

                itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

                cardManager.PlayerPointbarObject = playerPointbar;

                if (playerTurnManager.currentTurn == PlayerScript.PlayerTurn)
                {
                    Notification_Panel.SetActive(false);
                    BlackScreen.SetActive(false);
                }
                else
                {
                    Notification_Panel.SetActive(false);
                }
            }
            else
            {
                //Debug.LogError("Cek kondisi 3");
                if (!PhotonNetwork.IsConnectedAndReady)
                {
                    onPlayerDisconnected("Connection lost");
                }
                else if (PhotonNetwork.CurrentRoom.PlayerCount != Rooms.expectedMaxPlayer)
                {
                    onPlayerDisconnected("Other player disconnected");
                }

                if (newDice.RollDiceIsDone)
                {
                    if (IsMove)
                    {
                        if (playerScript.PlayerCurrentWaypointIndex.Equals(100) && PawnMoveExceedLastSquare)
                        {
                            playerScript.PlayerMoveBackward(newDice.DiceValueLeft);
                        }
                        else
                        {
                            playerScript.PlayerMove(playerScript.PlayerCurrentWaypointIndex, newDice.RollDiceResult);
                        }
                    }
                }

                newDice.MultipleRollCheck();

                if (isReadyMovebyItem)
                {
                    movePlayerByItem();
                }

                if (PlayerHasReachLastSquare)
                {
                    object[] winnerData = new object[] { PlayerScript.PlayerNickname };
                    PhotonNetwork.RaiseEvent(RaiseEventCode.PLAYER_STOP_AT_LAST_SQUARE, winnerData, RaiseEventOptions.Default, SendOptions.SendReliable);
                    StartCoroutine(GameIsOver(PlayerScript.PlayerNickname));
                }
            }
        }

        //if (!PhotonNetwork.IsConnectedAndReady)
        //{
        //    onPlayerDisconnected("Connection lost");
        //}
        //else if (PhotonNetwork.CurrentRoom.PlayerCount != Rooms.expectedMaxPlayer)
        //{
        //    onPlayerDisconnected("Other player disconnected");
        //}

        //if (newDice.RollDiceIsDone)
        //{
        //    if (IsMove)
        //    {
        //        if (playerScript.PlayerCurrentWaypointIndex.Equals(100) && PawnMoveExceedLastSquare)
        //        {
        //            playerScript.PlayerMoveBackward(newDice.DiceValueLeft);
        //        }
        //        else
        //        {
        //            playerScript.PlayerMove(playerScript.PlayerCurrentWaypointIndex, newDice.RollDiceResult);
        //        }
        //    }
        //}

        //newDice.MultipleRollCheck();

        //if (isReadyMovebyItem)
        //{
        //    movePlayerByItem();
        //}

        //if (PlayerHasReachLastSquare)
        //{
        //    object[] winnerData = new object[] { PlayerScript.PlayerNickname };
        //    PhotonNetwork.RaiseEvent(RaiseEventCode.PLAYER_STOP_AT_LAST_SQUARE, winnerData, RaiseEventOptions.Default, SendOptions.SendReliable);
        //    StartCoroutine(GameIsOver(PlayerScript.PlayerNickname));
        //}
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2f);

        otherPlayerReady = true;
        object[] _otherPlayerReady = new object[] { otherPlayerReady };
        PhotonNetwork.RaiseEvent(RaiseEventCode.OTHER_PLAYER_READY, _otherPlayerReady, RaiseEventOptions.Default, SendOptions.SendReliable);
        //Debug.LogError("otherPlayerDone terkirim");

        //BlackScreen.SetActive(false);
        //Notification_Panel.SetActive(false);
    }

    IEnumerator screenLoading()
    {
        NotificationInfo_Text.text = "Waiting Other Player..";
        Notification_Panel.SetActive(true);
        BlackScreen.SetActive(true);

        yield return new WaitForSeconds(3f);
    }

    void onPlayerDisconnected(string message)
    {
        ConnectionLostAlert.DisconnectedFromScene = SceneManager.GetActiveScene().name;
        ConnectionLostAlert.DisconnectCauses = message;
        PointBar.PlayerPointValue = 0;
        cardManager.CardPile.Clear();
        SceneManager.LoadScene("ConnLostAlert");
    }

    void spawnPlayer()
    {
        if (PlayerScript.PlayerNumber.Equals(1))
        {
            player1Avatar = PhotonNetwork.Instantiate("Player/Player1Avatar", waypoints[playerStartWaypoint].position, waypoints[playerStartWaypoint].rotation, 0);

            player1Avatar.name = player1Avatar.GetPhotonView().Owner.NickName;

            playerScript = GameObject.Find(player1Avatar.GetPhotonView().Owner.NickName).GetComponent<PlayerScript>();

            playerPointbar = Instantiate(PlayerPointbarPrefab, PlayerPointbarSpawnLocation.transform.position, Quaternion.identity);

            playerPointbar.name = "Pointbar";

            // player1Avatar.GetPhotonView().TransferOwnership(0);
        }
        else if (PlayerScript.PlayerNumber.Equals(2))
        {
            player2Avatar = PhotonNetwork.Instantiate("Player/Player2Avatar", waypoints[playerStartWaypoint].position, waypoints[playerStartWaypoint].rotation, 0);

            player2Avatar.name = player2Avatar.GetPhotonView().Owner.NickName;

            playerScript = GameObject.Find(player2Avatar.GetPhotonView().Owner.NickName).GetComponent<PlayerScript>();

            playerPointbar = Instantiate(PlayerPointbarPrefab, PlayerPointbarSpawnLocation.transform.position, Quaternion.identity);

            playerPointbar.name = "Pointbar";

            //GameObject.Find("Player1Avatar(Clone)").transform.localScale = new Vector3(0.3f, 0.3f, 1f);

            // player2Avatar.GetPhotonView().TransferOwnership(0);
        }
        else if (PlayerScript.PlayerNumber.Equals(3))
        {
            player3Avatar = PhotonNetwork.Instantiate("Player/Player3Avatar", waypoints[playerStartWaypoint].position, waypoints[playerStartWaypoint].rotation, 0);

            player3Avatar.name = player3Avatar.GetPhotonView().Owner.NickName;

            playerScript = GameObject.Find(player3Avatar.GetPhotonView().Owner.NickName).GetComponent<PlayerScript>();

            playerPointbar = Instantiate(PlayerPointbarPrefab, PlayerPointbarSpawnLocation.transform.position, Quaternion.identity);

            playerPointbar.name = "Pointbar";

            //GameObject.Find("Player1Avatar(Clone)").transform.localScale = new Vector3(0.3f, 0.3f, 1f);

            // player3Avatar.GetPhotonView().TransferOwnership(0);
        }
        else if (PlayerScript.PlayerNumber.Equals(4))
        {
            player4Avatar = PhotonNetwork.Instantiate("Player/Player4Avatar", waypoints[playerStartWaypoint].position, waypoints[playerStartWaypoint].rotation, 0);

            player4Avatar.name = player4Avatar.GetPhotonView().Owner.NickName;

            playerScript = GameObject.Find(player4Avatar.GetPhotonView().Owner.NickName).GetComponent<PlayerScript>();

            playerPointbar = Instantiate(PlayerPointbarPrefab, PlayerPointbarSpawnLocation.transform.position, Quaternion.identity);

            playerPointbar.name = "Pointbar";

            //GameObject.Find("Player1Avatar(Clone)").transform.localScale = new Vector3(0.3f, 0.3f, 1f);

            // player4Avatar.GetPhotonView().TransferOwnership(0);
        }

        PlayerNicknameText.text = PlayerScript.PlayerNickname;

        if (PhotonNetwork.IsMasterClient == true)
        {
            hostDone = true;

            object[] _hostDone = new object[] { hostDone };
            PhotonNetwork.RaiseEvent(RaiseEventCode.HOST_READY, _hostDone, RaiseEventOptions.Default, SendOptions.SendReliable);
            //Debug.LogError("hostDone terkirim");
        }
    }

    public void checkPlayerMeetItem(Item itemCheck, string owner1, string owner2)
    {
        if (itemCheck.Owner == owner1 || itemCheck.Owner == owner2)
        {
            if (itemCheck.Type == Card.CardType.Ladder || itemCheck.Type == Card.CardType.Ladder2Tail)
            {
                playerHitItem = itemCheck;
                isReadyMovebyItem = true;

            }
        }
        else if (itemCheck.Type == Card.CardType.Snake || itemCheck.Type == Card.CardType.Snake2Head)
        {
            Debug.Log("Hit snake!");
            playerHitItem = itemCheck;
            isReadyMovebyItem = true;
        }
    }

    public IEnumerator itemOnBoardAgeUpdate()
    {
        yield return new WaitForSeconds(3f);
        if (cardManager.ItemOnBoard.Count > 0)
        {
            foreach (Item item in cardManager.ItemOnBoard)
            {
                if (item.Owner == PlayerScript.PlayerNickname)
                {
                    item.AgeUpdate();
                    if (item.Age == 3)
                    {
                        object[] destroyedItemData = new object[] { item };

                        PhotonNetwork.RaiseEvent(RaiseEventCode.ITEM_DESTROYED_AFTER_THREE_TURN, destroyedItemData, RaiseEventOptions.Default, SendOptions.SendReliable);

                        DestroyedItemList.Add(item);
                    }
                    else
                    {
                        Debug.Log("item age: " + item.Age);
                    }
                }
            }
        }
        else
        {
            Debug.Log("There aren't item on board.");
        }

        if (DestroyedItemList.Count > 0)
            DestroyItemsAfterThreeTurns();
        else
            Debug.Log("Destroyed item list are empty.");
    }

    void DestroyItemsAfterThreeTurns()
    {
        foreach (Item destroyedItem in DestroyedItemList)
        {
            // if(destroyedItem.Type == Card.CardType.Snake || destroyedItem.Type == Card.CardType.Snake2Head)
            //     SoundManager.PlaySoundEffect("SnakeDied");
            // else
            //     SoundManager.PlaySoundEffect("LadderDestroyed");

            if (destroyedItem.Owner == PlayerScript.PlayerNickname)
            {
                PhotonNetwork.Destroy(destroyedItem.ItemObject);
            }
            cardManager.ItemOnBoard.Remove(destroyedItem);
        }

        DestroyedItemList.Clear();
    }

    void movePlayerByItem()
    {
        PhotonView playerHitItemPhotonView = playerHitItem.ItemObject.GetPhotonView();
        string itemOwnerName = Regex.Match(playerHitItemPhotonView.Owner.ToString(), @"([A-z])\w+").Value;

        if (itemOwnerName != PlayerScript.PlayerNickname)
        {
            playerHitItemPhotonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        object[] destroyedItemData = new object[] { playerHitItem };

        PhotonNetwork.RaiseEvent(RaiseEventCode.ITEM_USED_BY_PLAYER, destroyedItemData, RaiseEventOptions.Default, SendOptions.SendReliable);

        playerScript.PlayerMoveByItem(playerHitItem.Destination, playerHitItem.Type);

        if (StillMoveBecauseItem == false)
        {
            playerScript.PlayerWalkinItemPosition = 0;
            if (playerHitItem.Type == Card.CardType.Snake || playerHitItem.Type == Card.CardType.Snake2Head)
            {
                //SoundManager.PlaySoundEffect("SnakeDied");
            }
            else
            {
                //SoundManager.PlaySoundEffect("LadderDestroyed");
            }
            PhotonNetwork.Destroy(playerHitItem.ItemObject);
            cardManager.ItemOnBoard.Remove(playerHitItem);
            isReadyMovebyItem = false;
        }
    }

    public IEnumerator DisplayNotificationInfo(string message, float duration, bool isOtherTurnNotif)
    {
        if (message != null)
        {
            if (playerTurnManager.currentTurn == PlayerScript.PlayerTurn)
            {
                NotificationInfo_Text.text = message;
                Notification_Panel.SetActive(true);
                BlackScreen.SetActive(true);

                yield return new WaitForSeconds(duration);

                Notification_Panel.SetActive(false);
                BlackScreen.SetActive(false);
            }
            else
            {
                NotificationInfo_Text.text = message;
                Notification_Panel.SetActive(true);
                BlackScreen.SetActive(true);

                yield return new WaitForSeconds(duration);

                Notification_Panel.SetActive(false);
            }
        }
    }

    IEnumerator GameIsOver(string winnerNickname)
    {
        cardManager.IsGotPlayerPointbarObject = false;
        cardManager.CardPile.Clear();
        PointBar.PlayerPointValue = 0;
        Gameover.WinnerNickname = winnerNickname;

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Gameover");
    }

    public void DisconnectTestButton()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Disconnect();
        }
        else
        {
            PhotonNetwork.ReconnectAndRejoin();
        }
    }

    IEnumerator disconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnectedAndReady)
            yield return null;
    }

    new void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    new void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    public void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == RaiseEventCode.ITEM_USED_BY_PLAYER)
        {
            object[] usedItemData = (object[])obj.CustomData;
            cardManager.UsedItem = (Item)usedItemData[0];
            cardManager.DestroyUsedItem(cardManager.UsedItem);
        }
        else if (obj.Code == RaiseEventCode.PLAYER_STOP_AT_LAST_SQUARE)
        {
            object[] winnerData = (object[])obj.CustomData;
            string tempWinnerNickname = (string)winnerData[0];
            StartCoroutine(GameIsOver(tempWinnerNickname));
        }
        else if (obj.Code == RaiseEventCode.ITEM_DESTROYED_AFTER_THREE_TURN)
        {
            object[] destroyedItemListData = (object[])obj.CustomData;
            Item destroyedItem = (Item)destroyedItemListData[0];

            DestroyedItemList.Add(destroyedItem);

            if (DestroyedItemList.Count > 0)
                DestroyItemsAfterThreeTurns();
            else
                Debug.Log("Destroyed item list are empty.");
        }
        else if (obj.Code == RaiseEventCode.HOST_READY)
        {
            object[] getObject = (object[])obj.CustomData;
            hostDone = (bool)getObject[0];

           // Debug.LogError("hostDone 2 : " + (bool)_hostReady[0]);
        }
        else if (obj.Code == RaiseEventCode.OTHER_PLAYER_READY)
        {
            object[] getObject = (object[])obj.CustomData;
            otherPlayerReady = (bool)getObject[0];

            //Debug.LogError("otherPlayerReady 2 : " + (bool)otherPlayerReady[0]);
        }
    }
}