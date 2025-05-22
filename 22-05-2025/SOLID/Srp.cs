// SRP - Single Responsibility Principle

/*
A class or a function, should have One responsibility and only one reason to change.

This ensured that specific changes to working of a project are handled seperately
*/

//Violation example

class Player
{
    public string Name;
    public float Health;
    public float Mana;

    public void Move(int x, int y, int z)
    {
        System.Console.WriteLine($"Moving player to target {x}:{y}:{z}");
    }

    public void Save(Savedata data)
    {
        System.Console.WriteLine($"Writing data to savefile: {data.ToString()}");
    }

    public void Render(FrameBuffer buf, PlayerModel model)
    {
        //Handles rendering
        System.Console.WriteLine("Rendered player");
    }
}

// This is a heavily unmaintanable code, as if any logic for saving or rendering changes, the Player class needs to be modified as well

// Fixed Violations

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
        // Save logic could include using player info and data
        System.Console.WriteLine($"Writing {player.Name}'s data to savefile: {data.ToString()}");
    }
}


class PlayerRenderer
{
    public void Render(FrameBuffer buf, Player player, PlayerModel model)
    {
        // Actual rendering logic
        System.Console.WriteLine($"Rendered {player.Name} using model and framebuffer");
    }
}


// The responsibilites have been split, and each class is responsible for ONE functionality