using System.Text;

namespace Common;

public class MarkdownWriter
{
    private readonly string _filePath;
    private string[] _headers;
    private string _headerLine;
    private string _separatorLine;
    private readonly object _lock = new object();

    /// <summary>
    /// Initializes a new instance of the MarkdownWriter class.
    /// If the file exists, headers are read from the existing file.
    /// If the file does not exist, headers must be provided to create the file.
    /// </summary>
    /// <param name="filePath">The path to the markdown file.</param>
    /// <param name="headers">An array of header names for the table. Required if the file does not exist.</param>
    public MarkdownWriter(string filePath, string[] headers = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

        _filePath = filePath;

        if (File.Exists(_filePath))
        {
            ReadHeadersFromFile();
            if (_headers == null || _headers.Length == 0)
                throw new InvalidOperationException("Existing markdown file does not contain headers.");
        }
        else
        {
            if (headers == null || headers.Length == 0)
                throw new ArgumentException("Headers must be provided when creating a new markdown file.",
                    nameof(headers));

            _headers = headers;
            InitializeFile();
        }
    }

    /// <summary>
    /// Reads the headers from the existing markdown file.
    /// Assumes that the first non-empty line contains headers in markdown table format.
    /// </summary>
    private void ReadHeadersFromFile()
    {
        using (var reader = new StreamReader(_filePath, Encoding.UTF8))
        {
            string line;

            // Read lines until header line is found
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("|") && !string.IsNullOrWhiteSpace(line))
                {
                    _headerLine = line;
                    _headers = line.Trim('|').Split('|').Select(h => h.Trim()).ToArray();

                    // Read the separator line
                    line = reader.ReadLine();
                    if (line == null || !line.StartsWith("|"))
                        throw new InvalidOperationException("Invalid markdown table format: Missing separator line.");

                    _separatorLine = line;
                    return;
                }
            }

            throw new InvalidOperationException("No valid header line found in the markdown file.");
        }
    }

    /// <summary>
    /// Constructs the header and separator lines and writes them to the file.
    /// </summary>
    private void InitializeFile()
    {
        _headerLine = "| " + string.Join(" | ", _headers) + " |";
        _separatorLine = "|" + string.Join("|", _headers.Select(h => new string('-', h.Length + 2))) + "|";

        using (var writer = new StreamWriter(_filePath, false, Encoding.UTF8))
        {
            writer.WriteLine(_headerLine);
            writer.WriteLine(_separatorLine);
        }
    }

    /// <summary>
    /// Adds a new row to the markdown table.
    /// </summary>
    /// <param name="values">The values for each column in the row.</param>
    public void AddRow(params string[] values)
    {
        if (values == null)
            throw new ArgumentNullException(nameof(values));

        if (values.Length != _headers.Length)
            throw new ArgumentException(
                $"Number of values ({values.Length}) does not match number of headers ({_headers.Length}).",
                nameof(values));

        string row = "| " + string.Join(" | ", values.Select(v => v ?? string.Empty)) + " |";

        // Append the row to the file in a thread-safe manner
        lock (_lock)
        {
            using (var writer = new StreamWriter(_filePath, true, Encoding.UTF8))
            {
                writer.WriteLine(row);
            }
        }
    }

    /// <summary>
    /// Reads the entire markdown table from the file.
    /// </summary>
    /// <returns>The content of the markdown file as a string.</returns>
    public string ReadTable()
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException("The specified markdown file does not exist.", _filePath);

        return File.ReadAllText(_filePath);
    }

    /// <summary>
    /// Clears all data in the markdown table, keeping only the header and separator.
    /// </summary>
    public void ClearTable()
    {
        lock (_lock)
        {
            using (var writer = new StreamWriter(_filePath, false, Encoding.UTF8))
            {
                writer.WriteLine(_headerLine);
                writer.WriteLine(_separatorLine);
            }
        }
    }

    /// <summary>
    /// Gets the headers of the markdown table.
    /// </summary>
    public string[] Headers => _headers.ToArray();
}