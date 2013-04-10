using UnityEngine;
using System.Collections;


public class Food {

    private BakeryShop sceneManager;
    private Food instance;
	internal string name;
	internal int price;
	internal int costs;
	
	public Food ()
	{
        sceneManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BakeryShop>();        

		if (sceneManager.currentCustomer.list_goodsBag.Count > 0) {
			int r = Random.Range(0, sceneManager.currentCustomer.list_goodsBag.Count);

			instance = sceneManager.currentCustomer.list_goodsBag[r];
			this.name = instance.name;
			this.price = instance.price;	

			sceneManager.currentCustomer.list_goodsBag.Remove(instance);	

			Debug.Log("list_goodsBag.Count : " + sceneManager.currentCustomer.list_goodsBag.Count);
		}
        else {
			Debug.LogError("CustomerInstance.arr_goodsBag.Length == 0");
        }
	}
	
	public Food(string Init_name, int p_price) {
		this.instance = this;
		this.instance.name = Init_name;
		this.instance.price = p_price;
	}

	public Food(string Init_name, int p_price, int p_costs) {
		this.instance = this;
		this.instance.name = Init_name;
		this.instance.price = p_price;
		this.instance.costs = p_costs;
	}
}
