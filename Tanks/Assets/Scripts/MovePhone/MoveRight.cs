using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveRight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isMoveRight;
    public void OnPointerDown(PointerEventData eventData)
    {
        isMoveRight = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMoveRight = false;
    }
}
