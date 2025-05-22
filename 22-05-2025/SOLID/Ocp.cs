// OCP - Open/Closed Principle

/*
Software entities (classes, modules, functions) should be open for extension but closed for modification.

This means you should be able to add new functionality without modifying existing code.
*/

// Violation Example

class Enemy
{
    public string Type;

    public Enemy(string type)
    {
        Type = type;
    }

    public void Attack()
    {
        if (Type == "Goblin")
        {
            System.Console.WriteLine("Goblin slashes with knife!");
        }
        else if (Type == "Troll")
        {
            System.Console.WriteLine("Troll smashes with club!");
        }
        else if (Type == "Dragon")
        {
            System.Console.WriteLine("Dragon breathes fire!");
        }
    }
}

// This code violates OCP as we need to MODIFY the Attack() method every time a new enemy is added.

// Fixed Violations

abstract class EnemyBase
{
    public abstract void Attack();
}

class Goblin : EnemyBase
{
    public override void Attack()
    {
        System.Console.WriteLine("Goblin slashes with knife!");
    }
}

class Troll : EnemyBase
{
    public override void Attack()
    {
        System.Console.WriteLine("Troll smashes with club!");
    }
}

class Dragon : EnemyBase
{
    public override void Attack()
    {
        System.Console.WriteLine("Dragon breathes fire!");
    }
}

class GameEngine
{ 
    public void SimulateAttack(EnemyBase enemy)
    {
        enemy.Attack();
    }
}

/*
Now the system is EXTENDED by adding new classes (e.g. Orc, Skeleton, etc.),
not by modifying the existing ones.

This keeps the code clean, stable, and scalable.
*/
