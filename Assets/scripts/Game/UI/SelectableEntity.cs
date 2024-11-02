using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectableEntity : MonoBehaviour
{
    public int health;
    public float speed;
    public bool OpenCraftMenu = false;
    private bool selected = false;
    private Renderer[] childRenderers;
    private Material[] normalMaterials; // Array to store original materials
    public Material selectedMaterial;

    private void Start()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
        normalMaterials = new Material[childRenderers.Length];

        // Store the original material for each child renderer
        for (int i = 0; i < childRenderers.Length; i++)
        {
            normalMaterials[i] = childRenderers[i].material;
        }
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0)) // Left-click for selection/deselection
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        FindObjectOfType<GameController>().SelectEntity(this);
                        foreach (Renderer renderer in childRenderers)
                        {
                            renderer.material = selectedMaterial;
                        }
                        selected = true;
                        if (OpenCraftMenu)
                        {
                            FindObjectOfType<GameController>().OpenCraftMenu();
                        }
                    }
                    else if (selected)
                    {
                        DeselectEntity();
                    }
                }
            }

            if (Input.GetMouseButtonDown(1)) // Right-click
            {
                DeselectEntity();
            }
        }
    }

    private void DeselectEntity()
    {
        FindObjectOfType<GameController>().DeselectEntity();
        selected = false;

        if (OpenCraftMenu)
        {
            FindObjectOfType<GameController>().CloseCraftMenu();
        }

        // Restore each renderer to its original material
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material = normalMaterials[i];
        }
    }
}
