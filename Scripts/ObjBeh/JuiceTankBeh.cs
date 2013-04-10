using UnityEngine;
using System.Collections;

public class JuiceTankBeh : ObjectsBeh {
	
	public const string PineappleJuiceTank = "PineappleJuiceTank";
	public const string AppleJuiceTank = "AppleJuiceTank";
	public const string OrangeJuiceTank = "OrangeJuiceTank";
    public const string CocoaMilkTank = "CocoaMilkTank";
    public const string FreshMilkTank = "FreshMilkTank";

	public const string PineappleJuice = "PineappleJuice";
	public const string AppleJuice = "AppleJuice";
	public const string OrangeJuice = "OrangeJuice";
    public const string CocoaMilk = "CocoaMilk";
    public const string FreshMilk = "FreshMilk";
	
	
    //<!--- pineapple glass local position == vector3(-1.12f, -.48f, -.2f).
    //<!--- apple glass local pos == (-.815f, -.48f, -.2f).
    //<!--- orange glass local pos == (-.53f, -.48f, -.2f).	
    //<!-- Cocoamilk glass local position => (-0.133, -0.177, -.1);
    //<!-- Freshmilk glass local position => (-0.124, -0.145, -.1);
    private GameObject juice_glass_instance;
	private GlassBeh juiceGlassBeh;


	// Use this for initialization
    protected override void Start()
    {
        base.Start();
		
		stageManager = base.baseScene.GetComponent<BakeryShop>();			
    }

    #region <!-- OnInput.

	protected override void OnTouchDown()
    {
        if(juice_glass_instance == null)
		{
			if(this.gameObject.name == PineappleJuiceTank) 
				this.Create_PineappleJuiceGlass();
			else if(this.gameObject.name == AppleJuiceTank) 
				this.Create_AppleJuiceTank();
			else if(this.gameObject.name == OrangeJuiceTank) 
				this.Create_OrangeJuiceGlass();
			else if(this.gameObject.name == CocoaMilkTank) 
				this.Create_CocoaMilkGlass();
			else if(this.gameObject.name == FreshMilkTank)
				this.Create_FreshMilkGlass();
        }

        base.OnTouchDown();
    }

    #endregion

    private void Create_PineappleJuiceGlass()
    {
        stageManager.audioEffect.PlayOnecWithOutStop(stageManager.soundEffect_clips[2]);

        juice_glass_instance = Instantiate(Resources.Load(ObjectsBeh.SouseMachine_ResourcePath + PineappleJuice, typeof(GameObject))) as GameObject;
        juice_glass_instance.transform.parent = this.transform;
        juice_glass_instance.transform.localPosition = new Vector3(0.01f, -0.325f, 0);
		juice_glass_instance.gameObject.name = GoodDataStore.FoodMenuList.Pineapple_juice.ToString();
		
		juiceGlassBeh = juice_glass_instance.GetComponent<GlassBeh>();
		juiceGlassBeh.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Pineapple_juice].costs;

		juiceGlassBeh.putObjectOnTray_Event += new System.EventHandler(PutGlassOnFoodTray);
		juiceGlassBeh.destroyObj_Event += Handle_JuiceGlassBeh_destroyObj_Event;
    }
	
	void Create_AppleJuiceTank() {		
        stageManager.audioEffect.PlayOnecWithOutStop(stageManager.soundEffect_clips[2]);

        juice_glass_instance = Instantiate(Resources.Load(ObjectsBeh.SouseMachine_ResourcePath + AppleJuice, typeof(GameObject))) as GameObject;
        juice_glass_instance.transform.parent = this.transform;
        juice_glass_instance.transform.localPosition = new Vector3(0.01f, -0.325f, 0);
		juice_glass_instance.gameObject.name = GoodDataStore.FoodMenuList.Apple_juice.ToString();
		
		juiceGlassBeh = juice_glass_instance.GetComponent<GlassBeh>();
		juiceGlassBeh.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Apple_juice].costs;

		juiceGlassBeh.putObjectOnTray_Event += new System.EventHandler(PutGlassOnFoodTray);
		juiceGlassBeh.destroyObj_Event += Handle_JuiceGlassBeh_destroyObj_Event;
	}
	
	void Create_OrangeJuiceGlass() {
        stageManager.audioEffect.PlayOnecWithOutStop(stageManager.soundEffect_clips[2]);

        juice_glass_instance = Instantiate(Resources.Load(ObjectsBeh.SouseMachine_ResourcePath + OrangeJuice, typeof(GameObject))) as GameObject;
        juice_glass_instance.transform.parent = this.transform;
        juice_glass_instance.transform.localPosition = new Vector3(0.01f, -0.325f, 0);
		juice_glass_instance.gameObject.name = GoodDataStore.FoodMenuList.Orange_juice.ToString();
		
		juiceGlassBeh = juice_glass_instance.GetComponent<GlassBeh>();
		juiceGlassBeh.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Orange_juice].costs;

		juiceGlassBeh.putObjectOnTray_Event += new System.EventHandler(PutGlassOnFoodTray);
		juiceGlassBeh.destroyObj_Event += Handle_JuiceGlassBeh_destroyObj_Event;
	}

	void Create_CocoaMilkGlass ()
	{
        stageManager.audioEffect.PlayOnecWithOutStop(stageManager.soundEffect_clips[2]);

		juice_glass_instance = Instantiate(Resources.Load(ObjectsBeh.SouseMachine_ResourcePath + CocoaMilk, typeof(GameObject))) as GameObject;
		juice_glass_instance.transform.parent = this.transform;
		juice_glass_instance.transform.localPosition = new Vector3(-0.133f, -0.177f, -.1f);
		juice_glass_instance.gameObject.name = GoodDataStore.FoodMenuList.Cocoa_milk.ToString();
		
		juiceGlassBeh = juice_glass_instance.GetComponent<GlassBeh>();
		juiceGlassBeh.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Cocoa_milk].costs;

		juiceGlassBeh.putObjectOnTray_Event += new System.EventHandler(PutGlassOnFoodTray);
		juiceGlassBeh.destroyObj_Event += Handle_JuiceGlassBeh_destroyObj_Event;
	}

	void Create_FreshMilkGlass ()
	{
        stageManager.audioEffect.PlayOnecWithOutStop(stageManager.soundEffect_clips[2]);

		juice_glass_instance = Instantiate(Resources.Load(ObjectsBeh.SouseMachine_ResourcePath + FreshMilk, typeof(GameObject))) as GameObject;
		juice_glass_instance.transform.parent = this.transform;
		juice_glass_instance.transform.localPosition = new Vector3(-0.124f, -0.145f, -.1f);
		juice_glass_instance.gameObject.name = GoodDataStore.FoodMenuList.Freshmilk.ToString();
		
		juiceGlassBeh = juice_glass_instance.GetComponent<GlassBeh>();
		juiceGlassBeh.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Freshmilk].costs;

		juiceGlassBeh.putObjectOnTray_Event += new System.EventHandler(PutGlassOnFoodTray);
		juiceGlassBeh.destroyObj_Event += Handle_JuiceGlassBeh_destroyObj_Event;
	}

	
	private void PutGlassOnFoodTray(object sender, System.EventArgs e) {		
		GoodsBeh obj = sender as GoodsBeh;
		if (stageManager.foodTrayBeh.goodsOnTray_List.Contains (obj) == false && stageManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			stageManager.foodTrayBeh.goodsOnTray_List.Add (obj);
			stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			juice_glass_instance = null;
			juiceGlassBeh = null;
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
	}
    private void Handle_JuiceGlassBeh_destroyObj_Event (object sender, System.EventArgs e)
	{
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		stageManager.CreateDeductionsCoin (goods.costs);
		stageManager.ReFreshAvailableMoney();		
		stageManager.foodTrayBeh.goodsOnTray_List.Remove(goods);
		stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
