using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private CoinsController coinsController;
    private GridController gridController;
    private CameraController cameraController;
    private ComputerPlayer computerPlayer;

    private AudioSource endRoundAudio;

    private GameObject unPlacedCoin = null; //Coin that is above board, ready to be dropped
    private int selectedColumn = 0;
    private Vector3 targetPosition = new Vector3(0f, 8f, 0f);
    private float columnSize = 2f;

    public PlayerData[] playerData = new PlayerData[2];
    private int currentPlayer = 1; //Player whos turn it is
    private TMP_Text currentPlayerLabel;
    private WaitForSeconds timeBetweenTurns = new WaitForSeconds(0.5f);
    private bool gameStarted = false;
    private bool canDoTurn = false;
    public GameMode gameMode = GameMode.PlayerVsPlayer;

    public delegate void OnGameStart();
    public OnGameStart onGameStart;

    public delegate void OnGameEnd(int playerWin);
    public OnGameEnd onGameEnd;

    public delegate void OnGameRestart();
    public OnGameRestart onGameRestart;

    void Awake()
    {
        coinsController = GetComponent<CoinsController>();
        gridController = GetComponent<GridController>();
        cameraController = GetComponent<CameraController>();
        currentPlayerLabel = GameObject.Find("CurrentPlayerLabel").GetComponent<TMP_Text>();

        computerPlayer = new ComputerPlayer(gridController);
        endRoundAudio = GameObject.Find("EndRound").GetComponent<AudioSource>();

        InputController inputController = GetComponent<InputController>();
        inputController.onMouseClick += OnMouseClick;
        inputController.onColumnChange += OnColumnChange;
    }

    private void Start()
    {
        targetPosition = new Vector3(0f, gridController.GetDotPosition(0, gridController.GetGridSize().y + 1).y, 0f);
        cameraController.ChangeView("MainMenu");
    }

    void Update()
    {
        if (gameStarted && canDoTurn && unPlacedCoin)
        {
            MoveUnplacedCoin();
        }
    }

    public void StartGame()
    {
        onGameRestart?.Invoke();
        cameraController.ChangeView("Game");
        SetCurrentPlayer(Random.Range(1,3));//Set player randomly
        onGameStart?.Invoke();
        gameStarted = true;
        StartCoroutine(TimeBetweenTurns(false));
    }

    public void EndGame(bool win)
    {
        endRoundAudio.Play();
        gameStarted = false;
        onGameEnd?.Invoke(currentPlayer);
        currentPlayerLabel.text = win ? string.Format("{0} wins!", playerData[currentPlayer-1].name) : "Tie";
    }

    private void NextTurn(bool changePlayer = true)
    {
        if (changePlayer)//Change current player to other player
            SetCurrentPlayer(currentPlayer == 1 ? 2 : 1);

        unPlacedCoin = coinsController.SpawnCoin(targetPosition, currentPlayer);
        targetPosition = GetColumnPosition(targetPosition);

        canDoTurn = gameMode == GameMode.PlayerVsPlayer || (gameMode == GameMode.PlayerVsComputer && currentPlayer == 1);
        if (!canDoTurn)
            ComputerDoTurn();
    }

    private void DoTurn()
    {
        if (gridController.AddCoinToColumn(selectedColumn, currentPlayer, out Vector2Int gridPosition, out bool win))//Placed coin
        {
            canDoTurn = false;
            //unPlacedCoin.transform.position = gridController.GetDotPosition(gridPosition);
            unPlacedCoin.GetComponent<Coin>().Place(gridController.GetDotPosition(gridPosition));
            unPlacedCoin = null;

            if (win)
            {
                EndGame(true);
            }
            else if (gridController.GetAvailableColumns().Count <= 0)
            {
                EndGame(false);
            }
            else
            {
                StartCoroutine(TimeBetweenTurns());
            }
        }
    }

    private void ComputerDoTurn()
    {
        selectedColumn = computerPlayer.GetTurnSmart(currentPlayer);
        unPlacedCoin.transform.position = GetColumnPosition(targetPosition);
        DoTurn();
    }

    private IEnumerator TimeBetweenTurns(bool changePlayer = true)
    {
        yield return timeBetweenTurns;
        NextTurn(changePlayer);
    }

    private void MoveUnplacedCoin()
    {
        unPlacedCoin.transform.position = Vector3.Lerp(unPlacedCoin.transform.position, targetPosition, Time.deltaTime * 10f);
    }

    public void MainMenu()
    {
        gameStarted = false; 
        canDoTurn = false;
        cameraController.ChangeView("MainMenu");
    }

    private void OnMouseClick()
    {
        //Check if on UI element
        if (EventSystem.current.IsPointerOverGameObject())// || EventSystem.current.currentSelectedGameObject != null)
            return;

        if (gameStarted && canDoTurn)
        {
           DoTurn();
        }
    }

    private void OnColumnChange(int last, int current)
    {
        selectedColumn = current;

        targetPosition = GetColumnPosition(targetPosition);
    }

    public void SetCurrentPlayer(int playerID)
    {
        currentPlayer = playerID;
        currentPlayerLabel.text = playerData[playerID-1].name;
        currentPlayerLabel.color = GetPlayerColor(playerID);
    }

    private Vector3 GetColumnPosition(Vector3 position)
    {
        position.x = gridController.GetDotPosition(selectedColumn, 0).x;// (selectedColumn * columnSize) - (gridController.GetGridSize().x * columnSize / 2f) + (gridController.GetGridSize().x % 2 == 0 ? 0f : columnSize/2f);
        return position;
    }

    public Color GetPlayerColor(int playerID)
    {
        return playerData[playerID - 1].color;
    }
}
