using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftSlot : MonoBehaviour
{
    [SerializeField]
    private Image image;

    private GiftType currentGift;

    public GiftType CurrentGift { get { return currentGift; } }

    public bool RemoveGift()
    {
        if (currentGift == null)
            return false;

        currentGift = null;
        image.enabled = false;
        return true;
    }

    public bool AddGift(GiftType gift)
    {
        if (currentGift != null)
            return false;

        currentGift = gift;

        image.sprite = gift.GiftIcon;
        image.enabled = true;

        return true;
    }
}
