using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float CapacatyBoostTime = 1;
    float capacityTimer = 0;

    // Update is called once per frame
    void Update()
    {
        capacityTimer += Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("UI"));
        if (hit.collider) {
            var emitter = hit.collider.GetComponent<Emitter>();
            if (emitter) {
                emitter.currentCooldown -= Time.deltaTime * 5;
                if (capacityTimer > CapacatyBoostTime) {
                    emitter.ExtraCapacity++;
                }
            }
        }

        if(capacityTimer > CapacatyBoostTime) {
            capacityTimer = 0;
        }
    }
}
