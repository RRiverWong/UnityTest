using UnityEngine;

public enum RelationshipType
{
    ParentChild,
    Sibling,
    DistantRelative
}

public class GravitySoundManager : MonoBehaviour
{
    public float minFrequency = 200f;  // 提高最低频率
    public float maxFrequency = 1200f;
    public float duration = 0.5f;
    public float volume = 0.05f;

    public AnimationCurve envelopeCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.03f, 1f),
        new Keyframe(0.2f, 0.6f),
        new Keyframe(0.4f, 0.2f),
        new Keyframe(0.5f, 0f)
    );

    public void PlayRelationshipTone(float strength01, Vector3 from, Vector3 to, RelationshipType type)
    {
        float frequency = Mathf.Lerp(maxFrequency, minFrequency, strength01);
        float harmonyRatio = GetHarmonyRatio(type);
        AudioClip tone = GenerateHarmonicSineWave(frequency, harmonyRatio, duration);

        GameObject tempAudio = new GameObject("TempAudio");
        tempAudio.transform.position = from;
        tempAudio.transform.SetParent(this.transform);

        AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
        tempSource.clip = tone;
        tempSource.volume = volume;
        tempSource.loop = false;
        tempSource.playOnAwake = false;

        tempSource.pitch = Random.Range(0.95f, 1.05f);

        float pan = Mathf.Clamp((to.x - from.x) / 5f, -1f, 1f);
        tempSource.spatialBlend = 0f;
        tempSource.panStereo = pan;

        AudioLowPassFilter filter = tempAudio.AddComponent<AudioLowPassFilter>();
        float dist = Vector3.Distance(from, to);
        float cutoff = Mathf.Lerp(10000f, 3000f, Mathf.Clamp01(dist / 10f));  // 默认更亮
        filter.cutoffFrequency = cutoff;

        switch (type)
        {
            case RelationshipType.ParentChild:
                filter.cutoffFrequency *= 0.9f;
                tempSource.volume *= 1.1f;
                break;
            case RelationshipType.Sibling:
                break;
            case RelationshipType.DistantRelative:
                filter.cutoffFrequency *= 1.2f;
                tempSource.pitch += 0.05f;
                break;
        }

        tempSource.Play();
        Destroy(tempAudio, duration + 0.1f);
    }

    private float GetHarmonyRatio(RelationshipType type)
    {
        return type switch
        {
            RelationshipType.ParentChild => 1.5f,
            RelationshipType.Sibling => 1.25f,
            RelationshipType.DistantRelative => 1.41f,
            _ => 1.5f
        };
    }

    private AudioClip GenerateHarmonicSineWave(float frequency, float harmonyRatio, float duration)
    {
        int sampleRate = 44100;
        int sampleCount = (int)(sampleRate * duration);
        float[] samples = new float[sampleCount];

        float harmonyFreq = frequency * harmonyRatio;
        float brightFreq = frequency * 3.0f;  // 添加三倍频作为高泛音

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = envelopeCurve.Evaluate(t / duration);

            float primary = Mathf.Sin(2 * Mathf.PI * frequency * t);
            float harmony = Mathf.Sin(2 * Mathf.PI * harmonyFreq * t);
            float bright = Mathf.Sin(2 * Mathf.PI * brightFreq * t);

            samples[i] = envelope * (0.5f * primary + 0.3f * harmony + 0.2f * bright);
        }

        AudioClip clip = AudioClip.Create("HarmonicWave", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
