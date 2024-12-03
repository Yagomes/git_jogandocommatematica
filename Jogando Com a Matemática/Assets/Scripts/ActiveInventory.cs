using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ActiveInventory : MonoBehaviour
{

    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        foreach (UnityEngine.Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
         if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        if (transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>().GetWeaponInfo()==null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;
       
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
       
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
       
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());

        /* if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
         {
             Destroy(ActiveWeapon.Istance.CurrentActiveWeapon.gameObject);
         }

         if (!tranform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>())
         {
             ActiveWeapon.Instance.WeaponNull();
         }

         GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).GetComponenInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;
         GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.Tranform.Position, Quaternion.identity);
         ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
         newWeapon.transform.parent = ActiveWeapon.Instance.transform;
         ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());*/
    }
}

    /*  private int activeSlotIndexNum = 0;

      private PlayerControls playerControls;


      private void awake()
      {
          playerControls = new PlayerControls();
      }

      private void Start()
      {
          playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
          ToggleActiveHighlight(0);
      }

      private void ToggleActiveSlot(int Numvalue)
      {
          ToggleActiveHighlight(Numvalue - 1);
      }

      private void ToggleActiveHighlight(int indexNum)
      {
          activeSlotIndexNum = indexNum;

          foreach (Transform inventorySlot in this.trasform)
          {
              inventorySlot.GetChild(0).gameObject.SetActive(false);
          }

          this.tranform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

          ChangeActiveWeapon();
      }

      private void ChangeActiveWeapon()
      {

          //Debug.Log(transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot().GetWeaponInfo().weaponPrefab.name;
          if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
          {
              Destroy(ActiveWeapon.Istance.CurrentActiveWeapon.gameObject);
          }

          if (!tranform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>())
          {
              ActiveWeapon.Instance.WeaponNull();
          }

          GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).GetComponenInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;
          GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.Tranform.Position, Quaternion.identity);
          ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
          newWeapon.transform.parent = ActiveWeapon.Instance.transform;
          ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
      }}*/

