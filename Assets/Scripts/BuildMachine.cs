using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BuildMachine : MonoBehaviour
{
    public Consumer input;
    public HashSet<PartType> DiscoveredParts = new HashSet<PartType>();
    Dictionary<PartType, int> Inventory = new Dictionary<PartType, int>();
    ScienceMachine sm;

    // Start is called before the first frame update
    void Start()
    {
        foreach (PartType p in Part.AllTypes) {
            Inventory[p] = 0;
        }
        input.Handler = Input;
        RefreshButtons();
        sm = FindObjectOfType<ScienceMachine>();
    }

    void Input(PartType type)
    {
        Inventory[type]++;
        RefreshButtons();
    }

    bool InventoryHas(CostItem[] items)
    {
        foreach(var item in items) {
            if(Inventory[item.type] < item.number) {
                return false;
            }
        }
        return true;
    }

    void InventoryRemove(CostItem[] items)
    {
        foreach (var item in items) {
            Inventory[item.type] -= item.number;
        }
    }

    public void RefreshButtons()
    {
        FunnelButton.SetActive(InventoryHas(FunnelCost) && sm.FunnelUnlocked);
        FanButton.SetActive(InventoryHas(FanCost) && sm.FanUnlocked);
    }

    public GameObject FunnelButton;
    public GameObject FunnelPrefab;
    CostItem[] FunnelCost = { new CostItem(PartType.Gear, 10) };
    public void OnBuildFunnel()
    {
        if(InventoryHas(FunnelCost)){
            Instantiate(FunnelPrefab, transform.position + new Vector3(-3, 0, 0), Quaternion.identity);
            InventoryRemove(FunnelCost);
            RefreshButtons();
        }
    }

    public GameObject FanButton;
    public GameObject FanPrefab;
    CostItem[] FanCost = { new CostItem(PartType.Gear, 30) };
    public void OnBuildFan()
    {
        if (InventoryHas(FanCost)) {
            Instantiate(FanPrefab, transform.position + new Vector3(-3, 0, 0), Quaternion.identity);
            InventoryRemove(FanCost);
            RefreshButtons();
        }
    }
}
