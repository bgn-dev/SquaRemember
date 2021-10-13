using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainGame : MonoBehaviour
{
    private Queue<int> sQ = new Queue<int>();
    private List<string> squares = new List<string>();

    [SerializeField] private GameObject square;
    [SerializeField] private GameObject TapToPlayAgainT;
    [SerializeField] private GameObject CanvasAttachedTo;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private int rows = 3;
    [SerializeField] private int cols = 3;
    [SerializeField] private float squareSize = 1;

    private int round = 1;
    private float speed = 1.0F;
    // keyword "static" makes this variable a member of the class, not of any particular instance
    public static int highScore = 0;
    public static bool game = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // all the frames are paused
        Time.timeScale = 0;
        GenerateGrid();
        StartCoroutine(startRound());
    }

    // Update is called once per frame
    void Update()
    {
        if (game)
        {
            Time.timeScale = 1;

            if (Input.GetMouseButtonDown(0))
            {
                var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D collider = Physics2D.OverlapPoint(worldPoint);

                if (collider != null && sQ.Count != 0)
                {
                    if (collider.gameObject.name == squares[sQ.Dequeue()])
                    {
                        StartCoroutine(changeColor(collider));
                        if (sQ.Count == 0)
                        {
                            round++;
                            scoreText.text = (round - 1).ToString();
                            StartCoroutine(startRound());
                            // set enabled to false, so the new round can be shown to the player without clicks getting accepted
                            enabled = false;
                        }
                    }
                    else
                    {
                        collider.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                        Handheld.Vibrate();
                        UpdateHighScore();

                        // Create a text after gameOver
                        GameObject playAgainT = (GameObject)Instantiate(TapToPlayAgainT);
                        // assign it to its parent
                        playAgainT.transform.SetParent(CanvasAttachedTo.transform);
                        // set right position
                        playAgainT.transform.localPosition = new Vector3(-3, -147, 0);

                        game = false;

                        StartCoroutine(waitForInput()); 
                    }
                }
            }
        }
    }

    private IEnumerator waitForInput()
    {
        // wait for the actual frame to end, since Input.GetMouseButtonDown stays for an whole frame true
        yield return null;
        while (!Input.GetMouseButtonDown(0))
        {
            // wait for the actual frame to end, avoid overloads
            yield return null;
        }
        // load the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;

    }

    public void UpdateHighScore()
    {
        if (round - 1 > highScore)
        {
            highScore = round - 1;
        }
    }

    private IEnumerator changeColor(Collider2D collider)
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

    private IEnumerator startRound()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < round; i++)
        {
            int rnd = Random.Range(0, 9);
            GameObject.Find(squares[rnd]).GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(speed);
            GameObject.Find(squares[rnd]).GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.2F);
            sQ.Enqueue(rnd);
        }
        // set enabled to true, so the player can select the squares
        enabled = true;

        speed = speed - Mathf.Sqrt(0.02F * 1/10);
    }

}
