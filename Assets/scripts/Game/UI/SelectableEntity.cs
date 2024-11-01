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
    private Material normalMaterial;
    public Material selectedMaterial;

    private void Start()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
        normalMaterial = GetComponent<Renderer>().material;
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
                        FindObjectOfType<GameController>().DeselectEntity();
                        selected = false;
                        if (OpenCraftMenu)
                        {
                            FindObjectOfType<GameController>().CloseCraftMenu();
                        }
                        foreach (Renderer renderer in childRenderers)
                        {
                            renderer.material = normalMaterial;
                        }
                    }
                }
            }
            if (Input.GetMouseButtonDown(1)) // Right-click
            {
                FindObjectOfType<GameController>().DeselectEntity();
                selected = false;
                if (OpenCraftMenu)
                {
                    FindObjectOfType<GameController>().CloseCraftMenu();
                }
                foreach (Renderer renderer in childRenderers)
                {
                    renderer.material = normalMaterial;
                }
            }
        }

    }

}
