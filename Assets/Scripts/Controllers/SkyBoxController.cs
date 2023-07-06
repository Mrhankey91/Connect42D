using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    private Material skyBoxMaterial;
    private float skyBoxRotationSpeed = 1f;

    void Awake()
    {
        skyBoxMaterial = RenderSettings.skybox;
    }

    void Update()
    {
        UpdateSkyBoxRotation();
    }

    private void UpdateSkyBoxRotation()
    {
        float skyBoxRotation = skyBoxMaterial.GetFloat("_Rotation");
        if (skyBoxRotation < 0f)
            skyBoxRotation += 360f;

        skyBoxMaterial.SetFloat("_Rotation", skyBoxRotation - skyBoxRotationSpeed * Time.deltaTime);
    }
}
