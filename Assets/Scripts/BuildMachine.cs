using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildMachine : MonoBehaviour
{
    public Consumer input;
    public HashSet<PartType> DiscoveredParts = new HashSet<PartType>();
    Dictionary<PartType, int> Inventory = new Dictionary<PartType, int>();
    ScienceMachine sm;

    public GameObject ProductionIcon;
    public GameObject MissingIcon;
    public GameObject BuildButton;

    public GameObject FunnelPrefab;
    public GameObject FanPrefab;
    public GameObject BoosterPrefab;
    public GameObject PortalPrefab;

    CostItem[] FunnelCost = { new CostItem(PartType.Gear, 10) };
    CostItem[] FanCost = { new CostItem(PartType.Gear, 30), new CostItem(PartType.Plate, 4) };
    CostItem[] BoosterCost = { new CostItem(PartType.Gear, 10), new CostItem(PartType.Plate, 20) };
    CostItem[] PortalCost = { new CostItem(PartType.Gear, 50), new CostItem(PartType.Plate, 50), new CostItem(PartType.Circuit, 50) };

    private int current = 0;
    private GameObject[] buildOptions;


    CostItem[] CurrentCost()
    {
        switch (current) {
            default:
            case 0: return FunnelCost;
            case 1: return FanCost;
            case 2: return BoosterCost;
            case 3: return PortalCost;
        }
    }

    bool CurrentUnlocked()
    {
        switch (current) {
            default:
            case 0: return sm.FunnelUnlocked;
            case 1: return sm.FanUnlocked;
            case 2: return sm.BoosterUnlocked;
            case 3: return sm.PortalUnlocked;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buildOptions = new GameObject[4] { FunnelPrefab, FanPrefab, BoosterPrefab, PortalPrefab };
        foreach (PartType p in Part.AllTypes) {
            Inventory[p] = 0;
        }
        input.Handler = PartInput;
        sm = FindObjectOfType<ScienceMachine>();
        RefreshButtons();
    }

    void PartInput(PartType type)
    {
        Inventory[type]++;
        RefreshButtons();
    }

    bool InventoryHas(CostItem[] items)
    {
        foreach (var item in items) {
            if (Inventory[item.type] < PlayerData.ScaleCost(item.number)) {
                return false;
            }
        }
        return true;
    }

    PartType MissingPart()
    {
        foreach (var item in CurrentCost()) {
            if (Inventory[item.type] < PlayerData.ScaleCost(item.number)) {
                return item.type;
            }
        }
        return PartType.Gear;
    }

    void InventoryRemove(CostItem[] items)
    {
        foreach (var item in items) {
            Inventory[item.type] -= PlayerData.ScaleCost(item.number);
        }
    }

    public void NextOption()
    {
        current = (current + 1) % buildOptions.Length;
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        ProductionIcon.transform.localScale = new Vector3(1, 1, 1);
        ProductionIcon.GetComponent<SpriteRenderer>().sprite = buildOptions[current].GetComponentInChildren<SpriteRenderer>().sprite;

        var bounds = ProductionIcon.GetComponent<SpriteRenderer>().bounds;
        var yfactor = 0.9f / bounds.size.y;
        var xfactor = 0.9f / bounds.size.x;
        var factor = Mathf.Min(yfactor, xfactor);
        ProductionIcon.transform.localScale = new Vector3(factor, factor, factor);


        if (CurrentUnlocked()) {
            ProductionIcon.GetComponent<SetCensor>().CensorPixelLevel = 1;
            if (InventoryHas(CurrentCost())) {
                MissingIcon.SetActive(false);
                BuildButton.GetComponent<Image>().color = new Color(0.2f, 0.7f, 0.2f);
            } else {
                MissingIcon.SetActive(true);
                MissingIcon.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PlayerData>().PartSprite(MissingPart());

                MissingIcon.transform.localScale = new Vector3(1, 1, 1);

                var m_bounds = MissingIcon.GetComponent<SpriteRenderer>().bounds;
                var m_yfactor = 0.6f / m_bounds.size.y;
                var m_xfactor = 0.6f / m_bounds.size.x;
                var m_factor = Mathf.Min(m_yfactor, m_xfactor);
                MissingIcon.transform.localScale = new Vector3(m_factor, m_factor, m_factor);

                BuildButton.GetComponent<Image>().color = new Color(0.7f, 0.2f, 0.2f);
            }
        } else {
            ProductionIcon.GetComponent<SetCensor>().CensorPixelLevel = 64;
            MissingIcon.SetActive(false);
            BuildButton.GetComponent<Image>().color = new Color(0.7f, 0.2f, 0.2f);
        }
    }


    public void Build()
    {
        if (InventoryHas(CurrentCost()) && CurrentUnlocked()) {
            Instantiate(buildOptions[current], transform.position + new Vector3(-3, 0, 0), Quaternion.identity);
            InventoryRemove(CurrentCost());
            RefreshButtons();
        }
    }

    bool InObject = false;

    private void OnMouseEnter()
    {
        InObject = true;
    }

    private void OnMouseExit()
    {
        InObject = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1) && InObject) {
            NextOption();
        }
    }
}

