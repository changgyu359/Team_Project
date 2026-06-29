using UnityEngine;

public class RabbitPatrol : MonoBehaviour
{
    [SerializeField]
    private float minActionTime;
    [SerializeField]
    private float maxActionTime;


    private MonsterBase mBase;
    private Rigidbody rb;
    private Animator anim;


    private float moveSpeed;
    private float actionTimer;
    private int moveDirection = 0;







    private void Awake()
    {
        mBase = GetComponent<MonsterBase>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        ChooseNextAction();
        moveSpeed = mBase.MonsterSpeed;

    }

    private void Update()
    {
        actionTimer -= Time.deltaTime;

        if (actionTimer <= 0)
        {
            ChooseNextAction();
        }

        rb.linearVelocity = new Vector3(moveDirection * moveSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        

    }



    private void ChooseNextAction()
    {

        int randomAction = Random.Range(0, 3);

        if (randomAction == 0)
        {
            moveDirection = 0;
            anim.SetBool("isWalking", false);
        }
        else if (randomAction == 1)
        {
            moveDirection = -1;
            transform.rotation = Quaternion.LookRotation(Vector3.left);
            anim.SetBool("isWalking", true);
        }
        else if (randomAction == 2)
        {
            moveDirection = 1;
            transform.rotation = Quaternion.LookRotation(Vector3.right);
            anim.SetBool("isWalking", true);
        }

        actionTimer = Random.Range(minActionTime, maxActionTime);
    }
}
