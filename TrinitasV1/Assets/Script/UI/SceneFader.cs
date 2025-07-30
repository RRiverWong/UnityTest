using UnityEngine;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public float fadeDuration = 1.5f;

    public IEnumerator FadeInWhite()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = 1;
    }

    public IEnumerator FadeOutWhite()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = 0;
    }
}
