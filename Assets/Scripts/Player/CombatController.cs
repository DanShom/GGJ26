using System;
using TarodevController;
using Unity.Mathematics;
using Unity.XR.Oculus.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class CombatController : MonoBehaviour
{
    private InputSystem_Actions playerInput;
    private CharacterMovement movenet;
    [SerializeField] private LayerMask characterLayerMask;
    [SerializeField] public bool _RangeAttack = false;

    private ProjactileLogic plg;
    [Header("Projectle Logic")]
    public ObjectPool amunition;

    public Action OnRegularAttack;

    public ObjectPool Amunition
    {
        set
        {
            amunition = value;
            plg = Amunition.prefabe.GetComponent<ProjactileLogic>();
        }
        get
        {
            return amunition;
        }
    }


    void Awake()
    {
        movenet = GetComponent<CharacterMovement>();
        playerInput = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        playerInput.Player.Attack.Enable();
        playerInput.Player.Attack.performed += Attack;

    }
    private void OnDisable()
    {
        playerInput.Player.Attack.performed -= Attack;
        playerInput.Player.Attack.Disable();
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        if (_RangeAttack) RangeAttack();
        else RegularAttack();
    }

    private void RegularAttack()
    {
        Debug.Log("Attack");
        OnRegularAttack.Invoke();
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            this.transform.position + (movenet.direction.x > 0f ? Vector3.right : Vector3.left),
            1f,
            characterLayerMask
        );

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Hit: " + hit.name);
            Damageable<float> dmg = hit.gameObject.GetComponent<Damageable<float>>();
            if (dmg != null && hit.gameObject != this)
            {
                dmg.OnDamage(10f);
            }
        }

    }

    private void RangeAttack()
    {
        GameObject bullet = Amunition.GetInstance(this.transform.position);
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseScreenPos3 = new Vector3(
            mouseScreenPos.x,
            mouseScreenPos.y,
            Mathf.Abs(Camera.main.transform.position.z)
        );

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos3);
        ProjactileLogic plg = bullet.GetComponent<ProjactileLogic>();
        plg.Damage = this.plg.Damage;
        plg.SetTarget(mouseWorldPos, this.gameObject);
    }

    private void HeavyAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            this.transform.position + (movenet.direction.x > 0f ? Vector3.right : Vector3.left),
            1f,
            characterLayerMask
        );

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Hit: " + hit.name);
            Damageable<float> dmg = hit.gameObject.GetComponent<Damageable<float>>();
            if (dmg != null && hit.gameObject != this)
            {
                dmg.OnDamage(50f);
            }
        }
    }
}