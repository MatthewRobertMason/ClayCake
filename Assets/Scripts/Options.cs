using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    
    public bool ActivateFastMode = false;

    public void SetFastMode(bool value)
    {
        ActivateFastMode = value;
    }

    public static bool IsFastModeActive()
    {
        var options = FindObjectOfType<Options>();
        if (options) {
            return options.ActivateFastMode;
        }
        return true;  // If there is no options object, game was launched from editor, probably testing, fast mode on
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
