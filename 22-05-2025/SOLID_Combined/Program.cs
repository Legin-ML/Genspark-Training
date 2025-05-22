using System;

// SRP - Single Responsibility Principle

class Player
{
    public string Name;
    public float Health;
    public float Mana;

    public void Move(int x, int y, int z)
    {
        System.Console.WriteLine($"Moving player to target {x}:{y}:{z}");
    }
}

class SaveManager
{
    public void Save(Player player, Savedata data)
    {
        System.Console.WriteLine($"Saving {player.Name}'s data to savefile: {data.ToString()}");
    }
}

class PlayerRenderer
{
    public void Render(FrameBuffer buf, Player player, PlayerModel model)
    {
        System.Console.WriteLine($"Rendered {player.Name} using model and framebuffer");
    }
}

// OCP - Open/Closed Principle

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

class DragonEnemy : EnemyBase
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

// LSP - Liskov Substitution Principle

abstract class WeaponBase
{
    public abstract void Use();
}

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

class PlayerWithWeapon
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

// ISP - Interface Segregation Principle

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

class NormalPlayer : IMovable, IAttackable
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

class DragonCreature : IMovable, IAttackable, IFlyable
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

// DIP - Dependency Inversion Principle

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

class PlayerWithDifferentWeapon
{
    private IWeapon _weapon;

    public PlayerWithDifferentWeapon(IWeapon weapon)
    {
        _weapon = weapon;
    }

    public void PerformAttack()
    {
        _weapon.Attack();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // SRP Example
        var player = new Player { Name = "Hero", Health = 100, Mana = 50 };
        var saveManager = new SaveManager();
        var playerRenderer = new PlayerRenderer();

        saveManager.Save(player, new Savedata());
        playerRenderer.Render(new FrameBuffer(), player, new PlayerModel());

        // OCP Example
        var gameEngine = new GameEngine();
        var goblin = new Goblin();
        var troll = new Troll();
        var dragonEnemy = new DragonEnemy();
        gameEngine.SimulateAttack(goblin);
        gameEngine.SimulateAttack(troll);
        gameEngine.SimulateAttack(dragonEnemy);

        // LSP Example
        var blasphemousBlade = new BlasphemousBlade();
        var meteoriteStaff = new MeteoriteStaff();

        var playerWithWeapon = new PlayerWithWeapon();
        playerWithWeapon.UseWeapon(blasphemousBlade);
        playerWithWeapon.TryUpgrade(blasphemousBlade); // Will upgrade the blade
        playerWithWeapon.UseWeapon(meteoriteStaff);
        playerWithWeapon.TryUpgrade(meteoriteStaff); // Won't upgrade the staff

        // ISP Example
        var dragonCreature = new DragonCreature();
        dragonCreature.Move();
        dragonCreature.Attack();
        dragonCreature.Fly();

        var normalPlayer = new NormalPlayer();
        normalPlayer.Move();
        normalPlayer.Attack();

        // DIP Example
        var sword = new Sword();
        var bow = new Bow();
        var playerWithSword = new PlayerWithDifferentWeapon(sword);
        var playerWithBow = new PlayerWithDifferentWeapon(bow);
        
        playerWithSword.PerformAttack();
        playerWithBow.PerformAttack();
    }
}

public class Savedata
{
    public override string ToString()
    {
        return "Player Save Data";
    }
}

public class FrameBuffer { }

public class PlayerModel { }
