using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStrobe : MonoBehaviour
{
    public Sprite DisplaySprite;
    public float Duration = 2;

    float counter = 0;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = DisplaySprite;
        
        /*
        var bounds = sr.sprite.bounds;
        var yfactor = 1f / bounds.size.y;
        var xfactor = 1f / bounds.size.x;
        var factor = Mathf.Min(yfactor, xfactor);
        sr.transform.localScale = new Vector3(factor, factor, factor);
        */
    }


    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        float progress = counter / Duration;
        if(progress >= 1) {
            Object.Destroy(gameObject);
        }

        float time = progress * Mathf.PI * 2;
        var c = sr.color;
        c.a = Mathf.Sin(time % Mathf.PI) + Mathf.Sin(time / 2.0f);
        sr.color = c;

    }
}
