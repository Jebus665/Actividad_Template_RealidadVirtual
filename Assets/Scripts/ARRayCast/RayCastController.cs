using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using TMPro;

public class RaycastController : MonoBehaviour
{
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] GameObject objectToPlace;
    [SerializeField] TMP_Text debugText;

    List<ARRaycastHit> hits = new List<ARRaycastHit> ();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        var activeTouches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
        if (activeTouches.Count == 0)
        {
            debugText.text = "Esperando touch . . .";
            return;
        }
        var touch = activeTouches[0];

        Debug.Log("Touch Detectado");
        debugText.text = "Touch Detectado";

        if (touch.phase != UnityEngine.InputSystem.TouchPhase.Began)
            return;

        if (raycastManager.Raycast(activeTouches[0].screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Debug.Log("Hit Encontrado");
            Pose hitPose = hits[0].pose;
            debugText.text = "Hit: " + hitPose.position;
            Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
        }
        else
        {
            Debug.Log("No se encontro plano");
            debugText.text = "No Hit";
        }

    }
}
