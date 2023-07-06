using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerPlayer
{
    private GridController gridController;

    public ComputerPlayer(GridController gridController)
    {
        this.gridController = gridController;
    }

    public int GetTurnDumb(int currentPlayer)
    {
        int selectedColumn = 0;
        int columnPlayerCanWin = -1;
        int[,] currentGrid = gridController.GetGrid();
        int[,] gridCopy; //= new int[currentGrid.GetLength(0), currentGrid.GetLength(1)];
        bool foundWin = false;

        for (int i = 0; i < gridController.GetGridSize().x; ++i)
        {
            gridCopy = currentGrid.Clone() as int[,];
            if (gridController.AddCoinToColumn(ref gridCopy, i, currentPlayer, out Vector2Int gridPosition, out bool win) && win)
            {
                selectedColumn = i;
                foundWin = true;
                break;
            }
            gridCopy = currentGrid.Clone() as int[,];
            if (gridController.AddCoinToColumn(ref gridCopy, i, 1, out gridPosition, out win) && win)//check if player can win
            {
                columnPlayerCanWin = i;
            }
        }

        if (!foundWin)
        {
            if (columnPlayerCanWin == -1)
            {
                selectedColumn = gridController.GetRandomAvailableColumn();
            }
            else
            {
                selectedColumn = columnPlayerCanWin;//block player
            }
        }

        return selectedColumn;
    }

    public int GetTurnSmart(int currentPlayer)
    {
        int selectedColumn = -1;
        Node root = new Node(gridController, gridController.GetGrid().Clone() as int[,], null, currentPlayer);
        root.GenerateChildren();
        root.OrderChildren();

        if (root.children[root.children.Count - 1].value > 0)//Check if there any moves that wins the game
        {
            selectedColumn = root.children[root.children.Count - 1].column;
        }
        else if (root.children[0].value <= -100)//Check if opposite can make move to win, so can block
        {
            selectedColumn = root.children[0].column;
        }
        else//Just a random move
        {
            for (int i = 0; i < root.children.Count; ++i)
            {
                if (root.children[i].value >= 0) 
                {
                    selectedColumn = root.children[Random.Range(i, root.children.Count)].column;
                    break;
                }
            }
            if (selectedColumn == -1)
                selectedColumn = root.children[root.children.Count - 1].column;
        }

        return selectedColumn;
    }

    public class Node
    {
        protected GridController gridController;
        public List<Node> children = new List<Node>();
        public int[,] grid;
        public Node parent;
        protected int id = 1;
        protected bool win = false;
        public int column = 0;

        public int value = 0;

        public Node(GridController gridController, int[,] grid, Node parent, int id, bool win = false)
        {
            this.gridController = gridController;
            this.grid = grid;
            this.parent = parent;
            this.id = id;
        }

        public virtual void GenerateChildren()
        {
            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                int[,] gridCopy = (int[,])grid.Clone();
                bool coinPlaced = gridController.AddCoinToColumn(ref gridCopy, x, id, out Vector2Int gridPosition, out bool win);

                if (coinPlaced)
                { 
                    Node node = new ComputerNode(gridController, gridCopy, this, id, win);
                    node.column = x;

                    children.Add(node);

                    if (!win)//if not win check if player can win so could block
                    {
                        int[,] gridCopy2 = (int[,])grid.Clone();
                        coinPlaced = gridController.AddCoinToColumn(ref gridCopy2, x, id == 1 ? 2 : 1, out gridPosition, out win);

                        if (win)
                            node.value -= 100;
                    }
                }
            }
        }

        public void OrderChildren()
        {
            List<Node> sortedList = children.OrderBy(o => o.value).ToList();
            children = sortedList;

            foreach (Node child in children)
            {
                child.OrderChildren();
            }
        }
    }

    public class ComputerNode : Node
    {
        public ComputerNode(GridController gridController, int[,] grid, Node parent, int id, bool win =false) 
            : base(gridController, grid, parent, id, win)
        {
            if (win)
            {
                value += 10;
            }

            if (!win)
                GenerateChildren();
        }

        public override void GenerateChildren()
        {
            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                int[,] gridCopy = (int[,])grid.Clone();
                bool coinPlaced = gridController.AddCoinToColumn(ref gridCopy, x, id, out Vector2Int gridPosition, out bool win);

                if (coinPlaced)
                {
                    Node node = new OppositeNode(gridController, gridCopy, this, id == 1 ? 2 : 1, win);
                    node.column = x;

                    children.Add(node);
                }
            }
        }
    }

    public class OppositeNode : Node
    {
        public OppositeNode(GridController gridController, int[,] grid, Node parent, int id, bool win = false) 
            : base(gridController, grid, parent, id, win)
        {
            if (win)
                parent.value -= 10;
            else
                ;// GenerateChildren();
        }

        public override void GenerateChildren()
        {
            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                int[,] gridCopy = (int[,])grid.Clone();
                bool coinPlaced = gridController.AddCoinToColumn(ref gridCopy, x, id, out Vector2Int gridPosition, out bool win);

                if (coinPlaced)
                {
                    Node node = new ComputerNode(gridController, gridCopy, this, id == 1 ? 2 : 1, win);
                    node.column = x;

                    children.Add(node);
                }
            }
        }
    }
}
