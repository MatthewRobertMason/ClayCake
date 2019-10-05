using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    public static ProgressTracker Instance;
    public HashSet<PartType> DiscoveredParts = new HashSet<PartType>();

    public static bool IsDiscovered(PartType type)
    {
        return Instance.DiscoveredParts.Contains(type);
    }

    public void DiscoverPart(PartType type)
    {
        DiscoveredParts.Add(type);
        foreach(Part obj in Object.FindObjectsOfType<Part>()) {
            if(obj.Type == type) {
                obj.Discover();
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(Instance == null);
        Instance = this;
    }
}
