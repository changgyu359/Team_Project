using System.Collections;
using UnityEngine;

public class WolfAI : MonoBehaviour
{
    [Header("추적 및 공격 설정")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackDetectedRange = 1.5f;
    [SerializeField] private float attackRange = 2f; 
    [SerializeField] private float minActionTime = 1f;
    [SerializeField] private float maxActionTime = 3f;

    private MonsterBase mBase;
    private Rigidbody rb;
    private Animator anim;

    private float moveSpeed;
    private float actionTimer;
    private int moveDirection = 0;
    private bool isAttacking = false;

    
    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    private void Awake()
    {
        mBase = GetComponent<MonsterBase>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        moveSpeed = mBase.MonsterSpeed;
        ChooseNextAction();
    }

    private void Update()
    {
        if (mBase.IsHit || mBase.IsDead) return;

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 1. 상태 판단 (공격 중이 아닐 때만 상태 변경)
        if (!isAttacking)
        {
            if (distanceToPlayer <= attackDetectedRange)
            {
                currentState = State.Attack; // 사거리 내: 공격
            }
            else if (distanceToPlayer <= chaseRange)
            {
                currentState = State.Chase;  // 발견 거리 내: 추적
            }
            else
            {
                currentState = State.Patrol; // 그 외: 순찰
            }
        }

        // 2. 현재 상태에 따른 행동 실행
        switch (currentState)
        {
            case State.Patrol:
                UpdatePatrol();
                break;
            case State.Chase:
                UpdateChase(distanceToPlayer);
                break;
            case State.Attack:
                if (!isAttacking) StartCoroutine(AttackRoutine());
                break;
        }
    }

    // --- [순찰 로직] ---
    private void UpdatePatrol()
    {
        actionTimer -= Time.deltaTime;
        if (actionTimer <= 0)
        {
            ChooseNextAction();
        }

       

        Vector3 frontVec = new Vector3(transform.position.x + moveDirection * 0.2f, transform.position.y + 0.2f, transform.position.z);


        bool isGrounded = Physics.Raycast(frontVec, Vector3.down, out RaycastHit hit, transform.localScale.y / 2 + 0.2f, LayerMask.GetMask("Ground"));

        if (!isGrounded && moveDirection != 0)
        {
            moveDirection = 0;

        }

        rb.linearVelocity = new Vector3(moveDirection * moveSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        // 걷기: 블렌드 트리의 Speed 파라미터에 1을 전달
        anim.SetFloat("Speed", moveDirection != 0 ? 1f : 0f);
    }

    private void ChooseNextAction()
    {
        int randomAction = Random.Range(0, 3);
        if (randomAction == 0)
        {
            moveDirection = 0;
        }
        else if (randomAction == 1)
        {
            moveDirection = -1;
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }
        else if (randomAction == 2)
        {
            moveDirection = 1;
            transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
        actionTimer = Random.Range(minActionTime, maxActionTime);
    }

    // --- [추적 로직] ---
    private void UpdateChase(float distance)
    {
        // 플레이어가 있는 방향 계산
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        moveDirection = directionToPlayer.x > 0 ? 1 : -1;

        // 해당 방향 바라보기
        transform.rotation = Quaternion.LookRotation(moveDirection == 1 ? Vector3.right : Vector3.left);

        // 추적 시 이동 속도를 더 빠르게 적용 (예: 기본 속도의 1.5배)
        float runSpeed = moveSpeed * 4f;
        rb.linearVelocity = new Vector3(moveDirection * runSpeed, rb.linearVelocity.y, rb.linearVelocity.z);

        // 달리기: 블렌드 트리의 Speed 파라미터에 2를 전달
        anim.SetFloat("Speed", 2f);
    }

    // --- [공격 로직] ---
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 공격할 때는 미끄러지지 않도록 즉시 정지
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        anim.SetFloat("Speed", 0f);

        anim.SetTrigger("Attack");

        // 공격 애니메이션이 완전히 끝날 때까지 대기 (애니메이션 길이에 맞춰 수정하세요)
        yield return new WaitForSeconds(2.5f);

        isAttacking = false;
    }

    // 💡 애니메이션 이벤트에서 실행할 데미지 함수
    public void DealDamage()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // 하드코딩된 숫자(3) 대신, MonsterBase의 공격력(MonsterAtk) 프로퍼티를 활용!
            player.GetComponent<IDamageable>()?.TakeDamage(mBase.MonsterAtk);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 frontVec = new Vector3(transform.position.x + moveDirection * 0.2f, transform.position.y + 0.2f, transform.position.z);
        Gizmos.DrawRay(frontVec, Vector3.down * (transform.localScale.y / 2 + 0.2f));
    }


}