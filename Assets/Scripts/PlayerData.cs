using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameObject GearPrefab;


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
    }

    // Update is called once per frame
    void Update()
    {
        if(!Cursor.visible && Input.GetMouseButtonUp(0)) {
            // TODO set custom cursor texture here
            Cursor.visible = true;

            // Enable  all the starting emitters
            foreach(Emitter x in FindObjectsOfType<Emitter>()) {
                x.enabled = true;
            }
        }

        if (FindObjectOfType<ScienceMachine>().hidden && inventory.Count > 0) {
            FindObjectOfType<ScienceMachine>().Show();
        }

        if (Input.GetMouseButtonDown(1)) {
            if(inventory.Count > 0) {
                var type = inventory[0];
                inventory.RemoveAt(0);
                var pos = Input.mousePosition;
                pos.z = 10;
                var obj = Create(type, Camera.main.ScreenToWorldPoint(pos));
                obj.GetComponent<Part>().dropped = true;
            }
        }
    }
}
