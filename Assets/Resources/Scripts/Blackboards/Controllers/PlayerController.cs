using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected PlayerMovement _movement;
    [SerializeField] protected PlayerAim _aim;
    [SerializeField] protected GunsInventory _guns;
    [SerializeField] protected Shoot _shoot;

    protected (float x, float y) _speed;
    protected Vector2 _aimPoint;
    protected int _weaponDelta;
    protected bool _fire;
    protected bool _reload;

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateValues();
        
        _movement.Speed = _speed;
        _aim.Aim(_aimPoint);

        if(_weaponDelta != 0)_guns.ChangeWeapon(_weaponDelta);

        if (_guns.ActiveGun != null)
        {
            _guns.ActiveGun.Fire(_fire);
            if(_reload) _guns.ActiveGun.Reload();
        }
        
        _weaponDelta = 0;
        _fire = false;
        _reload = false;
    }

    protected virtual void UpdateValues()
    {
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Aim(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        float scrollDelta = Input.GetAxisRaw("Mouse ScrollWheel");
        int delta = scrollDelta != 0 ? (scrollDelta > 0 ? -1 : 1) : 0;
        if (Input.GetKeyDown(KeyCode.Tab)) delta = 1;
        ChangeWeapon(delta);

        if (Input.GetButton("Fire1")) Fire();
        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }

    public virtual bool Move(float x, float y)
    {
        _speed = (x, y);
        return true;
    }

    public virtual bool Aim(Vector2 aim)
    {
        _aimPoint = aim;
        return true;
    }

    public virtual bool ChangeWeapon(int delta)
    {
        _weaponDelta = delta;
        return true;
    }

    public virtual bool Fire()
    {
        _fire = true;
        return _guns.CanShoot;
    }

    public virtual bool Reload()
    {
        _reload = true;
        return _guns.CanReload;
    }
}
