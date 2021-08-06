using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    private Queue<int> sQ = new Queue<int>();
    private List<string> squares = new List<string>();

    public GameObject square;

    public Text scoreText;

    [SerializeField] private int rows = 3;
    [SerializeField] private int cols = 3;
    [SerializeField] private float squareSize = 1;

    private int round = 1;
    private int score = 0;
    // keyword "static" makes this variable a member of the class, not of any particular instance
    public static int highScore = 0;

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
                if (collider.gameObject.name == squares[sQ.Dequeue()])
                {
                    StartCoroutine(changeColor(collider));
                    if (sQ.Count == 0)
                    {
                        round++;
                        score++;
                        scoreText.text = score.ToString();
                        StartCoroutine(startRound());
                    }
                }
                else
                {
                    collider.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    UpdateHighScore();
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

    public void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
        }
    }

    IEnumerator changeColor(Collider2D collider)
    {
        collider.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(1);
        collider.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
            GameObject.Find(squares[rnd]).GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(1);
            GameObject.Find(squares[rnd]).GetComponent<SpriteRenderer>().color = Color.white;
            sQ.Enqueue(rnd);
        }
    }
}
