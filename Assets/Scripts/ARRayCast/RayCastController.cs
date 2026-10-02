using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RayCastController : MonoBehaviour
{
    [Header("Referencias AR")]
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;

    [Header("Modelos a instanciar")]
    [SerializeField] private GameObject horizontalModelPrefab;
    [SerializeField] private GameObject verticalModelPrefab;

    private GameObject selectedModel;

    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Update()
    {
        if (Touchscreen.current == null)
            return;

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            TryPlaceObject(touchPosition);
        }
    }

    public void SelectHorizontalModel()
    {
        selectedModel = horizontalModelPrefab;
        Debug.Log("Modelo horizontal seleccionado");
    }

    public void SelectVerticalModel()
    {
        selectedModel = verticalModelPrefab;
        Debug.Log("Modelo vertical seleccionado");
    }

    private void TryPlaceObject(Vector2 screenPosition)
    {
        if (selectedModel == null)
            return;

        if (!raycastManager.Raycast(
            screenPosition,
            hits,
            TrackableType.PlaneWithinPolygon))
            return;

        ARRaycastHit hit = hits[0];
        Pose hitPose = hit.pose;

        ARPlane plane = planeManager.GetPlane(hit.trackableId);

        if (plane == null)
            return;

        if (selectedModel == horizontalModelPrefab)
        {
            if (plane.alignment != PlaneAlignment.HorizontalUp &&
                plane.alignment != PlaneAlignment.HorizontalDown)
            {
                Debug.Log("El modelo horizontal solo puede colocarse en planos horizontales.");
                return;
            }
        }

        if (selectedModel == verticalModelPrefab)
        {
            if (plane.alignment != PlaneAlignment.Vertical)
            {
                Debug.Log("El modelo vertical solo puede colocarse en planos verticales.");
                return;
            }
        }

        Instantiate(
            selectedModel,
            hitPose.position,
            hitPose.rotation
        );
    }
}