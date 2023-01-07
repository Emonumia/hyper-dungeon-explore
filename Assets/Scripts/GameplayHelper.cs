using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameplayHelper : MonoBehaviour
{
    [Header("Start Interface")]
    [SerializeField]
    private GameObject startInterface;

    [SerializeField]
    private TMP_Text playerCountText;

    [Header("Gameplay Interface")]
    [SerializeField]
    private TMP_Text currentPlayerText;
    [SerializeField]
    private TMP_Text currentPhaseText;
    [SerializeField]
    private GameObject gameplayInterface;
    [SerializeField]
    private GameObject cursor;
    private SpriteRenderer cursorSpriteRenderer;

    [Header("End Interface")]
    [SerializeField]
    private GameObject endInterface;
    [SerializeField]
    private TMP_Text winPlayerText;

    [Header("Gameplay")]
    [SerializeField]
    private int damage;
    [SerializeField]
    private int damageVariation;

    [Header("Pathfinding")]
    [SerializeField]
    private MouseController mouseController;

    [Header("Players")]
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private List<PlayerSerializable> players;


    private PathFinder pathFinder = new PathFinder();
    private List<Character> characters = new List<Character>();
    private int currentPlayer = 0;

    private float _playerCount = 1f;

    public float playerCount {
        get {
            return _playerCount;
        }
        set {
            _playerCount = value;
            playerCountText.text = value.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // UI
        startInterface.SetActive(true);
        gameplayInterface.SetActive(false);
        endInterface.SetActive(false);

        // Cursor
        cursorSpriteRenderer = cursor.GetComponent<SpriteRenderer>();
        cursor.SetActive(false);
    }

    public void StartGame()
    {
        // Cursor
        cursor.SetActive(true);

        // Create players
        for (int i = 0; i < playerCount; i++)
        {
            GameObject player = Instantiate(playerPrefab, transform);
            Character character = player.GetComponent<Character>();
            character.SetupPlayer(this, players[i]);
            characters.Add(character);
            mouseController.MoveTo(character, Vector2.zero);
        }

        // UI
        startInterface.SetActive(false);
        gameplayInterface.SetActive(true);
        currentPlayerText.text = "Player " + (currentPlayer + 1).ToString();
        currentPlayerText.color = players[currentPlayer].textColor;
        currentPhaseText.text = "Move";

        // Hide tiles overlay
        foreach (KeyValuePair<Vector2Int, VisualizeTile> tile in MapContainer.Singleton.map)
        {
            tile.Value.HideTile();
        }
    }

    public CursorClickType CursorClick() {
        if (!GetCurrentCharacter().hasMoved) {
            GetCurrentCharacter().hasMoved = true;
            currentPhaseText.text = "Attack";
            return CursorClickType.Move;
        } else if (!GetCurrentCharacter().hasAttacked) {
            GetCurrentCharacter().hasAttacked = true;
            currentPhaseText.text = "Turn End";
            return CursorClickType.Attack;
        } else {
            return CursorClickType.None;
        }
    }

    public void Attack(VisualizeTile tile) {
        List<Character> localCharacters = new List<Character>(characters);
        localCharacters.ForEach(character => {
            if (character.standingOnTile == tile && character != GetCurrentCharacter() && characters.Contains(character)) {
                character.Damage(damage + Mathf.RoundToInt(Random.Range(-damageVariation, damageVariation)));
            }
        });
    }

    public void EndTurnClick() {
        // Reset character
        GetCurrentCharacter().hasMoved = false;
        GetCurrentCharacter().hasAttacked = false;

        // Next player
        currentPlayer++;
        if (currentPlayer >= characters.Count) {
            currentPlayer = 0;
        }

        // UI
        currentPlayerText.text = "Player " + players[currentPlayer].number;
        currentPlayerText.color = players[currentPlayer].textColor;
        currentPhaseText.text = "Move";
    }

    public void Die(Character character) {
        characters.Remove(character);
        players.Remove(players.Find(p => p.number == character.number));
        Destroy(character.gameObject);
        currentPlayer--;

        if (characters.Count <= 1) {
            // UI
            gameplayInterface.SetActive(false);
            endInterface.SetActive(true);
            winPlayerText.text = "Player " + characters[0].number;
            winPlayerText.color = players[0].textColor;
        }
    }

    public Character GetCurrentCharacter() {
        return characters[currentPlayer];
    }
}
