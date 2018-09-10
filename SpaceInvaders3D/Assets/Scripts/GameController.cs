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

    private int m_currentScore = 0;
    private string m_scoreDefaultText = "Score : ";

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(SpawnWaves());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWaitTime);
        while(true)
        {
            for (int count = 0; count < obstacleCount; count++)
            {

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
        m_scoreText.text = m_scoreDefaultText + m_currentScore;
        Debug.Log(m_currentScore);
    }

    public void OnPlayerWin()
    {
        Debug.Log("Player Wins!");
    }

    public void OnPlayerDestroyed()
    {
        Debug.Log("Player Loses!");
    }

    public void QuitGame()
    {
        print("Quiting Game");
        Application.Quit();
    }
}
