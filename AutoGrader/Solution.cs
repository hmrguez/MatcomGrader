namespace Exam;

public static class Solution
{
    public static int Solve(int a)
    {
        var random = new Random();

        var inta = random.Next(3);
        switch (inta)
        {
            case 0:
                return 5;
            case 1:
                throw new ArgumentException();
            case 2:
                System.Threading.Thread.Sleep(10000);
                throw new Exception();
        }

        throw new Exception();
    }
}