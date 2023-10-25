using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;

public class VirtualJoyStick2 : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Vector2 Value { get; private set; }

    private int pointerId;
    private bool isDragging = false;

    public void OnDrag(PointerEventData eventData)
    {
        Value = eventData.delta / Screen.dpi;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerId != eventData.pointerId)
        {
            return;
        }
        isDragging = false;
        //stick.rectTransform.position = originalPoint;
        //value = Vector2.zero;
    }
}
