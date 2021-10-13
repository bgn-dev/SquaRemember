using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI HighScore;
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
        // set the bool true, so the void update
        MainGame.game = true;
        
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
