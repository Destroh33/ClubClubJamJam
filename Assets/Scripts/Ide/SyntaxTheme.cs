using System;
using UnityEngine;

[Serializable]
public class SyntaxTheme
{
    public Color programName = new Color(1f, 0.83f, 0.45f);
    public Color keyword = new Color(0.55f, 0.82f, 1f);
    public Color number = new Color(0.98f, 0.62f, 0.55f);
    public Color fileName = new Color(0.68f, 0.9f, 0.62f);
    public Color comment = new Color(0.45f, 0.48f, 0.55f);
    public Color punctuation = new Color(0.62f, 0.65f, 0.72f);
    public Color plainText = new Color(0.88f, 0.9f, 0.94f);
    public Color errorLine = new Color(1f, 0.46f, 0.42f);
}
