using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using Platform.XBL.Save.Storage;

// Token: 0x02001258 RID: 4696
public static class LaunchPrefs
{
	// Token: 0x170011F6 RID: 4598
	// (get) Token: 0x06009579 RID: 38265 RVA: 0x00387FED File Offset: 0x003861ED
	public static IReadOnlyDictionary<string, ILaunchPref> All
	{
		get
		{
			return LaunchPrefs.s_launchPrefs;
		}
	}

	// Token: 0x0600957A RID: 38266 RVA: 0x00387FF4 File Offset: 0x003861F4
	[PublicizedFrom(EAccessModifier.Private)]
	public static ILaunchPref<T> Create<T>(T defaultValue, LaunchPrefs.LaunchPrefParser<T> parser, [CallerMemberName] string name = null)
	{
		if (parser == null)
		{
			throw new ArgumentNullException("parser");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new LaunchPrefs.LaunchPref<T>(name, defaultValue, parser);
	}

	// Token: 0x0600957B RID: 38267 RVA: 0x0038801C File Offset: 0x0038621C
	public static void InitStart()
	{
		object obj = LaunchPrefs.s_initializationLock;
		lock (obj)
		{
			if (LaunchPrefs.s_initializing)
			{
				throw new InvalidOperationException("LaunchPrefs.InitStart has already been called.");
			}
			LaunchPrefs.s_initializing = true;
		}
	}

	// Token: 0x0600957C RID: 38268 RVA: 0x00388070 File Offset: 0x00386270
	public static void InitEnd()
	{
		object obj = LaunchPrefs.s_initializationLock;
		lock (obj)
		{
			if (!LaunchPrefs.s_initializing)
			{
				throw new InvalidOperationException("LaunchPrefs.InitStart has not been called yet.");
			}
			if (LaunchPrefs.s_initialized)
			{
				throw new InvalidOperationException("LaunchPrefs.InitEnd has already been called.");
			}
			LaunchPrefs.s_initialized = true;
		}
	}

	// Token: 0x0600957D RID: 38269 RVA: 0x003880D4 File Offset: 0x003862D4
	[PublicizedFrom(EAccessModifier.Private)]
	public static LaunchPrefs.LaunchPrefParser<OUT> ThenTransform<IN, OUT>(this LaunchPrefs.LaunchPrefParser<IN> parser, Func<IN, OUT> transform)
	{
		return delegate(string representation, out OUT value)
		{
			IN @in;
			if (!parser(representation, out @in))
			{
				value = default(OUT);
				return false;
			}
			value = transform(@in);
			return true;
		};
	}

	// Token: 0x04006FFB RID: 28667
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object s_initializationLock = new object();

	// Token: 0x04006FFC RID: 28668
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initializing;

	// Token: 0x04006FFD RID: 28669
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initialized;

	// Token: 0x04006FFE RID: 28670
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, ILaunchPref> s_launchPrefs = new Dictionary<string, ILaunchPref>(StringComparer.OrdinalIgnoreCase);

	// Token: 0x04006FFF RID: 28671
	public static readonly ILaunchPref<bool> SkipNewsScreen = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "SkipNewsScreen");

	// Token: 0x04007000 RID: 28672
	public static readonly ILaunchPref<string> UserDataFolder = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "UserDataFolder");

	// Token: 0x04007001 RID: 28673
	public static readonly ILaunchPref<bool> PlayerPrefsFile = LaunchPrefs.Create<bool>((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent(), LaunchPrefs.Parsers.BOOL, "PlayerPrefsFile");

	// Token: 0x04007002 RID: 28674
	public static readonly ILaunchPref<bool> AllowCrossplay = LaunchPrefs.Create<bool>(true, LaunchPrefs.Parsers.BOOL, "AllowCrossplay");

	// Token: 0x04007003 RID: 28675
	public static readonly ILaunchPref<MapChunkDatabaseType> MapChunkDatabase = LaunchPrefs.Create<MapChunkDatabaseType>(MapChunkDatabaseType.Region, LaunchPrefs.EnumParsers<MapChunkDatabaseType>.CASE_INSENSITIVE, "MapChunkDatabase");

	// Token: 0x04007004 RID: 28676
	public static readonly ILaunchPref<bool> LoadSaveGame = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "LoadSaveGame");

	// Token: 0x04007005 RID: 28677
	public static readonly ILaunchPref<bool> AllowJoinConfigModded = LaunchPrefs.Create<bool>((DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent(), LaunchPrefs.Parsers.BOOL, "AllowJoinConfigModded");

	// Token: 0x04007006 RID: 28678
	public static readonly ILaunchPref<int> MaxWorldSizeHost = LaunchPrefs.Create<int>(PlatformOptimizations.DefaultMaxWorldSizeHost, LaunchPrefs.Parsers.INT, "MaxWorldSizeHost");

	// Token: 0x04007007 RID: 28679
	public static readonly ILaunchPref<int> MaxWorldSizeClient = LaunchPrefs.Create<int>(-1, LaunchPrefs.Parsers.INT, "MaxWorldSizeClient");

	// Token: 0x04007008 RID: 28680
	public static readonly ILaunchPref<string> SessionInvite = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "SessionInvite");

	// Token: 0x04007009 RID: 28681
	public static readonly ILaunchPref<bool> EnableProps = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "EnableProps");

	// Token: 0x0400700A RID: 28682
	public static readonly ILaunchPref<SaveStorageProvider> GameCoreSaveStorageProvider = LaunchPrefs.Create<SaveStorageProvider>(SaveStorageProvider.Blobs, LaunchPrefs.EnumParsers<SaveStorageProvider>.CASE_INSENSITIVE, "GameCoreSaveStorageProvider");

	// Token: 0x0400700B RID: 28683
	public static readonly ILaunchPref<bool> GameCoreBlobCache = LaunchPrefs.Create<bool>(true, LaunchPrefs.Parsers.BOOL, "GameCoreBlobCache");

	// Token: 0x0400700C RID: 28684
	public static readonly ILaunchPref<bool> GameCoreBlobCacheProactive = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "GameCoreBlobCacheProactive");

	// Token: 0x0400700D RID: 28685
	public static readonly ILaunchPref<int> GameCoreBlobOperationQueueMaxTotalSize = LaunchPrefs.Create<int>(PlatformOptimizations.DefaultGameCoreBlobOperationQueueMaxTotalSize, LaunchPrefs.Parsers.INT, "GameCoreBlobOperationQueueMaxTotalSize");

	// Token: 0x0400700E RID: 28686
	public static readonly ILaunchPref<string> RunAutomation = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "RunAutomation");

	// Token: 0x0400700F RID: 28687
	public static readonly ILaunchPref<string> SubmissionOverride = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "SubmissionOverride");

	// Token: 0x02001259 RID: 4697
	// (Invoke) Token: 0x06009580 RID: 38272
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool LaunchPrefParser<T>(string stringRepresentation, out T value);

	// Token: 0x0200125A RID: 4698
	[PublicizedFrom(EAccessModifier.Private)]
	public static class Parsers
	{
		// Token: 0x04007010 RID: 28688
		public static readonly LaunchPrefs.LaunchPrefParser<int> INT = delegate(string s, out int value)
		{
			return int.TryParse(s, out value);
		};

		// Token: 0x04007011 RID: 28689
		public static readonly LaunchPrefs.LaunchPrefParser<long> LONG = delegate(string s, out long value)
		{
			return long.TryParse(s, out value);
		};

		// Token: 0x04007012 RID: 28690
		public static readonly LaunchPrefs.LaunchPrefParser<ulong> ULONG = delegate(string s, out ulong value)
		{
			return ulong.TryParse(s, out value);
		};

		// Token: 0x04007013 RID: 28691
		public static readonly LaunchPrefs.LaunchPrefParser<bool> BOOL = delegate(string s, out bool value)
		{
			return bool.TryParse(s, out value);
		};

		// Token: 0x04007014 RID: 28692
		public static readonly LaunchPrefs.LaunchPrefParser<string> STRING = delegate(string s, out string value)
		{
			value = s;
			return true;
		};
	}

	// Token: 0x0200125C RID: 4700
	[PublicizedFrom(EAccessModifier.Private)]
	public static class EnumParsers<TEnum> where TEnum : struct, IConvertible
	{
		// Token: 0x04007016 RID: 28694
		public static readonly LaunchPrefs.LaunchPrefParser<TEnum> CASE_SENSITIVE = delegate(string s, out TEnum value)
		{
			return EnumUtils.TryParse<TEnum>(s, out value, false);
		};

		// Token: 0x04007017 RID: 28695
		public static readonly LaunchPrefs.LaunchPrefParser<TEnum> CASE_INSENSITIVE = delegate(string s, out TEnum value)
		{
			return EnumUtils.TryParse<TEnum>(s, out value, true);
		};
	}

	// Token: 0x0200125E RID: 4702
	[PublicizedFrom(EAccessModifier.Private)]
	public abstract class LaunchPref : ILaunchPref
	{
		// Token: 0x06009590 RID: 38288 RVA: 0x0038839C File Offset: 0x0038659C
		[PublicizedFrom(EAccessModifier.Protected)]
		public LaunchPref(string name)
		{
			if (LaunchPrefs.s_initializing)
			{
				throw new InvalidOperationException("LaunchPref should be instantiated before LaunchPrefs initialization begins.");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("LaunchPref requires a name", "name");
			}
			if (!LaunchPrefs.s_launchPrefs.TryAdd(name, this))
			{
				throw new InvalidOperationException("There is already a LaunchPref with the name '" + name + "'");
			}
			this.Name = name;
		}

		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06009591 RID: 38289 RVA: 0x00388404 File Offset: 0x00386604
		public string Name { get; }

		// Token: 0x06009592 RID: 38290
		public abstract bool TrySet(string stringRepresentation);
	}

	// Token: 0x0200125F RID: 4703
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class LaunchPref<T> : LaunchPrefs.LaunchPref, ILaunchPref<T>, ILaunchPref
	{
		// Token: 0x06009593 RID: 38291 RVA: 0x0038840C File Offset: 0x0038660C
		public LaunchPref(string name, T defaultValue, LaunchPrefs.LaunchPrefParser<T> parser)
			: base(name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_value = defaultValue;
			this.m_parser = parser;
		}

		// Token: 0x06009594 RID: 38292 RVA: 0x00388434 File Offset: 0x00386634
		public override bool TrySet(string stringRepresentation)
		{
			if (!LaunchPrefs.s_initializing || LaunchPrefs.s_initialized)
			{
				throw new InvalidOperationException("LaunchPref can only be set during LaunchPrefs initialization.");
			}
			T t;
			if (!this.m_parser(stringRepresentation, out t))
			{
				return false;
			}
			this.m_value = t;
			return true;
		}

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06009595 RID: 38293 RVA: 0x00388474 File Offset: 0x00386674
		public T Value
		{
			get
			{
				if (!LaunchPrefs.s_initialized)
				{
					throw new InvalidOperationException("LaunchPref can only be read after LaunchPrefs has finished initializing.");
				}
				return this.m_value;
			}
		}

		// Token: 0x0400701A RID: 28698
		[PublicizedFrom(EAccessModifier.Private)]
		public T m_value;

		// Token: 0x0400701B RID: 28699
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LaunchPrefs.LaunchPrefParser<T> m_parser;
	}
}
