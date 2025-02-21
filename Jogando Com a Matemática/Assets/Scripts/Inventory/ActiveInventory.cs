using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;
    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        if (indexNum < 0 || indexNum >= this.transform.childCount)
        {
            Debug.LogError("�ndice de invent�rio inv�lido: " + indexNum);
            return;
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance == null)
        {
            Debug.Log("ActiveWeapon.Instance est� nulo!");
            return;
        }

        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        if (activeSlotIndexNum < 0 || activeSlotIndexNum >= transform.childCount)
        {
            Debug.LogError("activeSlotIndexNum est� fora do intervalo v�lido!");
            return;
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        if (childTransform == null)
        {
            Debug.LogError("childTransform � nulo!");
            return;
        }

        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        if (inventorySlot == null)
        {
            Debug.LogError("InventorySlot n�o encontrado!");
            return;
        }

        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        if (weaponInfo == null)
        {
            Debug.Log("WeaponInfo est� nulo!");
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        if (weaponToSpawn == null)
        {
            Debug.LogError("weaponPrefab est� nulo!");
            return;
        }

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform);

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
