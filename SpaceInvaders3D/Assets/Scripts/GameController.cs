using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Asteroids,
    Fighters,
    AsteroidAndFighter,
};

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles = new GameObject[4];
    [SerializeField] GameObject[] enemyFighters = new GameObject[4];
    [SerializeField] EnemyType enemyType;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] int obstacleCount;
    [SerializeField] float startWaitTime;
    [SerializeField] float spawnWaitTime;
    [SerializeField] float waveSpawnTime;

    public UnityEngine.UI.Text m_scoreText;
    public UnityEngine.UI.Text m_finalScoreText;
    public UnityEngine.UI.Text m_gameStatusText;

    public GameObject m_finalMenuPanel;
    public GameObject m_pauseMenuPanel;

    private int m_currentScore = 0;

    private string m_gameWinText = "YOU WON !!!";
    private string m_gameLoseText = "You Lost! Noob!";
    private string m_scoreDefaultText = "Score : ";

    private bool m_gameInProgress = false;
    private bool m_isGamePaused = false;

    private float pauseBreak = 1.0f;
    private float pauseTime = 0.0f;

    private bool isKeyDown = false;
    private bool isKeyUp = false;

    // Use this for initialization
    void Start ()
    {
        m_gameInProgress = true;
        m_isGamePaused = false;
        m_currentScore = 0;
        m_finalMenuPanel = GameObject.FindWithTag("MainMenuPanel");
        m_pauseMenuPanel = GameObject.FindWithTag("PauseMenuPanel");

        if (m_pauseMenuPanel != null)
        {
            m_pauseMenuPanel.SetActive(false);
        }
        if(m_finalMenuPanel != null)
        {
            m_finalMenuPanel.SetActive(false);
        }

        isKeyDown = false;
        isKeyUp = false;

        StartCoroutine(SpawnWaves());
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isKeyDown = true;
        }

        if (Input.GetKeyUp(KeyCode.Escape) && isKeyDown)
        {
            isKeyUp = true;
            isKeyDown = false;
        }

        if(isKeyUp && !isKeyDown)
        {
            EscPressed();
            isKeyUp = false;
        }
    }

    private void EscPressed()
    {
        if (m_isGamePaused)
        {
            ContinueGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if(m_isGamePaused)
        {
            return; // To prevent multiple calls
        }

        m_isGamePaused = true;
        if(m_pauseMenuPanel != null)
        {
            m_pauseMenuPanel.SetActive(true);
        }
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        if(!m_isGamePaused)
        {
            return;
        }

        Time.timeScale = 1;
        m_isGamePaused = false;
        if (m_pauseMenuPanel != null)
        {
            Debug.Log("Coming Here 2");
            m_pauseMenuPanel.SetActive(false);
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWaitTime);
        while(true)
        {
            for (int count = 0; count < obstacleCount; count++)
            {
                if(!m_gameInProgress)
                {
                    break;
                }

                if(enemyType == EnemyType.Asteroids)
                {
                    int asteroidType = (int)(Random.Range(0, 3));
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnPoint.x, spawnPoint.x), Random.Range(-spawnPoint.y, spawnPoint.y), spawnPoint.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(obstacles[asteroidType], spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWaitTime);
                }
                else if(enemyType == EnemyType.Fighters)
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnPoint.x, spawnPoint.x), Random.Range(-spawnPoint.y, spawnPoint.y), spawnPoint.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(enemyFighters[0], spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWaitTime);
                }
                else if (enemyType == EnemyType.AsteroidAndFighter)
                {
                    bool isFighter = (Random.value > 0.5);
                    int asteroidType = (int)(Random.Range(0, 3));
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnPoint.x, spawnPoint.x), Random.Range(-spawnPoint.y, spawnPoint.y), spawnPoint.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(isFighter ? enemyFighters[0] : obstacles[asteroidType], spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWaitTime);
                }
            }
            if(!m_gameInProgress)
            {
                break;
            }
            yield return new WaitForSeconds(waveSpawnTime);
        }
    }
    public void AddPoints(int points)
    {
        m_currentScore += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if(m_gameInProgress)
        {
            m_scoreText.text = m_scoreDefaultText + m_currentScore;
            Debug.Log(m_currentScore);
        }
    }

    public void OnPlayerWin()
    {
        m_finalMenuPanel.SetActive(true);
        m_finalScoreText.text = m_scoreDefaultText + m_currentScore;
        m_gameStatusText.text = m_gameWinText;
        m_gameInProgress = false;
    }

    public void OnPlayerDestroyed()
    {
        m_finalMenuPanel.SetActive(true);
        m_finalScoreText.text = m_scoreDefaultText + m_currentScore;
        m_gameStatusText.text = m_gameLoseText;
        m_gameInProgress = false;
    }

}
