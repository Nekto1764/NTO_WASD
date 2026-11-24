using UnityEngine;

public class DepositingState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Lumberjack lumberjack = animator.GetComponent<Lumberjack>();

       if (lumberjack != null)
        {
            lumberjack.DepositResources();
        }
    }
}
