using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private SSPlayerController playerController;
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }

    public Transform aimTransform;
    public Transform gunEndPoint;

    void Start()
    {
        playerController = FindObjectOfType<SSPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        PointWeaponAtMouse();
        HandleShooting();
    }

    private void PointWeaponAtMouse()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                gunEndPointPosition = gunEndPoint.position,
                shootPosition = mousePosition
            });
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }
}
