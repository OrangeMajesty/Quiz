using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Interactors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomScripts
{
    public sealed class PrefabInteractor : Interactor
    {
        private PrefabRepository _repository;
        public Dictionary<string, Object> prefabs => this._repository.prefabs;
        
        public PrefabInteractor()
        {
            this._repository = Game.GetRepository<PrefabRepository>();
        }

        public GameObject GetPrefabByName(string name)
        {
            if (!prefabs.ContainsKey(name))
                throw new Exception($"Prefab {name} not found in bundle");
            return prefabs[name] as GameObject;
        }
        
        protected void Initialize() {}
        public void Save() {}
    }
}

