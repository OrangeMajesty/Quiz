using System;
using System.Collections.Generic;
using Core.Interactors;
using Core.Repositories;

namespace Core.Scenes.Config {
	public interface ISceneConfig {
		
		string sceneName { get; }
		
		
		Dictionary<Type, IRepository> CreateAllRepositories();
		Dictionary<Type, IInteractor> CreateAllInteractors();
	}
}