using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Mz_BaseScene : MonoBehaviour {
    
    public enum SceneNames { none = 0, LoadingScene = 1, MainMenu, WaitForStart, Town, BakeryShop, Sheepbank, Dressing, DisplayReward, };
		
	//<@-- Detect Touch and Input Data Fields.
    public Touch touch;
    public Vector3 mousePos;
    public Vector3 originalPos;
    public Vector3 currentPos;
    public bool _isDragMove = false;
	internal Mz_SmartDeviceInput smartDeviceInput;
	
	//<@-- Core game system.
	public ExtendsStorageManager extendsStorageManager;
	private HUDFPS hudFPS_Trace;

	//<@-- Banner.
#if UNITY_IOS
	
	public static bool _IsShowADBanner = false;
	protected Mz_ADBannerManager banner;
	
#endif


	//<@-- tutor.
	public GameObject cameraTutor_Obj;
	public GameObject plane_darkShadow;
	protected GameObject handTutor;
    protected List<GameObject> tutorDescriptions;

    internal void SetActivateTotorObject(bool activeState)
    {
        handTutor.active = activeState;
        if (activeState == false)
            iTween.Stop(handTutor);

        foreach (GameObject item in tutorDescriptions)
        {
            item.active = activeState;
        }
    }
	
	protected bool _onDestroyScene = false;
    public bool _hasQuitCommand = false;


	void Awake ()
	{
		//<@-- Trace OnScreen FPS.
//		this.gameObject.AddComponent<HUDFPS> ();
//		hudFPS_Trace = this.gameObject.GetComponent<HUDFPS>();

		this.gameObject.AddComponent<ExtendsStorageManager> ();
		extendsStorageManager = this.GetComponent<ExtendsStorageManager> ();

#if UNITY_IPHONE

		GameObject bannerObj = GameObject.Find ("Banner_Obj");
		if (Mz_BaseScene._IsShowADBanner && bannerObj == null) {
			bannerObj = new GameObject ("Banner_Obj", typeof(Mz_ADBannerManager));
			DontDestroyOnLoad (bannerObj);

			banner = bannerObj.GetComponent<Mz_ADBannerManager> ();
		}
		else if(Mz_BaseScene._IsShowADBanner && bannerObj) {
			banner = bannerObj.GetComponent<Mz_ADBannerManager> ();
		} 

#endif

		this.Initialization();
	}
	
	// Use this for initialization
	protected virtual void Initialization ()
	{
		Debug.Log("Mz_BaseScene :: Initialization");
	}
	
	//<!-- Audio Manage.
	public const string PATH_OF_APPRECIATE_CLIP = "AudioClips/AppreciateClips/";

	internal static bool ToggleAudioActive = true;
	public Base_AudioManager audioManager ;
	public GameEffectManager gameEffectManager;
	public AudioEffectManager audioEffect;
	public AudioDescribeManager audioDescribe;
	public GameObject audioBackground_Obj;
	public AudioClip background_clip;
	public List<AudioClip> description_clips;
	public List<AudioClip> soundEffect_clips;
	protected void CreateAudioObject()
    {
		Debug.Log("Scene :: InitializeAudio");

        //<!-- Setup All Audio Objects.
        if (audioEffect == null) {
                audioEffect = GameObject.FindGameObjectWithTag("AudioEffect").GetComponent<AudioEffectManager>();
			
                if(audioEffect) { 
					audioEffect.alternativeEffect_source = audioEffect.transform.GetComponentInChildren<AudioSource>();
				
					audioEffect.audio.mute = !ToggleAudioActive;
	            	audioEffect.alternativeEffect_source.audio.mute = !ToggleAudioActive;
				}
            }

        if (audioDescribe == null) {
                audioDescribe = GameObject.FindGameObjectWithTag("AudioDescribe").GetComponent<AudioDescribeManager>();
				audioDescribe.audio.mute = !ToggleAudioActive;
        }
        
        // <! Manage audio background.
		audioBackground_Obj = GameObject.FindGameObjectWithTag("AudioBackground");
        if (audioBackground_Obj == null)
        {
            audioBackground_Obj = new GameObject("AudioBackground", typeof(AudioSource));
            audioBackground_Obj.tag = "AudioBackground";
            audioBackground_Obj.audio.playOnAwake = true;
			audioBackground_Obj.audio.volume = 0.8f;
            audioBackground_Obj.audio.mute = !ToggleAudioActive;

            DontDestroyOnLoad(audioBackground_Obj);
        }
        else { 
            audioBackground_Obj.audio.mute = !ToggleAudioActive;
        }
    }

	protected void MuteAudioConfiguration ()
	{
		ToggleAudioActive = !ToggleAudioActive;
		this.SetActivateEnablePlayAudio();
	}

	void SetActivateEnablePlayAudio ()
	{		
		audioEffect.audio.mute = !ToggleAudioActive;
		audioBackground_Obj.audio.mute = !ToggleAudioActive;
		audioDescribe.audio.mute = !ToggleAudioActive;
	}

    //<!--- GUI_identity.
    public GameObject identityGUI_obj;
    public tk2dTextMesh usernameTextmesh;
    public tk2dTextMesh shopnameTextmesh;
    public tk2dTextMesh availableMoney;
    protected IEnumerator InitializeIdentityGUI()
    {
        if (Mz_StorageManage.Username != string.Empty)
        {
            usernameTextmesh.text = Mz_StorageManage.Username;
            usernameTextmesh.Commit();

            shopnameTextmesh.text = Mz_StorageManage.ShopName;
            shopnameTextmesh.Commit();

            availableMoney.text = Mz_StorageManage.AvailableMoney.ToString();
            availableMoney.Commit();
        }

        yield return null;
	}
	
	internal void ReFreshAvailableMoney()
	{
		this.availableMoney.text = Mz_StorageManage.AvailableMoney.ToString();
		this.availableMoney.Commit();
	}

    
    /// <summary>
    /// Virtual method. Used to generate game effect at runtime.
    /// Override this and implement your derived class. 
    /// </summary>
	protected virtual void InitializeGameEffectGenerator() {}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		if (smartDeviceInput == null) {
			this.gameObject.AddComponent<Mz_SmartDeviceInput>();
			smartDeviceInput = this.gameObject.GetComponent<Mz_SmartDeviceInput>();
		}

#if !UNITY_EDITOR && UNITY_IOS || UNITY_ANDROID
		smartDeviceInput.ImplementTouchInput ();
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
		smartDeviceInput.ImplementMouseInput ();
#endif

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
        {
            _hasQuitCommand = true;
        }
	}

	#region <!-- HasChangeTimeScale event.

	public static event EventHandler HasChangeTimeScale_Event;
	private void OnChangeTimeScale (EventArgs e) {
		if (HasChangeTimeScale_Event != null) 
				HasChangeTimeScale_Event (this, e);
	}
	protected void UpdateTimeScale(int delta) {
		Time.timeScale = delta;
		OnChangeTimeScale(EventArgs.Empty);
	}

	#endregion

    protected virtual void ImplementTouchPostion()
	{
//		Debug.Log ("ImplementTouchPostion");			
    }
	protected virtual void MovingCameraTransform ()
	{

	}

    public virtual void OnInput(string nameInput) {
    	Debug.Log("OnInput :: " + nameInput);
    }

    public virtual void OnPointerOverName(string nameInput) {
    	Debug.Log("OnPointerOverName :: " + nameInput);
    }
	
	protected virtual  void OnApplicationPause(bool pauseStatus) {
		Debug.Log("pauseStatus ==" + pauseStatus);
		this.extendsStorageManager.SaveDataToPermanentMemory();
    }

    void OnApplicationQuit() {
        this.extendsStorageManager.SaveDataToPermanentMemory();

#if UNITY_STANDALONE_WIN
        Application.CancelQuit();

        if(!Application.isLoadingLevel) {
            Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.MainMenu.ToString();
            Application.LoadLevelAsync(Mz_BaseScene.SceneNames.LoadingScene.ToString());
        }
#endif

#if UNITY_IPHONE || UNITY_ANDROID
        //<-- to do asking for quit game.
#endif
    }

    public virtual void OnDispose() { }
     
    protected virtual void OnGUI()
    {
        GUI.depth = 0;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));

        if (_hasQuitCommand)
        {			
            GUI.BeginGroup(new Rect(Screen.width / 2 - (200 * Mz_OnGUIManager.Extend_heightScale), Main.GAMEHEIGHT / 2 - 100, 400 * Mz_OnGUIManager.Extend_heightScale, 200), "Do you want to quit ?", GUI.skin.window);
            {
                if (GUI.Button(new Rect(60 * Mz_OnGUIManager.Extend_heightScale, 155, 100 * Mz_OnGUIManager.Extend_heightScale, 40), "Yes"))
                    Application.Quit();
                else if (GUI.Button(new Rect(240 * Mz_OnGUIManager.Extend_heightScale, 155, 100 * Mz_OnGUIManager.Extend_heightScale, 40), "No")) {
                    _hasQuitCommand = false; 
				}
            }
            GUI.EndGroup();
        }
    }
}
