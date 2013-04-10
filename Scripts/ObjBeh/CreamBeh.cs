using UnityEngine;
using System.Collections;

public class CreamBeh : ObjectsBeh {
	
	public const string ChocolateCream = "ChocolateCream";
	public const string StrawberryCream = "StrawberryCream";
	public const string BlueberryCream = "BlueberryCream";	

	/// <summary>
	/// Static game data.
	/// </summary>
	internal static string[] arr_CreamBehs = new string[3] { "", "", "", }; 
	

	void Awake() {
		iTween.Init(this.gameObject);
		this.originalPosition = this.transform.position;
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		this.stageManager = base.baseScene.GetComponent<BakeryShop>();
		
		base._canDragaable = false;
	}

	protected override void OnTouchDown ()
	{
		stageManager.SetAnimatedCreamInstance(false);

		animatedSprite.Play(animationName_001);				
		animatedSprite.animationCompleteDelegate = animationCompleteDelegate;
		baseScene.audioEffect.PlayOnecSound (baseScene.audioEffect.pop_clip);

		if(stageManager.cupcake != null) {
			stageManager.cupcake.WaitForIngredient(this.gameObject.name);
		}		
		if(stageManager.miniCake != null) {
			stageManager.miniCake.WaitForIngredient(this.gameObject.name);
		}		
		if(stageManager.cake != null) {
			stageManager.cake.WaitForIngredient(this.gameObject.name);
		}

		base.OnTouchDown ();
	}
}
