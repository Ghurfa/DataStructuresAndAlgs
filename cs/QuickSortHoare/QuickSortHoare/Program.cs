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
            int[] appendArrays(int[] leftArray, int[] rightArray)
            {
                int[] outputArray = new int[leftArray.Length + rightArray.Length];
                for (int i = 0; i < leftArray.Length; i++)
                {
                    outputArray[i] = leftArray[i];
                }
                for (int i = 0; i < rightArray.Length; i++)
                {
                    outputArray[i + leftArray.Length] = rightArray[i];
                }
                return outputArray;
            }
            int[] switchValues(int[] array, int leftValueIndex, int rightValueIndex)
            {
                int leftValue = array[leftValueIndex];
                array[leftValueIndex] = array[rightValueIndex];
                array[rightValueIndex] = leftValue;
                return array;
            }
            int[] quickSort(int[] arrayToSort)
            {
                if (arrayToSort.Length <= 1) return arrayToSort;
                int pivot = arrayToSort[0];
                int leftPointerIndex = 0;
                int leftPointerValue = arrayToSort[leftPointerIndex];
                int rightPointerIndex = arrayToSort.Length - 1;
                int rightPointerValue = arrayToSort[rightPointerIndex];
                while (leftPointerIndex < rightPointerIndex)
                {
                    while (leftPointerValue < pivot && leftPointerIndex < rightPointerIndex)
                    {
                        leftPointerIndex++;
                        leftPointerValue = arrayToSort[leftPointerIndex];
                    }
                    while (rightPointerValue > pivot && leftPointerIndex < rightPointerIndex)
                    {
                        rightPointerIndex--;
                        rightPointerValue = arrayToSort[rightPointerIndex];
                    }
                    if (leftPointerIndex < rightPointerIndex)
                    {
                        if (leftPointerValue == rightPointerValue)
                        {
                            leftPointerIndex++;
                        }
                        else
                        {
                            arrayToSort = switchValues(arrayToSort, leftPointerIndex, rightPointerIndex);
                            leftPointerValue = arrayToSort[leftPointerIndex];
                            rightPointerValue = arrayToSort[rightPointerIndex];
                        }
                    }
                }
                int[] leftArray = new int[rightPointerIndex + 1];
                for (int i = 0; i < rightPointerIndex + 1; i++)
                {
                    leftArray[i] = arrayToSort[i];
                }
                int[] rightArray = new int[arrayToSort.Length - rightPointerIndex - 1];
                for (int i = 0; i < arrayToSort.Length - rightPointerIndex - 1; i++)
                {
                    rightArray[i] = arrayToSort[i + rightPointerIndex + 1];
                }
                if (rightArray.Length <= 1) return leftArray;
                return appendArrays(quickSort(leftArray), quickSort(rightArray));
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
