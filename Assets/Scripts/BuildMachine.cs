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

    // Start is called before the first frame update
    void Start()
    {
        foreach (PartType p in Part.AllTypes) {
            Inventory[p] = 0;
        }
        input.Handler = Input;
        RefreshButtons();
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

    void RefreshButtons()
    {
        FunnelButton.SetActive(InventoryHas(FunnelCost));
    }

    public GameObject FunnelButton;
    CostItem[] FunnelCost = { new CostItem(PartType.Gear, 10) };
    public void OnBuildFunnel()
    {
        Debug.Log("Funnel");
        RefreshButtons();
    }
}
