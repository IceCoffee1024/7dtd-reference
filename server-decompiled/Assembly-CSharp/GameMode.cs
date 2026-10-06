using System;
using System.Collections.Generic;
using System.Reflection;
using Platform;

// Token: 0x020011E2 RID: 4578
public abstract class GameMode
{
	// Token: 0x0600923F RID: 37439
	public abstract GameMode.ModeGamePref[] GetSupportedGamePrefsInfo();

	// Token: 0x06009240 RID: 37440 RVA: 0x003731CC File Offset: 0x003713CC
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

	// Token: 0x06009241 RID: 37441
	public abstract void ResetGamePrefs();

	// Token: 0x06009242 RID: 37442
	public abstract string GetDescription();

	// Token: 0x06009243 RID: 37443
	public abstract int GetID();

	// Token: 0x06009244 RID: 37444
	public abstract void Init();

	// Token: 0x06009245 RID: 37445
	public abstract string GetName();

	// Token: 0x06009246 RID: 37446 RVA: 0x0037324D File Offset: 0x0037144D
	public string GetTypeName()
	{
		if (this.cachedTypeName == null)
		{
			this.cachedTypeName = base.GetType().Name;
		}
		return this.cachedTypeName;
	}

	// Token: 0x06009247 RID: 37447
	public abstract int GetRoundCount();

	// Token: 0x06009248 RID: 37448
	public abstract void StartRound(int _idx);

	// Token: 0x06009249 RID: 37449
	public abstract void EndRound(int _idx);

	// Token: 0x0600924A RID: 37450 RVA: 0x0004E540 File Offset: 0x0004C740
	public virtual string GetAdditionalGameInfo(World _world)
	{
		return string.Empty;
	}

	// Token: 0x0600924B RID: 37451 RVA: 0x00373270 File Offset: 0x00371470
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

	// Token: 0x0600924C RID: 37452 RVA: 0x003732E7 File Offset: 0x003714E7
	public static GameMode GetGameModeForId(int _id)
	{
		GameMode.InitGameModeDict();
		if (GameMode.gameModes.ContainsKey(_id))
		{
			return GameMode.gameModes[_id];
		}
		return null;
	}

	// Token: 0x0600924D RID: 37453 RVA: 0x00373308 File Offset: 0x00371508
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

	// Token: 0x0600924E RID: 37454 RVA: 0x0037337C File Offset: 0x0037157C
	public override string ToString()
	{
		if (this.localizedName == null)
		{
			this.localizedName = Localization.Get(this.GetName(), false, null);
		}
		return this.localizedName;
	}

	// Token: 0x0600924F RID: 37455 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameMode()
	{
	}

	// Token: 0x04006C51 RID: 27729
	public static GameMode[] AvailGameModes = new GameMode[]
	{
		new GameModeSurvival()
	};

	// Token: 0x04006C52 RID: 27730
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<EnumGamePrefs, GameMode.ModeGamePref> gamePrefs;

	// Token: 0x04006C53 RID: 27731
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedTypeName;

	// Token: 0x04006C54 RID: 27732
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName;

	// Token: 0x04006C55 RID: 27733
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, GameMode> gameModes;

	// Token: 0x020011E3 RID: 4579
	public struct ModeGamePref
	{
		// Token: 0x06009251 RID: 37457 RVA: 0x003733B4 File Offset: 0x003715B4
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

		// Token: 0x04006C56 RID: 27734
		public EnumGamePrefs GamePref;

		// Token: 0x04006C57 RID: 27735
		public GamePrefs.EnumType ValueType;

		// Token: 0x04006C58 RID: 27736
		public object DefaultValue;
	}
}
