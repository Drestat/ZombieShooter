using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Device)]
[HutongGames.PlayMaker.Tooltip("Sets the target frame rate")]
public class TargetFrameRate : FsmStateAction
{
	    [RequiredField]
		public FsmInt intValue;
	
		public override void Reset()
		{
			intValue = 30;
		}

	// Code that runs on entering the state.
	public override void OnEnter()
	{
		
		Application.targetFrameRate = intValue.Value;
		Finish();
	}


}
