using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HumanAging : MonoBehaviour
{
    public float maxAge = 100f;
    public float currentAge = 0f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        currentAge += Time.deltaTime;

        // 计算透明度（反比）
        float alpha = Mathf.Lerp(1f, 0f, currentAge / maxAge);
        Color color = sr.color;
        color.a = alpha;
        sr.color = color;

        // 到寿命终点后调用死亡逻辑
        if (currentAge >= maxAge)
        {
            GetComponent<HumanIdentity>()?.Die();
        }
    }
}

