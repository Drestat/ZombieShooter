// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Get the speed of a gameObject. No need to have it set up with a physic component.")]
	public class GetGameObjectSpeed : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The current speed")]
		[UIHint(UIHint.Variable)]
		public FsmFloat speed;
		
		[Tooltip("The current speed vector")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 speedVector;
		
		private GameObject go;
		
		private Vector3 lastPosition;
		
		public override void Reset()
		{
			gameObject = null;
			speed = null;
			speedVector = null;
		}

		public override void OnEnter()
		{
			go = Fsm.GetOwnerDefaultTarget(gameObject);
			lastPosition = go.transform.position;
			
		}

		public override void OnUpdate()
		{
			if (go == null) 
			{
				return;
			}
			
			doComputeSpeed();
		}
		
		
		void doComputeSpeed()
		{
			Vector3 currentPosition = go.transform.position;
			
			Vector3 delta = currentPosition-lastPosition;
			if (!speed.IsNone){
				speed.Value = delta.magnitude/Time.deltaTime;
			}
			speedVector.Value = delta;

			lastPosition = currentPosition;
		}

	}
}