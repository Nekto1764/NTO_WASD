using UnityEngine;

public class GoingToSupplyNodeState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Gatherer gatherer = animator.GetComponent<Gatherer>();
        gatherer.MoveTo(gatherer.gathererStructure);
    }
}
