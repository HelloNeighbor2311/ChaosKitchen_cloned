using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]private Animator animator;
    private const string is_walking = "IsWalking";
    [SerializeField] Player player;


    
    private void Update()
    {
        animator.SetBool(is_walking, player.IsWalking());
    }
}
