using UnityEngine;
using System.Collections;

public class GoodDataStore : ScriptableObject {
	
	public const int FoodDatabaseCapacity = 30;

    public enum FoodMenuList
    {
        //<!-- beverage.
        Pineapple_juice = 0,
        Apple_juice = 1,
        Cocoa_milk = 2,
        Orange_juice = 3,
        Freshmilk = 4,
        //<!--- Toast with jam.
        ToastWithStrawberryJam = 5,
        ToastWithBlueberryJam = 6,
        ToastWithButterJam = 7,
        ToastWithCustardJam = 8,
		//<!--- Cake.
		Chocolate_cupcake = 9,
		Blueberry_cupcake = 10, 
		Strawberry_cupcake = 11,	
		Chocolate_minicake = 12, 
		Blueberry_minicake = 13,
		Strawberry_minicake = 14,	
		Chocolate_cake = 15,
		Blueberry_cake = 16,
		Strawberry_cake = 17,     //</ cake >
		//<!-- Icecream.
		Strawberry_icecream = 18,
		Vanilla_icecream = 19,
		Chocolate_icecream = 20,
        //<!--- Sandwich.
        Tuna_sandwich = 21,
        DeepFriedChicken_sandwich = 22,
        Ham_sandwich = 23,
        Egg_sandwich = 24,
        //<!--- Cookie.
        Chocolate_cookie = 25,
        Fruit_cookie = 26,
        Butter_cookie = 27,

        Hotdog = 28,
        HotdogWithCheese = 29,
    };
	
   	public Food[] FoodDatabase_list = new Food[FoodDatabaseCapacity] {                               
	    new Food(FoodMenuList.Pineapple_juice.ToString(), 3, 1), //_can sell
        new Food(FoodMenuList.Apple_juice.ToString(), 3, 1),        // 1
        new Food(FoodMenuList.Cocoa_milk.ToString(), 5, 1),         // 2
        new Food(FoodMenuList.Orange_juice.ToString(), 3, 1),       // 3
        new Food(FoodMenuList.Freshmilk.ToString(), 5, 1),         // 4

	    new Food(FoodMenuList.ToastWithStrawberryJam.ToString(), 10, 2), //_can sell
        new Food(FoodMenuList.ToastWithBlueberryJam.ToString(), 10, 2),      // 6
        new Food(FoodMenuList.ToastWithButterJam.ToString(), 10, 2),         // 7
        new Food(FoodMenuList.ToastWithCustardJam.ToString(), 10, 2),        // 8
        
	    new Food(FoodMenuList.Chocolate_cupcake.ToString(), 13, 4), // 9 << can sell >>
        new Food(FoodMenuList.Blueberry_cupcake.ToString(), 13, 4),     // 10
        new Food(FoodMenuList.Strawberry_cupcake.ToString(), 13, 4),    // 11
		
        new Food(FoodMenuList.Chocolate_minicake.ToString(), 20, 6),    // 12
        new Food(FoodMenuList.Blueberry_minicake.ToString(), 20, 6),    // 13
        new Food(FoodMenuList.Strawberry_minicake.ToString(), 20, 6),   // 14
		
        new Food(FoodMenuList.Chocolate_cake.ToString(), 25, 8),	//15
        new Food(FoodMenuList.Blueberry_cake.ToString(), 25, 8),	//16	
        new Food(FoodMenuList.Strawberry_cake.ToString(), 25, 8),	//17
        
	    new Food(FoodMenuList.Strawberry_icecream.ToString(), 8, 2), 			//_can sell.
        new Food(FoodMenuList.Vanilla_icecream.ToString(), 11, 2),				//19
        new Food(FoodMenuList.Chocolate_icecream.ToString(), 11, 2),			//20

        new Food(FoodMenuList.Tuna_sandwich.ToString(), 13, 4),					//21
        new Food(FoodMenuList.DeepFriedChicken_sandwich.ToString(), 17, 5),		//22
        new Food(FoodMenuList.Ham_sandwich.ToString(), 21, 8),					//23
		new Food(FoodMenuList.Egg_sandwich.ToString(), 9, 3),					//24

        new Food(FoodMenuList.Chocolate_cookie.ToString(), 6, 1),				//25
        new Food(FoodMenuList.Fruit_cookie.ToString(), 7, 2),					//26
        new Food(FoodMenuList.Butter_cookie.ToString(), 6, 1),					//27

        new Food(FoodMenuList.Hotdog.ToString(), 10, 4),						//28
        new Food(FoodMenuList.HotdogWithCheese.ToString(), 13, 5),				//29
    };
	
    public GoodDataStore() {
        Debug.Log("Starting GoodDataStore");
    }
}
