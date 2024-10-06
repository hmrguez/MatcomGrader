using System.Diagnostics;
using Common;

var basePath = Path.Combine(Directory.GetCurrentDirectory());
var solutionsPath = Path.Combine(basePath, "Solutions");
var testPath = Path.Combine(basePath, "AutoGrader");
var baseFilePath = Path.Combine(solutionsPath, "_base.cs");
var resultPath = Path.Combine(basePath, "results.md");

string[] headers = ["Name", "Passed", "Wrong", "Exceptions", "Timeout"];

var mdWriter = new MarkdownWriter(resultPath, headers);

mdWriter.ClearTable();

File.Delete(Path.Combine(testPath, "Solution.cs"));

foreach (var filePath in Directory.GetFiles(solutionsPath))
{
    var fileName = Path.GetFileName(filePath);

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
    psi.EnvironmentVariables["CURRENT_SOLUTION_FILE"] = fileName;

    var process = Process.Start(psi);
    process.WaitForExit();

    // Handle output or errors if needed
    var output = process.StandardOutput.ReadToEnd();
    var error = process.StandardError.ReadToEnd();

    Console.WriteLine("Student: " + fileName + "\n" +  output);

    // Delete the copied file after testing
    File.Delete(destinationPath);

    var lastRow = mdWriter.ReadLastRow();
    if (!lastRow.Contains(fileName))
    {
        var toWrite = headers.Select(_ => "-").ToArray();
        toWrite[0] = fileName;
        mdWriter.AddRow(toWrite);
    }
}


File.Copy(baseFilePath, Path.Combine(testPath, "Solution.cs"), overwrite: true);