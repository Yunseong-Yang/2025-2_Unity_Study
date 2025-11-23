using UnityEngine;

public class PlayerAnimationCallbackAssignment : MonoBehaviour
{
    // PlayerAssigment를 처리하기 위해 다시 구성한 스크립트
    // 기존 PlayerAnimationCallback 스크립트와 동일
    private Animator anim;
    private PlayerAssignment player;
    void Start()
    {
        player = GetComponentInParent<PlayerAssignment>();
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
