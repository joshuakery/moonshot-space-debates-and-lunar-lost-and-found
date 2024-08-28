using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScavengerHunt.GameStateModule;
using ScavengerHunt.ItemsLoaderModule;

namespace ScavengerHunt.WindowsModule
{
    public class CorrectModal : MonoBehaviour
    {
        public GameState gameState;

        public TMP_Text ownerName;
        public TMP_Text ownerTitle;
        public TMP_Text ownerQuote;
        public RawImage itemImage;
        public RawImage ownerImage;

        public void SetTexts()
        {
            Item item = gameState.CurrentItem;

            ownerName.text = item.OwnerName;
            ownerTitle.text = item.OwnerTitle;
            ownerQuote.text = item.OwnerQuote;
            itemImage.texture = item.ItemTex;
            ownerImage.texture = item.OwnerTex;

            //ownerTitle.text = FlexibleUIText.PreventRunts(ownerTitle.text);
            ownerQuote.text = FlexibleUIText.PreventRunts(ownerQuote.text);
        }
    }
}


