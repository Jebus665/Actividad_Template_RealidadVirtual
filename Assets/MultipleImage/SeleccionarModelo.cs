
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SeleccionarModelo : MonoBehaviour
{
    [Header("Referencias")]
    public Camera camaraAR;
    public TMP_Text mensaje;

    [Header("Configuracion")]
    public float distanciaMaxima = 100f;

    void Update()
    {
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 posicionToque =
                Touchscreen.current.primaryTouch.position.ReadValue();

            Seleccionar(posicionToque);
        }
    }

    void Seleccionar(Vector2 posicion)
    {
        Ray rayo = camaraAR.ScreenPointToRay(posicion);

        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, distanciaMaxima))
        {
            NombreModelo info =
                impacto.collider.GetComponentInParent<NombreModelo>();

            if (info != null)
            {
                mensaje.text = "Modelo seleccionado: " +
                               info.nombreModelo;
                mensaje.gameObject.SetActive(true);
            }
        }
    }
}

