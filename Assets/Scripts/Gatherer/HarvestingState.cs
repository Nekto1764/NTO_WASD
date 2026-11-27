using UnityEngine;

public class HarvestingState : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Gatherer gatherer = animator.GetComponent<Gatherer>();
       gatherer.Harvest();
    }
}
