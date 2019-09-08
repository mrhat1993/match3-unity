using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshRenderer MeshRenderer => _meshRenderer ? _meshRenderer : (_meshRenderer = GetComponentInChildren<MeshRenderer>());

    public void SetColor(Color color)
    {
        MeshRenderer.material.SetColor("_Color", color);
    }
}