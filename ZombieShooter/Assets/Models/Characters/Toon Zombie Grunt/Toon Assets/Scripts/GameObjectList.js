/****************************************
	Copyright 2013 Unluck Software	
 	www.chemicalbliss.com		
 																						
*****************************************/
#pragma strict
import System.Collections.Generic;	//Used to sort particle system list

//Visible properties
var _prefabs:GameObject[];			//gameObjects to spawn
var maxButtons:int = 10;			//Maximum buttons per page	
var spawnOnAwake:boolean = true;	//Instantiate the first model on start
var removeTextFromButton:String;	//Unwanted text 
var _guiPosOffset:Vector2;

//Hidden properties
private var page:int = 0;			//Current page
private var pages:int;				//Number of pages
private var currentGO:GameObject;	//GameObject currently on stage
private var currentColor:Color;
private var _active:boolean = true;
private var counter:int = -1;



function Start(){
	//Sort particle system list alphabeticly
    _prefabs.Sort(_prefabs, function(g1,g2) String.Compare(g1.name, g2.name));
	//Calculate number of pages
	pages = Mathf.Ceil((_prefabs.length -1 )/ maxButtons);
	//Debug.Log(pages);
	if(spawnOnAwake){
		counter=0;
		ReplaceGO(_prefabs[counter]);
		}
}

function Update () {
	if(Input.GetKeyDown(KeyCode.Space)) {
    	if(_active){
    		_active = false;
    	}else{
    		_active = true;
    	}
	}
	if(Input.GetKeyDown(KeyCode.RightArrow)) {
		NextModel ();
	}
	if(Input.GetKeyDown(KeyCode.LeftArrow)) {
		counter--;
		if(counter < 0) counter = _prefabs.Length-1;
		ReplaceGO(_prefabs[counter]);		
	}
}

function NextModel () {
		counter++;
		if(counter > _prefabs.Length -1) counter = 0;
		ReplaceGO(_prefabs[counter]);
}

function OnGUI () {
	if(_active){
	if(_prefabs.length > maxButtons){
		//Prev button
		if(GUI.Button(Rect(20,_guiPosOffset.y+((maxButtons+1)*18),75,18),"Prev"))if(page > 0)page--;else page=pages;
		//Next button
		if(GUI.Button(Rect(95,_guiPosOffset.y+((maxButtons+1)*18),75,18),"Next"))if(page < pages)page++;else page=0;
		//Page text
		GUI.Label (Rect(60,_guiPosOffset.y+((maxButtons+2)*18),150,22), "Page" + (page+1) + " / " + (pages+1));
		
	}
	//Calculate how many buttons on current page (last page might have less)
	var pageButtonCount:int = _prefabs.length - (page*maxButtons);
	//Debug.Log(pageButtonCount);
	if(pageButtonCount > maxButtons)pageButtonCount = maxButtons;	
	//Adds buttons based on how many particle systems on page
	for(var i:int=0;i < pageButtonCount;i++){
		var buttonText:String = _prefabs[i+(page*maxButtons)].transform.name;
		if(removeTextFromButton != "")
		buttonText = buttonText.Replace(removeTextFromButton, "");
		if(GUI.Button(Rect(20,_guiPosOffset.y+(i*18+18),150,18),buttonText)){
			if(currentGO) Destroy(currentGO);
			var go:GameObject = Instantiate(_prefabs[i+page*maxButtons]);
			currentGO = go;
			counter = i + (page * maxButtons);
		}
	}
	}

}

function ReplaceGO (_go:GameObject){
		if(currentGO) Destroy(currentGO);
			var go:GameObject = Instantiate(_go);
			currentGO = go;
}