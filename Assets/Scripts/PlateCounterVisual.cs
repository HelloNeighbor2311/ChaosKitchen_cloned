using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] Transform counterTopPoint;
    [SerializeField] Transform plateVisualPrefab;
    [SerializeField] PlateCounter plateCounter;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }
    private void Start()
    {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateRemove += PlateCounter_OnPlateRemove;
    }

    private void PlateCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float offsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, offsetY * plateVisualGameObjectList.Count,0);

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
    private void PlateCounter_OnPlateRemove(object sender, System.EventArgs e)
    {
       GameObject lastPlate = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(lastPlate);
        Destroy(lastPlate);
    }
}
