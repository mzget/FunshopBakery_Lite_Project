using UnityEngine;
using System;
using System.Collections;

public class HotdogBeh : GoodsBeh {

	public const string HotdogWithSauce = "HotdogWithSauce";
	public const string HotdogWithCheese = "HotdogWithCheese";

	public static bool _IsActive = false;

	
	// Use this for initialization
    protected override void Start()
    {
        base.Start();
		
		base._canActive = true;
        base.offsetPos = Vector3.up * -0.1f;
		
		if(_canActive)
            base.waitForIngredientEvent += base.Handle_waitForIngredientEvent;
    }

	protected override void OnTouchDown ()
	{
		if (_canActive && _IsActive == false)
		{
			_IsActive = true;
			base.CheckingDelegationOfWaitFotIngredientEvent(this, EventArgs.Empty);
		}
		
		base.OnTouchDown();
	}
	

    public override void WaitForIngredient(string ingredientName)
    {
        base.WaitForIngredient(ingredientName);

		if(_isWaitFotIngredient == false)
			return;
		
		iTween.Stop(this.gameObject);
		this.transform.position = originalPosition;
		
		if(ingredientName == HotdogBeh.HotdogWithSauce) {
			base.animatedSprite.Play(HotdogBeh.HotdogWithSauce);
			base.animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {	
				this.gameObject.name = GoodDataStore.FoodMenuList.Hotdog.ToString();
				base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Hotdog].costs;
				base._canDragaable = true;
                base._canActive = false;
                HotdogBeh._IsActive = false;
                base._isWaitFotIngredient = false;
			};
			
			return;
		}
		
		if(ingredientName == HotdogBeh.HotdogWithCheese) {
			base.animatedSprite.Play(HotdogBeh.HotdogWithCheese);
			base.animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {		
				this.gameObject.name = GoodDataStore.FoodMenuList.HotdogWithCheese.ToString();
				base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.HotdogWithCheese].costs;
				base._canDragaable = true;
                base._canActive = false;
				HotdogBeh._IsActive = false;
                base._isWaitFotIngredient = false;
			};
			
			return;
		}
    }
}
