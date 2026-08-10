using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Sfx : MonoBehaviour
{
    const float Master = 1.0f;

    const float MusicVol = 0.22f;
    const float AmbienceVol = 0.10f;
    const float TickVol = 0.12f;
    const float KeyVol = 0.14f;
    const float SlideVol = 0.16f;
    const float DragVol = 0.18f;
    const float StepsVol = 0.22f;
    const float ButtonVol = 0.28f;
    const float EnterVol = 0.28f;
    const float DoorVol = 0.32f;
    const float BumpVol = 0.34f;
    const float CrackVol = 0.34f;
    const float CollectVol = 0.40f;
    const float BreakVol = 0.42f;
    const float UploadVol = 0.45f;
    const float ErrorVol = 0.45f;
    const float DeathVol = 0.58f;
    const float StingVol = 0.65f;
    const float lebronBaahVol = 0.3f;

    static Sfx instance;

    readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    AudioSource source;
    AudioSource music;
    AudioSource ambience;
    AudioSource steps;
    AudioSource dragging;

    float walking;

    [RuntimeInitializeOnLoadMethod]
    static void Create()
    {
        if (instance != null)
            return;

        var holder = new GameObject("Sfx");
        DontDestroyOnLoad(holder);
        holder.AddComponent<Sfx>();
    }

    void Awake()
    {
        instance = this;

        foreach (var clip in Resources.LoadAll<AudioClip>("Audio"))
            clips[clip.name] = clip;

        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;

        music = Loop("music", MusicVol);
        ambience = Loop("waves", AmbienceVol);

        if (music.clip != null)
            music.Play();
        steps = Loop("walk", StepsVol);
        dragging = Loop("drag", DragVol);

        if (ambience.clip != null)
            ambience.Play();
    }

    AudioSource Loop(string name, float volume)
    {
        var loop = gameObject.AddComponent<AudioSource>();
        loop.clip = Find(name);
        loop.loop = true;
        loop.volume = volume * Master;
        loop.playOnAwake = false;
        return loop;
    }

    void Update()
    {
        if (walking > 0f)
        {
            walking -= Time.deltaTime;
            if (walking <= 0f && steps.isPlaying)
                steps.Stop();
        }
    }

    static AudioClip Find(string name)
    {
        AudioClip clip;
        return instance != null && instance.clips.TryGetValue(name, out clip) ? clip : null;
    }

    static void Play(string name, float volume, float pitch)
    {
        var clip = Find(name);
        if (clip == null)
            return;

        instance.source.pitch = pitch;
        instance.source.PlayOneShot(clip, volume * Master);
    }

    public static void Walk()
    {
        if (instance == null || instance.steps.clip == null)
            return;

        instance.walking = 0.35f;
        if (!instance.steps.isPlaying)
            instance.steps.Play();
    }

    public static void Drag(bool held)
    {
        if (instance == null || instance.dragging.clip == null)
            return;

        if (held && !instance.dragging.isPlaying)
            instance.dragging.Play();
        else if (!held)
            instance.dragging.Stop();
    }

    public static void Collect()
    {
        Play("collect2", CollectVol, 1f);
    }

    public static void Give()
    {
        Play("collect2", CollectVol, 0.8f);
    }

    public static void Upload()
    {
        Play("upload", UploadVol, 1f);
    }

    public static void Error()
    {
        Play("error", ErrorVol, 1f);
    }

    public static void Key()
    {
        Play("keystroke", KeyVol, Random.Range(0.95f, 1.05f));
    }

    public static void Enter()
    {
        Play("commandenter", EnterVol, 1f);
    }

    public static void Tick()
    {
        Play("tick", TickVol, 1f);
    }

    public static void Bump()
    {
        Play("robotbump", BumpVol, Random.Range(0.96f, 1.04f));
    }

    public static void Death()
    {
        Play("robotdeath", DeathVol, 1f);
    }

    public static void SpikeBreak()
    {
        Play("spikebreak", CrackVol, 1f);
    }

    public static void BoxSlide()
    {
        Play("boxslide", SlideVol, Random.Range(0.96f, 1.04f));
    }

    public static void BoxBreak()
    {
        Play("boxbreak", BreakVol, 1f);
    }

    public static void ButtonPress()
    {
        Play("buttonpress", ButtonVol, 1f);
    }

    public static void Door(bool open)
    {
        Play(open ? "opendoor" : "closedoor", DoorVol, 1f);
    }

    public static void Win()
    {
        Play("levelwin", StingVol, 1f);
    }

    public static void Lose()
    {
        Play("levellose", StingVol, 1f);
    }

    public static void Baah()
    {
        Play("bah", lebronBaahVol, 1f);
    }
}
