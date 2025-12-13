using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BuildingGridController : MonoBehaviour
{
    [Header("Databases")]
    public BuildingObjectDatabase kyotoDatabase; // 京都
    public BuildingObjectDatabase otherDatabase; // その他

    [Header("Slots")]
    public Transform slotsRoot; // 36個のCubeの親

    [Header("Generation Settings")]
    [SerializeField] private int _numberOfKyotoBuildings = 35;
    [SerializeField] private int _numberOfOtherBuildings = 1;

    void Start()
    {
        GenerateAndReplace();
    }
    
    /// <summary>
    /// Creates a new queue of building data, shuffled randomly.
    /// </summary>
    private Queue<BuildingData> CreateShuffledQueue(BuildingData[] database)
    {
        if (database == null || database.Length == 0)
        {
            return new Queue<BuildingData>();
        }
        return new Queue<BuildingData>(database.OrderBy(_ => Random.value));
    }

    void GenerateAndReplace()
    {
        // --- Slot取得 ---
        BuildingSlot[] slots = slotsRoot.GetComponentsInChildren<BuildingSlot>(true);
        int actualSlotCount = slots.Length;

        if (actualSlotCount == 0)
        {
            Debug.LogError("No BuildingSlots found under slotsRoot. Cannot generate any buildings.");
            return;
        }

        // --- Databaseチェック ---
        if (kyotoDatabase == null || kyotoDatabase.OtherBuildings == null || kyotoDatabase.OtherBuildings.Length == 0)
        {
            Debug.LogError("KyotoDatabase is not properly configured or is empty.");
            _numberOfKyotoBuildings = 0; // Cannot generate Kyoto buildings
        }
        if (otherDatabase == null || otherDatabase.OtherBuildings == null || otherDatabase.OtherBuildings.Length == 0)
        {
            Debug.LogError("OtherDatabase is not properly configured or is empty.");
            _numberOfOtherBuildings = 0; // Cannot generate Other buildings
        }

        // --- 入力値のバリデーション ---
        _numberOfKyotoBuildings = Mathf.Max(0, _numberOfKyotoBuildings);
        _numberOfOtherBuildings = Mathf.Max(0, _numberOfOtherBuildings);

        int totalRequestedBuildings = _numberOfKyotoBuildings + _numberOfOtherBuildings;
        
        int finalKyotoCount = _numberOfKyotoBuildings;
        int finalOtherCount = _numberOfOtherBuildings;

        if (totalRequestedBuildings > actualSlotCount)
        {
            Debug.LogWarning($"Requested to generate {totalRequestedBuildings} buildings, but only {actualSlotCount} slots available. Adjusting counts.");
            // Adjust counts proportionally, but this can get complex. A simpler approach is to prioritize one type.
            // Let's prioritize 'Other' as the original logic did.
            finalOtherCount = Mathf.Min(_numberOfOtherBuildings, actualSlotCount);
            finalKyotoCount = actualSlotCount - finalOtherCount;
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.TotalOthersBuildingCount = finalOtherCount;
        }
        else
        {
            Debug.LogError("ScoreManager.Instance is null. Cannot set TotalOthersBuildingCount.");
        }


        List<BuildingData> finalList = new List<BuildingData>();
        var kyotoQueue = CreateShuffledQueue(kyotoDatabase.OtherBuildings);
        var otherQueue = CreateShuffledQueue(otherDatabase.OtherBuildings);

        // ===============================
        // ① 'Other' Buildings を選ぶ
        // ===============================
        for (int i = 0; i < finalOtherCount; i++)
        {
            if (otherQueue.Count == 0)
            {
                otherQueue = CreateShuffledQueue(otherDatabase.OtherBuildings);
            }
            finalList.Add(otherQueue.Dequeue());
        }

        // ===============================
        // ② 'Kyoto' Buildings を選ぶ
        // ===============================
        for (int i = 0; i < finalKyotoCount; i++)
        {
            if (kyotoQueue.Count == 0)
            {
                kyotoQueue = CreateShuffledQueue(kyotoDatabase.OtherBuildings);
            }
            finalList.Add(kyotoQueue.Dequeue());
        }
        
        // ===============================
        // ③ 最終シャッフル（配置ランダム）
        // ===============================
        Queue<BuildingData> finalQueue =
            new Queue<BuildingData>(
                finalList.OrderBy(_ => Random.value)
            );

        Debug.Log("=== Final Building Queue ===");
        Debug.Log(string.Join(", ", finalQueue.Select(b => b.BuildingName)));

        // ===============================
        // ④ 全部そろってから一括置き換え
        // ===============================
        for (int i = 0; i < actualSlotCount; i++)
        {
            if (finalQueue.Count > 0)
            {
                BuildingData data = finalQueue.Dequeue();
                Instantiate(data.BuildingObject, slots[i].transform.position, slots[i].transform.rotation, slotsRoot);
            }
            // Destroy the slot regardless of whether it was filled
            Destroy(slots[i].gameObject);
        }
        
        Debug.Log($"Buildings配置完了: 生成された京都 {finalKyotoCount} + その他 {finalOtherCount}. 合計 {finalList.Count}.");
    }
}
