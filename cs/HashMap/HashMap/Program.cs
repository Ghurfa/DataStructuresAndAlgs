using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class Program
    {
        static int[] valuesFromString(string input)
        {
            int[] output = new int[0];
            int value = 0;
            int negativeMultiplier = 1;
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    value = 10 * value + negativeMultiplier * Convert.ToInt32(input[i].ToString());
                }
                else if (input[i] == '-')
                {
                    negativeMultiplier = -negativeMultiplier;
                }
                else if (value != 0)
                {
                    if (output.Length == 0) output = new int[1] { value };
                    else
                    {
                        int[] oldValues = output;
                        output = new int[output.Length + 1];
                        for (int j = 0; j < oldValues.Length; j++)
                        {
                            output[j] = oldValues[j];
                        }
                        output[oldValues.Length] = value;
                    }
                    value = 0;
                    negativeMultiplier = 1;
                }
            }
            if (value != 0)
            {
                if (output.Length == 0) output = new int[1] { value };
                else
                {
                    int[] oldValues = output;
                    output = new int[output.Length + 1];
                    for (int j = 0; j < oldValues.Length; j++)
                    {
                        output[j] = oldValues[j];
                    }
                    output[oldValues.Length] = value;
                }
            }
            return output;
        }
        static void Main(string[] args)
        {
            HashMap<int, int> hashMap = new HashMap<int, int>();
            Console.WriteLine("Type Command: Insert [ins], Delete [del], Contains Key-Value pair [ckv], Contains key [con], Clear [clr], Print [prn]");
            while (true)
            {
                string command = Console.ReadLine();
                if (command.Length < 3) continue;
                int number;
                int key;
                switch (command.ToLower().Substring(0, 3))
                {
                    case "ins":
                        Console.WriteLine("Type key to insert at:");
                        try
                        {
                            key = Convert.ToInt32(Console.ReadLine());
                        }
                        catch(FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        Console.WriteLine("Type value to insert:");
                        try
                        {
                            number = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        hashMap.Add(key, number);
                        Console.WriteLine($"Inserted value {number}");
                        break;
                    case "del":
                        if (command.Length > 4)
                        {
                            int[] keys = valuesFromString(command.Substring(4, command.Length - 4));
                            if (keys == null) continue;
                            for (int i = 0; i < keys.Length; i++)
                                Console.WriteLine($"Deleting key {keys[i]}: {hashMap.Remove(keys[i]).ToString()}");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Type key to delete:");
                            try
                            {
                                key = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Invalid Input");
                                continue;
                            }
                            Console.WriteLine($"Deleting key {key}: {hashMap.Remove(key).ToString()}");
                        }
                        break;
                    case "ckv":
                        Console.WriteLine("Type key:");
                        try
                        {
                            key = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        Console.WriteLine("Type value:");
                        try
                        {
                            number = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        bool contains = hashMap.Contains(new KeyValuePair<int, int>(key, number));
                        Console.WriteLine($"Hash map {(contains ? "contains" : "does not contain")} the key-value pair {key}-{number}");
                        break;
                    case "con":
                        Console.WriteLine("Type key:");
                        try
                        {
                            key = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        contains = hashMap.ContainsKey(key);
                        Console.WriteLine($"Hash map {(contains ? "contains" : "does not contain")} the key {key}");
                        break;
                    case "clr":
                        hashMap.Clear();
                        Console.WriteLine("Cleared Hash Map");
                        break;
                    case "prn":
                        hashMap.PrintMap();
                        break;
                }
            }
        }
    }
}
