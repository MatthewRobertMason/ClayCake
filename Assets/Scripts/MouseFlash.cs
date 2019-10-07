using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFlash : MonoBehaviour
{
    public Sprite MouseUpSprite;
    public Sprite MouseDownSprite;

    bool up = true;
    int flips = 0;
    public int TargetFlips;

    float flipTime = 0;
    public float TimePerFlip = 0.8f;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = MouseUpSprite;
        flipTime = TimePerFlip;
    }

    // Update is called once per frame
    void Update()
    {
        flipTime -= Time.deltaTime;
        if(flipTime <= 0) {
            flips++;
            if(flips >= TargetFlips) {
                Object.Destroy(gameObject);
                return;
            }

            if (up) {
                sr.sprite = MouseDownSprite;
            } else {
                sr.sprite = MouseUpSprite;
            }
            flipTime = TimePerFlip;
            up = !up;
        }
    }
}
