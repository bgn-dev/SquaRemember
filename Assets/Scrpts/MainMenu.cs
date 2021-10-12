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
        //lädt Spielszene, Menü wird nicht entladen
        //SceneManager.LoadScene(1,LoadSceneMode.Additive);
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
