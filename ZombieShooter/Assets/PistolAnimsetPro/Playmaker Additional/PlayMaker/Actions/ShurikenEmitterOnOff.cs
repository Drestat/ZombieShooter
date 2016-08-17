using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Turns a particle emitter ON or OFF.")]
	public class ShurikenEmitterOnOff : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[Tooltip("Set to True to turn it ON and False to turn it OFF.")]
		public FsmBool emitOnOff;
		private GameObject go;
		
		public override void Reset()
		{
			gameObject = null;
			emitOnOff = false;
			go = null;
		}
	
		public override void OnEnter()
		{
			go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (emitOnOff.Value == true)
			{
				go.GetComponent<ParticleSystem>().Play();
			}
			else
			{
				go.GetComponent<ParticleSystem>().Stop();
			}
		}
	}
}