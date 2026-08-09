using UnityEngine;

public class NameTag : MonoBehaviour
{
    public TextMesh text;
    public string label;

    void Start()
    {
        if (text != null)
            text.text = label;
    }
}
