using DeNovoGreedy;

string input = "a_long_long_long_time";

//var s = new DenovoSuperstring(new string[] { "AAA", "AAB", "ABB", "BBB", "BBA" }, 1);
int c = 0;
int N = 10000;
for (int i = 0; i < N; i++)
{
    var s = new DenovoSuperstring(input, 8, 1);

    string super = s.FindSuperstring(false);

    if (input == super)
    {
        Console.WriteLine(super + "\t OK");
        c++;
    }
    else
    {
        Console.WriteLine(super);
    }
}
Console.WriteLine($"Uspesnost: {c / (float)N * 100}%");