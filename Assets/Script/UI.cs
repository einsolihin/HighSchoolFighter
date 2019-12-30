using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public GameObject endGamePanel;
    public GameObject btnPause;
    public GameObject player;
    public GameObject enemy;
    public GameObject Hurt;
    public GameObject HealthBar;
    public GameObject HealthBackGround;
    public List<GameObject> enemyList;
    public Transform posA;
    public Transform posB;
    bool Phurt = false;

    
    Vector2 HPposition;
    float MaxHealth = 100;
    Image ImGHealth;
    float currentTime = 0;

    float timeCount = 1f;
    float playerHealth;
    // Use this for initialization

    
    void Start () {
        endGamePanel.SetActive(false);
        btnPause.SetActive(true);
        ImGHealth = HealthBar.GetComponent<Image>();
        MaxHealth = player.GetComponent<Player>().playerHealth;
        playerHealth = player.GetComponent<Player>().playerHealth;
        HPposition = HealthBackGround.transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.P))
            PlayerHurt();
        if (currentTime < 0)
        {
            Phurt = false;
        }
        else
        {
            StartCoroutine(ShakeEffect());
            currentTime -= Time.deltaTime;
        }
        
        if (playerHealth <= 0)
        {
            player.GetComponent<Player>().Play("KO");
            EndGame();
        }
        if (!EnemyAlive())
            SpawnEnemy();

        Hurt.GetComponent<Animator>().SetFloat("PlayerHp", playerHealth);

        

    }

    #region EnemySystem
    //Enemy System
    bool EnemyAlive()
    {
        timeCount -= Time.deltaTime;
        if(timeCount<0)
        {
            timeCount = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        return true;
    }

    void SpawnEnemy()
    {
        int a = Random.Range(3, 5);
        for (int i = 0; i < a; i++)
        {
            StartCoroutine(SpawnWave(0.2f,i));
        }
    }

    IEnumerator SpawnWave(float time, int i)
    {
        yield return new WaitForSeconds(time);
        enemyList.Add(enemy);
        if (Random.value > 0.5)
        {
            Instantiate(enemyList[i], posA.transform);

        }
        else
        {
            Instantiate(enemyList[i], posB.transform);
        }
        enemyList[i].GetComponent<EnemyBehavior>().player = player;
    }
    #endregion

    #region ButtonSystem
    //Button System
    public void quitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1",LoadSceneMode.Single);
    }

    public void MenuGame()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void EndGame()
    {
        endGamePanel.SetActive(true);
        btnPause.SetActive(false);
    }

    #endregion

    #region PlayerSystem
    //Player System
    
    public void PlayerHurt()
    {
        playerHealth = player.GetComponent<Player>().playerHealth;
        ImGHealth.fillAmount = player.GetComponent<Player>().playerHealth / MaxHealth;
        Phurt = true;
        currentTime = 1;
        Hurt.GetComponent<Animator>().SetTrigger("Hurt");
    }

    public IEnumerator ShakeEffect()
    {
        float newPosX = HealthBackGround.transform.position.x + Random.Range(-5, 5);
        float newPosY = HealthBackGround.transform.position.y + Random.Range(-5, 5);
        HealthBackGround.transform.position = new Vector2(newPosX, newPosY);
        yield return new WaitForSeconds(Time.deltaTime);
        HealthBackGround.transform.position = HPposition;
    }
    #endregion
}
