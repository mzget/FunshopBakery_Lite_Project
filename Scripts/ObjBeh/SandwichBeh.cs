using UnityEngine;
using System.Collections;

public class SandwichBeh : GoodsBeh {

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		base._canDragaable = true;
        base.offsetPos = Vector3.up * -0.1f;
	}
}
