using System.Collections.Generic;
using Core.Repositories;
using CustomScripts.Util;
using UnityEngine;

namespace CustomScripts
{
    public sealed class CardRepository : Repository
    {
        private const string _bundleName = "items";
        public Dictionary<string, Dictionary<string, Object>> sprites { get; private set; }
        protected override void Initialize()
        {
            List<Texture2D> textures = BundleLoader.LoadAssetBundle<Texture2D>(_bundleName);
            
            var packs = new Dictionary<string, Dictionary<string, Object>>();
            
            foreach (Texture2D texture in textures)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                string[] assetInfo = texture.name.Split('_');
            
                if (assetInfo.Length > 0)
                {
                    string type = assetInfo[0];
                    string name = assetInfo[1];
            
                    if (!packs.ContainsKey(type))
                        packs[type] = new Dictionary<string, Object>(); 
            
                    packs[type].Add(name, sprite);
                }
            }
            
            sprites = packs;
        }
    }
}
