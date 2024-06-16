
[System.Serializable]
public class BossStat
{
    public float MaxHealth = 100f;
    public float Health = 100f;

    public void Initialize()
    {
        Health = MaxHealth;
    }

    public void DoDamage(float damage)
    {
        Health -= damage;
        Health = UnityEngine.Mathf.Clamp(Health, 0f, MaxHealth);
    }
}