using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

public class BruteForceDecryptor
{
    public static string BruteForceDecrypt(string encryptedPassword, byte[] key, byte[] iv, out TimeSpan elapsedTime)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string result = null;
        bool found = false;

        void BruteForceThread(string prefix)
        {
            char[] charset = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            StringBuilder attempt = new StringBuilder(prefix);

            while (!found && Increment(attempt, charset.Length))
            {
                string attemptStr = attempt.ToString();
                try
                {
                    string decrypted = PasswordManager.DecryptPassword(encryptedPassword, key, iv);
                    if (decrypted == attemptStr)
                    {
                        result = attemptStr;
                        found = true;
                    }
                }
                catch
                {
                    // Ignore exceptions from incorrect decrypt attempts
                }
            }
        }

        int threadCount = 4;
        Thread[] threads = new Thread[threadCount];
        for (int i = 0; i < threadCount; i++)
        {
            string prefix = ((char)('a' + i)).ToString();
            threads[i] = new Thread(() => BruteForceThread(prefix));
            threads[i].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        stopwatch.Stop();
        elapsedTime = stopwatch.Elapsed;
        return result;
    }

    private static bool Increment(StringBuilder attempt, int baseLength)
    {
        for (int i = attempt.Length - 1; i >= 0; i--)
        {
            if (attempt[i] < 'z')
            {
                attempt[i]++;
                return true;
            }
            attempt[i] = 'a';
        }

        attempt.Insert(0, 'a');
        return true;
    }
}
