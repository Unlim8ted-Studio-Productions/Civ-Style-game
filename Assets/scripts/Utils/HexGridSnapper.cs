using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class HexGridSnapper : MonoBehaviour
{
    void Update()
    {
        if (!Application.isPlaying)
        {
            SnapToGrid();
            SnapRotation();
        }
    }

    private void SnapToGrid()
    {
        float width = 1.004f; // Full width of the hex tile from the mesh bounds
        float depth = 1.154f; // Full depth of the hex tile from the mesh bounds
        float verticalSpacing = depth * 0.75f; // Vertical distance between rows
        float horizontalSpacing = width; // Horizontal distance between columns

        Vector3 position = transform.position;
        int row = Mathf.RoundToInt(position.z / verticalSpacing);
        int col = Mathf.RoundToInt((position.x / horizontalSpacing) - (row % 2) * 0.5f);

        position.x = col * horizontalSpacing + (row % 2) * horizontalSpacing / 2;
        position.z = row * verticalSpacing;
        position.y = 0; // Adjust y-position to a fixed level if necessary

        transform.position = position;
    }

    private void SnapRotation()
    {
        Quaternion rotation = transform.rotation;
        float yRotation = rotation.eulerAngles.y;
        float snapAngle = 60f; // Degrees to snap to
        float snappedYRotation = Mathf.Round(yRotation / snapAngle) * snapAngle;

        transform.rotation = Quaternion.Euler(0, snappedYRotation, 0);
    }
}
