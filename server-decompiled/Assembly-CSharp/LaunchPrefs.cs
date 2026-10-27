using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using Platform.XBL.Save.Storage;

// Token: 0x0200126A RID: 4714
public static class LaunchPrefs
{
	// Token: 0x17001201 RID: 4609
	// (get) Token: 0x060095D0 RID: 38352 RVA: 0x0038A3E9 File Offset: 0x003885E9
	public static IReadOnlyDictionary<string, ILaunchPref> All
	{
		get
		{
			return LaunchPrefs.s_launchPrefs;
		}
	}

	// Token: 0x060095D1 RID: 38353 RVA: 0x0038A3F0 File Offset: 0x003885F0
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

	// Token: 0x060095D2 RID: 38354 RVA: 0x0038A418 File Offset: 0x00388618
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

	// Token: 0x060095D3 RID: 38355 RVA: 0x0038A46C File Offset: 0x0038866C
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

	// Token: 0x060095D4 RID: 38356 RVA: 0x0038A4D0 File Offset: 0x003886D0
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

	// Token: 0x0400703C RID: 28732
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object s_initializationLock = new object();

	// Token: 0x0400703D RID: 28733
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initializing;

	// Token: 0x0400703E RID: 28734
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initialized;

	// Token: 0x0400703F RID: 28735
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, ILaunchPref> s_launchPrefs = new Dictionary<string, ILaunchPref>(StringComparer.OrdinalIgnoreCase);

	// Token: 0x04007040 RID: 28736
	public static readonly ILaunchPref<bool> SkipNewsScreen = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "SkipNewsScreen");

	// Token: 0x04007041 RID: 28737
	public static readonly ILaunchPref<string> UserDataFolder = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "UserDataFolder");

	// Token: 0x04007042 RID: 28738
	public static readonly ILaunchPref<bool> PlayerPrefsFile = LaunchPrefs.Create<bool>((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent(), LaunchPrefs.Parsers.BOOL, "PlayerPrefsFile");

	// Token: 0x04007043 RID: 28739
	public static readonly ILaunchPref<bool> AllowCrossplay = LaunchPrefs.Create<bool>(true, LaunchPrefs.Parsers.BOOL, "AllowCrossplay");

	// Token: 0x04007044 RID: 28740
	public static readonly ILaunchPref<MapChunkDatabaseType> MapChunkDatabase = LaunchPrefs.Create<MapChunkDatabaseType>(MapChunkDatabaseType.Region, LaunchPrefs.EnumParsers<MapChunkDatabaseType>.CASE_INSENSITIVE, "MapChunkDatabase");

	// Token: 0x04007045 RID: 28741
	public static readonly ILaunchPref<bool> LoadSaveGame = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "LoadSaveGame");

	// Token: 0x04007046 RID: 28742
	public static readonly ILaunchPref<bool> AllowJoinConfigModded = LaunchPrefs.Create<bool>((DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent(), LaunchPrefs.Parsers.BOOL, "AllowJoinConfigModded");

	// Token: 0x04007047 RID: 28743
	public static readonly ILaunchPref<int> MaxWorldSizeHost = LaunchPrefs.Create<int>(PlatformOptimizations.DefaultMaxWorldSizeHost, LaunchPrefs.Parsers.INT, "MaxWorldSizeHost");

	// Token: 0x04007048 RID: 28744
	public static readonly ILaunchPref<int> MaxWorldSizeClient = LaunchPrefs.Create<int>(-1, LaunchPrefs.Parsers.INT, "MaxWorldSizeClient");

	// Token: 0x04007049 RID: 28745
	public static readonly ILaunchPref<string> SessionInvite = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "SessionInvite");

	// Token: 0x0400704A RID: 28746
	public static readonly ILaunchPref<bool> EnableProps = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "EnableProps");

	// Token: 0x0400704B RID: 28747
	public static readonly ILaunchPref<SaveStorageProvider> GameCoreSaveStorageProvider = LaunchPrefs.Create<SaveStorageProvider>(SaveStorageProvider.Blobs, LaunchPrefs.EnumParsers<SaveStorageProvider>.CASE_INSENSITIVE, "GameCoreSaveStorageProvider");

	// Token: 0x0400704C RID: 28748
	public static readonly ILaunchPref<bool> GameCoreBlobCache = LaunchPrefs.Create<bool>(true, LaunchPrefs.Parsers.BOOL, "GameCoreBlobCache");

	// Token: 0x0400704D RID: 28749
	public static readonly ILaunchPref<bool> GameCoreBlobCacheProactive = LaunchPrefs.Create<bool>(false, LaunchPrefs.Parsers.BOOL, "GameCoreBlobCacheProactive");

	// Token: 0x0400704E RID: 28750
	public static readonly ILaunchPref<int> GameCoreBlobOperationQueueMaxTotalSize = LaunchPrefs.Create<int>(PlatformOptimizations.DefaultGameCoreBlobOperationQueueMaxTotalSize, LaunchPrefs.Parsers.INT, "GameCoreBlobOperationQueueMaxTotalSize");

	// Token: 0x0400704F RID: 28751
	public static readonly ILaunchPref<string> RunAutomation = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "RunAutomation");

	// Token: 0x04007050 RID: 28752
	public static readonly ILaunchPref<string> SubmissionOverride = LaunchPrefs.Create<string>(string.Empty, LaunchPrefs.Parsers.STRING, "SubmissionOverride");

	// Token: 0x0200126B RID: 4715
	// (Invoke) Token: 0x060095D7 RID: 38359
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool LaunchPrefParser<T>(string stringRepresentation, out T value);

	// Token: 0x0200126C RID: 4716
	[PublicizedFrom(EAccessModifier.Private)]
	public static class Parsers
	{
		// Token: 0x04007051 RID: 28753
		public static readonly LaunchPrefs.LaunchPrefParser<int> INT = delegate(string s, out int value)
		{
			return int.TryParse(s, out value);
		};

		// Token: 0x04007052 RID: 28754
		public static readonly LaunchPrefs.LaunchPrefParser<long> LONG = delegate(string s, out long value)
		{
			return long.TryParse(s, out value);
		};

		// Token: 0x04007053 RID: 28755
		public static readonly LaunchPrefs.LaunchPrefParser<ulong> ULONG = delegate(string s, out ulong value)
		{
			return ulong.TryParse(s, out value);
		};

		// Token: 0x04007054 RID: 28756
		public static readonly LaunchPrefs.LaunchPrefParser<bool> BOOL = delegate(string s, out bool value)
		{
			return bool.TryParse(s, out value);
		};

		// Token: 0x04007055 RID: 28757
		public static readonly LaunchPrefs.LaunchPrefParser<string> STRING = delegate(string s, out string value)
		{
			value = s;
			return true;
		};
	}

	// Token: 0x0200126E RID: 4718
	[PublicizedFrom(EAccessModifier.Private)]
	public static class EnumParsers<TEnum> where TEnum : struct, IConvertible
	{
		// Token: 0x04007057 RID: 28759
		public static readonly LaunchPrefs.LaunchPrefParser<TEnum> CASE_SENSITIVE = delegate(string s, out TEnum value)
		{
			return EnumUtils.TryParse<TEnum>(s, out value, false);
		};

		// Token: 0x04007058 RID: 28760
		public static readonly LaunchPrefs.LaunchPrefParser<TEnum> CASE_INSENSITIVE = delegate(string s, out TEnum value)
		{
			return EnumUtils.TryParse<TEnum>(s, out value, true);
		};
	}

	// Token: 0x02001270 RID: 4720
	[PublicizedFrom(EAccessModifier.Private)]
	public abstract class LaunchPref : ILaunchPref
	{
		// Token: 0x060095E7 RID: 38375 RVA: 0x0038A798 File Offset: 0x00388998
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

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x060095E8 RID: 38376 RVA: 0x0038A800 File Offset: 0x00388A00
		public string Name { get; }

		// Token: 0x060095E9 RID: 38377
		public abstract bool TrySet(string stringRepresentation);
	}

	// Token: 0x02001271 RID: 4721
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class LaunchPref<T> : LaunchPrefs.LaunchPref, ILaunchPref<T>, ILaunchPref
	{
		// Token: 0x060095EA RID: 38378 RVA: 0x0038A808 File Offset: 0x00388A08
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

		// Token: 0x060095EB RID: 38379 RVA: 0x0038A830 File Offset: 0x00388A30
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

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x060095EC RID: 38380 RVA: 0x0038A870 File Offset: 0x00388A70
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

		// Token: 0x0400705B RID: 28763
		[PublicizedFrom(EAccessModifier.Private)]
		public T m_value;

		// Token: 0x0400705C RID: 28764
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LaunchPrefs.LaunchPrefParser<T> m_parser;
	}
}
