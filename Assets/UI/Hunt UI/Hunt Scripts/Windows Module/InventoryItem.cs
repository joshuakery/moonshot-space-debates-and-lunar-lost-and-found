using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScavengerHunt.GameStateModule;
using ScavengerHunt.ItemsLoaderModule;

//todo                 UpdateImageAudioEvent.Raise();

namespace ScavengerHunt.WindowsModule
{
    public class InventoryItem : MonoBehaviour
    {
        public GameState gameState;
        public ItemsLoader itemsLoader;

        public GameEvent InventoryUpdated;

        public GenericWindow1 genericWindow;

        public string code;

        private void Start()
        {
            if (itemsLoader == null)
                itemsLoader = GameObject.Find("ItemsLoader").GetComponent<ItemsLoader>();

            if (genericWindow == null)
                genericWindow = GetComponent<GenericWindow1>();
        }

        public void UpdateImages(int number)
        {
            Transform defaultParent = gameObject.transform.GetChild(0);
            Transform riParent = gameObject.transform.GetChild(1);

            if (number > -1 && number < gameState.found.Count)
            {
                code = gameState.found[number];
                // Debug.Log(code);
                Item item = itemsLoader.itemsData[code];

                RawImage ownerRI = riParent.FindDeepChild("Owner").GetComponent<RawImage>();
                ownerRI.texture = item.OwnerTex;
                RawImage itemRI = riParent.Find("Item").GetComponent<RawImage>();
                itemRI.texture = item.ItemTex;

                defaultParent.gameObject.SetActive(false);
                riParent.gameObject.SetActive(true);

                gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                riParent.gameObject.SetActive(false);
                defaultParent.gameObject.SetActive(true);

                gameObject.GetComponent<Button>().interactable = false;
            }
        }

        public void RaiseInventoryUpdated()
        {
            if (InventoryUpdated != null)
                InventoryUpdated.Raise();
        }

        public void SetSelfAsCurrentItem()
        {
            gameState.CurrentItem = itemsLoader.itemsData[code];
        }

        public void PulseIfCurrentItemAndNotRecent()
        {
            if (!gameState.recentFind &&
                !System.String.IsNullOrEmpty(code) &&
                gameState.CurrentItem == itemsLoader.itemsData[code]
                )
            {
                genericWindow.Pulse(0.18f);
            }
        }
    }
}


