using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadScaleDisable : MonoBehaviour
{
    public GameObject target;
    public Canvas canvas;
    public RectTransform sizeLeader;

    public Vector2 lastSize;

    private void Start()
    {
        lastSize = sizeLeader.sizeDelta;
    }

    private void Update()
    {
        if (lastSize != sizeLeader.sizeDelta)
        {
            target.transform.localScale = new Vector3(sizeLeader.rect.width, sizeLeader.rect.height, 1);
            lastSize = sizeLeader.sizeDelta;
        }

        if (canvas.enabled != target.activeSelf)
        {
            target.SetActive(canvas.enabled);
        }
    }
}
