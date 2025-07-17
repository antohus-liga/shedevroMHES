using UnityEngine;

[CreateAssetMenu(fileName = "BiomeSet", menuName = "Tile Generator/Biome Set")]
public class BiomeSet : ScriptableObject
{
    [System.Serializable]
    public class BiomePrefab
    {
        public TerrainType terrainType;
        public GameObject prefab;
    }

    public BiomePrefab[] biomePrefabs;
}