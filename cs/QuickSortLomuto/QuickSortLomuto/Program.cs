using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSortLomuto
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
            Random pivotChooser = new Random();
            void moveValue(ref int[] arrayToChange, int currentIndex, int newIndex)
            {
                int value = arrayToChange[currentIndex];
                int moveDirection = Math.Sign(newIndex - currentIndex);
                for (int i = currentIndex; i * moveDirection < newIndex * moveDirection; i += moveDirection)
                {
                    arrayToChange[i] = arrayToChange[i + moveDirection];
                }
                arrayToChange[newIndex] = value;
            }
            int[] joinArraysAndPivot(int[] leftArray, int pivot, int[] rightArray)
            {
                int[] outputArray = new int[leftArray.Length + 1 + rightArray.Length];
                for (int i = 0; i < leftArray.Length; i++)
                {
                    outputArray[i] = leftArray[i];
                }
                outputArray[leftArray.Length] = pivot;
                for (int i = 0; i < rightArray.Length; i++)
                {
                    outputArray[i + leftArray.Length + 1] = rightArray[i];
                }
                return outputArray;
            }
            int[] quickSort(int[] arrayToSort)
            {
                if (arrayToSort.Length <= 1) return arrayToSort;
                int pivot = arrayToSort[arrayToSort.Length - 1];
                int leftSubArraySize = 0; 
                for(int i = 0; i < arrayToSort.Length - 1; i++)
                {
                    if(arrayToSort[i] <= pivot)
                    {
                        moveValue(ref arrayToSort, i, leftSubArraySize);
                        leftSubArraySize++;
                    }
                }
                int rightSubArraySize = arrayToSort.Length - leftSubArraySize - 1;
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
                return joinArraysAndPivot(quickSort(leftSubArray), pivot, quickSort(rightSubArray));
            }
            NumbersToSort = quickSort(NumbersToSort);
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
