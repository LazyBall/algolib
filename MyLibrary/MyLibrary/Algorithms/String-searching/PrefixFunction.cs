
namespace MyLibrary.Algorithms.StringSearch
{
    static class PrefixFunction
    {
        public static int[] Compute(string str)
        {
            var pi = new int[str.Length]; // значения префикс-функции
            pi[0] = 0; // для префикса из одного символа функция равна нулю
            int currentPosition = 0;

            for (int i = 1; i < str.Length; i++)
            {
                while ((currentPosition > 0) && (str[currentPosition] != str[i]))
                {
                    currentPosition = pi[currentPosition - 1];
                }

                if (str[currentPosition] == str[i])
                {
                    currentPosition++;
                }
                pi[i] = currentPosition;
            }

            return pi;
        }

        public static int[] BuildFromZFunction(int[] z)
        {
            var pi = new int[z.Length];

            for (int i = 1; i < z.Length; i++)
            {
                for (int j = z[i] - 1; j >= 0; j--)
                {
                    if (pi[i + j] > 0)
                    {
                        break;
                    }
                    else
                    {
                        pi[i + j] = j + 1;
                    }
                }
            }

            return pi;
        }
    }
}