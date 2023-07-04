using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpPipe : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isUpTower;
    public void OnPointerDown(PointerEventData eventData)
    {
        isUpTower = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isUpTower = false;
    }
}
