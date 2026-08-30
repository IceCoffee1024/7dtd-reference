using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x02000A2A RID: 2602
public sealed class SaveDataPrefsFile : ISaveDataPrefs
{
	// Token: 0x06004E9A RID: 20122 RVA: 0x001DE4D0 File Offset: 0x001DC6D0
	public SaveDataPrefsFile(string filePath)
	{
		foreach (PrefType prefType in EnumUtils.Values<PrefType>())
		{
			if (!SaveDataPrefsFile.PrefTypeMapping.ContainsKey(prefType))
			{
				throw new KeyNotFoundException(string.Format("Expected {0} to have key '{1}'.", "PrefTypeMapping", prefType));
			}
		}
		this.m_storageFilePath = filePath;
		this.m_storage = new Dictionary<string, SaveDataPrefsFile.Pref>();
		this.Load();
	}

	// Token: 0x06004E9B RID: 20123 RVA: 0x001DE568 File Offset: 0x001DC768
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static void Escape(TextWriter writer, ReadOnlySpan<char> raw, bool ignoreSeparator)
	{
		bool flag = false;
		ReadOnlySpan<char> readOnlySpan = raw;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			char c = (char)(*readOnlySpan[i]);
			if ((!ignoreSeparator || c != '=') && SaveDataPrefsFile.EscapeMapping.ContainsKey(c))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			writer.Write(raw);
			return;
		}
		readOnlySpan = raw;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			char c2 = (char)(*readOnlySpan[i]);
			char c3;
			if ((ignoreSeparator && c2 == '=') || !SaveDataPrefsFile.EscapeMapping.TryGetValue(c2, out c3))
			{
				writer.Write(c2);
			}
			else
			{
				writer.Write('\\');
				writer.Write(c3);
			}
		}
	}

	// Token: 0x06004E9C RID: 20124 RVA: 0x001DE60C File Offset: 0x001DC80C
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static void Unescape(StringBuilder builder, ReadOnlySpan<char> escaped)
	{
		if (escaped.IndexOf('\\') < 0)
		{
			builder.Append(escaped);
			return;
		}
		bool flag = false;
		int num = -1;
		ReadOnlySpan<char> readOnlySpan = escaped;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			char c = (char)(*readOnlySpan[i]);
			num++;
			if (flag)
			{
				flag = false;
				char c2;
				if (!SaveDataPrefsFile.UnescapeMapping.TryGetValue(c, out c2))
				{
					Log.Warning(string.Format("Unexpected character after escape prefix at offset {0} (will be taken as-is): {1}", num, c));
					builder.Append(c);
				}
				builder.Append(c2);
			}
			else if (c == '\\')
			{
				flag = true;
			}
			else
			{
				builder.Append(c);
			}
		}
	}

	// Token: 0x06004E9D RID: 20125 RVA: 0x001DE6AC File Offset: 0x001DC8AC
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static int IndexOfFirstUnescapedSeparator(ReadOnlySpan<char> search)
	{
		bool flag = false;
		int num = -1;
		ReadOnlySpan<char> readOnlySpan = search;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			char c = (char)(*readOnlySpan[i]);
			num++;
			if (flag)
			{
				flag = false;
			}
			else if (c == '\\')
			{
				flag = true;
			}
			else if (c == '=')
			{
				return num;
			}
		}
		return -1;
	}

	// Token: 0x06004E9E RID: 20126 RVA: 0x001DE6FC File Offset: 0x001DC8FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveInternal()
	{
		object storageLock = this.m_storageLock;
		lock (storageLock)
		{
			if (this.m_dirty)
			{
				try
				{
					string directoryName = Path.GetDirectoryName(this.m_storageFilePath);
					if (!SdDirectory.Exists(directoryName))
					{
						SdDirectory.CreateDirectory(directoryName);
					}
					using (StreamWriter streamWriter = SdFile.CreateText(this.m_storageFilePath))
					{
						int num = 0;
						foreach (KeyValuePair<string, SaveDataPrefsFile.Pref> keyValuePair in this.m_storage)
						{
							string text;
							SaveDataPrefsFile.Pref pref;
							keyValuePair.Deconstruct(out text, out pref);
							string text2 = text;
							SaveDataPrefsFile.Pref pref2 = pref;
							string text3;
							if (!pref2.TryToString(out text3))
							{
								Log.Out(string.Format("[{0}] Failed to convert pref '{1}' of type {2} to a string representation.", "SaveDataPrefsFile", text2, pref2.Type));
							}
							else
							{
								SaveDataPrefsFile.Escape(streamWriter, text2, false);
								streamWriter.Write('=');
								SaveDataPrefsFile.Escape(streamWriter, text3, true);
								streamWriter.WriteLine();
								num++;
							}
						}
						this.m_dirty = false;
						Log.Out(string.Format("[{0}] Saved {1} player pref(s) to: {2}", "SaveDataPrefsFile", num, this.m_storageFilePath));
					}
				}
				catch (IOException ex)
				{
					Log.Error("[SaveDataPrefsFile] Failed to Save: " + ex.Message);
					Log.Exception(ex);
				}
			}
		}
	}

	// Token: 0x06004E9F RID: 20127 RVA: 0x001DE8C0 File Offset: 0x001DCAC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void LoadInternal()
	{
		object storageLock = this.m_storageLock;
		lock (storageLock)
		{
			this.m_storage.Clear();
			if (SdFile.Exists(this.m_storageFilePath))
			{
				try
				{
					using (StreamReader streamReader = SdFile.OpenText(this.m_storageFilePath))
					{
						StringBuilder stringBuilder = new StringBuilder();
						int num = 0;
						int num2 = 0;
						for (;;)
						{
							string text = streamReader.ReadLine();
							if (text == null)
							{
								break;
							}
							num2++;
							ReadOnlySpan<char> readOnlySpan = text;
							int num3 = SaveDataPrefsFile.IndexOfFirstUnescapedSeparator(readOnlySpan);
							if (num3 < 0)
							{
								Log.Error(string.Format("[{0}] Skipping line {1} since is missing unescaped separator '{2}'. Contents: {3}", new object[] { "SaveDataPrefsFile", num2, '=', text }));
							}
							else
							{
								StringBuilder stringBuilder2 = stringBuilder;
								ReadOnlySpan<char> readOnlySpan2 = readOnlySpan;
								SaveDataPrefsFile.Unescape(stringBuilder2, readOnlySpan2.Slice(0, num3));
								string text2 = stringBuilder.ToString();
								stringBuilder.Clear();
								StringBuilder stringBuilder3 = stringBuilder;
								readOnlySpan2 = readOnlySpan;
								int num4 = num3 + 1;
								SaveDataPrefsFile.Unescape(stringBuilder3, readOnlySpan2.Slice(num4, readOnlySpan2.Length - num4));
								string text3 = stringBuilder.ToString();
								stringBuilder.Clear();
								SaveDataPrefsFile.Pref pref;
								if (!SaveDataPrefsFile.Pref.TryParse(text3, out pref))
								{
									Log.Error("[SaveDataPrefsFile] Failed to parse pref '" + text2 + "' with string representation: " + text3);
								}
								else
								{
									this.m_storage[text2] = pref;
									num++;
								}
							}
						}
						Log.Out(string.Format("[{0}] Loaded {1} player pref(s) from: {2}", "SaveDataPrefsFile", num, this.m_storageFilePath));
					}
					return;
				}
				catch (IOException ex)
				{
					Log.Error("[SaveDataPrefsFile] Failed to Load: " + ex.Message);
					Log.Exception(ex);
					return;
				}
			}
			Log.Out("[SaveDataPrefsFile] Using empty player prefs, as none exists at: " + this.m_storageFilePath);
		}
	}

	// Token: 0x06004EA0 RID: 20128 RVA: 0x001DEAD0 File Offset: 0x001DCCD0
	public float GetFloat(string key, float defaultValue)
	{
		object storageLock = this.m_storageLock;
		float num;
		lock (storageLock)
		{
			SaveDataPrefsFile.Pref pref;
			float num2;
			num = ((this.m_storage.TryGetValue(key, out pref) && pref.TryGet(out num2)) ? num2 : defaultValue);
		}
		return num;
	}

	// Token: 0x06004EA1 RID: 20129 RVA: 0x001DEB2C File Offset: 0x001DCD2C
	public void SetFloat(string key, float value)
	{
		object storageLock = this.m_storageLock;
		lock (storageLock)
		{
			SaveDataPrefsFile.Pref pref;
			if (this.m_storage.TryGetValue(key, out pref))
			{
				float num;
				if (!pref.TryGet(out num) || num != value)
				{
					pref.Set(value);
					this.m_dirty = true;
				}
			}
			else
			{
				pref = new SaveDataPrefsFile.Pref(value);
				this.m_storage[key] = pref;
				this.m_dirty = true;
			}
		}
	}

	// Token: 0x06004EA2 RID: 20130 RVA: 0x001DEBB4 File Offset: 0x001DCDB4
	public int GetInt(string key, int defaultValue)
	{
		object storageLock = this.m_storageLock;
		int num;
		lock (storageLock)
		{
			SaveDataPrefsFile.Pref pref;
			int num2;
			num = ((this.m_storage.TryGetValue(key, out pref) && pref.TryGet(out num2)) ? num2 : defaultValue);
		}
		return num;
	}

	// Token: 0x06004EA3 RID: 20131 RVA: 0x001DEC10 File Offset: 0x001DCE10
	public void SetInt(string key, int value)
	{
		object storageLock = this.m_storageLock;
		lock (storageLock)
		{
			SaveDataPrefsFile.Pref pref;
			if (this.m_storage.TryGetValue(key, out pref))
			{
				int num;
				if (!pref.TryGet(out num) || num != value)
				{
					pref.Set(value);
					this.m_dirty = true;
				}
			}
			else
			{
				pref = new SaveDataPrefsFile.Pref(value);
				this.m_storage[key] = pref;
				this.m_dirty = true;
			}
		}
	}

	// Token: 0x06004EA4 RID: 20132 RVA: 0x001DEC98 File Offset: 0x001DCE98
	public string GetString(string key, string defaultValue)
	{
		object storageLock = this.m_storageLock;
		string text;
		lock (storageLock)
		{
			SaveDataPrefsFile.Pref pref;
			string text2;
			text = ((this.m_storage.TryGetValue(key, out pref) && pref.TryGet(out text2)) ? text2 : defaultValue);
		}
		return text;
	}

	// Token: 0x06004EA5 RID: 20133 RVA: 0x001DECF4 File Offset: 0x001DCEF4
	public void SetString(string key, string value)
	{
		object storageLock = this.m_storageLock;
		lock (storageLock)
		{
			SaveDataPrefsFile.Pref pref;
			if (this.m_storage.TryGetValue(key, out pref))
			{
				string text;
				if (!pref.TryGet(out text) || !(text == value))
				{
					pref.Set(value);
					this.m_dirty = true;
				}
			}
			else
			{
				pref = new SaveDataPrefsFile.Pref(value);
				this.m_storage[key] = pref;
				this.m_dirty = true;
			}
		}
	}

	// Token: 0x06004EA6 RID: 20134 RVA: 0x001DED80 File Offset: 0x001DCF80
	public bool HasKey(string key)
	{
		object storageLock = this.m_storageLock;
		bool flag2;
		lock (storageLock)
		{
			flag2 = this.m_storage.ContainsKey(key);
		}
		return flag2;
	}

	// Token: 0x06004EA7 RID: 20135 RVA: 0x001DEDC8 File Offset: 0x001DCFC8
	public void DeleteKey(string key)
	{
		object storageLock = this.m_storageLock;
		lock (storageLock)
		{
			this.m_storage.Remove(key);
		}
	}

	// Token: 0x06004EA8 RID: 20136 RVA: 0x001DEE10 File Offset: 0x001DD010
	public void DeleteAll()
	{
		object storageLock = this.m_storageLock;
		lock (storageLock)
		{
			this.m_storage.Clear();
		}
	}

	// Token: 0x06004EA9 RID: 20137 RVA: 0x001DEE58 File Offset: 0x001DD058
	public void Save()
	{
		this.SaveInternal();
	}

	// Token: 0x06004EAA RID: 20138 RVA: 0x001DEE60 File Offset: 0x001DD060
	public void Load()
	{
		this.LoadInternal();
	}

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x06004EAB RID: 20139 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public bool CanLoad
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04003C68 RID: 15464
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<char, char> EscapeMapping = new Dictionary<char, char>
	{
		{ '\0', '0' },
		{ '\r', 'r' },
		{ '\n', 'n' },
		{ '=', '=' },
		{ '\\', '\\' }
	};

	// Token: 0x04003C69 RID: 15465
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<char, char> UnescapeMapping = SaveDataPrefsFile.EscapeMapping.ToDictionary<KeyValuePair<char, char>, char, char>((KeyValuePair<char, char> pair) => pair.Value, (KeyValuePair<char, char> pair) => pair.Key);

	// Token: 0x04003C6A RID: 15466
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<PrefType, char> PrefTypeMapping = new Dictionary<PrefType, char>
	{
		{
			PrefType.Float,
			'F'
		},
		{
			PrefType.Int,
			'I'
		},
		{
			PrefType.String,
			'S'
		}
	};

	// Token: 0x04003C6B RID: 15467
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<char, PrefType> PrefTypeUnmapping = SaveDataPrefsFile.PrefTypeMapping.ToDictionary<KeyValuePair<PrefType, char>, char, PrefType>((KeyValuePair<PrefType, char> kv) => kv.Value, (KeyValuePair<PrefType, char> kv) => kv.Key);

	// Token: 0x04003C6C RID: 15468
	[PublicizedFrom(EAccessModifier.Private)]
	public const char KeyValueSeparator = '=';

	// Token: 0x04003C6D RID: 15469
	[PublicizedFrom(EAccessModifier.Private)]
	public const char EscapePrefix = '\\';

	// Token: 0x04003C6E RID: 15470
	[PublicizedFrom(EAccessModifier.Private)]
	public const char PrefTypeSeparator = ':';

	// Token: 0x04003C6F RID: 15471
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_storageFilePath;

	// Token: 0x04003C70 RID: 15472
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, SaveDataPrefsFile.Pref> m_storage;

	// Token: 0x04003C71 RID: 15473
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_storageLock = new object();

	// Token: 0x04003C72 RID: 15474
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_dirty;

	// Token: 0x02000A2B RID: 2603
	[PublicizedFrom(EAccessModifier.Private)]
	public class Pref
	{
		// Token: 0x06004EAD RID: 20141 RVA: 0x001DEF33 File Offset: 0x001DD133
		public Pref(float value)
		{
			this.Set(value);
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x001DEF42 File Offset: 0x001DD142
		public Pref(int value)
		{
			this.Set(value);
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x001DEF51 File Offset: 0x001DD151
		public Pref(string value)
		{
			this.Set(value);
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06004EB0 RID: 20144 RVA: 0x001DEF60 File Offset: 0x001DD160
		public PrefType Type
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x001DEF68 File Offset: 0x001DD168
		public bool TryGet(out float value)
		{
			if (this.m_type != PrefType.Float)
			{
				value = 0f;
				return false;
			}
			value = this.m_values.Float;
			return true;
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x001DEF89 File Offset: 0x001DD189
		public bool TryGet(out int value)
		{
			if (this.m_type != PrefType.Int)
			{
				value = 0;
				return false;
			}
			value = this.m_values.Int;
			return true;
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x001DEFA7 File Offset: 0x001DD1A7
		public bool TryGet(out string value)
		{
			if (this.m_type != PrefType.String)
			{
				value = null;
				return false;
			}
			value = this.m_refs.String;
			return true;
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x001DEFC5 File Offset: 0x001DD1C5
		public void Set(float value)
		{
			this.m_type = PrefType.Float;
			this.m_values.Float = value;
			this.m_refs.String = null;
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x001DEFE6 File Offset: 0x001DD1E6
		public void Set(int value)
		{
			this.m_type = PrefType.Int;
			this.m_values.Int = value;
			this.m_refs.String = null;
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x001DF007 File Offset: 0x001DD207
		public void Set(string value)
		{
			this.m_type = PrefType.String;
			this.m_refs.String = value;
			this.m_values.Int = 0;
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x001DF028 File Offset: 0x001DD228
		public bool TryToString(out string stringRepresentation)
		{
			char c;
			if (!SaveDataPrefsFile.PrefTypeMapping.TryGetValue(this.m_type, out c))
			{
				Log.Warning(string.Format("[{0}] No char mapping for pref type '{1}'.", "SaveDataPrefsFile", this.m_type));
				stringRepresentation = null;
				return false;
			}
			switch (this.m_type)
			{
			case PrefType.Float:
				stringRepresentation = string.Format("{0}{1}{2:R}", c, ':', this.m_values.Float);
				return true;
			case PrefType.Int:
				stringRepresentation = string.Format("{0}{1}{2}", c, ':', this.m_values.Int);
				return true;
			case PrefType.String:
				stringRepresentation = string.Format("{0}{1}{2}", c, ':', this.m_refs.String);
				return true;
			default:
				Log.Error(string.Format("[{0}] Missing to string implementation for '{1}'.", "SaveDataPrefsFile", this.m_type));
				stringRepresentation = null;
				return false;
			}
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x001DF128 File Offset: 0x001DD328
		public unsafe static bool TryParse(ReadOnlySpan<char> stringRepresentation, out SaveDataPrefsFile.Pref pref)
		{
			if (stringRepresentation.Length < 2 || *stringRepresentation[1] != 58)
			{
				pref = null;
				return false;
			}
			PrefType prefType;
			if (!SaveDataPrefsFile.PrefTypeUnmapping.TryGetValue((char)(*stringRepresentation[0]), out prefType))
			{
				pref = null;
				return false;
			}
			ReadOnlySpan<char> readOnlySpan = stringRepresentation;
			ReadOnlySpan<char> readOnlySpan2 = readOnlySpan.Slice(2, readOnlySpan.Length - 2);
			switch (prefType)
			{
			case PrefType.Float:
			{
				float num;
				if (!float.TryParse(readOnlySpan2, out num))
				{
					pref = null;
					return false;
				}
				pref = new SaveDataPrefsFile.Pref(num);
				return true;
			}
			case PrefType.Int:
			{
				int num2;
				if (!int.TryParse(readOnlySpan2, out num2))
				{
					pref = null;
					return false;
				}
				pref = new SaveDataPrefsFile.Pref(num2);
				return true;
			}
			case PrefType.String:
				pref = new SaveDataPrefsFile.Pref(new string(readOnlySpan2));
				return true;
			default:
				Log.Error(string.Format("[{0}] Missing parse implementation for '{1}'.", "SaveDataPrefsFile", prefType));
				pref = null;
				return false;
			}
		}

		// Token: 0x04003C73 RID: 15475
		[PublicizedFrom(EAccessModifier.Private)]
		public PrefType m_type;

		// Token: 0x04003C74 RID: 15476
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataPrefsFile.Pref.PrefValues m_values;

		// Token: 0x04003C75 RID: 15477
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataPrefsFile.Pref.PrefRefs m_refs;

		// Token: 0x02000A2C RID: 2604
		[PublicizedFrom(EAccessModifier.Private)]
		[StructLayout(LayoutKind.Explicit)]
		public struct PrefValues
		{
			// Token: 0x04003C76 RID: 15478
			[FieldOffset(0)]
			public float Float;

			// Token: 0x04003C77 RID: 15479
			[FieldOffset(0)]
			public int Int;
		}

		// Token: 0x02000A2D RID: 2605
		[PublicizedFrom(EAccessModifier.Private)]
		[StructLayout(LayoutKind.Explicit)]
		public struct PrefRefs
		{
			// Token: 0x04003C78 RID: 15480
			[FieldOffset(0)]
			public string String;
		}
	}
}
