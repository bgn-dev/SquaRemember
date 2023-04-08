using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainGame : MonoBehaviour
{
    private Queue<int> sQ = new Queue<int>();

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
    
    /* Start is called before the first frame update
    * @param timeScale: 0 the game is paused, 1 the game is in normal speed
    */
    void Start()
    {
        Time.timeScale = 0;
        GenerateGrid();
        StartCoroutine(StartRound());
    }

    /* Update is called once per frame
    * @param game boolean used to avoid the functionality of the method, until the game is started
    * @param worldPoint is a vector which is created by the input of the mouse
    * @param enabled boolean toggles the method Update
    */
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
                    if (collider.gameObject.name == sQ.Dequeue().ToString())
                    {
                        StartCoroutine(ChangeColor(collider.transform.gameObject.GetComponent<SpriteRenderer>(), Color.green, 1));
                        if (sQ.Count == 0)
                        {
                            round++;
                            scoreText.text = (round - 1).ToString();
                            StartCoroutine(StartRound());
                            enabled = false;
                        }
                    }
                    else
                    {
                        collider.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                        // Handheld.Vibrate();
                        UpdateHighScore();

                        // Create a text after the game is lost
                        GameObject playAgainT = (GameObject) Instantiate(TapToPlayAgainT);
                        // assign it to its parent canvas
                        playAgainT.transform.SetParent(CanvasAttachedTo.transform);
                        // set the right position
                        playAgainT.transform.localPosition = new Vector3(0, -152, 0);
                        enabled = false;
                        game = false;
                        StartCoroutine(WaitForInput()); 
                    }
                }
            }
        }
    }

    /* waitForInput waits for an input, after the game is lost
    * load the game scene, so its playable
    */
    private IEnumerator WaitForInput()
    {
        // wait for the actual frame to end, since Input.GetMouseButtonDown stays for an whole frame true
        yield return null;
        while (!Input.GetMouseButtonDown(0))
        {
            // wait for the actual frame to end, avoid overloads
            yield return null;
        }
        // wait 
        yield return new WaitForSeconds(0.2F);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }

    /* method is called, after the game is lost */
    public void UpdateHighScore()
    {
        if (round - 1 > highScore)
        {
            highScore = round - 1;
        }
    }

    /* method used to change the colors of the square
    * @param spriteRenderer get acces to the color of the square
    * @param sec past time between the color change
    */
    private IEnumerator ChangeColor(SpriteRenderer spriteRenderer, Color color, float sec)
    {
        spriteRenderer.color = color;
        yield return new WaitForSeconds(sec);
        spriteRenderer.color = Color.white;
    }

    /* GenerateGrid generates the grid
    * @param rows number of row
    * @param cols number of column
    * create the square as an GameObject
    * change the name of the created square
    * set the right position
    */
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

                square.name = i.ToString();
                square.transform.position = new Vector2(posX, posY);
                i++;
            }
        }
        Destroy(referenceSquare);

        float gridW = cols * squareSize;
        float gridH = rows * squareSize;
        transform.position = new Vector2(-gridW / 2 + squareSize / 2, gridH / 2 - squareSize / 2);
    }

    /* StartRound called at the beginning of the method Start() and after every succesfull round
    * @param squareAnzahl random number for the length of the round
    * @param iSquare number of the selected square
    * @param speed calculated the new speediness
    */
    private IEnumerator StartRound()
    {
        yield return new WaitForSeconds(1);
        int squareAnzahl = Random.Range(0, 5);
        for (int i = 0; i <= squareAnzahl; i++)
        {
            int iSquare = Random.Range(0, 9);
            StartCoroutine(ChangeColor(GameObject.Find(iSquare.ToString()).GetComponent<SpriteRenderer>(), Color.blue, speed));
            // wait until the method changeColor is "done", add 0.2ms for visibility if a square gets selected in mutilple times
            yield return new WaitForSeconds(speed + 0.2F);
            sQ.Enqueue(iSquare);
        }
        enabled = true;
        speed = speed - Mathf.Sqrt(0.02F * 1/10);
    }

}
