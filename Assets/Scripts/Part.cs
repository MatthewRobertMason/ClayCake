using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PartType
{
    Gear
}


public class Part : MonoBehaviour
{
    public PartType Type;
    public Sprite KnownSprite;


    // Start is called before the first frame update
    void Start()
    {
        var sm = Object.FindObjectOfType<ScienceMachine>();
        if (sm.DiscoveredParts.Contains(Type)) {
            Discover();
        }
    }

    // Called by the Progress Tracker on all parts of a given type when they become known
    public void Discover()
    {
        GetComponent<SpriteRenderer>().sprite = KnownSprite;
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy automatically if out of bounds
        if (Mathf.Abs(transform.position.x) > 10.0f || Mathf.Abs(transform.position.y) > 10.0f) {
            GameObject.Destroy(this.gameObject);
        }        
    }
}
