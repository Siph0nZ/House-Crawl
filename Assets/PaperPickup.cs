using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            InterfaceInventory inventory = other.GetComponent<InterfaceInventory>();

            if (inventory != null)
            {
                inventory.Paper = inventory.Paper + value;
                print("Paper: " + inventory.Paper);

                Destroy(gameObject);
            }
        }
    }
}
