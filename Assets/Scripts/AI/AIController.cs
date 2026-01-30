using TarodevController;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AICharacterController2D))]
public class AIController : MonoBehaviour, Damageable<float>
{
    private enum States
    {
        IDLE,
        CHASE,
        ATTACK
    }
    [SerializeField] private States states;
    private States oldstates;
    private GameObject player;
    private AICharacterController2D agent;
    private CharacterMovement movment;
    [SerializeField] private LayerMask characterLayerMask;
    [SerializeField] private FloatReference AttackRange;

    private SmartSwitch stSwitch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        movment = gameObject.GetComponent<CharacterMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = gameObject.GetComponent<AICharacterController2D>();
        t = Random.value * 0.2f;
    }

    private void SetStateGraph()
    {
        switch (states)
        {
            case States.IDLE:
                agent.EnableAgnet(false);
                break;

            case States.CHASE:
                agent.EnableAgnet(true);
                agent.SetTarget(player.transform);
                break;

            case States.ATTACK:
                Debug.Log("Attack");
                agent.EnableAgnet(false);
                Attack();
                break;

        }
    }

    private void checkState()
    {

        switch (states)
        {
            case States.IDLE:
                if (CommonFunctions.IsClose(this.transform.position, player.transform.position,30f))
                {
                    states = States.CHASE;
                }
                break;

            case States.CHASE:
                if (agent._hasTarget && agent._reachedTarget)
                {
                    states = States.ATTACK;
                }
                break;

            case States.ATTACK:
                if (!CommonFunctions.IsClose(this.transform.position, player.transform.position, AttackRange.Value))
                {
                    states = States.CHASE;
                }
                break;

        }
    }
    // Update is called once per frame

    float t = 0f;
    void Update()
    {
        t += Time.deltaTime;
        if (t > 0.2f)
        {
            checkState();
            t = 0f;
        }

        stSwitch.Update(oldstates == states);
        if(stSwitch.OnRelese())
        {
            SetStateGraph();
        }
        oldstates = states;
    }

    private void Attack()
    {
        Debug.Log("Attack");

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            this.transform.position + (movment.direction.x > 0f ? Vector3.right : Vector3.left),
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

    public void OnDamage(float log)
    {
        Debug.Log("Enemy " + this.name + " Got hit! " + log + " Damage");
        //throw new System.NotImplementedException();
    }

    public bool IsDead()
    {
        throw new System.NotImplementedException();
    }
}
