using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBar : MonoBehaviour {
    // untuk poinbar player 1, selama testing pake yang p1 dulu dengan asset yang berbeda
    public static Sprite PlayerPointBarVisualValue;
    public static SpriteRenderer PlayerPointBarRenderer;

    // Tambahan dari aku
    GameObject playerPointBar;
    PlayerTurnManager playerTurnManager;

    public static int PlayerPointValue = 0;

    void Start() {
        playerPointBar = GameObject.Find("Pointbar");
        PlayerPointBarRenderer = playerPointBar.GetComponent<SpriteRenderer>();
    }

    public void AddPlayerPointValue(int amount) {
        if(PlayerPointValue < 20) {
                PlayerPointValue += amount;
        } else if(PlayerPointValue >= 20) {
            PlayerPointValue = 20;
        }

        PlayerPointBarVisualValue = Resources.Load<Sprite>("New Images Asset/PointBar/Default/" + PlayerPointValue.ToString());

        PlayerPointBarRenderer.sprite = PlayerPointBarVisualValue;
    }

    public void SubtractPlayerPointValue(int amount) {
        PlayerPointValue -= amount;

        PlayerPointBarVisualValue = Resources.Load<Sprite>("New Images Asset/PointBar/Default/" + PlayerPointValue.ToString());

        PlayerPointBarRenderer.sprite = PlayerPointBarVisualValue;
    }
}
