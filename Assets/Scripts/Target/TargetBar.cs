using Core;
using CustomScripts.Target;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TargetBar : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        Game.OnGameInitializedEvent += InitInterators;
        
        text = GetComponent<Text>();
        SetMessage();
    }

    private void InitInterators()
    {
        var interactor = Game.GetInteractor<TargetInteractor>();
        interactor.ConnectGameObject(this.gameObject);
    }

    public void SetMessage(string message = "")
    {
        ShowFade();
        text.text = message;
    }

    public void ShowFade()
    {
        text.DOFade(1f, 0.3f);
    }

    public void HideFade()
    {
        text.DOFade(0f, 0.3f);
    }
}
