using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class HPColor {
    public int hp;
    public Color color;
}

public class Character : MonoBehaviour
{
    [Header("Character")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private TMP_Text numberText;
    [SerializeField]
    private TMP_Text hpText;

    [Header("HP")]
    [SerializeField]
    private int HP = 100;
    [Tooltip("Couleur quand HP <= value")]
    [SerializeField]
    private List<HPColor> hpColors;

    [HideInInspector]
    public VisualizeTile standingOnTile;

    [HideInInspector]
    public bool hasMoved = false;

    [HideInInspector]
    public bool hasAttacked = false;

    [HideInInspector]
    public int number;

    private GameplayHelper game;


    public void SetupPlayer(GameplayHelper game, PlayerSerializable player) {
        this.game = game;
        this.number = player.number;
        // Player Data Setup
        numberText.text = "P" + player.number.ToString();
        numberText.color = player.textColor;
        spriteRenderer.color = player.color;
        // HP Setup
        Damage(0);
    }

    public void Damage(int damage) {
        HP -= damage;
        hpText.text = HP.ToString()+ "HP";
        if (HP <= 0) {
            game.Die(this);
        }
        else {
            foreach (HPColor hpColor in hpColors) {
                if (HP <= hpColor.hp) {
                    hpText.color = hpColor.color;
                    break;
                }
            }
        }
    }
}
