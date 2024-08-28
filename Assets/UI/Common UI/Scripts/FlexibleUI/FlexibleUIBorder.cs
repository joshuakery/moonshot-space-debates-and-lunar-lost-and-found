using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlexibleUIBorder : FlexibleUI
{
    public enum BorderColor
    {
        PrimaryGradient,
        SecondaryGradient,
        White
        
    }

    Image image;

    public BorderColor borderColor;

    public override void Awake()
    {
        image = GetComponent<Image>();

        base.Awake();
    }

    protected override void OnSkinUI()
    {
        image.type = Image.Type.Simple;

        switch(borderColor)
        {
            case BorderColor.PrimaryGradient:
                image.sprite = skinData.primaryLineGradient;
                break;
            case BorderColor.SecondaryGradient:
                image.sprite = skinData.secondaryLineGradient;
                break;
            case BorderColor.White:
                image.sprite = null;
                break;
        }

        
    }
}