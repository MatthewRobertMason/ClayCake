using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanel : MonoBehaviour
{
    public GameObject victoryPanel;

    public void SetVictoryPanelActive(bool active)
    {
        victoryPanel.SetActive(active);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
