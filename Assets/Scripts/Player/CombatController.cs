using TarodevController;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.Member;

public class CombatController : MonoBehaviour
{
    private InputSystem_Actions playerInput;
    private CharacterMovement movenet;
    [SerializeField] private LayerMask characterLayerMask;
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
        Debug.Log("Attack");

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            this.transform.position + (movenet.direction.x > 0f ? Vector3.right : Vector3.left),
            1f,
            characterLayerMask
        );

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Hit: "+ hit.name);
            Damageable<float> dmg = hit.gameObject.GetComponent<Damageable<float>>();
            if (dmg != null && hit.gameObject != this)
            {
                dmg.OnDamage(10f);
            }
        }
    }
}
