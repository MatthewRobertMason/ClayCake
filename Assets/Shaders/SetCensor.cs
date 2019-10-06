using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCensor : MonoBehaviour
{
    public float CensorPixelLevel = 16.0f;
    
    // Update is called once per frame
    void Update()
    {
        this.GetComponent<SpriteRenderer>().material.SetFloat("Pixel", CensorPixelLevel);
    }
}
