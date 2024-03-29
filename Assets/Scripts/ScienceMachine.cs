﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct CostItem
{
    public CostItem(PartType i, int v)
    {
        type = i;
        number = v;
    }

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
    public bool hidden = true;
    public Consumer input;


    public HashSet<PartType> DiscoveredParts = new HashSet<PartType>();

    public ResearchProject[] ResearchSteps;
    public int CurrentResearch = -1;
    Dictionary<PartType, int> ResearchProgress;
    float totalResearchParts = 0;
    float currentResearchParts = 0;

    public SpriteRenderer machineSprite;
    public SpriteRenderer ProjectSprite;
    SetCensor SpriteCensor;
    const int MaxCensor = 32;
    bool MouseOver = false;

    public GameObject BuildMachinePrefab;

    public GameObject SpriteStrobePrefab;
    private GameObject CurrentSpriteStrobe;


    // Start is called before the first frame update
    void Start()
    {
        input.Handler = ProgressMade;
        SpriteCensor = ProjectSprite.gameObject.GetComponent<SetCensor>();
        NextResearch();
    }

    public void Show()
    {
        hidden = false;
        foreach(SpriteRenderer sp in GetComponentsInChildren<SpriteRenderer>()) {
            sp.enabled = true;
        }
    }

    bool OnFinalResearch()
    {
        return CurrentResearch == ResearchSteps.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && MouseOver && !OnFinalResearch()) {
            if (CurrentSpriteStrobe == null) {
                PartType type = PartType.Gear;
                foreach (var item in ResearchProgress.Keys) {
                    type = item;
                    break;
                }
                CurrentSpriteStrobe = Instantiate(SpriteStrobePrefab, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
                CurrentSpriteStrobe.GetComponent<SpriteStrobe>().DisplaySprite = FindObjectOfType<PlayerData>().PartSprite(type);
                if (!DiscoveredParts.Contains(type)) {
                    CurrentSpriteStrobe.GetComponent<SetCensor>().CensorPixelLevel = 64;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        MouseOver = true;
    }

    private void OnMouseExit()
    {
        MouseOver = false;
    }

    void NextResearch()
    {
        CurrentResearch++;
        ProjectSprite.sprite = ResearchSteps[CurrentResearch].icon;

        if (ProjectSprite.sprite) {
            var bounds = ProjectSprite.sprite.bounds;
            var yfactor = 0.6f / bounds.size.y;
            var xfactor = 0.6f / bounds.size.x;
            var factor = Mathf.Min(yfactor, xfactor);
            ProjectSprite.transform.localScale = new Vector3(factor, factor, factor);
            SpriteCensor.CensorPixelLevel = MaxCensor;
        }

        ResearchProgress = new Dictionary<PartType, int>();
        totalResearchParts = 0;
        currentResearchParts = 0;
        foreach (CostItem item in ResearchSteps[CurrentResearch].cost) {
            int number = PlayerData.ScaleCost(item.number);
            ResearchProgress[item.type] = number;
            totalResearchParts += number;
        }
    }


    void ProgressMade(PartType type)
    {
        if (OnFinalResearch()) {
            input.gameObject.SetActive(false);
        }

        Debug.LogFormat("Science Machine got {0}", type);
        if (ResearchProgress.ContainsKey(type)) {
            ResearchProgress[type] -= 1;
            if(ResearchProgress[type] <= 0) {
                ResearchProgress.Remove(type);
            }
            currentResearchParts += 1;
            SpriteCensor.CensorPixelLevel = (int)System.Math.Ceiling(MaxCensor * (1 - currentResearchParts/totalResearchParts));
        }

        if(ResearchProgress.Count == 0) {
            ResearchSteps[CurrentResearch].researchEffect.Invoke();
            NextResearch();
        }
    }

    public void R0UnlockResearch()
    {
        GetComponent<SetCensor>().CensorPixelLevel = 1;
    }

    public void R1IdentifyGears() {
        DiscoverPart(PartType.Gear);
    }

    public void R2UnlockBuildMachine()
    {
        Instantiate(BuildMachinePrefab, transform.position + new Vector3(13, 0, 0), Quaternion.identity);
    }

    public bool FunnelUnlocked = false;
    public void R3UnlockFunnel()
    {
        FunnelUnlocked = true;
        foreach(BuildMachine b in Object.FindObjectsOfType<BuildMachine>()) {
            b.RefreshButtons();
        }
    }

    public bool PageMovement = false;
    public void R4UnlockMoving()
    {
        PageMovement = true;
    }


    public bool FanUnlocked = false;
    public void R5UnlockFan()
    {
        FanUnlocked = true;
        foreach (BuildMachine b in Object.FindObjectsOfType<BuildMachine>()) {
            b.RefreshButtons();
        }
    }

    public void R6IdentifyPlate()
    {
        DiscoverPart(PartType.Plate);
    }

    public bool BoosterUnlocked = false;
    public void R7UnlockBooster()
    {
        BoosterUnlocked = true;
        foreach (BuildMachine b in Object.FindObjectsOfType<BuildMachine>()) {
            b.RefreshButtons();
        }
    }

    public void R8IdentifyCircuit()
    {
        DiscoverPart(PartType.Circuit);
    }

    public bool PortalUnlocked = false;
    public void R9UnlockPortal()
    {
        PortalUnlocked = true;
        foreach (BuildMachine b in Object.FindObjectsOfType<BuildMachine>()) {
            b.RefreshButtons();
        }
    }

    public void R10RunForever()
    {
        CurrentResearch--;
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
