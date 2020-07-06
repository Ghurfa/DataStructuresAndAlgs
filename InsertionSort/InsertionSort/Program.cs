using System;
using System.Collections.Generic;

namespace InsertionSort
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
                for (int i = 0; i < numOfNums; i++)
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
            List<int> sortedNumbers = new List<int>();
            for (int i = 0; i < NumbersToSort.Length; i++)
            {
                int lowerNumIndex = -1;
                for (int j = 0; j < sortedNumbers.Count; j++)
                {
                    if(sortedNumbers[j] < NumbersToSort[i])
                    {
                        lowerNumIndex = j;
                    }
                }
                //Console.WriteLine($"Inserting {NumbersToSort[i]} at index {lowerNumIndex + 1}");
                sortedNumbers.Insert(lowerNumIndex + 1, NumbersToSort[i]);
            }
            Console.WriteLine("Array is sorted");
            Console.Write("Array: ");
            for (int i = 0; i < sortedNumbers.Count; i++)
            {
                Console.Write(sortedNumbers[i] + " "); ;
            }
            Console.ReadKey();
        }
    }
}
