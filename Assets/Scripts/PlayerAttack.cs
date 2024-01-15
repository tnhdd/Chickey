using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject shovel;
    bool canAttack = true;
    [SerializeField] float attackCooldown = 0.75f;
    Animator animator;
    public bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canAttack)
            {
                ShovelAttack();
            }
        }
    }

    public void ShovelAttack()
    {
        isAttacking = true;
        canAttack = false;
        animator.SetTrigger("attack");
        StartCoroutine(ResetAttackCooldown());

    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }
}
