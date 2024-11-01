using UnityEngine;

public class GameController : MonoBehaviour
{
    public int gold, iron, wood, stone;

    private void Update()
    {
        // Example to update UI every frame; could be triggered by game events instead
        UIManager.Instance.UpdateResourceUI(gold, iron, wood, stone);
    }

    public void SelectEntity(SelectableEntity entity)
    {
        UIManager.Instance.UpdateInfoPanel(entity);
    }

    public void DeselectEntity()
    {
        UIManager.Instance.HideInfoPanel();
    }

    public void OpenCraftMenu(){
        UIManager.Instance.OpenCraftUI();
    }
        public void CloseCraftMenu(){
        UIManager.Instance.CloseCraftUI();
    }
}
