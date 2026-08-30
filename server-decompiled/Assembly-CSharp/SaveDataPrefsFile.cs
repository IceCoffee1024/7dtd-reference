using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x02000A3B RID: 2619
public sealed class SaveDataPrefsFile : ISaveDataPrefs
{
	// Token: 0x06004F20 RID: 20256 RVA: 0x001E0F68 File Offset: 0x001DF168
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

	// Token: 0x06004F21 RID: 20257 RVA: 0x001E1000 File Offset: 0x001DF200
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

	// Token: 0x06004F22 RID: 20258 RVA: 0x001E10A4 File Offset: 0x001DF2A4
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

	// Token: 0x06004F23 RID: 20259 RVA: 0x001E1144 File Offset: 0x001DF344
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

	// Token: 0x06004F24 RID: 20260 RVA: 0x001E1194 File Offset: 0x001DF394
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

	// Token: 0x06004F25 RID: 20261 RVA: 0x001E1358 File Offset: 0x001DF558
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

	// Token: 0x06004F26 RID: 20262 RVA: 0x001E1568 File Offset: 0x001DF768
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

	// Token: 0x06004F27 RID: 20263 RVA: 0x001E15C4 File Offset: 0x001DF7C4
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

	// Token: 0x06004F28 RID: 20264 RVA: 0x001E164C File Offset: 0x001DF84C
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

	// Token: 0x06004F29 RID: 20265 RVA: 0x001E16A8 File Offset: 0x001DF8A8
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

	// Token: 0x06004F2A RID: 20266 RVA: 0x001E1730 File Offset: 0x001DF930
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

	// Token: 0x06004F2B RID: 20267 RVA: 0x001E178C File Offset: 0x001DF98C
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

	// Token: 0x06004F2C RID: 20268 RVA: 0x001E1818 File Offset: 0x001DFA18
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

	// Token: 0x06004F2D RID: 20269 RVA: 0x001E1860 File Offset: 0x001DFA60
	public void DeleteKey(string key)
	{
		object storageLock = this.m_storageLock;
		lock (storageLock)
		{
			this.m_storage.Remove(key);
		}
	}

	// Token: 0x06004F2E RID: 20270 RVA: 0x001E18A8 File Offset: 0x001DFAA8
	public void DeleteAll()
	{
		object storageLock = this.m_storageLock;
		lock (storageLock)
		{
			this.m_storage.Clear();
		}
	}

	// Token: 0x06004F2F RID: 20271 RVA: 0x001E18F0 File Offset: 0x001DFAF0
	public void Save()
	{
		this.SaveInternal();
	}

	// Token: 0x06004F30 RID: 20272 RVA: 0x001E18F8 File Offset: 0x001DFAF8
	public void Load()
	{
		this.LoadInternal();
	}

	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x06004F31 RID: 20273 RVA: 0x00020021 File Offset: 0x0001E221
	public bool CanLoad
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04003CF9 RID: 15609
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<char, char> EscapeMapping = new Dictionary<char, char>
	{
		{ '\0', '0' },
		{ '\r', 'r' },
		{ '\n', 'n' },
		{ '=', '=' },
		{ '\\', '\\' }
	};

	// Token: 0x04003CFA RID: 15610
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<char, char> UnescapeMapping = SaveDataPrefsFile.EscapeMapping.ToDictionary<KeyValuePair<char, char>, char, char>((KeyValuePair<char, char> pair) => pair.Value, (KeyValuePair<char, char> pair) => pair.Key);

	// Token: 0x04003CFB RID: 15611
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

	// Token: 0x04003CFC RID: 15612
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<char, PrefType> PrefTypeUnmapping = SaveDataPrefsFile.PrefTypeMapping.ToDictionary<KeyValuePair<PrefType, char>, char, PrefType>((KeyValuePair<PrefType, char> kv) => kv.Value, (KeyValuePair<PrefType, char> kv) => kv.Key);

	// Token: 0x04003CFD RID: 15613
	[PublicizedFrom(EAccessModifier.Private)]
	public const char KeyValueSeparator = '=';

	// Token: 0x04003CFE RID: 15614
	[PublicizedFrom(EAccessModifier.Private)]
	public const char EscapePrefix = '\\';

	// Token: 0x04003CFF RID: 15615
	[PublicizedFrom(EAccessModifier.Private)]
	public const char PrefTypeSeparator = ':';

	// Token: 0x04003D00 RID: 15616
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_storageFilePath;

	// Token: 0x04003D01 RID: 15617
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, SaveDataPrefsFile.Pref> m_storage;

	// Token: 0x04003D02 RID: 15618
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_storageLock = new object();

	// Token: 0x04003D03 RID: 15619
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_dirty;

	// Token: 0x02000A3C RID: 2620
	[PublicizedFrom(EAccessModifier.Private)]
	public class Pref
	{
		// Token: 0x06004F33 RID: 20275 RVA: 0x001E19CB File Offset: 0x001DFBCB
		public Pref(float value)
		{
			this.Set(value);
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x001E19DA File Offset: 0x001DFBDA
		public Pref(int value)
		{
			this.Set(value);
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x001E19E9 File Offset: 0x001DFBE9
		public Pref(string value)
		{
			this.Set(value);
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06004F36 RID: 20278 RVA: 0x001E19F8 File Offset: 0x001DFBF8
		public PrefType Type
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x001E1A00 File Offset: 0x001DFC00
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

		// Token: 0x06004F38 RID: 20280 RVA: 0x001E1A21 File Offset: 0x001DFC21
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

		// Token: 0x06004F39 RID: 20281 RVA: 0x001E1A3F File Offset: 0x001DFC3F
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

		// Token: 0x06004F3A RID: 20282 RVA: 0x001E1A5D File Offset: 0x001DFC5D
		public void Set(float value)
		{
			this.m_type = PrefType.Float;
			this.m_values.Float = value;
			this.m_refs.String = null;
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x001E1A7E File Offset: 0x001DFC7E
		public void Set(int value)
		{
			this.m_type = PrefType.Int;
			this.m_values.Int = value;
			this.m_refs.String = null;
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x001E1A9F File Offset: 0x001DFC9F
		public void Set(string value)
		{
			this.m_type = PrefType.String;
			this.m_refs.String = value;
			this.m_values.Int = 0;
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x001E1AC0 File Offset: 0x001DFCC0
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

		// Token: 0x06004F3E RID: 20286 RVA: 0x001E1BC0 File Offset: 0x001DFDC0
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

		// Token: 0x04003D04 RID: 15620
		[PublicizedFrom(EAccessModifier.Private)]
		public PrefType m_type;

		// Token: 0x04003D05 RID: 15621
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataPrefsFile.Pref.PrefValues m_values;

		// Token: 0x04003D06 RID: 15622
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataPrefsFile.Pref.PrefRefs m_refs;

		// Token: 0x02000A3D RID: 2621
		[PublicizedFrom(EAccessModifier.Private)]
		[StructLayout(LayoutKind.Explicit)]
		public struct PrefValues
		{
			// Token: 0x04003D07 RID: 15623
			[FieldOffset(0)]
			public float Float;

			// Token: 0x04003D08 RID: 15624
			[FieldOffset(0)]
			public int Int;
		}

		// Token: 0x02000A3E RID: 2622
		[PublicizedFrom(EAccessModifier.Private)]
		[StructLayout(LayoutKind.Explicit)]
		public struct PrefRefs
		{
			// Token: 0x04003D09 RID: 15625
			[FieldOffset(0)]
			public string String;
		}
	}
}
