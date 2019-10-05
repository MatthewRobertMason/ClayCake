using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : MonoBehaviour
{
    public List<PartType> Filter;
    public List<PartType> Contents;
    public System.Action<PartType> Handler;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var part = collision.gameObject.GetComponent<Part>();
        if (part) {
            if (Filter.Contains(part.Type)) {
                if (Handler != null) {
                    Handler(part.Type);
                } else {
                    Contents.Add(part.Type);
                }
            }
            Destroy(collision.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
