using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text HighScore;
    public void Start()
    {
        HighScore.text = PlayerPrefs.GetInt("Score", 0).ToString();
    }

    public void Update()
    {
        UpdateHighScore();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateHighScore()
    {
        if (MainGame.highScore > PlayerPrefs.GetInt("Score", 0))
        {
            HighScore.text = MainGame.highScore.ToString();
            PlayerPrefs.SetInt("Score", MainGame.highScore);
            PlayerPrefs.Save();
        }
    }
}
