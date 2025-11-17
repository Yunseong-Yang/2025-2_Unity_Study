using UnityEngine;

public class PlayerAnimationCallback : MonoBehaviour
{
    private Animator anim;
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
    }

    private void DisableAttackAnimation()
    {
        anim.SetBool("attack", false);
        player.setMove(true);
    }

    private void EnableAttackOverlapEvent()
    {
        player.AttackOverlap();
    }
}
