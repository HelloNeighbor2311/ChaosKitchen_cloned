using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjTrash;

    new public static void ResetStaticData()
    {
        OnAnyObjTrash = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnAnyObjTrash?.Invoke(this, EventArgs.Empty);
        }
    }
}
