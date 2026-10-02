using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDimensions : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private GameObject dimensionTextPrefab;

    private void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (ARPlane plane in args.added)
        {
            CreateDimensionText(plane);
        }

        foreach (ARPlane plane in args.updated)
        {
            UpdateDimensionText(plane);
        }
    }

    private void CreateDimensionText(ARPlane plane)
    {
        GameObject textObject = Instantiate(
            dimensionTextPrefab,
            plane.transform
        );

        textObject.transform.localPosition = Vector3.zero;
        textObject.transform.localRotation = Quaternion.identity;

        textObject.name = "Dimensiones";

        UpdateDimensionText(plane);
    }

    private void UpdateDimensionText(ARPlane plane)
    {
        Transform textTransform = plane.transform.Find("Dimensiones");

        if (textTransform == null)
            return;

        TextMeshPro text = textTransform.GetComponent<TextMeshPro>();

        if (text == null)
            return;

        float width = plane.size.x;
        float height = plane.size.y;

        text.text = $"{width:F2} m × {height:F2} m";
    }
}