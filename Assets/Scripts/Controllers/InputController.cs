using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private int numberColumns = 7;
    private float columnSize = 2;

    private int currentSelectedColumn = 0;

    public delegate void OnMouseClick();
    public OnMouseClick onMouseClick;

    public delegate void OnColumnChange(int last, int current);
    public OnColumnChange onColumnChange;

    private void Awake()
    {
        numberColumns = GetComponent<GridController>().GetGridSize().x;
    }

    private void Start()
    {
        columnSize = GetComponent<GridController>().GetDotSpace();
    }

    void Update()
    {
        UpdateSelectedColumn();

        if (Input.GetMouseButtonDown(0))
            onMouseClick?.Invoke();
    }

    private void UpdateSelectedColumn()
    {
        int lastColumn = currentSelectedColumn;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if(worldPosition.x >= 0f)
            currentSelectedColumn = (int)((worldPosition.x + columnSize/2f) / columnSize);
        else
            currentSelectedColumn = (int)((worldPosition.x - columnSize/2f) / columnSize);

        currentSelectedColumn += Mathf.FloorToInt(numberColumns / 2f);
        currentSelectedColumn = Mathf.Clamp(currentSelectedColumn, 0, numberColumns - 1); //Avoid negative number and number higher then columns count minus one. So value is in grid index
    
        if(lastColumn != currentSelectedColumn)
        {
            onColumnChange?.Invoke(lastColumn, currentSelectedColumn);
        }
    }
}
