using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemove;

    [SerializeField] KitchenObjectSO plateKitchenObjectSO;
    private float spawnTimer;
    private float spawnTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedMaxAmount = 4;


    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnTimerMax)
        {
            spawnTimer = 0f;
            if (!HasKitchenObject())
            {
                if(KitchenGameManager.Instance.isGamePlaying() && platesSpawnedAmount < platesSpawnedMaxAmount)
                {
                    platesSpawnedAmount++;

                    OnPlateSpawned?.Invoke(this, EventArgs.Empty); 
                }
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemove?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
