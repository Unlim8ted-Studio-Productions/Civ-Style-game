using UnityEngine;


public class HexGridGenerator : MonoBehaviour
{
    public GameObject waterDeepTile;
    public GameObject waterShallowTile;
    public GameObject grassTile;
    public GameObject forestTile;
    public GameObject mountainTile;
    public int width = 200;
    public int height = 200;
    public float tileXOffset = .5f;
    public float tileZOffset = .5f;

    private float[,] heightMap;

    void Start()
    {
        GenerateHeightMap();
        GenerateMap();
    }

    void OnValidate()
    {
        if (Application.isPlaying) // Only regenerate the map if the application is playing
        {
            GenerateHeightMap();
            GenerateMap();
        }
    }

    void GenerateHeightMap()
    {
        heightMap = new float[width, height];
        float scale = 20.0f;
        float offsetX = Random.Range(0f, 999f);
        float offsetZ = Random.Range(0f, 999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float sampleX = x / scale + offsetX;
                float sampleZ = z / scale + offsetZ;
                heightMap[x, z] = Mathf.PerlinNoise(sampleX, sampleZ);
            }
        }
    }

    void GenerateMap()
    {
        ClearMap(); // Clear previous tiles before generating new ones
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject selectedTile = SelectTile(x, z);
                Vector3 position = CalculateWorldPosition(x, z);
                GameObject tile = Instantiate(selectedTile, position, Quaternion.identity, transform);
                SnapToGrid(tile.transform, position);
            }
        }
    }

    void ClearMap()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    GameObject SelectTile(int x, int z)
    {
        float height = heightMap[x, z];

        if (height < 0.4)
            return waterDeepTile;
        else if (height < 0.5)
            return waterShallowTile;
        else if (height < 0.6)
            return grassTile;
        else if (height < 0.8)
            return forestTile;
        else
            return mountainTile;
    }

    Vector3 CalculateWorldPosition(int x, int z)
    {
        float xPos = x * tileXOffset;
        if (x % 2 == 1)
        {
            xPos += tileXOffset / 2;
        }
        return new Vector3(xPos, 0, z * tileZOffset);
    }

    void SnapToGrid(Transform tileTransform, Vector3 initialPosition)
    {
        float width = 1.004f;  // Full width of the hex tile from the mesh bounds
        float depth = 1.154f;  // Full depth of the hex tile from the mesh bounds
        float verticalSpacing = depth * 0.75f;  // Vertical distance between rows
        float horizontalSpacing = width;  // Horizontal distance between columns

        int row = Mathf.RoundToInt(initialPosition.z / verticalSpacing);
        int col = Mathf.RoundToInt((initialPosition.x / horizontalSpacing) - (row % 2) * 0.5f);

        Vector3 position = initialPosition;
        position.x = col * horizontalSpacing + (row % 2) * horizontalSpacing / 2;
        position.z = row * verticalSpacing;

        tileTransform.position = position;  // Snap position
        tileTransform.rotation = Quaternion.Euler(0, 0, 0);  // Reset rotation if needed
    }
}
