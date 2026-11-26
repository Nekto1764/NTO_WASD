using UnityEngine;

public class IdleStateGatherer : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Gatherer gatherer = animator.GetComponent<Gatherer>();
       gatherer.assignedNode = null;
    }
}
