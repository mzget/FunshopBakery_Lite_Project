using UnityEngine;
using System;
using System.Collections;

public class CakeBeh : GoodsBeh {

    public static bool _IsActive = false;

	public const string Cupcake = "Cupcake";
	public const string MiniCake = "MiniCake";
	public const string Cake = "Cake";
	
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		base._canActive = true;
		
		if(_canActive)
            base.waitForIngredientEvent += base.Handle_waitForIngredientEvent;
	}

    protected override void OnTouchDown()
    {		
        if (_canActive && CakeBeh._IsActive == false)
        {
            CakeBeh._IsActive = true;
            base.CheckingDelegationOfWaitFotIngredientEvent(this, EventArgs.Empty);
			stageManager.SetAnimatedCreamInstance(true);

            base.OnTouchDown();
        }
		else {
			base.OnTouchDown();
		}
		
		if(MainMenu._HasNewGameEvent && this.name == CakeBeh.Cupcake) {
            Vector3 currentPos = stageManager.chocolate_cream_Instance.transform.position;
            stageManager.chocolate_cream_Instance.transform.position = new Vector3(currentPos.x, currentPos.y, -9f);
			stageManager.SetActivateTotorObject(false);
			stageManager.CreateTabFoodIngredientTutorEvent();
		}
    }

    public override void WaitForIngredient(string ingredientName)
    {
        base.WaitForIngredient(ingredientName);
        if (_isWaitFotIngredient == false)
            return;
        if (MainMenu._HasNewGameEvent)
            stageManager.SetActivateTotorObject(false);

        iTween.Stop(this.gameObject);
		this.transform.position = base.originalPosition;
		baseScene.audioEffect.PlayOnecSound (baseScene.soundEffect_clips [3]);

        if (ingredientName == CreamBeh.ChocolateCream)
        {
            base.animatedSprite.Play(CreamBeh.ChocolateCream);
            base.animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite animSprite, int id)
            {
                if (this.gameObject.name == Cupcake) {
                    this.gameObject.name = GoodDataStore.FoodMenuList.Chocolate_cupcake.ToString();
					base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Chocolate_cupcake].costs;

                    if (MainMenu._HasNewGameEvent)
                    {
                        Vector3 chocolateCreamPos = stageManager.chocolate_cream_Instance.transform.position;
                        stageManager.chocolate_cream_Instance.transform.position = new Vector3(chocolateCreamPos.x, chocolateCreamPos.y, 0);

                        Vector3 currentPos = this.gameObject.transform.position;
                        this.gameObject.transform.position = new Vector3(currentPos.x, currentPos.y, -9f);
                        stageManager.CreateDragGoodsToTrayTutorEvent();
                    }
                }
                else if (this.gameObject.name == MiniCake) {
					this.gameObject.name = GoodDataStore.FoodMenuList.Chocolate_minicake.ToString();
					base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Chocolate_minicake].costs;
				}
				else if (this.gameObject.name == Cake) {
					this.gameObject.name = GoodDataStore.FoodMenuList.Chocolate_cake.ToString();
					base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Chocolate_cake].costs;
				}

                base._canDragaable = true;
                CakeBeh._IsActive = false;
                base._canActive = false;
                base._isWaitFotIngredient = false;
            };
        }
        else if (ingredientName == CreamBeh.BlueberryCream)
        {
            base.animatedSprite.Play(CreamBeh.BlueberryCream);
            base.animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
            {
                if (this.gameObject.name == Cupcake) {
                    this.gameObject.name = GoodDataStore.FoodMenuList.Blueberry_cupcake.ToString();
					base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Blueberry_cupcake].costs;
				}
                else if (this.gameObject.name == MiniCake) {
					this.gameObject.name = GoodDataStore.FoodMenuList.Blueberry_minicake.ToString();
					base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Blueberry_minicake].costs;
				}
                else if (this.gameObject.name == Cake) {
					this.gameObject.name = GoodDataStore.FoodMenuList.Blueberry_cake.ToString();
					base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Blueberry_cake].costs;
				}

                base._canDragaable = true;
                CakeBeh._IsActive = false;
                base._canActive = false;
                base._isWaitFotIngredient = false;
            };
        }
        else if (ingredientName == CreamBeh.StrawberryCream)
        {
            base.animatedSprite.Play(CreamBeh.StrawberryCream);
            base.animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
            {
                if (this.gameObject.name == Cupcake) {
                    this.gameObject.name = GoodDataStore.FoodMenuList.Strawberry_cupcake.ToString();
					base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Strawberry_cupcake].costs;
				}
                else if (this.gameObject.name == MiniCake) {
					this.gameObject.name = GoodDataStore.FoodMenuList.Strawberry_minicake.ToString();
					base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Strawberry_minicake].costs;
				}
                else if (this.gameObject.name == Cake) {
					this.gameObject.name = GoodDataStore.FoodMenuList.Strawberry_cake.ToString();
					base.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Strawberry_cake].costs;
				}

                base._canDragaable = true;
                CakeBeh._IsActive = false;
                base._canActive = false;
                base._isWaitFotIngredient = false;
            };
        }
    }

	public override void OnDispose ()
	{
		base.OnDispose ();

		Destroy(this);
	}
}
