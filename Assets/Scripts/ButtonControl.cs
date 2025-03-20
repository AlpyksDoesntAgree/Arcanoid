using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    private PlayerMove player;
    [SerializeField] private Animator LevelPanelAnimator;
    [SerializeField] private Animator SettingsAnimator;
    [SerializeField] private Text _currentAcc;
    private void Start()
    {
        string curAcc = PlayerPrefs.GetString("UserName");
        _currentAcc.text = $"Current account: {curAcc}"; 
        if(GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.Find("Player").GetComponent<PlayerMove>();
        LevelPanelAnimator.enabled = false;
        SettingsAnimator.enabled = false;
    }
    public void OpenLevelPanel()
    {
        LevelPanelAnimator.enabled = true;
        LevelPanelAnimator.SetBool("IsOpen", true);
    }

    public void CloseLevelPanel()
    {
        LevelPanelAnimator.SetBool("IsOpen", false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings()
    {
        SettingsAnimator.enabled = true;
        SettingsAnimator.SetBool("IsOpenSettings", true);
    }

    public void CloseSettings()
    {
        SettingsAnimator.SetBool("IsOpenSettings", false);
    }

    public void Pause()
    {
        if (Time.timeScale == 1)
        {
           Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void ChangeMovement(int movement)
    {
        PlayerPrefs.SetInt("Movement", movement);
        PlayerPrefs.Save();
        player.savedMovement = movement;
    }

    public void OffGame()
    {
        Application.Quit();
    }    
}
