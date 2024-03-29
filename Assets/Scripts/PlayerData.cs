﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameObject GearPrefab;
    public GameObject PlatePrefab;
    public GameObject CircuitPrefab;

    public GameObject volumePanel;

    public FadeMusic fadeMusic;
    public GameObject LeftClickerImage;
    
    public static int XMaximum = 45;
    public static int YMaximum = 20;

    public bool FastModeActive = false;

    public bool cursorVisible = false;

    public static int ScaleCost(int cost)
    {
        if (Options.IsFastModeActive()) {
            return cost / 10 + 1;
        }
        return cost;
    }

    public GameObject MouseFlashPrefab;

    ScienceMachine sm;
    bool dragging;
    Vector3 dragAnchor;

    public GameObject Create(PartType type, Vector3 position)
    {
        switch (type) {
            case PartType.Gear: return Instantiate(GearPrefab, position, Quaternion.identity);
            case PartType.Plate: return Instantiate(PlatePrefab, position, Quaternion.identity);
            case PartType.Circuit: return Instantiate(CircuitPrefab, position, Quaternion.identity);
        }
        return null;
    }

    public Sprite PartSprite(PartType type)
    {
        switch (type) {
            case PartType.Gear: return GearPrefab.GetComponent<SpriteRenderer>().sprite;
            case PartType.Plate: return PlatePrefab.GetComponent<SpriteRenderer>().sprite;
            case PartType.Circuit: return CircuitPrefab.GetComponent<SpriteRenderer>().sprite;
        }
        return null;
    }

    public List<PartType> inventory = new List<PartType>();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = cursorVisible;

        // FOR DEBUGGING
        for(int ii = 0; ii < 50; ii++) {
        //    inventory.Add(PartType.Gear);
        }
        sm = FindObjectOfType<ScienceMachine>();
    }

    bool MouseOnObject()
    {
        var pt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pt, Vector2.zero, Mathf.Infinity);
        return hit.collider != null;
    }

    Vector3 CameraBottomLeft()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
    }

    Vector3 CameraTopRight()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z));
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

            fadeMusic.fadeMusic = true;
            Destroy(LeftClickerImage);
            volumePanel.SetActive(true);
        }

        if (sm.hidden && inventory.Count > 0) {
            sm.Show();
            Instantiate(MouseFlashPrefab, sm.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        }

        if (Input.GetMouseButtonDown(1) && !MouseOnObject()) {
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
            var board = new Plane(new Vector3(0, 0, 1), 0);
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

                var tr = CameraTopRight();
                if (tr.x + delta.x > XMaximum) {
                    delta.x = XMaximum - tr.x;
                }
                if(tr.y + delta.y > YMaximum) {
                    delta.y = YMaximum - tr.y;
                }

                var bl = CameraBottomLeft();
                if (bl.x + delta.x < -XMaximum) {
                    delta.x = -XMaximum - bl.x;
                }
                if (bl.y + delta.y < -YMaximum) {
                    delta.y = -YMaximum - bl.y;
                }


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
