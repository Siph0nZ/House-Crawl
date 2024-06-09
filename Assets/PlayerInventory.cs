using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, InterfaceInventory
{
    public int Paper { get => paper; set => paper = value; }
    public int paper = 0;
}
