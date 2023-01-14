using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewDice : MonoBehaviour
{
    public Button RollButton;
    public SpriteRenderer DiceSpriteRenderer;
    public static int RandomDiceSides = 0;
    public static int GetSixValueOnDice = 0;
    
    public bool RollDiceIsDone = false;
    public int RollDiceResult = 0;
    public int GetSixCounts = 0;
    public int DiceValueLeft = 0;

    Sprite[] diceSides;
    GameplayManager gameplayManager;
    PlayerTurnManager playerTurnManager;

    // Start is called before the first frame update
    void Start()
    {
        //diceSides = Resources.LoadAll<Sprite>("New Images Asset/Dice/Default/");
        //DiceSpriteRenderer.sprite = diceSides[0];

        //gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();
        //playerTurnManager = GameObject.Find("GameManager").GetComponent<PlayerTurnManager>();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);

        diceSides = Resources.LoadAll<Sprite>("New Images Asset/Dice/Default/");
        DiceSpriteRenderer.sprite = diceSides[0];

        gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();
        playerTurnManager = GameObject.Find("GameManager").GetComponent<PlayerTurnManager>();


    }

    public void OnClick_Dice() {
        StartCoroutine(rollDice());
    }

    IEnumerator rollDice() {
        RollButton.interactable = false;

        //SoundManager.PlaySoundEffect("DiceRoll");

        for (int index = 0; index < 8; index++) {
            RandomDiceSides = Random.Range(0, 6);
            DiceSpriteRenderer.sprite = diceSides[RandomDiceSides];
            yield return new WaitForSeconds(0.15f);
        }

        RollDiceResult = RandomDiceSides + 1;

        if (RollDiceResult == 6) {
            GetSixCounts += 1;
            if(GetSixCounts == 1) {
                gameplayManager.IsBonusTurnNotifDisplayed = true;
            }
        }

        RollDiceIsDone = true;

        gameplayManager.IsMove = true;
    }

    public void MultipleRollCheck() {
        if(GetSixCounts == 1 && !gameplayManager.IsMove && gameplayManager.IsBonusTurnNotifDisplayed) {
            string message = "bonus turn";
            StartCoroutine(gameplayManager.DisplayNotificationInfo(message, 2f, false));
            RollButton.interactable = true;
            gameplayManager.IsBonusTurnNotifDisplayed = false;
        } else if(GetSixCounts == 2) {
            RollButton.interactable = false;
            GetSixCounts = 0;
        }
    }
}
