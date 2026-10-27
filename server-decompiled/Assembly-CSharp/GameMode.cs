using System;
using System.Collections.Generic;
using System.Reflection;
using Platform;

// Token: 0x020011F2 RID: 4594
public abstract class GameMode
{
	// Token: 0x06009295 RID: 37525
	public abstract GameMode.ModeGamePref[] GetSupportedGamePrefsInfo();

	// Token: 0x06009296 RID: 37526 RVA: 0x00375340 File Offset: 0x00373540
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

	// Token: 0x06009297 RID: 37527
	public abstract void ResetGamePrefs();

	// Token: 0x06009298 RID: 37528
	public abstract string GetDescription();

	// Token: 0x06009299 RID: 37529
	public abstract int GetID();

	// Token: 0x0600929A RID: 37530
	public abstract void Init();

	// Token: 0x0600929B RID: 37531
	public abstract string GetName();

	// Token: 0x0600929C RID: 37532 RVA: 0x003753C1 File Offset: 0x003735C1
	public string GetTypeName()
	{
		if (this.cachedTypeName == null)
		{
			this.cachedTypeName = base.GetType().Name;
		}
		return this.cachedTypeName;
	}

	// Token: 0x0600929D RID: 37533
	public abstract int GetRoundCount();

	// Token: 0x0600929E RID: 37534
	public abstract void StartRound(int _idx);

	// Token: 0x0600929F RID: 37535
	public abstract void EndRound(int _idx);

	// Token: 0x060092A0 RID: 37536 RVA: 0x0004E500 File Offset: 0x0004C700
	public virtual string GetAdditionalGameInfo(World _world)
	{
		return string.Empty;
	}

	// Token: 0x060092A1 RID: 37537 RVA: 0x003753E4 File Offset: 0x003735E4
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

	// Token: 0x060092A2 RID: 37538 RVA: 0x0037545B File Offset: 0x0037365B
	public static GameMode GetGameModeForId(int _id)
	{
		GameMode.InitGameModeDict();
		if (GameMode.gameModes.ContainsKey(_id))
		{
			return GameMode.gameModes[_id];
		}
		return null;
	}

	// Token: 0x060092A3 RID: 37539 RVA: 0x0037547C File Offset: 0x0037367C
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

	// Token: 0x060092A4 RID: 37540 RVA: 0x003754F0 File Offset: 0x003736F0
	public override string ToString()
	{
		if (this.localizedName == null)
		{
			this.localizedName = Localization.Get(this.GetName(), false, null);
		}
		return this.localizedName;
	}

	// Token: 0x060092A5 RID: 37541 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameMode()
	{
	}

	// Token: 0x04006C69 RID: 27753
	public static GameMode[] AvailGameModes = new GameMode[]
	{
		new GameModeSurvival()
	};

	// Token: 0x04006C6A RID: 27754
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<EnumGamePrefs, GameMode.ModeGamePref> gamePrefs;

	// Token: 0x04006C6B RID: 27755
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedTypeName;

	// Token: 0x04006C6C RID: 27756
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName;

	// Token: 0x04006C6D RID: 27757
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, GameMode> gameModes;

	// Token: 0x020011F3 RID: 4595
	public struct ModeGamePref
	{
		// Token: 0x060092A7 RID: 37543 RVA: 0x00375528 File Offset: 0x00373728
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

		// Token: 0x04006C6E RID: 27758
		public EnumGamePrefs GamePref;

		// Token: 0x04006C6F RID: 27759
		public GamePrefs.EnumType ValueType;

		// Token: 0x04006C70 RID: 27760
		public object DefaultValue;
	}
}
