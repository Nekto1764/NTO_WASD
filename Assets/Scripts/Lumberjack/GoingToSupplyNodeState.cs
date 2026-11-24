using UnityEngine;

public class GoingToSupplyNodeState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Lumberjack lumberjack = animator.GetComponent<Lumberjack>();
        lumberjack.MoveTo(lumberjack.sawmill);
    }
}
