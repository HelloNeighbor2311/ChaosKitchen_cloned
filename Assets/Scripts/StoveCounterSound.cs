using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private StoveCounter stoveCounter;
    private float warningSoundTimer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStageChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }
    public void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        bool show = stoveCounter.isFried() && e.progressNormalized > burnShowProgressAmount;
    }

    public void StoveCounter_OnStageChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound) {
            audioSource.Play();

        }
        else
        {
            audioSource.Pause();
        }

        
    }

    private void Update()
    {
        warningSoundTimer -=Time.deltaTime;
        if(warningSoundTimer < 0)
        {
            float warningSoundTimerMax = .2f;
            warningSoundTimer = warningSoundTimerMax;
            SoundManager.Instance.playWarningSound(stoveCounter.transform.position);
        }
    }
}
