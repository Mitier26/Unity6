using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;
    public SerializableVector3(Vector3 v) { x = v.x; y = v.y; z = v.z; }
    public Vector3 ToVector3() => new Vector3(x, y, z);
}

[System.Serializable]
public class InventorySaveData
{
    public List<string> inventoryItemIDs = new List<string>();
}

[System.Serializable]
public class GameStateSaveData
{
    public string currentPuzzleStepID;
    public SerializableVector3 playerPosition;
    public float playerRotationY;
}

[System.Serializable]
public class WorldStateSaveData
{
    public List<string> collectedObjectIDs = new();
    public List<string> hiddenByPuzzleStepIDs = new();
}


public static class SaveSystem
{
    private static string InventoryPath => Path.Combine(Application.persistentDataPath, "inventory.json");
    private static string GameStatePath => Path.Combine(Application.persistentDataPath, "gamestate.json");

    private static string WorldStatePath => Path.Combine(Application.persistentDataPath, "worldstate.json");

    private static HashSet<string> collectedIDs = new();
    private static HashSet<string> hiddenByStepIDs = new();

    public static void SaveInventory(List<InventoryItemData> items)
    {
        InventorySaveData data = new InventorySaveData();
        foreach (var item in items)
        {
            data.inventoryItemIDs.Add(item.itemID);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(InventoryPath, json);
        Debug.Log("인벤토리 저장됨");
        Debug.Log(Application.persistentDataPath);
    }

    public static List<string> LoadInventory()
    {
        if (!File.Exists(InventoryPath))
        {
            Debug.LogWarning("인벤토리 저장 파일 없음");
            return new List<string>();
        }

        string json = File.ReadAllText(InventoryPath);
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);
        return data.inventoryItemIDs;
    }

    public static void SaveGameState()
    {
        var player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다.");
            return;
        }

        GameStateSaveData data = new GameStateSaveData
        {
            currentPuzzleStepID = PuzzleManager.Instance.CurrentStep?.stepID,
            playerPosition = new SerializableVector3(player.position),
            playerRotationY = player.eulerAngles.y
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GameStatePath, json);
        Debug.Log("게임 상태 저장됨");
    }

    public static GameStateSaveData LoadGameState()
    {
        if (!File.Exists(GameStatePath))
        {
            Debug.LogWarning("게임 상태 저장 파일 없음");
            return null;
        }

        string json = File.ReadAllText(GameStatePath);
        return JsonUtility.FromJson<GameStateSaveData>(json);
    }

    public static void SaveAll(List<InventoryItemData> items)
    {
        SaveInventory(items);
        SaveGameState();
    }

    public static void LoadAll(System.Action<List<string>> applyInventory,
        System.Action<GameStateSaveData> applyGameState)
    {
        var itemIDs = LoadInventory();
        applyInventory?.Invoke(itemIDs);

        var gameState = LoadGameState();
        if (gameState != null)
            applyGameState?.Invoke(gameState);
    }

    public static void MarkObjectCollected(string id)
    {
        collectedIDs.Add(id);
    }

    public static void MarkObjectHiddenByStep(string id)
    {
        hiddenByStepIDs.Add(id);
    }

    public static bool IsCollected(string id) => collectedIDs.Contains(id);
    public static bool IsHiddenByStep(string id) => hiddenByStepIDs.Contains(id);

    public static void SaveWorldState()
    {
        WorldStateSaveData data = new WorldStateSaveData
        {
            collectedObjectIDs = new List<string>(collectedIDs),
            hiddenByPuzzleStepIDs = new List<string>(hiddenByStepIDs)
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(WorldStatePath, json);
        Debug.Log("월드 상태 저장 완료");
    }

    public static void LoadWorldState()
    {
        if (!File.Exists(WorldStatePath)) return;

        string json = File.ReadAllText(WorldStatePath);
        var data = JsonUtility.FromJson<WorldStateSaveData>(json);

        collectedIDs = new HashSet<string>(data.collectedObjectIDs);
        hiddenByStepIDs = new HashSet<string>(data.hiddenByPuzzleStepIDs);
    }
    
    public static void DeleteAllSaves()
    {
        DeleteFile(InventoryPath);
        DeleteFile(GameStatePath);
        DeleteFile(WorldStatePath);
        Debug.Log("모든 저장 데이터 삭제 완료!");
    }

    private static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"삭제됨: {path}");
        }
        else
        {
            Debug.Log($"삭제할 파일 없음: {path}");
        }
    }
}
