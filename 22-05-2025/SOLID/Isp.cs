// ISP - Interface Segregation Principle

/*
Clients should not be forced to depend on interfaces they do not use.

This means that no class should be required to implement methods it doesn't need.
Large interfaces should be split into smaller, more specific ones.
*/

// Violation Example

interface ICharacter
{
    void Move();
    void Attack();
    void Fly(); // Not all characters can fly
}

class Player : ICharacter
{
    public void Move()
    {
        System.Console.WriteLine("Player moves");
    }

    public void Attack()
    {
        System.Console.WriteLine("Player swings sword.");
    }

    public void Fly()
    {
        // Warrior can't fly
        throw new System.NotImplementedException("Player cannot fly!");
    }
}

// This violates ISP because Warrior is forced to implement Fly() even though it doesn't apply.

// Fixed Violations

interface IMovable
{
    void Move();
}

interface IAttackable
{
    void Attack();
}

interface IFlyable
{
    void Fly(); 
}

class Player : IMovable, IAttackable
{
    public void Move()
    {
        System.Console.WriteLine("Player moves");
    }

    public void Attack()
    {
        System.Console.WriteLine("Player swings weapon.");
    }
}

class Dragon : IMovable, IAttackable, IFlyable
{
    public void Move()
    {
        System.Console.WriteLine("Dragon stomps forward.");
    }

    public void Attack()
    {
        System.Console.WriteLine("Dragon breathes fire.");
    }

    public void Fly()
    {
        System.Console.WriteLine("Dragon flies through the skies.");
    }
}

/*
Now each class only implements the interfaces relevant to it.

*/
