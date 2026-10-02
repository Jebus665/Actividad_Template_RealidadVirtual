using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDetectorDebug : MonoBehaviour
{
    private ARPlaneManager planeManager;

    void Start()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        foreach (ARPlane plane in planeManager.trackables)
        {
            Debug.Log(
                "Plano detectado: " +
                plane.alignment +
                " | Centro: " +
                plane.center
            );
        }
    }
}