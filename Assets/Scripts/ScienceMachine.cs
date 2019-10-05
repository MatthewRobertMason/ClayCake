using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct CostItem
{
    public PartType type;
    public int number;
}

[System.Serializable]
public struct ResearchProject
{
    public Sprite icon;
    public CostItem[] cost;
    public UnityEvent researchEffect;
}


public class ScienceMachine : MonoBehaviour
{
    public HashSet<PartType> DiscoveredParts = new HashSet<PartType>();

    public ResearchProject[] ResearchSteps;
    int CurrentResearch = -1;
    Dictionary<PartType, int> ResearchProgress;

    public SpriteRenderer ProjectSprite;
    public Sprite UnlockedSprite;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Consumer>().Handler = ProgressMade;
        NextResearch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NextResearch()
    {
        CurrentResearch++;
        ProjectSprite.sprite = ResearchSteps[CurrentResearch].icon;
        ResearchProgress = new Dictionary<PartType, int>(); 
        foreach(CostItem item in ResearchSteps[CurrentResearch].cost) {
            ResearchProgress[item.type] += item.number;
        }
    }


    void ProgressMade(PartType type)
    {
        if (ResearchProgress.ContainsKey(type)) {
            ResearchProgress[type] -= 1;
            if(ResearchProgress[type] <= 0) {
                ResearchProgress.Remove(type);
            }
        }

        if(ResearchProgress.Count == 0) {
            ResearchSteps[CurrentResearch].researchEffect.Invoke();
            NextResearch();
        }
    }

    public void R1IdentifyGears() {
        DiscoverPart(PartType.Gear);
    }

    public void R2UnlockBuildMachine()
    {

    }

    public void DiscoverPart(PartType type)
    {
        DiscoveredParts.Add(type);
        foreach (Part obj in Object.FindObjectsOfType<Part>()) {
            if (obj.Type == type) {
                obj.Discover();
            }
        }
    }

}
