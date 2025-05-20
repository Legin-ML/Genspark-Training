

class Post
{
    public string Caption { get; set; }
    public int Likes { get; set; }

    public Post()
    {
        Caption = string.Empty;
        Likes = 0;
    }
}

class Program
{
    static void Main()
    {
        string[][] input = GetInputs(); 
        PrintPosts(input);
        //Post[][] input = GetInputsClasses(); PrintPostsClasses(input);

    }

    static void PrintPostsClasses(Post[][] input)
    {

        //CLASS IMPLEMENTATION
        Console.WriteLine("\n--- Displaying Instagram Posts ---");
        for (int i = 0; i < input.Length; i++)
        {
            Console.WriteLine($"\n     -User {i + 1}-     ");
            for (int j = 0; j < input[i].Length; j++)  
            {
                Console.WriteLine($"Post {j + 1} - Caption: {input[i][j].Caption} | Likes: {input[i][j].Likes}");
            }
        }
    }

    static Post[][] GetInputsClasses()
    {

        // CLASS IMPLEMENTATION
        Console.Write("Enter the number of Users: ");
        int.TryParse(Console.ReadLine(), out int num);

        if (num <= 0)
        {
            ExitApp();
        }

        Post[][] input = new Post[num][];

        for (int i = 0; i < num; i++)
        {
            Console.Write($"\nUser {i + 1}: How many posts?: ");
            int.TryParse(Console.ReadLine(), out int postCount);
            if (postCount <= 0)
            {
                ExitApp();
            }

            input[i] = new Post[postCount];

            for (int j = 0; j < postCount; j++)
            {
                Console.Write($"Enter caption for post {j + 1}: ");
                string caption = Console.ReadLine();
                Console.Write("Enter Likes: ");
                int.TryParse(Console.ReadLine(), out int likes);

                if (string.IsNullOrEmpty(caption) || likes < 0)
                {
                    Console.WriteLine("--- Please enter valid inputs");
                    j--;  
                    continue;
                }

                input[i][j] = new Post
                {
                    Caption = caption,
                    Likes = likes
                };
            }
        }

        return input;
    }




    static void PrintPosts(string[][] input)
    {
        Console.WriteLine("\n--- Displaying Instagram Posts ---");
        for (int i = 0; i < input.Length; i++)  
        {
            Console.WriteLine($"\n     -User {i + 1}-     ");
            int postCount = input[i].Length / 2;  
            for (int j = 0; j < postCount; j++)  
            {
                Console.WriteLine($"Post {j + 1} - Caption: {input[i][j * 2]} | Likes: {input[i][j * 2 + 1]}");
            }
        }
    }


    static void ExitApp()
    {
        System.Console.WriteLine("Please enter valid input");
        System.Environment.Exit(1);
    }

    static string[][] GetInputs()
    {
        System.Console.Write("Enter the number of Users: ");
        int.TryParse(Console.ReadLine(), out int num);

        if (num <= 0)
        {
            ExitApp();
        }

        string[][] input = new string[num][];

        for (int i = 0; i < num; i++)
        {
            System.Console.Write($"\nUser {i + 1}: How many posts?: ");
            int.TryParse(Console.ReadLine(), out int post_count);
            if (post_count <= 0)
            {
                ExitApp();
            }

            input[i] = new string[post_count * 2];

            for (int j = 0; j < post_count; j++)
            {
                Console.Write($"Enter caption for post {j+1}:");
                string caption = Console.ReadLine();
                Console.Write("Enter Likes: ");
                int.TryParse(Console.ReadLine(), out int likes);

                if (string.IsNullOrEmpty(caption) || likes < 0)
                {
                    System.Console.WriteLine("---Please enter valid inputs");
                    j--;
                }

                input[i][j * 2] = caption;
                input[i][j * 2 + 1] = likes.ToString();
            }
        }

        return input;
    }
}


