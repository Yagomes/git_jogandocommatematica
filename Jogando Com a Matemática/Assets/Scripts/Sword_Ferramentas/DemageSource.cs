using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemageSource : MonoBehaviour
{
    // script do colisor da arma, para identificar em que a arma cortou. 
    private int damageAmount;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            EnemyHealth enymeHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enymeHealth?.TakeDemage(damageAmount);
      
    }
}
