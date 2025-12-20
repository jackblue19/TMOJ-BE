using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;

enum Verdict { AC, WA, TLE, RE, CE }

class program
{
    static string CPP_COMPILER = "g++";
    static string SUBMISSION_CPP = "D:\\FPT\\Spring_2026\\ref\\Solution_AC.cpp";  //  ac
    //static string SUBMISSION_CPP = "D:\\FPT\\Spring_2026\\ref\\Code_WA.cpp";  // wa
    //static string SUBMISSION_CPP = "D:\\FPT\\Spring_2026\\ref\\Code_TLE.cpp";    //  tle
    static string TEST_ROOT = "D:\\FPT\\Spring_2026\\ref\\icpc_prob_006\\icpc_prob_006";
    static string PROBLEM_NAME = "icpc_prob_006";

    //  Main
    static async Task Main()
    {
        if ( !File.Exists(SUBMISSION_CPP) )
        {
            Console.WriteLine("code is not found");
            return;
        }

        if ( !Directory.Exists(TEST_ROOT) )
        {
            Console.WriteLine("sai path");
            return;
        }

        var workDir = CreateWorkDir();
        var exePath = Path.Combine(workDir , "solution.exe");

        var ce = await Compile(SUBMISSION_CPP , exePath);
        if ( ce != null )
        {
            Console.WriteLine("ce ???????");
            Console.WriteLine(ce);
            return;
        }

        var testFolders = Directory.GetDirectories(TEST_ROOT)
                                   .OrderBy(x => x)
                                   .ToList();

        int passed = 0;
        foreach ( var folder in testFolders )
        {
            var id = Path.GetFileName(folder);

            var inp = Path.Combine(folder , $"{PROBLEM_NAME}.inp");
            var outp = Path.Combine(folder , $"{PROBLEM_NAME}.out");

            if ( !File.Exists(inp) || !File.Exists(outp) )
            {
                Console.WriteLine($"Skip {id} (missing inp/out)");
                continue;
            }

            var input = await File.ReadAllTextAsync(inp);
            var expected = await File.ReadAllTextAsync(outp);

            var result = await Run(exePath , input , 1000);

            if ( result.verdict == Verdict.TLE )
            {
                Console.WriteLine($"TLE at test {id}");
                return;
            }

            if ( result.verdict == Verdict.RE )
            {
                Console.WriteLine($"RE at test {id}");
                Console.WriteLine(result.stderr);
                return;
            }

            if ( Normalize(result.stdout) != Normalize(expected) )
            {
                Console.WriteLine($"WA at test {id}");
                Console.WriteLine("Expected:");
                Console.WriteLine(expected);
                Console.WriteLine("Got:");
                Console.WriteLine(result.stdout);
                return;
            }

            Console.WriteLine($"Test {id}: AC ({result.timeMs} ms)");
            passed++;
        }
        Console.WriteLine($"\nAC – Passed {passed}/{testFolders.Count} tests");
    }

    //  Helper method

    static string CreateWorkDir()
    {
        var dir = Path.Combine(Path.GetTempPath() , "judge_" + Guid.NewGuid());
        Directory.CreateDirectory(dir);
        return dir;
    }

    static string Normalize(string input)
    {
        return input.Replace("\r\n" , "\n")
                    .Replace("\r" , "\n")
                    .TrimEnd();
    }

    static async Task<(Verdict verdict, string stdout, string stderr, long timeMs)> Run(
    string exe , string input , int timeLimitMs)
    {
        var psi = new ProcessStartInfo
        {
            FileName = exe ,
            RedirectStandardInput = true ,
            RedirectStandardOutput = true ,
            RedirectStandardError = true ,
            UseShellExecute = false ,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = psi };

        var stdout = new StringBuilder();
        var stderr = new StringBuilder();

        process.OutputDataReceived += (_ , e) =>
        {
            if ( e.Data != null ) stdout.AppendLine(e.Data);
        };
        process.ErrorDataReceived += (_ , e) =>
        {
            if ( e.Data != null ) stderr.AppendLine(e.Data);
        };

        var sw = Stopwatch.StartNew();
        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.StandardInput.WriteAsync(input);
        process.StandardInput.Close();

        var exitTask = process.WaitForExitAsync();
        var delayTask = Task.Delay(timeLimitMs);

        var finished = await Task.WhenAny(exitTask , delayTask);

        if ( finished == delayTask )
        {
            try { process.Kill(entireProcessTree: true); } catch { }
            return (Verdict.TLE, "", "", sw.ElapsedMilliseconds);
        }

        await exitTask; // ensure exited
        sw.Stop();

        if ( process.ExitCode != 0 )
        {
            return (Verdict.RE, stdout.ToString(), stderr.ToString(), sw.ElapsedMilliseconds);
        }

        return (Verdict.AC, stdout.ToString(), stderr.ToString(), sw.ElapsedMilliseconds);
    }

    static async Task<string?> Compile(string cpp , string exe)
    {
        var psi = new ProcessStartInfo
        {
            FileName = CPP_COMPILER ,
            Arguments = $"-std=c++17 -O2 \"{cpp}\" -o \"{exe}\"" ,
            RedirectStandardError = true ,
            RedirectStandardOutput = true ,
            UseShellExecute = false
        };

        var p = Process.Start(psi)!;
        await p.WaitForExitAsync();

        if ( p.ExitCode != 0 )
            return await p.StandardError.ReadToEndAsync();

        return null;
    }
}