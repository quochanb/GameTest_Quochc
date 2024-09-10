using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private Transform blockParent;
    [SerializeField] private ButtonItem[] blockButtons;
    [SerializeField] private GameObject blockPrefab;
    private string dataSavePath;

    private void Start()
    {
        dataSavePath = Application.persistentDataPath + "/gamestate.dat";
        LoadData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveData();
            Application.Quit();
        }
    }

    public void SaveData()
    {
        int[] blockQuantities = new int[blockButtons.Length];
        for (int i = 0; i < blockQuantities.Length; i++)
        {
            blockQuantities[i] = blockButtons[i].GetQuantity();
        }

        List<BlockData> blockDataList = new List<BlockData>();
        foreach (Transform block in blockParent)
        {
            Vector3 position = block.position;
            Color color = block.GetComponent<MeshRenderer>().material.color;
            blockDataList.Add(new BlockData(position, color));
        }

        GameState gameState = new GameState(blockQuantities, blockDataList);
        string jsonData = JsonUtility.ToJson(gameState);
        File.WriteAllText(dataSavePath, jsonData);
    }

    public void LoadData()
    {
        if (File.Exists(dataSavePath))
        {
            string jsonData = File.ReadAllText(dataSavePath);
            if (string.IsNullOrEmpty(jsonData))
            {
                GameState gameState = JsonUtility.FromJson<GameState>(jsonData);

                for (int i = 0; i < blockButtons.Length; i++)
                {
                    blockButtons[i].UpdateQuantity(gameState.blockQuantities[i] - blockButtons[i].GetQuantity());
                }

                foreach (Transform block in blockParent)
                {
                    if (block != null)
                    {
                        Destroy(block.gameObject);
                    }
                }

                foreach (BlockData blockData in gameState.blockList)
                {
                    GameObject newObject = Instantiate(blockPrefab, blockParent);
                    newObject.GetComponent<MeshRenderer>().material.color = blockData.color;
                    newObject.transform.position = blockData.position;
                }
            }
        }
    }
}
