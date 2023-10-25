using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoyStick : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum Axis
    {
        Horizontal,
        Vertical
    }
    public Image stick;
    public float radius;

    private Vector3 originalPoint;
    private RectTransform rectTr;

    private Vector2 value;

    private int pointerId;
    private bool isDragging = false;

    public void Start()
    {
        rectTr = GetComponent<RectTransform>();
        originalPoint = stick.rectTransform.position;
    }

    public float GetAxis(Axis axis)
    {
        switch(axis)
        {
            case Axis.Horizontal:
                return value.x;
            case Axis.Vertical:
                return value.y;
        }
        return 0f;
    }

    private void Update()
    {
        Debug.Log($"{GetAxis(Axis.Horizontal)} / {GetAxis(Axis.Vertical)}");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isDragging)
        {
            return;
        }

        isDragging = true;
        pointerId = eventData.pointerId;
        UpdateStickPos(eventData.position);
    }

    public void UpdateStickPos(Vector3 screenPos)
    {
  RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTr, screenPos, null, out Vector3 newPoint);
        var delta = Vector3.ClampMagnitude(newPoint - originalPoint, radius);
        value = delta.normalized;

        stick.rectTransform.position = originalPoint + delta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(pointerId != eventData.pointerId)
        {
            return;
        }
        UpdateStickPos(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UpdateStickPos(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(pointerId != eventData.pointerId)
        {
            return;
        }
        isDragging = false;
        stick.rectTransform.position = originalPoint;
        value = Vector2.zero;
    }
}
