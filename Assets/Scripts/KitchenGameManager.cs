using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{

    public static KitchenGameManager Instance { get; private set; }
    
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;

    private enum State{
        WaitingForStart,
        Countdown,
        Playing,
        GameOver
    }

    private State state;
    
    private float CountdownTimer = 3f;
    private float playingTimer = 0;
    private float playingTimerMax = 30f;
    private bool isPause = false;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingForStart;
    }
    private void Start()
    {
        GameInput.instance.onPauseAction += GameInput_OnPauseAction;
        GameInput.instance.onInteractAction += GameInput_OnInteractAction;
    }
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingForStart)
        {
            state = State.Countdown;
            OnStateChanged?.Invoke(this,  EventArgs.Empty);
        }
    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingForStart:
                
                break;
            case State.Countdown:
                CountdownTimer -= Time.deltaTime;
                if (CountdownTimer < 0f)
                {
                    playingTimer = playingTimerMax;
                    state = State.Playing;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                playingTimer -= Time.deltaTime;
                if (playingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break; 
        }
    }

    public bool isGamePlaying()
    {
        return state == State.Playing;
    }

    public bool isCountdown()
    {
        return state == State.Countdown;
    }
    public bool isGameOver()
    {
        return state == State.GameOver;
    }

    public float GetPlayingTimerNormalized()
    {
        return  (playingTimer/playingTimerMax);
    }
    public float CountdownToStartTimer()
    {
        return CountdownTimer;
    }

    public void TogglePauseGame()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0f;
            
            OnGamePaused?.Invoke(this, EventArgs.Empty );
        }
        else
        {
            Time.timeScale = 1f;
            
            OnGameResumed?.Invoke(this, EventArgs.Empty);
        }
    }
}
