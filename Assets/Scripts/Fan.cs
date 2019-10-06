using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector3 push = transform.localToWorldMatrix * new Vector3(3000 * Time.deltaTime, 0, 0);
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(push);
    }
}
