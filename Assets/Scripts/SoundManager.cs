
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    public static SoundManager Instance { get; private set; }


    private const string SOUND_EFFECT_VOLUME = "SoundEffectVolume";
    private float volume = 1f;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(SOUND_EFFECT_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCuttingProgress += CuttingCounter_OnAnyCuttingProgress;
        Player.Instance.pickup += Player_Pickup;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlaceHere;
        TrashCounter.OnAnyObjTrash += TrashCounter_OnAnyObjTrash;
    }
    private void TrashCounter_OnAnyObjTrash(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position, volume);
    }
    private void BaseCounter_OnAnyObjectPlaceHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objDrop, baseCounter.transform.position, volume);
    }
    private void Player_Pickup(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objPickup, Player.Instance.transform.position);
    }
    private void CuttingCounter_OnAnyCuttingProgress(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position,volume);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position, volume);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position, volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier*volume);
    }
    public void Player_FootStepSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position);
    }
    public void CountdownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }
    public void playWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f) {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(SOUND_EFFECT_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

}
