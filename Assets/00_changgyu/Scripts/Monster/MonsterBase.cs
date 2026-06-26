using System.Collections;
using UnityEngine;

public class MonsterBase : MonoBehaviour,IDamageable
{
    [SerializeField]
    private MonsterSO myData;
    [SerializeField]
    private Animator anim;

    private string monsterName;
    private int monsterHP;
    private int monsterAtk;
    private float monsterSpeed;

    public void TakeDamage(int _damage)
    {
        monsterHP -= _damage;

        if (monsterHP <= 0)
            StartCoroutine(Dead());

            
    }

    private void Start()
    {
        monsterName = myData.monsterName;
        monsterHP = myData.monsterHP;
        monsterAtk = myData.monsterAtk;
        monsterSpeed = myData.monsterSpeed;
    }

    private IEnumerator Dead()
    {
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(3f);
    }


    private void Patrol()
    {
        
    }

}
