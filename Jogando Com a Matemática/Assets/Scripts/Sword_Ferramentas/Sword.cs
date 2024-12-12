using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    // script da faca.
    [SerializeField] private GameObject SlashAnimPrefab;
    [SerializeField] private Transform SlashAnimSpawnPoint;
    [SerializeField] private Transform SlashSpawnPoint_D;
    [SerializeField] private Transform SlashSpawnPoint_U;
    [SerializeField] private SpriteRenderer mySpriteRenderer;

    [SerializeField] private WeaponInfo weaponInfo;
    private Transform weanponCollider;
    private Animator MyAnimator;



    private GameObject SlashAnim;

    private void Awake()
    {
        MyAnimator = GetComponent<Animator>();



    }

    private void Start()
    {
        weanponCollider = PlayerController.Instance.GetWeaponCollider();
        SlashAnimSpawnPoint = GameObject.Find("SlashSpawnPoint").transform;
        SlashSpawnPoint_U = GameObject.Find("SlashSpawnPoint_U").transform;
        SlashSpawnPoint_D = GameObject.Find("SlashSpawnPoint_D").transform;
    }
    private void Update()
    {
        MouseFollowWithOffset();
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack()
    {
        if (PlayerController.Instance.Is_Y_Donw)
        {
            SlashAnim = Instantiate(SlashAnimPrefab, SlashSpawnPoint_D.position, Quaternion.identity);
            MyAnimator.SetTrigger("Attack_D");
            EnableWeaponCollider();
            StartCoroutine(DisableColliderAfterDelay(0.5f)); // Ajuste o tempo de acordo com sua anima��o
        }
        else if (PlayerController.Instance.Is_Y_Up)
        {
            SlashAnim = Instantiate(SlashAnimPrefab, SlashSpawnPoint_U.position, Quaternion.identity);
            MyAnimator.SetTrigger("Attack_U");
            EnableWeaponCollider();
            StartCoroutine(DisableColliderAfterDelay(0.5f));
        }
        else if (PlayerController.Instance.Is_X)
        {
            SlashAnim = Instantiate(SlashAnimPrefab, SlashAnimSpawnPoint.position, Quaternion.identity);
            SlashAnim.transform.parent = this.transform.parent;
            MyAnimator.SetTrigger("Attack_X");
            EnableWeaponCollider();
            StartCoroutine(DisableColliderAfterDelay(0.5f));
        }
    }

    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableWeaponCollider();
    }


    private void EnableWeaponCollider()
    {
        if (weanponCollider != null)
        {
            weanponCollider.gameObject.SetActive(true);
        }
    }

    private void DisableWeaponCollider()
    {
        if (weanponCollider != null)
        {
            weanponCollider.gameObject.SetActive(false);
        }
    }

    public void DoneAttackingAnimEvent()
    {
        DisableWeaponCollider();
    }



    private void RightLayer()
    {
        mySpriteRenderer.sortingOrder = 0;
    }

    private void RightLayer_Up()
    {
        mySpriteRenderer.sortingOrder = -1;
    }


    public void SwingUpFlipAnimEvent()
    {
        if (SlashAnim.gameObject != null)
        {
            SlashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

            if (PlayerController.Instance.FacingLeft)
            {
                SlashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    public void SwingDonwFlipAnimEvent()
    {
        if (SlashAnim.gameObject != null)
        {
            SlashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (PlayerController.Instance.FacingLeft)
            {
                SlashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    public void Swing_Donw_True_FlipAnimEvent()
    {
        if (SlashAnim.gameObject != null)
        {
            SlashAnim.gameObject.transform.rotation = Quaternion.Euler(540, 0, -245); //indo 
        }

    }

    public void Swing_Donw_True_Ba_FlipAnimEvent()
    {
        if (SlashAnim.gameObject != null)
        {
            SlashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 245);//voltando
        }
    }

    public void Swing_Up_True_FlipAnimEvent()
    {
        if (SlashAnim.gameObject != null)
        {
            SlashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 640);//indo 
        }

    }

    public void Swing_Up_True_Ba_FlipAnimEvent()
    {
        if (SlashAnim.gameObject != null)
        {
            SlashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90); //voltando
        }

    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;

        float anglex = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;// angulo para arma


        if (PlayerController.Instance.Is_X)// flip x da arma
        {
            MyAnimator.SetBool("IsX", true);

            MyAnimator.SetBool("IsYD", false);

            MyAnimator.SetBool("IsYU", false);

            if (PlayerController.Instance.FacingLeft)//esquerda
            {
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, anglex);
                weanponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
            }

            else//direita
            {
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, anglex);
                weanponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        }

        else
        { // flip y da arma

            if (PlayerController.Instance.Is_Y_Donw)
            {
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);

                weanponCollider.transform.rotation = Quaternion.Euler(190, 0, 130);

                MyAnimator.SetBool("IsYD", true);

                MyAnimator.SetBool("IsX", false);

                MyAnimator.SetBool("IsYU", false);
            }

            else if (PlayerController.Instance.Is_Y_Up)
            {
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);

                weanponCollider.transform.rotation = Quaternion.Euler(190, 0, 290);

                MyAnimator.SetBool("IsYU", true);

                MyAnimator.SetBool("IsX", false);

                MyAnimator.SetBool("IsYD", false);
            }
        }
    }
}
