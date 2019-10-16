using UnityEngine;
using UnityEditor;
using System.Collections;

public class AutoTileTexture : MonoBehaviour
{
    public float coeff = 0.5F;
    void OnDrawGizmos()
    {
        GetComponent<Renderer>().material.SetTextureScale(
            "_MainTex", new Vector2(gameObject.transform.lossyScale.x, gameObject.transform.lossyScale.z) * coeff
            );

    }
}