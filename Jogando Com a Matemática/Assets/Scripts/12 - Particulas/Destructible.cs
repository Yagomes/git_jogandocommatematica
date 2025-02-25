using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour // Componente que destrói o objeto quando entra em colisão com
                                          // um projétil ou fonte de dano, e gera efeitos visuais de destruição.

{
    [SerializeField] private GameObject destroyVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<DemageSource>() || other.gameObject.GetComponent<Projectile>()) 
        {
            GetComponent<PickUpSpawner>().DropItems();  
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }

}
