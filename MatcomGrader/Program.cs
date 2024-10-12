using System.Diagnostics;
using Common;

var basePath = Path.Combine(Directory.GetCurrentDirectory());
var solutionsPath = Path.Combine(basePath, "Solutions");
var testPath = Path.Combine(basePath, "AutoGrader");
var baseFilePath = Path.Combine(solutionsPath, "_base.cs");
var resultPath = Path.Combine(basePath, Constants.ResultFileName);

string[] headers = Constants.GlobalHeaders;

if (File.Exists(resultPath))
{
    File.Delete(resultPath);
}

File.Delete(Path.Combine(testPath, "Solution.cs"));

foreach (var filePath in Directory.GetFiles(solutionsPath))
{
    var fileName = Path.GetFileName(filePath);
    var studentName = Utils.StudentNameFromFile(fileName);


    if (fileName.StartsWith("_base.cs") || fileName.StartsWith('.'))
        continue;


    var destinationPath = Path.Combine(testPath, fileName);
    File.Copy(filePath, destinationPath, overwrite: true);

    var psi = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = "test",
        WorkingDirectory = testPath,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false
    };
    psi.EnvironmentVariables["CURRENT_SOLUTION_FILE"] = studentName;
    psi.EnvironmentVariables["GLOBAL"] = "FALSE";
    psi.EnvironmentVariables["NUMBER_OF_CASES"] = "4";

    var process = Process.Start(psi);
    process!.WaitForExit();

    // Handle output or errors if needed
    var output = process.StandardOutput.ReadToEnd();
    // var error = process.StandardError.ReadToEnd();


    Console.WriteLine("Student: " + studentName + "\n" + output);

    // Delete the copied file after testing
    File.Delete(destinationPath);

    var mdWriter = new MarkdownWriter(resultPath, headers);

    var lastRow = mdWriter.ReadLastRow();
    if (!lastRow.Contains(studentName))
    {
        var toWrite = headers.Select(_ => "-").ToArray();
        toWrite[0] = studentName;
        toWrite[1] = "🔴";
        mdWriter.AddRow(toWrite);
    }
}


File.Copy(baseFilePath, Path.Combine(testPath, "Solution.cs"), overwrite: true);