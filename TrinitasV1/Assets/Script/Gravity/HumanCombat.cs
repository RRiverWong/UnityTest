using UnityEngine;

public class HumanCombat : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public int attackPower = 10;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 这里暂时先直接销毁 GameObject，后续可拓展为“生成十字事件点”等逻辑
        Destroy(gameObject);
    }
}

