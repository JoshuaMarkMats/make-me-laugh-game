using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    private Slider resourceBar;

    void Awake()
    {
        resourceBar = GetComponent<Slider>();
    }

    public void SetMaxValue(float value)
    {
        resourceBar.maxValue = value;
        resourceBar.value = value;
    }

    public void SetValue(float value)
    {
        resourceBar.value = value;
    }
}