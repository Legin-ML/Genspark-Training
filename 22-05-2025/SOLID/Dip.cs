// DIP - Dependency Inversion Principle

/*
High-level modules should not depend on low-level modules. Both should depend on abstractions.

Abstractions should not depend on details. Details should depend on abstractions.

(i.e) There should be no tight coupling as far as possible
*/

// Violation Example

class Sword
{
    public void Attack()
    {
        System.Console.WriteLine("Swinging sword!");
    }
}

class Player
{
    private Sword _sword;

    public Player()
    {
        _sword = new Sword(); // Directly depends on a concrete weapon
    }

    public void PerformAttack()
    {
        _sword.Attack(); // Hard-coded dependency
    }
}

// This violates DIP because Player is tightly coupled to the Sword class.
// To use a different weapon, Player must be modified 

// Fixed Violations

interface IWeapon
{
    void Attack();
}

class Sword : IWeapon
{
    public void Attack()
    {
        System.Console.WriteLine("Swinging sword!");
    }
}

class Bow : IWeapon
{
    public void Attack()
    {
        System.Console.WriteLine("Shooting an arrow!");
    }
}

class Player
{
    private IWeapon _weapon;

    public Player(IWeapon weapon)
    {
        _weapon = weapon;
    }

    public void PerformAttack()
    {
        _weapon.Attack();
    }
}

/*
Now the Player class is decoupled from specific weapon implementations.
New weapons can be added without changing the Player class.

*/
