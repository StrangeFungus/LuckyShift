using UnityEngine;

public class RotateShaderMaterialUV : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float rotateSpeed = 10f;

    private void Update()
    {
        float currentDegree = material.GetFloat("_RotateDegree");
        material.SetFloat("_RotateDegree", currentDegree += Time.deltaTime * rotateSpeed);
    }
}
