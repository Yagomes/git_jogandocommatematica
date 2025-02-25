using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour // Representa um slot de inventário que armazena informações sobre uma arma.
{
    [SerializeField] private WeaponInfo weaponInfo;

    public WeaponInfo GetWeaponInfo() { return weaponInfo; }    
}
