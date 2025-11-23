using UnityEngine;

public class EnemyAnimationCallback : MonoBehaviour
{
    private Animator anim;
    private Enemy enemy;

    private void Start()
    {
        anim = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();
    }

    private void SuccessAttack()
    { 
        enemy.setAttackCheck(true);
    }

    private void AttackEnd()
    {
        anim.SetBool("attack", false);
        enemy.setMove(true);
    }
}
