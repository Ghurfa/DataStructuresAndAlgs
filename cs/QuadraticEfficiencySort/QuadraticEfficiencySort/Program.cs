using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadraticEfficiencySort
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
            void swapValues(ref int[] array, int firstIndex, int secondIndex)
            {
                int tempValue = array[firstIndex];
                array[firstIndex] = array[secondIndex];
                array[secondIndex] = tempValue;
            }
            int numofChecks = 0;
            for (int i = 0; i < NumbersToSort.Length; i++)
            {
                for (int j = 0; j < NumbersToSort.Length; j++)
                {
                    numofChecks++;
                    if(NumbersToSort[j] < NumbersToSort[i])
                    {
                        swapValues(ref NumbersToSort, i, j);
                    }
                }
            }
            Console.WriteLine($"Number of checks: {numofChecks}");
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
