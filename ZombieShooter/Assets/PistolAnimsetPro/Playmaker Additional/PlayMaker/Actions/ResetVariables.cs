namespace HutongGames.PlayMaker.Actions{

	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets the value of listed Variables to Zero or Null.")]
	
	public sealed class ResetVariables : FsmStateAction{

        [UIHint(UIHint.Variable)] public FsmInt[] Integers;
        [UIHint(UIHint.Variable)] public FsmFloat[] Floats;
        [UIHint(UIHint.Variable)] public FsmBool[] Bools;
        public FsmGameObject[] GameObjects;

        public ResetVariables(){ Reset();}

	    public override void Reset(){
            Bools = new FsmBool[0];
            Integers = new FsmInt[0];
            Floats = new FsmFloat[0];
			GameObjects = new FsmGameObject[0];
		}

		public override void OnEnter()	{
            foreach (FsmBool fsmBool in Bools) fsmBool.Value = false;
            foreach (FsmInt fsmInt in Integers) fsmInt.Value = 0;
            foreach (FsmFloat fsmFloat in Floats) fsmFloat.Value = 0;
            foreach (FsmGameObject go in GameObjects) go.Value = null;
            Finish();		
		}
	}
}