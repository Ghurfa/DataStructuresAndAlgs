using System;

namespace CalculateFactorial
{
    class Program
    {
        static void Main(string[] args)
        {
            int factorialStep(int numSoFar, int numToMultiplyBy)
            {
                if (numToMultiplyBy == 1) return numSoFar;
                else return factorialStep(numSoFar * numToMultiplyBy, numToMultiplyBy - 1);
            }
            int factorial(int numberToCalculateFactorial)
            {
                return factorialStep(1, numberToCalculateFactorial);
            }
            Console.WriteLine("Num to calculate factorial of:");
            int number = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Factorial of {number}: {factorial(number)}");
            Console.ReadKey();
        }
    }
}
