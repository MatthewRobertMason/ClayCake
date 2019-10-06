using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverBox : MonoBehaviour
{
    public Consumer input;

    // Start is called before the first frame update
    void Start()
    {
        input.Handler = ItemGot;
    }

    void ItemGot(PartType type)
    {
        Debug.LogFormat("Cover box hit and dropping");
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().WakeUp();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy automatically if out of bounds
        if (Mathf.Abs(transform.position.x) > PlayerData.XMaximum || Mathf.Abs(transform.position.y) > PlayerData.YMaximum) {
            GameObject.Destroy(this.gameObject);
        }
    }
}
