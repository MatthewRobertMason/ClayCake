using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalClick : MonoBehaviour
{
    bool InObject = false;

    private void OnMouseEnter()
    {
        Debug.Log("IN PORTAL");
        InObject = true;
    }

    private void OnMouseExit()
    {
        Debug.Log("OUT PORTAL");
        InObject = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && InObject)
        {
            Debug.Log("CLICK PORTAL");
            FindObjectOfType<VictoryPanel>().SetVictoryPanelActive(true);
        }
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("CLICK PORTAL2");
        FindObjectOfType<VictoryPanel>().SetVictoryPanelActive(true);
    }
}