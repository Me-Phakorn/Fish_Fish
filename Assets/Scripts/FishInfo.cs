using UnityEngine;

[System.Serializable]
public class FishInfo
{
    [Header("Fish Info")]
    public string fishName;
    public string description;
    public Sprite fishSprite;
    public FishType fishType;
    public FishSize fishSize;

    public int fishPrice;

    // Fish Stats
    [Header("Fish Stats")]
    public float maxHealth;
    public float maxHunger;
    public float maxStamina;

    // Fish Attributes
    [Header("Fish Attributes")]
    public float attack;
    public float defense;
    public float speed;

    [Header("Fish Class")]
    public FishClass fishClass;

    public virtual void UseSkill(){}
}