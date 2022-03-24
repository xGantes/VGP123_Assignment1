using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager _instances = null;
    public GameObject playerPrefabs;

    [HideInInspector] public GameObject playerInstances;
    [HideInInspector] public Level currentLevel;
    [HideInInspector] public UnityEvent<int> onLifeEvent;
    [HideInInspector] public UnityEvent<int> onHealthEvent;
    [HideInInspector] public UnityEvent<int> onStaminaEvent;

    public static GameManager instances
    {
        get
        {
            return _instances;
        }
        set
        {
            _instances = value;
        }
    }

    int _lives = 1;
    int _stamina = 0;
    int _hp = 5;

    public int maxLives;
    public int maxStamina;
    public int maxHp;

    public int lives
    {
        get
        {
            return _lives;
        }
        set
        {
            if (_lives > value)
            {
                //Destroy(playerInstances);
                //spawnPlayer(currentLevel.spawnPoint);
            }

            _lives = value;
            onLifeEvent.Invoke(value);

            if (_lives > maxLives)
            {
                _lives = maxLives;
            }
            if (_lives <= -1)
            {
                //GameOver Scene
                SceneManager.LoadScene("GameOverScene");
            }
            Debug.Log("Lives set to: " + lives.ToString());
        }
    }
    public int stamina
    {
        get
        {
            return _stamina;
        }
        set
        {
            _stamina = value;
            onStaminaEvent.Invoke(value);

            if (_stamina > maxStamina)
            {
                _stamina = maxStamina;
            }
            Debug.Log("Stamina set to: " + stamina.ToString());
        }
    }
    public int health
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            
            if (_hp > maxHp)
            {
                _hp = maxHp;
            }
            Debug.Log("Health is set to: " + health.ToString());
            //SoundManager.soundInstances.audio.PlayOneShot(SoundManager.soundInstances.hurt);
            onHealthEvent.Invoke(value);
            if (_hp <= 0)
            {
                //Debug.Log("Died");
                //Destroy(gameObject);
                _hp = 10;
                lives--;

                Destroy(playerInstances);
                spawnPlayer(currentLevel.spawnPoint);
            }
        }
    }
    private void Start()
    {
        if (instances)
        {
            Destroy(gameObject);
        }
        else
        {
            instances = this;
            DontDestroyOnLoad(gameObject);
        }

        if (maxLives >= 0)
        {
            maxLives = 1;
        }
        if (maxHp >= 0)
        {
            maxHp = 10;
        }
        if (maxStamina >= 0)
        {
            maxStamina = 10;
        }
    }


    private void Update()
    {
        //test
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Damage(1);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Heal(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                            Application.Quit();
            #endif
        }
    }

    public void spawnPlayer(Transform spawnLocation)
    {
        playerInstances = Instantiate(playerPrefabs, spawnLocation.position, spawnLocation.rotation);
    }

    //public void Damage(int amount)
    //{
    //    if (amount < 0)
    //    {
    //        throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
    //    }
    //    health -= amount;

    //    if (health <= 0)
    //    {
    //        Debug.Log("Game Over");
    //        Destroy(gameObject);
    //    }
    //}
    //public void Heal(int amount)
    //{
    //    if (amount < 0)
    //    {
    //        throw new System.ArgumentOutOfRangeException("Cannot have negative heal");
    //    }
    //    bool isOverMaxHp = health + amount > maxHp;
    //    if (isOverMaxHp)
    //    {
    //        health = maxHp;
    //    }
    //    else
    //    {
    //        health += amount;
    //    }
    //}
}
