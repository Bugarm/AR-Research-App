using UnityEngine;

public class UpdateDistanceLine : MonoBehaviour
{
    private Transform mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //mainCamera = GameObject.Find("SimulationCamera").transform;
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalData.CurrentDistance > 0)
        {
            //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, GlobalData.CurrentDistance * 100);
            transform.GetComponent<SpriteRenderer>().size = new Vector2(transform.GetComponent<SpriteRenderer>().size.x, GlobalData.CurrentDistance - 0.45f);
            transform.LookAt(mainCamera, transform.forward); // Make the line face the camera
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0); // Keep the line upright
        }
    }

}
