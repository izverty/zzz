using System;

struct ComplexNumber
{
    public double Re; 
    public double Im; 
    public void Create(double re, double im)
    {
        Re = re;
        Im = im;
    }
    public void Add(ref ComplexNumber other)
    {
        Re = Re + other.Re;
        Im = Im + other.Im;
    }
    public void Subtract(ref ComplexNumber other)
    {
        Re = Re - other.Re;
        Im = Im - other.Im;
    }

    public void Multiply(ref ComplexNumber other)
    {
        double newRe = Re * other.Re - Im * other.Im;
        double newIm = Re * other.Im + Im * other.Re;
        Re = newRe;
        Im = newIm;
    }
    public void Divide(ref ComplexNumber other)
    {
        double denom = other.Re * other.Re + other.Im * other.Im;

        double newRe = (Re * other.Re + Im * other.Im) / denom;
        double newIm = (Im * other.Re - Re * other.Im) / denom;

        Re = newRe;
        Im = newIm;
    }
    public double Module()
    {
        return Math.Sqrt(Re * Re + Im * Im);
    }
    public double Argument()
    {
        return Math.Atan2(Im, Re);
    }
    public double GetRe()
    {
        return Re;
    }
    public double GetIm()
    {
        return Im;
    }
    public void Print()
    {
        Console.WriteLine("Текущее число: " + Re + " + " + Im + "i");
    }
}

class Program
{
    static void Main()
    {
        ComplexNumber z = new ComplexNumber();
        z.Create(0, 0);

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("C - ввести комплексное число");
            Console.WriteLine("A - сложение");
            Console.WriteLine("S - вычитание");
            Console.WriteLine("M - умножение");
            Console.WriteLine("D - деление");
            Console.WriteLine("R - вещественная часть");
            Console.WriteLine("I - мнимая часть");
            Console.WriteLine("O - модуль");
            Console.WriteLine("G - аргумент");
            Console.WriteLine("P - вывести число");
            Console.WriteLine("Q - выход");

            Console.Write("Введите команду: ");
            char cmd = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (cmd)
            {
                case 'C':
                case 'c':
                    Console.Write("Введите вещественную часть: ");
                    double re = double.Parse(Console.ReadLine());
                    Console.Write("Введите мнимую часть: ");
                    double im = double.Parse(Console.ReadLine());
                    z.Create(re, im);
                    break;

                case 'A':
                case 'a':
                    ComplexNumber a = ReadSecond();
                    z.Add(ref a);
                    break;

                case 'S':
                case 's':
                    ComplexNumber s = ReadSecond();
                    z.Subtract(ref s);
                    break;

                case 'M':
                case 'm':
                    ComplexNumber m = ReadSecond();
                    z.Multiply(ref m);
                    break;

                case 'D':
                case 'd':
                    ComplexNumber d = ReadSecond();
                    z.Divide(ref d);
                    break;

                case 'R':
                case 'r':
                    Console.WriteLine("Вещественная часть: " + z.GetRe());
                    break;

                case 'I':
                case 'i':
                    Console.WriteLine("Мнимая часть: " + z.GetIm());
                    break;

                case 'O':
                case 'o':
                    Console.WriteLine("Модуль: " + z.Module());
                    break;

                case 'G':
                case 'g':
                    Console.WriteLine("Аргумент: " + z.Argument());
                    break;

                case 'P':
                case 'p':
                    z.Print();
                    break;

                case 'Q':
                case 'q':
                    return;

                default:
                    Console.WriteLine("неизвестная команда");
                    break;
            }
        }
    }
    static ComplexNumber ReadSecond()
    {
        ComplexNumber temp = new ComplexNumber();

        Console.Write("Введите вещественную часть: ");
        double re = double.Parse(Console.ReadLine());
        Console.Write("Введите мнимую часть: ");
        double im = double.Parse(Console.ReadLine());

        temp.Create(re, im);
        return temp;
    }
}

