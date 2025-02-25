using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour // Representa um slot de invent�rio que armazena informa��es sobre uma arma.
{
    [SerializeField] private WeaponInfo weaponInfo;

    public WeaponInfo GetWeaponInfo() { return weaponInfo; }    
}
