using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageSpawner : MonoBehaviour
{
    [Header("AR")]
    [SerializeField] ARTrackedImageManager imageManager;

    [Header("AvailablePotions")]
    [SerializeField] List<ModelData> potions = new List<ModelData>();

    Dictionary<TrackableId, TrackedPotion> spawnedPotions = new Dictionary<TrackableId, TrackedPotion>();

    public IEnumerable<TrackedPotion> SpawnedPotions => spawnedPotions.Values; 

    private void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnImagesChanged); 
    }
    private void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnImagesChanged);
    }

    void OnImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach(ARTrackedImage addedimage in args.added)
        {
            RegisterImage(addedimage);
        }
        foreach(ARTrackedImage updatedimage in args.updated)
        {
            RegisterImage(updatedimage);
        }
        foreach (var removedimage in args.removed)
        {
            RemoveImage(removedimage.Key);
        }
    }
    void RegisterImage(ARTrackedImage image)
    {
        TrackableId id = image.trackableId;

        if(spawnedPotions.TryGetValue(id, out TrackedPotion existing))
        {
            existing.trackedImage = image;
            UpdateVisibility(existing);
            return;
        }

        string imageName = image.referenceImage.name;
        ModelData data = FindPotion(imageName);

        if (data == null || data.prefab == null)

            return;

        GameObject model = Instantiate(data.prefab, image.transform);

        TrackedPotion potion = new TrackedPotion(image, data, model);

        spawnedPotions.Add(id, potion);

        UpdateVisibility(potion);

        Debug.Log("Potion detected: " + data.modelName);

    }

    ModelData FindPotion(string imageName)
    {
        foreach(ModelData data in potions)
        {
            if(data != null && data.modelName == imageName)
            {
                return data;
            }
        }

        return null;
    }

    void RemoveImage(TrackableId id)
    {
        if(spawnedPotions.TryGetValue(id,out TrackedPotion potion))
        {
            potion.isRemoved = true;

            if(potion.instance != null)
            {
                Destroy(potion.instance);
            }

            spawnedPotions.Remove(id);
        }
    }
    void UpdateVisibility(TrackedPotion potion)
    {
        bool shouldBeVisible = potion.IsTracking() && !potion.isReacting;

        potion.SetVisible(shouldBeVisible);
    }

    public void SetPosionReacting(TrackedPotion potion, bool reacting)
    {
        potion.SetReacting(reacting);
        UpdateVisibility(potion);
    }
}
