using UnityEditor;
using UnityEngine;

public class TileGenerator : EditorWindow
{
    float tempScale = 0.05f;
    float moistScale = 0.05f;
    float elevScale = 0.08f;
    GameObject tile;
    Vector2Int size = new Vector2Int(5, 5);

    [MenuItem("Window / Tile Generator")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(TileGenerator));
        window.position = new Rect(0, 0, 275, 150);
    }

    private void OnGUI()
    {
        tile = (GameObject)EditorGUI.ObjectField(new Rect(5f, 80f, position.width - 6f, 20f), "Tile", tile, typeof(GameObject), true);
        if (tile != null && !tile.GetComponent<Tile>())
            GUILayout.Label("No tile component attached to Tile object");

        size.x = EditorGUILayout.IntField("Width", size.x);
        size.y = EditorGUILayout.IntField("Length", size.y);

        if (GUILayout.Button("Generate"))
        {
            GenerateGrid();
        }
    }

    Vector2 TileSize()
    {
        Bounds b = tile.GetComponent<MeshFilter>().sharedMesh.bounds;

        float x = b.extents.x * 4;
        float y = b.extents.z * 4;

        return new Vector2(x, y);
    }

    void GenerateGrid()
    {
        Vector3 position = Vector3.zero;
        GameObject parent = new GameObject("Tiles");
        Vector2 tileSize = TileSize();

        for (int x = 0; x < size.x; x++)
        {
            position.x += tileSize.x;
            position.z = 0f;

            for (int z = 0; z < size.y; z++)
            {
                position.z += tileSize.y;

                CreateTile(position, new Vector2Int(x, z), parent.transform);
            }
        }
    }

    // void CreateTile(Vector3 pos, Vector2Int id, Transform parent)
    // {
    //     GameObject newTile = Instantiate(tile, pos, Quaternion.identity, parent.transform);
    //     newTile.GetComponent<Tile>().GridCoordinates = id;
    // }
    TerrainType GetBiome(float temperature, float moisture, float elevation)
    {
        if (elevation < 0.3f)
            return TerrainType.Water;
        if (elevation > 0.8f)
            return TerrainType.Mountain;

        if (temperature < 0.3f)
        {
            if (moisture < 0.3f) return TerrainType.Tundra;
            else if (moisture < 0.6f) return TerrainType.Taiga;
            else return TerrainType.SnowForest;
        }
        else if (temperature < 0.6f)
        {
            if (moisture < 0.3f) return TerrainType.Plains;
            else if (moisture < 0.6f) return TerrainType.Forest;
            else return TerrainType.Swamp;
        }
        else
        {
            if (moisture < 0.3f) return TerrainType.Desert;
            else if (moisture < 0.6f) return TerrainType.Savanna;
            else return TerrainType.Rainforest;
        }
    }
    
    void CreateTile(Vector3 pos, Vector2Int id, Transform parent)
    {
        float nx = (float)id.x / size.x;
        float ny = (float)id.y / size.y;

        float temperature = Mathf.PerlinNoise(nx / tempScale, ny / tempScale);
        float moisture = Mathf.PerlinNoise((nx + 100) / moistScale, (ny + 100) / moistScale);
        float elevation = Mathf.PerlinNoise((nx + 200) / elevScale, (ny + 200) / elevScale);

        TerrainType biome = GetBiome(temperature, moisture, elevation);

        GameObject newTile = Instantiate(tile, pos, Quaternion.identity, parent);
        
        Tile tileComponent = newTile.GetComponent<Tile>();
        tileComponent.GridCoordinates = id;
        tileComponent.Temperature = temperature;
        tileComponent.Moisture = moisture;
        tileComponent.Elevation = elevation;
        tileComponent.Terrain = biome;

        #if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(newTile, "Create Tile");
        #endif
    }
}