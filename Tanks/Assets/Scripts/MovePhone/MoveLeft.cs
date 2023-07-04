using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveLeft : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isMoveLeft;
    public void OnPointerDown(PointerEventData eventData)
    {
        isMoveLeft = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMoveLeft = false;
    }
}
