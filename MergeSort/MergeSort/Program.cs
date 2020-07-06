using System;

namespace MergeSort
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
                    //Console.WriteLine($"Number: {newNumber}");
                }
            }
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
            void mergeSubArrays(ref int[] array, int firstArrayStartIndex, int firstArrayEndIndex, int secondArrayStartIndex, int secondArrayEndIndex)
            {
                int numsMoved = 0;
                for (int f = firstArrayStartIndex; f <= firstArrayEndIndex + numsMoved; f++)
                {
                    for (int s = secondArrayStartIndex + numsMoved; s <= secondArrayEndIndex; s++)
                    {
                        if (array[s] <= array[f])
                        {
                            moveValue(ref array, s, f);
                            numsMoved++;
                            f++;
                        }
                    }
                }
            }
            int[] mergeSort(ref int[] array, int subArraySize)
            {
                if (subArraySize >= array.Length)
                {
                    return array;
                }
                for (int i = 0; i < Math.Ceiling((decimal)array.Length / subArraySize / 2); i++)
                {
                    int firstArrayStartIndex = 2 * i * subArraySize;
                    int firstArrayEndIndex = firstArrayStartIndex + subArraySize - 1;
                    int secondArrayStartIndex = firstArrayEndIndex + 1;
                    int secondArrayEndIndex = secondArrayStartIndex + subArraySize - 1;
                    secondArrayEndIndex = secondArrayEndIndex > array.Length - 1 ? array.Length - 1 : secondArrayEndIndex;
                    mergeSubArrays(ref array, firstArrayStartIndex, firstArrayEndIndex, secondArrayStartIndex, secondArrayEndIndex);
                }
                return mergeSort(ref array, subArraySize * 2);
            }
            NumbersToSort = mergeSort(ref NumbersToSort, 1);
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
