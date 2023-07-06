using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    private GameController gameController;

    public GameObject coinPrefab; //Coin prefab to spawn
    private List<GameObject> coinPool = new List<GameObject>(); //List of disabled coins to reuse later.
    private List<GameObject> activeCoins = new List<GameObject>();
    private Transform coinParent;

    void Awake()
    {
        coinParent = GameObject.Find("Coins").transform;
        gameController = GetComponent<GameController>();
        gameController.onGameRestart += RestartGame;
        gameController.onGameEnd += EndGame;
    }

    private void RestartGame()
    {
        //add coins to pool to reuse later
        foreach(GameObject obj in activeCoins)
        {
            DisableCoin(obj);
        }
        activeCoins = new List<GameObject>();//empty list
    }

    private void EndGame(int playerID)
    {
        foreach(GameObject obj in activeCoins)
        {
            Coin coin = obj.GetComponent<Coin>();
            if (coin.PlayerID == playerID)
                coin.Win();
        }
    }

    //Spawn coin at location. Check if coin objects available in pool otherwise make new object
    public GameObject SpawnCoin(Vector3 position, int playerID = 1)
    {
        GameObject obj;

        if(coinPool.Count > 0)
        {
            obj = coinPool[coinPool.Count - 1];//Reuse coin object from pool
            coinPool.RemoveAt(coinPool.Count - 1);//Remove coin from pool, because we will use it
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(coinPrefab, coinParent);
        }

        obj.transform.position = position;
        Coin coin = obj.GetComponent<Coin>();
        coin.PlayerID = playerID;
        coin.SetColor(gameController.GetPlayerColor(playerID));
        coin.Reset();

        activeCoins.Add(obj);//add to list to disable when game restarts

        return obj;
    }

    public void DisableCoin(GameObject obj)
    {
        obj.SetActive(false);
        coinPool.Add(obj);
    }
}
