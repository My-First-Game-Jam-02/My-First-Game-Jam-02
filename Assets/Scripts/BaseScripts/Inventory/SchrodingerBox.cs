using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchrodingerBox : MonoBehaviour
{

    private bool canOpen;

    public GameObject item;
    public GameObject openBoxSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canOpen = false;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(canOpen && Input.GetButtonDown("Fire1"))
        {
            RevealItem();    
        }        
    }

    public void RevealItem()
    {

        Instantiate(openBoxSFX, transform.position, transform.rotation);
        Instantiate(item, transform.position, transform.rotation);
       
        this.gameObject.SetActive(false);
    }
}
