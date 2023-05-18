using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    public Button[] lvls;
    public Text coinText;
    public Slider musicSlider, soundsSlider;
    // Start is called before the first frame update
    void Start()
    {
        
        if (PlayerPrefs.HasKey("Lvl"))
            for (int i = 0; i < lvls.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                    lvls[i].interactable = true;
                else
                    lvls[i].interactable = false;
            }
        if (!PlayerPrefs.HasKey("hp"))
            PlayerPrefs.SetInt("hp", 0);

        if (!PlayerPrefs.HasKey("bg"))
            PlayerPrefs.SetInt("bg", 0);

        if (!PlayerPrefs.HasKey("rg"))
            PlayerPrefs.SetInt("rg", 0);

        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        if (!PlayerPrefs.HasKey("SoundVolume"))
            PlayerPrefs.SetFloat("SoundVolume", 0.5f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundsSlider.value = PlayerPrefs.GetFloat("SoundVolume");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", soundsSlider.value);
        if (PlayerPrefs.HasKey("coins"))
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        else
            coinText.text = "0";
    }
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void DelKeys()
    {
        PlayerPrefs.DeleteAll();
        print(PlayerPrefs.GetInt("Lvl"));
    }
    public void Buy_hp(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }
    public void Buy_bg(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("bg", PlayerPrefs.GetInt("bg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }
    public void Buy_rg(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("rg", PlayerPrefs.GetInt("rg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }
}
