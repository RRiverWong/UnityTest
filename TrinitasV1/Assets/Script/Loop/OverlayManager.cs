using System.Collections;
using UnityEngine;
using TMPro;
using System; // ✅ 新增：用于 Action

public class OverlayManager : MonoBehaviour
{
    public CanvasGroup fadePanel;         // 白色覆盖面板
    public CanvasGroup summaryGroup;      // 文字整体容器（可以为空）
    public TextMeshProUGUI line1Text;
    public TextMeshProUGUI line2Text;
    public TextMeshProUGUI line3Text;

    public float fadeDuration = 1.5f;
    public float waitPerLine = 2f;

    // ✅ 修改：添加 Action 回调
    public IEnumerator RunSummarySequence(string[] lines, Action onFadeOutComplete = null)
    {
        summaryGroup.gameObject.SetActive(true);

        // 初始化
        line1Text.text = "";
        line2Text.text = "";
        line3Text.text = "";
        summaryGroup.alpha = 1;
        fadePanel.alpha = 0;

        // 1. 淡入白色背景
        yield return StartCoroutine(FadeToWhite());

        // 2. 分行淡入、停留、淡出
        yield return StartCoroutine(ShowLine(line1Text, lines.Length > 0 ? lines[0] : ""));
        yield return StartCoroutine(ShowLine(line2Text, lines.Length > 1 ? lines[1] : ""));
        yield return StartCoroutine(ShowLine(line3Text, lines.Length > 2 ? lines[2] : ""));

        // 3. 淡出白色背景
        yield return StartCoroutine(FadeFromWhite());

        summaryGroup.gameObject.SetActive(false);

        // ✅ 调用回调（如果有）
        onFadeOutComplete?.Invoke();
    }

    IEnumerator ShowLine(TextMeshProUGUI target, string content)
    {
        target.text = content;
        target.canvasRenderer.SetAlpha(0);
        target.CrossFadeAlpha(1, fadeDuration, false);
        yield return new WaitForSeconds(waitPerLine);
        target.CrossFadeAlpha(0, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }

    public IEnumerator FadeToWhite()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 1;
    }

    IEnumerator FadeFromWhite()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 0;
    }
    // ✅ 新增：纯粹淡入白色背景（不带文字）
    public IEnumerator PureFadeToWhite()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 1;
    }

    // ✅ 新增：纯粹淡出白色背景（不带文字）
    public IEnumerator PureFadeFromWhite()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 0;
    }

}
