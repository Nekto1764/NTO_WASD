using UnityEngine;

public class DepositingState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Gatherer gatherer = animator.GetComponent<Gatherer>();

       if (gatherer != null)
        {
            gatherer.DepositResources();
        }
    }
}
