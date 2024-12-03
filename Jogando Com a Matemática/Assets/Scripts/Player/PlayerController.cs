using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    //script do controle do player.
    public bool FacingLeft { get { return facingLeft; } }


    [SerializeField] private float MoveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;//sombra
    [SerializeField] private Transform weaponCollider;

    private PlayerControls PlayerControls;
    private Vector2 Movement;
    private Rigidbody2D Rb;

    private Animator MyAnimator;
    private SpriteRenderer MySpriteRenderer;
    private float startingMoveSpeed;//sombra

    private CapsuleCollider2D MyCapsuleCollider;

    AnimatorStateInfo stateInfo;

    public bool Is_Y_Donw;
    public bool Is_Y_Up;
    public bool Is_X;

    public bool es = false;


    private bool facingLeft = false;
    private bool isDashing = false;//sombra

    private Knockback knockback;


    private void Start()
    {
        PlayerControls.Combat.Dash.performed += _ => Dash();//sombra
        startingMoveSpeed = MoveSpeed;//sombra
    }

    protected override void Awake() {
        base.Awake();
        PlayerControls = new PlayerControls();
        Rb = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();  
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        MyCapsuleCollider = GetComponent<CapsuleCollider2D>();
        knockback = GetComponent<Knockback>();
    }

    private void OnEnable()
    {
        // Ativa o controle do jogo.
        PlayerControls.Enable();
    }

    private void Update()//A cada framessegundos essa função é chamada no jogo.
    {
        stateInfo = MyAnimator.GetCurrentAnimatorStateInfo(0);

        PlayerInput(); //A cada atualização, obtem quando tecla o palyer apertou. 
    }

    private void FixedUpdate()// A cada tempo fixo essa função é chamada.
    {
        AdjustPlayerFacingDirection();// Qual lado o player vai.
        Move(); // Faz a movimentação do player. 
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput()
    {
        Movement = PlayerControls.Movement.Move.ReadValue<Vector2>();

        MyAnimator.SetFloat("moveX", Movement.x);
        MyAnimator.SetFloat("moveY", Movement.y);

        if(Movement.y > 0 || Movement.y < 0) // Se o player está indo para cima ou para baixo.
        {
            MyCapsuleCollider.offset = new Vector2( 0.0f, -0.18f); // Ajusta o colisor para a posição que o player está.
            MyCapsuleCollider.size = new Vector2(0.73f, 0.47f); // Ajusta o colisor para a posição que o player está.
        }


    }

    private void Move()
    {
        if (knockback.GettingKnockedBack) { return; }
        Rb.MovePosition(Rb.position + Movement * (MoveSpeed * Time.fixedDeltaTime)); // Faz o player andar.
    }

    private void AdjustPlayerFacingDirection() // Ajusta a direção do player da esquerda para direita de acordo com a posição do mouse e do próprio player.
    {
        

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        float distanceX =  mousePos.x - playerScreenPoint.x;

        float margin = 50f;



        if (stateInfo.IsName("Player_IdleUp") == false && (stateInfo.IsName("Player_IdleDonw") == false) && (stateInfo.IsName("Player_RunningDonw") == false) && (stateInfo.IsName("Player_RunningUp") == false)) 
        {
            Is_Y_Donw = false;
            Is_Y_Up = false;
            Is_X = true;

            if(math.abs(distanceX) > margin) 
            { 

            if (distanceX < 0)// esquerda
            {
               MySpriteRenderer.flipX = true;

               MyCapsuleCollider.offset = new Vector2(0.15f, MyCapsuleCollider.offset.y);
               MyCapsuleCollider.size = new Vector2(0.73f, 0.47f);

               facingLeft = true;
             }
            else
            {
               MySpriteRenderer.flipX = false;

               MyCapsuleCollider.offset = new Vector2(-0.15f, -0.18f);
               MyCapsuleCollider.size = new Vector2(0.73f, 0.47f);

                facingLeft = false;
             }

            }
        }

        if ((stateInfo.IsName("Player_IdleUp") == true) || (stateInfo.IsName("Player_IdleDonw") == true) || (stateInfo.IsName("Player_RunningDonw") == true) || (stateInfo.IsName("Player_RunningUp") == true))
        {

            if ((mousePos.y < playerScreenPoint.y))
            {
                MyAnimator.SetBool("IsYD", true);
                MyAnimator.SetBool("IsYU", false);
                Is_Y_Donw = true;
                Is_Y_Up = false;
                Is_X = false;
            }
            else 
            {
                MyAnimator.SetBool("IsYU", true);
                MyAnimator.SetBool("IsYD", false);
                Is_Y_Up = true;
                Is_X = false;
                Is_Y_Donw = false;
            }
        }

    }

    private void Dash()//sombra do player
    {
        if (!isDashing)
        {
            isDashing = true;
            MoveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());

        }
    }

    private IEnumerator EndDashRoutine()//fim da sombra do player
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        MoveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}

