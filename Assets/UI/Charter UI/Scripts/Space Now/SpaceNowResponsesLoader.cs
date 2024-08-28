using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using CharterRules.SpaceNow.GameStateModule;

namespace CharterRules.SpaceNow.DataTracking
{
    public class SpaceNowResponsesLoader : ContentLoader
    {
        public SpaceNowGameState gameState;

        private void Start()
        {
            gameState.contentFilename = contentFilename;
        }

        protected override IEnumerator PopulateContent(string contentData)
        {
            Dictionary<string, Dictionary<string, Dictionary<int, int>>> _responses = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<int, int>>>>(contentData);

            if (_responses == null)
                yield break;

            gameState.responses = _responses;

            yield break;
        }
    }
}


