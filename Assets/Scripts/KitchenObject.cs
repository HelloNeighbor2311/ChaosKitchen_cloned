using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitChenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    public void SetKitchenObjectParent(IKitChenObjectParent kitchen_Object_Parent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchen_Object_Parent;
        this.kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchen_Object_Parent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitChenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
     
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool tryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitChenObjectParent kitChenObjectParent)
    {
        Transform kitchenObject_Transform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObject_Transform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitChenObjectParent);
        return kitchenObject;
    }
}
