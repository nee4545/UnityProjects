using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWeaponType
{
    NONE = 0,
    PISTOL,
    MACHINE_GUN,
    ROCKET_LAUNCHER,
    SEMI_AUTOMATIC,
}

public enum eAmmoType
{
    REGULAR,
    PROJECTILE,
}


[Serializable]
public struct WeaponDef
{
    public string WeaponName;
    public eWeaponType WeaponType;
    public int AmmoCount;
    public int shotsPerHit;
    public AmmoDef Ammo;
    public float FireRate;
}

[Serializable]
public struct AmmoDef
{
    public eAmmoType AmmoType;
    public float AmmoDamage;
    public float AmmoSpeed;
    bool IsAreaEffect;

}

[Serializable]
public class WeaponDefinition
{
    public List<WeaponDef> Weapons;

}
