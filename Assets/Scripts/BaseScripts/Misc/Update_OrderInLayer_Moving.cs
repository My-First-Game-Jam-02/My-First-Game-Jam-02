using UnityEngine;
using System.Collections;


public class Update_OrderInLayer_Moving : MonoBehaviour {

	[SerializeField] private int internalSort = 0;

	private SpriteRenderer spriteRenderer;
	private Transform objectTransform;

	public int sortingOrderFactor = 100;

	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		objectTransform = GetComponent<Transform> ();
		spriteRenderer.sortingOrder = (int)(objectTransform.position.y * -sortingOrderFactor) + internalSort;
	}

	void Update(){
		spriteRenderer.sortingOrder = (int)(objectTransform.position.y * -sortingOrderFactor) + internalSort;
	}
}

