using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{ 
    [SerializeField] private int rows = 3;
    [SerializeField] private int cols = 3;
    [SerializeField] private float squareSize = 1;
    public GameObject square;
    Queue sQ = new Queue();
    List<string> squares = new List<string>();
    int round = 13;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        StartCoroutine(startRound());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(worldPoint);


            if (collider != null)
            {
                Debug.Log(collider.gameObject.GetInstanceID());
                collider.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }

        }
    }

    private void GenerateGrid()
    {
        GameObject referenceSquare = Instantiate(square);
        int i = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject square = (GameObject)Instantiate(referenceSquare, transform);

                

                float posX = col * squareSize;
                float posY = row * -squareSize;

                square.name = "S" + i;
                squares.Add("S" + i);
                square.transform.position = new Vector2(posX, posY);
                i++;
            }
        }
        Destroy(referenceSquare);

        float gridW = cols * squareSize;
        float gridH = rows * squareSize;
        transform.position = new Vector2(-gridW / 2 + squareSize / 2, gridH / 2 - squareSize / 2);
    }

    IEnumerator startRound()
    {
        for (int i = 0; i < round; i++)
        {
            int rnd = Random.Range(0, 9);
            GameObject.Find(squares[rnd]).GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(1);
            GameObject.Find(squares[rnd]).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
