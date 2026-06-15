using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Simulation;

public class AR_Model : MonoBehaviour
{
    [SerializeField] private GameObject modelPrefab; 
    private ARTrackedImageManager raycastManager;
    private GameObject spawnedModel;
    [SerializeField] private TMP_Text distanceText;

    private GameObject simImage;

    private void OnEnable()
    {
        raycastManager = GetComponent<ARTrackedImageManager>();
        raycastManager.trackablesChanged.AddListener(OnImageChanged);
    }

    private void Start()
    {
        StartCoroutine(DelayCalculateDistance());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDistance();
    }

    IEnumerator DelayCalculateDistance()
    {
        yield return new WaitForSeconds(0.01f); // Adjust the delay as needed
        simImage = GameObject.FindGameObjectWithTag("Image");
    }

    private void CalculateDistance()
    {
        if (spawnedModel != null)
        {
            float distance = Vector3.Distance(Camera.main.transform.position, spawnedModel.transform.position);
            distanceText.text = $"Distance: {distance:F2} meters";
            GlobalData.CurrentDistance = distance; // Update the global distance variable
            if (Application.platform != RuntimePlatform.Android &&
            Application.platform != RuntimePlatform.IPhonePlayer)
            {
                GlobalData.AngleOfImage = spawnedModel.transform.eulerAngles; // Update the global angle variable
                if (simImage != null)
                { 
                    string name = simImage.GetComponent<SimulatedTrackedImage>().texture.name;

                    GlobalData.ImageLink = name; // Update the global image link variable
                }
            }
        }
    }

    public void OnImageChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (spawnedModel == null)
            {
                spawnedModel = Instantiate(modelPrefab, trackedImage.transform);
                spawnedModel.transform.localPosition = Vector3.zero;
                spawnedModel.transform.localRotation = Quaternion.identity;
                spawnedModel.transform.localScale = Vector3.one * 0.1f; // Adjust scale as needed
                print("Model instantiated at position: " + trackedImage.transform.position);
            }
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            if (spawnedModel != null)
            {
                spawnedModel.transform.position = trackedImage.transform.position;
                spawnedModel.transform.rotation = trackedImage.transform.rotation;
            }
        }
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedModel != null)
            {
                Destroy(spawnedModel);
                spawnedModel = null;
            }
        }
    }
}
