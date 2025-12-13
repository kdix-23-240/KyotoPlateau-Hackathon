using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BuildingGridController : MonoBehaviour
{
    [Header("Databases")]
    public BuildingObjectDatabase kyotoDatabase; // 京都
    public BuildingObjectDatabase otherDatabase; // その他（1個使う）

    [Header("Slots")]
    public Transform slotsRoot; // 36個のCubeの親

    void Start()
    {
        GenerateAndReplace();
    }

    void GenerateAndReplace()
    {
        // --- Slot取得 ---
        BuildingSlot[] slots =
            slotsRoot.GetComponentsInChildren<BuildingSlot>(true);

        int slotCount = slots.Length; // 36想定
        if (slotCount != 36)
        {
            Debug.LogError($"Slot数が36ではありません: {slotCount}");
            return;
        }

        // --- Databaseチェック ---
        if (kyotoDatabase.OtherBuildings.Length == 0)
        {
            Debug.LogError("KyotoDatabase に BuildingData がありません");
            return;
        }

        if (otherDatabase.OtherBuildings.Length == 0)
        {
            Debug.LogError("OtherDatabase に BuildingData がありません");
            return;
        }

        // ===============================
        // ① Other から必ず1個選ぶ
        // ===============================
        BuildingData otherBuilding =
            otherDatabase.OtherBuildings[
                Random.Range(0, otherDatabase.OtherBuildings.Length)
            ];

        // ===============================
        // ② Kyoto 用：一巡保証Queueを作る
        // ===============================
        Queue<BuildingData> kyotoPrimaryQueue =
            new Queue<BuildingData>(
                kyotoDatabase.OtherBuildings
                    .OrderBy(_ => Random.value)
            );

        List<BuildingData> finalList = new List<BuildingData>();
        finalList.Add(otherBuilding); // ← Other を必ず1回

        // ===============================
        // ③ 残り35個を Kyoto で埋める
        // ===============================
        while (finalList.Count < slotCount)
        {
            if (kyotoPrimaryQueue.Count > 0)
            {
                // まずは全種類を一度ずつ
                finalList.Add(kyotoPrimaryQueue.Dequeue());
            }
            else
            {
                // 使い切った後は重複OK
                int r = Random.Range(0, kyotoDatabase.OtherBuildings.Length);
                finalList.Add(kyotoDatabase.OtherBuildings[r]);
            }
        }

        // ===============================
        // ④ 最終シャッフル（配置ランダム）
        // ===============================
        Queue<BuildingData> finalQueue =
            new Queue<BuildingData>(
                finalList.OrderBy(_ => Random.value)
            );

        Debug.Log("=== Final Building Queue ===");
        Debug.Log(string.Join(
            ", ",
            finalQueue.Select(b => b.BuildingName)
        ));

        // ===============================
        // ⑤ 全部そろってから一括置き換え
        // ===============================
        foreach (var slot in slots)
        {
            BuildingData data = finalQueue.Dequeue();

            Instantiate(
                data.BuildingObject,
                slot.transform.position,
                slot.transform.rotation
            );

            Destroy(slot.gameObject);
        }

        Debug.Log("Kyoto + Other（35 + 1）配置完了");
    }
}
