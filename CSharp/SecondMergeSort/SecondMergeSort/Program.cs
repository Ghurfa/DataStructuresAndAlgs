using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondMergeSort
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
            int[] mergeSubArrays(int[] leftSubArray, int[] rightSubArray)
            {
                int[] outputArray = new int[leftSubArray.Length + rightSubArray.Length];
                int leftSubArrayIndex = 0;
                int rightSubArrayIndex = 0;
                for(int i = 0; i < outputArray.Length; i++)
                {
                    if(leftSubArrayIndex >= leftSubArray.Length)
                    {
                        outputArray[i] = rightSubArray[rightSubArrayIndex];
                        rightSubArrayIndex++;
                    }
                    else if(rightSubArrayIndex >= rightSubArray.Length)
                    {
                        outputArray[i] = leftSubArray[leftSubArrayIndex];
                        leftSubArrayIndex++;
                    }
                    else if(leftSubArray[leftSubArrayIndex] < rightSubArray[rightSubArrayIndex])
                    {
                        outputArray[i] = leftSubArray[leftSubArrayIndex];
                        leftSubArrayIndex++;
                    }
                    else
                    {
                        outputArray[i] = rightSubArray[rightSubArrayIndex];
                        rightSubArrayIndex++;
                    }
                }
                return outputArray;
            }
            int[] mergeSort(int[] arrayToSort)
            {
                if (arrayToSort.Length == 1) return arrayToSort;

                int leftSubArraySize = arrayToSort.Length / 2;
                int rightSubArraySize = arrayToSort.Length - leftSubArraySize;
                int[] leftSubArray = new int[leftSubArraySize];
                for (int i = 0; i < leftSubArraySize; i++)
                {
                    leftSubArray[i] = arrayToSort[i];
                }
                int[] rightSubArray = new int[rightSubArraySize];
                for (int i = 0; i < rightSubArraySize; i++)
                {
                    rightSubArray[i] = arrayToSort[i + leftSubArraySize];
                }
                return mergeSubArrays(mergeSort(leftSubArray), mergeSort(rightSubArray));
            }
            NumbersToSort = mergeSort(NumbersToSort);
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
