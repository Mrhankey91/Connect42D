using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class CameraView
    {
        public string name;
        public Vector3 worldPosition;
    }
    public CameraView[] cameraViews;

    private Dictionary<string, Vector3> cameraViewsDict;

    private string current;
    private Vector3 targetPosition;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraViewsDict = ToDictionary(cameraViews);
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * 5f);
    }

    public void ChangeView(string view)
    {
        if(current != view)
        {
            current = view;
            targetPosition = cameraViewsDict[current];
        }
    }

    public Dictionary<string, Vector3> ToDictionary(CameraController.CameraView[] array)
    {
        Dictionary<string, Vector3> temp = new Dictionary<string, Vector3>();

        foreach (CameraView cv in array)
        {
            temp.Add(cv.name, cv.worldPosition);
        }

        return temp;
    }
}
