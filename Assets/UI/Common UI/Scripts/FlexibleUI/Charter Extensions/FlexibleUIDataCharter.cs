using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Flexible UI Data - Charter")]
public class FlexibleUIDataCharter : ScriptableObject
{
    [Header("Charter Specific Color Palette")]
    public Color32 optionAColor;
    public Color32 optionAShade;
    public TMP_ColorGradient optionATextGradient;
    public Color32 optionBColor;
    public Color32 optionBShade;
    public TMP_ColorGradient optionBTextGradient;
}
