using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirePhone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isFire;
    public void OnPointerDown(PointerEventData eventData)
    {
        isFire = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isFire = false;
    }
}
