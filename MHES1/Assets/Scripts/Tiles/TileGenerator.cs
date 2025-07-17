using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class TileGenerator : EditorWindow
{
    [System.Serializable]
    public class BiomePrefab
    {
        public TerrainType terrainType;
        public GameObject prefab;
    }

    public BiomeSet biomeSet;
    private Dictionary<TerrainType, GameObject> prefabLookup;

    float tempScale = 0.05f;
    float moistScale = 0.05f;
    float elevScale = 0.08f;
    
    Vector2Int size = new Vector2Int(5, 5);

    [MenuItem("Window / Tile Generator")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(TileGenerator));
        window.position = new Rect(0, 0, 275, 150);
    }

    private void OnGUI()
    {
        size.x = EditorGUILayout.IntField("Width", size.x);
        size.y = EditorGUILayout.IntField("Length", size.y);

        if (GUILayout.Button("Generate"))
        {
            GenerateGrid();
        }
    }

    Vector2 TileSize()
    {
        if (biomeSet == null)
        {
            biomeSet = AssetDatabase.LoadAssetAtPath<BiomeSet>("Assets/Scripts/Tiles/DefaultBiomeSet.asset");
            if (biomeSet == null)
            {
                Debug.LogError("DefaultBiomeSet.asset not found in Assets/Scripts/Tiles/");
                return new Vector2(1, 1); // Safe fallback
            }
        }

        foreach (var biome in biomeSet.biomePrefabs)
        {
            if (biome.prefab != null)
            {
                var meshFilter = biome.prefab.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    Bounds b = meshFilter.sharedMesh.bounds;
                    float x = b.extents.x * 4;
                    float y = b.extents.z * 4;
                    return new Vector2(x, y);
                }
            }
        }

        Debug.LogWarning("No valid prefab in biome set to calculate tile size. Defaulting to (1, 1).");
        return new Vector2(1, 1);
    }

    void GenerateGrid()
    {
        Vector3 position = Vector3.zero;
        GameObject parent = new GameObject("Tiles");
        Vector2 tileSize = TileSize();
        
        if (biomeSet == null)
        {
            biomeSet = AssetDatabase.LoadAssetAtPath<BiomeSet>("Assets/Scripts/Tiles/DefaultBiomeSet.asset");
            if (biomeSet == null)
            {
                Debug.LogError("DefaultBiomeSet.asset not found in Assets/Scripts/Tiles/.");
                return;
            }
        }

        // Build the prefab lookup
        prefabLookup = new Dictionary<TerrainType, GameObject>();
        foreach (var biome in biomeSet.biomePrefabs)
        {
            if (!prefabLookup.ContainsKey(biome.terrainType))
                prefabLookup.Add(biome.terrainType, biome.prefab);
        }

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

        if (!prefabLookup.ContainsKey(biome))
        {
            Debug.LogWarning($"No prefab assigned for biome: {biome}. Tile skipped.");
            return;
        }

        GameObject selectedPrefab = prefabLookup[biome];
        GameObject newTile = Instantiate(selectedPrefab, pos, Quaternion.identity, parent);
        
        Tile tileComponent = newTile.GetComponent<Tile>();
        tileComponent.GridCoordinates = id;
        tileComponent.Temperature = temperature;
        tileComponent.Moisture = moisture;
        tileComponent.Elevation = elevation;
        tileComponent.Terrain = biome;

        #if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(newTile, "Create Tile");
        #endif
    }
}