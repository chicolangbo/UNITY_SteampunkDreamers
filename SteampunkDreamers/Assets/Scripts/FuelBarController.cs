using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBarController : MonoBehaviour
{
    private RectTransform fuelBar;
    private PlayerController player;
    private float initialFuelValue;

    public void Awake()
    {
        fuelBar = GetComponent<RectTransform>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        initialFuelValue = player.fuelTimer;
    }

    public void Update()
    {
        var scaleX = player.fuelTimer / initialFuelValue;
        fuelBar.localScale = new Vector3(scaleX, fuelBar.localScale.y, fuelBar.localScale.z);
    }
}
