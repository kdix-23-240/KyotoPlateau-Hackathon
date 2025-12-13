using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SlotIdAssigner : MonoBehaviour
{
    public Transform slotsRoot; // Cube をまとめた親

    [ContextMenu("Assign SlotID & BuildingIndex & Log")]
    public void AssignAndLogAll()
    {
        // Hierarchy順で取得（順序を固定）
        BuildingSlot[] slots =
            slotsRoot.GetComponentsInChildren<BuildingSlot>(true);

        int count = slots.Length;

        // 1) SlotID を 0～35 で自動付与（固定）
        for (int i = 0; i < count; i++)
        {
            slots[i].slotId = i;
            slots[i].gameObject.name = $"Slot_{i:D2}";
        }

        // 2) buildingIndex 用のランダム Queue を生成
        Queue<int> queue = GenerateRandomQueue(count);

        // 3) Console に Queue の中身を表示（壊さない）
        Debug.Log(
            "Queue contents (1-based): " +
            string.Join(", ", queue.ToArray().Select(i => (i + 1).ToString()))
        );

        // 4) Queue から取り出して buildingIndex を割当
        string log = "=== SlotID -> BuildingIndex ===\n";

        for (int i = 0; i < count; i++)
        {
            slots[i].buildingIndex = queue.Dequeue();

            log +=
                $"SlotID {slots[i].slotId:D2} -> Building {slots[i].buildingIndex + 1}\n";
        }

        // ⑤ 対応関係をまとめて Console 出力
        Debug.Log(log);
    }

    // 0～count-1 をランダム順にした Queue を作る
    Queue<int> GenerateRandomQueue(int count)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < count; i++) list.Add(i);

        // Fisher–Yates シャッフル
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }

        return new Queue<int>(list);
    }
}
