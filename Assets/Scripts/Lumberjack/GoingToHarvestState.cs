using UnityEngine;

public class GoingToHarvestState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Lumberjack lumberjack = animator.GetComponent<Lumberjack>();
       if (lumberjack.assignedTree != null)
        {
            lumberjack.MoveTo(lumberjack.assignedTree);
        }
    }
}
