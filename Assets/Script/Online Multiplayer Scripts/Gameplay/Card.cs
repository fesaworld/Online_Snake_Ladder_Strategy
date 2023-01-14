using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int Value;
    public CardLevel Level;
    public CardType Type;
    public Sprite Image;
    public SpriteRenderer ImageRender;
    public GameObject CardObject;
    public string Owner;

    public Card(int _value, CardLevel _level, CardType _type){
        this.Value = _value;
        this.Level = _level;
        this.Type = _type;
    }

    public enum CardLevel {
        None,
        One,
        Two,
        Three
    }

    public enum CardType {
        None,
        Snake,
        Snake2Head,
        Ladder,
        Ladder2Tail
    }

    public void DisplayCard(GameObject goCard) {
        this.ImageRender = goCard.GetComponent<SpriteRenderer>();
        this.Image = Resources.Load<Sprite>(
            "New Images Asset/Card/"+this.Level+"/"+this.Type+"/"+this.Value
        );
        this.ImageRender.sprite = this.Image;
    }

    public void DisplaySaveCard(int saveCardIndex, bool isCardTakenOrSwitchTurn){
        this.CardObject = GameObject.Find("Sv_Card"+saveCardIndex.ToString()+"_Button");
        if (isCardTakenOrSwitchTurn)
        {
            string levelCardFolder = (saveCardIndex == 1) ? "One"
                : (saveCardIndex == 2) ? "Two"
                : "Three";
            this.CardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(
                "New Images Asset/Card/0"
            );
            this.CardObject.GetComponent<Button>().interactable = false;
        } else
        {
            this.CardObject.GetComponent<Image>().sprite = this.Image;
            this.CardObject.GetComponent<Button>().interactable = true;
        }
    }
}
