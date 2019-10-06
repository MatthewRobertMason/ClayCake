using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    bool dragging = false;
    Vector3 dragAnchor;

    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        dragging = false;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        dragging = true;
        var pos = Input.mousePosition;
        pos.z = 10;
        dragAnchor = Camera.main.ScreenToWorldPoint(pos);
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (dragging) {
            var pos = Input.mousePosition;
            pos.z = 10;
            var newPosition = Camera.main.ScreenToWorldPoint(pos);
            var delta = newPosition - dragAnchor;
            delta.z = 0;
            transform.Translate(delta);            
            dragAnchor = newPosition;
        }
    }

}
