using UnityEngine;
using System.Collections;

public class DecorationBeh : ObjectsBeh {

	public const string Sauce = "Sauce";
	public const string Cheese = "Cheese";

	protected override void OnTouchDown ()
	{
		base.animatedSprite.Play();
		base.animatedSprite.animationCompleteDelegate = AnimationComplete;
		baseScene.audioEffect.PlayOnecSound (baseScene.soundEffect_clips [6]);

		if (this.name == DecorationBeh.Sauce) {
			stageManager.hotdog.WaitForIngredient (HotdogBeh.HotdogWithSauce);
		}
		if (this.name == DecorationBeh.Cheese) {
			stageManager.hotdog.WaitForIngredient (HotdogBeh.HotdogWithCheese);
		}

		base.OnTouchDown ();
	}
	
	public void AnimationComplete(tk2dAnimatedSprite sprite, int clipId) {
		animatedSprite.StopAndResetFrame();
		
		base.animatedSprite.animationCompleteDelegate -= AnimationComplete;
	}
}
