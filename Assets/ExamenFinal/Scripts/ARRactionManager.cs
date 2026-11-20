using System.Collections.Generic;
using UnityEngine;

public class ARRactionManager : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private ARImageSpawner imageSpawner;

    [Header("Available Reactions")]
    [SerializeField] List<PotionReaction> reactions = new List<PotionReaction>();

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    GameObject resultInstance;

    TrackedPotion reactingA;
    TrackedPotion reactingB;

    bool hasInteracted = false;

    private void Update()
    {
        if(hasInteracted)
        {
            if(reactingA == null || reactingB == null || !reactingA.IsTracking() || !reactingB.IsTracking())
            {
                CancelReaction();
                return;
            }
            return;
        }

        List<TrackedPotion> visiblePotions = new List<TrackedPotion>();

        foreach(TrackedPotion potion in imageSpawner.SpawnedPotions)
        {
            if(potion.IsTracking() && !potion.isReacting)
            {
                visiblePotions.Add(potion);
            }
        }

        for(int i = 0; i < visiblePotions.Count; i++)
        {
            for(int j = i + 1; j < visiblePotions.Count; j++)
            {
                if (CheckReaction(visiblePotions[i], visiblePotions[j]))
                    return;
            }
        }
    }

    bool CheckReaction(TrackedPotion a, TrackedPotion b)
    {
        foreach(PotionReaction reaction in reactions)
        {
            if (reaction == null)
                continue;

            if (!reaction.CanReact(a.data, b.data))
                continue;

            float distance = Vector3.Distance(a.GetPosition(), b.GetPosition());

            if (distance > reaction.interactionDistance)
                continue;

            if (reaction.result == null || reaction.result.prefab == null)
                continue;

            ExecuteReaction(a, b, reaction);

            return true;
        }

        return false;
    }

    void ExecuteReaction(TrackedPotion a, TrackedPotion b, PotionReaction reaction)
    {
        hasInteracted = true;

        reactingA = a;
        reactingB = b;

        imageSpawner.SetPosionReacting(a, true);
        imageSpawner.SetPosionReacting(b, true);

        Vector3 midPoint = (a.GetPosition() + b.GetPosition()) / 2f;

        resultInstance = Instantiate(reaction.result.prefab, midPoint, Quaternion.identity);

        ModelInfo info = resultInstance.GetComponent<ModelInfo>();

        if(info != null) 
        {
            info.data = reaction.result;
        }

        if(audioSource  != null && reaction.reactionSound != null)
        {
            audioSource.PlayOneShot(reaction.reactionSound);
        }    

        Debug.Log($"Reaction: {a.data.modelName} + {b.data.modelName} = {reaction.result.modelName}");
    }

    void CancelReaction()
    {
        if(resultInstance != null)
        {
            Destroy(resultInstance);
            resultInstance = null;
        }

        if(reactingA != null) 
        {
                imageSpawner.SetPosionReacting(reactingA, false);

        }
        if(reactingB != null) 
        {
                imageSpawner.SetPosionReacting(reactingB, false);

        }
        
        reactingA = reactingB = null;

        hasInteracted = false;

        Debug.Log("Reaction cancelled");
    }

    public void ResetReaction()
    {
        if(hasInteracted)
        {
            CancelReaction();
        }
    }
}
