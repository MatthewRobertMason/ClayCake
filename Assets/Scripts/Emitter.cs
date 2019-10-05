using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    public GameObject EmittedPrefab;
    public int LivingLimit = 2;
    public float EmitCooldown = 3;
    public float EmitCooldownJiggle = 2;
    public Vector2 Velocity = new Vector2(0, 0);

    private float currentCooldown;
    private List<GameObject> living = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = 2;
    }

    void ResetCooldown()
    {
        currentCooldown = EmitCooldown + Random.Range(-EmitCooldownJiggle, EmitCooldownJiggle);
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (living.Count < LivingLimit || LivingLimit < 0) {
            if(currentCooldown <= 0) {
                EmitObject();
                ResetCooldown();
            }
        }
        living.RemoveAll(item => item == null);
    }

    void EmitObject()
    {
        var obj = Instantiate(EmittedPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Velocity.x, Velocity.y);
        //   obj.transform.Rotate(Vector3.up, Random.Range(-180.0f, 180.0f));
        living.Add(obj);
    }
}
