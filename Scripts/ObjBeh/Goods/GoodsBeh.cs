using UnityEngine;
using System;
using System.Collections;

public class GoodsBeh : ObjectsBeh {    
    public const string ClassName = "GoodsBeh";
	internal Vector3 offsetPos;	
	internal int costs;

	//@-- WaitForIngredientEvent.
	protected bool _isWaitFotIngredient = false;	
	protected event System.EventHandler waitForIngredientEvent;
    protected void CheckingDelegationOfWaitFotIngredientEvent(object sender, EventArgs e) {
        if (waitForIngredientEvent != null)
            waitForIngredientEvent(sender, System.EventArgs.Empty);
    }
	protected virtual void Handle_waitForIngredientEvent (object sender, System.EventArgs e)
	{
		_isWaitFotIngredient = true;
	}
	public virtual void WaitForIngredient(string ingredientName) {			
		Debug.Log("WaitForIngredient :: " + ingredientName);
	}
	
	/// Put goods objects intance on food tray.
	public event System.EventHandler putObjectOnTray_Event;
	protected void OnPutOnTray_event (System.EventArgs eventArgs) {
		if (putObjectOnTray_Event != null) 
        {
			putObjectOnTray_Event (this, eventArgs);
            stageManager.audioEffect.PlayOnecWithOutStop(stageManager.soundEffect_clips[5]);

			Debug.Log (putObjectOnTray_Event + " : " + this.name);

            if(MainMenu._HasNewGameEvent)
                stageManager.CheckingGoodsObjInTray("newgame_event");
		}
	}
    
	protected override void ImplementDraggableObject ()
	{
		base.ImplementDraggableObject ();
		
		Ray cursorRay;
		RaycastHit hit;		
		cursorRay = new Ray(this.transform.position, Vector3.forward);
		
		if(Physics.Raycast(cursorRay, out hit)) 
        {
			if(hit.collider.name == stageManager.binBeh.name) {			
				if(this._isDropObject == true) {
					stageManager.binBeh.PlayOpenAnimation();
                    this.OnDispose();
                    OnDestroyObject_event(System.EventArgs.Empty);
				}
			}
			else if(hit.collider.name == stageManager.foodsTray_obj.name) {
                if(this._isDropObject) {
					this._isDropObject = false;
	                base._isDraggable = false;
					base._canActive = false;
					this._isWaitFotIngredient = false;
					this.waitForIngredientEvent -= this.Handle_waitForIngredientEvent;

                    OnPutOnTray_event(System.EventArgs.Empty);
					
					if(stageManager.toasts[0] == this) {
						stageManager.toasts[0] = null;
					}
					else if(stageManager.toasts[1] == this) {
						stageManager.toasts[1] = null;
					}
                }
            }
        	else {
	            if(this._isDropObject) {
	                this.transform.position = originalPosition;
	                this._isDropObject = false;
	                base._isDraggable = false;
	            }
        	}
		}
		else {
            if(this._isDropObject) {
//            if(_isDraggable == false) {
                this.transform.position = originalPosition;
                this._isDropObject = false;
                base._isDraggable = false;
            }
		}
		
		Debug.DrawRay(cursorRay.origin, Vector3.forward, Color.red);
	}

    private void StopActiveAnimation() {
		//<!--- On object active.
        if(animation) {
			this.animation.Stop();
		}
		else if(animatedSprite) {
            animatedSprite.Stop();
		}
    }
	
	// <!-- OnInput Events.
	
	protected override void OnTouchBegan()
	{
		base.OnTouchBegan();
		
		this.transform.localScale = new Vector3(1.2f, 1.2f, 1);
		baseScene.audioEffect.PlayOnecWithOutStop(baseScene.audioEffect.pop_clip);
	}

	protected override void OnTouchDown ()
	{
		if(_canActive && _isWaitFotIngredient) {
			//<!--- On object active.
			if(animatedSprite && animationName_001 != string.Empty) {
				animatedSprite.Play(animationName_001);				
				animatedSprite.animationCompleteDelegate = animationCompleteDelegate;
			}
			else { 
				iTween.PunchPosition(this.gameObject, iTween.Hash("y", 0.2f, "time", 1f, "looptype", iTween.LoopType.loop));
			}
		}

		base.OnTouchDown();
	}

	protected override void OnTouchEnded ()
	{
		base.OnTouchEnded();
		
		this.transform.localScale = new Vector3(1f, 1f, 1);
		
		if(base._isDraggable)
			_isDropObject = true;
	}

    public override void OnDispose()
    {
        base.OnDispose();

        Destroy(this.gameObject);
        this.waitForIngredientEvent -= this.Handle_waitForIngredientEvent;
    }
}