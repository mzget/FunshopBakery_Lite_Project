using System;
using UnityEngine;

public class UpgradeInsideManager : MonoBehaviour {
	//<@-- Upgrade button.
	public GameObject[,] upgradeButton_Objs = new GameObject[2,4];
	private tk2dSprite[,] upgradeButton_Sprites = new tk2dSprite[2,4];
	//<@-- Upgrade item objs.
	public GameObject[,] upgradeInsideObj2D = new GameObject[2,4];
	private tk2dSprite[,] upgradeInsideSprite2D = new tk2dSprite[2, 4];
    private tk2dTextMesh[,] upgradeInside_PriceTextmesh = new tk2dTextMesh[2, 4];
    //<@-- Confirmation window obj.
    public GameObject confirmationWindow;
	
    private string[,] firstPage_spriteNames = new string[2, 4] {
       {"blueberry_jam", "blueberry_cream", "miniCake", "Cake"},
       {"vanilla_icecream", "tuna_sandwich", "chocolate_chip_cookie", "hotdog"},
    };
    private string[,] secondPage_spriteNames = new string[2, 4] {
        {"appleJuiceTank", "chocolateMilkTank", "butter_jam", "strawberry_cream"},
        {"chocolate_icecream", "deep_fried_chicken_sandwich", "fruit_cookie", "orangeJuiceTank"},
    };
    private string[,] thirdPage_spriteNames = new string[2, 4] {
		{"freshMilkTank", "custard_jam", "ham_sandwich", "egg_sandwich"},
		{"butter_cookie", "hotdog_cheese", "", ""},
	};
    private int[,] firstPage_prices = new int[,] {
        {300, 300, 1500, 3000},
        {800, 900, 500, 1000},
    };
    private int[,] secondPage_prices = new int[,] {
        {100, 150, 300, 300},
        {1400, 1200, 500, 100},
    };
    private int[,] thirdPage_prices = new int[,] {
        {150, 200, 1600, 700},
        {500, 1300,0,0},
    };
	
	private int currentPageIndex = 0;
    private const int MAX_PAGE_NUMBER = 3;
	private int activeUpgradeButtonID = 25;
	private const int UnActiveUpgradeButtonID = 29;
    public tk2dTextMesh displayCurrentPageID_Textmesh;
	private bool _isInitialize = false;

    private event EventHandler<OnUpdateEvenArgs> OnUpgrade_Event;
    private void OnUpgradeEvent_checkingDelegation(OnUpdateEvenArgs e) {
        if (OnUpgrade_Event != null)
            OnUpgrade_Event(this, e);
    }
    private class OnUpdateEvenArgs : EventArgs
    {
        public int I = 0;
        public int J = 0;
        public int Item_id;
		public string AdditionalParams = string.Empty;
		
        public OnUpdateEvenArgs() { }
    };
    private OnUpdateEvenArgs currentOnUpdateTarget;

	/// <summary>
	/// Calss references.
	/// </summary>
	private SheepBank sceneController;
	
	
	// Use this for initialization
	void Start () {
		currentPageIndex = 0;

        confirmationWindow.gameObject.SetActiveRecursively(false);

		var controller = GameObject.FindGameObjectWithTag("GameController");
		sceneController = controller.GetComponent<SheepBank>();

        OnUpgrade_Event += Handle_OnUpgrade_Event;
	}
	
	internal void ReInitializeData() {
		currentPageIndex = 0;

		CalculateObjectsToDisplay();
	}
	
	// Update is called once per frame
	void Update () {
		if(_isInitialize == false)
			InitailizeDataFields();
	}
	
	void InitailizeDataFields() {
		if(upgradeInsideSprite2D[0,0] == null) 
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					upgradeInsideSprite2D[i, j] = upgradeInsideObj2D[i, j].GetComponent<tk2dSprite>();
					upgradeInside_PriceTextmesh[i, j] = upgradeInsideObj2D[i, j].transform.GetComponentInChildren<tk2dTextMesh>();
					
					upgradeButton_Sprites[i,j] = upgradeButton_Objs[i,j].GetComponent<tk2dSprite>();
				}
			}
		}	
		
		_isInitialize  = true;
//		CalculateObjectsToDisplay();
	}
	
	public void GotoNextPage() {
	    foreach(GameObject obj in upgradeInsideObj2D) {
            obj.animation.Play();
        }
		
		if(currentPageIndex < MAX_PAGE_NUMBER - 1)	currentPageIndex += 1;
		else currentPageIndex = 0;
		
        CalculateObjectsToDisplay();
	}
	
	public void BackToPreviousPage() {		
	    foreach(GameObject obj in upgradeInsideObj2D) {
            obj.animation.Play();
        }

        if(currentPageIndex > 0)             
			currentPageIndex -= 1;
		else
			currentPageIndex = MAX_PAGE_NUMBER - 1;
		
		CalculateObjectsToDisplay();
	}

    private void CalculateObjectsToDisplay()
    {
        if(currentPageIndex == 0)
        {
            #region <@-- Page index == 0

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string nameSpecify = firstPage_spriteNames[i, j];
                    upgradeInsideSprite2D[i, j].spriteId = upgradeInsideSprite2D[i, j].GetSpriteIdByName(nameSpecify);
                    upgradeInsideSprite2D[i, j].color = Color.white;

                    upgradeInside_PriceTextmesh[i, j].text = firstPage_prices[i, j].ToString();
                    upgradeInside_PriceTextmesh[i, j].Commit();
					
					upgradeButton_Objs[i,j].active = true;
					upgradeButton_Sprites[i,j].spriteId = activeUpgradeButtonID;
                }
            }			
			
			if(BakeryShop.NumberOfCansellItem.Contains(6) || Mz_StorageManage.AccountBalance < firstPage_prices[0,0]) {
				upgradeButton_Sprites[0,0].spriteId = UnActiveUpgradeButtonID;

                if(BakeryShop.NumberOfCansellItem.Contains(6)) {
                    upgradeInsideSprite2D[0, 0].color = Color.grey;
					upgradeButton_Objs[0,0].SetActiveRecursively(false);
				}
			}
			if(CreamBeh.arr_CreamBehs[1] != string.Empty || Mz_StorageManage.AccountBalance < firstPage_prices[0,1]) {
				upgradeButton_Sprites[0,1].spriteId = UnActiveUpgradeButtonID;

                if (CreamBeh.arr_CreamBehs[1] != string.Empty) {
                    upgradeInsideSprite2D[0, 1].color = Color.grey;
					upgradeButton_Objs[0,1].SetActiveRecursively(false);
				}
			}
            if (BakeryShop.NumberOfCansellItem.Contains(12) || BakeryShop.NumberOfCansellItem.Contains(13) || BakeryShop.NumberOfCansellItem.Contains(14) || Mz_StorageManage.AccountBalance < firstPage_prices[0, 2]) 
			{
				upgradeButton_Sprites[0,2].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(12) || BakeryShop.NumberOfCansellItem.Contains(13) || BakeryShop.NumberOfCansellItem.Contains(14))
                {
                    upgradeInsideSprite2D[0, 2].color = Color.grey;
					upgradeButton_Objs[0,2].SetActiveRecursively(false);
				}
			}
            if (BakeryShop.NumberOfCansellItem.Contains(15) || BakeryShop.NumberOfCansellItem.Contains(16) || BakeryShop.NumberOfCansellItem.Contains(17) || Mz_StorageManage.AccountBalance < firstPage_prices[0, 3])
            {
				upgradeButton_Sprites[0,3].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(15) || BakeryShop.NumberOfCansellItem.Contains(16) || BakeryShop.NumberOfCansellItem.Contains(17))
                {
                    upgradeInsideSprite2D[0, 3].color = Color.grey;
					upgradeButton_Objs[0,3].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(19) || Mz_StorageManage.AccountBalance < firstPage_prices[1,0]) {
				upgradeButton_Sprites[1,0].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(19)) {
                    upgradeInsideSprite2D[1, 0].color = Color.grey;
					upgradeButton_Objs[1,0].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(21) || Mz_StorageManage.AccountBalance < firstPage_prices[1,1]) {
				upgradeButton_Sprites[1,1].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(21)) {
                    upgradeInsideSprite2D[1, 1].color = Color.grey;
					upgradeButton_Objs[1,1].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(25) || Mz_StorageManage.AccountBalance < firstPage_prices[1,2]) {
				upgradeButton_Sprites[1,2].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(25)) {
                    upgradeInsideSprite2D[1, 2].color = Color.grey;
					upgradeButton_Objs[1,2].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(28) || Mz_StorageManage.AccountBalance < firstPage_prices[1,3])	{
				upgradeButton_Sprites[1,3].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(28)) {
                    upgradeInsideSprite2D[1, 3].color = Color.grey;
					upgradeButton_Objs[1,3].SetActiveRecursively(false);
				}
            }

            #endregion
        }
        else if(currentPageIndex == 1)
        {
            #region <@-- Page index = 1;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string nameSpecify = secondPage_spriteNames[i, j];
                    upgradeInsideSprite2D[i, j].spriteId = upgradeInsideSprite2D[i, j].GetSpriteIdByName(nameSpecify);
                    upgradeInsideSprite2D[i, j].color = Color.white;

                    upgradeInside_PriceTextmesh[i, j].text = secondPage_prices[i, j].ToString();
                    upgradeInside_PriceTextmesh[i, j].Commit();
					
					upgradeButton_Objs[i,j].active = true;
					upgradeButton_Sprites[i,j].spriteId = activeUpgradeButtonID;
                }
            }
			
			if(BakeryShop.NumberOfCansellItem.Contains(1) || Mz_StorageManage.AccountBalance < secondPage_prices[0,0]) {
				upgradeButton_Sprites[0,0].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(1)) {
                    upgradeInsideSprite2D[0, 0].color = Color.grey;
					upgradeButton_Objs[0,0].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(2) || Mz_StorageManage.AccountBalance < secondPage_prices[0,1]) {
				upgradeButton_Sprites[0,1].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(2)) {
                    upgradeInsideSprite2D[0, 1].color = Color.grey;
					upgradeButton_Objs[0,1].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(7) || Mz_StorageManage.AccountBalance < secondPage_prices[0,2]) {
				upgradeButton_Sprites[0,2].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(7)) {
                    upgradeInsideSprite2D[0, 2].color = Color.grey;
					upgradeButton_Objs[0,2].SetActiveRecursively(false);
				}
			}
            if (CreamBeh.arr_CreamBehs[2] != string.Empty || Mz_StorageManage.AccountBalance < secondPage_prices[0, 3]) {
                upgradeButton_Sprites[0, 3].spriteId = UnActiveUpgradeButtonID;

                if (CreamBeh.arr_CreamBehs[2] != string.Empty) {
                    upgradeInsideSprite2D[0, 3].color = Color.grey;
					upgradeButton_Objs[0,3].SetActiveRecursively(false);
				}
            }
			if(BakeryShop.NumberOfCansellItem.Contains(20) || Mz_StorageManage.AccountBalance < secondPage_prices[1,0]) {
				upgradeButton_Sprites[1,0].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(20)) {
                    upgradeInsideSprite2D[1, 0].color = Color.grey;
					upgradeButton_Objs[1,0].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(22) || Mz_StorageManage.AccountBalance < secondPage_prices[1,1])	{
				upgradeButton_Sprites[1,1].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(22)) {
                    upgradeInsideSprite2D[1, 1].color = Color.grey;
					upgradeButton_Objs[1,1].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(26) || Mz_StorageManage.AccountBalance < secondPage_prices[1,2]) {
				upgradeButton_Sprites[1,2].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(26)) {
                    upgradeInsideSprite2D[1, 2].color = Color.grey;
					upgradeButton_Objs[1,2].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(3) || Mz_StorageManage.AccountBalance < secondPage_prices[1,3]) {
				upgradeButton_Sprites[1,3].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(3)) {
                    upgradeInsideSprite2D[1, 3].color = Color.grey;
					upgradeButton_Objs[1,3].SetActiveRecursively(false);
				}
            }

            #endregion
        }
        else if(currentPageIndex == 2)
        {
            #region <@-- Page index == 2

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string nameSpecify = thirdPage_spriteNames[i, j];
					if(nameSpecify != "") {
                    	upgradeInsideSprite2D[i, j].spriteId = upgradeInsideSprite2D[i, j].GetSpriteIdByName(nameSpecify);
                        upgradeInsideSprite2D[i, j].color = Color.white;

                    	upgradeInside_PriceTextmesh[i, j].text = thirdPage_prices[i, j].ToString();
                    	upgradeInside_PriceTextmesh[i, j].Commit();
						
						upgradeButton_Objs[i,j].active = true;
						upgradeButton_Sprites[i,j].spriteId = activeUpgradeButtonID;
					}
					else {
						tk2dSprite sprite = upgradeInsideSprite2D[i,j];
						tk2dTextMesh textmesh = upgradeInside_PriceTextmesh[i, j];
						sprite.spriteId = 27;
						textmesh.text = "none";
						textmesh.transform.localPosition = new Vector3(1, textmesh.transform.localPosition.y, textmesh.transform.localPosition.z);
						textmesh.Commit();
						
						upgradeButton_Objs[i,j].active = false;
					}
                }
            }
			
			if(BakeryShop.NumberOfCansellItem.Contains(4) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,0]) {
				upgradeButton_Sprites[0,0].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(4)) {
                    upgradeInsideSprite2D[0, 0].color = Color.grey;
					upgradeButton_Objs[0,0].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(8) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,1]) {
				upgradeButton_Sprites[0,1].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(8)) {
                    upgradeInsideSprite2D[0, 1].color = Color.grey;
					upgradeButton_Objs[0,1].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(23) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,2]) {
				upgradeButton_Sprites[0,2].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(23)) {
                    upgradeInsideSprite2D[0, 2].color = Color.grey;
					upgradeButton_Objs[0,2].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(24) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,3]) {
				upgradeButton_Sprites[0,3].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(24)) {
                    upgradeInsideSprite2D[0, 3].color = Color.grey;
					upgradeButton_Objs[0,3].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(27) || Mz_StorageManage.AccountBalance < thirdPage_prices[1,0]) {
				upgradeButton_Sprites[1,0].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(27)) {
                    upgradeInsideSprite2D[1, 0].color = Color.grey;
					upgradeButton_Objs[1,0].SetActiveRecursively(false);
				}
			}
			if(BakeryShop.NumberOfCansellItem.Contains(29) || Mz_StorageManage.AccountBalance < thirdPage_prices[1,1]) {
				upgradeButton_Sprites[1,1].spriteId = UnActiveUpgradeButtonID;

                if (BakeryShop.NumberOfCansellItem.Contains(29)) {
                    upgradeInsideSprite2D[1, 1].color = Color.grey;
					upgradeButton_Objs[1,1].SetActiveRecursively(false);
				}
            }

            #endregion
        }

        int temp_pageID = currentPageIndex + 1;
        displayCurrentPageID_Textmesh.text = temp_pageID + "/" + MAX_PAGE_NUMBER;
        displayCurrentPageID_Textmesh.Commit();
    }

    internal void BuyingUpgradeMechanism(string upgradeName) {
        if(currentPageIndex == 0) 
		{
			if(Main.Mz_AppVersion.GetAppVersion == Main.Mz_AppVersion.AppVersion.Pro) {
				#region <!-- page 0, low 0.
				
	            if(upgradeName == "upgrade_00")
	            {
	                #region <@-- "buying : blueberry_jam".

	                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,0]) 
					{
	                    int item_id = (int)GoodDataStore.FoodMenuList.ToastWithBlueberryJam;
						if(BakeryShop.NumberOfCansellItem.Contains(item_id) == false) {
	                        Debug.Log("buying : blueberry_jam");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = item_id, };
	                        this.ActiveComfirmationWindow();

	                        if (MainMenu._HasNewGameEvent) {
	                            sceneController.SetActivateTotorObject(false);
	                        }
	                    }
						else{
							this.PlaySoundWarning();
						}
	                }
					else {
						this.PLayWarningCannotBuyItem();
	                }

	                #endregion
	            }
	            else if(upgradeName == "upgrade_01")
	            {
	                #region <@-- "buying : blueberry_cream"

	                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,1])
	                {				
						if(CreamBeh.arr_CreamBehs[1] == string.Empty) 
						{
	                        int blueberry_cupcake = (int)GoodDataStore.FoodMenuList.Blueberry_cupcake;
	                        int blueberry_minicake = (int)GoodDataStore.FoodMenuList.Blueberry_minicake;
	                        int blueberry_cake = (int)GoodDataStore.FoodMenuList.Blueberry_cake;

	                        if (BakeryShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Chocolate_cupcake) 
							    && BakeryShop.NumberOfCansellItem.Contains(blueberry_cupcake) == false)
	                        {
								Debug.Log("buying : blueberry_cream");
	                            currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_id = blueberry_cupcake, AdditionalParams = CreamBeh.BlueberryCream };
	                            this.ActiveComfirmationWindow();
	                        }

	                        if (BakeryShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Chocolate_minicake)
							    && BakeryShop.NumberOfCansellItem.Contains(blueberry_minicake) == false) 
							{
								Debug.Log("buying : blueberry_cream");
								currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J=1, Item_id = blueberry_minicake, AdditionalParams = CreamBeh.BlueberryCream }; 
								this.ActiveComfirmationWindow();
							}

	                        if (BakeryShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Chocolate_cake)
	                            && BakeryShop.NumberOfCansellItem.Contains(blueberry_cake) == false)
							{
								Debug.Log("buying : blueberry_cream");
	                            currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_id = blueberry_cake, AdditionalParams = CreamBeh.BlueberryCream };
								this.ActiveComfirmationWindow();
							}
						}
						else {
							this.PlaySoundWarning();
						}
	                }
					else {
						this.PLayWarningCannotBuyItem();
	                }

	                #endregion
	            }
				else if(upgradeName == "upgrade_02")
	            {
	                #region <@-- buying : Mini Cake.

	                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,2])
					{
	                    Debug.Log("buying : Mini Cake");
						int chocolate_minicake = (int)GoodDataStore.FoodMenuList.Chocolate_minicake;
						int Blueberry_minicake = (int)GoodDataStore.FoodMenuList.Blueberry_minicake;
						int Strawberry_minicake = (int)GoodDataStore.FoodMenuList.Strawberry_minicake;

	                    if (!BakeryShop.NumberOfCansellItem.Contains(chocolate_minicake)
	                        && !BakeryShop.NumberOfCansellItem.Contains(Blueberry_minicake)
	                        && !BakeryShop.NumberOfCansellItem.Contains(Strawberry_minicake))
	                    {
	                        if (BakeryShop.NumberOfCansellItem.Contains(chocolate_minicake) == false && CreamBeh.arr_CreamBehs[0] == CreamBeh.ChocolateCream)
	                        {
	                            this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_id = chocolate_minicake };
	                            this.ActiveComfirmationWindow();
	                        }
	                        if (BakeryShop.NumberOfCansellItem.Contains(Blueberry_minicake) == false && CreamBeh.arr_CreamBehs[1] == CreamBeh.BlueberryCream)
	                        {
	                            this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_id = Blueberry_minicake, };
	                            this.ActiveComfirmationWindow();
	                        }
	                        if (BakeryShop.NumberOfCansellItem.Contains(Strawberry_minicake) == false && CreamBeh.arr_CreamBehs[2] == CreamBeh.StrawberryCream)
	                        {
	                            this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_id = Strawberry_minicake, };
	                            this.ActiveComfirmationWindow();
	                        }
	                    }
	                    else
	                    {
	                        this.PlaySoundWarning();
	                    }
					}
					else {
						PLayWarningCannotBuyItem();
	                }

	                #endregion
	            }
				else if(upgradeName == "upgrade_03")
	            {
	                #region <@-- "buying : Cake".

	                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0, 3]) 
	                {
	                    int Chocolate_cake = (int)GoodDataStore.FoodMenuList.Chocolate_cake;
	                    int Blueberry_cake = (int)GoodDataStore.FoodMenuList.Blueberry_cake;
	                    int Strawberry_cake = (int)GoodDataStore.FoodMenuList.Strawberry_cake;

	                    Debug.Log("buying : Cake");

	                    if(BakeryShop.NumberOfCansellItem.Contains(Chocolate_cake) == false && 
	                        BakeryShop.NumberOfCansellItem.Contains(Blueberry_cake) == false &&                    
	                        BakeryShop.NumberOfCansellItem.Contains(Strawberry_cake) == false)
	                    {
						    if(BakeryShop.NumberOfCansellItem.Contains(Chocolate_cake) == false && CreamBeh.arr_CreamBehs[0] == CreamBeh.ChocolateCream) {
	                            this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_id = Chocolate_cake };
	                            this.ActiveComfirmationWindow();
							}
	                        if (BakeryShop.NumberOfCansellItem.Contains(Blueberry_cake) == false && CreamBeh.arr_CreamBehs[1] == CreamBeh.BlueberryCream) {
	                            this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_id = Blueberry_cake };
	                            this.ActiveComfirmationWindow();
	                        }
	                        if (BakeryShop.NumberOfCansellItem.Contains(Strawberry_cake) == false && CreamBeh.arr_CreamBehs[2] == CreamBeh.StrawberryCream) {
	                            this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_id = Strawberry_cake };
	                            this.ActiveComfirmationWindow();
	                        }
	                    }
						else{
							this.PlaySoundWarning();
						}
					}
					else {
						PLayWarningCannotBuyItem();
	                }

	                #endregion
	            }
				
				#endregion 

				#region <!-- page 0, Low 1.
				
				else if(upgradeName == "upgrade_10") 
				{	
					#region <@-- "buying : vanilla_icecream".

	                if (Mz_StorageManage.AccountBalance >= firstPage_prices[1, 0])
	                {
	                    int id = (int)GoodDataStore.FoodMenuList.Vanilla_icecream;
	                    if (BakeryShop.NumberOfCansellItem.Contains(id) == false)
	                    {
	                        Debug.Log("buying : vanilla_icecream");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 0, Item_id = id, };
	                        this.ActiveComfirmationWindow();
	                    }
	                    else
	                        this.PlaySoundWarning();
	                }
	                else
	                {
						PLayWarningCannotBuyItem();
	                }

					#endregion
				}
				else if(upgradeName == "upgrade_11") 
				{		
					#region <@-- "buying : tuna_sandwich".
					
					if(Mz_StorageManage.AccountBalance >= firstPage_prices[1, 1]) 
	                {
	                    int id = (int)GoodDataStore.FoodMenuList.Tuna_sandwich;
	                    if (BakeryShop.NumberOfCansellItem.Contains(id) == false)
	                    {
	                        Debug.Log("buying : tuna_sandwich");
	                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 1, Item_id = id, };
	                        this.ActiveComfirmationWindow();
						}
						else{
							this.PlaySoundWarning();
						}
					}
					else {
						this.PLayWarningCannotBuyItem();
					}
					
					#endregion
				}
				else if(upgradeName == "upgrade_12")
				{				
					#region <@-- "buying : chocolate_chip_cookie".

					if(Mz_StorageManage.AccountBalance >= firstPage_prices[1, 2])
	                {
	                    int id = (int)GoodDataStore.FoodMenuList.Chocolate_cookie;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : chocolate_chip_cookie");
	                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 2, Item_id = id };
	                        this.ActiveComfirmationWindow();
						}
						else {
							this.PlaySoundWarning();
						}
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_13") 
				{
					#region <@-- "buying : hotdog".

					if(Mz_StorageManage.AccountBalance >= firstPage_prices[1, 3]) 
	                {
	                    int id = (int)GoodDataStore.FoodMenuList.Hotdog;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : hotdog");
	                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 3, Item_id = id };
	                        this.ActiveComfirmationWindow();
						}
						else {
							this.PlaySoundWarning();
						}
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				
				#endregion
			}
			else if(Main.Mz_AppVersion.GetAppVersion == Main.Mz_AppVersion.AppVersion.Lite) {
				if(upgradeName == "upgrade_00")
				{
					#region <@-- "buying : blueberry_jam".
					
					if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,0]) {
						int item_id = (int)GoodDataStore.FoodMenuList.ToastWithBlueberryJam;
						if(BakeryShop.NumberOfCansellItem.Contains(item_id) == false) {
							Debug.Log("buying : blueberry_jam");
							currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = item_id, };
							this.ActiveComfirmationWindow();
							
							if (MainMenu._HasNewGameEvent) {
								sceneController.SetActivateTotorObject(false);
							}
						}
						else{
							this.PlaySoundWarning();
						}
					}
					else {
						this.PLayWarningCannotBuyItem();
					}
					
					#endregion
				}
	            else if(upgradeName == "upgrade_01")
	            {
	                #region <@-- "buying : blueberry_cream"

	                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,1])
	                {				
						if(CreamBeh.arr_CreamBehs[1] == string.Empty) 
						{
	                        int blueberry_cupcake = (int)GoodDataStore.FoodMenuList.Blueberry_cupcake;
	                        int blueberry_minicake = (int)GoodDataStore.FoodMenuList.Blueberry_minicake;
	                        int blueberry_cake = (int)GoodDataStore.FoodMenuList.Blueberry_cake;

	                        if (BakeryShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Chocolate_cupcake) 
							    && BakeryShop.NumberOfCansellItem.Contains(blueberry_cupcake) == false)
	                        {
								Debug.Log("buying : blueberry_cream");
	                            currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_id = blueberry_cupcake, AdditionalParams = CreamBeh.BlueberryCream };
	                            this.ActiveComfirmationWindow();
	                        }

	                        if (BakeryShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Chocolate_minicake)
							    && BakeryShop.NumberOfCansellItem.Contains(blueberry_minicake) == false) 
							{
								Debug.Log("buying : blueberry_cream");
								currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J=1, Item_id = blueberry_minicake, AdditionalParams = CreamBeh.BlueberryCream }; 
								this.ActiveComfirmationWindow();
							}

	                        if (BakeryShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Chocolate_cake)
	                            && BakeryShop.NumberOfCansellItem.Contains(blueberry_cake) == false)
							{
								Debug.Log("buying : blueberry_cream");
	                            currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_id = blueberry_cake, AdditionalParams = CreamBeh.BlueberryCream };
								this.ActiveComfirmationWindow();
							}
						}
						else {
							this.PlaySoundWarning();
						}
	                }
					else {
						this.PLayWarningCannotBuyItem();
	                }

	                #endregion
	            }
				else {
//					TK_news.GotoiTunesStore();
					this.ActiveLimitVersionWindows();
				}
			}
        }
		else if(currentPageIndex == 1)
		{
			if(Main.Mz_AppVersion.GetAppVersion == Main.Mz_AppVersion.AppVersion.Pro) {
				#region <!-- Page 1 low 0.
				
				if(upgradeName == "upgrade_00")
				{	
					#region <@-- "buying : appleJuiceTank".

	                if(Mz_StorageManage.AccountBalance >= secondPage_prices[0,0]) 
	                {
						Debug.Log("buying : appleJuiceTank");
	                    int id = (int)GoodDataStore.FoodMenuList.Apple_juice;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
	                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = id };
							this.ActiveComfirmationWindow();
						}
						else
							PlaySoundWarning();
	                }
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_01") 
				{
					#region <@-- "buying : chocolateMilkTank".

	                if(Mz_StorageManage.AccountBalance >= secondPage_prices[0,1])
	                {
	                    int id = (int)GoodDataStore.FoodMenuList.Cocoa_milk;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : chocolateMilkTank");
	                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_id = id };
	                        this.ActiveComfirmationWindow();
						}
						else
							PlaySoundWarning();
	                }
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_02") 
				{
					#region <@-- "buying : butter_jam".

					if(Mz_StorageManage.AccountBalance >= secondPage_prices[0,2]) 
	                {
	                    int id = (int)GoodDataStore.FoodMenuList.ToastWithButterJam;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : butter_jam");
	                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_id = id };
	                        this.ActiveComfirmationWindow();
						}
						else
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_03") 
				{
					#region <@-- "buying : strawberry_cream".

	                if (Mz_StorageManage.AccountBalance >= secondPage_prices[0, 3])
	                {
	                    if (CreamBeh.arr_CreamBehs[2] == string.Empty)
	                    {
	                        Debug.Log("buying : strawberry_cream");

	                        int Strawberry_cupcake = (int)GoodDataStore.FoodMenuList.Strawberry_cupcake;
	                        int Strawberry_minicake = (int)GoodDataStore.FoodMenuList.Strawberry_minicake;
	                        int Strawberry_cake = (int)GoodDataStore.FoodMenuList.Strawberry_cake;

	                        if (BakeryShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Chocolate_cupcake) && BakeryShop.NumberOfCansellItem.Contains(Strawberry_cupcake) == false) 
	                        {
	                            this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_id = Strawberry_cupcake, AdditionalParams = CreamBeh.StrawberryCream };
	                            this.ActiveComfirmationWindow();
	                        }

	                        if (BakeryShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Chocolate_minicake) && BakeryShop.NumberOfCansellItem.Contains(Strawberry_minicake) == false)
	                        {
	                            this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_id = Strawberry_minicake, AdditionalParams = CreamBeh.StrawberryCream };
	                            this.ActiveComfirmationWindow();
	                        }

	                        if (BakeryShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Chocolate_cake) && BakeryShop.NumberOfCansellItem.Contains(Strawberry_cake) == false) 
	                        {
	                            this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_id = Strawberry_cake, AdditionalParams = CreamBeh.StrawberryCream };
	                            this.ActiveComfirmationWindow();
	                        }
	                    }
	                    else
	                        PlaySoundWarning();
	                }
	                else
					{
						this.PLayWarningCannotBuyItem();
	                }

					#endregion
				}
				
				#endregion

				#region <!-- page1 low 1.

				if(upgradeName == "upgrade_10")
				{				
					#region <@-- "buying : chocolate_icecream".

					if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,0]) 
	                {
	                    int id = (int)GoodDataStore.FoodMenuList.Chocolate_icecream;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : chocolate_icecream");
	                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 0, Item_id = id };
	                        this.ActiveComfirmationWindow();
						}
						else 
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_11")
				{				
					#region <@-- "buying : deep_fried_chicken_sandwich".

					if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,1])
	                {		
				        int id = (int)GoodDataStore.FoodMenuList.DeepFriedChicken_sandwich;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : deep_fried_chicken_sandwich");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 1, Item_id = id };
	                        ActiveComfirmationWindow();
						}
						else 
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_12")
				{				
					#region <@-- "buying : fruit_cookie".

					if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,2]) 
	                {
	                    int id = (int)GoodDataStore.FoodMenuList.Fruit_cookie;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : fruit_cookie");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 2, Item_id = id };
	                        ActiveComfirmationWindow();
	                    }
						else
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_13")
				{				
					#region <@-- "buying : orangeJuiceTank".

					if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,3]) 
					{
	                    int id = (int)GoodDataStore.FoodMenuList.Orange_juice;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : orangeJuiceTank");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 3, Item_id = id };
	                        ActiveComfirmationWindow();
						}
						else
							PlaySoundWarning();
	                }
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}

				#endregion
			}
			else if(Main.Mz_AppVersion.GetAppVersion == Main.Mz_AppVersion.AppVersion.Lite) {
//				TK_news.GotoiTunesStore();
				this.ActiveLimitVersionWindows();
			}
		}
		else if(currentPageIndex == 2) 
		{
			if(Main.Mz_AppVersion.GetAppVersion == Main.Mz_AppVersion.AppVersion.Pro) {
				#region <!-- page2 low 0.
				
				if(upgradeName == "upgrade_00")
				{				
					#region <@-- "buying : freshMilkTank".

					if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,0]) {
	                    int id = (int)GoodDataStore.FoodMenuList.Freshmilk;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : freshMilkTank");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = id };
	                        ActiveComfirmationWindow();
						}
						else 
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_01")
				{		
					#region <@-- "buying : custard_jam".

					if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,1]) {
	                    int id = (int)GoodDataStore.FoodMenuList.ToastWithCustardJam;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : custard_jam");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_id = id };
	                        ActiveComfirmationWindow();
						}
						else 
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_02") 
				{
					#region <@-- "buying : ham_sandwich".

					if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,2]) {
	                    int id = (int)GoodDataStore.FoodMenuList.Ham_sandwich;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : ham_sandwich");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_id = id };
	                        ActiveComfirmationWindow();
						}
						else
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_03") 
				{
					#region <@-- "buying : egg_sandwich".

					if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,3]) 
					{
	                    int id = (int)GoodDataStore.FoodMenuList.Egg_sandwich;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false)  {
							Debug.Log("buying : egg_sandwich");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_id = id };
	                        ActiveComfirmationWindow();
						}
						else
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}

				#endregion

				#region <!-- page2 low 1.

				if(upgradeName == "upgrade_10")
				{	
					#region <@-- "buying : butter_cookie".

					if(Mz_StorageManage.AccountBalance >= thirdPage_prices[1,0]) {
	                    int id = (int)GoodDataStore.FoodMenuList.Butter_cookie;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : butter_cookie");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 0, Item_id = id };
	                        ActiveComfirmationWindow();
						}
						else
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}
				else if(upgradeName == "upgrade_11") 
				{		
					#region <@-- "buying : hotdog_cheese".

					if(Mz_StorageManage.AccountBalance >= thirdPage_prices[1,1]) 
					{
	                    int id = (int)GoodDataStore.FoodMenuList.HotdogWithCheese;
						if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
							Debug.Log("buying : hotdog_cheese");
	                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 1, Item_id = id };
	                        ActiveComfirmationWindow();
						}
						else
							PlaySoundWarning();
					}
					else {
						this.PLayWarningCannotBuyItem();
					}

					#endregion
				}

				#endregion
			}
			else if(Main.Mz_AppVersion.GetAppVersion == Main.Mz_AppVersion.AppVersion.Lite) {
//				TK_news.GotoiTunesStore();
				ActiveLimitVersionWindows();
			}
		}
    }

	void BuyingUpgradeComplete (OnUpdateEvenArgs e) 
    {
        if (currentPageIndex == 0)
            Mz_StorageManage.AccountBalance -= firstPage_prices[e.I, e.J];
        else if (currentPageIndex == 1)
            Mz_StorageManage.AccountBalance -= secondPage_prices[e.I, e.J];
        else if (currentPageIndex == 2)
            Mz_StorageManage.AccountBalance -= thirdPage_prices[e.I, e.J];
		
        if (e.AdditionalParams != string.Empty)
        {
            switch (e.AdditionalParams)
            {
                case CreamBeh.BlueberryCream:
                    CreamBeh.arr_CreamBehs[1] = CreamBeh.BlueberryCream;
                        BakeryShop.NumberOfCansellItem.Add(10);
                    if (BakeryShop.NumberOfCansellItem.Contains(12))
                        BakeryShop.NumberOfCansellItem.Add(13);
                    if (BakeryShop.NumberOfCansellItem.Contains(15))
                        BakeryShop.NumberOfCansellItem.Add(16);
                    break;
                case CreamBeh.StrawberryCream:
                    CreamBeh.arr_CreamBehs[2] = CreamBeh.StrawberryCream;
                        BakeryShop.NumberOfCansellItem.Add(11);
                    if (BakeryShop.NumberOfCansellItem.Contains(12))
                        BakeryShop.NumberOfCansellItem.Add(14);
                    if (BakeryShop.NumberOfCansellItem.Contains(15))
                        BakeryShop.NumberOfCansellItem.Add(17);
                    break;
                default:
                    break;
            }
        }

        if(BakeryShop.NumberOfCansellItem.Contains(e.Item_id) == false)
            BakeryShop.NumberOfCansellItem.Add(e.Item_id);

        upgradeButton_Sprites[e.I, e.J].spriteId = UnActiveUpgradeButtonID;
		CalculateObjectsToDisplay();

		sceneController.ManageAvailabelMoneyBillBoard();
        sceneController.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, upgradeButton_Objs[e.I, e.J].transform);
        sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.longBring_clip);
	}

	void PlaySoundWarning ()
	{
		sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
		Debug.LogWarning("This Item has be upgraded");
	}

	private void PLayWarningCannotBuyItem() {
		sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);

		Debug.Log(SheepBank.Warning_CannotBuyItem);
		sceneController.audioDescribe.PlayOnecSound(sceneController.description_clips[11]);
		sceneController.CreateDepositIcon();
	}

    private void ActiveComfirmationWindow()
    {
        confirmationWindow.SetActiveRecursively(true);
    }

	internal void UnActiveComfirmationWindow ()
	{
    	confirmationWindow.SetActiveRecursively(false);
		currentOnUpdateTarget = null;
	}

    internal void UserComfirm()
    {
        this.OnUpgradeEvent_checkingDelegation(currentOnUpdateTarget);
    }

    private void Handle_OnUpgrade_Event(object sender, OnUpdateEvenArgs e)
    {
        this.BuyingUpgradeComplete(e);
        this.UnActiveComfirmationWindow();
    }
	
	private void ActiveLimitVersionWindows() {
		sceneController.limitVersionWindow_obj.SetActive(true);
		sceneController.shadowPlane_Obj.SetActive(true);
		sceneController.shadowPlane_Obj.transform.position = new Vector3(0,0,-23);
	}
}
