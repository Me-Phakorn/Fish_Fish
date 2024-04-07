using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishObject : MonoBehaviour
{
    [SerializeField]
    private FishInfo fish; // ใส่ค่ามาแล้ว

    [Header("Fish Stats")]
    public float health;
    public float hunger;
    public float stamina;

    [Header("Fish Image")]
    public SpriteRenderer spriteRenderer;

    public void SetFish(FishInfo _fish)
    {
        fish = _fish;

        health = fish.maxHealth;
        hunger = fish.maxHunger;
        stamina = fish.maxStamina;

        spriteRenderer.sprite = fish.fishSprite;
    }
}