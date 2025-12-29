using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator animator;
    private int previousCountdownNumber;
    private const string Number_PopUp = "NumberPopUp";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    public void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.isCountdown())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.CountdownToStartTimer());
        countdownText.text = countdownNumber.ToString() ;
        if (previousCountdownNumber != countdownNumber) { 
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(Number_PopUp);
            SoundManager.Instance.CountdownSound();

        }
    }

    private void Show() { 
        gameObject.SetActive(true);
    }
    private void Hide() { 
        gameObject.SetActive(false);
    }
}
