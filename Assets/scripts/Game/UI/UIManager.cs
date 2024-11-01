using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject resourcePanel;
    public GameObject infoPanel;

    public GameObject craftingPanel;

    [Header("Resource Texts")]
    public TMP_Text goldText;
    public TMP_Text ironText;
    public TMP_Text woodText;
    public TMP_Text stoneText;

    [Header("Info Texts")]
    public TMP_Text healthText;
    public TMP_Text speedText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateResourceUI(int gold, int iron, int wood, int stone)
    {
        goldText.text = "Gold: " + gold;
        ironText.text = "Iron: " + iron;
        woodText.text = "Wood: " + wood;
        stoneText.text = "Stone: " + stone;
    }

    public void UpdateInfoPanel(SelectableEntity entity)
    {
        healthText.text = "Health: " + entity.health;
        speedText.text = "Speed: " + entity.speed;
        infoPanel.SetActive(true);
    }

    public void OpenCraftUI()
    {
        craftingPanel.SetActive(true);
    }
    public void CloseCraftUI()
    {
        craftingPanel.SetActive(false);
    }

    public void HideInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }
}
