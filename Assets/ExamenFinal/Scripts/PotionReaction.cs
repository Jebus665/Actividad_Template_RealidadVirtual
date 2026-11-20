using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionReaction", menuName = "AR/ Potion Reaction")]
public class PotionReaction : ScriptableObject
{
    [Header("Required Potions")]
    public ModelData Pocion1;
    public ModelData Pocion2;

    [Header("Result")]
    public ModelData result;

    [Header("Interaction")]
    public float interactionDistance = .15f;
    public AudioClip reactionSound;

    public bool CanReact(ModelData a, ModelData b)
    {
        return (a == Pocion1 && b == Pocion2) || (a == Pocion2 && b == Pocion1);
    }
}
