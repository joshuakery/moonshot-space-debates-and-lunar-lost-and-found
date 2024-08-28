using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleUITextCharter : FlexibleUIText
{
    public enum OptionColorOverride
    {
        None,
        OptionA,
        OptionB
    }

    public FlexibleUIDataCharter charterSkinData;

    public OptionColorOverride optionColorOverride;
    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        switch(optionColorOverride)
        {
            case OptionColorOverride.None:
                break;
            case OptionColorOverride.OptionA:
                tmp_text.color = Color.white;
                tmp_text.enableVertexGradient = true;
                tmp_text.colorGradientPreset = charterSkinData.optionATextGradient;
                break;
            case OptionColorOverride.OptionB:
                tmp_text.color = Color.white;
                tmp_text.enableVertexGradient = true;
                tmp_text.colorGradientPreset = charterSkinData.optionBTextGradient;
                break;
        }

    }
}
