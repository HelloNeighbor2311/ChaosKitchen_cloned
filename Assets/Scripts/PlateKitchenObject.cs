using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler <OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    private List<KitchenObjectSO> plateKitchenObjectSOList;
    [SerializeField]private List<KitchenObjectSO> validPlateKitchenObjectSOList;
   

    private void Awake()
    {
        plateKitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (validPlateKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            if (plateKitchenObjectSOList.Contains(kitchenObjectSO))
            {
                //Already has this ingredient
                return false;
            }
            else
            {
                plateKitchenObjectSOList.Add(kitchenObjectSO);

                OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
                {
                    kitchenObjectSO = kitchenObjectSO
                });
                return true;
            }
        }
        else
        {
            //Not a valid ingredient for this plate
            return false;
        }
    }
    public List<KitchenObjectSO> GetPlateKitchenObjectSOList()
    {
        return plateKitchenObjectSOList;
    }
}
