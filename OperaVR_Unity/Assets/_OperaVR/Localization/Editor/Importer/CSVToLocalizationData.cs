using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperaVR
{
    public static class CSVToLocalizationData
    {
        public static List<string[]> ParseCsv(string csv)
        {
            var result = new List<string[]>();
            var inQuotes = false;
            var field = new StringBuilder();
            var row = new List<string>();

            for (int i = 0; i < csv.Length; i++)
            {
                char c = csv[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < csv.Length && csv[i + 1] == '"')
                        {
                            field.Append('"'); // Escaped quote
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        field.Append(c);
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else if (c == ',')
                    {
                        row.Add(field.ToString());
                        field.Clear();
                    }
                    else if (c == '\r')
                    {
                        // skip
                    }
                    else if (c == '\n')
                    {
                        row.Add(field.ToString());
                        field.Clear();
                        if (row.Count > 0 || !row.Any(r => r.Length > 0))
                        {
                            result.Add(row.ToArray());
                        }
                        row.Clear();
                    }
                    else
                    {
                        field.Append(c);
                    }
                }
            }

            // Add final row
            if (field.Length > 0 || row.Count > 0)
            {
                row.Add(field.ToString());
                result.Add(row.ToArray());
            }

            return result;
        }
    }
}
