using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StaxelAPI
{
	public class INI
	{
		public class Entry
		{
			public class Bool : Entry
			{
				readonly bool defaultValue;

				public Bool(INI ini, string section, string key, bool defaultValue) : base(ini, section, key)
				{
					this.defaultValue = defaultValue;

					if (!ini.Has(section, key))
						ini[section, key] = defaultValue.ToString();

					ini.Save();
				}

				public new bool Value
				{
					get
					{
						if (ini.Has(section, key) && bool.TryParse(ini[section, key], out bool result))
							return result;

						return defaultValue;
					}
					set
					{
						base.Value = value;
					}
				}
			}

			public class Int : Entry
			{
				readonly int defaultValue;

				public Int(INI ini, string section, string key, int defaultValue) : base(ini, section, key)
				{
					this.defaultValue = defaultValue;

					if (!ini.Has(section, key))
						ini[section, key] = defaultValue.ToString();

					ini.Save();
				}

				public new int Value
				{
					get
					{
						if (ini.Has(section, key) && int.TryParse(ini[section, key], out int result))
							return result;

						return defaultValue;
					}
					set
					{
						base.Value = value;
					}
				}
			}

			public class Float : Entry
			{
				readonly float defaultValue;

				public Float(INI ini, string section, string key, float defaultValue) : base(ini, section, key)
				{
					this.defaultValue = defaultValue;

					if (!ini.Has(section, key))
						ini[section, key] = defaultValue.ToString();

					ini.Save();
				}

				public new float Value
				{
					get
					{
						if (ini.Has(section, key) && float.TryParse(ini[section, key], out float result))
							return result;

						return defaultValue;
					}
					set
					{
						base.Value = value;
					}
				}
			}

			public class String : Entry
			{
				readonly string defaultValue;

				public String(INI ini, string section, string key, string defaultValue) : base(ini, section, key)
				{
					this.defaultValue = defaultValue;

					if (!ini.Has(section, key))
						ini[section, key] = defaultValue.ToString();

					ini.Save();
				}

				public new string Value
				{
					get
					{
						if (ini.Has(section, key))
							return ini[section, key];

						return defaultValue;
					}
					set
					{
						base.Value = value;
					}
				}
			}

			readonly INI ini;
			readonly string section;
			readonly string key;

			public object Value
			{
				set
				{
					ini[section, key] = value.ToString();

					ini.Save();
				}
			}

			public Entry(INI ini, string section, string key)
			{
				this.ini = ini;
				this.section = section;
				this.key = key;
			}
		}

		const string SECTION_START = "[";
		const string SECTION_END = "]";
		const string KV = "=";
		const string COMMENT = "#";
		const string NEWLINE = "\r\n|\r|\n";

		readonly Dictionary<string, Dictionary<string, string>> list =
			new Dictionary<string, Dictionary<string, string>>();
		readonly string path;
		DateTime lastTaken;

		public delegate void UpdateListener();

		public event UpdateListener OnUpdate;

		public string this[string section, string key]
		{
			get
			{
				if (!list.ContainsKey(section) ||
					!list[section].ContainsKey(key))
					return null;

				return list[section][key];
			}
			set
			{
				if (!list.ContainsKey(section))
					list[section] = new Dictionary<string, string>();

				if (key != null)
					list[section][key] = value;
				else if (list[section].ContainsKey(key))
					list[section].Remove(key);
			}
		}

		public INI(string path)
		{
			this.path = path;

			Load();
		}

		public bool Has(string section)
		{
			return list.ContainsKey(section);
		}

		public bool Has(string section, string key)
		{
			return Has(section) && list[section].ContainsKey(key);
		}

		public Entry.Bool Bool(string section, string key, bool defaultValue)
		{
			return new Entry.Bool(this, section, key, defaultValue);
		}

		public Entry.Float Float(string section, string key, float defaultValue)
		{
			return new Entry.Float(this, section, key, defaultValue);
		}

		public Entry.Int Int(string section, string key, int defaultValue)
		{
			return new Entry.Int(this, section, key, defaultValue);
		}

		public Entry.String String(string section, string key, string defaultValue)
		{
			return new Entry.String(this, section, key, defaultValue);
		}

		internal void Update()
		{
			if (Load())
				OnUpdate?.Invoke();
		}

		bool Load()
		{
			try
			{
				if (path == null || !File.Exists(path))
					return false;

				DateTime nextLastTaken = File.GetLastWriteTime(path);

				if (lastTaken == nextLastTaken)
					return false;

				string data = File.ReadAllText(path);
				lastTaken = nextLastTaken;

				list.Clear();

				if (data == null || data.Length == 0)
					return false;

				string section = "";
				int n;

				foreach (string line in Regex.Split(data, NEWLINE))
					if (line.StartsWith(SECTION_START) && line.EndsWith(SECTION_END))
						section = line.Substring(1, line.Length - 2);
					else if (!line.StartsWith(COMMENT) && (n = line.IndexOf(KV)) != -1)
						this[section, line.Substring(0, n).Trim()] = line.Substring(n + 1).Trim();

				return true;
			}
			catch
			{
				return false;
			}
		}

		bool Save()
		{
			try
			{
				string directory = Path.GetDirectoryName(path);

				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				string data = "";

				foreach (KeyValuePair<string, Dictionary<string, string>> pair0 in list)
				{
					data += $"[{pair0.Key}]\n";

					foreach (KeyValuePair<string, string> pair1 in pair0.Value)
						data += $"{pair1.Key} = {pair1.Value}\n";
				}

				File.WriteAllText(path, data);

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
