using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    const float Duration = 0.55f;

    static readonly Color Sheet = new Color(0.055f, 0.063f, 0.09f, 1f);

    static SceneFade instance;

    Image cover;
    float alpha = 1f;
    float target;
    string pending;

    [RuntimeInitializeOnLoadMethod]
    static void Create()
    {
        if (instance != null)
            return;

        var holder = new GameObject("SceneFade");
        DontDestroyOnLoad(holder);
        instance = holder.AddComponent<SceneFade>();
        instance.Build();
    }

    void Build()
    {
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;

        var holder = new GameObject("Cover", typeof(RectTransform));
        holder.transform.SetParent(transform, false);

        cover = holder.AddComponent<Image>();
        cover.color = Sheet;
        cover.raycastTarget = false;

        var rect = cover.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    void Update()
    {
        alpha = Mathf.MoveTowards(alpha, target, Time.unscaledDeltaTime / Duration);

        var shade = Sheet;
        shade.a = alpha;
        cover.color = shade;
        cover.raycastTarget = alpha > 0.02f;

        if (pending == null || alpha < 1f)
            return;

        string scene = pending;
        pending = null;
        target = 0f;
        SceneManager.LoadScene(scene);
    }

    public static void Go(string scene)
    {
        if (instance == null)
        {
            SceneManager.LoadScene(scene);
            return;
        }

        instance.pending = scene;
        instance.target = 1f;
    }
}
