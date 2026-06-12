using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

[System.Serializable]
public class ScanData
{
    public string ingredient;
    public float distance;
    public float confidence;
    public bool isAccurate;
}

[System.Serializable]
public class WrappingClass
{
    public List<ScanData> Inventory;
}

public class DataGather : MonoBehaviour
{
    [SerializeField] private int dataGatherCount = 10; // Number of times to gather data

    List<ScanData> scanDataList = new List<ScanData>();

    Coroutine dataGatherCoroutine;

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    private void GatherData()
    {
        // Example of gathering data from GlobalData and printing it to the console
        string ingredient = GlobalData.CurrentIngredient;
        float distance = GlobalData.CurrentDistance;
        float confidence = GlobalData.Confidence;

        scanDataList.Add(new ScanData
        {
            ingredient = ingredient,
            distance = distance,
            confidence = confidence,
            isAccurate = confidence > 0.8f // Example threshold for accuracy
        });
    }

    public void StartDataGathering()
    {
        if(dataGatherCoroutine == null)
        {
            dataGatherCoroutine = StartCoroutine(DelayData());
        }
    }

    public IEnumerator DelayData()
    {
        print("Starting data gathering...");
        int i = 0;
        while(i < dataGatherCount) // Example: gather data 10 times with a delay
        {
            print("start");
            yield return new WaitForSeconds(3f); // Wait for 3 seconds before gathering data again
            GatherData();
            i++;
        }
        PrintData();
        WriteData();
        dataGatherCoroutine = null; // Reset the coroutine reference after completion
    }

    private void WriteData()
    {
        print("writing");
        var path = $"{Application.persistentDataPath}/IngredientData.json";
        WrappingClass wrappingClass = new WrappingClass { Inventory = scanDataList };
        var json = JsonUtility.ToJson(wrappingClass, true); 
        
        File.WriteAllText(path, json);
    }

    private void PrintData()
    {
        foreach (var data in scanDataList)
        {
            print($"Ingredient: {data.ingredient}, Distance: {data.distance}, Confidence: {data.confidence}, Accurate: {data.isAccurate}");
        }
    }
}
