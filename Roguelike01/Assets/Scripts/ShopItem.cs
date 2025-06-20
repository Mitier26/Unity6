using System;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public GameObject buyMassage;

    private bool inBuyZone;

    public bool isHealthRestore, isHealthUpgrade, isWeapon;

    public int itemCost;

    public int healthUpgradeAmount;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (LevelManager.instance.currentCoins >= itemCost)
            {
                LevelManager.instance.currentCoins -= itemCost;

                if (isHealthRestore)
                {
                    PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                }

                if (isHealthUpgrade)
                {
                    PlayerHealthController.instance.IncreaseMaxHealth(healthUpgradeAmount);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMassage.SetActive(true);

            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMassage.SetActive(false);

            inBuyZone = false;
        }
    }
}
