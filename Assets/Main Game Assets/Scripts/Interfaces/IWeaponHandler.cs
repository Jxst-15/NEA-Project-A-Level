// For things like weaponHeld and other actions featuring weapons, used on characters
public interface IWeaponHandler
{
    void SetWeaponToNull();

    void SetWeaponHeld(bool val);
}
