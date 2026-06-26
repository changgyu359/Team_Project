using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float minActionTime;
    [SerializeField]
    private float maxActionTime;

    private Rigidbody rb;
    private Animator anim;

    private float actionTimer;
    private int moveDirection=0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }




    private void ChooseNextAction()
    {
        int randomAction = Random.Range(0, 3);

        if(randomAction==0)
        {
            moveDirection = 0;
        }
        else if(randomAction==1)
        {
            moveDirection = -1;

        }
    }
}
