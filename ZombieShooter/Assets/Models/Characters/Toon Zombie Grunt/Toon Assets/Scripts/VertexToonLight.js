#pragma strict
@script ExecuteInEditMode()
var _materials:Transform[];

function Start () {

}

function Update () {
	for(var i:int; i < _materials.length;i++){
		_materials[i].GetComponent.<Renderer>().sharedMaterial.SetFloat( "_LightPosX", transform.position.x );
		_materials[i].GetComponent.<Renderer>().sharedMaterial.SetFloat( "_LightPosY", transform.position.y );
		_materials[i].GetComponent.<Renderer>().sharedMaterial.SetFloat( "_LightPosZ", transform.position.z );
	}
}