#pragma strict

var UseLifeTime = false;
var lifetime = 2;

function Start () {
if(UseLifeTime)
Destroy (gameObject, lifetime);
}
