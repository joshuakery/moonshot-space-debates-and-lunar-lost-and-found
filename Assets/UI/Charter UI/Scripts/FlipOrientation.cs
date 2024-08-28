using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipOrientation : MonoBehaviour
{
    public RectTransform rt;

    private void Awake()
    {
        if (rt == null) { rt = GetComponent<RectTransform>(); }
    }

    public void Flip()
    {
        float zRotation = rt.rotation.z == 0 ? 180 : 0;
        rt.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }
}
