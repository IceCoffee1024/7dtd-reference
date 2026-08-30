using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using Platform.XBL.Save.Storage;

// Token: 0x02001240 RID: 4672
public static class LaunchPrefs
{
	// Token: 0x170011B2 RID: 4530
	// (get) Token: 0x060094EB RID: 38123 RVA: 0x003837A5 File Offset: 0x003819A5
	public static IReadOnlyDictionary<string, ILaunchPref> All
	{
		get
		{
			return LaunchPrefs.s_launchPrefs;
		}
	}

	// Token: 0x060094EC RID: 38124 RVA: 0x003837AC File Offset: 0x003819AC
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

	// Token: 0x060094ED RID: 38125 RVA: 0x003837D4 File Offset: 0x003819D4
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

	// Token: 0x060094EE RID: 38126 RVA: 0x00383828 File Offset: 0x00381A28
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

	// Token: 0x060094EF RID: 38127 RVA: 0x0038388C File Offset: 0x00381A8C
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

	// Token: 0x04006F91 RID: 28561
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object s_initializationLock = new object();

	// Token: 0x04006F92 RID: 28562
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initializing;

	// Token: 0x04006F93 RID: 28563
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initialized;

	// Token: 0x04006F94 RID: 28564
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, ILaunchPref> s_launchPrefs = new Dictionary<string, ILaunchPref>(StringComparer.OrdinalIgnoreCase);

	// Token: 0x04006F95 RID: 28565
	public static readonly ILaunchPref<bool> SkipNewsScreen = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "SkipNewsScreen");

	// Token: 0x04006F96 RID: 28566
	public static readonly ILaunchPref<string> UserDataFolder = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "UserDataFolder");

	// Token: 0x04006F97 RID: 28567
	public static readonly ILaunchPref<bool> PlayerPrefsFile = LaunchPrefs.Create<bool>((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent(), LaunchPrefs.Parsers.BOOL, "PlayerPrefsFile");

	// Token: 0x04006F98 RID: 28568
	public static readonly ILaunchPref<bool> AllowCrossplay = LaunchPrefs.Create<bool>(true, LaunchPrefs.Parsers.BOOL, "AllowCrossplay");

	// Token: 0x04006F99 RID: 28569
	public static readonly ILaunchPref<MapChunkDatabaseType> MapChunkDatabase = LaunchPrefs.Create<MapChunkDatabaseType>(MapChunkDatabaseType.Region, LaunchPrefs.EnumParsers<MapChunkDatabaseType>.CASE_INSENSITIVE, "MapChunkDatabase");

	// Token: 0x04006F9A RID: 28570
	public static readonly ILaunchPref<bool> LoadSaveGame = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "LoadSaveGame");

	// Token: 0x04006F9B RID: 28571
	public static readonly ILaunchPref<bool> AllowJoinConfigModded = LaunchPrefs.Create<bool>((DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent(), LaunchPrefs.Parsers.BOOL, "AllowJoinConfigModded");

	// Token: 0x04006F9C RID: 28572
	public static readonly ILaunchPref<int> MaxWorldSizeHost = LaunchPrefs.Create<int>(PlatformOptimizations.DefaultMaxWorldSizeHost, LaunchPrefs.Parsers.INT, "MaxWorldSizeHost");

	// Token: 0x04006F9D RID: 28573
	public static readonly ILaunchPref<int> MaxWorldSizeClient = LaunchPrefs.Create<int>(-1, LaunchPrefs.Parsers.INT, "MaxWorldSizeClient");

	// Token: 0x04006F9E RID: 28574
	public static readonly ILaunchPref<string> SessionInvite = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "SessionInvite");

	// Token: 0x04006F9F RID: 28575
	public static readonly ILaunchPref<bool> EnableProps = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "EnableProps");

	// Token: 0x04006FA0 RID: 28576
	public static readonly ILaunchPref<SaveStorageProvider> GameCoreSaveStorageProvider = LaunchPrefs.Create<SaveStorageProvider>(SaveStorageProvider.Blobs, LaunchPrefs.EnumParsers<SaveStorageProvider>.CASE_INSENSITIVE, "GameCoreSaveStorageProvider");

	// Token: 0x04006FA1 RID: 28577
	public static readonly ILaunchPref<bool> GameCoreBlobCache = LaunchPrefs.Create<bool>(true, LaunchPrefs.Parsers.BOOL, "GameCoreBlobCache");

	// Token: 0x04006FA2 RID: 28578
	public static readonly ILaunchPref<bool> GameCoreBlobCacheProactive = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "GameCoreBlobCacheProactive");

	// Token: 0x04006FA3 RID: 28579
	public static readonly ILaunchPref<int> GameCoreBlobOperationQueueMaxTotalSize = LaunchPrefs.Create<int>(PlatformOptimizations.DefaultGameCoreBlobOperationQueueMaxTotalSize, LaunchPrefs.Parsers.INT, "GameCoreBlobOperationQueueMaxTotalSize");

	// Token: 0x04006FA4 RID: 28580
	public static readonly ILaunchPref<string> RunAutomation = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "RunAutomation");

	// Token: 0x04006FA5 RID: 28581
	public static readonly ILaunchPref<string> SubmissionOverride = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "SubmissionOverride");

	// Token: 0x02001241 RID: 4673
	// (Invoke) Token: 0x060094F2 RID: 38130
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool LaunchPrefParser<T>(string stringRepresentation, out T value);

	// Token: 0x02001242 RID: 4674
	[PublicizedFrom(EAccessModifier.Private)]
	public static class Parsers
	{
		// Token: 0x04006FA6 RID: 28582
		public static readonly LaunchPrefs.LaunchPrefParser<int> INT = delegate(string s, out int value)
		{
			return int.TryParse(s, out value);
		};

		// Token: 0x04006FA7 RID: 28583
		public static readonly LaunchPrefs.LaunchPrefParser<long> LONG = delegate(string s, out long value)
		{
			return long.TryParse(s, out value);
		};

		// Token: 0x04006FA8 RID: 28584
		public static readonly LaunchPrefs.LaunchPrefParser<ulong> ULONG = delegate(string s, out ulong value)
		{
			return ulong.TryParse(s, out value);
		};

		// Token: 0x04006FA9 RID: 28585
		public static readonly LaunchPrefs.LaunchPrefParser<bool> BOOL = delegate(string s, out bool value)
		{
			return bool.TryParse(s, out value);
		};

		// Token: 0x04006FAA RID: 28586
		public static readonly LaunchPrefs.LaunchPrefParser<string> STRING = delegate(string s, out string value)
		{
			value = s;
			return true;
		};
	}

	// Token: 0x02001244 RID: 4676
	[PublicizedFrom(EAccessModifier.Private)]
	public static class EnumParsers<TEnum> where TEnum : struct, IConvertible
	{
		// Token: 0x04006FAC RID: 28588
		public static readonly LaunchPrefs.LaunchPrefParser<TEnum> CASE_SENSITIVE = delegate(string s, out TEnum value)
		{
			return EnumUtils.TryParse<TEnum>(s, out value, false);
		};

		// Token: 0x04006FAD RID: 28589
		public static readonly LaunchPrefs.LaunchPrefParser<TEnum> CASE_INSENSITIVE = delegate(string s, out TEnum value)
		{
			return EnumUtils.TryParse<TEnum>(s, out value, true);
		};
	}

	// Token: 0x02001246 RID: 4678
	[PublicizedFrom(EAccessModifier.Private)]
	public abstract class LaunchPref : ILaunchPref
	{
		// Token: 0x06009502 RID: 38146 RVA: 0x00383B54 File Offset: 0x00381D54
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

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x06009503 RID: 38147 RVA: 0x00383BBC File Offset: 0x00381DBC
		public string Name { get; }

		// Token: 0x06009504 RID: 38148
		public abstract bool TrySet(string stringRepresentation);
	}

	// Token: 0x02001247 RID: 4679
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class LaunchPref<T> : LaunchPrefs.LaunchPref, ILaunchPref<T>, ILaunchPref
	{
		// Token: 0x06009505 RID: 38149 RVA: 0x00383BC4 File Offset: 0x00381DC4
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

		// Token: 0x06009506 RID: 38150 RVA: 0x00383BEC File Offset: 0x00381DEC
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

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x06009507 RID: 38151 RVA: 0x00383C2C File Offset: 0x00381E2C
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

		// Token: 0x04006FB0 RID: 28592
		[PublicizedFrom(EAccessModifier.Private)]
		public T m_value;

		// Token: 0x04006FB1 RID: 28593
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LaunchPrefs.LaunchPrefParser<T> m_parser;
	}
}
