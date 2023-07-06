using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameController gameController;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void StartPvPGame() //Player vs player
    {
        gameController.gameMode = GameMode.PlayerVsPlayer;
        gameController.StartGame();
    }

    public void StartPvCGame()//Player vs computer 
    {
        gameController.gameMode = GameMode.PlayerVsComputer;
        gameController.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
