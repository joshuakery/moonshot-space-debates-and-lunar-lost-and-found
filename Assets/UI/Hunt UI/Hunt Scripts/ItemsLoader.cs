using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.EventSystems;
using rlmg.logging;
using ScavengerHunt.GameStateModule;

namespace ScavengerHunt.ItemsLoaderModule
{
    [System.Serializable]
    public class Item
    {
        public string ItemName;
        public string ItemImageFilename;
        public string OwnerName;
        public string OwnerImageFilename;
        public string OwnerTitle;
        public string OwnerQuote;

        public Texture2D ItemTex;
        public Texture2D OwnerTex;
    }

    public class ItemsLoader : ContentLoader
    {
        public EventSystem eventSystem;

        public GameState gameState;

        public Dictionary<string,Item> itemsData = new Dictionary<string,Item>();

        public string ownerDir = "Owner_Images";
        public string itemDir = "Item_Images";

        private Texture2D GetTexture2DFromPath(string path)
        {
            Texture2D tex = new Texture2D(2,2);

            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                tex.LoadImage(bytes);
            }
            catch (Exception e)
            {
                RLMGLogger.Instance.Log(String.Format("Failed to read item images: {0}.",e.ToString()), MESSAGETYPE.ERROR);
            }

            return tex;
        }

        protected override IEnumerator PopulateContent(string contentData)
        {
            itemsData = JsonConvert.DeserializeObject<Dictionary<string,Item>>(contentData);

            if (itemsData == null)
                yield break;

            string itemDirPath = Path.Join(Application.streamingAssetsPath,itemDir);
            string ownerDirPath = Path.Join(Application.streamingAssetsPath, ownerDir);

            foreach (KeyValuePair<string,Item> kvp in itemsData)
            {
                Item item = kvp.Value;

                string itemPath = Path.Join(itemDirPath,item.ItemImageFilename);
                item.ItemTex = GetTexture2DFromPath(itemPath);

                string ownerPath = Path.Join(ownerDirPath,item.OwnerImageFilename);
                item.OwnerTex = GetTexture2DFromPath(ownerPath);
            }

            //set gamestate target
            if (gameState != null)
                gameState.target = itemsData.Count;

            yield break;
        }
    }
}



