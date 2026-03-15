using System;

while (true)
{
    Console.WriteLine("Введите первую строку (exit для выхода):");
    string s1 = Console.ReadLine();

    if (s1 == "exit")
    {
        break;
    }

    Console.WriteLine("Введите вторую строку:");
    string s2 = Console.ReadLine();

    int dist = Rasstoyanie(s1, s2);

    Console.WriteLine("'" + s1 + "','" + s2 + "' -> " + dist);
}

int Rasstoyanie(string str1Param, string str2Param)
{
    int len1 = str1Param.Length;
    int len2 = str2Param.Length;

    if (len1 == 0) return len2;
    if (len2 == 0) return len1;

    string str1 = str1Param.ToUpper();
    string str2 = str2Param.ToUpper();

    int[,] mat = new int[len1 + 1, len2 + 1];

    for (int i = 0; i <= len1; i++)
    {
        mat[i, 0] = i;
    }
    for (int j = 0; j <= len2; j++)
    {
        mat[0, j] = j;
    }

    for (int i = 1; i <= len1; i++)
    {
        for (int j = 1; j <= len2; j++)
        {
            int cost = 0;
            if (str1[i - 1] != str2[j - 1])
            {
                cost = 1;
            }

            int var1 = mat[i - 1, j] + 1;
            int var2 = mat[i, j - 1] + 1;
            int var3 = mat[i - 1, j - 1] + cost;

            int minimum = var1;
            if (var2 < minimum) minimum = var2;
            if (var3 < minimum) minimum = var3;

            mat[i, j] = minimum;

            if (i > 1 && j > 1 && str1[i - 1] == str2[j - 2] && str1[i - 2] == str2[j - 1])
            {
                int var4 = mat[i - 2, j - 2] + cost;
                if (var4 < mat[i, j])
                {
                    mat[i, j] = var4;
                }
            }
        }
    }

    return mat[len1, len2];
}