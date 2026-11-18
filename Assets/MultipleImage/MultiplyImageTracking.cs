using NUnit.Framework;
using System.Net.Http.Headers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
public class MultiplyImage : MonoBehaviour
{
    [Serializable]
    public class ImagePrefab
    {
        public string imageName;
        public GameObject imagePrefab;
    }
    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private List<ImagePrefab> imagePrefab;

    private Dictionary<TrackableId, GameObject> spawnedObjects = new Dictionary<TrackableId, GameObject>();

    private void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnImagesChanged);
    }
    private void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnImagesChanged);
    }
    private void OnImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage>args)
    {
        foreach(var image in args.added)
        {
            foreach (var item in imagePrefab)
            {
                if(image.referenceImage.name==item.imageName && item.imagePrefab != null)
                {
                    GameObject InstancedPrefab = Instantiate(item.imagePrefab, image.transform);

                    InstancedPrefab.transform.localPosition = Vector3.zero;
                    //InstancedPrefab.transform.localRotation=Quaternion.identity;
                    spawnedObjects.Add(image.trackableId, InstancedPrefab);
                    
                    InstancedPrefab.SetActive(image.trackingState == TrackingState.Tracking);
                    break;
                }
            }
        }
        foreach(var image in args.updated)
        {
            if(spawnedObjects.TryGetValue(image.trackableId, out GameObject InstancedPrefab))
            {
                InstancedPrefab.SetActive(image.trackingState == TrackingState.Tracking);
            }
        }
        foreach(var removed in args.removed)
        {
            if(spawnedObjects.TryGetValue(removed.Key, out GameObject InstancePrefab))
            {
                Destroy(InstancePrefab);
                spawnedObjects.Remove(removed.Key);
            }
        }
    }
}
