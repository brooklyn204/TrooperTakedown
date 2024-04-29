using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] GameObject startPos;
    [SerializeField] GameObject endPos;

    [SerializeField] EnemyManager enemyManager;
    [SerializeField] Ahsoka player;

    [SerializeField] TextMeshProUGUI readyBanner;
    [SerializeField] TextMeshProUGUI goBanner;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI gameOverBanner;
    [SerializeField] TextMeshProUGUI victoryBanner;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI restartButtonText;
    [SerializeField] Button exitButton;
    [SerializeField] TextMeshProUGUI exitButtonText;

    [SerializeField] float alphaFadeSpeed = 0.01f;
    [SerializeField] float cameraZoomTime;
    [SerializeField] float timerMoveSpeed;
    [SerializeField] float timerPadding;
    [SerializeField] float gameEndPause;

    public float gameStartPause;
    public bool gameOver = false;
    public bool started = false;

    private RectTransform timerTransform;
    private RectTransform gameOverTransform;
    private bool playerWon;
    private float time;
    private float gameOverTime;
    private int minutes;
    private int seconds;
    private Image restartButtonImage;
    private Image exitButtonImage;
    private Vector2 timerEndPos;

    // Start is called before the first frame update
    void Start()
    {
        timerTransform = timer.GetComponent<RectTransform>();
        gameOverTransform = gameOverBanner.GetComponent<RectTransform>();
        timerEndPos = gameOverTransform.anchoredPosition;
        timerEndPos.y -= (gameOverTransform.rect.height / 2) + timerPadding; 
        restartButtonImage = restartButton.GetComponent<Image>();
        exitButtonImage = exitButton.GetComponent<Image>();

        started = false;
        IEnumerator start = StartGame();
        StartCoroutine(start);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        // If game is still going, update timer
        if (!gameOver && started)
        {
            minutes = (int)time / 60;
            seconds = (int)time % 60;
            timer.text = string.Format("{0,2:00}:{1,2:00}", minutes, seconds);
        }
    }

    private IEnumerator StartGame()
    {
        camera.transform.position = startPos.transform.position;
        Vector3 posChange = endPos.transform.position - startPos.transform.position;
        while (time < cameraZoomTime)
        {
            camera.transform.position += posChange * Time.deltaTime / cameraZoomTime;
            yield return null;
        }
        time = 0;
        float timePerStage = gameStartPause / 2;
        readyBanner.alpha = 1;
        while (time < timePerStage)
        {
            readyBanner.alpha -= Time.deltaTime / timePerStage;
            yield return null;
        }

        goBanner.alpha = 1;
        while (time < gameStartPause)
        {
            goBanner.alpha -= Time.deltaTime / timePerStage;
            yield return null;
        }

        time = 0;
        started = true;
    }
    
    private IEnumerator FadeInPlayerLost()
    {
        while (gameOverBanner.alpha < 1 || timerTransform.anchoredPosition != timerEndPos || exitButtonImage.color.a < 1 || restartButtonImage.color.a < 1)
        {
            // Fade in gameover banner
            if (gameOverBanner.alpha < 1)
            {
                gameOverBanner.alpha += alphaFadeSpeed * Time.deltaTime;
            }

            // Move timer to the center of the screen
            if (timerTransform.anchoredPosition != timerEndPos)
            {
                timerTransform.anchoredPosition = Vector2.MoveTowards(
                    timerTransform.anchoredPosition,
                    timerEndPos,
                    timerMoveSpeed * Time.deltaTime);
            }

            // Fade in restart button
            if (restartButtonImage.color.a < 1)
            {
                Color col = restartButtonImage.color;
                col.a += alphaFadeSpeed * Time.deltaTime;
                restartButtonImage.color = col;
                restartButtonText.alpha += alphaFadeSpeed * Time.deltaTime;
            }

            // Fade in exit button
            if (exitButtonImage.color.a < 1)
            {
                Color col = exitButtonImage.color;
                col.a += alphaFadeSpeed * Time.deltaTime;
                exitButtonImage.color = col;
                exitButtonText.alpha += alphaFadeSpeed * Time.deltaTime;
            }

            yield return null;
        }
    } 

    private IEnumerator FadeInPlayerWon()
    {
        while (victoryBanner.alpha < 1 || timer.alpha > 0 || exitButtonImage.color.a < 1 || restartButtonImage.color.a < 1)
        {
            // Fade in victory banner
            if (victoryBanner.alpha < 1)
            {
                victoryBanner.alpha += alphaFadeSpeed * Time.deltaTime;

            }

            // Fade out timer
            if (timer.alpha > 0)
            {
                timer.alpha -= alphaFadeSpeed * Time.deltaTime;
            }

            // Fade in restart button
            if (restartButtonImage.color.a < 1)
            {
                Color col = restartButtonImage.color;
                col.a += alphaFadeSpeed * Time.deltaTime;
                restartButtonImage.color = col;
                restartButtonText.alpha += alphaFadeSpeed * Time.deltaTime;
            }

            // Fade in exit button
            if (exitButtonImage.color.a < 1)
            {
                Color col = exitButtonImage.color;
                col.a += alphaFadeSpeed * Time.deltaTime;
                exitButtonImage.color = col;
                exitButtonText.alpha += alphaFadeSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }

    public void GameOver(bool won)
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverTime = Time.time;
            if (won)
            {
                playerWon = true;
                IEnumerator fadeIn = FadeInPlayerWon();
                StartCoroutine(fadeIn);
        }
            else
            {
                playerWon = false;
                Vector3 oldpos = timerTransform.position;
                timerTransform.anchorMin = gameOverTransform.anchorMin;
                timerTransform.anchorMax = gameOverTransform.anchorMax;
                timerTransform.pivot = gameOverTransform.pivot;
                timerTransform.position = oldpos;

                IEnumerator fadeIn = FadeInPlayerLost();
                StartCoroutine(fadeIn);

        }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
