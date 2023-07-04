using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DownPipe : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isDownTower;
    public void OnPointerDown(PointerEventData eventData)
    {
        isDownTower = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDownTower = false;
    }
}
