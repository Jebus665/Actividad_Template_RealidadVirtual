using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackerManager : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;
    private void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnChanged);
    }
    private void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnChanged);
    }
    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        //cuando la cam reconoce imagen por primera vez
        foreach(var newImage in eventArgs.added)
        {
            Debug.Log("ImagenDetectada: "+ newImage.referenceImage.name);
        }
        foreach(var updatedImage in eventArgs.updated)
        {
            switch (updatedImage.trackingState)
            {
                case TrackingState.Tracking:
                    Debug.Log("Imagen rastreada correctamente");
                    break;
                case TrackingState.Limited:
                    Debug.Log("Seguimiento Limitado");
                    break;
                case TrackingState.None:
                    Debug.Log("Imagen sin seguimiento");
                    break;
            }
        }
        foreach(var removedImage in eventArgs.removed)
        {
            TrackableId removedImageTrackableID = removedImage.Key;
            Debug.Log("imagen eliminada del tracking");
        }
    }
    void SetLibrary(ARTrackedImageManager imageManager, XRReferenceImageLibrary imageLibrary)
    {
        imageManager.referenceLibrary = imageLibrary;
    }

}
