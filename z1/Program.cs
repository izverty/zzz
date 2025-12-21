using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "input.txt";
        StreamWriter writer = new StreamWriter(path);
        writer.WriteLine(3);
        writer.WriteLine("1 0 0");
        writer.WriteLine("0 1 0");
        writer.WriteLine("0 0 1");

        writer.WriteLine("2 2 2");

        writer.Close();

        StreamReader reader = new StreamReader(path);

        int n = int.Parse(reader.ReadLine());

        double[,] g = new double[n, n];

        for (int i = 0; i < n; i++)
        {
            string[] parts = reader.ReadLine().Split(' ');
            for (int j = 0; j < n; j++)
            {
                g[i, j] = double.Parse(parts[j]);
            }
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (g[i, j] != g[j, i])
                {
                    Console.WriteLine("Матрица не симметрична!");
                    reader.Close();
                    return;
                }
            }
        }

        double[] v = new double[n];

        string[] vecParts = reader.ReadLine().Split(' ');
        for (int i = 0; i < n; i++)
        {
            v[i] = double.Parse(vecParts[i]);
        }

        reader.Close();
        double sum = 0.0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                sum += g[i, j] * v[i] * v[j];
            }
        }

        double length = Math.Sqrt(sum);

        Console.WriteLine("Длина вектора: " + length);
    }
}