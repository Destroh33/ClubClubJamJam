using UnityEngine;

public class GridOverlay : MonoBehaviour
{
    public int width = 20;
    public int height = 10;
    public Color color = new Color(0.05f, 0.06f, 0.12f, 0.5f);
    public float thickness = 0.03f;
    public int sortingOrder = -1;

    void Start()
    {
        var material = new Material(Shader.Find("Sprites/Default"));

        for (int x = 0; x <= width; x++)
            Line(material, new Vector3(x - 0.5f, -0.5f), new Vector3(x - 0.5f, height - 0.5f));

        for (int y = 0; y <= height; y++)
            Line(material, new Vector3(-0.5f, y - 0.5f), new Vector3(width - 0.5f, y - 0.5f));
    }

    void Line(Material material, Vector3 a, Vector3 b)
    {
        var holder = new GameObject("line");
        holder.transform.parent = transform;

        var line = holder.AddComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = 2;
        line.SetPosition(0, a);
        line.SetPosition(1, b);
        line.startWidth = thickness;
        line.endWidth = thickness;
        line.startColor = color;
        line.endColor = color;
        line.material = material;
        line.sortingOrder = sortingOrder;
    }
}
