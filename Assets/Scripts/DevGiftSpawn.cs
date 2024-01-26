using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevGiftSpawn : MonoBehaviour
{
    [SerializeField]
    private bool spawnGift = false;
    [SerializeField]
    private GameObject giftObject;

    void Update()
    {
        if (spawnGift)
        {
            spawnGift = false;
            Instantiate(giftObject, transform.position, transform.rotation);
        }
    }
}
