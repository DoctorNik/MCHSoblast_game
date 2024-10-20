using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light flashlight;
    private bool FlashToogle = false;
    [SerializeField] private int FlashEnergy = 100;
    private bool isRecharging = false;

    void Start()
    {
        flashlight = GetComponent<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
            FlashToogle = flashlight.enabled;

            if (FlashToogle)
            {
                InvokeRepeating("DecreaseEnergy", 0f, 2f);
            }
            else
            {
                CancelInvoke("DecreaseEnergy");
            }
        }
    }

    void DecreaseEnergy()
    {
        if (FlashEnergy > 0)
        {
            FlashEnergy -= 1;
            Debug.Log("Flash Energy:" + FlashEnergy);
        }
        else
        {
            flashlight.enabled = false;
            FlashToogle = false;
            CancelInvoke("DecreaseEnergy");
            Debug.Log("Energy end");
            StartCoroutine(RechargeEnergy(5f));
        }

    }

    IEnumerator RechargeEnergy(float delay)
    {
        isRecharging = true;
        yield return new WaitForSeconds(delay);

        InvokeRepeating("IncreaseEnergy", 0f, 2f);
    }

    void IncreaseEnergy()
    {
        if ((FlashEnergy < 100) && (!FlashToogle))
        {
            FlashEnergy += 1;
            Debug.Log("Recharging Energy:" + FlashEnergy);
        }
        else
        {
            CancelInvoke("IncreaseEnergy");
            isRecharging = false;
            Debug.Log("Energy is 100");
        }
    }

    void OnDisable()
    {
        if (FlashEnergy > 0 && !isRecharging)
        {
            StartCoroutine(RechargeEnergy(5f));
        }
    }
}
