using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private int[,] grid = new int[7,6]; //Grid to see where players put their coins and to check if win condition - 0,0 = left bottom
    private List<int> availableColumns = new List<int>();

    public GameObject dotBG;
    private Transform parent;

    private float spaceBetweenDots = 1f;

    void Awake()
    {
        GetComponent<GameController>().onGameRestart += ResetGrid;
        parent = GameObject.Find("Grid").transform;
        SpawnGrid();
    }

    public void ResetGrid()
    {
        grid = new int[grid.GetLength(0), grid.GetLength(1)]; //Make grid empty again
        availableColumns = new List<int>();
        for(int i = 0; i < grid.GetLength(1); ++i)
        {
            availableColumns.Add(i);
        }
    }

    private void SpawnGrid()
    {
        for (int x = 0; x < grid.GetLength(0); ++x)
        {
            for (int y = 0; y < grid.GetLength(1); ++y)
            {
                Instantiate(dotBG, GetDotPosition(x, y), Quaternion.identity, parent);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column"></param>
    /// <param name="playerID"></param>
    /// <returns>TRUE if coin is placed</returns>
    public bool AddCoinToColumn(int column, int playerID, out Vector2Int gridPosition, out bool win)
    {
        bool coinPlaced = AddCoinToColumn(ref grid, column, playerID, out gridPosition, out win);

        if (coinPlaced)
        {
            if (grid[column, grid.GetLength(1) - 1] != 0)
                availableColumns.Remove(column);
        }

        return coinPlaced;
    }

    // Can use this with copy of grid for computer turn check
    public bool AddCoinToColumn(ref int[,] grid, int column, int playerID, out Vector2Int gridPosition, out bool win)
    {
        win = false;
        if (column < grid.GetLength(0) && column >= 0)
        {
            for (int i = 0; i < grid.GetLength(1); ++i)//Go throught rows to check if 0. If 0 store playerID in grid.
            {
                if (grid[column, i] == 0)
                {
                    grid[column, i] = playerID;
                    gridPosition = new Vector2Int(column, i);
                    win = CheckIfWin(grid, gridPosition);
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("Wrong column number! Index is outside grid 2d array.");
        }

        gridPosition = Vector2Int.zero;
        return false;
    }

    /// <summary>
    /// Check if player wins with given grid and last placed coin
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="lastAddedCoinPos">Last added coin position</param>
    /// <returns>TRUE if win</returns>
    public bool CheckIfWin(int[,] grid, Vector2Int lastAddedCoinPos)
    {
        int playerID = grid[lastAddedCoinPos.x, lastAddedCoinPos.y];

        return CheckForFourInARow(grid, lastAddedCoinPos, playerID);
    }

    private bool CheckForFourInARow(int[,] grid, Vector2Int lastAddedCoinPos, int playerID)
    {
        Vector2Int[] directionChecks = new Vector2Int[]{ new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1,1), new Vector2Int(1,-1) }; //horizontal, vertical, diagonalrightup, diagonalrightdown
        int connectedCoins = 0;

        foreach (Vector2Int direction in directionChecks)
        {
            connectedCoins = 0;

            for (int opposite = -1; opposite <= 1; opposite += 2) //Make sure to check both ways
            {
                for (int i = 1; i <= 3; ++i)//Check up to 3 positions
                {
                    int x = lastAddedCoinPos.x + direction.x * opposite * i;
                    int y = lastAddedCoinPos.y + direction.y * opposite * i;

                    //Check if outside of grid
                    if (x < 0 || y < 0 || x >= grid.GetLength(0) || y >= grid.GetLength(1))
                        break;

                    if (grid[x, y] == playerID)
                        connectedCoins++;
                    else
                        break;
                }
            }

            if (connectedCoins >= 3)
                return true;
        }

        return false;
    }

    private bool CheckHorizontalLine(int[,] grid, Vector2Int lastAddedCoinPos, int playerID)
    {
        int x = lastAddedCoinPos.x;
        int connectedCoins = 0;

        //Check right of added coin
        for(int i = 1; i <= 3; ++i) 
        {
            if(x + i < grid.GetLength(0))
            {
                if(grid[x + i, lastAddedCoinPos.y] == playerID)
                {
                    connectedCoins++;
                }
                else
                {
                    break;
                }
            }
        }

        if(connectedCoins >= 3)
            return true;

        x = lastAddedCoinPos.x;

        //Check left of added coin
        for (int i = 1; i <= 3; ++i)
        {
            if (x - i >= 0)
            {
                if (grid[x - i, lastAddedCoinPos.y] == playerID)
                {
                    connectedCoins++;
                }
                else
                {
                    break;
                }
            }
        }

        if (connectedCoins >= 3)
            return true;
        else
            return false;
    }

    private bool CheckVerticalLine(int[,] grid, Vector2Int lastAddedCoinPos, int playerID)
    {
        int y = lastAddedCoinPos.y;
        int connectedCoins = 0;

        //Check up from added coin
        for (int i = 1; i <= 3; ++i)
        {
            if (y + i < grid.GetLength(1))
            {
                if (grid[lastAddedCoinPos.x, y + i] == playerID)
                {
                    connectedCoins++;
                }
                else
                {
                    break;
                }
            }
        }

        if (connectedCoins >= 3)
            return true;

        y = lastAddedCoinPos.y;

        //Check down from added coin
        for (int i = 1; i <= 3; ++i)
        {
            if (y - i >= 0)
            {
                if (grid[lastAddedCoinPos.x, y - i] == playerID)
                {
                    connectedCoins++;
                }
                else
                {
                    break;
                }
            }
        }

        if (connectedCoins >= 3)
            return true;
        else
            return false;
    }

    //Columns where a coins still can be dropped
    public List<int> GetAvailableColumns()
    {
        return availableColumns;
    }

    public int GetRandomAvailableColumn()
    {
        return availableColumns[Random.Range(0, availableColumns.Count-1)];
    }

    public Vector2Int GetGridSize()
    {
        return new Vector2Int(grid.GetLength(0), grid.GetLength(1));
    }

    public float GetDotSpace()
    {
        return spaceBetweenDots;
    }

    public Vector3 GetDotPosition(int x, int y)
    {
        Vector3 position = new Vector3(x * spaceBetweenDots, y * spaceBetweenDots, 0f);
        position.x -= grid.GetLength(0) / 2f;
        position.x += spaceBetweenDots / 2f;
        position.y -= (grid.GetLength(1) / 2f);
        position.y += spaceBetweenDots / 2f;
        return position;
    }

    public Vector3 GetDotPosition(Vector2Int gridPosition)
    {
        return GetDotPosition(gridPosition.x, gridPosition.y);
    }

    public int[,] GetGrid()
    {
        return grid;
    }
}
