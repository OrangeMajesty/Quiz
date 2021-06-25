using System.Collections.Generic;
using Core.Repositories;
using CustomScripts.Util;
using UnityEngine;

namespace CustomScripts
{
    public sealed class PrefabRepository : Repository
    {
        private const string _bundleName = "prefabs";
        public Dictionary<string, Object> prefabs { get; private set; }
        protected override void Initialize()
        {
            List<GameObject> prefabList = BundleLoader.LoadAssetBundle<GameObject>(_bundleName);
            
            prefabs = new Dictionary<string, Object>();
            
            foreach (GameObject prefab in prefabList)
            {
                prefabs.Add(prefab.name, prefab);
            }
        }
    }
}
