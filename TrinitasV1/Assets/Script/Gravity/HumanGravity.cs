using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(HumanIdentity))]
public class HumanGravity : MonoBehaviour
{
    public float baseStrength = 25f;
    public float maxSpeed = 1.2f;
    public float drag = 2.5f;
    public float noiseStrength = 0.1f;

    private Rigidbody2D rb;
    private HumanIdentity identity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearDamping = drag;
        identity = GetComponent<HumanIdentity>();

        HumanGravityManager.Register(this);
    }

    void OnDestroy()
    {
        HumanGravityManager.Unregister(this);
    }

    public void ApplyForce(Vector2 force)
    {
        if (!identity.IsAlive) return;

        // 添加扰动模拟漂浮感
        Vector2 noise = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * noiseStrength;

        force += noise;

        // 应用速度限制
        Vector2 newVelocity = rb.linearVelocity + force * Time.fixedDeltaTime;
        newVelocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
        rb.linearVelocity = newVelocity;
    }

    // 提供外部访问 Identity 的方法
    public HumanIdentity GetIdentity()
    {
        return identity;
    }

    // 提供外部访问 Transform 的方法
    public Transform GetTransform()
    {
        return transform;
    }
}
