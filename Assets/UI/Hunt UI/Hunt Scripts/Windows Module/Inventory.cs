using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerHunt.GameStateModule;
using ScavengerHunt.ItemsLoaderModule;
using DG.Tweening;

namespace ScavengerHunt.WindowsModule
{
    public class Inventory : MonoBehaviour
    {
        public GameState gameState;
        public ItemsLoader itemsLoader;

        public Transform inventoryItems;
        private InventoryItem[] inventoryItemComponents;
        private UITweener[] uiTweeners;
        public UISequenceManager sequenceManager;

        public string alreadyFoundTrigger;
        public string newlyFoundTrigger;
        // public string notYetFoundTrigger;

        public GameEvent UpdateImageAudioEvent;

        private void Start()
        {
            if (itemsLoader == null)
                itemsLoader = (ItemsLoader)Object.FindObjectOfType(typeof(ItemsLoader));

            inventoryItemComponents = inventoryItems.GetComponentsInChildren<InventoryItem>();
            uiTweeners = new UITweener[inventoryItemComponents.Length];
            for (int i = 0; i < inventoryItemComponents.Length; i++)
            {
                InventoryItem item = inventoryItemComponents[i];

                UITweener uiTweener = item.gameObject.GetComponent<UITweener>();
                uiTweeners[i] = uiTweener;

                item.gameObject.SetActive(i < gameState.target);
            }
        }

        //private IEnumerator WaitToTrigger(int i, Animator animator, string triggerName)
        //{
        //    yield return new WaitForSeconds((float)i * 0.4f);
        //    animator.SetTrigger(triggerName);
        //}

        public void UpdateInventory()
        {
            UpdateInventory(false); //do not append; join.
        }

        public void UpdateInventory(bool doAppend)
        {
            if (gameState.recentFind)
            {
                Sequence updateSequence = GetUpdateSequence();
                if (updateSequence != null)
                {
                    if (doAppend)
                        sequenceManager.currentSequence.Append(updateSequence);
                    else
                        sequenceManager.currentSequence.Join(updateSequence);
                }
                else
                    UpdateAllImages();
            }
            else
            {
                UpdateAllImages();
            }

        }

        private void UpdateAllImages() //failsafe
        {
            for (int i = 0; i < inventoryItemComponents.Length; i++)
            {
                InventoryItem inventoryItem = inventoryItemComponents[i];
                inventoryItem.UpdateImages(i);
            }
        }

        private Sequence GetUpdateSequence()
        {
            float maxDuration = 0.33f;

            Sequence updateSequence = DOTween.Sequence();

            for (int i=0; i<inventoryItemComponents.Length; i++)
            {
                InventoryItem inventoryItem = inventoryItemComponents[i];
                UITweener tweener = uiTweeners[i];

                if (tweener != null)
                {
                    if (gameState.recentFind && i < gameState.found.Count)
                    {
                        //float offset = (float)i * (0.4f / (gameState.found.Count - 1));
                        float offset = (float)i * 0.3f;
                        if (i > 1 && i == gameState.found.Count - 1) offset += 0.15f;

                        Tween entryTween = tweener.GetTween(UITweener.TweenType.Entry);
                        if (entryTween != null)
                            updateSequence.Insert(offset, entryTween);

                        float entryDur = tweener.UITweensDict[UITweener.TweenType.Entry][0].UIAnimation.duration;
                        Tween exitTween = tweener.GetTween(UITweener.TweenType.Exit);
                        if (exitTween != null)
                            updateSequence.Insert(offset + entryDur, exitTween);

                        //float exitDur = tweener.UITweensDict[UITweener.TweenType.Exit].UIAnimation.duration;
                        int k = i;
                        updateSequence.InsertCallback(offset + entryDur, () =>
                        {
                            inventoryItem.UpdateImages(k);

                            if (k == gameState.found.Count - 1 && UpdateImageAudioEvent != null)
                            {
                                UpdateImageAudioEvent.Raise();
                            }
                        });
                    }
                    else
                    {
                        inventoryItem.UpdateImages(i);
                    }
                }
            }

            //updateSequence.timeScale = updateSequence.Duration() / maxDuration;
            //Debug.Log(updateSequence.timeScale);
            

            //if (updateSequence.Duration() > maxDuration)
            //{
            //    Debug.Log("remapping");
            //    Debug.Log((float)updateSequence.Duration() / maxDuration);
            //    updateSequence.timeScale = ((float)updateSequence.Duration() / maxDuration);
            //}
            //updateSequence.timeScale = 0f;

            if (updateSequence.Duration() == 0) { return null; } //if empty sequence i.e. if for-loop didn't iterate
            else { return updateSequence; }

            

            

            //int i = 0;
            //InventoryItem[] inventoryItemComponents = inventoryItems.GetComponentsInChildren<InventoryItem>();
            //foreach (InventoryItem inventoryItem in inventoryItemComponents)
            //{
            //    inventoryItem.number = i;

            //    if (gameState.recentFind && i < gameState.found.Count)
            //    {
            //        Animator animator = inventoryItem.gameObject.GetComponent<Animator>();
            //        if (i == gameState.found.Count - 1) //last item
            //        {
            //            StartCoroutine(WaitToTrigger(i, animator, newlyFoundTrigger)); //UpdateImages is called in the animation
            //        }
            //        else
            //        {
            //            inventoryItem.gameObject.GetComponent<InventoryItem>().UpdateImages();
            //            StartCoroutine(WaitToTrigger(i, animator, alreadyFoundTrigger));
            //        }

            //    }
            //    else
            //    {
            //        inventoryItem.gameObject.GetComponent<InventoryItem>().UpdateImages();
            //    }

            //    inventoryItem.gameObject.SetActive(i < gameState.target);
            //    i++;
            //}
            //gameState.recentFind = false;
        }
    }
}


