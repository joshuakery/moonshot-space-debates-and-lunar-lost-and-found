using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlexibleUIIcon : FlexibleUI
{
    Image image;
    public ImageColor imageColor;

    public enum ImageColor
    {
        White,
        Black,
        Primary,
        Secondary,
        Custom
    }

    public override void Awake()
    {
        image = GetComponent<Image>();

        base.Awake();
    }

    protected override void OnSkinUI()
    {

        switch (imageColor)
        {
            case ImageColor.White:
                image.color = skinData.whiteColor;
                break;
            case ImageColor.Black:
                image.color = skinData.blackColor;
                break;
            case ImageColor.Primary:
                image.color = skinData.primaryColor;
                break;
            case ImageColor.Secondary:
                image.color = skinData.secondaryColor;
                break;
            case ImageColor.Custom:
                break;
        }

        base.OnSkinUI();
    }
}