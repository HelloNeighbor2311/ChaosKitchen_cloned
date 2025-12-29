using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //The counter is empty
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            //The counter has something
            if (!player.HasKitchenObject())
            {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                //Player is carrying something
                if (player.GetKitchenObject().tryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is carrying a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        //Successfully added ingredient to plate
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //player is carrying something that is not a plate
                    if(GetKitchenObject().tryGetPlate(out PlateKitchenObject plateKitchenObjectOnCounter))
                    {
                        //Counter is carrying a plate
                        if (plateKitchenObjectOnCounter.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            //Successfully added ingredient to plate
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }

            }
        }
    }

   
}
