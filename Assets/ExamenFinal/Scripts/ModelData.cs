using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[CreateAssetMenu(
    fileName = "NewModelData", 
    menuName = "AR/ModelData"
    )]
public class ModelData : ScriptableObject
{
    [Header("Informacion")]

    public string modelName;

    [TextArea(3, 6)]
    public string description;

    public string category;

    public int level;

    public Sprite icon;

    [Header("Modelo")]
    public GameObject prefab;

}
