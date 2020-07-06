using System;

namespace BubbleSort
{
    class Program
    {
        static void Main(string[] args)
        {
            string generateChoice = "";
            bool isValidChoice = false;
            do
            {
                Console.WriteLine("Manually [manual] or automatically [auto] generate the numbers?");
                generateChoice = Console.ReadLine().ToLower();
                isValidChoice = generateChoice == "manual" || generateChoice == "auto";
            } while (!isValidChoice);
            Console.WriteLine("How many numbers?");
            int numOfNums = Convert.ToInt32(Console.ReadLine());
            int[] NumbersToSort = new int[numOfNums];
            if (generateChoice == "manual")
            {
                for(int i = 0; i < numOfNums; i++)
                {
                    Console.WriteLine("Number:");
                    NumbersToSort[i] = Convert.ToInt32(Console.ReadLine());
                }
            }
            else
            {
                Random random = new Random();
                for (int i = 0; i < numOfNums; i++)
                {
                    int newNumber = random.Next(1000);
                    NumbersToSort[i] = newNumber;
                    Console.WriteLine($"Number: {newNumber}");
                }
            }
            void swapWithNextValue(ref int[] array, int index)
            {
                int tempValue = array[index];
                array[index] = array[index + 1];
                array[index + 1] = tempValue;
            }
            bool isSorted = false;
            while (!isSorted)
            {
                isSorted = true;
                for(int i = 0; i < NumbersToSort.Length - 1; i++)
                {
                    int currentValue = NumbersToSort[i];
                    int nextValue = NumbersToSort[i + 1];
                    if (currentValue > nextValue)
                    {
                        swapWithNextValue(ref NumbersToSort, i);
                        isSorted = false;
                    }
                }
            }
            Console.WriteLine("Array is sorted");
            Console.Write("Array: ");
            for (int i = 0; i < NumbersToSort.Length; i++)
            {
                Console.Write(NumbersToSort[i] + " "); ;
            }
            Console.ReadKey();
        }
    }
}
