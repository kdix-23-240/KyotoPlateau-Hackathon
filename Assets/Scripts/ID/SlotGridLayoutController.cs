using UnityEngine;

[ExecuteAlways]
public class SlotGridLayoutController : MonoBehaviour
{
    [Header("Block Grid (全体)")]
    public int blockColumns = 3;
    public int blockRows = 3;

    [Header("Cubes per Block")]
    public int cubesPerBlockX = 2;
    public int cubesPerBlockZ = 2;

    [Header("Spacing(ここで調節)")]
    public float cubeSpacing = 1.2f;   // Cube 同士
    public float blockSpacing = 3.0f;  // ブロック同士

    [Header("Offset")]
    public Vector3 startOffset = Vector3.zero;

    public void UpdateLayout()
    {
        int index = 0;

        for (int bz = 0; bz < blockRows; bz++)
        {
            for (int bx = 0; bx < blockColumns; bx++)
            {
                for (int cz = 0; cz < cubesPerBlockZ; cz++)
                {
                    for (int cx = 0; cx < cubesPerBlockX; cx++)
                    {
                        if (index >= transform.childCount)
                            return;

                        Transform cube = transform.GetChild(index);

                        float x =
                            bx * (cubesPerBlockX * cubeSpacing + blockSpacing)
                            + cx * cubeSpacing;

                        float z =
                            bz * (cubesPerBlockZ * cubeSpacing + blockSpacing)
                            + cz * cubeSpacing;

                        cube.localPosition =
                            new Vector3(x, 0, z) + startOffset;

                        index++;
                    }
                }
            }
        }
    }

    void OnValidate()
    {
        UpdateLayout();
    }
}
