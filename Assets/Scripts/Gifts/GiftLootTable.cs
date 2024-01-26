using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GiftLootTable : ScriptableObject 
{
    [Range(0f, 100f)]
    public float dropChance;
    public Gift[] giftTable;

    public Gift GetGift()
    {
        return giftTable[Random.Range(0, giftTable.Length)];
    }
}
