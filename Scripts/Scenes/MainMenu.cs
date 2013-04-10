using UnityEngine;
using System;
using System.Collections;

public class MainMenu : Mz_BaseScene {

    public GameObject baseBuilding_Obj;
    public GameObject movingCloud_Objs;
    public GameObject flyingBird_group;
    public CharacterAnimationManager characterAnimationManager;
    public Transform mainmenu_Group;
    public Transform newgame_Group;
    public Transform initializeNewGame_Group;
	public Transform options_group_transform;
	private InitializeNewShop initializeNewShop;

	public GUIOptionsManager optionsManager = new GUIOptionsManager();	
	private void SetActivateGUIOptionsGroup (bool activeState)
	{
        if(activeState) {
			plane_darkShadow.active = true;
            iTween.MoveTo(optionsManager.selectLanguage_Obj, iTween.Hash("y", 0.1f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutBounce));
        }
        else {
			plane_darkShadow.active = false;
            iTween.MoveTo(optionsManager.selectLanguage_Obj, iTween.Hash("y", 2f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutBounce));
        }
	}

	public TK_news tknewsManager;
	private void SetActivateTKNews(bool activeState) {
		if(activeState) {
			plane_darkShadow.active = true;
            iTween.MoveTo(tknewsManager.gameObject, iTween.Hash("y", 0f, "islocal", true, "time", 1f, "oncomplete", "ShakeFacebookButton", "oncompletetarget", tknewsManager.gameObject, "easetype", iTween.EaseType.easeOutBounce));
		}
		else {
			plane_darkShadow.active = false;
            tknewsManager.StopShakeFacebookButton();
			iTween.MoveTo(tknewsManager.gameObject, iTween.Hash("y", 2f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutBounce));
		}
	}


    public Transform loadgame_Group;
    public GameObject back_button;
    private GameObject OK_button_Obj;
    const string CREATE_NEWSHOP_BUTTON_NAME = "CreateNewShop_button";
    const string LOADSHOP_BUTTON_NAME = "LoadShop_button";

 
    private Hashtable moveDownTransform_Data = new Hashtable();
    private Hashtable moveUpTransform_Data = new Hashtable();
    
    public GUISkin mainmenu_Skin;
    public GUIStyle saveSlot_buttonStyle;
    private GUIStyle notification_TextboxStyle;
	public enum SceneState { 
		none = 0, 
        //showOption, 
        //ShowTKNews, 
		showNewGame, 
		showNewShop, 
		showLoadGame, 
	};
    private SceneState currentSceneState;

    private string username = string.Empty;
	private string shopName = string.Empty;
	
#if UNITY_IOS || UNITY_ANDROID
	private TouchScreenKeyboard touchScreenKeyboard;
#endif
    
	private bool _isNullUsernameNotification = false;
    private bool _isDuplicateUsername = false;
    private bool _isFullSaveGameSlot;
    private string player_1;
    private string player_2;
    private string player_3;
	public bool _showSkinLayout;
	Rect newgame_Textfield_rect;
	Rect newShopName_rect;
    Rect notification_Rect;
	float group_width = 400;
	Rect showSaveGameSlot_GroupRect;
	Rect slot_1Rect;
	Rect slot_2Rect;
	Rect slot_3Rect;


	internal static bool _HasNewGameEvent = false;
		
	// Use this for initialization
	void Start () {
		this.InitailizeDataFields();
		StartCoroutine(ReInitializeAudioClipData());
        StartCoroutine(PreparingAudio());
		this.PlayWelcomeEvent();

        Mz_ResizeScale.ResizingScale(baseBuilding_Obj.transform);

        iTween.MoveTo(mainmenu_Group.gameObject, moveDownTransform_Data);
		iTween.MoveTo(options_group_transform.gameObject, iTween.Hash("y", 0.85f, "time", 1f, "easetype",  iTween.EaseType.easeOutSine));

        newgame_Group.gameObject.SetActiveRecursively(false);
		initializeNewShop = initializeNewGame_Group.GetComponent<InitializeNewShop>();
        initializeNewGame_Group.gameObject.SetActiveRecursively(false);
        loadgame_Group.gameObject.SetActiveRecursively(false);
        back_button.gameObject.active = false;
		
        iTween.MoveTo(flyingBird_group, iTween.Hash("x", 1.8f, "time", 16f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.loop));
        //iTween.MoveTo(cloudAndFog_Objs[0].gameObject, iTween.Hash("y", 0f, "time", 3f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong)); 
        //iTween.MoveTo(cloudAndFog_Objs[1].gameObject, iTween.Hash("y", 0.2f, "time", 3.5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong)); 
        //iTween.MoveTo(cloudAndFog_Objs[2].gameObject, iTween.Hash("y", 0.5f, "time", 4f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong)); 
		iTween.MoveTo(movingCloud_Objs, iTween.Hash("x", -1f, "time", 16f, "easetype", iTween.EaseType.easeInOutSine, "looptype", iTween.LoopType.pingPong)); 

		//<@-- Add new game eventhandle. 
		_HasNewGameEvent = false;
	}
	
	protected IEnumerator PreparingAudio ()
	{
		base.CreateAudioObject ();
		
        audioBackground_Obj.audio.clip = base.background_clip;
        audioBackground_Obj.audio.loop = true;
        audioBackground_Obj.audio.Play();
		
		yield return 0;
	}

    private const string PATH_OF_DYNAMIC_CLIP = "AudioClips/GameIntroduce/Mainmenu/";
    private IEnumerator ReInitializeAudioClipData()
    {
		description_clips.Clear();
		if(Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.TH) {
        	description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_game_intro", typeof(AudioClip)) as AudioClip);
        	description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_create_newgame", typeof(AudioClip)) as AudioClip);
        	description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_create_newshop", typeof(AudioClip)) as AudioClip);
		}
		else if(Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.EN) {
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_game_intro", typeof(AudioClip)) as AudioClip);
        	description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_create_newgame", typeof(AudioClip)) as AudioClip);
        	description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_create_newshop", typeof(AudioClip)) as AudioClip);
		}		
		
        yield return 0;
    }
	
    void InitailizeDataFields()
    {
		moveDownTransform_Data.Add("position", new Vector3(0.2f, 0f, -2));
		moveDownTransform_Data.Add("time", 1f);
		moveDownTransform_Data.Add("easetype", iTween.EaseType.spring);
		
		moveUpTransform_Data.Add("position", new Vector3(0.2f, 2, -2));
		moveUpTransform_Data.Add("time", 1f);
		moveUpTransform_Data.Add("easetype", iTween.EaseType.linear);

        newgame_Textfield_rect = new Rect((Mz_OnGUIManager.viewPort_rect.width / 2) - (70 * Mz_OnGUIManager.Extend_heightScale), Mz_OnGUIManager.viewPort_rect.height / 2 + 58, 300 * Mz_OnGUIManager.Extend_heightScale, 82);
        newShopName_rect = new Rect((Mz_OnGUIManager.viewPort_rect.width / 2) - (63 * Mz_OnGUIManager.Extend_heightScale), Mz_OnGUIManager.viewPort_rect.height / 2 - 110, 400 * Mz_OnGUIManager.Extend_heightScale, 80);
        notification_Rect = new Rect(Mz_OnGUIManager.viewPort_rect.width / 2 - (300 * Mz_OnGUIManager.Extend_heightScale), 0, 600 * Mz_OnGUIManager.Extend_heightScale, 64);
        group_width = 400 * Mz_OnGUIManager.Extend_heightScale;
        showSaveGameSlot_GroupRect = new Rect((Mz_OnGUIManager.viewPort_rect.width / 2) + (75 * Mz_OnGUIManager.Extend_heightScale) - (group_width / 2), (Main.GAMEHEIGHT / 2) - 70, group_width, 300);
        slot_1Rect = new Rect(32*Mz_OnGUIManager.Extend_heightScale, 12, group_width - (60 * Mz_OnGUIManager.Extend_heightScale), 80);
		slot_2Rect = new Rect(slot_1Rect.x, 112, group_width - (60 * Mz_OnGUIManager.Extend_heightScale), 80);
		slot_3Rect = new Rect(slot_1Rect.x, 212, group_width - (60 * Mz_OnGUIManager.Extend_heightScale), 80);

        saveSlot_buttonStyle.normal.textColor = Color.white;
        saveSlot_buttonStyle.active.textColor = Color.green;
        notification_TextboxStyle = new GUIStyle(mainmenu_Skin.textArea);
        notification_TextboxStyle.fontSize = 16;
        notification_TextboxStyle.fontStyle = FontStyle.Normal;
        
		player_1 = PlayerPrefs.GetString(1 + Mz_StorageManage.KEY_USERNAME);
		player_2 = PlayerPrefs.GetString(2 + Mz_StorageManage.KEY_USERNAME);
		player_3 = PlayerPrefs.GetString(3 + Mz_StorageManage.KEY_USERNAME);
    }

	private void PlayWelcomeEvent ()
	{		
		audioDescribe.PlayOnecSound(description_clips[0]);
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();

        if (username == "MzReset")
        {
            PlayerPrefs.DeleteAll();
            username = string.Empty;
            Mz_StorageManage.SaveSlot = 0;
        }
        
        if(OK_button_Obj != null && OK_button_Obj.active)
            iTween.ScaleTo(OK_button_Obj, iTween.Hash("scale", new Vector3(1.2f, 1.2f, 1f), "time", .6f, "looptype", iTween.LoopType.pingPong));
	}
	
	protected override void OnGUI() {
        base.OnGUI();

		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));
		
		GUI.BeginGroup(Mz_OnGUIManager.viewPort_rect);
        {
            if(_showSkinLayout) {
				GUI.Box(new Rect(0, 0, Mz_OnGUIManager.viewPort_rect.width, Mz_OnGUIManager.viewPort_rect.height), "Skin layout", GUI.skin.box);
            }

            if(currentSceneState == SceneState.showNewGame) {
                this.DrawNewGameTextField();                
            }
            else if(currentSceneState == SceneState.showNewShop) {
                this.DrawNewShopGUI();
            }
            else if(currentSceneState == SceneState.showLoadGame) {                
                // Call ShowSaveGameSlot Method.
                this.ShowSaveGameSlot(_isFullSaveGameSlot);
            }
            else if(currentSceneState == SceneState.none) {
                _isDuplicateUsername = false;
                _isNullUsernameNotification = false;
                _isFullSaveGameSlot = false;
                username = "";
                shopName = "";
            }

		    string notificationText = "";
		    string dublicateNoticeText = "";

            //notificationText = "Please Fill Your Username. \n ��س������ͼ�����";
            //dublicateNoticeText = "This name already exists. \n ���͹������������";

			notificationText = "Please Fill Your Username.";
			dublicateNoticeText = "This name already exists.";

            if (_isNullUsernameNotification)           
                GUI.Box(notification_Rect, notificationText, notification_TextboxStyle);
            if (_isDuplicateUsername)
                GUI.Box(notification_Rect, dublicateNoticeText, notification_TextboxStyle);
        }
        GUI.EndGroup();
    }
    
    private void DrawNewShopGUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
		//<!-- "Please Insert Shopname !".
		shopName = GUI.TextField(newShopName_rect, shopName, 13, saveSlot_buttonStyle);
#elif UNITY_IOS || UNITY_ANDROID
			if(GUI.Button(newShopName_rect, shopName, saveSlot_buttonStyle)) {
				touchScreenKeyboard = TouchScreenKeyboard.Open(shopName, TouchScreenKeyboardType.ASCIICapable, false, false, false, true);
			}
			if(touchScreenKeyboard != null && touchScreenKeyboard.active)
				shopName = touchScreenKeyboard.text;
			if(touchScreenKeyboard != null && touchScreenKeyboard.active == false)
				touchScreenKeyboard.text = string.Empty;
#endif
    }

    private void DrawNewGameTextField()
    {
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
        {
            audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);
                    
            this.CheckUserNameFormInput();
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
            //<!-- "Please Insert Username !".
            GUI.SetNextControlName("Username");
            username = GUI.TextField(newgame_Textfield_rect, username, 13, saveSlot_buttonStyle);

            if (GUI.GetNameOfFocusedControl() == string.Empty || GUI.GetNameOfFocusedControl() == "")
            {
                GUI.FocusControl("Username");
            }
#elif UNITY_IPHONE || UNITY_ANDROID
			if(GUI.Button(newgame_Textfield_rect, username, saveSlot_buttonStyle)) {
				touchScreenKeyboard = TouchScreenKeyboard.Open(username, TouchScreenKeyboardType.ASCIICapable, false, false, false, true);
			}
			if(touchScreenKeyboard != null && touchScreenKeyboard.active)
				username = touchScreenKeyboard.text;
			if(touchScreenKeyboard != null && touchScreenKeyboard.active == false)
				touchScreenKeyboard.text = string.Empty;
#endif
	}

    private void CheckUserNameFormInput()
    {
        username.Trim('\n');

        if (username == "" || username == string.Empty || username == null) {
            Debug.LogWarning("Username == null");
	        _isNullUsernameNotification = true;
            _isDuplicateUsername = false;

            this.characterAnimationManager.PlayEyeAnimation(CharacterAnimationManager.NameAnimationsList.agape);
            audioEffect.PlayOnecWithOutStop(audioEffect.wrong_Clip);
	    }
        else if (username == player_1 || username == player_2 || username == player_3) {
            Debug.LogWarning("Duplicate Username");
	        _isDuplicateUsername = true;
            _isNullUsernameNotification = false;
            username = string.Empty;

            this.characterAnimationManager.PlayEyeAnimation(CharacterAnimationManager.NameAnimationsList.agape);
            audioEffect.PlayOnecWithOutStop(audioEffect.wrong_Clip);
	    }
        else
        {
            _isDuplicateUsername = false;
            _isNullUsernameNotification = false;

            this.characterAnimationManager.PlayGoodAnimation();
            audioEffect.PlayOnecWithOutStop(audioEffect.correct_Clip);

            this.EnterUsername();
	    }
    }
    //<!-- Enter Username from User. 
    void EnterUsername()
    {
        Debug.Log("EnterUsername");

        //<!-- Autosave Mechanicism. When have empty game slot.  
        if (player_1 == string.Empty) {
            Mz_StorageManage.SaveSlot = 1;
            //this.SaveNewPlayer();
            StartCoroutine(ShowInitializeNewShop());
        }
        else if (player_2 == string.Empty) {
			Mz_StorageManage.SaveSlot = 2;
            //this.SaveNewPlayer();
            StartCoroutine(ShowInitializeNewShop());
        }
        else if (player_3 == string.Empty) {
			Mz_StorageManage.SaveSlot = 3;
            //this.SaveNewPlayer();
            StartCoroutine(ShowInitializeNewShop());
        }
        else {
			Mz_StorageManage.SaveSlot = 0;
            _isFullSaveGameSlot = true;
            StartCoroutine(ShowLoadShop());

            Debug.Log("<!-- Full Slot Call Showsavegameslot method.");
        }
    }

    // Save default game data for new player.
    private void SaveNewPlayer()
    {
		PlayerPrefs.SetString(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_USERNAME, this.username);
		PlayerPrefs.SetString(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_SHOP_NAME, this.shopName);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_MONEY, 1500);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_ACCOUNTBALANCE, 0);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot +  Mz_StorageManage.KEY_SHOP_LOGO, initializeNewShop.currentLogoID);
		PlayerPrefs.SetString(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_SHOP_LOGO_COLOR , initializeNewShop.currentLogoColor);

        int[] IdOfCanSellItem = new int[] { 0, 5, 9, 18 };
        PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CANSELLGOODSLIST, IdOfCanSellItem);
		string[] availableCreams = new string[] { CreamBeh.ChocolateCream, string.Empty, string.Empty, };
		PlayerPrefsX.SetStringArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_AVAILABLE_CREAM, availableCreams);

		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_ROOF_ID, 255);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_AWNING_ID, 255);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_TABLE_ID, 255);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_ACCESSORY_ID, 255);
		
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_TK_CLOTHE_ID, 255);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_TK_HAT_ID, 255);

        // <@-- Initailizing pet data.
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_PET_ID, 0);
        int[] defaultPetAliment_id = new int[] { 0 };
        PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + ExtendsStorageManager.KEY_CAN_ALIMENT_PET_LIST, defaultPetAliment_id);

        //@!-- Donation data.
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CONSERVATION_ANIMAL_LV, 0);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_AIDSFOUNDATION_LV, 0);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_LOVEDOGFOUNDATION_LV, 0);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_LOVEKIDFOUNDATION_LV, 0);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_ECOFOUNDATION_LV, 0);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_GLOBALWARMING_LV, 0);

        //<@-- CAN_EQUIP_CLOTHE_LIST
        int[] newPlayerClothes = new int[] { 0, 1, 2 };
        PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_EQUIP_CLOTHE_LIST, newPlayerClothes);
        //<@-- Can equip hat-list.
        int[] defaultHat_id = new int[] { 0, 1, 2 };
        PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_EQUIP_HAT_LIST, defaultHat_id);

		//<@-- Can decoration shop outside.
		int[] roof_temp_arr = new int[1] { 255 };
		PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_DECORATE_ROOF_LIST, roof_temp_arr); 
		int[] awning_temp_array = new int[1] { 255};
		PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_DECORATE_AWNING_LIST, awning_temp_array);
		int[] table_temp_array = new int[1] {255};
		PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_DECORATE_TABLE_LIST, table_temp_array);
		int[] accessories_temp_array = new int[1] {255};
        PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_DECORATE_ACCESSORIES_LIST, accessories_temp_array);

        //<!-- Notice user to upgrade them shop.
        PlayerPrefsX.SetBool(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_NOTICE_USER_TO_UPGRADE, false);
		//<!-- Force User to play tutor before.
		PlayerPrefsX.SetBool(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_IS_USER_PLAY_TUTOR, true);

        Debug.Log("Store new player data complete.");
    }

    private void LoadSceneTarget() {
        if(Application.isLoadingLevel == false) {
			Town.newGameStartup_Event += Town.Handle_NewGameStartupEvent;

			Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.Town.ToString();
			Application.LoadLevel(Mz_BaseScene.SceneNames.LoadingScene.ToString());					
		}
    }

    //<!-- Show save game slot. If slot is full.
    void ShowSaveGameSlot(bool _toSaveGame)
    {
        if (_toSaveGame) 
		{   
            //<!-- Full save game slot. Show notice message.
            string message = string.Empty;			
            //message = "���͡��ͧ����ͧ��� ����ź��������� ��зѺ���¢���������";
            message = "Select Data Slot To Replace New Data";
			GUI.Box(notification_Rect, message, notification_TextboxStyle);
		}

        GUI.BeginGroup(showSaveGameSlot_GroupRect);
        {
            if (_toSaveGame)			
            {
                // Display To Save Username.
//                GUI.Box(textbox_header_rect, username, mainmenu_Skin.textField);
                // Choose SaveGame Slot for replace new data.
                if (GUI.Button(slot_1Rect, new GUIContent(PlayerPrefs.GetString(1 + Mz_StorageManage.KEY_USERNAME), "button"), saveSlot_buttonStyle))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                    Mz_StorageManage.SaveSlot = 1;
//                    SaveNewPlayer();
					StartCoroutine(ShowInitializeNewShop());
                }
                else if (GUI.Button(slot_2Rect, new GUIContent(PlayerPrefs.GetString(2 + Mz_StorageManage.KEY_USERNAME), "button"), saveSlot_buttonStyle))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                    Mz_StorageManage.SaveSlot = 2;
//				    SaveNewPlayer();
					StartCoroutine(ShowInitializeNewShop());
                }
                else if (GUI.Button(slot_3Rect, new GUIContent(PlayerPrefs.GetString(3 + Mz_StorageManage.KEY_USERNAME), "button"), saveSlot_buttonStyle))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                    Mz_StorageManage.SaveSlot = 3;
                    //  SaveNewPlayer();
					StartCoroutine(ShowInitializeNewShop());
                }
            }
            else {
//                string headerText = "";
//                headerText = "���͡��ͧ����ͧ���������������¤�Ѻ";
//				headerText = "Select Data Slot";
//                GUI.Box(textbox_header_rect, headerText, mainmenu_Skin.textField);
                /// Choose SaveGame Slot for Load Save Data.
                string slot_1 = string.Empty;
                string slot_2 = string.Empty;
                string slot_3 = string.Empty;

                if (player_1 == string.Empty) slot_1 = "Empty";
                else slot_1 = player_1;
                if (player_2 == string.Empty) slot_2 = "Empty";
                else slot_2 = player_2;
                if (player_3 == string.Empty) slot_3 = "Empty";
                else slot_3 = player_3;

                #region <!-- GUI data slot button.

                if (GUI.Button(slot_1Rect, new GUIContent(slot_1, "button"), saveSlot_buttonStyle)) // saveSlot_buttonStyle))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                    if(player_1 != string.Empty) {
                        Mz_StorageManage.SaveSlot = 1;
                        base.extendsStorageManager.LoadSaveDataToGameStorage();
                        this.LoadSceneTarget();
                    }
                }
                else if (GUI.Button(slot_2Rect, new GUIContent(slot_2, "button"), saveSlot_buttonStyle))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                    if(player_2 != string.Empty) {
                        Mz_StorageManage.SaveSlot =2;
                        base.extendsStorageManager.LoadSaveDataToGameStorage();
                        this.LoadSceneTarget();
                    }
                }
                else if (GUI.Button(slot_3Rect, new GUIContent(slot_3, "button"), saveSlot_buttonStyle))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                    if(player_3 != string.Empty) {
                        Mz_StorageManage.SaveSlot = 3;
                        base.extendsStorageManager.LoadSaveDataToGameStorage();
                        this.LoadSceneTarget();
                    }
                }

                #endregion
            } 
        }
        GUI.EndGroup();
    }

    public override void OnInput(string nameInput)
    {
        base.OnInput(nameInput);

        switch (nameInput)
        {
            case "Audio":
                base.MuteAudioConfiguration();
                if (Mz_BaseScene.ToggleAudioActive)
                    optionsManager.audio_ui_sprite.spriteId = GUIOptionsManager.AUDIO_ON_ID;
                else
                    optionsManager.audio_ui_sprite.spriteId = GUIOptionsManager.AUDIO_OFF_ID;
                break;
            case "Flag_UI":
                this.SetActivateGUIOptionsGroup(true);
                break;
            case "EN":
                this.audioDescribe.PlayOnecWithOutStop(optionsManager.english_clip);
                Main.Mz_AppLanguage.appLanguage = Main.Mz_AppLanguage.SupportLanguage.EN;
                Mz_StorageManage.Language_id = (int)Main.Mz_AppLanguage.SupportLanguage.EN;
                PlayerPrefs.SetInt(Mz_StorageManage.KEY_SYSTEM_LANGUAGE, Mz_StorageManage.Language_id);
                StartCoroutine(this.ReInitializeAudioClipData());
                this.SetActivateGUIOptionsGroup(false);
                break;
            case "TH":
                this.audioDescribe.PlayOnecWithOutStop(optionsManager.thai_clip);
                Main.Mz_AppLanguage.appLanguage = Main.Mz_AppLanguage.SupportLanguage.TH;
                Mz_StorageManage.Language_id = (int)Main.Mz_AppLanguage.SupportLanguage.TH;
                PlayerPrefs.SetInt(Mz_StorageManage.KEY_SYSTEM_LANGUAGE, Mz_StorageManage.Language_id);
                StartCoroutine(this.ReInitializeAudioClipData());
                this.SetActivateGUIOptionsGroup(false);
                break;
            case "TKNews_button":
                this.SetActivateTKNews(true);
                break;
            case "Close_button":
                this.SetActivateTKNews(false);
                break;
            case "FacebookLike_button":
                Application.OpenURL("https://www.facebook.com/Taokaenoi.game");
                break;
		case "Bakery Shop" :
			Application.OpenURL("http://itunes.apple.com/WebObjects/MZStore.woa/wa/viewSoftware?id=626645567&mt=8");
			break;
            case "Up_button":
                tknewsManager.MoveUpPage();
                break;
            case "Down_button":
                tknewsManager.MoveDownPage();
                break;
            default:
                break;
        }

        if(mainmenu_Group.gameObject.active) {
            if (nameInput == CREATE_NEWSHOP_BUTTON_NAME) {
                //<!-- SceneState.showNewShop -->
                StartCoroutine(ShowCreateNewGame());
				this.characterAnimationManager.PlayGoodAnimation();
				audioDescribe.PlayOnecSound(description_clips[1]);
                return;
            }
            else if (nameInput == LOADSHOP_BUTTON_NAME) {
                //<!-- SceneState.showLoadGame -->
                StartCoroutine(ShowLoadShop());
                this.characterAnimationManager.RandomPlayGoodAnimation();

                return;
            }
        }
        else if(newgame_Group.gameObject.active) {
            //<!-- GUIState.showNewGame -->
            if(nameInput == back_button.name) {
                StartCoroutine(ShowMainMenu());
            }
            else if(nameInput == "OK_button") {
                this.CheckUserNameFormInput();
            }
        }
        else if(loadgame_Group.gameObject.active) {
            if(nameInput == back_button.name) {
                StartCoroutine(ShowMainMenu());
            }
        }
        else if(initializeNewGame_Group.gameObject.active) {
            if(nameInput == back_button.name) {
                StartCoroutine(ShowMainMenu());
            }
            else if(nameInput == "OK_button") {
				if(shopName != "") {
                    this.characterAnimationManager.RandomPlayGoodAnimation();
                    audioEffect.PlayOnecWithOutStop(audioEffect.correct_Clip);
					this.SaveNewPlayer();					
					base.extendsStorageManager.LoadSaveDataToGameStorage();					
					this.LoadSceneTarget();
				}
                else{
                    this.characterAnimationManager.PlayEyeAnimation(CharacterAnimationManager.NameAnimationsList.agape);
                    base.audioEffect.PlayOnecWithOutStop(audioEffect.wrong_Clip);
                }
            }
			else if(nameInput == "Previous_button") {
				initializeNewShop.HavePreviousCommand();
                this.characterAnimationManager.RandomPlayGoodAnimation();
			}
			else if(nameInput == "Next_button") {
				initializeNewShop.HaveNextCommand();
                this.characterAnimationManager.RandomPlayGoodAnimation();
			}
			else if(nameInput == "Blue") {
				initializeNewShop.HaveChangeLogoColor("Blue");
			}
			else if(nameInput == "Green") {
				initializeNewShop.HaveChangeLogoColor("Green");
			}
			else if(nameInput == "Pink") {
				initializeNewShop.HaveChangeLogoColor("Pink");
			}
			else if(nameInput == "Red") {
				initializeNewShop.HaveChangeLogoColor("Red");
			}
			else if(nameInput == "Yellow") {
				initializeNewShop.HaveChangeLogoColor("Yellow");
			}
        }
    }

    private IEnumerator ShowInitializeNewShop()
    {
        currentSceneState = SceneState.showNewShop;
		audioDescribe.PlayOnecSound(description_clips[2]);

        initializeNewGame_Group.gameObject.SetActiveRecursively(true);
        OK_button_Obj = initializeNewGame_Group.transform.Find("OK_button").gameObject;        

        if(mainmenu_Group.gameObject.active)
            iTween.MoveTo(mainmenu_Group.gameObject, moveUpTransform_Data);
        if (newgame_Group.gameObject.active)
            iTween.MoveTo(newgame_Group.gameObject, moveUpTransform_Data);
        if (loadgame_Group.gameObject.active)
            iTween.MoveTo(loadgame_Group.gameObject, moveUpTransform_Data);

		iTween.MoveTo(options_group_transform.gameObject, iTween.Hash("y", 2f, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));

        yield return new WaitForSeconds(1);

        iTween.MoveTo(initializeNewGame_Group.gameObject, moveDownTransform_Data);
        mainmenu_Group.gameObject.SetActiveRecursively(false);
        newgame_Group.gameObject.SetActiveRecursively(false);
        loadgame_Group.gameObject.SetActiveRecursively(false);
    }

    private IEnumerator ShowMainMenu()
    {
        currentSceneState = SceneState.none;

        mainmenu_Group.gameObject.SetActiveRecursively(true);

        if(newgame_Group.gameObject.active)
            iTween.MoveTo(newgame_Group.gameObject, moveUpTransform_Data);
        if (initializeNewGame_Group.gameObject.active)
            iTween.MoveTo(initializeNewGame_Group.gameObject, moveUpTransform_Data);
        if (loadgame_Group.gameObject.active)
            iTween.MoveTo(loadgame_Group.gameObject, moveUpTransform_Data);

        yield return new WaitForSeconds(1);

        iTween.MoveTo(mainmenu_Group.gameObject, moveDownTransform_Data);
		iTween.MoveTo(options_group_transform.gameObject, iTween.Hash("y", 0.85f, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));

        newgame_Group.gameObject.SetActiveRecursively(false);
        initializeNewGame_Group.gameObject.SetActiveRecursively(false);
        loadgame_Group.gameObject.SetActiveRecursively(false);
        back_button.gameObject.SetActiveRecursively(false);
    }

    private IEnumerator ShowCreateNewGame()
    {
        iTween.MoveTo(mainmenu_Group.gameObject, moveUpTransform_Data);
		iTween.MoveTo(options_group_transform.gameObject, iTween.Hash("y", 2f, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));

        newgame_Group.gameObject.SetActiveRecursively(true);
        back_button.gameObject.SetActiveRecursively(true);
        OK_button_Obj = newgame_Group.transform.Find("OK_button").gameObject; 

        yield return new WaitForSeconds(1);

        iTween.MoveTo(newgame_Group.gameObject, moveDownTransform_Data);
        mainmenu_Group.gameObject.SetActiveRecursively(false);

        currentSceneState = SceneState.showNewGame;
    }

    private IEnumerator ShowLoadShop() {        
        if(newgame_Group.gameObject.active)
            iTween.MoveTo(newgame_Group.gameObject, moveUpTransform_Data);
        if (initializeNewGame_Group.gameObject.active)
            iTween.MoveTo(initializeNewGame_Group.gameObject, moveUpTransform_Data);
        if(mainmenu_Group.gameObject.active)
            iTween.MoveTo(mainmenu_Group.gameObject, moveUpTransform_Data);
		
		iTween.MoveTo(options_group_transform.gameObject, iTween.Hash("y", 2f, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));

        loadgame_Group.gameObject.SetActiveRecursively(true);
        back_button.gameObject.SetActiveRecursively(true);

        yield return new WaitForSeconds(1);

        iTween.MoveTo(loadgame_Group.gameObject, moveDownTransform_Data);
        mainmenu_Group.gameObject.SetActiveRecursively(false);
        newgame_Group.gameObject.SetActiveRecursively(false);

        currentSceneState = SceneState.showLoadGame;
    }
}
