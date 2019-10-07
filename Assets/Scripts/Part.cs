using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PartType
{
    Gear,
    Plate,
    Circuit
}



public class Part : MonoBehaviour
{
    static public PartType[] AllTypes = { PartType.Gear, PartType.Plate, PartType.Circuit };

    public PartType Type;
    PlayerData player;
    public bool dropped = false;

    // Start is called before the first frame update
    void Start()
    {
        var sm = Object.FindObjectOfType<ScienceMachine>();
        if (sm.DiscoveredParts.Contains(Type)) {
            Discover();
        }
        player = Object.FindObjectOfType<PlayerData>();
    }

    // Called by the Progress Tracker on all parts of a given type when they become known
    public void Discover()
    {
        GetComponent<SetCensor>().CensorPixelLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy automatically if out of bounds
        if (Mathf.Abs(transform.position.x) > PlayerData.XMaximum || Mathf.Abs(transform.position.y) > PlayerData.YMaximum) {
            GameObject.Destroy(this.gameObject);
        }        
    }

    private void OnMouseOver()
    {
        if (!dropped) {
            player.inventory.Add(Type);
            Debug.LogFormat("Added {0} object to player inventory, now has {1} items.", Type, player.inventory.Count);
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnMouseUpAsButton()
    {
        player.inventory.Add(Type);
        Debug.LogFormat("Added {0} object to player inventory, now has {1} items.", Type, player.inventory.Count);
        GameObject.Destroy(this.gameObject);
    }
}
