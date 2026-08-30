using System;
using System.Collections.Generic;
using System.Reflection;
using Platform;

// Token: 0x020011AE RID: 4526
public abstract class GameMode
{
	// Token: 0x060090AD RID: 37037
	public abstract GameMode.ModeGamePref[] GetSupportedGamePrefsInfo();

	// Token: 0x060090AE RID: 37038 RVA: 0x003696E8 File Offset: 0x003678E8
	public Dictionary<EnumGamePrefs, GameMode.ModeGamePref> GetGamePrefs()
	{
		if (this.gamePrefs == null)
		{
			this.gamePrefs = new EnumDictionary<EnumGamePrefs, GameMode.ModeGamePref>();
			GameMode.ModeGamePref[] supportedGamePrefsInfo = this.GetSupportedGamePrefsInfo();
			int length = supportedGamePrefsInfo.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				EnumGamePrefs gamePref = supportedGamePrefsInfo[i].GamePref;
				GamePrefs.EnumType valueType = supportedGamePrefsInfo[i].ValueType;
				object defaultValue = supportedGamePrefsInfo[i].DefaultValue;
				this.gamePrefs.Add(gamePref, new GameMode.ModeGamePref(gamePref, valueType, defaultValue, null));
			}
		}
		return this.gamePrefs;
	}

	// Token: 0x060090AF RID: 37039
	public abstract void ResetGamePrefs();

	// Token: 0x060090B0 RID: 37040
	public abstract string GetDescription();

	// Token: 0x060090B1 RID: 37041
	public abstract int GetID();

	// Token: 0x060090B2 RID: 37042
	public abstract void Init();

	// Token: 0x060090B3 RID: 37043
	public abstract string GetName();

	// Token: 0x060090B4 RID: 37044 RVA: 0x00369769 File Offset: 0x00367969
	public string GetTypeName()
	{
		if (this.cachedTypeName == null)
		{
			this.cachedTypeName = base.GetType().Name;
		}
		return this.cachedTypeName;
	}

	// Token: 0x060090B5 RID: 37045
	public abstract int GetRoundCount();

	// Token: 0x060090B6 RID: 37046
	public abstract void StartRound(int _idx);

	// Token: 0x060090B7 RID: 37047
	public abstract void EndRound(int _idx);

	// Token: 0x060090B8 RID: 37048 RVA: 0x0004E124 File Offset: 0x0004C324
	public virtual string GetAdditionalGameInfo(World _world)
	{
		return string.Empty;
	}

	// Token: 0x060090B9 RID: 37049 RVA: 0x0036978C File Offset: 0x0036798C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void InitGameModeDict()
	{
		if (GameMode.gameModes == null)
		{
			GameMode.gameModes = new Dictionary<int, GameMode>();
			Type typeFromHandle = typeof(GameMode);
			foreach (Type type in Assembly.GetCallingAssembly().GetTypes())
			{
				if (!type.IsAbstract && typeFromHandle.IsAssignableFrom(type))
				{
					GameMode gameMode = Activator.CreateInstance(type) as GameMode;
					GameMode.gameModes.Add(gameMode.GetID(), gameMode);
				}
			}
		}
	}

	// Token: 0x060090BA RID: 37050 RVA: 0x00369803 File Offset: 0x00367A03
	public static GameMode GetGameModeForId(int _id)
	{
		GameMode.InitGameModeDict();
		if (GameMode.gameModes.ContainsKey(_id))
		{
			return GameMode.gameModes[_id];
		}
		return null;
	}

	// Token: 0x060090BB RID: 37051 RVA: 0x00369824 File Offset: 0x00367A24
	public static GameMode GetGameModeForName(string _name)
	{
		GameMode.InitGameModeDict();
		foreach (KeyValuePair<int, GameMode> keyValuePair in GameMode.gameModes)
		{
			if (keyValuePair.Value.GetTypeName().EqualsCaseInsensitive(_name))
			{
				return keyValuePair.Value;
			}
		}
		return null;
	}

	// Token: 0x060090BC RID: 37052 RVA: 0x00369898 File Offset: 0x00367A98
	public override string ToString()
	{
		if (this.localizedName == null)
		{
			this.localizedName = Localization.Get(this.GetName(), false, null);
		}
		return this.localizedName;
	}

	// Token: 0x060090BD RID: 37053 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameMode()
	{
	}

	// Token: 0x04006B0F RID: 27407
	public static GameMode[] AvailGameModes = new GameMode[]
	{
		new GameModeSurvival()
	};

	// Token: 0x04006B10 RID: 27408
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<EnumGamePrefs, GameMode.ModeGamePref> gamePrefs;

	// Token: 0x04006B11 RID: 27409
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedTypeName;

	// Token: 0x04006B12 RID: 27410
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName;

	// Token: 0x04006B13 RID: 27411
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, GameMode> gameModes;

	// Token: 0x020011AF RID: 4527
	public struct ModeGamePref
	{
		// Token: 0x060090BF RID: 37055 RVA: 0x003698D0 File Offset: 0x00367AD0
		public ModeGamePref(EnumGamePrefs _gamePref, GamePrefs.EnumType _valueType, object _defaultValue, Dictionary<DeviceFlag, object> _deviceDefaults = null)
		{
			this.GamePref = _gamePref;
			this.ValueType = _valueType;
			if (_deviceDefaults != null && _deviceDefaults.ContainsKey(DeviceFlag.StandaloneWindows))
			{
				this.DefaultValue = _deviceDefaults[DeviceFlag.StandaloneWindows];
				return;
			}
			this.DefaultValue = _defaultValue;
		}

		// Token: 0x04006B14 RID: 27412
		public EnumGamePrefs GamePref;

		// Token: 0x04006B15 RID: 27413
		public GamePrefs.EnumType ValueType;

		// Token: 0x04006B16 RID: 27414
		public object DefaultValue;
	}
}
