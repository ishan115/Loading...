using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTeleport : StateMachineBehaviour
{
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DialogueCore.References.SetForwardErrorVisible(false);
        ScreenTransition transition = FindObjectOfType<ScreenTransition>();
        transition.Transition();

		PlayerAnimator.mainPlayer.AdvanceSprite();
    }
}