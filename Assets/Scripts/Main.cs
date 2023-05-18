using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Text coinText;
    public Image[] hearts;
    public Sprite isLife, nonLife;
    public GameObject PauseScreen;
    public GameObject InventoryScreen;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public float timer = 0f;
    public Text timeText;
    public TimeWork timeWork;
    public float countdown;
    public GameObject InventoryPan;
    public SoundEffector soundEffector;
    public AudioSource musicSource, soundSource;
    public bool inv = false;
    public void ReloadLevel()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Start()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        soundSource.volume = PlayerPrefs.GetFloat("SoundVolume");
        if ((int)timeWork == 2)
            timer = countdown;
    }
    public void Update()
    {
        coinText.text = player.GetCoins().ToString();
        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHP() > i)
                hearts[i].sprite = isLife;
            else
                hearts[i].sprite = nonLife;
        }
        if ((int)timeWork == 1)
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":");
        }
        else if ((int)timeWork == 2)
        {
            timer -= Time.deltaTime;
            /*timeText.text = timer.ToString("F2").Replace(",", ":");*/
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - ((int)timer / 60) * 60).ToString("D2");
            if (timer <= 0)
                Lose();
        }
        else
            timeText.gameObject.SetActive(false);
    }
    public void PauseOn()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        PauseScreen.SetActive(true);
    }
    public void PauseOff()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        PauseScreen.SetActive(false);
    }
    public void InventoryOn()
    {
        if (inv == false)
        {
            InventoryScreen.SetActive(true);
            inv = true;
        }
        else
        {
            InventoryScreen.SetActive(false);
            inv = false;
        }
    }
    public void Win()
    {
        soundEffector.PlayWinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);
        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);
        print(PlayerPrefs.GetInt("Lvl"));
        if (PlayerPrefs.HasKey("coins"))
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());
        else
            PlayerPrefs.SetInt("coins", player.GetCoins());
        print(PlayerPrefs.GetInt("coins"));

        InventoryPan.SetActive(false);
        GetComponent<Inventory>().RecountItems();
    }
    public void Lose()
    {
        soundEffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        LoseScreen.SetActive(true);
        InventoryPan.SetActive(false);
        GetComponent<Inventory>().RecountItems();
    }
    public void MenuLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene("Menu");
    }
    public void NextLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

public enum TimeWork
{ 
None,
Stopwatch,
Timer
}

