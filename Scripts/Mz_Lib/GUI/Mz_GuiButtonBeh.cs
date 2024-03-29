using UnityEngine;
using System.Collections;

[AddComponentMenu("Mz_ScriptLib/GUI/Mz_GuiButtonBeh")]
public class Mz_GuiButtonBeh : Base_ObjectBeh {
	
	public bool enablePlayAudio = true;
	
	private Mz_BaseScene gameController;
    private Vector3 originalScale;
		
	
	void Awake() {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Mz_BaseScene> ();		
	}
	
	// Use this for initialization
    void Start ()
	{
        originalScale = this.transform.localScale;
	}

	void OnApplicationPause (bool pause) {
		collider.enabled = !pause;
	}
	
	protected override void OnTouchBegan ()
	{
		base.OnTouchBegan ();
		
		if(this.enablePlayAudio)
			gameController.audioEffect.PlayOnecSound(gameController.audioEffect.buttonDown_Clip);

		this.transform.localScale = originalScale * 1.1f;
	}

	protected override void OnTouchDown ()
	{
        gameController.OnInput(this.gameObject.name);
		
		base.OnTouchDown ();
	}
	protected override void OnTouchEnded ()
	{
		base.OnTouchEnded ();		
		
        this.transform.localScale = originalScale;
	}
}
