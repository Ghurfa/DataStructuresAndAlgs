namespace BTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree23<int> tree = new();
            tree.Insert(5);
            tree.Insert(8);
            tree.Insert(90);
            tree.Insert(12);
            tree.Insert(23);
            tree.Insert(10);
            tree.Insert(45);
            tree.Insert(4);
            tree.Insert(4);
            tree.Insert(2);

            while (true)
            {
                string command = Console.ReadLine().Trim().ToLower();
                string[] parts = command.Split(' ');
                if (parts.Length > 0)
                {
                    switch (parts[0])
                    {
                        case "a":
                        case "i":
                            if (parts.Length > 1 && int.TryParse(parts[1], out int addNum))
                            {
                                tree.Insert(addNum);
                            }
                            break;
                        case "r":
                        case "d":
                            if (parts.Length > 1 && int.TryParse(parts[1], out int deleteNum))
                            {
                                Console.WriteLine("Remove: " + tree.Remove(deleteNum));
                            }
                            break;
                        case "c":
                            if (parts.Length > 1 && int.TryParse(parts[1], out int containsNum))
                            {
                                Console.WriteLine("Contains: " + tree.Contains(containsNum));
                            }
                            break;
                        case "p":
                            tree.Print();
                            break;
                    }
                }
            }
        }
    }
}