using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SteelSeries.GameSense;
using SteelSeries.GameSense.DeviceZone;
using UnityEngine;

// Token: 0x020011D4 RID: 4564
public class GameSenseManager : IEntityBuffsChanged
{
	// Token: 0x17001142 RID: 4418
	// (get) Token: 0x060091A6 RID: 37286 RVA: 0x0037103B File Offset: 0x0036F23B
	// (set) Token: 0x060091A5 RID: 37285 RVA: 0x00371033 File Offset: 0x0036F233
	public static GameSenseManager Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060091A7 RID: 37287 RVA: 0x00371042 File Offset: 0x0036F242
	[PublicizedFrom(EAccessModifier.Private)]
	static GameSenseManager()
	{
		if (GameManager.IsDedicatedServer || Application.isEditor)
		{
			return;
		}
		if (GameUtils.GetLaunchArgument("nogamesense") != null)
		{
			return;
		}
		GameSenseManager.Instance = new GameSenseManager();
	}

	// Token: 0x17001143 RID: 4419
	// (get) Token: 0x060091A8 RID: 37288 RVA: 0x0037106A File Offset: 0x0036F26A
	public static bool GameSenseInstalled
	{
		get
		{
			return SdFile.Exists(GSClient._getPropsPath());
		}
	}

	// Token: 0x060091A9 RID: 37289 RVA: 0x00371078 File Offset: 0x0036F278
	public void Init()
	{
		if (!GameSenseManager.GameSenseInstalled)
		{
			Log.Out("GameSense server not found (no props file), disabling");
			GameSenseManager.Instance = null;
			return;
		}
		new GameObject("GameSense")
		{
			hideFlags = HideFlags.HideAndDontSave
		}.AddComponent<GSClient>();
		GSClient.Instance.RegisterGame("7dtd", "7 Days to Die", "The Fun Pimps");
		GameStats.OnChangedDelegates += this.GameStats_OnChanged;
		ThreadManager.StartCoroutine(this.BindEventsCo());
	}

	// Token: 0x060091AA RID: 37290 RVA: 0x003710EB File Offset: 0x0036F2EB
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator BindEventsCo()
	{
		yield return new WaitForSeconds(0.1f);
		this.BindEventTime();
		this.BindEventAmmo();
		this.BindEventHealth();
		this.BindEventDurability();
		this.BindEventStealth();
		this.BindEventBleeding();
		this.BindEventBloodmoon();
		this.BindEventCompass();
		this.BindEventHit();
		this.BindEventDukes();
		yield break;
	}

	// Token: 0x060091AB RID: 37291 RVA: 0x003710FC File Offset: 0x0036F2FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventTime()
	{
		ColorRanges colorRanges = ColorRanges.Create(new ColorRange[]
		{
			new ColorRange(0U, 16U, ColorStatic.Create(byte.MaxValue, 0, 0)),
			new ColorRange(17U, 32U, ColorStatic.Create(byte.MaxValue, 160, 0)),
			new ColorRange(33U, 49U, ColorStatic.Create(byte.MaxValue, byte.MaxValue, 0)),
			new ColorRange(50U, 66U, ColorStatic.Create(byte.MaxValue, byte.MaxValue, 0)),
			new ColorRange(67U, 74U, ColorStatic.Create(byte.MaxValue, 160, 0)),
			new ColorRange(75U, 82U, ColorStatic.Create(byte.MaxValue, 80, 0)),
			new ColorRange(83U, 91U, ColorStatic.Create(byte.MaxValue, 40, 0)),
			new ColorRange(92U, 100U, ColorStatic.Create(byte.MaxValue, 0, 0))
		});
		AbstractHandler abstractHandler = ColorHandler.Create(ScriptableObject.CreateInstance<RGBPerkeyZoneFunctionKeys>(), IlluminationMode.Percent, colorRanges);
		this.BindEventWrapper("TIME", 0, 100, EventIconId.Clock, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x060091AC RID: 37292 RVA: 0x00371210 File Offset: 0x0036F410
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventAmmo()
	{
		AbstractHandler abstractHandler = ColorHandler.Create(ScriptableObject.CreateInstance<RGBPerkeyZoneNumberKeys>(), IlluminationMode.Percent, ColorGradient.Create(new RGB(160, 0, byte.MaxValue), new RGB(0, 0, byte.MaxValue)));
		this.BindEventWrapper("AMMO", 0, 100, EventIconId.Ammo, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x060091AD RID: 37293 RVA: 0x00371264 File Offset: 0x0036F464
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventHealth()
	{
		AbstractHandler abstractHandler = ColorHandler.Create(ScriptableObject.CreateInstance<RGBPerkeyZoneRowQ>(), IlluminationMode.Percent, ColorGradient.Create(new RGB(byte.MaxValue, 0, 0), new RGB(0, byte.MaxValue, 0)), RateRange.Create(new FreqRepeatLimitPair[]
		{
			new FreqRepeatLimitPair(0U, 10U, 2U, 0U),
			new FreqRepeatLimitPair(11U, 20U, 1U, 0U)
		}));
		this.BindEventWrapper("HEALTH", 0, 100, EventIconId.Health, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x060091AE RID: 37294 RVA: 0x003712E4 File Offset: 0x0036F4E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventDurability()
	{
		RGBPerkeyZoneCustom rgbperkeyZoneCustom = ScriptableObject.CreateInstance<RGBPerkeyZoneCustom>();
		rgbperkeyZoneCustom.zone = new byte[] { 83, 84, 85, 86 };
		AbstractHandler abstractHandler = ColorHandler.Create(rgbperkeyZoneCustom, IlluminationMode.Percent, ColorGradient.Create(new RGB(130, 130, 0), new RGB(0, byte.MaxValue, 200)));
		this.BindEventWrapper("DURABILITY", 0, 100, EventIconId.Item, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x060091AF RID: 37295 RVA: 0x00371358 File Offset: 0x0036F558
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventStealth()
	{
		RGBPerkeyZoneCustom rgbperkeyZoneCustom = ScriptableObject.CreateInstance<RGBPerkeyZoneCustom>();
		rgbperkeyZoneCustom.zone = new byte[] { 224, 225, 57, 43, 53 };
		AbstractHandler abstractHandler = ColorHandler.Create(rgbperkeyZoneCustom, IlluminationMode.Percent, ColorGradient.Create(new RGB(50, 120, 50), new RGB(0, 250, 250)));
		this.BindEventWrapper("STEALTH", 0, 100, EventIconId.Default, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x060091B0 RID: 37296 RVA: 0x003713C4 File Offset: 0x0036F5C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventBleeding()
	{
		AbstractHandler abstractHandler = ColorHandler.Create(ScriptableObject.CreateInstance<RGBPerkeyZoneNavCluster>(), IlluminationMode.Color, ColorStatic.Create(byte.MaxValue, 0, 0), RateStatic.Create(5U, 0U));
		this.BindEventWrapper("BLEEDING", 0, 1, EventIconId.Default, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x060091B1 RID: 37297 RVA: 0x0037140C File Offset: 0x0036F60C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventBloodmoon()
	{
		ColorRanges colorRanges = ColorRanges.Create(new ColorRange[]
		{
			new ColorRange(0U, 0U, ColorStatic.Create(0, 0, 0)),
			new ColorRange(1U, 10U, ColorStatic.Create(60, 0, 0)),
			new ColorRange(11U, 20U, ColorStatic.Create(70, 0, 0)),
			new ColorRange(21U, 30U, ColorStatic.Create(80, 0, 0)),
			new ColorRange(31U, 40U, ColorStatic.Create(90, 0, 0)),
			new ColorRange(41U, 50U, ColorStatic.Create(105, 0, 0)),
			new ColorRange(51U, 60U, ColorStatic.Create(120, 0, 0)),
			new ColorRange(61U, 70U, ColorStatic.Create(135, 0, 0)),
			new ColorRange(71U, 80U, ColorStatic.Create(150, 0, 0)),
			new ColorRange(81U, 90U, ColorStatic.Create(190, 0, 0)),
			new ColorRange(91U, 100U, ColorStatic.Create(byte.MaxValue, 0, 0))
		});
		AbstractHandler abstractHandler = ColorHandler.Create(ScriptableObject.CreateInstance<RGBPerkeyZoneRowA>(), IlluminationMode.Color, colorRanges, RateRange.Create(new FreqRepeatLimitPair[]
		{
			new FreqRepeatLimitPair(81U, 90U, 1U, 0U)
		}));
		this.BindEventWrapper("BLOODMOON", 0, 100, EventIconId.Default, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x060091B2 RID: 37298 RVA: 0x0037155C File Offset: 0x0036F75C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventCompass()
	{
		foreach (Tuple<string, byte> tuple in new Tuple<string, byte>[]
		{
			new Tuple<string, byte>("NORTH", 96),
			new Tuple<string, byte>("NORTHEAST", 97),
			new Tuple<string, byte>("EAST", 94),
			new Tuple<string, byte>("SOUTHEAST", 91),
			new Tuple<string, byte>("SOUTH", 90),
			new Tuple<string, byte>("SOUTHWEST", 89),
			new Tuple<string, byte>("WEST", 92),
			new Tuple<string, byte>("NORTHWEST", 95)
		})
		{
			RGBPerkeyZoneCustom rgbperkeyZoneCustom = ScriptableObject.CreateInstance<RGBPerkeyZoneCustom>();
			rgbperkeyZoneCustom.zone = new byte[] { tuple.Item2 };
			AbstractHandler abstractHandler = ColorHandler.Create(rgbperkeyZoneCustom, IlluminationMode.Color, ColorStatic.Create(50, byte.MaxValue, 0));
			this.BindEventWrapper(tuple.Item1, 0, 1, EventIconId.Compass, false, new AbstractHandler[] { abstractHandler });
		}
	}

	// Token: 0x060091B3 RID: 37299 RVA: 0x00371648 File Offset: 0x0036F848
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventHit()
	{
		AbstractHandler abstractHandler = ColorHandler.Create(ScriptableObject.CreateInstance<MouseZoneAll>(), IlluminationMode.Color, ColorStatic.Create(byte.MaxValue, 0, 0), RateStatic.Create(8U, 0U));
		this.BindEventWrapper("HIT", 0, 1, EventIconId.Headshot, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x060091B4 RID: 37300 RVA: 0x00371690 File Offset: 0x0036F890
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventDukes()
	{
		LineData lineData = LineData.Create(LineDataText.Create("Dukes: ", "", true, 0U), LineDataAccessor.ContextFrameKey("dukesstring"));
		FrameDataMultiLine frameDataMultiLine = FrameDataMultiLine.Create(new LineData[] { lineData }, new FrameModifiers(10000U, false), EventIconId.Money);
		AbstractScreenDevice_Zone abstractScreenDevice_Zone = ScriptableObject.CreateInstance<ScreenedZoneOne>();
		ScreenMode screenMode = ScreenMode.screen;
		AbstractFrameData[] array = new FrameDataMultiLine[] { frameDataMultiLine };
		ScreenHandler screenHandler = ScreenHandler.Create(abstractScreenDevice_Zone, screenMode, array);
		this.BindEventWrapper("DUKES", 0, 1, EventIconId.Money, true, new AbstractHandler[] { screenHandler });
	}

	// Token: 0x060091B5 RID: 37301 RVA: 0x0037170B File Offset: 0x0036F90B
	public void Cleanup()
	{
		GameStats.OnChangedDelegates -= this.GameStats_OnChanged;
	}

	// Token: 0x060091B6 RID: 37302 RVA: 0x0037171E File Offset: 0x0036F91E
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventWrapper(string _eventName, int _minValue, int _maxValue, EventIconId _icon, bool _optionalValue, params AbstractHandler[] _handlers)
	{
		GSClient.Instance.BindEvent(_eventName, _minValue, _maxValue, _icon, _handlers, _optionalValue);
		this.registeredEvents.Add(_eventName);
	}

	// Token: 0x060091B7 RID: 37303 RVA: 0x00371740 File Offset: 0x0036F940
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetEventValues()
	{
		foreach (string text in this.registeredEvents)
		{
			GSClient.Instance.SendEvent(text, 0, null);
		}
	}

	// Token: 0x060091B8 RID: 37304 RVA: 0x0037179C File Offset: 0x0036F99C
	public void SessionStarted(EntityPlayerLocal _playerEntity)
	{
		this.ResetEventValues();
		this.entityPlayer = new WeakReference<EntityPlayerLocal>(_playerEntity);
		_playerEntity.PlayerStats.AddBuffChangedDelegate(this);
		this.lastTimePercent = -1;
		this.previousAmmoPercentage = -1;
		this.previousSlotIndex = -1;
		this.previousDurability = -1;
		this.previousStealth = -1;
		this.bloodMoonWarning = GameStats.GetInt(EnumGameStats.BloodMoonWarning);
		this.duskDawnTime = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
		this.lastBloodMoonValue = -1;
		this.lastDirection = GameSenseManager.EDirection45.None;
		this.dukesItem = ItemClass.GetItem(TraderInfo.CurrencyItem, false);
	}

	// Token: 0x060091B9 RID: 37305 RVA: 0x00371828 File Offset: 0x0036FA28
	public void SessionEnded()
	{
		this.ResetEventValues();
		EntityPlayerLocal entityPlayerLocal;
		if (this.entityPlayer.TryGetTarget(out entityPlayerLocal))
		{
			entityPlayerLocal.PlayerStats.RemoveBuffChangedDelegate(this);
		}
		this.entityPlayer = null;
		this.dukesItem = null;
	}

	// Token: 0x060091BA RID: 37306 RVA: 0x00371864 File Offset: 0x0036FA64
	[PublicizedFrom(EAccessModifier.Private)]
	public void GameStats_OnChanged(EnumGameStats _gameStat, object _newValue)
	{
		if (_gameStat == EnumGameStats.BloodMoonDay)
		{
			this.bloodMoonDay = GameStats.GetInt(EnumGameStats.BloodMoonDay);
		}
	}

	// Token: 0x060091BB RID: 37307 RVA: 0x00371878 File Offset: 0x0036FA78
	public void Update()
	{
		if (!GSClient.Instance.isClientActive())
		{
			return;
		}
		this.UpdateEventAmmo();
		this.UpdateEventDurability();
		this.UpdateEventStealth();
		this.UpdateEventDukes();
	}

	// Token: 0x060091BC RID: 37308 RVA: 0x003718A0 File Offset: 0x0036FAA0
	public void UpdateEventTime(ulong _worldTime)
	{
		if (!GSClient.Instance.isClientActive())
		{
			return;
		}
		this.UpdateEventBloodmoon(_worldTime);
		int num = (int)(_worldTime % 24000UL) / 240;
		if (this.lastTimePercent == num)
		{
			return;
		}
		this.lastTimePercent = num;
		GSClient.Instance.SendEvent("TIME", num, null);
	}

	// Token: 0x060091BD RID: 37309 RVA: 0x003718F4 File Offset: 0x0036FAF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEventAmmo()
	{
		EntityPlayerLocal entityPlayerLocal;
		if (this.entityPlayer == null || !this.entityPlayer.TryGetTarget(out entityPlayerLocal) || entityPlayerLocal == null)
		{
			return;
		}
		int focusedItemIdx = entityPlayerLocal.inventory.GetFocusedItemIdx();
		ItemValue itemValue = entityPlayerLocal.inventory.GetItem(focusedItemIdx).itemValue;
		ItemClass itemClass;
		if (itemValue == null || itemValue.type == 0 || (itemClass = itemValue.ItemClass) == null)
		{
			this.previousAmmoPercentage = 0;
			GSClient.Instance.SendEvent("AMMO", 0, null);
			return;
		}
		ItemActionAttack itemActionAttack = itemClass.Actions[0] as ItemActionAttack;
		int num;
		if (itemActionAttack == null || itemActionAttack is ItemActionMelee || (num = (int)EffectManager.GetValue(PassiveEffects.MagazineSize, itemValue, 0f, entityPlayerLocal, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false)) <= 0)
		{
			this.previousAmmoPercentage = 0;
			GSClient.Instance.SendEvent("AMMO", 0, null);
			return;
		}
		int meta = itemValue.Meta;
		int num2 = 100 * meta / num;
		if (num2 == this.previousAmmoPercentage)
		{
			return;
		}
		this.previousAmmoPercentage = num2;
		GSClient.Instance.SendEvent("AMMO", num2, null);
	}

	// Token: 0x060091BE RID: 37310 RVA: 0x00371A02 File Offset: 0x0036FC02
	public void UpdateEventHealth(int _value)
	{
		if (!GSClient.Instance.isClientActive())
		{
			return;
		}
		GSClient.Instance.SendEvent("HEALTH", _value, null);
	}

	// Token: 0x060091BF RID: 37311 RVA: 0x00371A24 File Offset: 0x0036FC24
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEventDurability()
	{
		EntityPlayerLocal entityPlayerLocal;
		if (this.entityPlayer == null || !this.entityPlayer.TryGetTarget(out entityPlayerLocal) || entityPlayerLocal == null)
		{
			return;
		}
		int focusedItemIdx = entityPlayerLocal.inventory.GetFocusedItemIdx();
		ItemValue itemValue = entityPlayerLocal.inventory.GetItem(focusedItemIdx).itemValue;
		ItemClass itemClass;
		if (itemValue == null || itemValue.type == 0 || (itemClass = itemValue.ItemClass) == null || !itemClass.ShowQualityBar)
		{
			this.previousDurability = 0;
			GSClient.Instance.SendEvent("DURABILITY", 0, null);
			return;
		}
		int num = (int)(100f * itemValue.PercentUsesLeft);
		if (num == this.previousDurability)
		{
			return;
		}
		this.previousDurability = num;
		GSClient.Instance.SendEvent("DURABILITY", num, null);
	}

	// Token: 0x060091C0 RID: 37312 RVA: 0x00371ADC File Offset: 0x0036FCDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEventStealth()
	{
		EntityPlayerLocal entityPlayerLocal;
		if (this.entityPlayer == null || !this.entityPlayer.TryGetTarget(out entityPlayerLocal) || entityPlayerLocal == null)
		{
			return;
		}
		int num = (int)(100f * entityPlayerLocal.Stealth.ValuePercentUI);
		if (num == this.previousStealth)
		{
			return;
		}
		this.previousStealth = num;
		GSClient.Instance.SendEvent("STEALTH", num, null);
	}

	// Token: 0x060091C1 RID: 37313 RVA: 0x00371B3F File Offset: 0x0036FD3F
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEventBleeding(bool _isBleeding)
	{
		GSClient.Instance.SendEvent("BLEEDING", _isBleeding ? 1 : 0, null);
	}

	// Token: 0x060091C2 RID: 37314 RVA: 0x00371B58 File Offset: 0x0036FD58
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEventBloodmoon(ulong _worldTime)
	{
		int num = 0;
		ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(_worldTime);
		int item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		int item3 = valueTuple.Item3;
		if (GameUtils.IsBloodMoonTime(_worldTime, this.duskDawnTime, this.bloodMoonDay))
		{
			num = 100;
		}
		else if (this.bloodMoonDay == item && this.bloodMoonWarning >= 0 && this.bloodMoonWarning <= item2)
		{
			int num2 = this.duskDawnTime.Item1 - 2;
			if (item2 > num2)
			{
				num = 85;
			}
			else
			{
				float num3 = (float)item2 + (float)item3 / 60f - (float)this.bloodMoonWarning;
				float num4 = (float)(num2 - this.bloodMoonWarning);
				num = (int)(80f * num3 / num4);
			}
		}
		if (num == this.lastBloodMoonValue)
		{
			return;
		}
		this.lastBloodMoonValue = num;
		GSClient.Instance.SendEvent("BLOODMOON", num, null);
	}

	// Token: 0x060091C3 RID: 37315 RVA: 0x00371C20 File Offset: 0x0036FE20
	public void UpdateEventCompass(float _rotation)
	{
		if (!GSClient.Instance.isClientActive())
		{
			return;
		}
		GameSenseManager.EDirection45 edirection;
		if ((double)_rotation > 337.5 || (double)_rotation <= 22.5)
		{
			edirection = GameSenseManager.EDirection45.N;
		}
		else if ((double)_rotation <= 67.5)
		{
			edirection = GameSenseManager.EDirection45.NE;
		}
		else if ((double)_rotation <= 112.5)
		{
			edirection = GameSenseManager.EDirection45.E;
		}
		else if ((double)_rotation <= 157.5)
		{
			edirection = GameSenseManager.EDirection45.SE;
		}
		else if ((double)_rotation <= 202.5)
		{
			edirection = GameSenseManager.EDirection45.S;
		}
		else if ((double)_rotation <= 247.5)
		{
			edirection = GameSenseManager.EDirection45.SW;
		}
		else if ((double)_rotation <= 292.5)
		{
			edirection = GameSenseManager.EDirection45.W;
		}
		else
		{
			edirection = GameSenseManager.EDirection45.NW;
		}
		if (this.lastDirection == edirection)
		{
			return;
		}
		this.lastDirection = edirection;
		GSClient.Instance.SendEvent("NORTH", (edirection == GameSenseManager.EDirection45.N) ? 1 : 0, null);
		GSClient.Instance.SendEvent("NORTHEAST", (edirection == GameSenseManager.EDirection45.NE) ? 1 : 0, null);
		GSClient.Instance.SendEvent("EAST", (edirection == GameSenseManager.EDirection45.E) ? 1 : 0, null);
		GSClient.Instance.SendEvent("SOUTHEAST", (edirection == GameSenseManager.EDirection45.SE) ? 1 : 0, null);
		GSClient.Instance.SendEvent("SOUTH", (edirection == GameSenseManager.EDirection45.S) ? 1 : 0, null);
		GSClient.Instance.SendEvent("SOUTHWEST", (edirection == GameSenseManager.EDirection45.SW) ? 1 : 0, null);
		GSClient.Instance.SendEvent("WEST", (edirection == GameSenseManager.EDirection45.W) ? 1 : 0, null);
		GSClient.Instance.SendEvent("NORTHWEST", (edirection == GameSenseManager.EDirection45.NW) ? 1 : 0, null);
	}

	// Token: 0x060091C4 RID: 37316 RVA: 0x00371D90 File Offset: 0x0036FF90
	public void UpdateEventHit()
	{
		if (!GSClient.Instance.isClientActive())
		{
			return;
		}
		this.stopHitEventTime = Time.unscaledTime + 0.5f;
		if (this.stopHitEventCoroutine == null)
		{
			GSClient.Instance.SendEvent("HIT", 1, null);
			this.stopHitEventCoroutine = ThreadManager.StartCoroutine(this.StopHitCo());
		}
	}

	// Token: 0x060091C5 RID: 37317 RVA: 0x00371DE5 File Offset: 0x0036FFE5
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator StopHitCo()
	{
		while (Time.unscaledTime < this.stopHitEventTime)
		{
			yield return null;
		}
		GSClient.Instance.SendEvent("HIT", 0, null);
		this.stopHitEventCoroutine = null;
		yield break;
	}

	// Token: 0x060091C6 RID: 37318 RVA: 0x00371DF4 File Offset: 0x0036FFF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEventDukes()
	{
		float unscaledTime = Time.unscaledTime;
		if (unscaledTime < this.dukesNextTime)
		{
			return;
		}
		this.dukesNextTime = unscaledTime + 5f;
		EntityPlayerLocal entityPlayerLocal;
		if (this.entityPlayer == null || !this.entityPlayer.TryGetTarget(out entityPlayerLocal) || entityPlayerLocal == null)
		{
			return;
		}
		int num = entityPlayerLocal.inventory.GetItemCount(this.dukesItem, false, -1, -1, true);
		num += entityPlayerLocal.bag.GetItemCount(this.dukesItem, -1, -1, true);
		ContextFrameObject contextFrameObject = new ContextFrameObject();
		contextFrameObject["dukesstring"] = ((double)num).FormatNumberWithMetricPrefix(true, 2);
		ContextFrameObject contextFrameObject2 = contextFrameObject;
		GSClient.Instance.SendEvent("DUKES", num, contextFrameObject2);
	}

	// Token: 0x060091C7 RID: 37319 RVA: 0x00371E9C File Offset: 0x0037009C
	public void EntityBuffAdded(BuffValue _buff)
	{
		if (_buff.BuffClass.Name.EqualsCaseInsensitive("buffInjuryBleeding"))
		{
			this.UpdateEventBleeding(true);
		}
	}

	// Token: 0x060091C8 RID: 37320 RVA: 0x00371EBC File Offset: 0x003700BC
	public void EntityBuffRemoved(BuffValue _buff)
	{
		if (_buff.BuffClass.Name.EqualsCaseInsensitive("buffInjuryBleeding"))
		{
			this.UpdateEventBleeding(false);
		}
	}

	// Token: 0x04006CBF RID: 27839
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<string> registeredEvents = new HashSet<string>();

	// Token: 0x04006CC0 RID: 27840
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameSenseGameName = "7dtd";

	// Token: 0x04006CC1 RID: 27841
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameSenseGameNameFull = "7 Days to Die";

	// Token: 0x04006CC2 RID: 27842
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameSenseGameDeveloper = "The Fun Pimps";

	// Token: 0x04006CC3 RID: 27843
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventHealth = "HEALTH";

	// Token: 0x04006CC4 RID: 27844
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventAmmo = "AMMO";

	// Token: 0x04006CC5 RID: 27845
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventTime = "TIME";

	// Token: 0x04006CC6 RID: 27846
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventDurability = "DURABILITY";

	// Token: 0x04006CC7 RID: 27847
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventStealth = "STEALTH";

	// Token: 0x04006CC8 RID: 27848
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventBleeding = "BLEEDING";

	// Token: 0x04006CC9 RID: 27849
	[PublicizedFrom(EAccessModifier.Private)]
	public const string BleedingBuffName = "buffInjuryBleeding";

	// Token: 0x04006CCA RID: 27850
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventBloodmoon = "BLOODMOON";

	// Token: 0x04006CCB RID: 27851
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BloodmoonBlinkWarningHours = 2;

	// Token: 0x04006CCC RID: 27852
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventHit = "HIT";

	// Token: 0x04006CCD RID: 27853
	[PublicizedFrom(EAccessModifier.Private)]
	public const float HitEventDuration = 0.5f;

	// Token: 0x04006CCE RID: 27854
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventDukes = "DUKES";

	// Token: 0x04006CCF RID: 27855
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DukesStringValueKey = "dukesstring";

	// Token: 0x04006CD0 RID: 27856
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DukesLabelLocKey = "gamesenseDukesLabel";

	// Token: 0x04006CD1 RID: 27857
	[PublicizedFrom(EAccessModifier.Private)]
	public WeakReference<EntityPlayerLocal> entityPlayer;

	// Token: 0x04006CD2 RID: 27858
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastTimePercent = -1;

	// Token: 0x04006CD3 RID: 27859
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousSlotIndex = -1;

	// Token: 0x04006CD4 RID: 27860
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousAmmoPercentage = -1;

	// Token: 0x04006CD5 RID: 27861
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousDurability = -1;

	// Token: 0x04006CD6 RID: 27862
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousStealth = -1;

	// Token: 0x04006CD7 RID: 27863
	[PublicizedFrom(EAccessModifier.Private)]
	public int bloodMoonDay;

	// Token: 0x04006CD8 RID: 27864
	[PublicizedFrom(EAccessModifier.Private)]
	public int bloodMoonWarning;

	// Token: 0x04006CD9 RID: 27865
	[TupleElementNames(new string[] { "duskHour", "dawnHour" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<int, int> duskDawnTime;

	// Token: 0x04006CDA RID: 27866
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastBloodMoonValue = -1;

	// Token: 0x04006CDB RID: 27867
	[PublicizedFrom(EAccessModifier.Private)]
	public GameSenseManager.EDirection45 lastDirection = GameSenseManager.EDirection45.None;

	// Token: 0x04006CDC RID: 27868
	[PublicizedFrom(EAccessModifier.Private)]
	public float stopHitEventTime = -1f;

	// Token: 0x04006CDD RID: 27869
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine stopHitEventCoroutine;

	// Token: 0x04006CDE RID: 27870
	[PublicizedFrom(EAccessModifier.Private)]
	public float dukesNextTime;

	// Token: 0x04006CDF RID: 27871
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue dukesItem;

	// Token: 0x020011D5 RID: 4565
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EDirection45
	{
		// Token: 0x04006CE1 RID: 27873
		N,
		// Token: 0x04006CE2 RID: 27874
		NE,
		// Token: 0x04006CE3 RID: 27875
		E,
		// Token: 0x04006CE4 RID: 27876
		SE,
		// Token: 0x04006CE5 RID: 27877
		S,
		// Token: 0x04006CE6 RID: 27878
		SW,
		// Token: 0x04006CE7 RID: 27879
		W,
		// Token: 0x04006CE8 RID: 27880
		NW,
		// Token: 0x04006CE9 RID: 27881
		None
	}
}
