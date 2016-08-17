#pragma strict
var _rotate:Vector3;


function Update () {
	transform.Rotate(_rotate*Time.deltaTime);
}