using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] Button mainMenuBtn;
    [SerializeField] Button resumeBtn;
    [SerializeField] Button optionBtn;


    private void Awake()
    {
        resumeBtn.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        mainMenuBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionBtn.onClick.AddListener(() => {
            Hide();
            OptionUI.Instance.Show(Show);
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameResumed += KitchenGameManager_OnGameResumed;
        
        Hide();
    }

    public void KitchenGameManager_OnGamePaused(object sender, EventArgs e){
        Show();
    }
    public void KitchenGameManager_OnGameResumed(object sender, EventArgs e)
    {
        Hide(); 
    }
    private void Show()
    {
        gameObject.SetActive(true);
        resumeBtn.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
