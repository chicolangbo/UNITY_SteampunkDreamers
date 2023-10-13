using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour
{
    Slider Fuel;
    private PlayerController player;
    private float initialFuelValue;

    public void Awake()
    {
        Fuel = GetComponent<Slider>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        initialFuelValue = player.fuelTimer;
    }


        // Start is called before the first frame update
        void Start()
        {
        
        }

    // Update is called once per frame
    void Update()
    {
    Fuel.value = player.fuelTimer / initialFuelValue;
    }
}
