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

    /// <summary>
    /// remove a gift from the inventory slot
    /// </summary>
    /// <returns>Whether gift removal was successful (false if there was no gift)</returns>
    public bool RemoveGift()
    {
        if (currentGift == null)
            return false;

        currentGift = null;
        image.enabled = false;
        return true;
    }

    /// <summary>
    /// Add a gift to the inventory slot
    /// </summary>
    /// <param name="gift">The type of gift to add</param>
    /// <returns>Whether the gift addition was successful (false if there was a pre-existing gift)</returns>
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
