using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChecker : MonoBehaviour
{
    public string[] snowLayersNames = { "Snow1", "Snow2", "Snow3" };

    public Terrain currentTerrain;
    private TerrainData terrainData;

    public Material currentMaterial;

    private int[] snowLayersIndices;
    public string environmentType;

    private FootstepsManager[] footstepsManagers;

    private void Start()
    {
        snowLayersIndices = new int[snowLayersNames.Length];

        footstepsManagers = FindObjectsOfType<FootstepsManager>();
    }

    private void Update()
    {
        CheckTerrainUnderPlayer();

        if (currentTerrain == null && currentMaterial == null)
        {
            environmentType = "None";
            SetSurfaceToFootsteps();
            return;
        }

        if (currentTerrain != null)
        {
            Vector3 playerPos = transform.position;
            Vector3 terrainPos = playerPos - currentTerrain.transform.position;

            int mapX = Mathf.RoundToInt((terrainPos.x / terrainData.size.x) * terrainData.alphamapWidth);
            int mapZ = Mathf.RoundToInt((terrainPos.z / terrainData.size.z) * terrainData.alphamapHeight);

            float[,,] alphaMaps = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

            environmentType = GetSurfaceType(alphaMaps);
            SetSurfaceToFootsteps();
        }
        else if (currentMaterial != null)
        {
            environmentType = currentMaterial.name.Replace(" (Instance)", "");
            SetSurfaceToFootsteps();
        }
    }

    public void SetSurfaceToFootsteps()
    {
        foreach (var manager in footstepsManagers)
        {
            manager.SetSurface(environmentType);
        }
    }

    private void CheckTerrainUnderPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                if (terrain != currentTerrain)
                {
                    currentMaterial = null;
                    currentTerrain = terrain;
                    terrainData = terrain.terrainData;

                    snowLayersIndices = GetLayerIndices(snowLayersNames);
                }
            }
            else
            {
                currentTerrain = null;
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    currentMaterial = renderer.sharedMaterial;
                }
                else
                {
                    environmentType = "Unknown";
                }
            }
        }
        else
        {
            currentTerrain = null;
        }
    }

    private int[] GetLayerIndices(string[] layerNames)
    {
        int[] indices = new int[layerNames.Length];
        for (int i = 0; i < layerNames.Length; i++)
        {
            for (int j = 0; j < terrainData.terrainLayers.Length; j++)
            {
                if (terrainData.terrainLayers[j].name == layerNames[i])
                {
                    indices[i] = j;
                    break;
                }
            }
        }
        return indices;
    }

    public string GetSurfaceType(float[,,] alphaMaps)
    {
        bool isSnow = false;

        for (int i = 0; i < terrainData.terrainLayers.Length; i++)
        {
            float alpha = alphaMaps[0, 0, i];

            if (System.Array.Exists(snowLayersIndices, index => index == i) && alpha > 0.1f)
            {
                isSnow = true;
            }
            else if (alpha > 0.5f)
            {
                return terrainData.terrainLayers[i].name;
            }
        }

        return isSnow ? "Snow" : "Unknown";
    }
}
