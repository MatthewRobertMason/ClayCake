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
    public int ExtraCapacity = 0;

    private float currentTotalCooldown;
    public float currentCooldown;
    private List<GameObject> living = new List<GameObject>();

    private GameObject nextOut;
    private Vector3 nextOutFinalScale;

    // Start is called before the first frame update
    void Start()
    {
        currentTotalCooldown = currentCooldown = 2;
        PrepareNext();
    }

    void ResetCooldown()
    {
        currentTotalCooldown = currentCooldown = EmitCooldown + Random.Range(-EmitCooldownJiggle, EmitCooldownJiggle);
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown -= Time.deltaTime;
        nextOut.transform.localScale = nextOutFinalScale * (1 - (Mathf.Max(currentCooldown, 1) / currentTotalCooldown));
        if (living.Count < LivingLimit + ExtraCapacity || LivingLimit < 0) {
            if(currentCooldown <= 0) {
                EmitObject();
                ResetCooldown();
            }
        }
        living.RemoveAll(item => item == null);
    }

    void EmitObject()
    {
        var obj = nextOut;
        obj.transform.localScale = nextOutFinalScale;
        obj.GetComponent<Rigidbody2D>().simulated = true;
        obj.GetComponent<Rigidbody2D>().WakeUp();
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Velocity.x, Velocity.y);
        //   obj.transform.Rotate(Vector3.up, Random.Range(-180.0f, 180.0f));
        if (ExtraCapacity > 0) {
            ExtraCapacity--;
        } else {
            living.Add(obj);
        }

        PrepareNext();
    }

    void PrepareNext()
    {
        nextOut = Instantiate(EmittedPrefab, transform.position, Quaternion.identity);
        nextOut.GetComponent<Rigidbody2D>().simulated = false;
        nextOutFinalScale = nextOut.transform.localScale;
        nextOut.transform.localScale = new Vector3(0, 0);
    }
}
