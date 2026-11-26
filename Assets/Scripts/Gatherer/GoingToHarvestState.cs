using UnityEngine;

public class GoingToHarvestState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Gatherer gatherer = animator.GetComponent<Gatherer>();
        if (gatherer.assignedNode != null)
        {
            gatherer.MoveTo(gatherer.assignedNode);
        }
        else
        {
            
        }
    }
}
