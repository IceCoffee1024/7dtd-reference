using System;
using System.Globalization;
using System.IO;
using SandboxOptions;

// Token: 0x0200122B RID: 4651
public class GameStats
{
	// Token: 0x140000F9 RID: 249
	// (add) Token: 0x0600941E RID: 37918 RVA: 0x0037FFAC File Offset: 0x0037E1AC
	// (remove) Token: 0x0600941F RID: 37919 RVA: 0x0037FFE0 File Offset: 0x0037E1E0
	public static event GameStats.OnChangedDelegate OnChangedDelegates;

	// Token: 0x06009420 RID: 37920 RVA: 0x00380014 File Offset: 0x0037E214
	[PublicizedFrom(EAccessModifier.Private)]
	public void initPropertyDecl()
	{
		this.propertyList = new GameStats.PropertyDecl[]
		{
			new GameStats.PropertyDecl(EnumGameStats.GameState, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.GameModeId, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.TimeLimitActive, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.TimeLimitThisRound, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.FragLimitActive, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.FragLimitThisRound, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.DayLimitActive, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.DayLimitThisRound, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.ShowWindow, true, GameStats.EnumType.String, string.Empty),
			new GameStats.PropertyDecl(EnumGameStats.LoadScene, true, GameStats.EnumType.String, string.Empty),
			new GameStats.PropertyDecl(EnumGameStats.CurrentRoundIx, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.ShowAllPlayersOnMap, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.ShowFriendPlayerOnMap, true, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.ShowSpawnWindow, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.IsSpawnNearOtherPlayer, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.TimeOfDayIncPerSec, true, GameStats.EnumType.Int, 20),
			new GameStats.PropertyDecl(EnumGameStats.IsCreativeMenuEnabled, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.IsTeleportEnabled, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.IsFlyingEnabled, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.IsPlayerDamageEnabled, true, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.IsPlayerCollisionEnabled, true, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.IsSaveSupplyCrates, false, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.IsResetMapOnRestart, false, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.IsSpawnEnemies, true, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.PlayerKillingMode, true, GameStats.EnumType.Int, EnumPlayerKillingMode.KillStrangersOnly),
			new GameStats.PropertyDecl(EnumGameStats.ScorePlayerKillMultiplier, true, GameStats.EnumType.Int, 1),
			new GameStats.PropertyDecl(EnumGameStats.ScoreZombieKillMultiplier, true, GameStats.EnumType.Int, 1),
			new GameStats.PropertyDecl(EnumGameStats.ScoreDiedMultiplier, true, GameStats.EnumType.Int, -5),
			new GameStats.PropertyDecl(EnumGameStats.EnemyCount, false, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.AnimalCount, false, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.IsVersionCheckDone, false, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.ZombieHordeMeter, false, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.DropOnDeath, true, GameStats.EnumType.Int, 1),
			new GameStats.PropertyDecl(EnumGameStats.DropOnQuit, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.GameDifficulty, true, GameStats.EnumType.Int, 2),
			new GameStats.PropertyDecl(EnumGameStats.BloodMoonEnemyCount, true, GameStats.EnumType.Int, 8),
			new GameStats.PropertyDecl(EnumGameStats.EnemySpawnMode, true, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.EnemyDifficulty, true, GameStats.EnumType.Int, EnumEnemyDifficulty.Normal),
			new GameStats.PropertyDecl(EnumGameStats.DayLightLength, true, GameStats.EnumType.Int, 18),
			new GameStats.PropertyDecl(EnumGameStats.LandClaimCount, true, GameStats.EnumType.Int, 5),
			new GameStats.PropertyDecl(EnumGameStats.LandClaimSize, true, GameStats.EnumType.Int, 41),
			new GameStats.PropertyDecl(EnumGameStats.LandClaimDeadZone, true, GameStats.EnumType.Int, 30),
			new GameStats.PropertyDecl(EnumGameStats.LandClaimExpiryTime, true, GameStats.EnumType.Int, 3),
			new GameStats.PropertyDecl(EnumGameStats.LandClaimDecayMode, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.LandClaimOnlineDurabilityModifier, true, GameStats.EnumType.Int, 32),
			new GameStats.PropertyDecl(EnumGameStats.LandClaimOfflineDurabilityModifier, true, GameStats.EnumType.Int, 32),
			new GameStats.PropertyDecl(EnumGameStats.LandClaimOfflineDelay, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.BedrollExpiryTime, true, GameStats.EnumType.Int, 45),
			new GameStats.PropertyDecl(EnumGameStats.AirDropFrequency, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.GlobalMessageToShow, false, GameStats.EnumType.String, ""),
			new GameStats.PropertyDecl(EnumGameStats.AirDropMarker, true, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.PartySharedKillRange, true, GameStats.EnumType.Int, 100),
			new GameStats.PropertyDecl(EnumGameStats.ChunkStabilityEnabled, false, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.AutoParty, true, GameStats.EnumType.Bool, false),
			new GameStats.PropertyDecl(EnumGameStats.OptionsPOICulling, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.BloodMoonDay, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.BlockDamagePlayer, true, GameStats.EnumType.Int, 100),
			new GameStats.PropertyDecl(EnumGameStats.XPMultiplier, true, GameStats.EnumType.Int, 100),
			new GameStats.PropertyDecl(EnumGameStats.BloodMoonWarning, true, GameStats.EnumType.Int, 8),
			new GameStats.PropertyDecl(EnumGameStats.AllowedViewDistance, false, GameStats.EnumType.Int, 12),
			new GameStats.PropertyDecl(EnumGameStats.TwitchBloodMoonAllowed, true, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.DeathPenalty, true, GameStats.EnumType.Int, EnumDeathPenalty.XPOnly),
			new GameStats.PropertyDecl(EnumGameStats.QuestProgressionDailyLimit, true, GameStats.EnumType.Int, 4),
			new GameStats.PropertyDecl(EnumGameStats.BiomeProgression, true, GameStats.EnumType.Bool, true),
			new GameStats.PropertyDecl(EnumGameStats.StormFreq, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.CameraRestrictionMode, true, GameStats.EnumType.Int, 0),
			new GameStats.PropertyDecl(EnumGameStats.JarRefund, true, GameStats.EnumType.Int, 60),
			new GameStats.PropertyDecl(EnumGameStats.SandboxPreset, true, GameStats.EnumType.String, ""),
			new GameStats.PropertyDecl(EnumGameStats.SandboxCode, true, GameStats.EnumType.String, ""),
			new GameStats.PropertyDecl(EnumGameStats.DayNightLength, true, GameStats.EnumType.Int, 60),
			new GameStats.PropertyDecl(EnumGameStats.BlockDamageAI, true, GameStats.EnumType.Int, 100),
			new GameStats.PropertyDecl(EnumGameStats.BlockDamageAIBM, true, GameStats.EnumType.Int, 100),
			new GameStats.PropertyDecl(EnumGameStats.LootAbundance, true, GameStats.EnumType.Int, 100),
			new GameStats.PropertyDecl(EnumGameStats.LootRespawnDays, true, GameStats.EnumType.Int, 7),
			new GameStats.PropertyDecl(EnumGameStats.GlobalGSModifier, true, GameStats.EnumType.Int, 100),
			new GameStats.PropertyDecl(EnumGameStats.BiomeGSModifier, true, GameStats.EnumType.Int, 100),
			new GameStats.PropertyDecl(EnumGameStats.GlobalLSModifier, true, GameStats.EnumType.Int, 100),
			new GameStats.PropertyDecl(EnumGameStats.BiomeLSModifier, true, GameStats.EnumType.Int, 100)
		};
	}

	// Token: 0x170011DB RID: 4571
	// (get) Token: 0x06009421 RID: 37921 RVA: 0x0038072E File Offset: 0x0037E92E
	public static GameStats Instance
	{
		get
		{
			if (GameStats.m_Instance == null)
			{
				GameStats.m_Instance = new GameStats();
				GameStats.m_Instance.initPropertyDecl();
				GameStats.m_Instance.initDefault();
			}
			return GameStats.m_Instance;
		}
	}

	// Token: 0x06009422 RID: 37922 RVA: 0x0038075C File Offset: 0x0037E95C
	[PublicizedFrom(EAccessModifier.Private)]
	public void initDefault()
	{
		foreach (GameStats.PropertyDecl propertyDecl in this.propertyList)
		{
			int name = (int)propertyDecl.name;
			this.propertyValues[name] = propertyDecl.defaultValue;
		}
	}

	// Token: 0x06009423 RID: 37923 RVA: 0x0038079C File Offset: 0x0037E99C
	public void Write(BinaryWriter _write)
	{
		foreach (GameStats.PropertyDecl propertyDecl in this.propertyList)
		{
			if (propertyDecl.bPersistent)
			{
				switch (propertyDecl.type)
				{
				case GameStats.EnumType.Int:
					_write.Write(GameStats.GetInt(propertyDecl.name));
					break;
				case GameStats.EnumType.Float:
					_write.Write(GameStats.GetFloat(propertyDecl.name));
					break;
				case GameStats.EnumType.String:
					_write.Write(GameStats.GetString(propertyDecl.name));
					break;
				case GameStats.EnumType.Bool:
					_write.Write(GameStats.GetBool(propertyDecl.name));
					break;
				case GameStats.EnumType.Binary:
					_write.Write(Utils.ToBase64(GameStats.GetString(propertyDecl.name)));
					break;
				}
			}
		}
	}

	// Token: 0x06009424 RID: 37924 RVA: 0x00380860 File Offset: 0x0037EA60
	public void Read(BinaryReader _reader)
	{
		foreach (GameStats.PropertyDecl propertyDecl in this.propertyList)
		{
			if (propertyDecl.bPersistent)
			{
				int name = (int)propertyDecl.name;
				switch (propertyDecl.type)
				{
				case GameStats.EnumType.Int:
					this.propertyValues[name] = _reader.ReadInt32();
					break;
				case GameStats.EnumType.Float:
					this.propertyValues[name] = _reader.ReadSingle();
					break;
				case GameStats.EnumType.String:
					this.propertyValues[name] = _reader.ReadString();
					break;
				case GameStats.EnumType.Bool:
					this.propertyValues[name] = _reader.ReadBoolean();
					break;
				case GameStats.EnumType.Binary:
					this.propertyValues[name] = Utils.FromBase64(_reader.ReadString());
					break;
				}
			}
		}
	}

	// Token: 0x06009425 RID: 37925 RVA: 0x0038092C File Offset: 0x0037EB2C
	public static object Parse(EnumGameStats _enum, string _val)
	{
		int num = GameStats.find(_enum);
		if (num == -1)
		{
			return null;
		}
		switch (GameStats.Instance.propertyList[num].type)
		{
		case GameStats.EnumType.Int:
			return int.Parse(_val);
		case GameStats.EnumType.Float:
			return StringParsers.ParseFloat(_val, 0, -1, NumberStyles.Any);
		case GameStats.EnumType.String:
			return _val;
		case GameStats.EnumType.Bool:
			return StringParsers.ParseBool(_val, 0, -1, true);
		case GameStats.EnumType.Binary:
			return _val;
		default:
			return null;
		}
	}

	// Token: 0x06009426 RID: 37926 RVA: 0x003809AC File Offset: 0x0037EBAC
	public static string GetString(EnumGameStats _eProperty)
	{
		string text;
		try
		{
			text = (string)GameStats.Instance.propertyValues[(int)_eProperty];
		}
		catch (InvalidCastException)
		{
			Log.Error("GetString: InvalidCastException " + _eProperty.ToStringCached<EnumGameStats>());
			text = string.Empty;
		}
		return text;
	}

	// Token: 0x06009427 RID: 37927 RVA: 0x003809FC File Offset: 0x0037EBFC
	public static float GetFloat(EnumGameStats _eProperty)
	{
		float num;
		try
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				BaseSandboxOption baseSandboxOption = GameStats.Instance.sandboxReferences[(int)_eProperty];
				if (baseSandboxOption != null)
				{
					return baseSandboxOption.GetFloatValue();
				}
			}
			num = (float)GameStats.Instance.propertyValues[(int)_eProperty];
		}
		catch (InvalidCastException)
		{
			Log.Error("GetFloat: InvalidCastException " + _eProperty.ToStringCached<EnumGameStats>());
			num = 0f;
		}
		return num;
	}

	// Token: 0x06009428 RID: 37928 RVA: 0x00380A74 File Offset: 0x0037EC74
	public static int GetInt(EnumGameStats _eProperty)
	{
		int num;
		try
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				BaseSandboxOption baseSandboxOption = GameStats.Instance.sandboxReferences[(int)_eProperty];
				if (baseSandboxOption != null)
				{
					return baseSandboxOption.GetIntValue();
				}
			}
			num = (int)GameStats.Instance.propertyValues[(int)_eProperty];
		}
		catch (InvalidCastException)
		{
			Log.Error("GetInt: InvalidCastException " + _eProperty.ToStringCached<EnumGameStats>());
			num = 0;
		}
		return num;
	}

	// Token: 0x06009429 RID: 37929 RVA: 0x00380AE8 File Offset: 0x0037ECE8
	public static bool GetBool(EnumGameStats _eProperty)
	{
		bool flag;
		try
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				BaseSandboxOption baseSandboxOption = GameStats.Instance.sandboxReferences[(int)_eProperty];
				if (baseSandboxOption != null)
				{
					return baseSandboxOption.GetBoolValue();
				}
			}
			flag = (bool)GameStats.Instance.propertyValues[(int)_eProperty];
		}
		catch (InvalidCastException)
		{
			Log.Error("GetBool: InvalidCastException " + _eProperty.ToStringCached<EnumGameStats>());
			flag = false;
		}
		return flag;
	}

	// Token: 0x0600942A RID: 37930 RVA: 0x00380B5C File Offset: 0x0037ED5C
	public static void SetupSandboxReferences()
	{
		GameStats.Instance.sandboxReferences = new BaseSandboxOption[81];
		for (int i = 0; i < GameStats.Instance.propertyList.Length; i++)
		{
			GameStats.PropertyDecl propertyDecl = GameStats.Instance.propertyList[i];
			global::SandboxOptions.SandboxOptions sandboxOptions = global::SandboxOptions.SandboxOptions.Max;
			if (Enum.TryParse<global::SandboxOptions.SandboxOptions>(propertyDecl.name.ToString(), out sandboxOptions))
			{
				GameStats.Instance.sandboxReferences[(int)propertyDecl.name] = SandboxOptionManager.GetOption(sandboxOptions);
			}
			else
			{
				EnumGameStats name = propertyDecl.name;
				if (name != EnumGameStats.BlockDamagePlayer)
				{
					if (name == EnumGameStats.LootAbundance)
					{
						GameStats.Instance.sandboxReferences[(int)propertyDecl.name] = SandboxOptionManager.GetOption(global::SandboxOptions.SandboxOptions.GlobalLootCount);
					}
				}
				else
				{
					GameStats.Instance.sandboxReferences[(int)propertyDecl.name] = SandboxOptionManager.GetOption(global::SandboxOptions.SandboxOptions.BlockDamage);
				}
			}
		}
	}

	// Token: 0x0600942B RID: 37931 RVA: 0x00380C26 File Offset: 0x0037EE26
	public static object GetObject(EnumGameStats _eProperty)
	{
		return GameStats.Instance.propertyValues[(int)_eProperty];
	}

	// Token: 0x0600942C RID: 37932 RVA: 0x00380C34 File Offset: 0x0037EE34
	[PublicizedFrom(EAccessModifier.Private)]
	public static int find(EnumGameStats _eProperty)
	{
		for (int i = 0; i < GameStats.Instance.propertyList.Length; i++)
		{
			if (GameStats.Instance.propertyList[i].name == _eProperty)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600942D RID: 37933 RVA: 0x00380C73 File Offset: 0x0037EE73
	public static void SetObject(EnumGameStats _eProperty, object _value)
	{
		GameStats.Instance.propertyValues[(int)_eProperty] = _value;
		if (GameStats.OnChangedDelegates != null)
		{
			GameStats.OnChangedDelegates(_eProperty, _value);
		}
	}

	// Token: 0x0600942E RID: 37934 RVA: 0x00380C95 File Offset: 0x0037EE95
	public static void Set(EnumGameStats _eProperty, int _value)
	{
		GameStats.SetObject(_eProperty, _value);
	}

	// Token: 0x0600942F RID: 37935 RVA: 0x00380CA3 File Offset: 0x0037EEA3
	public static void Set(EnumGameStats _eProperty, float _value)
	{
		GameStats.SetObject(_eProperty, _value);
	}

	// Token: 0x06009430 RID: 37936 RVA: 0x00380CB1 File Offset: 0x0037EEB1
	public static void Set(EnumGameStats _eProperty, string _value)
	{
		GameStats.SetObject(_eProperty, _value);
	}

	// Token: 0x06009431 RID: 37937 RVA: 0x00380CBA File Offset: 0x0037EEBA
	public static void Set(EnumGameStats _eProperty, bool _value)
	{
		GameStats.SetObject(_eProperty, _value);
	}

	// Token: 0x06009432 RID: 37938 RVA: 0x00380CC8 File Offset: 0x0037EEC8
	public static bool IsDefault(EnumGameStats _eProperty)
	{
		return GameStats.Instance.propertyValues[(int)_eProperty] != null && GameStats.Instance.propertyValues[(int)_eProperty].Equals(GameStats.Instance.propertyList[(int)_eProperty].defaultValue);
	}

	// Token: 0x06009433 RID: 37939 RVA: 0x00380D00 File Offset: 0x0037EF00
	public static GameStats.EnumType? GetStatType(EnumGameStats _eProperty)
	{
		foreach (GameStats.PropertyDecl propertyDecl in GameStats.Instance.propertyList)
		{
			if (propertyDecl.name == _eProperty)
			{
				return new GameStats.EnumType?(propertyDecl.type);
			}
		}
		return null;
	}

	// Token: 0x04006EE8 RID: 28392
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStats.PropertyDecl[] propertyList;

	// Token: 0x04006EE9 RID: 28393
	[PublicizedFrom(EAccessModifier.Private)]
	public object[] propertyValues = new object[81];

	// Token: 0x04006EEA RID: 28394
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseSandboxOption[] sandboxReferences;

	// Token: 0x04006EEB RID: 28395
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameStats m_Instance;

	// Token: 0x0200122C RID: 4652
	// (Invoke) Token: 0x06009436 RID: 37942
	public delegate void OnChangedDelegate(EnumGameStats _gameState, object _newValue);

	// Token: 0x0200122D RID: 4653
	public enum EnumType
	{
		// Token: 0x04006EED RID: 28397
		Int,
		// Token: 0x04006EEE RID: 28398
		Float,
		// Token: 0x04006EEF RID: 28399
		String,
		// Token: 0x04006EF0 RID: 28400
		Bool,
		// Token: 0x04006EF1 RID: 28401
		Binary
	}

	// Token: 0x0200122E RID: 4654
	[PublicizedFrom(EAccessModifier.Private)]
	public struct PropertyDecl
	{
		// Token: 0x06009439 RID: 37945 RVA: 0x00380D61 File Offset: 0x0037EF61
		public PropertyDecl(EnumGameStats _name, bool _bPersistent, GameStats.EnumType _type, object _defaultValue)
		{
			this.name = _name;
			this.type = _type;
			this.defaultValue = _defaultValue;
			this.bPersistent = _bPersistent;
		}

		// Token: 0x04006EF2 RID: 28402
		public EnumGameStats name;

		// Token: 0x04006EF3 RID: 28403
		public GameStats.EnumType type;

		// Token: 0x04006EF4 RID: 28404
		public object defaultValue;

		// Token: 0x04006EF5 RID: 28405
		public bool bPersistent;
	}
}
