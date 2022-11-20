namespace HuffmanCoding
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string text = File.ReadAllText(@"..\..\..\beeMovieScript.txt");
            HuffmanTree tree = HuffmanTree.CreateTree(text);
            var encoded = tree.Encode(text);
            var decoded = tree.Decode(encoded);
            if (text == decoded.ToString())
            {
                Console.WriteLine("Success");
                Console.WriteLine("Original size: " + text.Length);
                Console.WriteLine("Compressed size: " + encoded.Length);
            }
        }
    }
}