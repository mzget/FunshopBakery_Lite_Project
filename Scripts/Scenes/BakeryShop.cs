using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UE = UnityEngine;

[System.Serializable]
public class BakeryShopTutor {
	public GameObject greeting_textSprite;
	public GameObject greeting_textmesh;
	public GameObject goaway_button_obj;

    public enum TutorStatus {
        None = 0,
        Greeting,
        AcceptOrders,
        CheckAccuracy,
        Billing,
    };
    public TutorStatus currentTutorState;
};

public class BakeryShop : Mz_BaseScene {
	
	public Transform shop_background;
    public GameObject bakeryShop_backgroup_group;
    public ExtendAudioDescribeData audioDescriptionData = new ExtendAudioDescribeData();
	public CharacterAnimationManager TK_animationManager;
	public tk2dSprite shopLogo_sprite;
	public GoodDataStore goodDataStore;
	public static List<int> NumberOfCansellItem = new List<int>(30);
	public List<Food> CanSellGoodLists = new List<Food>();
	
	//@-- Audio_clip.
	public AudioClip[] en_greeting_clip = new AudioClip[6];
	public AudioClip[] th_greeting_clip = new AudioClip[7];
    private AudioClip[] apologize_clip = new AudioClip[5];
//	private AudioClip[] appreciate_clips = new AudioClip[5];
	private AudioClip[] thanksCustomer_clips = new AudioClip[2];
	
	//<!-- in game button.
	public GameObject close_button;	
	public GameObject billingMachine;
    private tk2dAnimatedSprite billingAnimatedSprite;
	private AnimationState billingMachine_animState;
	private const string TH_001 = "TH_001";
	private const string TH_002 = "TH_002";
	private const string TH_003 = "TH_003";
	private const string TH_004 = "TH_004";
	private const string TH_005 = "TH_005";
	private const string TH_006 = "TH_006";
	private const string EN_001 = "EN_001";
	private const string EN_002 = "EN_002";
	private const string EN_003 = "EN_003";
	private const string EN_004 = "EN_004";
	private const string EN_005 = "EN_005";
	private const string EN_006 = "EN_006";
	private const string EN_007 = "EN_007";
	
	//<!-- Miscellaneous game objects.	
	public BinBeh binBeh;
	public GameObject foodsTray_obj;
	public FoodTrayBeh foodTrayBeh;
    public GameObject calculator_group_instance;
    public GameObject receiptGUIForm_groupObj;
    public GameObject giveTheChangeGUIForm_groupObj;
    public tk2dTextMesh totalPrice_textmesh;
    public tk2dTextMesh receiveMoney_textmesh;
    public tk2dTextMesh change_textmesh;
    public tk2dTextMesh displayAnswer_textmesh;
    public GameObject baseOrderUI_Obj;
	public GameObject greetingMessage_ObjGroup;
	public GameObject darkShadowPlane;
	public GameObject rollingDoor_Obj;
	
	public GameObject[] arr_addNotations = new GameObject[2];
	public GameObject[] arr_goodsLabel = new GameObject[3];
	public tk2dSprite[] arr_GoodsTag = new tk2dSprite[3];
	public tk2dTextMesh[] arr_GoodsPrice_textmesh = new tk2dTextMesh[3];
    public tk2dSprite[] arr_orderingBaseItems = new tk2dSprite[3];
	private const string BASE_ORDER_ITEM_NORMAL = "Order_BaseItem";
	private const string BASE_ORDER_ITEM_COMPLETE = "Order_BaseItem_complete";
	public tk2dSprite[] arr_orderingItems = new tk2dSprite[3];
	private Mz_CalculatorBeh calculatorBeh;
    private GameObject cash_obj;
	private tk2dSprite cash_sprite;
    private GameObject packaging_Obj;
	public BakeryShopTutor bakeryShopTutor;

    //<!-- Core data
    public enum GamePlayState { 
		none = 0,
		GreetingCustomer = 1,
        Ordering,
		calculationPrice,
		receiveMoney,
		giveTheChange, 
		TradeComplete,
	};
    public GamePlayState currentGamePlayState;

    #region <!-- SouseMachine data fields group. 

//    public GameObject juiceTank_base_Obj;
	public tk2dSprite juiceTank_base_Sprite;
//	public GameObject pineAppple_tank_Obj;
	public GameObject appleTank_Obj;
	public GameObject orangeTank_Obj;
	public GameObject cocoaMilkTank_Obj;
	public GameObject freshMilkTank_Obj;

    #endregion
	
	#region <!-- Icecream data fields group.
	
	const string NameOfBaseTankIcecream_001 = "icecream_lv_1";
	const string NameOfBaseTankIcecream_002 = "icecream_lv_2";
	const string NameOfBaseTankIcecream_003 = "icecream_lv_3";
	public const string icecreamStrawberryTank_name = "Strawberry_machine";
	public const string icecreamVanillaTank_name = "Vanilla_machine";
	public const string icecreamChocolateTank_name = "Chocolate_machine";
	public tk2dSprite icecreamTankBase_Sprite;	
	public GameObject icecreamStrawberryTank_obj;
	public GameObject icecreamVanillaTank_obj;
	public GameObject icecreamChocolateTank_obj;
	
	#endregion

	#region <!-- Toast && Jam Obj group;

    public Transform toastObj_transform_group;
	public ToastBeh[] toasts = new ToastBeh[2]; 
	private Vector3 toast_1_pos = new Vector3(-0.415f, 0.419f, -1);
	private Vector3 toast_2_pos = new Vector3(-0.220f, 0.418f, -1);
    public GameObject strawberryJam_instance;
    public GameObject blueberryJam_instance;
    public GameObject freshButterJam_instance;
    public GameObject custardJam_instance;
	
	internal void SetAnimatedJamInstance(bool activeState) {	
		if(activeState)	{ 			
			if(this.strawberryJam_instance) 
				iTween.PunchPosition(this.strawberryJam_instance, iTween.Hash("y", .1f, "time", 1f, "looptype", iTween.LoopType.loop));	
			
			if(this.blueberryJam_instance)
				iTween.PunchPosition(this.blueberryJam_instance, iTween.Hash("y", -.1f, "time", 1f, "looptype", iTween.LoopType.loop));	

			if(this.freshButterJam_instance)
				iTween.PunchPosition(this.freshButterJam_instance, iTween.Hash("y", .1f, "time", 1f, "looptype", iTween.LoopType.loop));	
			
			if(this.custardJam_instance)
				iTween.PunchPosition(this.custardJam_instance, iTween.Hash("y", -.1f, "time", 1f, "looptype", iTween.LoopType.loop));	
		}
		else {			
			if(this.blueberryJam_instance) {
				iTween.Stop(this.blueberryJam_instance);	
				blueberryJam_instance.transform.position = blueberryJam_instance.GetComponent<JamBeh>().originalPosition;
			}
			if(this.custardJam_instance) {
				iTween.Stop(this.custardJam_instance);	
				custardJam_instance.transform.position = custardJam_instance.GetComponent<JamBeh>().originalPosition;
			}
			if(this.freshButterJam_instance) {
				iTween.Stop(this.freshButterJam_instance);
				freshButterJam_instance.transform.position = freshButterJam_instance.GetComponent<JamBeh>().originalPosition;	
			}
			if(this.strawberryJam_instance) {
				iTween.Stop(this.strawberryJam_instance);
				strawberryJam_instance.transform.position = strawberryJam_instance.GetComponent<JamBeh>().originalPosition;	
			}
		}
	}

	#endregion

	#region <!-- Cakes && CreamBeh data fields.
	
	public Transform cupcakeBase_transform;
	public Transform miniCakeBase_transform;
	public Transform cakeBase_transform;
	public CakeBeh cupcake;
	public CakeBeh miniCake;
	public CakeBeh cake;
    public GameObject chocolate_cream_Instance;
    public GameObject blueberry_cream_Instance;
    public GameObject strawberry_cream_Instance;
	
	internal void SetAnimatedCreamInstance(bool activeState) {	
		if(activeState)	{ 
			if(this.chocolate_cream_Instance)
				iTween.PunchPosition(this.chocolate_cream_Instance, iTween.Hash("y", .1f, "time", 1f, "looptype", iTween.LoopType.pingPong));
			if(this.strawberry_cream_Instance)
				iTween.PunchPosition(this.strawberry_cream_Instance, iTween.Hash("y", -.1f, "time", 1f, "looptype", iTween.LoopType.pingPong));			
			if(this.blueberry_cream_Instance)
				iTween.PunchPosition(this.blueberry_cream_Instance, iTween.Hash("y", .1f, "time", 1f, "looptype", iTween.LoopType.pingPong));	
		}
		else {			
			if(this.chocolate_cream_Instance) {
				iTween.Stop(this.chocolate_cream_Instance);		
				chocolate_cream_Instance.transform.position = chocolate_cream_Instance.GetComponent<CreamBeh>().originalPosition;
			}
			if(this.blueberry_cream_Instance) {
				iTween.Stop(this.blueberry_cream_Instance);	
				blueberry_cream_Instance.transform.position = blueberry_cream_Instance.GetComponent<CreamBeh>().originalPosition;
			}
			if(this.strawberry_cream_Instance) {
				iTween.Stop(this.strawberry_cream_Instance);	
				strawberry_cream_Instance.transform.position = strawberry_cream_Instance.GetComponent<CreamBeh>().originalPosition;
			}
		}
	}

	#endregion

    #region <!-- Sandwich && Cookie data fields group.

    public Transform sandwichCookieTray_Transform;
    public SandwichBeh tunaSandwich;
    public SandwichBeh deepFriedChickenSandwich;
    public SandwichBeh hamSandwich;
    public SandwichBeh eggSandwich;
    public CookieBeh chocolateChip_cookie;
    public CookieBeh fruit_cookie;
    public CookieBeh butter_cookie;

    #endregion

    #region <!-- Hotdog data fields group.

    public Transform hotdogTray_transform;
    internal HotdogBeh hotdog;
	public GameObject hotdogSauce;
	public GameObject hotdogCheese;

	#endregion

	#region <!-- Customer data group.

    public GameObject customerMenu_group_Obj;
    internal CustomerBeh currentCustomer;

    public event EventHandler nullCustomer_event;
    private void OnNullCustomer_event(EventArgs e) {
        if(nullCustomer_event != null) {
            nullCustomer_event(this, e);
		
			Debug.Log("Callback :: nullCustomer_event");
        }
    }
	
	#endregion	
    
    /// Manage goods complete Event handle.
    public event System.EventHandler manageGoodsComplete_event;
    private void OnManageGoodComplete(System.EventArgs e)
    {
        if (manageGoodsComplete_event != null) {
            manageGoodsComplete_event(this, e);
        }
    }

	// Use this for initialization
	IEnumerator Start () {		
		Mz_ResizeScale.ResizingScale(shop_background);
		
        yield return StartCoroutine(this.InitailizeSceneObject());
        StartCoroutine(this.InitializeGameEffect());
        this.OpenShop();
	}

    private IEnumerator InitailizeSceneObject()
    {
		foodTrayBeh = new FoodTrayBeh();
        goodDataStore = new GoodDataStore();
		
		StartCoroutine(ReInitializeAudioClipData());
        StartCoroutine(this.SceneInitializeAudio());
		StartCoroutine(this.ChangeShopLogoIcon());
		StartCoroutine(this.InitializeObjectAnimation());

		//<!-- Unactive Souse Tanks. 
		appleTank_Obj.SetActiveRecursively(false);
		orangeTank_Obj.SetActiveRecursively(false);
		cocoaMilkTank_Obj.SetActiveRecursively(false);
		freshMilkTank_Obj.SetActiveRecursively(false);
        //<@-- Unactive Icecream tanks.
		icecreamVanillaTank_obj.SetActiveRecursively(false);
		icecreamChocolateTank_obj.SetActiveRecursively(false);
		//<!-- Manage Jam Instance.
		blueberryJam_instance.SetActiveRecursively(false);
		freshButterJam_instance.SetActiveRecursively(false);
		custardJam_instance.SetActiveRecursively(false);

        StartCoroutine(CreateToastInstance());
        // <@-- Cake && Cream initilaize.
		StartCoroutine(this.InitializeCreamBeh());
        StartCoroutine(CreateCupcakeInstance());
        StartCoroutine(InitializeMinicakeInstance());
        StartCoroutine(InitializeCakeInstance());
        //<@-- Sandwich Initialize.
        StartCoroutine(InitializeTunaSandwichInstance());
		StartCoroutine(this.Initialize_deepFriedChickenSandwich());
		StartCoroutine(this.Initailize_HamSandwich());
		StartCoroutine(this.Initialize_EggSandwich());

        StartCoroutine(this.InitializeChocolateChipCookie());
		StartCoroutine(this.Initializing_FriutCookie());
		StartCoroutine(this.Initializing_ButterCookie());

        StartCoroutine(this.InitializeHotdogInstance());
		hotdogSauce.gameObject.active = false;
		hotdogCheese.gameObject.active = false;

        yield return null;
        
        calculator_group_instance.SetActiveRecursively(false);

		StartCoroutine(this.InitailizeShopLabelGUI());

        // Debug can sell list.
        StartCoroutine(this.InitializeCanSellGoodslist());
		
		close_button.active = true;
    }

    private void OpenShop() {
        iTween.MoveTo(rollingDoor_Obj, iTween.Hash("position", new Vector3(0, 2.2f, 1), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeInSine));
        audioEffect.PlayOnecSound(base.soundEffect_clips[0]);
				
        nullCustomer_event += new EventHandler(BakeryShop_nullCustomer_event);
        OnNullCustomer_event(EventArgs.Empty);

		if(MainMenu._HasNewGameEvent == false) {
			Destroy(bakeryShopTutor.greeting_textmesh);
			bakeryShopTutor = null;
		}
    }
   
	private IEnumerator SceneInitializeAudio ()
	{
        base.CreateAudioObject();
		
        audioBackground_Obj.audio.clip = base.background_clip;
        audioBackground_Obj.audio.loop = true;
        audioBackground_Obj.audio.volume = 0.8f;
        audioBackground_Obj.audio.Play();
		
		yield return null;
	}

    private const string PATH_OF_DYNAMIC_CLIP = "AudioClips/GameIntroduce/Shop/";
    private const string PATH_OF_MERCHANDISC_CLIP = "AudioClips/AudioDescribe/";
	private const string PATH_OD_APOLOGIZE_CLIP = "AudioClips/ApologizeClips/";
	private const string PATH_OF_APPRECIATE_CLIP = "AudioClips/AppreciateClips/";
	private const string PATH_OF_THANKS_CLIP = "AudioClips/ThanksClips/";
    private IEnumerator ReInitializeAudioClipData()
    {
        description_clips.Clear();
        if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.TH)
        {
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "1.TH_greeting", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "2.TH_ordering", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "3.TH_dragGoodsToTray", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "4.TH_checkingAccuracy", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "5.TH_billing", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "6.TH_calculationPrice", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "7.TH_giveTheChange", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "8.TH_completeTutor", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_noticeUserToUpgrade", typeof(AudioClip)) as AudioClip);
			
			apologize_clip[0] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_shortSorry_0001", typeof(AudioClip)) as AudioClip;
			apologize_clip[1] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_shortSorry_0002", typeof(AudioClip)) as AudioClip;
			apologize_clip[2] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_longSorry_0001", typeof(AudioClip)) as AudioClip;
			apologize_clip[3] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_longSorry_0002", typeof(AudioClip)) as AudioClip;
			apologize_clip[4] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_longSorry_0003", typeof(AudioClip)) as AudioClip;
			
			thanksCustomer_clips[0] = Resources.Load(PATH_OF_THANKS_CLIP + "TH_Thank_0001", typeof(AudioClip)) as AudioClip;
			thanksCustomer_clips[1] = Resources.Load(PATH_OF_THANKS_CLIP + "TH_Thank_0002", typeof(AudioClip)) as AudioClip;
			
//			appreciate_clips[0] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_001", typeof(AudioClip)) as AudioClip;
//			appreciate_clips[1] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_002", typeof(AudioClip)) as AudioClip;
//			appreciate_clips[2] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_003", typeof(AudioClip)) as AudioClip;
//			appreciate_clips[3] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_004", typeof(AudioClip)) as AudioClip;
//			appreciate_clips[4] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_005", typeof(AudioClip)) as AudioClip;
		}
        else if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.EN)
        {
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "1.EN_greeting", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "2.EN_ordering", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "3.EN_dragGoodsToTray", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "4.EN_checkingAccuracy", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "5.EN_billing", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "6.EN_calculationPrice", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "7.EN_giveTheChange", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "8.EN_completeTutor", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_noticeUserToUpgrade", typeof(AudioClip)) as AudioClip);
			
			apologize_clip[0] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_shortSorry_0001", typeof(AudioClip)) as AudioClip;
			apologize_clip[1] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_shortSorry_0002", typeof(AudioClip)) as AudioClip;
			apologize_clip[2] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_longSorry_0001", typeof(AudioClip)) as AudioClip;
			apologize_clip[3] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_longSorry_0002", typeof(AudioClip)) as AudioClip;
			apologize_clip[4] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_longSorry_0003", typeof(AudioClip)) as AudioClip;
			
			thanksCustomer_clips[0] = Resources.Load(PATH_OF_THANKS_CLIP + "EN_Thank_0001", typeof(AudioClip)) as AudioClip;
			thanksCustomer_clips[1] = Resources.Load(PATH_OF_THANKS_CLIP + "EN_Thank_0002", typeof(AudioClip)) as AudioClip;
			
//			appreciate_clips[0] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_001", typeof(AudioClip)) as AudioClip;
//			appreciate_clips[1] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_002", typeof(AudioClip)) as AudioClip;
//			appreciate_clips[2] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_003", typeof(AudioClip)) as AudioClip;
//			appreciate_clips[3] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_004", typeof(AudioClip)) as AudioClip;
//			appreciate_clips[4] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_005", typeof(AudioClip)) as AudioClip;
		}

        this.ReInitializingMerchandiseNameAudio();

        yield return 0;
    }

    private void ReInitializingMerchandiseNameAudio()
    {
		audioDescriptionData.merchandiseNameDescribes = new AudioClip[30];
		
        if(Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.EN) {
            for (int i = 0; i < 30; i++) {
                audioDescriptionData.merchandiseNameDescribes[i] =
                    Resources.Load(PATH_OF_MERCHANDISC_CLIP + "EN/" + goodDataStore.FoodDatabase_list[i].name, typeof(AudioClip)) as AudioClip;
            }
        }
        else if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.TH)
        {
            for (int i = 0; i < 30; i++)
            {
                audioDescriptionData.merchandiseNameDescribes[i] = 
                    Resources.Load(PATH_OF_MERCHANDISC_CLIP + "TH/" + goodDataStore.FoodDatabase_list[i].name, typeof(AudioClip)) as AudioClip;
            }
        }
    }

    private IEnumerator InitializeGameEffect()
    {
        if (gameEffectManager == null) {
            this.gameObject.AddComponent<GameEffectManager>();
            gameEffectManager = this.GetComponent<GameEffectManager>();
        }
        else
            yield return null;
    }

	#region <!-- Tutor systems.
	
	void CreateTutorObjectAtRuntime ()
	{
		cameraTutor_Obj = GameObject.FindGameObjectWithTag("MainCamera");
		
		handTutor = Instantiate(Resources.Load("Tutor_Objs/Town/HandTutor", typeof(GameObject))) as GameObject;
		handTutor.transform.parent = cameraTutor_Obj.transform;
		handTutor.transform.localPosition = new Vector3(0.3f, 0.75f, 3f);
		handTutor.transform.localScale = Vector3.one;
		
		GameObject tutorText_0 = Instantiate(Resources.Load("Tutor_Objs/SheepBank/Tutor_description", typeof(GameObject))) as GameObject;
		tutorText_0.transform.parent = cameraTutor_Obj.transform;
		tutorText_0.transform.localPosition = new Vector3(0.65f, 0.8f, 3f);
		tutorText_0.transform.localScale = Vector3.one;

		base.tutorDescriptions = new List<GameObject>();
		tutorDescriptions.Add(tutorText_0);
		//<@-- Animated hand with tweening.
		iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 0.65f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

	void CreateGreetingCustomerTutorEvent ()
    {
		bakeryShopTutor.greeting_textSprite.active = false;
		bakeryShopTutor.greeting_textmesh.active = true;
		darkShadowPlane.active = true;
		darkShadowPlane.transform.position += Vector3.back * 2f;

        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "GREETINGS";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();

        audioDescribe.PlayOnecSound(description_clips[0]);
	}

	void CreateAcceptOrdersTutorEvent ()
    {
		this.SetActivateTotorObject(true);
		bakeryShopTutor.goaway_button_obj.active = false;
		
		handTutor.transform.localPosition = new Vector3(-0.62f, -0.13f, 3f);
		
		tutorDescriptions[0].transform.localPosition = new Vector3(0f, 0f, 3f);
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "ACCEPT ORDERS";
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
		//<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", -0.2f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
		
		if(_isPlayAcceptOuderSound == false)
			StartCoroutine(this.WaitForHelloCustomer());
	}

	void GenerateGoodOrderTutorEvent ()
	{
		base.SetActivateTotorObject(true);
		cupcake.gameObject.transform.position += Vector3.back * 9f;
		cupcake.originalPosition = cupcake.transform.position;

        audioDescribe.PlayOnecSound(description_clips[2]);

        handTutor.transform.localPosition = new Vector3(-0.68f, 0.38f, 3f);
		
		tutorDescriptions[0].transform.localPosition = new Vector3(-0.48f, 0.5f, 3f);
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "TAP";
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
		//<@-- Animated hand with tweening.
		iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 0.45f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

	internal void CreateTabFoodIngredientTutorEvent ()
	{
//		throw new NotImplementedException ();
		base.SetActivateTotorObject(true);
				
		handTutor.transform.localPosition = new Vector3(-1.1f, 0.56f, 3f);
		handTutor.transform.rotation = Quaternion.Euler(0,0,220);
		
		tutorDescriptions[0].transform.localPosition = new Vector3(-0.9f, 0.75f, 3f);
//		tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "TAP";
//		tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();

		//<@-- Animated hand with tweening.
		iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 0.35f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

    internal void CreateDragGoodsToTrayTutorEvent()
    {
        Vector3 originalFoodTrayPos = foodsTray_obj.transform.position;
        foodsTray_obj.transform.position = new Vector3(originalFoodTrayPos.x, originalFoodTrayPos.y, -5.5f);

        base.SetActivateTotorObject(true);

        handTutor.transform.localPosition = new Vector3(-0.68f, 0.085f, 0.5f);
		handTutor.transform.rotation = Quaternion.Euler(Vector3.zero);
        tk2dSprite hand_sprite = handTutor.GetComponent<tk2dSprite>();
        hand_sprite.spriteId = hand_sprite.GetSpriteIdByName("HandDrag_tutor");

        tutorDescriptions[0].transform.localPosition = new Vector3(0.1f, 0f, 3f);
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "DRAG TO TRAY";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
        //<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("x", -0.18f, "y", -0.75f, "Time", 1f, "delay", 0.5f, "easetype", iTween.EaseType.easeInOutSine, "looptype", iTween.LoopType.loop));
    }
	
	private bool _isPlayAcceptOuderSound = false;
    private IEnumerator WaitForHelloCustomer()
    {
		_isPlayAcceptOuderSound = true;
		
        yield return new WaitForSeconds(en_greeting_clip[0].length);
        audioDescribe.PlayOnecSound(description_clips[1]);
    }

    private void CreateCheckingAccuracyTutorEvent()
    {
        this.SetActivateTotorObject(true);
        bakeryShopTutor.goaway_button_obj.active = false;
        Vector3 originalFoodTrayPos = foodsTray_obj.transform.position;
        foodsTray_obj.transform.position = new Vector3(originalFoodTrayPos.x, originalFoodTrayPos.y, -2f);

        audioDescribe.PlayOnecSound(description_clips[3]);

        handTutor.transform.localPosition = new Vector3(-0.62f, -0.13f, 3f);
        tk2dSprite hand_sprite = handTutor.GetComponent<tk2dSprite>();
        hand_sprite.spriteId = hand_sprite.GetSpriteIdByName("Hand_tutor");

        tutorDescriptions[0].transform.localPosition = new Vector3(0f, 0f, 3f);
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "CHECK ACCURACY";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
        //<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", -0.2f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
    }

    private void CreateBillingTutorEvent()
    {
        bakeryShopTutor.currentTutorState = BakeryShopTutor.TutorStatus.Billing;

        base.SetActivateTotorObject(true);
		darkShadowPlane.active = true;
        billingMachine.transform.position += Vector3.back * 5f;

        handTutor.transform.localPosition = new Vector3(-0.25f, -0.1f, 3f);

        tutorDescriptions[0].transform.localPosition = new Vector3(0.1f, 0f, 3f);
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "BILLING";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
        //<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 0f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));

        audioDescribe.PlayOnecSound(description_clips[4]);
    }

	void CreateNoticeUpgradeShopEvent ()
	{
		GameObject upgradeShop_button = Instantiate(Resources.Load("Tutor_Objs/NoticeUpgradeButton", typeof(GameObject))) as GameObject;
		upgradeShop_button.transform.position = new Vector3(0.4f, -0.75f, -4f);
		upgradeShop_button.name = "NoticeUpgradeButton";
		
		audioDescribe.PlayOnecSound(description_clips[8]);

		iTween.PunchScale(upgradeShop_button, 
			iTween.Hash("amount", Vector3.one * 0.2f, "time", 1f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

	#endregion

	IEnumerator ChangeShopLogoIcon ()
	{
		shopLogo_sprite.spriteId = shopLogo_sprite.GetSpriteIdByName(InitializeNewShop.shopLogo_NameSpecify[Mz_StorageManage.ShopLogo]);
		shopLogo_sprite.color = InitializeNewShop.shopLogos_Color[Mz_StorageManage.ShopLogoColor];

		yield return 0;
	}

	IEnumerator InitailizeShopLabelGUI ()
	{		
		if(Mz_StorageManage.Username != string.Empty) {
			base.shopnameTextmesh.text = Mz_StorageManage.ShopName;
			base.shopnameTextmesh.Commit();

			base.availableMoney.text = Mz_StorageManage.AvailableMoney.ToString();
			base.availableMoney.Commit();
		}
		yield return null;
	}

	IEnumerator InitializeObjectAnimation ()
	{
        billingMachine_animState = billingMachine.animation["billingMachine_anim"];
        billingMachine_animState.wrapMode = WrapMode.Once;
        billingAnimatedSprite = billingMachine.GetComponent<tk2dAnimatedSprite>();

		yield return 0;
	}

    private IEnumerator InitializeMinicakeInstance()
    {
        yield return StartCoroutine(CreateMiniCakeInstance());	
		miniCake.gameObject.active = false;
    }

    private IEnumerator InitializeCakeInstance()
    {
		yield return StartCoroutine(CreateCakeInstance());			
		cake.gameObject.active = false;
    }

    private IEnumerator InitializeHotdogInstance()
    {
        yield return StartCoroutine(this.CreateHotdog());
        hotdog.gameObject.SetActiveRecursively(false);
    }

    private IEnumerator InitializeCanSellGoodslist()
    {
		if(Mz_StorageManage.Username == string.Empty) {
	        BakeryShop.NumberOfCansellItem.Clear();
            for (int i = 0; i < 30; i++)
            {
                BakeryShop.NumberOfCansellItem.Add(i);
            }
            base.extendsStorageManager.SaveCanSellGoodListData();
		}
        if(BakeryShop.NumberOfCansellItem.Count == 0)
            base.extendsStorageManager.LoadCanSellGoodsListData();		

        yield return new WaitForFixedUpdate();

        foreach (int id in NumberOfCansellItem)
        {
            CanSellGoodLists.Add(goodDataStore.FoodDatabase_list[id]);
			
			#region Has page1 upgraded.
			
            if (id == 6)
                blueberryJam_instance.active = true;
			
			#region <@-- Cake object Instance.
			
            if (id == 12 || id == 13 || id == 14)
                miniCake.gameObject.active = true;
			if(id == 15 || id == 16 || id == 17)
				cake.gameObject.active = true;	
			
			#endregion
			
			#region <@-- IcecreamTank management.
			
			if(NumberOfCansellItem.Contains(19) && !NumberOfCansellItem.Contains(20)) {
				icecreamTankBase_Sprite.spriteId = icecreamTankBase_Sprite.GetSpriteIdByName(NameOfBaseTankIcecream_002);
				icecreamVanillaTank_obj.SetActiveRecursively(true);
				icecreamChocolateTank_obj.SetActiveRecursively(false);
			}
			if(NumberOfCansellItem.Contains(19) && NumberOfCansellItem.Contains(20)) {
				icecreamTankBase_Sprite.spriteId = icecreamTankBase_Sprite.GetSpriteIdByName(NameOfBaseTankIcecream_003);
				icecreamVanillaTank_obj.SetActiveRecursively(true);
				icecreamChocolateTank_obj.SetActiveRecursively(true);
			}
			if(!NumberOfCansellItem.Contains(19) && NumberOfCansellItem.Contains(20)) {
				icecreamTankBase_Sprite.spriteId = icecreamTankBase_Sprite.GetSpriteIdByName(NameOfBaseTankIcecream_003);
				icecreamVanillaTank_obj.SetActiveRecursively(false);
				icecreamChocolateTank_obj.SetActiveRecursively(true);
			}
			
			#endregion
			
			if(id == 21)
                tunaSandwich.gameObject.SetActiveRecursively(true);
            if (id == 25)
                chocolateChip_cookie.gameObject.SetActiveRecursively(true);
            if (id == 28) {
                hotdog.gameObject.SetActiveRecursively(true);
				hotdogSauce.gameObject.active = true;
			}

			#endregion
			
			#region Has page2 upgraded.
			
			if(id == 1) {
				appleTank_Obj.SetActiveRecursively(true);
				juiceTank_base_Sprite.spriteId = juiceTank_base_Sprite.GetSpriteIdByName("juiceTank_lv_2");
			}
            if(id == 2)
				cocoaMilkTank_Obj.SetActiveRecursively(true);
			if(id == 7)
				freshButterJam_instance.SetActiveRecursively(true);
			if(id == 22)
				deepFriedChickenSandwich.gameObject.SetActiveRecursively(true);
			if(id == 26)
				fruit_cookie.gameObject.SetActiveRecursively(true);
			if(id == 3)
				orangeTank_Obj.SetActiveRecursively(true);
			
			#endregion
			
			#region Has Page 3 Upgraded.

			if(id == 4)
				freshMilkTank_Obj.SetActiveRecursively(true);
			if(id == 8)
				custardJam_instance.SetActiveRecursively(true);
			if(id == 23)
				hamSandwich.gameObject.SetActiveRecursively(true);
			if(id == 24)
				eggSandwich.gameObject.SetActiveRecursively(true);
            if (id == 27)
                butter_cookie.gameObject.SetActiveRecursively(true);
            if (id == 29) {
                hotdog.gameObject.SetActiveRecursively(true);
				hotdogCheese.gameObject.active = true;
			}

			#endregion
        }
		
		Debug.Log("CanSellGoodLists.Count : " + CanSellGoodLists.Count);
		foreach (var item in CanSellGoodLists) {	
			Debug.Log(item.name);
		}
    }
	
	#region <!-- Cake && Cream object mechanism section.

	IEnumerator InitializeCreamBeh ()
	{
		chocolate_cream_Instance.active = true;
		if(CreamBeh.arr_CreamBehs[1] != string.Empty)
			blueberry_cream_Instance.active = true;
		else
			blueberry_cream_Instance.active = false;
		if(CreamBeh.arr_CreamBehs[2] != string.Empty)
			strawberry_cream_Instance.active = true;
		else
			strawberry_cream_Instance.active = false;
		

		yield return null;
	}
	
	IEnumerator CreateCupcakeInstance() {		
		yield return new WaitForFixedUpdate();
		
		if(cupcake == null) {
			GameObject temp_cupcake = Instantiate(Resources.Load(ObjectsBeh.Cakes_ResourcePath + CakeBeh.Cupcake, typeof(GameObject))) as GameObject;
			temp_cupcake.transform.parent = cupcakeBase_transform;
			temp_cupcake.transform.localPosition = new Vector3(0, 0.13f, -.2f);
			temp_cupcake.name = CakeBeh.Cupcake;
			cupcake = temp_cupcake.GetComponent<CakeBeh>();
			cupcake.destroyObj_Event += Handle_CupcakedestroyObj_Event;
			cupcake.putObjectOnTray_Event += Handle_CupcakeputObjectOnTray_Event;
		}
	}
	void Handle_CupcakedestroyObj_Event (object sender, System.EventArgs e) {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();

		StartCoroutine(CreateCupcakeInstance());
	}
	void Handle_CupcakeputObjectOnTray_Event (object sender, System.EventArgs e)
	{	
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			cupcake = null;			
			StartCoroutine(CreateCupcakeInstance());
		} 
		else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
	}

    IEnumerator CreateMiniCakeInstance() {
		yield return new WaitForFixedUpdate();
		
		if(miniCake == null) {
			GameObject temp_Minicake = Instantiate(Resources.Load(ObjectsBeh.Cakes_ResourcePath + CakeBeh.MiniCake, typeof(GameObject))) as GameObject;
			temp_Minicake.transform.parent = miniCakeBase_transform;
			temp_Minicake.transform.localPosition = new Vector3(-0.01f, 0.11f, -0.1f);
			temp_Minicake.name = CakeBeh.MiniCake;

			miniCake = temp_Minicake.GetComponent<CakeBeh>();
            miniCake.offsetPos = Vector3.up * -0.05f;
			miniCake.destroyObj_Event += new EventHandler(miniCake_destroyObj_Event);
            miniCake.putObjectOnTray_Event += new EventHandler(miniCake_putObjectOnTray_Event);
		}
    }
    void miniCake_destroyObj_Event(object sender, EventArgs e) {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();
		
		StartCoroutine(CreateMiniCakeInstance());
    }
    void miniCake_putObjectOnTray_Event(object sender, EventArgs e) {
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			miniCake = null;			
			StartCoroutine(CreateMiniCakeInstance());			
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
    }
	
    IEnumerator CreateCakeInstance() {
		yield return new WaitForFixedUpdate();
		
		if(cake == null) {
			GameObject temp_cake = Instantiate(Resources.Load(ObjectsBeh.Cakes_ResourcePath + CakeBeh.Cake, typeof(GameObject))) as GameObject;
			temp_cake.transform.parent = cakeBase_transform;
			temp_cake.transform.localPosition = new Vector3(-0.01f, 0.2f, -0.1f);
			temp_cake.name = CakeBeh.Cake;

			cake = temp_cake.GetComponent<CakeBeh>();
			cake.destroyObj_Event += new EventHandler(Cake_destroyObj_Event);
            cake.putObjectOnTray_Event += new EventHandler(Cake_putObjectOnTray_Event);
		}
    }
    void Cake_destroyObj_Event(object sender, EventArgs e) {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();
		
		StartCoroutine(CreateCakeInstance());
    }
    void Cake_putObjectOnTray_Event(object sender, EventArgs e) {
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
						
			cake = null;
			StartCoroutine(CreateCakeInstance());
			
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
    }
	
	#endregion
	
	#region <!-- Toast gameobject mechanism section.
	
    private IEnumerator CreateToastInstance()
    {
		yield return new WaitForFixedUpdate();
		
        if(toasts[0] == null) {
            GameObject temp_0 = Instantiate(Resources.Load(ObjectsBeh.ToastAndJam_ResourcePath + "breadPure", typeof(GameObject))) as GameObject;
		    toasts[0] = temp_0.GetComponent<ToastBeh>();
		    toasts[0].transform.parent = toastObj_transform_group;
		    toasts[0].transform.localPosition = toast_1_pos;
            toasts[0].putObjectOnTray_Event += new System.EventHandler(PutToastOnTrayEvent);
            toasts[0].destroyObj_Event += new System.EventHandler(DestroyToastEvent);
        }
		
        if(toasts[1] == null) {
		    GameObject temp_1 = Instantiate(Resources.Load(ObjectsBeh.ToastAndJam_ResourcePath + "breadPure", typeof(GameObject))) as GameObject;
		    toasts[1] = temp_1.GetComponent<ToastBeh>();
		    toasts[1].transform.parent = toastObj_transform_group;
		    toasts[1].transform.localPosition = toast_2_pos;
            toasts[1].putObjectOnTray_Event += new System.EventHandler(PutToastOnTrayEvent);
            toasts[1].destroyObj_Event += new System.EventHandler(DestroyToastEvent);
        }
    }
	
	public void DestroyToastEvent(object sender, System.EventArgs e) {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();

		StartCoroutine(CreateToastInstance());
	}

    public void PutToastOnTrayEvent(object sender, System.EventArgs e) {
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			StartCoroutine(CreateToastInstance());			
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
    }
	
	#endregion
	
	#region <!-- Sandwich Obj behavior.

	IEnumerator InitializeTunaSandwichInstance()
	{
		yield return StartCoroutine(this.CreateTunaSandwich());
		tunaSandwich.gameObject.SetActiveRecursively(false);
	}
	
	IEnumerator Initialize_deepFriedChickenSandwich()
	{
		yield return StartCoroutine(this.CreateDeepFriedChickenSandwich());
		deepFriedChickenSandwich.gameObject.SetActiveRecursively(false);
	}
	
	IEnumerator Initailize_HamSandwich ()
	{
		yield return StartCoroutine(this.CreateHamSanwich());
		hamSandwich.gameObject.SetActiveRecursively(false);
	}
	
	IEnumerator Initialize_EggSandwich ()
	{
		yield return StartCoroutine(this.CreateEggSandwich());
		eggSandwich.gameObject.SetActiveRecursively(false);
	}
	
	/// <summary>
	/// Creates the tuna sandwich.
	/// </summary>
	IEnumerator CreateTunaSandwich() {
        yield return new WaitForFixedUpdate();

		if(tunaSandwich == null) {
			GameObject sandwich = Instantiate(Resources.Load(ObjectsBeh.Sandwich_ResourcePath + "TunaSandwich", typeof(GameObject))) as GameObject;
            sandwich.transform.parent = sandwichCookieTray_Transform;
            sandwich.transform.localPosition = new Vector3(.235f, -.15f, -.1f);
            sandwich.gameObject.name = GoodDataStore.FoodMenuList.Tuna_sandwich.ToString();

            tunaSandwich = sandwich.GetComponent<SandwichBeh>();
			tunaSandwich.costs = this.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Tuna_sandwich].costs;

            tunaSandwich.putObjectOnTray_Event += new EventHandler(tunaSandwich_putObjectOnTray_Event);
            tunaSandwich.destroyObj_Event += new EventHandler(tunaSandwich_destroyObj_Event);
		}
	}
    void tunaSandwich_putObjectOnTray_Event(object sender, EventArgs e) {
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			tunaSandwich = null;
			StartCoroutine(this.CreateTunaSandwich());
			
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
    }
    void tunaSandwich_destroyObj_Event(object sender, EventArgs e) {		
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine(this.CreateTunaSandwich());
    }
	
	/// <summary>
	/// Creates the deep fried chicken sandwich.
	/// </summary>
	IEnumerator CreateDeepFriedChickenSandwich() {
        yield return new WaitForFixedUpdate();

		if(deepFriedChickenSandwich == null) {
			GameObject sandwich = Instantiate(Resources.Load(ObjectsBeh.Sandwich_ResourcePath + "DeepFriedChickenSandwich", typeof(GameObject))) as GameObject;
			sandwich.transform.parent = sandwichCookieTray_Transform;
			sandwich.transform.localPosition = new Vector3(0.105f, -.16f, -.2f);
            sandwich.gameObject.name = GoodDataStore.FoodMenuList.DeepFriedChicken_sandwich.ToString();
			
			deepFriedChickenSandwich = sandwich.GetComponent<SandwichBeh>();
			deepFriedChickenSandwich.costs = this.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.DeepFriedChicken_sandwich].costs;

			deepFriedChickenSandwich.putObjectOnTray_Event += Handle_DeepFriedChickenSandwich_putObjectOnTray_Event;
			deepFriedChickenSandwich.destroyObj_Event += Handle_DeepFriedChickenSandwich_destroyObj_Event;
		}
	}
	void Handle_DeepFriedChickenSandwich_putObjectOnTray_Event(object sender, EventArgs e)
	{
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			deepFriedChickenSandwich = null;
			StartCoroutine(this.CreateDeepFriedChickenSandwich());			
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
	}
	void Handle_DeepFriedChickenSandwich_destroyObj_Event (object sender, EventArgs e)
	{
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine(this.CreateDeepFriedChickenSandwich());
	}
	
	/// <summary>
	/// Creates the ham sanwich.
	/// </summary>
	IEnumerator CreateHamSanwich() {
        yield return new WaitForFixedUpdate();
        
        if (hamSandwich == null) {
			GameObject sandwich = Instantiate(Resources.Load(ObjectsBeh.Sandwich_ResourcePath + "HamSandwich", typeof(GameObject))) as GameObject;
            sandwich.transform.parent = sandwichCookieTray_Transform;
            sandwich.transform.localPosition = new Vector3(-.015f, -.17f, -.3f);
            sandwich.gameObject.name = GoodDataStore.FoodMenuList.Ham_sandwich.ToString();

			hamSandwich = sandwich.GetComponent<SandwichBeh>();
			hamSandwich.costs = this.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Ham_sandwich].costs;

            hamSandwich.putObjectOnTray_Event += Handle_HamSandwich_putObjectOnTray_Event;
            hamSandwich.destroyObj_Event += Handle_HamSandwich_destroyObj_Event;
		}
	}
	void Handle_HamSandwich_putObjectOnTray_Event (object sender, EventArgs e)
	{
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			hamSandwich = null;
			StartCoroutine(this.CreateHamSanwich());
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
	}
	void Handle_HamSandwich_destroyObj_Event (object sender, EventArgs e)
	{
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine(this.CreateHamSanwich());
	}
	
	/// <summary>
	/// Creates the egg sandwich.
	/// </summary>
	IEnumerator CreateEggSandwich() {
        yield return new WaitForFixedUpdate();

		if(eggSandwich == null) {
			GameObject sandwich = Instantiate(Resources.Load(ObjectsBeh.Sandwich_ResourcePath + "EggSandwich", typeof(GameObject))) as GameObject;
            sandwich.transform.parent = sandwichCookieTray_Transform;
            sandwich.transform.localPosition = new Vector3(-.14f, -.17f, -.4f);
            sandwich.gameObject.name = GoodDataStore.FoodMenuList.Egg_sandwich.ToString();

			eggSandwich = sandwich.GetComponent<SandwichBeh>();
			eggSandwich.costs = this.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Egg_sandwich].costs;

            eggSandwich.putObjectOnTray_Event += Handle_EggSandwich_putObjectOnTray_Event;
            eggSandwich.destroyObj_Event += Handle_EggSandwich_destroyObj_Event;
		}
	}
	void Handle_EggSandwich_putObjectOnTray_Event (object sender, EventArgs e)
	{
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			eggSandwich = null;
			StartCoroutine(this.CreateEggSandwich());	
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
	}
	void Handle_EggSandwich_destroyObj_Event (object sender, EventArgs e)
	{
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine(this.CreateEggSandwich());
	}
	
	#endregion

    #region <!-- Cookie Object Behavior.

	/// Creates the chocolate chip_ cookie.
    IEnumerator InitializeChocolateChipCookie()
    {
        yield return StartCoroutine(this.CreateChocolateChip_Cookie());
        chocolateChip_cookie.gameObject.SetActiveRecursively(false);
    }  
    IEnumerator CreateChocolateChip_Cookie() {
        yield return new WaitForFixedUpdate();

        if(chocolateChip_cookie == null) {
            GameObject cookie = Instantiate(Resources.Load(ObjectsBeh.Cookie_ResourcePath + "ChocolateChip_Cookie", typeof(GameObject))) as GameObject;
            cookie.transform.parent = sandwichCookieTray_Transform;
            cookie.transform.localPosition = new Vector3(-.165f, 0.1f, -.1f);
            cookie.gameObject.name = GoodDataStore.FoodMenuList.Chocolate_cookie.ToString();

            chocolateChip_cookie = cookie.GetComponent<CookieBeh>();
			chocolateChip_cookie.costs = this.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Chocolate_cookie].costs;

            chocolateChip_cookie.putObjectOnTray_Event += new EventHandler(chocolateChip_cookie_putObjectOnTray_Event);
            chocolateChip_cookie.destroyObj_Event += new EventHandler(chocolateChip_cookie_destroyObj_Event);
        }
    }
    void chocolateChip_cookie_putObjectOnTray_Event(object sender, EventArgs e) {
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
						
			chocolateChip_cookie = null;
			StartCoroutine(this.CreateChocolateChip_Cookie());	
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
    }
	void chocolateChip_cookie_destroyObj_Event(object sender, EventArgs e) {    
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine(this.CreateChocolateChip_Cookie());
    }
	
	/// Create instance of fruit_cookie object and cookie behavior.
	IEnumerator Initializing_FriutCookie ()
	{
		yield return StartCoroutine(this.CreateFruitCookie());
		fruit_cookie.gameObject.SetActiveRecursively(false);
	}
	IEnumerator CreateFruitCookie() {
		yield return new WaitForFixedUpdate();

		if(fruit_cookie == null) {			
            GameObject cookie = Instantiate(Resources.Load(ObjectsBeh.Cookie_ResourcePath + "Fruit_Cookie", typeof(GameObject))) as GameObject;
            cookie.transform.parent = sandwichCookieTray_Transform;
            cookie.transform.localPosition = new Vector3(0.02f, 0.1f, -.1f);
            cookie.gameObject.name = GoodDataStore.FoodMenuList.Fruit_cookie.ToString();

            fruit_cookie = cookie.GetComponent<CookieBeh>();
			fruit_cookie.costs = this.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Fruit_cookie].costs;

            fruit_cookie.putObjectOnTray_Event += new EventHandler(Handle_FruitCookie_putObjectOnTray_Event);
            fruit_cookie.destroyObj_Event += new EventHandler(Handle_FruitCookie_DestroyObj_Event);
		}
	}
	void Handle_FruitCookie_putObjectOnTray_Event(object sender, EventArgs e) {
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;

			fruit_cookie = null;
			StartCoroutine(this.CreateFruitCookie());		
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
	}
	void Handle_FruitCookie_DestroyObj_Event(object sender, EventArgs e) {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine(this.CreateFruitCookie());
	}
    
    /// Create instance of ButterCookie object and cookie behavior.
	IEnumerator Initializing_ButterCookie ()
	{
		yield return StartCoroutine(this.CreateButterCookie());
		butter_cookie.gameObject.SetActiveRecursively(false);
	}
    IEnumerator CreateButterCookie()
    {
		yield return new WaitForFixedUpdate();

        if(butter_cookie == null) {
            GameObject cookie = Instantiate(Resources.Load(ObjectsBeh.Cookie_ResourcePath + "Butter_Cookie", typeof(GameObject))) as GameObject;
            cookie.transform.parent = sandwichCookieTray_Transform;
            cookie.transform.localPosition = new Vector3(.2f, 0.11f, -.1f);
            cookie.gameObject.name = GoodDataStore.FoodMenuList.Butter_cookie.ToString();

            butter_cookie = cookie.GetComponent<CookieBeh>();
			butter_cookie.costs = this.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Butter_cookie].costs;

            butter_cookie.putObjectOnTray_Event += new EventHandler(butter_cookie_putObjectOnTray_Event);
            butter_cookie.destroyObj_Event += new EventHandler(butter_cookie_destroyObj_Event);
        }
    }
    void butter_cookie_putObjectOnTray_Event(object sender, EventArgs e) 
	{
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;

			butter_cookie = null;
			StartCoroutine(this.CreateButterCookie());	
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
    }
	void butter_cookie_destroyObj_Event(object sender, EventArgs e) {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine(this.CreateButterCookie());
    }

    #endregion

    #region <!-- Hotdog object behavior.

    private IEnumerator CreateHotdog()
    {
        yield return new WaitForFixedUpdate();

        if(hotdog == null) {
            GameObject hotdog_obj = Instantiate(Resources.Load(ObjectsBeh.Hotdog_ResourcePath + "Hotdog", typeof(GameObject))) as GameObject;
            hotdog_obj.transform.parent = hotdogTray_transform;
            hotdog_obj.transform.localPosition = new Vector3(0.07f, 0.1f, -0.1f);
            hotdog_obj.gameObject.name = "Hotdog";

            hotdog = hotdog_obj.GetComponent<HotdogBeh>();
            hotdog.putObjectOnTray_Event += new EventHandler(hotdog_putObjectOnTray_Event);
            hotdog.destroyObj_Event += new EventHandler(hotdog_destroyObj_Event);
        }
    }
    void hotdog_putObjectOnTray_Event(object sender, EventArgs e) {    
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
						
			hotdog = null;
			StartCoroutine(this.CreateHotdog());	
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
    }
	void hotdog_destroyObj_Event(object sender, EventArgs e) {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		this.CreateDeductionsCoin (goods.costs);
		this.ReFreshAvailableMoney();		
		this.foodTrayBeh.goodsOnTray_List.Remove(goods);
		this.foodTrayBeh.ReCalculatatePositionOfGoods();
		
        StartCoroutine(this.CreateHotdog());
    }

    #endregion

	/// Handle shop_null customer_event.
    private void BakeryShop_nullCustomer_event(object sender, EventArgs e) {
		if(MainMenu._HasNewGameEvent) {
			StartCoroutine(this.WaitForCreateCustomer());
			close_button.gameObject.active = false;
		}
		else {
			StartCoroutine(CreateCustomer());
			darkShadowPlane.active = false;
			close_button.gameObject.active = true;
		}
    }

	IEnumerator WaitForCreateCustomer ()
	{
		yield return StartCoroutine(this.CreateCustomer());
		this.CreateTutorObjectAtRuntime();
		this.CreateGreetingCustomerTutorEvent();
	}

    private IEnumerator CreateCustomer() { 
		yield return new WaitForSeconds(1f);
		
        if(currentCustomer == null) {
            audioEffect.PlayOnecSound(audioEffect.dingdong_clip);
            this.manageGoodsComplete_event += Handle_manageGoodsComplete_event;
			
			GameObject customer = Instantiate(Resources.Load("Customers/CustomerBeh_obj", typeof(GameObject))) as GameObject;
            currentCustomer = customer.GetComponent<CustomerBeh>();
			
			currentCustomer.customerSprite_Obj = Instantiate(Resources.Load("Customers/Customer_AnimatedSprite", typeof(GameObject))) as GameObject;
			currentCustomer.customerSprite_Obj.transform.parent = customerMenu_group_Obj.transform;
			currentCustomer.customerSprite_Obj.transform.localPosition = new Vector3(0, 0, -.1f);
			
			currentCustomer.customerOrderingIcon_Obj = Instantiate(Resources.Load("Customers/CustomerOrdering_icon", typeof(GameObject))) as GameObject;
			currentCustomer.customerOrderingIcon_Obj.transform.parent = customerMenu_group_Obj.transform;
			currentCustomer.customerOrderingIcon_Obj.transform.localPosition = new Vector3(.35f, .05f, -.2f);
			currentCustomer.customerOrderingIcon_Obj.name = "OrderingIcon";
			
			currentCustomer.customerOrderingIcon_Obj.active = false;
        }
		else {
			Debug.LogError("Current Cusstomer does not correct destroying..." + " :: " + currentCustomer);
		}

		currentGamePlayState = GamePlayState.GreetingCustomer;
		this.SetActiveGreetingMessage(true);
    }

    private IEnumerator ExpelCustomer() {
        yield return new WaitForSeconds(1f);

	    if(currentCustomer != null) {
	        currentCustomer.Dispose();
			foreach (GoodsBeh item in foodTrayBeh.goodsOnTray_List) {
				item.OnDispose();
			}
			foodTrayBeh.goodsOnTray_List.Clear();
			StartCoroutine(this.CollapseOrderingGUI());
			this.manageGoodsComplete_event -= Handle_manageGoodsComplete_event;
	    }
		
		yield return new WaitForFixedUpdate();	
		
		OnNullCustomer_event(EventArgs.Empty);
    }

    public void Handle_manageGoodsComplete_event(object sender, System.EventArgs eventArgs)
	{
//		int r = UE.Random.Range(0, appreciate_clips.Length);
//		this.PlayAppreciateAudioClip(appreciate_clips[r]);
		
		audioEffect.PlayOnecWithOutStop(audioEffect.correctBring_clip);
		
		currentGamePlayState = GamePlayState.calculationPrice;

        TK_animationManager.PlayGoodAnimation();
        currentCustomer.customerOrderingIcon_Obj.active = false;

        StartCoroutine(this.ShowReceiptGUIForm());
    }

    private IEnumerator ShowReceiptGUIForm()
    {
		yield return new WaitForSeconds(0.5f);

		darkShadowPlane.active = true;
        
        audioEffect.PlayOnecSound(audioEffect.receiptCash_clip);
		
		this.CreateTKCalculator();
		calculatorBeh.result_Textmesh = displayAnswer_textmesh;
		receiptGUIForm_groupObj.SetActiveRecursively(true);
		this.DeActiveCalculationPriceGUI();
		this.ManageCalculationPriceGUI();

        if(MainMenu._HasNewGameEvent) {
            audioDescribe.PlayOnecSound(description_clips[5]);
        }
    }

	void DeActiveCalculationPriceGUI ()
	{
		for (int i = 0; i < arr_addNotations.Length; i++) {
			arr_addNotations[i].active = false;
		}
		for (int i = 0; i < arr_goodsLabel.Length; i++) {
			arr_goodsLabel[i].SetActiveRecursively(false);
		}
	}

	void ManageCalculationPriceGUI ()
	{		
		for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++) {
			arr_goodsLabel[i].SetActiveRecursively(true);
			arr_GoodsTag[i].spriteId = arr_GoodsTag[i].GetSpriteIdByName(currentCustomer.customerOrderRequire[i].food.name);
			arr_GoodsPrice_textmesh[i].text = currentCustomer.customerOrderRequire[i].food.price.ToString();
			arr_GoodsPrice_textmesh[i].Commit();
			if(i != 0)
				arr_addNotations[i - 1].active = true;
		}
	}

    internal void GenerateOrderGUI ()
	{
		foreach (tk2dSprite item in arr_orderingBaseItems) {
			item.spriteId = item.GetSpriteIdByName(BASE_ORDER_ITEM_NORMAL);
			item.gameObject.SetActiveRecursively (false);
		}

		for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++) {
			arr_orderingBaseItems[i].gameObject.SetActiveRecursively(true);	
			arr_orderingItems[i].spriteId = arr_orderingItems[i].GetSpriteIdByName(currentCustomer.customerOrderRequire[i].food.name);
            arr_orderingItems[i].gameObject.name = currentCustomer.customerOrderRequire[i].food.name;
		}

		StartCoroutine(this.ShowOrderingGUI());
		currentGamePlayState = GamePlayState.Ordering;
    }

	IEnumerator ShowOrderingGUI ()
	{
		if(MainMenu._HasNewGameEvent) {
			iTween.MoveTo(baseOrderUI_Obj.gameObject, 
			              iTween.Hash("position", new Vector3(-0.85f, .06f, -2.5f), "islocal", true, "time", .5f, "easetype", iTween.EaseType.spring));

            if (bakeryShopTutor.currentTutorState == BakeryShopTutor.TutorStatus.AcceptOrders)
                this.CreateAcceptOrdersTutorEvent();
            else if (bakeryShopTutor.currentTutorState == BakeryShopTutor.TutorStatus.CheckAccuracy) {
                base.SetActivateTotorObject(false);
                this.CreateCheckingAccuracyTutorEvent();
            }
		}
		else {
			iTween.MoveTo(baseOrderUI_Obj.gameObject, 
		              iTween.Hash("position", new Vector3(-0.85f, .06f, -1f), "islocal", true, "time", .5f, "easetype", iTween.EaseType.spring));
		}

		yield return new WaitForFixedUpdate();
		
		this.CheckingGoodsObjInTray(GoodsBeh.ClassName);
		currentCustomer.customerOrderingIcon_Obj.active = false;
		darkShadowPlane.active = true;
		
		foreach (var item in arr_orderingItems) {
			iTween.Resume(item.gameObject);
            iTween.MoveTo(item.gameObject, iTween.Hash("y", 0.1f, "islocal", true, "time", .3f, "looptype", iTween.LoopType.pingPong));
		}
	}

	IEnumerator CollapseOrderingGUI ()
	{
        if (MainMenu._HasNewGameEvent)
        {
			base.SetActivateTotorObject(false);
            
			iTween.MoveTo(baseOrderUI_Obj.gameObject,
                      iTween.Hash("position", new Vector3(-0.85f, -2f, 0f), "islocal", true, "time", 0.5f, "easetype", iTween.EaseType.linear));

            if (bakeryShopTutor.currentTutorState == BakeryShopTutor.TutorStatus.AcceptOrders)
            {
                this.GenerateGoodOrderTutorEvent();
                yield return new WaitForSeconds(0.5f);
                foreach (var item in arr_orderingItems)
                {
                    iTween.Pause(item.gameObject);
                }

                if (currentCustomer)
                {
                    currentCustomer.customerOrderingIcon_Obj.active = true;

                    iTween.PunchPosition(currentCustomer.customerOrderingIcon_Obj,
                        iTween.Hash("x", .1f, "y", .1f, "delay", 1f, "time", .5f, "looptype", iTween.LoopType.pingPong));
                }
            }
            else if(bakeryShopTutor.currentTutorState == BakeryShopTutor.TutorStatus.CheckAccuracy) {
				this.CreateBillingTutorEvent();
			}
        }
        else
        {
            iTween.MoveTo(baseOrderUI_Obj.gameObject,
                      iTween.Hash("position", new Vector3(-0.85f, -2f, 0f), "islocal", true, "time", 0.5f, "easetype", iTween.EaseType.linear));

            yield return new WaitForSeconds(0.5f);

            darkShadowPlane.active = false;
            foreach (var item in arr_orderingItems)
            {
                iTween.Pause(item.gameObject);
            }

            if (currentCustomer)
            {
                currentCustomer.customerOrderingIcon_Obj.active = true;

                iTween.PunchPosition(currentCustomer.customerOrderingIcon_Obj,
                    iTween.Hash("x", .1f, "y", .1f, "delay", 1f, "time", .5f, "looptype", iTween.LoopType.pingPong));
            }
        }
	}

	private void SetActiveGreetingMessage (bool activeState)
	{
		if(activeState) {
			greetingMessage_ObjGroup.SetActiveRecursively(true);
			iTween.ScaleTo(greetingMessage_ObjGroup, iTween.Hash("x", 1f, "y", 1f, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));
		}
		else {
			iTween.ScaleTo(greetingMessage_ObjGroup, iTween.Hash("x", 0.1f, "y", 0.1f, "time", 0.5f, "easetype", iTween.EaseType.easeInExpo,
				"oncomplete", "UnActiveGreetingMessage", "oncompletetarget", this.gameObject));
		}
	}
	
	void UnActiveGreetingMessage() {		
		greetingMessage_ObjGroup.SetActiveRecursively(false);
		
		if(MainMenu._HasNewGameEvent) {
			currentCustomer.GenerateTutorGoodOrderEvent();
		}
		else
			currentCustomer.GenerateGoodOrder();
	}
	
	IEnumerator PlayApologizeCustomer (AudioClip clip)
	{
		this.TK_animationManager.PlayEyeAnimation(CharacterAnimationManager.NameAnimationsList.agape);
		currentCustomer.PlayRampage_animation();
		
		while (TK_animationManager._isPlayingAnimation) {
			yield return null;
		}
		
		TK_animationManager.PlayTalkingAnimation();
		this.PlayApologizeAudioClip(clip);
	}

	#region <!-- Play Audio method.

	IEnumerator PlayGreetingAudioClip (AudioClip clip)
	{
		audioDescribe.audio.clip = clip;		
		audioDescribe.audio.Play();
		this.SetActiveGreetingMessage(false);
		
		TK_animationManager.PlayTalkingAnimation();
		
		yield return null;
	}

	void PlayAppreciateAudioClip (AudioClip audioClip)
	{
		this.audioDescribe.PlayOnecSound(audioClip);
	}

	void PlayApologizeAudioClip (AudioClip audioClip)
	{
		this.audioDescribe.PlayOnecSound(audioClip);
	}

	#endregion
    
    private void CreateTKCalculator() {
        if(calculator_group_instance) {
            calculator_group_instance.SetActiveRecursively(true);
			
			if(calculatorBeh == null) {
				calculatorBeh = calculator_group_instance.GetComponent<Mz_CalculatorBeh>();
			}
        }

        if (calculatorBeh == null)
            Debug.LogError(calculatorBeh);
    }
	
    private IEnumerator ReceiveMoneyFromCustomer() {
        currentGamePlayState = GamePlayState.receiveMoney;

        if (MainMenu._HasNewGameEvent)
            audioDescribe.PlayOnecSound(description_clips[6]);
        
		if (cash_obj == null)
        {
            cash_obj = Instantiate(Resources.Load("Money/Cash", typeof(GameObject))) as GameObject;
			cash_obj.transform.position = new Vector3(0, 0, -5);
			cash_sprite = cash_obj.GetComponent<tk2dSprite>();
			
            if(currentCustomer.amount < 20) {
				cash_sprite.spriteId = cash_sprite.GetSpriteIdByName("cash_20");
				currentCustomer.payMoney = 20;
            }
            else if(currentCustomer.amount < 50) {
				cash_sprite.spriteId = cash_sprite.GetSpriteIdByName("cash_50");
				currentCustomer.payMoney = 50;
            }
            else if(currentCustomer.amount <= 100) {
				cash_sprite.spriteId = cash_sprite.GetSpriteIdByName("cash_100");
				currentCustomer.payMoney = 100;
            }
        }
		
		yield return new WaitForSeconds(3);
		
		Destroy(cash_obj.gameObject);
		calculator_group_instance.SetActiveRecursively(true);
		this.DeActiveCalculationPriceGUI();

		this.ShowGiveTheChangeForm();
		currentGamePlayState = GamePlayState.giveTheChange;
    }

	void ShowGiveTheChangeForm ()
	{
        giveTheChangeGUIForm_groupObj.SetActiveRecursively(true);
		darkShadowPlane.active = true;
		
		audioEffect.PlayOnecSound(audioEffect.giveTheChange_clip);

        totalPrice_textmesh.text = currentCustomer.amount.ToString();
        totalPrice_textmesh.Commit();
        receiveMoney_textmesh.text = currentCustomer.payMoney.ToString();
        receiveMoney_textmesh.Commit();

        calculatorBeh.result_Textmesh = change_textmesh;
	}

    private void TradingComplete() {
        currentGamePlayState = GamePlayState.TradeComplete;

        foreach(var good in foodTrayBeh.goodsOnTray_List) {
            Destroy(good.gameObject);
        }

        foodTrayBeh.goodsOnTray_List.Clear();

        StartCoroutine(this.PackagingGoods());

        if(MainMenu._HasNewGameEvent) {
            MainMenu._HasNewGameEvent = false;
			Town.IntroduceGameUI_Event += Town.Handle_IntroduceGameUI_Event;
			
            Destroy(bakeryShopTutor.greeting_textmesh);
            bakeryShopTutor.goaway_button_obj.active = true;
            bakeryShopTutor = null;
            darkShadowPlane.transform.position += Vector3.forward * 2f;

            audioDescribe.PlayOnecSound(description_clips[7]);
        }
		else {
			int r = UE.Random.Range(0, thanksCustomer_clips.Length);
			audioDescribe.PlayOnecSound(thanksCustomer_clips[r]);
		}
    }

    private IEnumerator PackagingGoods()
    {
        if(packaging_Obj == null) {
            packaging_Obj = Instantiate(Resources.Load(ObjectsBeh.Packages_ResourcePath + "Packages_Sprite", typeof(GameObject))) as GameObject;
            packaging_Obj.transform.parent = foodsTray_obj.transform;
            packaging_Obj.transform.localPosition = new Vector3(0, .1f, -.1f);
        }
		
		TK_animationManager.RandomPlayGoodAnimation();

        yield return new WaitForSeconds(2);
        
        StartCoroutine(this.CreateGameEffect());
		audioEffect.PlayOnecSound(audioEffect.longBring_clip);
		this.CreateEarnTKCoin(currentCustomer.amount);        		
		TK_animationManager.RandomPlayGoodAnimation();

        billingAnimatedSprite.Play("Thanks");
        billingAnimatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
			billingAnimatedSprite.Play("Billing");
		};
        
        Mz_StorageManage.AvailableMoney += currentCustomer.amount;
        base.availableMoney.text = Mz_StorageManage.AvailableMoney.ToString();
        base.availableMoney.Commit();

		if(Mz_StorageManage._IsNoticeUser == false & Mz_StorageManage.AvailableMoney >= 350) {
			Mz_StorageManage._IsNoticeUser = true;

			this.CreateNoticeUpgradeShopEvent();
		}  

        //<!-- Clare resource data.
		Destroy(packaging_Obj);
		StartCoroutine(ExpelCustomer());
    }

    private IEnumerator CreateGameEffect()
    {
        gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, foodsTray_obj.transform);

        yield return 0;
    }
	
	public override void OnInput(string nameInput)
	{	
		if(MainMenu._HasNewGameEvent) {
			if(nameInput == "EN_001_textmesh") {
				StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[0]));
                bakeryShopTutor.currentTutorState = BakeryShopTutor.TutorStatus.AcceptOrders;

                return;
			}
		}

        //<!-- Close shop button.
		if(nameInput == close_button.name) {
			if(Application.isLoadingLevel == false && _onDestroyScene == false) {
				_onDestroyScene = true;
				
                base.extendsStorageManager.SaveDataToPermanentMemory();
                this.PreparingToCloseShop();		
				
				return;
			}
		}

        if (nameInput == "NoticeUpgradeButton")
        {
            if(Application.isLoadingLevel == false && _onDestroyScene == false) {
                _onDestroyScene = true;

                base.extendsStorageManager.SaveDataToPermanentMemory();
                this.PreparingToCloseShop();

                Mz_LoadingScreen.LoadSceneName = SceneNames.Sheepbank.ToString();
                Application.LoadLevel(SceneNames.LoadingScene.ToString());

                return;
            }
            else
                return;
        }
		
		if (calculator_group_instance.active) 
        {
			if(currentGamePlayState == GamePlayState.calculationPrice) {
				if(nameInput == "ok_button") {
					this.CallCheckAnswerOfTotalPrice();
					return;
				}
			}
			else if(currentGamePlayState == GamePlayState.giveTheChange) {
				if(nameInput == "ok_button") {
					this.CallCheckAnswerOfGiveTheChange();
					return;
				}
			}
			
			calculatorBeh.GetInput(nameInput);
		}

		if(currentGamePlayState == GamePlayState.GreetingCustomer) {
			switch (nameInput) {
			case TH_001: 				StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[0]));
				break;
			case TH_002:				StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[1]));
				break;
			case TH_003:				StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[2]));
				break;
			case TH_004:				StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[3]));
				break;
			case TH_005:				StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[4]));
				break;
			case TH_006:				StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[5]));
				break;
			case EN_001:				StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[0]));
				break;
			case EN_002:				StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[1]));
				break;
			case EN_003:				StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[2]));
				break;
			case EN_004:				StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[3]));
				break;
			case EN_005:				StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[4]));				
				break;
			case EN_006:				StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[5]));
				break;
			case EN_007:				StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[6]));
				break;
			default:
			break;
			}
		}
        else if (currentGamePlayState == GamePlayState.Ordering)
        {
            if (nameInput == GoodDataStore.FoodMenuList.Apple_juice.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Apple_juice]);
            else if (nameInput == GoodDataStore.FoodMenuList.Blueberry_cake.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Blueberry_cake]);
            else if (nameInput == GoodDataStore.FoodMenuList.Blueberry_cupcake.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Blueberry_cupcake]);
            else if (nameInput == GoodDataStore.FoodMenuList.Blueberry_minicake.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Blueberry_minicake]);
            else if (nameInput == GoodDataStore.FoodMenuList.Butter_cookie.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Butter_cookie]);
            else if (nameInput == GoodDataStore.FoodMenuList.Chocolate_cake.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Chocolate_cake]);
            else if (nameInput == GoodDataStore.FoodMenuList.Chocolate_cookie.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Chocolate_cookie]);
            else if (nameInput == GoodDataStore.FoodMenuList.Chocolate_cupcake.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Chocolate_cupcake]);
            else if (nameInput == GoodDataStore.FoodMenuList.Chocolate_icecream.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Chocolate_icecream]);
            else if (nameInput == GoodDataStore.FoodMenuList.Chocolate_minicake.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Chocolate_minicake]);
            else if (nameInput == GoodDataStore.FoodMenuList.Cocoa_milk.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Cocoa_milk]);
            else if (nameInput == GoodDataStore.FoodMenuList.DeepFriedChicken_sandwich.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.DeepFriedChicken_sandwich]);
            else if (nameInput == GoodDataStore.FoodMenuList.Egg_sandwich.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Egg_sandwich]);
            else if (nameInput == GoodDataStore.FoodMenuList.Freshmilk.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Freshmilk]);
            else if (nameInput == GoodDataStore.FoodMenuList.Fruit_cookie.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Fruit_cookie]);
            else if (nameInput == GoodDataStore.FoodMenuList.Ham_sandwich.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Ham_sandwich]);
            else if (nameInput == GoodDataStore.FoodMenuList.Hotdog.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Hotdog]);
            else if (nameInput == GoodDataStore.FoodMenuList.HotdogWithCheese.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.HotdogWithCheese]);
            else if (nameInput == GoodDataStore.FoodMenuList.Orange_juice.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Orange_juice]);
            else if (nameInput == GoodDataStore.FoodMenuList.Pineapple_juice.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Pineapple_juice]);
            else if (nameInput == GoodDataStore.FoodMenuList.Strawberry_cake.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Strawberry_cake]);
            else if (nameInput == GoodDataStore.FoodMenuList.Strawberry_cupcake.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Strawberry_cupcake]);
            else if (nameInput == GoodDataStore.FoodMenuList.Strawberry_icecream.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Strawberry_icecream]);
            else if (nameInput == GoodDataStore.FoodMenuList.Strawberry_minicake.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Strawberry_minicake]);
            else if (nameInput == GoodDataStore.FoodMenuList.ToastWithBlueberryJam.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.ToastWithBlueberryJam]);
            else if (nameInput == GoodDataStore.FoodMenuList.ToastWithButterJam.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.ToastWithButterJam]);
            else if (nameInput == GoodDataStore.FoodMenuList.ToastWithCustardJam.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.ToastWithCustardJam]);
            else if (nameInput == GoodDataStore.FoodMenuList.ToastWithStrawberryJam.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.ToastWithStrawberryJam]);
            else if (nameInput == GoodDataStore.FoodMenuList.Tuna_sandwich.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Tuna_sandwich]);
            else if (nameInput == GoodDataStore.FoodMenuList.Vanilla_icecream.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Vanilla_icecream]);
            else
            {
                switch (nameInput)
                {
                    case "OK_button":
                        StartCoroutine(this.CollapseOrderingGUI());
                        break;
                    case "Goaway_button":
                        currentCustomer.PlayRampage_animation();
                        StartCoroutine(this.ExpelCustomer());
                        break;
                    case "OrderingIcon": StartCoroutine(this.ShowOrderingGUI());
                        break;
                    case "Billing_machine":
                        if (MainMenu._HasNewGameEvent) {
                            base.SetActivateTotorObject(false);
                        }
                        audioEffect.PlayOnecSound(audioEffect.calc_clip);
                        billingMachine.animation.Play(billingMachine_animState.name);
                        StartCoroutine(this.CheckingUNITYAnimationComplete(billingMachine.animation, billingMachine_animState.name));
                        break;
                    default:
                        break;
                }
            }
        }
	}

    private void CallCheckAnswerOfTotalPrice() {
        if(currentCustomer.amount == calculatorBeh.GetDisplayResultTextToInt()) {
			audioEffect.PlayOnecSound(audioEffect.correct_Clip);
		
			calculatorBeh.ClearCalcMechanism();
            calculator_group_instance.SetActiveRecursively(false);
            receiptGUIForm_groupObj.SetActiveRecursively(false);
            darkShadowPlane.active = false;

            StartCoroutine(this.ReceiveMoneyFromCustomer());
        }
        else {
			audioEffect.PlayOnecSound(audioEffect.wrong_Clip);
			
			int r = UE.Random.Range(2, 5);
			audioDescribe.PlayOnecSound(apologize_clip[r]);
			
            calculatorBeh.ClearCalcMechanism();
			
            Debug.LogWarning("Wrong answer !. Please recalculate");
        }
    }	
	
	private void CallCheckAnswerOfGiveTheChange() {
		int correct_TheChange = currentCustomer.payMoney - currentCustomer.amount;
		if(correct_TheChange == calculatorBeh.GetDisplayResultTextToInt()) {
			calculatorBeh.ClearCalcMechanism();
			calculator_group_instance.SetActiveRecursively(false);
            giveTheChangeGUIForm_groupObj.SetActiveRecursively(false);
            darkShadowPlane.active = false;
			
			audioEffect.PlayOnecWithOutStop(audioEffect.correct_Clip);
			
			Debug.Log("give the change :: correct");

            this.TradingComplete();
		}
        else {			
			audioEffect.PlayOnecWithOutStop(audioEffect.wrong_Clip);
			calculatorBeh.ClearCalcMechanism();
            currentCustomer.PlayRampage_animation();
			
			int r = UE.Random.Range(2, 5);
			audioDescribe.PlayOnecSound(apologize_clip[r]);
			
            Debug.Log("Wrong answer !. Please recalculate");
        }
	}

    private IEnumerator CheckingUNITYAnimationComplete(Animation targetAnimation, string targetAnimatedName)
    {
        do
        {
            yield return null;
        } while (targetAnimation.IsPlaying(targetAnimatedName));

		Debug.LogWarning(targetAnimatedName + " finish !");

		this.CheckingGoodsObjInTray(string.Empty);
    }

    internal void CheckingGoodsObjInTray(string callFrom)
    {
		Debug.Log("CheckingGoodsObjInTray");
		
        if (callFrom == GoodsBeh.ClassName) {
			// Check correctly of goods with arr_orderingItems.
			// and change color of arr_orderingBaseItems.
			foreach (tk2dSprite item in arr_orderingItems) 
				item.transform.parent.GetComponent<tk2dSprite>().spriteId = arr_orderingBaseItems[0].GetSpriteIdByName(BASE_ORDER_ITEM_NORMAL);
			if(foodTrayBeh.goodsOnTray_List.Count == 0) {
				return;
			}
			
            List<CustomerOrderRequire> list_goodsTemp = new List<CustomerOrderRequire>();
            Food temp_goods = null;

            for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++)
            {
                foreach (GoodsBeh item in this.foodTrayBeh.goodsOnTray_List)
                {
                    if (item.name == currentCustomer.customerOrderRequire[i].food.name)
                    {
                        temp_goods = currentCustomer.customerOrderRequire[i].food;
                    }
                }
				
				if(temp_goods != null) {
	                list_goodsTemp.Add(new CustomerOrderRequire() { food = temp_goods, });
					
					/// Check correctly of goods with arr_orderingItems.
					/// and change color of arr_orderingBaseItems.
					foreach (tk2dSprite item in arr_orderingItems) {		
						if(list_goodsTemp[i] != null) {
							if(item.gameObject.name == list_goodsTemp[i].food.name)
								item.transform.parent.GetComponent<tk2dSprite>().spriteId = arr_orderingBaseItems[0].GetSpriteIdByName(BASE_ORDER_ITEM_COMPLETE);
						}
					};
					
					if (list_goodsTemp.Count == currentCustomer.customerOrderRequire.Count)
						this.billingMachine.animation.Play();
	
	                temp_goods = null;
				}
				else {
					list_goodsTemp.Add(new CustomerOrderRequire() { food = null });
					audioEffect.PlayOnecWithOutStop(audioEffect.wrong_Clip);
				}
            }
        }
        else if (callFrom == "newgame_event") {
            // Check correctly of goods with arr_orderingItems.
            // and change color of arr_orderingBaseItems.
            foreach (tk2dSprite item in arr_orderingItems)
                item.transform.parent.GetComponent<tk2dSprite>().spriteId = arr_orderingBaseItems[0].GetSpriteIdByName(BASE_ORDER_ITEM_NORMAL);
            if (foodTrayBeh.goodsOnTray_List.Count == 0)
            {
                Debug.Log("food on tray is empty.");
                return;
            }
            else if (this.foodTrayBeh.goodsOnTray_List.Count != currentCustomer.customerOrderRequire.Count)
            {
                Debug.Log("food on tray != customer require.");
                return;
            }
            else {
                List<CustomerOrderRequire> list_goodsTemp = new List<CustomerOrderRequire>();
                Food temp_goods = null;

                for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++)
                {
                    foreach (GoodsBeh item in this.foodTrayBeh.goodsOnTray_List)
                    {
                        if (item.name == currentCustomer.customerOrderRequire[i].food.name)
                        {
                            temp_goods = currentCustomer.customerOrderRequire[i].food;
                        }
                    }

                    if (temp_goods != null)
                    {
                        list_goodsTemp.Add(new CustomerOrderRequire() { food = temp_goods, });

                        /// Check correctly of goods with arr_orderingItems.
                        /// and change color of arr_orderingBaseItems.
                        foreach (tk2dSprite item in arr_orderingItems)
                        {
                            if (list_goodsTemp[i] != null)
                            {
                                if (item.gameObject.name == list_goodsTemp[i].food.name)
                                    item.transform.parent.GetComponent<tk2dSprite>().spriteId = arr_orderingBaseItems[0].GetSpriteIdByName(BASE_ORDER_ITEM_COMPLETE);
                            }
                        };

                        if (list_goodsTemp.Count == currentCustomer.customerOrderRequire.Count)
                        {
                            bakeryShopTutor.currentTutorState = BakeryShopTutor.TutorStatus.CheckAccuracy;
                            this.billingMachine.animation.Play();
                            StartCoroutine(this.ShowOrderingGUI());
                        }

                        temp_goods = null;
                    }
                    else
                    {
                        list_goodsTemp.Add(new CustomerOrderRequire() { food = null });
                    }
                }
            }
        }
        else if (callFrom == string.Empty) {
            if (this.foodTrayBeh.goodsOnTray_List.Count == 0)
            {
                Debug.Log("food on tray is empty.");

                StartCoroutine(this.PlayApologizeCustomer(apologize_clip[0]));
            }
            else if (this.foodTrayBeh.goodsOnTray_List.Count != currentCustomer.customerOrderRequire.Count)
            {
                Debug.Log("food on tray != customer require.");

                StartCoroutine(this.PlayApologizeCustomer(apologize_clip[1]));
            }
            else
            {
                List<CustomerOrderRequire> list_goodsTemp = new List<CustomerOrderRequire>();
                Food temp_goods = null;

                for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++)
                {
                    foreach (GoodsBeh item in this.foodTrayBeh.goodsOnTray_List)
                    {
                        if (item.name == currentCustomer.customerOrderRequire[i].food.name)
                        {
                            temp_goods = currentCustomer.customerOrderRequire[i].food;
                        }
                    }

                    if (temp_goods != null)
                    {
                        list_goodsTemp.Add(new CustomerOrderRequire() { food = temp_goods, });

                        Debug.Log(list_goodsTemp[i].food.name);

                        if (list_goodsTemp.Count == currentCustomer.customerOrderRequire.Count)
                            OnManageGoodComplete(EventArgs.Empty);

                        temp_goods = null;
                    }
                    else
                    {
                        list_goodsTemp.Add(new CustomerOrderRequire() { food = null });
                        StartCoroutine(this.PlayApologizeCustomer(apologize_clip[1]));
                        return;
                    }
                }
            }
        }
    }
    
    private void PreparingToCloseShop()
    {
        this.OnDispose();

        iTween.MoveTo(rollingDoor_Obj, iTween.Hash("position", new Vector3(0, 0, 1), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutSine,
            "oncomplete", "RollingDoor_close", "oncompletetarget", this.gameObject));
		audioEffect.PlayOnecWithOutStop(base.soundEffect_clips[0]);
        rollingDoor_Obj.SetActiveRecursively(true);
    }

    private void RollingDoor_close() {
        Mz_LoadingScreen.LoadSceneName = SceneNames.Town.ToString();
        Application.LoadLevel(SceneNames.LoadingScene.ToString());	
    }

    public override void OnDispose()
    {
        base.OnDispose();
		
        CakeBeh._IsActive = false;
		ToastBeh._IsActive = false;
		HotdogBeh._IsActive = false;
        
        Destroy(cupcake.gameObject);
        Destroy(miniCake.gameObject);
        Destroy(cake.gameObject);

        Destroy(toasts[0].gameObject);
        Destroy(toasts[1].gameObject);
        
        Destroy(blueberry_cream_Instance.gameObject);
        Destroy(chocolate_cream_Instance.gameObject);
        Destroy(strawberry_cream_Instance.gameObject);

        Destroy(blueberryJam_instance.gameObject);
        Destroy(custardJam_instance);
        Destroy(freshButterJam_instance);
        Destroy(strawberryJam_instance);

        Destroy(hotdog.gameObject);
	}

	internal void CreateDeductionsCoin (int p_value)
	{
		GameObject tk_coin = Instantiate (Resources.Load ("Money/Coin", typeof(GameObject))) as GameObject;
		tk_coin.transform.parent = binBeh.transform;
		tk_coin.transform.localPosition = Vector3.up * 0.4f;
		Transform animatedCoin = tk_coin.transform.Find("TK_Coin");
		Transform value_transform = animatedCoin.transform.Find ("TextMesh");
		tk2dTextMesh value_textmesh = value_transform.GetComponent<tk2dTextMesh>();
		value_textmesh.text = "-" + p_value.ToString();
		value_textmesh.Commit ();
		
		animatedCoin.animation.Play ();
		StartCoroutine_Auto(CheckingUnityAnimationComplete.ICheckAnimationComplete (animatedCoin.animation, "CoinAnim", null, string.Empty));
		CheckingUnityAnimationComplete.TargetAnimationComplete_event += (object sender, EventArgs e) => { 
			Destroy(tk_coin);
		};
	}
	
	private void CreateEarnTKCoin(int p_value)
	{
		GameObject tk_coin = Instantiate (Resources.Load ("Money/Coin", typeof(GameObject))) as GameObject;
		tk_coin.transform.position = new Vector3(.8f, .5f, -3);
		Transform animatedCoin = tk_coin.transform.Find("TK_Coin");
		Transform value_transform = animatedCoin.transform.Find ("TextMesh");
		tk2dTextMesh value_textmesh = value_transform.GetComponent<tk2dTextMesh>();
		value_textmesh.text = "+" + p_value.ToString();
		value_textmesh.Commit ();
		
		animatedCoin.animation.Play ();
		StartCoroutine_Auto(CheckingUnityAnimationComplete.ICheckAnimationComplete (animatedCoin.animation, "CoinAnim", null, string.Empty));
		CheckingUnityAnimationComplete.TargetAnimationComplete_event += (object sender, EventArgs e) => { 
			Destroy(tk_coin);
		};
	}
	
	protected override void OnApplicationPause (bool pauseStatus)
	{
		base.OnApplicationPause (pauseStatus);
	}
}
