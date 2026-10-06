using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SteelSeries.GameSense;
using SteelSeries.GameSense.DeviceZone;
using UnityEngine;

// Token: 0x02001208 RID: 4616
public class GameSenseManager : IEntityBuffsChanged
{
	// Token: 0x170011C1 RID: 4545
	// (get) Token: 0x06009338 RID: 37688 RVA: 0x0037AB4F File Offset: 0x00378D4F
	// (set) Token: 0x06009337 RID: 37687 RVA: 0x0037AB47 File Offset: 0x00378D47
	public static GameSenseManager Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06009339 RID: 37689 RVA: 0x0037AB56 File Offset: 0x00378D56
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

	// Token: 0x170011C2 RID: 4546
	// (get) Token: 0x0600933A RID: 37690 RVA: 0x0037AB7E File Offset: 0x00378D7E
	public static bool GameSenseInstalled
	{
		get
		{
			return SdFile.Exists(GSClient._getPropsPath());
		}
	}

	// Token: 0x0600933B RID: 37691 RVA: 0x0037AB8C File Offset: 0x00378D8C
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

	// Token: 0x0600933C RID: 37692 RVA: 0x0037ABFF File Offset: 0x00378DFF
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

	// Token: 0x0600933D RID: 37693 RVA: 0x0037AC10 File Offset: 0x00378E10
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

	// Token: 0x0600933E RID: 37694 RVA: 0x0037AD24 File Offset: 0x00378F24
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventAmmo()
	{
		AbstractHandler abstractHandler = ColorHandler.Create(ScriptableObject.CreateInstance<RGBPerkeyZoneNumberKeys>(), IlluminationMode.Percent, ColorGradient.Create(new RGB(160, 0, byte.MaxValue), new RGB(0, 0, byte.MaxValue)));
		this.BindEventWrapper("AMMO", 0, 100, EventIconId.Ammo, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x0600933F RID: 37695 RVA: 0x0037AD78 File Offset: 0x00378F78
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

	// Token: 0x06009340 RID: 37696 RVA: 0x0037ADF8 File Offset: 0x00378FF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventDurability()
	{
		RGBPerkeyZoneCustom rgbperkeyZoneCustom = ScriptableObject.CreateInstance<RGBPerkeyZoneCustom>();
		rgbperkeyZoneCustom.zone = new byte[] { 83, 84, 85, 86 };
		AbstractHandler abstractHandler = ColorHandler.Create(rgbperkeyZoneCustom, IlluminationMode.Percent, ColorGradient.Create(new RGB(130, 130, 0), new RGB(0, byte.MaxValue, 200)));
		this.BindEventWrapper("DURABILITY", 0, 100, EventIconId.Item, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x06009341 RID: 37697 RVA: 0x0037AE6C File Offset: 0x0037906C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventStealth()
	{
		RGBPerkeyZoneCustom rgbperkeyZoneCustom = ScriptableObject.CreateInstance<RGBPerkeyZoneCustom>();
		rgbperkeyZoneCustom.zone = new byte[] { 224, 225, 57, 43, 53 };
		AbstractHandler abstractHandler = ColorHandler.Create(rgbperkeyZoneCustom, IlluminationMode.Percent, ColorGradient.Create(new RGB(50, 120, 50), new RGB(0, 250, 250)));
		this.BindEventWrapper("STEALTH", 0, 100, EventIconId.Default, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x06009342 RID: 37698 RVA: 0x0037AED8 File Offset: 0x003790D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventBleeding()
	{
		AbstractHandler abstractHandler = ColorHandler.Create(ScriptableObject.CreateInstance<RGBPerkeyZoneNavCluster>(), IlluminationMode.Color, ColorStatic.Create(byte.MaxValue, 0, 0), RateStatic.Create(5U, 0U));
		this.BindEventWrapper("BLEEDING", 0, 1, EventIconId.Default, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x06009343 RID: 37699 RVA: 0x0037AF20 File Offset: 0x00379120
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

	// Token: 0x06009344 RID: 37700 RVA: 0x0037B070 File Offset: 0x00379270
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

	// Token: 0x06009345 RID: 37701 RVA: 0x0037B15C File Offset: 0x0037935C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventHit()
	{
		AbstractHandler abstractHandler = ColorHandler.Create(ScriptableObject.CreateInstance<MouseZoneAll>(), IlluminationMode.Color, ColorStatic.Create(byte.MaxValue, 0, 0), RateStatic.Create(8U, 0U));
		this.BindEventWrapper("HIT", 0, 1, EventIconId.Headshot, false, new AbstractHandler[] { abstractHandler });
	}

	// Token: 0x06009346 RID: 37702 RVA: 0x0037B1A4 File Offset: 0x003793A4
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

	// Token: 0x06009347 RID: 37703 RVA: 0x0037B21F File Offset: 0x0037941F
	public void Cleanup()
	{
		GameStats.OnChangedDelegates -= this.GameStats_OnChanged;
	}

	// Token: 0x06009348 RID: 37704 RVA: 0x0037B232 File Offset: 0x00379432
	[PublicizedFrom(EAccessModifier.Private)]
	public void BindEventWrapper(string _eventName, int _minValue, int _maxValue, EventIconId _icon, bool _optionalValue, params AbstractHandler[] _handlers)
	{
		GSClient.Instance.BindEvent(_eventName, _minValue, _maxValue, _icon, _handlers, _optionalValue);
		this.registeredEvents.Add(_eventName);
	}

	// Token: 0x06009349 RID: 37705 RVA: 0x0037B254 File Offset: 0x00379454
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetEventValues()
	{
		foreach (string text in this.registeredEvents)
		{
			GSClient.Instance.SendEvent(text, 0, null);
		}
	}

	// Token: 0x0600934A RID: 37706 RVA: 0x0037B2B0 File Offset: 0x003794B0
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

	// Token: 0x0600934B RID: 37707 RVA: 0x0037B33C File Offset: 0x0037953C
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

	// Token: 0x0600934C RID: 37708 RVA: 0x0037B378 File Offset: 0x00379578
	[PublicizedFrom(EAccessModifier.Private)]
	public void GameStats_OnChanged(EnumGameStats _gameStat, object _newValue)
	{
		if (_gameStat == EnumGameStats.BloodMoonDay)
		{
			this.bloodMoonDay = GameStats.GetInt(EnumGameStats.BloodMoonDay);
		}
	}

	// Token: 0x0600934D RID: 37709 RVA: 0x0037B38C File Offset: 0x0037958C
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

	// Token: 0x0600934E RID: 37710 RVA: 0x0037B3B4 File Offset: 0x003795B4
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

	// Token: 0x0600934F RID: 37711 RVA: 0x0037B408 File Offset: 0x00379608
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

	// Token: 0x06009350 RID: 37712 RVA: 0x0037B516 File Offset: 0x00379716
	public void UpdateEventHealth(int _value)
	{
		if (!GSClient.Instance.isClientActive())
		{
			return;
		}
		GSClient.Instance.SendEvent("HEALTH", _value, null);
	}

	// Token: 0x06009351 RID: 37713 RVA: 0x0037B538 File Offset: 0x00379738
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

	// Token: 0x06009352 RID: 37714 RVA: 0x0037B5F0 File Offset: 0x003797F0
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

	// Token: 0x06009353 RID: 37715 RVA: 0x0037B653 File Offset: 0x00379853
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEventBleeding(bool _isBleeding)
	{
		GSClient.Instance.SendEvent("BLEEDING", _isBleeding ? 1 : 0, null);
	}

	// Token: 0x06009354 RID: 37716 RVA: 0x0037B66C File Offset: 0x0037986C
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

	// Token: 0x06009355 RID: 37717 RVA: 0x0037B734 File Offset: 0x00379934
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

	// Token: 0x06009356 RID: 37718 RVA: 0x0037B8A4 File Offset: 0x00379AA4
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

	// Token: 0x06009357 RID: 37719 RVA: 0x0037B8F9 File Offset: 0x00379AF9
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

	// Token: 0x06009358 RID: 37720 RVA: 0x0037B908 File Offset: 0x00379B08
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

	// Token: 0x06009359 RID: 37721 RVA: 0x0037B9B0 File Offset: 0x00379BB0
	public void EntityBuffAdded(BuffValue _buff)
	{
		if (_buff.BuffClass.Name.EqualsCaseInsensitive("buffInjuryBleeding"))
		{
			this.UpdateEventBleeding(true);
		}
	}

	// Token: 0x0600935A RID: 37722 RVA: 0x0037B9D0 File Offset: 0x00379BD0
	public void EntityBuffRemoved(BuffValue _buff)
	{
		if (_buff.BuffClass.Name.EqualsCaseInsensitive("buffInjuryBleeding"))
		{
			this.UpdateEventBleeding(false);
		}
	}

	// Token: 0x04006E02 RID: 28162
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<string> registeredEvents = new HashSet<string>();

	// Token: 0x04006E03 RID: 28163
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameSenseGameName = "7dtd";

	// Token: 0x04006E04 RID: 28164
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameSenseGameNameFull = "7 Days to Die";

	// Token: 0x04006E05 RID: 28165
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameSenseGameDeveloper = "The Fun Pimps";

	// Token: 0x04006E06 RID: 28166
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventHealth = "HEALTH";

	// Token: 0x04006E07 RID: 28167
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventAmmo = "AMMO";

	// Token: 0x04006E08 RID: 28168
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventTime = "TIME";

	// Token: 0x04006E09 RID: 28169
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventDurability = "DURABILITY";

	// Token: 0x04006E0A RID: 28170
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventStealth = "STEALTH";

	// Token: 0x04006E0B RID: 28171
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventBleeding = "BLEEDING";

	// Token: 0x04006E0C RID: 28172
	[PublicizedFrom(EAccessModifier.Private)]
	public const string BleedingBuffName = "buffInjuryBleeding";

	// Token: 0x04006E0D RID: 28173
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventBloodmoon = "BLOODMOON";

	// Token: 0x04006E0E RID: 28174
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BloodmoonBlinkWarningHours = 2;

	// Token: 0x04006E0F RID: 28175
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventHit = "HIT";

	// Token: 0x04006E10 RID: 28176
	[PublicizedFrom(EAccessModifier.Private)]
	public const float HitEventDuration = 0.5f;

	// Token: 0x04006E11 RID: 28177
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EventDukes = "DUKES";

	// Token: 0x04006E12 RID: 28178
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DukesStringValueKey = "dukesstring";

	// Token: 0x04006E13 RID: 28179
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DukesLabelLocKey = "gamesenseDukesLabel";

	// Token: 0x04006E14 RID: 28180
	[PublicizedFrom(EAccessModifier.Private)]
	public WeakReference<EntityPlayerLocal> entityPlayer;

	// Token: 0x04006E15 RID: 28181
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastTimePercent = -1;

	// Token: 0x04006E16 RID: 28182
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousSlotIndex = -1;

	// Token: 0x04006E17 RID: 28183
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousAmmoPercentage = -1;

	// Token: 0x04006E18 RID: 28184
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousDurability = -1;

	// Token: 0x04006E19 RID: 28185
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousStealth = -1;

	// Token: 0x04006E1A RID: 28186
	[PublicizedFrom(EAccessModifier.Private)]
	public int bloodMoonDay;

	// Token: 0x04006E1B RID: 28187
	[PublicizedFrom(EAccessModifier.Private)]
	public int bloodMoonWarning;

	// Token: 0x04006E1C RID: 28188
	[TupleElementNames(new string[] { "duskHour", "dawnHour" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<int, int> duskDawnTime;

	// Token: 0x04006E1D RID: 28189
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastBloodMoonValue = -1;

	// Token: 0x04006E1E RID: 28190
	[PublicizedFrom(EAccessModifier.Private)]
	public GameSenseManager.EDirection45 lastDirection = GameSenseManager.EDirection45.None;

	// Token: 0x04006E1F RID: 28191
	[PublicizedFrom(EAccessModifier.Private)]
	public float stopHitEventTime = -1f;

	// Token: 0x04006E20 RID: 28192
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine stopHitEventCoroutine;

	// Token: 0x04006E21 RID: 28193
	[PublicizedFrom(EAccessModifier.Private)]
	public float dukesNextTime;

	// Token: 0x04006E22 RID: 28194
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue dukesItem;

	// Token: 0x02001209 RID: 4617
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EDirection45
	{
		// Token: 0x04006E24 RID: 28196
		N,
		// Token: 0x04006E25 RID: 28197
		NE,
		// Token: 0x04006E26 RID: 28198
		E,
		// Token: 0x04006E27 RID: 28199
		SE,
		// Token: 0x04006E28 RID: 28200
		S,
		// Token: 0x04006E29 RID: 28201
		SW,
		// Token: 0x04006E2A RID: 28202
		W,
		// Token: 0x04006E2B RID: 28203
		NW,
		// Token: 0x04006E2C RID: 28204
		None
	}
}
