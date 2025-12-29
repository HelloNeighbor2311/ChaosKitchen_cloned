using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject 
{
    public AudioClip[] chop;
    public AudioClip[] footstep;
    public AudioClip[] deliverySuccess;
    public AudioClip[] deliveryFail;
    public AudioClip stoveSizzle;
    public AudioClip[] trash;
    public AudioClip[] objDrop;
    public AudioClip[] objPickup;
    public AudioClip[] warning;
}
