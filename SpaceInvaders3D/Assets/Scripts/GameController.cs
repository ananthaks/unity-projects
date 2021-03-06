﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Asteroids,
    Fighters,
    AsteroidAndFighter,
    FighterFormation
};

[System.Serializable]
public class GunShipFormation
{
    public GameObject m_gunShip;
    public int m_numGunShips;
    public float m_gunShipGap;
    public Vector3 m_spawnPoint;
}

[System.Serializable]
public class FighterFormation
{
    public GameObject m_fighter;
    public int m_formationX;
    public int m_formationZ;
    public float m_formationGapX;
    public float m_formationGapZ;
    public Vector3 m_spawnPoint;
}

[System.Serializable]
public class RandomPickUpsSpawns
{
    public GameObject[] m_pickUpObjects;
    public int timeBetweenPickups;
    public Vector3 boundaryMinLimit;
    public Vector3 boundaryMaxLimit;
}

public enum PickUpState
{
    NoActivePickUp,
    PickUpSpawned,
    PickUpActive,
}

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles = new GameObject[4];
    [SerializeField] GameObject[] enemyFighters = new GameObject[4];
    [SerializeField] EnemyType enemyType;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] int obstacleCount;
    [SerializeField] int waveCount = 5;
    [SerializeField] float startWaitTime;
    [SerializeField] float spawnWaitTime;
    [SerializeField] float waveSpawnTime;

    [SerializeField] int maxPlayerHealth = 100;
    [SerializeField] GameObject playerHealthBar;

    [SerializeField] UnityEngine.UI.Text m_scoreText;
    [SerializeField] UnityEngine.UI.Text m_finalScoreText;
    [SerializeField] UnityEngine.UI.Text m_gameStatusText;

    [SerializeField] GunShipFormation m_gunShipFormation;
    [SerializeField] FighterFormation m_fighterFormation;
    [SerializeField] RandomPickUpsSpawns m_randomPickSpawns;

    [SerializeField] GameObject playerExplosion;
    [SerializeField] GameObject asteroidExplosion;
    [SerializeField] GameObject enemyExplosion;

    [SerializeField] float explosionLifeTime;

    private List<GunshipController> m_gunShipControllers = new List<GunshipController>();
    private EnemyController[,] m_enemyControllers;

    private GameObject m_finalMenuPanel;
    private GameObject m_pauseMenuPanel;

    private int m_currentScore = 0;
    private int m_currentEnemyCount;

    private string m_gameWinText = "YOU WON !!!";
    private string m_gameLoseText = "You Lost!";
    private string m_scoreDefaultText = "Score : ";

    private bool m_gameInProgress = false;
    private bool m_isGamePaused = false;

    private float pauseBreak = 1.0f;
    private float pauseTime = 0.0f;

    private bool isKeyDown = false;
    private bool isKeyUp = false;

    private PickUpState m_pickUpState;

    private int m_currentPlayerHealth;

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
        SpawnGunShips();

        m_currentPlayerHealth = maxPlayerHealth;
        if (playerHealthBar != null)
        {
            playerHealthBar.GetComponent<SimpleHealthBar>().UpdateBar(m_currentPlayerHealth, maxPlayerHealth);
        }

        if (enemyType == EnemyType.FighterFormation)
        {
            SpawnEnemyFormation();
            m_currentEnemyCount = m_fighterFormation.m_formationX * m_fighterFormation.m_formationZ;

            m_pickUpState = PickUpState.NoActivePickUp;

            if (m_randomPickSpawns != null)
            {
                StartCoroutine(SpawnRandomPickups());
            }
        }
        else
        {
            StartCoroutine(SpawnWaves());
        }
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

    private void SpawnGunShips()
    {
        if(m_gunShipFormation.m_gunShip == null)
        {
            return;
        }
        bool isEvenFormation = m_gunShipFormation.m_numGunShips % 2 == 0;
        Quaternion spawnRotation = Quaternion.identity;
        float offset = 0;

        for (int i = 1; i <= m_gunShipFormation.m_numGunShips; ++i)
        {
            if(isEvenFormation)
            {
                offset = (m_gunShipFormation.m_gunShipGap * (i - 1) / 2 + m_gunShipFormation.m_gunShipGap / 2) * (i % 2 == 0 ? 1 : -1);
            }
            else
            {
                offset = m_gunShipFormation.m_gunShipGap * (i / 2) * (i % 2 == 0 ? 1 : -1);
            }

            Vector3 spawnPoint = new Vector3
            (
            m_gunShipFormation.m_spawnPoint.x + offset,
            m_gunShipFormation.m_spawnPoint.y,
            m_gunShipFormation.m_spawnPoint.z
            );

            GameObject gameObject = Instantiate(m_gunShipFormation.m_gunShip, spawnPoint, spawnRotation) as GameObject;
            GunshipController controller = gameObject.GetComponent<GunshipController>();
            m_gunShipControllers.Add(controller);
        }
    }

    private void SpawnEnemyFormation()
    {
        if(m_fighterFormation != null)
        {
            m_enemyControllers = new EnemyController[m_fighterFormation.m_formationX, m_fighterFormation.m_formationZ];
            float offsetX = 0;
            float offsetZ = 0;

            for (int i = 1; i <= m_fighterFormation.m_formationX; ++i)
            {

                if (m_fighterFormation.m_formationX % 2 == 0)
                {
                    offsetX = (m_fighterFormation.m_formationGapX * (i - 1) / 2 + m_fighterFormation.m_formationGapX / 2) * (i % 2 == 0 ? 1 : -1);
                }
                else
                {
                    offsetX = m_fighterFormation.m_formationGapX * (i / 2) * (i % 2 == 0 ? 1 : -1);
                }

                for (int j = 1; j <= m_fighterFormation.m_formationZ; ++j)
                {

                    if (m_fighterFormation.m_formationZ % 2 == 0)
                    {
                        offsetZ = (m_fighterFormation.m_formationGapZ * (j - 1) / 2 + m_fighterFormation.m_formationGapZ / 2) * (j % 2 == 0 ? 1 : -1);
                    }
                    else
                    {
                        offsetZ = m_fighterFormation.m_formationGapZ * (j / 2) * (j % 2 == 0 ? 1 : -1);
                    }

                    Vector3 spawnPointLocal = new Vector3
                    (
                    m_fighterFormation.m_spawnPoint.x + offsetX,
                    m_fighterFormation.m_spawnPoint.y,
                    m_fighterFormation.m_spawnPoint.z + offsetZ
                    );

                    // Spawn it
                    Quaternion spawnRotation = Quaternion.identity;
                    GameObject newGameObject = Instantiate(m_fighterFormation.m_fighter, spawnPointLocal, spawnRotation) as GameObject;
                    m_enemyControllers[i - 1, j - 1] = newGameObject.GetComponent<EnemyController>();

                    m_enemyControllers[i - 1, j - 1].SetIndex(i - 1, j - 1);

                    if(j != 1)
                    {
                        m_enemyControllers[i - 1, j - 1].EnableDisableFire(false);
                    }
                }
            }
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

    public void OnPlayerPickUp(PickUpState pickupState)
    {
        m_pickUpState = pickupState;
    }

    public void OnPlayerHealth()
    {
        m_currentPlayerHealth = maxPlayerHealth;
        if (playerHealthBar != null)
        {
            playerHealthBar.GetComponent<SimpleHealthBar>().UpdateBar(m_currentPlayerHealth, maxPlayerHealth);
        }
    }

    public void OnPlayerHit(Collider hitCollider)
    {
        bool isHitSuccess = true;
        PlayerController playerController = hitCollider.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            isHitSuccess = playerController.OnPlayerHit();
        }

        Debug.Log("");

        if (!isHitSuccess)
        {
            return;
        }

        if (playerExplosion != null)
        {
            GameObject explosionObject = Instantiate(playerExplosion, hitCollider.gameObject.transform.position, hitCollider.gameObject.transform.rotation) as GameObject;
            Destroy(explosionObject, explosionLifeTime);
        }

        m_currentPlayerHealth -= 20;
        m_currentPlayerHealth = Mathf.Max(m_currentPlayerHealth, 0);

        if (playerHealthBar != null)
        {
            playerHealthBar.GetComponent<SimpleHealthBar>().UpdateBar(m_currentPlayerHealth, maxPlayerHealth);
        }

        if(m_currentPlayerHealth == 0)
        {
            OnPlayerDestroyed();
            Destroy(hitCollider.gameObject);
        }
    }

    public void OnGunShipHit(Collider hitCollider)
    {
        GunshipController controller = hitCollider.gameObject.GetComponent<GunshipController>();
        controller.OnShotHit();
    }

    public void OnEnemytHit(Collider hitCollider)
    {
        AddPoints(2);
        if (enemyExplosion != null)
        {
            GameObject explosionObject = Instantiate(enemyExplosion, hitCollider.gameObject.transform.position, hitCollider.gameObject.transform.rotation) as GameObject;
            Destroy(explosionObject, explosionLifeTime);
        }

        Destroy(hitCollider.gameObject);
        ResetFormation(hitCollider.gameObject.GetComponent<EnemyController>());
    }

    public void OnAsteroidHit(Collider hitCollider)
    {
        AddPoints(1);
        if (asteroidExplosion != null)
        {
            GameObject explosionObject = Instantiate(asteroidExplosion, hitCollider.gameObject.transform.position, hitCollider.gameObject.transform.rotation) as GameObject;
            Destroy(explosionObject, explosionLifeTime);
        }
        Destroy(hitCollider.gameObject);
    }


    public void ResetFormation(EnemyController enemyController)
    {
        if (enemyType == EnemyType.FighterFormation)
        {
            if(enemyController != null)
            {
                int indX = enemyController.GetIndexX();
                int indZ = enemyController.GetIndexZ();

                if(indX >= 0 && indZ >= 0 && indZ < (m_fighterFormation.m_formationZ - 1))
                {
                    m_enemyControllers[indX, indZ + 1].EnableDisableFire(true); 
                }
            }
             
            m_currentEnemyCount--;
            if(m_currentEnemyCount == 0)
            {
                OnPlayerWin();
            }
        }
    }

    public void FireAlternateGuns()
    {
        for(int i = 0; i < m_gunShipControllers.Count; ++i)
        {
            GunshipController controller = m_gunShipControllers[i];
            if(controller != null)
            {
                controller.FireWeapons();
            }
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
            m_pauseMenuPanel.SetActive(false);
        }
    }

    IEnumerator SpawnRandomPickups()
    {
        yield return new WaitForSeconds(startWaitTime);
        while(true)
        {
            if(m_pickUpState == PickUpState.NoActivePickUp)
            {
                int pickUpType = (int)(Random.Range(0, m_randomPickSpawns.m_pickUpObjects.Length));

                Vector3 spawnPosition = new Vector3(Random.Range(m_randomPickSpawns.boundaryMinLimit.x, m_randomPickSpawns.boundaryMaxLimit.x),
                    Random.Range(m_randomPickSpawns.boundaryMinLimit.y, m_randomPickSpawns.boundaryMaxLimit.y),
                    Random.Range(m_randomPickSpawns.boundaryMinLimit.z, m_randomPickSpawns.boundaryMaxLimit.z));

                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(m_randomPickSpawns.m_pickUpObjects[pickUpType], spawnPosition, spawnRotation);

                m_pickUpState = PickUpState.PickUpSpawned;
            }

            yield return new WaitForSeconds(m_randomPickSpawns.timeBetweenPickups);
        }
    }


    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWaitTime);
        for (int wave = 0; wave < waveCount; wave++)
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
        if(m_gameInProgress)
        {
            OnPlayerWin();
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
