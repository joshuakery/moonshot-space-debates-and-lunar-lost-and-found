using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class AnimatedTextureUV : MonoBehaviour
{
    public int uvAnimationTileX = 24;
    public int uvAnimationTileY = 1;
    public float framesPerSecond = 10f;
    public Material material;

    private void Update()
    {
        int index = (int)(Time.time * framesPerSecond);
        index = index % (uvAnimationTileX * uvAnimationTileY);

        Vector2 size = new Vector2(1f / (float)uvAnimationTileX, 1f / (float)uvAnimationTileY);

        int uIndex = index % uvAnimationTileX;
        int vIndex = index / uvAnimationTileX;

        Vector2 offset = new Vector2((float)uIndex * size.x, 1f - (float)vIndex * size.y);

        material.SetTextureOffset("_FaceTex", offset);
        material.SetTextureScale ("_FaceTex", size);
    }

}
