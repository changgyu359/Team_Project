using UnityEngine;

public class TempPlayer : MonoBehaviour,IDamageable
{
    private int hp = 10;
    private float moveSpeed = 5f;
    private Rigidbody rb;

    public void TakeDamage(int _damage)
    {
        hp-=_damage;
        Debug.Log("아야,현재체력:"+hp);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float moveX=Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector3(moveX * moveSpeed, rb.linearVelocity.y, 0);

        
    }

}
