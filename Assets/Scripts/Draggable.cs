using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    public bool AllowRotate = false;
    bool dragging = false;
    Vector3 dragAnchor;
    public GameObject target;
    public GameObject rotateTarget;

    private void Start()
    {
        if (target == null) target = gameObject;
        if (rotateTarget == null) rotateTarget = gameObject;
    }

    private void OnMouseUp()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        dragging = false;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        dragging = true;
        var pos = Input.mousePosition;
        pos.z = 10;
        dragAnchor = Camera.main.ScreenToWorldPoint(pos);
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (dragging) {
            var pos = Input.mousePosition;
            pos.z = 10;
            var newPosition = Camera.main.ScreenToWorldPoint(pos);
            var delta = newPosition - dragAnchor;
            delta.z = 0;
            target.transform.Translate(delta);
            dragAnchor = newPosition;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(2) && AllowRotate) {
            rotateTarget.transform.Rotate(new Vector3(0, 0, 1), 45);
        }
    }
}
