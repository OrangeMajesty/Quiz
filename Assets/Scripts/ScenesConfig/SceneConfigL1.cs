using System;
using System.Collections.Generic;
using Core.Interactors;
using Core.Repositories;
using Core.Scenes.Config;
using CustomScripts.Target;
using UnityEngine;

namespace CustomScripts {
    public sealed class SceneConfigL1 : SceneConfigBase {

        #region CONSTANTS

        public const string name = "Game";

        #endregion
        
        public override string sceneName { get; }

        public SceneConfigL1() {
            this.sceneName = name;
        }

        public override Dictionary<Type, IRepository> CreateAllRepositories() {
            var createdReposisories = new Dictionary<Type, IRepository>();

            this.CreateRepository<PrefabRepository>(createdReposisories);
            this.CreateRepository<CardRepository>(createdReposisories);

            return createdReposisories;
        }

        public override Dictionary<Type, IInteractor> CreateAllInteractors() {
            var createdInteractors = new Dictionary<Type, IInteractor>();
            
            this.CreateInteractor<TargetInteractor>(createdInteractors);
            this.CreateInteractor<PrefabInteractor>(createdInteractors);
            this.CreateInteractor<CardInteractor>(createdInteractors);
            this.CreateInteractor<LevelInteractor>(createdInteractors);

            return createdInteractors;
        }
    }
}