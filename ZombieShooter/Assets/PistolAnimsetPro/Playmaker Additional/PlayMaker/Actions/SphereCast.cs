// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Performs a sphereCast hit")]
	public class SphereCast : FsmStateAction
	{
		
					[ActionSection("Spherecast Settings")] 
		
		[Tooltip("The center of the sphere at the start of the sweep. \nOr use From Position parameter.")]
		public FsmOwnerDefault fromGameObject;

		[Tooltip("The center of the sphere at the start of the sweep. \nOr use Game Object parameter.")]
		public FsmVector3 fromPosition;
		
		[Tooltip("The radius of the shpere.")]
		public FsmFloat radius;

		[Tooltip("The direction into which to sweep the sphere.")]
		public FsmVector3 direction;

		[Tooltip("Cast the sphere in world or local space. Note if no Game Object is specified, the direction is in world space.")]
		public Space space;

		[Tooltip("The length of the ray. Set to -1 for infinity.")]
		public FsmFloat distance;

		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;
		
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		[Tooltip("If true the script will store point and distance information regardless of whether or not the ray hit something.")]
		public FsmBool storeDataOnMiss;

					[ActionSection("RayCast Debug")] 
		
		[Tooltip("The color to use for the debug line.")]
		public FsmColor debugColor;

		[Tooltip("Draw a debug line. Note: Check Gizmos in the Game View to see it in game.")]
		public FsmBool debug;
		
					[ActionSection("Hit")] 
		
		[Tooltip("Set a bool variable to true if hit something, otherwise false.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeDidHit;

		[Tooltip("Store the game object hit in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeHitObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("Get the world position of the ray hit point and store it in a variable.")]
		public FsmVector3 storeHitPoint;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the normal at the hit point and store it in a variable.")]
		public FsmVector3 storeHitNormal;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		public FsmFloat storeHitDistance;
			
		[Tooltip("Event to send when there is a hit.")]
		public FsmEvent hitEvent;

		[Tooltip("Event to send if there is no hit at all")]
		public FsmEvent noHitEvent;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		

		public override void Reset()
		{
			
			fromGameObject = null;
			fromPosition = new FsmVector3 { UseVariable = true };
			direction = Vector3.forward;
			radius = 1f;
			space = Space.Self;
			distance = 100;
			storeHitObject = null;
			everyFrame = false;
			
			
			layerMask = new FsmInt[0];
			invertMask = false;
			debugColor = Color.yellow;
			debug = false;
			
			
			hitEvent = null;
			noHitEvent = null;
		}

		public override void OnUpdate()
		{
			DoSphereCast();

			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public void DoSphereCast()
		{
			if (distance.Value == 0)
			{
				return;
			}

			var go = Fsm.GetOwnerDefaultTarget(fromGameObject);
			
			var originPos = go != null ? go.transform.position : fromPosition.Value;
			
			var rayLength = Mathf.Infinity;
			if (distance.Value > 0 )
			{
				rayLength = distance.Value;
			}

			var dirVector = direction.Value;
			if(go != null && space == Space.Self)
			{
				dirVector = go.transform.TransformDirection(direction.Value);
			}
			
			if (debug.Value)
			{
				var debugRayLength = Mathf.Min(rayLength, 1000);
				Debug.DrawLine(originPos, originPos + dirVector * debugRayLength, debugColor.Value);
			}
			
			RaycastHit hitInfo;
			Physics.SphereCast(originPos, radius.Value, dirVector, out hitInfo, rayLength, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));	
			Fsm.RaycastHitInfo = hitInfo;
			
			var didHit = hitInfo.collider != null;
			storeDidHit.Value = didHit;

			// this is for storing the data if you dont hit anything, good for live updating
			if (!didHit & storeDataOnMiss.Value)
			{
				storeHitDistance.Value = rayLength;
				storeHitPoint.Value = (originPos + new Vector3 (0,0, rayLength));
			}

			if (!didHit)
			{
				Fsm.Event(noHitEvent);
			}

			if (didHit)
			{
				storeHitObject.Value = hitInfo.collider.GetComponent<Collider>().gameObject;
				storeHitPoint.Value = Fsm.RaycastHitInfo.point;
				storeHitNormal.Value = Fsm.RaycastHitInfo.normal;
				storeHitDistance.Value = Fsm.RaycastHitInfo.distance;
				Fsm.Event(hitEvent);
			}

		}
	}
}