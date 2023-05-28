using UnityEngine;
using UnityEngine.UI;

public class OneBPanelManager : MonoBehaviour
{
    public GameObject Panel;
    public bool checkButtonInteractive;
    public Sprite ImageActive;
    public Sprite ImageDeactive;
    public Image ImageButton;

    private void Start()
    {
        Panel.SetActive(false);
        if (checkButtonInteractive)
            ImageButton.sprite = ImageDeactive;
    }

    public void setPanel()
    {
        Panel.SetActive(!Panel.activeSelf);
        if (checkButtonInteractive)
            ImageButton.sprite = (Panel.activeSelf) ? ImageActive : ImageDeactive;
    }
}
