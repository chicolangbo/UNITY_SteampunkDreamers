using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarController : MonoBehaviour
{
    //private RectTransform fuelBar;
    private Slider fuelBar;
    private PlayerController playerController;
    private float initialFuelValue;

    public void Start()
    {
        fuelBar = GetComponent<Slider>();
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
        initialFuelValue = playerController.fuelTimer;
    }

    public void Update()
    {
        fuelBar.value = playerController.fuelTimer / initialFuelValue;
    }
}
