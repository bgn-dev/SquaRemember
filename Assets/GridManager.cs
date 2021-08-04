using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int rows = 3;
    [SerializeField] private int cols = 3;
    [SerializeField] private float tileSize = 1;
    public GameObject square;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        GameObject referenceSquare = Instantiate(square);
        int i = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tile = (GameObject)Instantiate(referenceSquare, transform);

                float posX = col * tileSize;
                float posY = row * -tileSize;

                tile.name = "Square" + i;
                tile.transform.position = new Vector2(posX, posY);
                i++;
            }
        }
        Destroy(referenceSquare);

        float gridW = cols * tileSize;
        float gridH = rows * tileSize;
        transform.position = new Vector2(-gridW / 2 + tileSize / 2, gridH / 2 - tileSize / 2);
    }
}
