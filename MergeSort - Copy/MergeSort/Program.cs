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
                    Console.WriteLine($"Number: {newNumber}");
                }
            }
            int[] mergeArrays(int[] firstArray, int[] secondArray)
            {
                int[] outputArray = new int[firstArray.Length + secondArray.Length];
                for (int i = 0; i < firstArray.Length; i++)
                {
                    outputArray[i] = firstArray[i];
                }
                void insertValueAtIndex(ref int[] array, int value, int index)
                {
                    for (int i = index; i < array.Length - index; i++)
                    {
                        array[i + 1] = array[i];
                    }
                    array[index] = value;
                }
                for (int i = 0; i < secondArray.Length; i++)
                {
                    for (int j = 0; j < firstArray.Length; j++)
                    {
                        if (firstArray[j] <= secondArray[i])
                        {
                            insertValueAtIndex(ref outputArray, secondArray[i], j + 1);
                        }
                    }
                }
                /*void moveValue(ref int[] array, int currentIndex, int newIndex)
                {
                    int value = array[currentIndex];
                    int moveDirection = Math.Sign(newIndex - currentIndex);
                    for (int i = currentIndex; i * moveDirection < newIndex * moveDirection; i += moveDirection)
                    {
                        array[i] = array[i + moveDirection];
                    }
                    array[newIndex] = value;
                }*/
                return outputArray;
            }
            void mergeSubArrays(ref int[] array, int firstArrayStartIndex, int firstArrayEndIndex, int secondArrayStartIndex, int secondArrayEndIndex)
            {
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
                int numsMoved = 0;
                for (int f = firstArrayStartIndex; f <= firstArrayEndIndex + numsMoved; f++)
                {
                    for (int s = secondArrayStartIndex + numsMoved; s <= secondArrayEndIndex; s++)
                    {
                        if (array[s] <= array[f])
                        {
                            moveValue(ref array, s, f);
                            numsMoved++;
                            break;
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
                for (int i = 0; i < Math.Floor((decimal)array.Length / subArraySize / 2); i++)
                {
                    int firstArrayStartIndex = 2 * i * subArraySize;
                    int firstArrayEndIndex = firstArrayStartIndex + subArraySize - 1;
                    int secondArrayStartIndex = firstArrayEndIndex + 1;
                    int secondArrayEndIndex = secondArrayStartIndex + subArraySize - 1;
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
