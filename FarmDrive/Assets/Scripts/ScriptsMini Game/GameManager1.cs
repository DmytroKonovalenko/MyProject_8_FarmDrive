using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance;

    public float slowDownFactor = 10f;
    public float slowMotionTime = 1f;
    public bool gameOver = false;
    public bool freezeTiles = false;
    public bool gameEnded = false;

    private int _score = 0;
    public GameObject panel_loading;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        gameOver = true;
        freezeTiles = true;
        PlayerPrefs.GetInt("TopScore_zigzag", 0);
        gameEnded = false;
        UIManager1.instance.ArriveIntoGame();
    }

    public void StartGame()
    {
        gameOver = false;
        _score = 0;
    }

    public void IncrementScore(int increment)
    {
        _score += increment;
        UIManager1.instance.UpdateScoreText(this._score);
    }

    public void EndGame()
    {
        gameOver = true;
        UIManager1.instance.ScoreFadeOut();
        TopScore();
        StartCoroutine(SlowDownAndStop());
    }

    private void TopScore()
    {
        if (_score > PlayerPrefs.GetInt("TopScore_zigzag", 0))
        {
            PlayerPrefs.SetInt("TopScore_zigzag", _score);
        }
    }

    private IEnumerator SlowDownAndStop()
    {
        Time.timeScale = 1 / slowDownFactor;
        Time.fixedDeltaTime = Time.fixedDeltaTime / slowDownFactor;

        yield return new WaitForSeconds(slowMotionTime / slowDownFactor);

        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowDownFactor;
        freezeTiles = true;
        gameEnded = true;

        UIManager1.instance.SetFinalScoreBoard(_score, PlayerPrefs.GetInt("TopScore_zigzag", 0));
    }

    public int GetBestScore()
    {
        return PlayerPrefs.GetInt("TopScore_zigzag", 0);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void click_menu()
    {
        panel_loading.SetActive(true); 
        SceneManager.LoadSceneAsync(0 , LoadSceneMode.Single);
    }
}
