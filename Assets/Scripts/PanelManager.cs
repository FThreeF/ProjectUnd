using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PanelSystem
{
    public GameObject buttonInteractive;
    public GameObject panel;
}

public class PanelManager : MonoBehaviour
{
    public List<PanelSystem> panelSystem;
    public bool checkButtonInteractive = true;
    public Sprite ButtonActive;
    public Sprite ButtonDeactive;
    public void setPanel(GameObject panel)
    {
        foreach (var element in panelSystem)
        {
            element.panel.SetActive(element.panel == panel);
            if (checkButtonInteractive)
                element.buttonInteractive.GetComponent<Image>().sprite = (element.panel == panel) ? ButtonActive : ButtonDeactive;
        }
    }

    public void setOffPanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
