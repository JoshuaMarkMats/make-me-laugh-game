using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevGiftSpawn : MonoBehaviour
{
    [SerializeField]
    private bool spawnGift = false;
    [SerializeField]
    private GiftType giftType;
    [SerializeField]
    private Gift giftObject;

    void Update()
    {
        if (spawnGift)
        {
            spawnGift = false;
            Gift newGift = Instantiate(giftObject, transform.position, transform.rotation);
            newGift.GiftType = giftType;
        }
    }
}
