using UnityEngine;

public class HarvestingState : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Lumberjack lumberjack = animator.GetComponent<Lumberjack>();
       lumberjack.Harvest();
    }
}
