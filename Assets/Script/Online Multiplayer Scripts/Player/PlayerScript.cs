using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class PlayerScript : MonoBehaviour
{   
    public static string PlayerNickname;
    public static int PlayerNumber = 0;
    public static int PlayerTurn = 0;

    [HideInInspector]
    public int PlayerMovingWaypointIndex = 0;
    public int PlayerNextWaypointIndex = 0;
    public int PlayerCurrentWaypointIndex = 0;
    public int PlayerWalkinItemPosition;
    public float MoveSpeed;

    public Canvas PlayerUICanvas;
    public Text PlayerNicknameText;
    public PhotonView PlayerPhotonView;

    SpriteRenderer playerSpriteRenderer;
    GameplayManager gameplayManager;
    CardManager cardManager;
    PointBar pointBar;
    NewDice newDice;
    Rooms rooms;

    void Start() {
        PlayerNickname = PhotonNetwork.LocalPlayer.NickName;

        if(SceneManager.GetActiveScene().name.Equals("RoomLobby2Players") || SceneManager.GetActiveScene().name.Equals("RoomLobby3Players") || SceneManager.GetActiveScene().name.Equals("RoomLobby4Players")) {
            playerSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            PlayerNicknameText.text = PlayerPhotonView.Owner.NickName;
            if(PlayerPhotonView.Owner.NickName == PlayerNickname) {
                PlayerNicknameText.fontStyle = FontStyle.Bold;
            }

        } else if(SceneManager.GetActiveScene().name.Equals("Gameplay")) {
            //PlayerUICanvas.gameObject.SetActive(false);

            //gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();
            //newDice = GameObject.Find("Dice").GetComponent<NewDice>();
            //cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
            //pointBar = GameObject.Find("Pointbar").GetComponent<PointBar>();

            //this.gameObject.transform.localScale = gameplayManager.Player1SpawnLocation.transform.localScale;

            gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();

            StartCoroutine(Timer());
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Gameplay"))
        {
            //Debug.Log("gameObject local scale : " + this.gameObject.transform.localScale);
            //Debug.Log("gameplayManager local scale : " + gameplayManager.Player1SpawnLocation.transform.localScale);
            this.gameObject.transform.localScale = gameplayManager.Player1SpawnLocation.transform.localScale;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);

        PlayerUICanvas.gameObject.SetActive(false);

        //gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();
        //this.gameObject.transform.localScale = gameplayManager.Player1SpawnLocation.transform.localScale;

        newDice = GameObject.Find("Dice").GetComponent<NewDice>();
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        pointBar = GameObject.Find("Pointbar").GetComponent<PointBar>();


    }

    public void PlayerMove(int playerCurrentPosition, int RollDiceResult) {
        gameplayManager.EndTurnButton.interactable = false;
        PlayerNextWaypointIndex = playerCurrentPosition + RollDiceResult;

        if(PlayerNextWaypointIndex > 100) {
            newDice.DiceValueLeft = PlayerNextWaypointIndex % 100;
            gameplayManager.PawnMoveExceedLastSquare = true;

            PlayerNextWaypointIndex = 100;
        } else {
            gameplayManager.PawnMoveExceedLastSquare = false;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, gameplayManager.waypoints[PlayerMovingWaypointIndex].position, MoveSpeed * Time.deltaTime);

        if(transform.position == gameplayManager.waypoints[PlayerMovingWaypointIndex].position && transform.position != gameplayManager.waypoints[PlayerNextWaypointIndex].position) {
            PlayerMovingWaypointIndex += 1;
            pointBar.AddPlayerPointValue(1);
            gameplayManager.WaypointsLeft--;
        } else if(transform.position == gameplayManager.waypoints[PlayerNextWaypointIndex].position) {
            SetCurrentPositionPlayer(RollDiceResult);
            if(!gameplayManager.PawnMoveExceedLastSquare) {
                gameplayManager.IsMove = false;
                newDice.RollDiceIsDone = false;
            }
            playerEndMove();
        }
    }

    public void PlayerMoveBackward(int diceValueLeft) {
        gameplayManager.EndTurnButton.interactable = false;
        PlayerNextWaypointIndex = 100 - diceValueLeft;

        transform.position = Vector2.MoveTowards(transform.position, gameplayManager.waypoints[PlayerMovingWaypointIndex].position, MoveSpeed * Time.deltaTime);

        if(transform.position == gameplayManager.waypoints[PlayerMovingWaypointIndex].position && transform.position != gameplayManager.waypoints[PlayerNextWaypointIndex].position) {
            PlayerMovingWaypointIndex -= 1;
            gameplayManager.WaypointsLeft += diceValueLeft;
        } else if(transform.position == gameplayManager.waypoints[PlayerNextWaypointIndex].position) {
            SetCurrentPositionPlayer(-diceValueLeft);
            newDice.DiceValueLeft = 0;
            gameplayManager.PawnMoveExceedLastSquare = false;
            gameplayManager.IsMove = false;
            newDice.RollDiceIsDone = false;
            playerEndMove();
        }
    }

    void playerEndMove() {
        if (PlayerCurrentWaypointIndex == 100 && !gameplayManager.PawnMoveExceedLastSquare) {
            gameplayManager.PlayerHasReachLastSquare = true;
        } else {
            if (cardManager.ItemOnBoard.Count > 0) {
                int index = cardManager.ItemOnBoard.FindIndex(f => f.BoardPosition.Equals(PlayerCurrentWaypointIndex));

                if(index >= 0) {
                    Item itemCheck = cardManager.ItemOnBoard[index];
                    gameplayManager.checkPlayerMeetItem(itemCheck, PlayerNickname, PlayerNickname);
                } 
                else {
                    //return;
                }
            }
        }
        gameplayManager.EndTurnButton.interactable = true;
    }

    public void PlayerMoveByItem(Transform tailPiece, Card.CardType itemType)
    {
        gameplayManager.EndTurnButton.interactable = false;

        if(PlayerWalkinItemPosition <= 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, tailPiece.transform.position, MoveSpeed * Time.deltaTime);

            GameplayManager.StillMoveBecauseItem = true;

            if(transform.position == tailPiece.transform.position)
            {
                if(itemType == Card.CardType.Snake || itemType == Card.CardType.Snake2Head)
                {
                    //SoundManager.PlaySoundEffect("GotSnakeEffect");
                } 
                else
                {
                    //SoundManager.PlaySoundEffect("GotLadderEffect");
                }

                PlayerWalkinItemPosition += 1;
            }
        }
        else
        {
            PlayerMovingWaypointIndex = Waypoint.WaypointIndexContainPlayer;
            SetCurrentPositionPlayer(Waypoint.WaypointIndexContainPlayer);
            GameplayManager.StillMoveBecauseItem = false;
        }

        gameplayManager.EndTurnButton.interactable = true;
    }

    public void OtherPlayerMoveByItem()
    {
        
    }

    public void SetCurrentPositionPlayer(int currentWaypointIndex) {
        if(GameplayManager.StillMoveBecauseItem) {
           PlayerCurrentWaypointIndex = currentWaypointIndex;
        } else {
            PlayerCurrentWaypointIndex += currentWaypointIndex;
        }

        if(PlayerCurrentWaypointIndex > 100) {
            PlayerCurrentWaypointIndex = 100;
        }
        
    }
}
