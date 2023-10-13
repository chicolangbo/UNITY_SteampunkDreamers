using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBarController : MonoBehaviour
{
    private RectTransform fuelBar;
    private PlayerController playerController;
    private float initialFuelValue;

    public void Awake()
    {
        fuelBar = GetComponent<RectTransform>();
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
        initialFuelValue = playerController.fuelTimer;
    }

    public void Update()
    {
        var scaleX = playerController.fuelTimer / initialFuelValue;
        fuelBar.localScale = new Vector3(scaleX, fuelBar.localScale.y, fuelBar.localScale.z);
    }
}
