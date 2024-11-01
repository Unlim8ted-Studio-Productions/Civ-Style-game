using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class SettlerUnit : MonoBehaviour
{
    public Material normalMaterial;
    public Material selectedMaterial;
    public GameObject castlePrefab;
    public GameObject settlerUnit;
    public Button settleButton;
    private NavMeshAgent agent;
    public LineRenderer pathLineRenderer;
    private bool isSelected = false;

    private bool moving = false;
    public float moveSpeed = 1.45f;

    private Renderer[] childRenderers;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>() ?? gameObject.AddComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        settleButton.gameObject.SetActive(false);
        settleButton.onClick.AddListener(Settle);
        childRenderers = GetComponentsInChildren<Renderer>();
        pathLineRenderer.enabled = false;
        pathLineRenderer.widthMultiplier = 0.1f;
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0)) // Left-click for selection/deselection
            {
                ProcessMouseSelection();
            }
            if (isSelected)
            {
                if (Input.GetMouseButtonDown(1)) // Right-click to initiate movement
                {
                    agent.speed = moveSpeed;
                    ProcessMovement();
                    DeselectUnit();
                    moving = true;
                }
                else
                {
                    if (isSelected){
                        update_path();
                    }
                    agent.speed = 0;
                    UpdatePathpreviewTowardsMouse();
                }
            }
            
            if (moving){
                update_path();
            }
        }
        else{
            pathLineRenderer.enabled=false;
        }

    }

    private void update_path()
    {
        if (agent.hasPath)
        {
            pathLineRenderer.positionCount = agent.path.corners.Length;
            pathLineRenderer.SetPositions(agent.path.corners);
            pathLineRenderer.enabled = true;
        }
        else
        {
            moving = false;
            pathLineRenderer.enabled = false;
        }
    }

    private void ProcessMouseSelection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.collider.gameObject == gameObject)
            {
                SelectUnit();
            }
            else if (isSelected)
            {
                DeselectUnit();
            }
        }
    }

    private void UpdatePathpreviewTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 100.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
        }
    }

    private void ProcessMovement()
    {
        // This method confirms the movement on right-click
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
        }
    }

    private void SelectUnit()
    {
        isSelected = true;
        settleButton.gameObject.SetActive(true);
    }

    private void DeselectUnit()
    {
        isSelected = false;
        settleButton.gameObject.SetActive(false);
        pathLineRenderer.enabled=false;
    }

    private void Settle()
    {
        if (settlerUnit == null) return; // Ensure the settler unit is still available

        // Using the hex grid snapping logic for placing the castle
        float width = 1.004f; // Full width of the hex tile from the mesh bounds
        float depth = 1.154f; // Full depth of the hex tile from the mesh bounds
        float verticalSpacing = depth * 0.75f; // Vertical distance between rows
        float horizontalSpacing = width; // Horizontal distance between columns

        Vector3 snapPosition = transform.position;
        int row = Mathf.RoundToInt(snapPosition.z / verticalSpacing);
        int col = Mathf.RoundToInt((snapPosition.x / horizontalSpacing) - (row % 2) * 0.5f);

        snapPosition.x = col * horizontalSpacing + (row % 2) * horizontalSpacing / 2;
        snapPosition.z = row * verticalSpacing;
        snapPosition.y = 0; // Ensuring the castle is placed at ground level

        Instantiate(castlePrefab, snapPosition, Quaternion.identity);
        settleButton.gameObject.SetActive(false);
        Destroy(settlerUnit);
    }
}
