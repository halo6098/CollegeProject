using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

/*
 * This script is for making the slider in the bottom left of the UI slide with the players mana usage
 */

public class ManaSlider : MonoBehaviour
{
    [SerializeField] private Slider mainSlider;

    // Update is called once per frame
    void Update()
    {
        mainSlider.value = PlayerController.manaPercent /100;
    }
}
