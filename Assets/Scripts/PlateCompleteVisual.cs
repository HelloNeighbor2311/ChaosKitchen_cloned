using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [System.Serializable]
    public struct IngredientVisual
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    public List<IngredientVisual> ingredientVisualList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (var ingredientVisual in ingredientVisualList)
        {
           
            ingredientVisual.gameObject.SetActive(false);
        }
    }

    public void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var ingredientVisual in ingredientVisualList) { 
            if(ingredientVisual.kitchenObjectSO == e.kitchenObjectSO)
            {
                ingredientVisual.gameObject.SetActive(true);
            }
        }
    }
}
