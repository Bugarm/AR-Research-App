using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;

[System.Serializable]
public class ScanData
{
    public string inControlledEnvironment;
    public string imageLink;
    public string expectedIngredient;
    public string scannedIngredient;
    public float distance;
    public float angleOfScan;
    public Vector3 angleOfImage;
    public float confidence;
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

    string inControlledEnv = "No";

    int dataCount = 0;

    private void Start()
    {
        //Debug.Log(Application.persistentDataPath);

        if(Application.isEditor)
        {
            inControlledEnv = "Yes";
        }
    }

    private void GatherData()
    {
        // Example of gathering data from GlobalData and printing it to the console
        string imageLink = GlobalData.ImageLink;
        string expectedIngredient = GlobalData.ExpectedIngredient;
        string ingredient = GlobalData.CurrentIngredient;
        float distance = GlobalData.CurrentDistance;
        float confidence = GlobalData.Confidence;
        float angleOfScan = GlobalData.AngleOfScan;
        Vector3 angleOfImage = Vector3.zero;
        if (Application.isEditor)
        {
            angleOfImage = GlobalData.AngleOfImage;
        }

        scanDataList.Add(new ScanData
        {
            imageLink = imageLink,
            inControlledEnvironment = inControlledEnv,
            expectedIngredient = expectedIngredient,
            scannedIngredient = ingredient,
            distance = distance,
            angleOfScan = angleOfScan,
            angleOfImage = angleOfImage,
            confidence = confidence,
        });
    }

    public void StartDataGathering()
    {

        if(dataGatherCoroutine == null)
        {
            ClearData(); // Clear previous data before starting new gathering
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
        //PrintData();
        WriteData();
        dataGatherCoroutine = null; // Reset the coroutine reference after completion
    }

    private void WriteData()
    {
        print("writing");
        var path = $"{Application.persistentDataPath}/IngredientData_{dataCount}.json";
        WrappingClass wrappingClass = new WrappingClass { Inventory = scanDataList };
        var json = JsonUtility.ToJson(wrappingClass, true); 
        
        File.WriteAllText(path, json);
        dataCount++;
    }

    public void ClearData()
    {
        scanDataList.Clear();
        print("Data cleared.");
    }

    public void ExpectedIngredientInput(TMP_InputField inputField)
    { 
        GlobalData.ExpectedIngredient = inputField.text; 
        print($"Expected Ingredient set to: {GlobalData.ExpectedIngredient}");
    }

    private void PrintData()
    {
        foreach (var data in scanDataList)
        {
            print($"Expected Ingredient: {data.expectedIngredient}, Scanned Ingredient: {data.scannedIngredient}, Distance: {data.distance}, Confidence: {data.confidence}, Angle of Scan: {data.angleOfScan}");
        }
    }
}
