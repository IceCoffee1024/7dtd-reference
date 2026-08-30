using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using Platform.XBL.Save.Storage;

// Token: 0x02001224 RID: 4644
public static class LaunchPrefs
{
	// Token: 0x17001177 RID: 4471
	// (get) Token: 0x060093E7 RID: 37863 RVA: 0x0037E495 File Offset: 0x0037C695
	public static IReadOnlyDictionary<string, ILaunchPref> All
	{
		get
		{
			return LaunchPrefs.s_launchPrefs;
		}
	}

	// Token: 0x060093E8 RID: 37864 RVA: 0x0037E49C File Offset: 0x0037C69C
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

	// Token: 0x060093E9 RID: 37865 RVA: 0x0037E4C4 File Offset: 0x0037C6C4
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

	// Token: 0x060093EA RID: 37866 RVA: 0x0037E518 File Offset: 0x0037C718
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

	// Token: 0x060093EB RID: 37867 RVA: 0x0037E57C File Offset: 0x0037C77C
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

	// Token: 0x04006EB8 RID: 28344
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object s_initializationLock = new object();

	// Token: 0x04006EB9 RID: 28345
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initializing;

	// Token: 0x04006EBA RID: 28346
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initialized;

	// Token: 0x04006EBB RID: 28347
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, ILaunchPref> s_launchPrefs = new Dictionary<string, ILaunchPref>(StringComparer.OrdinalIgnoreCase);

	// Token: 0x04006EBC RID: 28348
	public static readonly ILaunchPref<bool> SkipNewsScreen = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "SkipNewsScreen");

	// Token: 0x04006EBD RID: 28349
	public static readonly ILaunchPref<string> UserDataFolder = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "UserDataFolder");

	// Token: 0x04006EBE RID: 28350
	public static readonly ILaunchPref<bool> PlayerPrefsFile = LaunchPrefs.Create<bool>((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent(), LaunchPrefs.Parsers.BOOL, "PlayerPrefsFile");

	// Token: 0x04006EBF RID: 28351
	public static readonly ILaunchPref<bool> AllowCrossplay = LaunchPrefs.Create<bool>(true, LaunchPrefs.Parsers.BOOL, "AllowCrossplay");

	// Token: 0x04006EC0 RID: 28352
	public static readonly ILaunchPref<MapChunkDatabaseType> MapChunkDatabase = LaunchPrefs.Create<MapChunkDatabaseType>(MapChunkDatabaseType.Region, LaunchPrefs.EnumParsers<MapChunkDatabaseType>.CASE_INSENSITIVE, "MapChunkDatabase");

	// Token: 0x04006EC1 RID: 28353
	public static readonly ILaunchPref<bool> LoadSaveGame = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "LoadSaveGame");

	// Token: 0x04006EC2 RID: 28354
	public static readonly ILaunchPref<bool> AllowJoinConfigModded = LaunchPrefs.Create<bool>((DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent(), LaunchPrefs.Parsers.BOOL, "AllowJoinConfigModded");

	// Token: 0x04006EC3 RID: 28355
	public static readonly ILaunchPref<int> MaxWorldSizeHost = LaunchPrefs.Create<int>(PlatformOptimizations.DefaultMaxWorldSizeHost, LaunchPrefs.Parsers.INT, "MaxWorldSizeHost");

	// Token: 0x04006EC4 RID: 28356
	public static readonly ILaunchPref<int> MaxWorldSizeClient = LaunchPrefs.Create<int>(-1, LaunchPrefs.Parsers.INT, "MaxWorldSizeClient");

	// Token: 0x04006EC5 RID: 28357
	public static readonly ILaunchPref<string> SessionInvite = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "SessionInvite");

	// Token: 0x04006EC6 RID: 28358
	public static readonly ILaunchPref<bool> EnableProps = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "EnableProps");

	// Token: 0x04006EC7 RID: 28359
	public static readonly ILaunchPref<SaveStorageProvider> GameCoreSaveStorageProvider = LaunchPrefs.Create<SaveStorageProvider>(SaveStorageProvider.Blobs, LaunchPrefs.EnumParsers<SaveStorageProvider>.CASE_INSENSITIVE, "GameCoreSaveStorageProvider");

	// Token: 0x04006EC8 RID: 28360
	public static readonly ILaunchPref<bool> GameCoreBlobCache = LaunchPrefs.Create<bool>(true, LaunchPrefs.Parsers.BOOL, "GameCoreBlobCache");

	// Token: 0x04006EC9 RID: 28361
	public static readonly ILaunchPref<bool> GameCoreBlobCacheProactive = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "GameCoreBlobCacheProactive");

	// Token: 0x04006ECA RID: 28362
	public static readonly ILaunchPref<int> GameCoreBlobOperationQueueMaxTotalSize = LaunchPrefs.Create<int>(PlatformOptimizations.DefaultGameCoreBlobOperationQueueMaxTotalSize, LaunchPrefs.Parsers.INT, "GameCoreBlobOperationQueueMaxTotalSize");

	// Token: 0x04006ECB RID: 28363
	public static readonly ILaunchPref<string> RunAutomation = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "RunAutomation");

	// Token: 0x04006ECC RID: 28364
	public static readonly ILaunchPref<string> SubmissionOverride = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "SubmissionOverride");

	// Token: 0x02001225 RID: 4645
	// (Invoke) Token: 0x060093EE RID: 37870
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool LaunchPrefParser<T>(string stringRepresentation, out T value);

	// Token: 0x02001226 RID: 4646
	[PublicizedFrom(EAccessModifier.Private)]
	public static class Parsers
	{
		// Token: 0x04006ECD RID: 28365
		public static readonly LaunchPrefs.LaunchPrefParser<int> INT = delegate(string s, out int value)
		{
			return int.TryParse(s, out value);
		};

		// Token: 0x04006ECE RID: 28366
		public static readonly LaunchPrefs.LaunchPrefParser<long> LONG = delegate(string s, out long value)
		{
			return long.TryParse(s, out value);
		};

		// Token: 0x04006ECF RID: 28367
		public static readonly LaunchPrefs.LaunchPrefParser<ulong> ULONG = delegate(string s, out ulong value)
		{
			return ulong.TryParse(s, out value);
		};

		// Token: 0x04006ED0 RID: 28368
		public static readonly LaunchPrefs.LaunchPrefParser<bool> BOOL = delegate(string s, out bool value)
		{
			return bool.TryParse(s, out value);
		};

		// Token: 0x04006ED1 RID: 28369
		public static readonly LaunchPrefs.LaunchPrefParser<string> STRING = delegate(string s, out string value)
		{
			value = s;
			return true;
		};
	}

	// Token: 0x02001228 RID: 4648
	[PublicizedFrom(EAccessModifier.Private)]
	public static class EnumParsers<TEnum> where TEnum : struct, IConvertible
	{
		// Token: 0x04006ED3 RID: 28371
		public static readonly LaunchPrefs.LaunchPrefParser<TEnum> CASE_SENSITIVE = delegate(string s, out TEnum value)
		{
			return EnumUtils.TryParse<TEnum>(s, out value, false);
		};

		// Token: 0x04006ED4 RID: 28372
		public static readonly LaunchPrefs.LaunchPrefParser<TEnum> CASE_INSENSITIVE = delegate(string s, out TEnum value)
		{
			return EnumUtils.TryParse<TEnum>(s, out value, true);
		};
	}

	// Token: 0x0200122A RID: 4650
	[PublicizedFrom(EAccessModifier.Private)]
	public abstract class LaunchPref : ILaunchPref
	{
		// Token: 0x060093FE RID: 37886 RVA: 0x0037E844 File Offset: 0x0037CA44
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

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x060093FF RID: 37887 RVA: 0x0037E8AC File Offset: 0x0037CAAC
		public string Name { get; }

		// Token: 0x06009400 RID: 37888
		public abstract bool TrySet(string stringRepresentation);
	}

	// Token: 0x0200122B RID: 4651
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class LaunchPref<T> : LaunchPrefs.LaunchPref, ILaunchPref<T>, ILaunchPref
	{
		// Token: 0x06009401 RID: 37889 RVA: 0x0037E8B4 File Offset: 0x0037CAB4
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

		// Token: 0x06009402 RID: 37890 RVA: 0x0037E8DC File Offset: 0x0037CADC
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

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x06009403 RID: 37891 RVA: 0x0037E91C File Offset: 0x0037CB1C
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

		// Token: 0x04006ED7 RID: 28375
		[PublicizedFrom(EAccessModifier.Private)]
		public T m_value;

		// Token: 0x04006ED8 RID: 28376
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LaunchPrefs.LaunchPrefParser<T> m_parser;
	}
}
