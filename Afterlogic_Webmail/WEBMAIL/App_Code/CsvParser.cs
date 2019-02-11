using System.Collections;
using System.Data;

namespace WebMail
{
	/// <summary>
    /// This class provides methods supporting CSV importing of address book data
	/// </summary>
	public class CsvParser
	{
		private char _delimiter = ',';
		private string _csvText = string.Empty;

		public char Delimiter
		{
			get { return _delimiter; }
			set { _delimiter = value; }
		}

		public string CsvText
		{
			get { return _csvText; }
		}

		public static char DetermineDelimiter(string csvText)
		{
			bool inQuotes = false;
			for (int i = 0; i < csvText.Length; i++)
			{
				switch (csvText[i])
				{
					case '"':
						inQuotes = !inQuotes;
						break;
					case ';':
						if (!inQuotes) return ';';
						break;
					case ',':
						if (!inQuotes) return ',';
						break;
				}
			}
			return ',';
		}

		public CsvParser(string csvText, bool determineDelimiterAutomatically)
		{
			_csvText = csvText.Trim();
			if (determineDelimiterAutomatically)
			{
				_delimiter = DetermineDelimiter(_csvText);
			}
		}

		public DataTable Parse()
		{
			DataTable resultTable = new DataTable();
			bool inQuotes = false;
			bool endRowOccured = false;
			int fieldStartIndex = 0;
			string field = null;

			ArrayList csvRow = new ArrayList();
			int i = 0;
			while (fieldStartIndex < _csvText.Length)
			{
				if (i >= _csvText.Length)
				{
					field = _csvText.Substring(fieldStartIndex, _csvText.Length - fieldStartIndex);
					fieldStartIndex = i;
				}
				else
				{
					if (_csvText[i] == _delimiter)
					{
						if (!inQuotes)
						{
							field = _csvText.Substring(fieldStartIndex, i - fieldStartIndex);
							fieldStartIndex = i + 1;
						}
					}
					else if (_csvText[i] == '"')
					{
						inQuotes = !inQuotes;
					}
					else if (_csvText[i] == '\n')
					{
						if (!inQuotes)
						{
							endRowOccured = true;
							field = _csvText.Substring(fieldStartIndex, (i /*\r*/) - fieldStartIndex);
							fieldStartIndex = i + 1;
						}
					}
				}
				i++;

				if (field != null)
				{
					field = field.Trim();
					if (field.Length > 1)
					{
						if ((field[0] == '"') && (field[field.Length - 1] == '"'))
						{
							field = field.Substring(1, field.Length - 2);
						}
					}
					string fieldValue = field.Replace("\"\"", "\"");
					csvRow.Add(fieldValue);
					field = null;
				}
				if ((endRowOccured) || (i-1 == _csvText.Length))
				{
					endRowOccured = false;
					if (resultTable.Columns.Count == 0)
					{
						foreach (string columnName in csvRow)
						{
							resultTable.Columns.Add(columnName, typeof(string));
						}
					}
					else
					{
						if (csvRow.Count > 0)
						{
							if (resultTable.Columns.Count >= csvRow.Count)
							{
								resultTable.Rows.Add(csvRow.ToArray());
							}
						}
					}
					csvRow.Clear();
				}
			}

			return resultTable;
		}
	}
}
