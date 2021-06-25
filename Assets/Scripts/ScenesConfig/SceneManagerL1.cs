using Core.Scenes;

namespace CustomScripts {
	public sealed class SceneManagerL1 : SceneManagerBase{
		protected override void InitializeSceneConfigs() {
			this.scenesConfigMap[SceneConfigL1.name] = new SceneConfigL1();
		}
	}
}