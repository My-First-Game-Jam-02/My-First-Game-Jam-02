using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Update_OrderInLayer_NotMoving : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Transform objectTransform;
	public Transform parentTransform;
	private int sortingNumber;
	private Canvas canvas;

	public int sortingOrderFactor = 100;
	// PL_Mod
	[HideInInspector]
    public bool hasFinishedSettingSortingOrder = false;
	// sorting order messes up at levels more negative than -32769;
	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		objectTransform = GetComponent<Transform> ();
		canvas = GetComponent<Canvas>();

		if(spriteRenderer != null)
        {
			spriteRenderer.sortingOrder = (int)(objectTransform.position.y * -sortingOrderFactor);
		}
		

		if(canvas != null)
        {
			canvas.sortingOrder = (int)(parentTransform.position.y * -sortingOrderFactor);
		}

		hasFinishedSettingSortingOrder = true;
	}
}

