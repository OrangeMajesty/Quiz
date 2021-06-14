using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BlockController : MonoBehaviour
{
    // Значение спрайта
    public string key;
    // public UnityEvent clickEvent;

    void Awake()
    {
        // if(clickEvent == null)
        //     clickEvent = new UnityEvent();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        

        // clickEvent.AddListens(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.)
    }

    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    // }
}
