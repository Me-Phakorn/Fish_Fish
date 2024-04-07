
[System.Serializable]
public class FishStat
{
    public float MaxHungry = 100f;
    public float Hungry = 100f;

    public float HungryRate = 1f;

    public void Initialize()
    {
        Hungry = MaxHungry;
    }

    public void Update(float deltaTime)
    {
        Hungry -= HungryRate * deltaTime;
        Hungry = UnityEngine.Mathf.Clamp(Hungry, 0f, MaxHungry);
    }
}