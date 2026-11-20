using UnityEngine;

public class ModelInfo : MonoBehaviour
{
    
    //[SerializeField] private string modelName;
    //public string ModelName => modelName;

    [SerializeField] private ModelData modelData;
    public ModelData data { get => modelData; set => modelData = value; }
}
