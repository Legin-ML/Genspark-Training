// LSP - Liskov Substitution Principle

/*
Subtypes must be substitutable for their base types without altering the behavior of the program.

This ensures that any class that inherits from a base class can be used in its place without causing bugs or unexpected behavior.
*/

// Violation Example

class Weapon
{
    public virtual void Use()
    {
        System.Console.WriteLine("Using weapon.");
    }

    public virtual void Upgrade()
    {
        System.Console.WriteLine("Weapon upgraded.");
    }
}
 
class BlasphemousBlade : Weapon
{
    public override void Upgrade()
    {
        System.Console.WriteLine("Blasphemous Blade has been upgraded!");
    }
}

class MeteoriteStaff : Weapon
{
    public override void Upgrade()
    {
        // Here because base class REQUIRES an implementation
        throw new System.Exception("Meteorite Staff cannot be upgraded!");
    }
}

class Player
{
    public void UpgradeWeapon(Weapon weapon)
    {
        weapon.Upgrade(); 
    }
}
// This violates LSP because MeteoriteStaff is a Weapon, but using it breaks the expected behavior of Weapon.

// ✅ Fixed Violation – Game logic

// LSP - Liskov Substitution Principle

/*
Subtypes must be substitutable for their base types without altering the behavior of the program.

Here, only upgradable weapons implement IUpgradable. Others don't pretend to.
*/

// Base weapon type
abstract class WeaponBase
{
    public abstract void Use();
}

// Interface for upgradable weapons only
interface IUpgradable
{
    void Upgrade();
}

class BlasphemousBlade : WeaponBase, IUpgradable
{
    public override void Use()
    {
        System.Console.WriteLine("Swinging the Blasphemous Blade.");
    }

    public void Upgrade()
    {
        System.Console.WriteLine("Blasphemous Blade has been upgraded!");
    }
}

class MeteoriteStaff : WeaponBase
{
    public override void Use()
    {
        System.Console.WriteLine("Casting a spell with the Meteorite Staff.");
    }
}

class Player
{
    public void UseWeapon(WeaponBase weapon)
    {
        weapon.Use();
    }

    public void TryUpgrade(object weapon)
    {
        if (weapon is IUpgradable upgradableWeapon)
        {
            upgradableWeapon.Upgrade();
        }
        else
        {
            System.Console.WriteLine("This weapon cannot be upgraded.");
        }
    }
}


/*
Now all weapon types behave in a way that fits the base class contract.

*/
