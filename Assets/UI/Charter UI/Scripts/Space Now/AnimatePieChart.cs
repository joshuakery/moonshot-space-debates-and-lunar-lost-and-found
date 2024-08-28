using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimatePieChart : MonoBehaviour
{
    public Transform slicesContainer;

    public UIAnimation uiAnimation;

    private float targetFill;

    public void SetTopSliceToZero()
    {
        Transform topSliceT = slicesContainer.GetChild(slicesContainer.childCount - 1);
        Image topSlice = topSliceT.gameObject.GetComponent<Image>();
        if (topSlice != null)
        {
            targetFill = topSlice.fillAmount;
            topSlice.fillAmount = 0;
        }
    }

    public void AnimateInTopSlice()
    {
        Transform topSliceT = slicesContainer.GetChild(slicesContainer.childCount - 1);
        Image topSlice = topSliceT.gameObject.GetComponent<Image>();
        if (topSlice != null)
        {
            Tween sliceTween = DOTween.To(
                () => topSlice.fillAmount,
                x => topSlice.fillAmount = x,
                targetFill,
                uiAnimation.duration
            );
            sliceTween.SetDelay(uiAnimation.delay);
            UITweener.SetEase(sliceTween, uiAnimation);
        }
    }
}
