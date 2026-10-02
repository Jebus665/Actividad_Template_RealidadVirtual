using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Color_Script : MonoBehaviour
{
    public Material Amarillo;
    public Material Azul;

    public GameObject dimensionTextPrefab;

    private ARPlane plano;
    private MeshRenderer meshRenderer;
    private TextMeshPro dimensionText;

    void Awake()
    {
        plano = GetComponent<ARPlane>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        if (plano.alignment == PlaneAlignment.HorizontalUp)
        {
            meshRenderer.material = Amarillo;
        }
        else if (plano.alignment == PlaneAlignment.Vertical)
        {
            meshRenderer.material = Azul;
        }

        CrearTextoDimensiones();
    }

    void Update()
    {
        ActualizarDimensiones();
    }

    void CrearTextoDimensiones()
    {
        GameObject texto = Instantiate(
            dimensionTextPrefab,
            plano.transform
        );

        texto.transform.localPosition = Vector3.zero;
        texto.transform.localRotation = Quaternion.identity;

        dimensionText = texto.GetComponent<TextMeshPro>();
    }

    void ActualizarDimensiones()
    {
        if (dimensionText == null)
            return;

        float ancho = plano.size.x;
        float largo = plano.size.y;

        dimensionText.text = $"{ancho:F2} m × {largo:F2} m";
    }
}