using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameObject GearPrefab;


    ScienceMachine sm;
    bool dragging;
    Vector3 dragAnchor;

    public GameObject Create(PartType type, Vector3 position)
    {
        switch (type) {
            case PartType.Gear: return Instantiate(GearPrefab, position, Quaternion.identity);
        }
        return null;
    }


    public List<PartType> inventory = new List<PartType>();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        // FOR DEBUGGING
        for(int ii = 0; ii < 50; ii++) {
            inventory.Add(PartType.Gear);
        }
        sm = FindObjectOfType<ScienceMachine>();
    }

    bool MouseOnObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Cursor.visible && Input.GetMouseButtonUp(0)) {
            // TODO set custom cursor texture here
            Cursor.visible = true;

            // Enable  all the starting emitters
            foreach (Emitter x in FindObjectsOfType<Emitter>()) {
                x.enabled = true;
            }
        }

        if (sm.hidden && inventory.Count > 0) {
            sm.Show();
        }

        if (Input.GetMouseButtonDown(1)) {
            if (inventory.Count > 0) {
                var type = inventory[0];
                inventory.RemoveAt(0);
                var pos = Input.mousePosition;
                pos.z = 10;
                var obj = Create(type, Camera.main.ScreenToWorldPoint(pos));
                obj.GetComponent<Part>().dropped = true;
            }
        }

        if (sm.PageMovement) {
            var board = new Plane(new Vector3(0, 0, 1), -10);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hit_delta;

            if (Input.GetMouseButtonDown(0) && !dragging) {
                if (!MouseOnObject()) {
                    dragging = true;
                    board.Raycast(ray, out hit_delta);
                    dragAnchor = ray.GetPoint(hit_delta);
                }
            }
            else if (Input.GetMouseButton(0) && dragging) {
                board.Raycast(ray, out hit_delta);
                var newPoint = ray.GetPoint(hit_delta);
                Vector3 delta = dragAnchor - newPoint;
                delta.z = 0;
                Camera.main.transform.Translate(delta);

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                board.Raycast(ray, out hit_delta);
                dragAnchor = ray.GetPoint(hit_delta);
            } else if (dragging) {
                dragging = false;
            }
        }
    }
}
