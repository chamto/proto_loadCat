using UnityEngine;
using System.Collections;

public class Cat_StateMachineBehaviour : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		base.OnStateEnter (animator, stateInfo, layerIndex);
		//Debug.Log (stateInfo.fullPathHash); // == "Base Layer.state_rush"
		//Debug.Log (stateInfo.shortNameHash); //== "state_rush"

		//Debug.Log (animator.GetInteger ("state"));
		//animator.SetInteger ("state", 1);
		//animator.speed = 2;

		CatAniInit_MonoBehaviour mono = animator.gameObject.GetComponentInParent<CatAniInit_MonoBehaviour> ();
		if (null != mono) 
		{
			if (GlobalConstants.Hash_Ani.hide == stateInfo.fullPathHash) 
			{
				mono.AniInitHide();
			}
			if (GlobalConstants.Hash_Ani.rush == stateInfo.fullPathHash) 
			{
				mono.AniInitRush();
			}
			if (GlobalConstants.Hash_Ani.eat == stateInfo.fullPathHash) 
			{
				mono.AniInitEat();
			}
		}






	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
