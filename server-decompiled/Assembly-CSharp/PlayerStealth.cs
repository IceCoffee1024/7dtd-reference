using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000501 RID: 1281
public struct PlayerStealth
{
	// Token: 0x060029A7 RID: 10663 RVA: 0x00106A5A File Offset: 0x00104C5A
	public void Init(EntityPlayer _player)
	{
		this.player = _player;
		this.playerLocal = _player as EntityPlayerLocal;
		this.noises = new List<PlayerStealth.NoiseData>();
		this.barColorUI = new Color32(0, 0, 0, byte.MaxValue);
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x00106A90 File Offset: 0x00104C90
	public void TickServer()
	{
		float num = this.player.speedForward * this.player.speedForward + this.player.speedStrafe * this.player.speedStrafe;
		if (num > 0.01f)
		{
			this.speedAverage = Utils.FastLerpUnclamped(this.speedAverage, (float)Math.Sqrt((double)num), 0.2f);
		}
		else
		{
			this.speedAverage *= 0.5f;
		}
		float num3;
		float num2 = LightManager.GetStealthLightLevel(this.player, out num3);
		float num4 = num3 / (num2 + 0.05f);
		num4 = Utils.FastClamp(num4, 0.5f, 3.2f);
		num2 += num3 * num4;
		if (this.player.IsCrouching)
		{
			num2 *= 0.6f;
		}
		this.player.Buffs.SetCustomVar("_lightlevel", num2 * 100f, true, CVarOperation.set, false);
		num2 *= 1f + this.speedAverage * 0.15f;
		float num5 = EffectManager.GetValue(PassiveEffects.LightMultiplier, null, 1f, this.player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.lightAttackPercent = ((num3 < 0.1f) ? num5 : 1f);
		num5 = 0.32f + 0.68f * num5;
		float num6 = num2 * num5 * 100f;
		this.lightLevel = Utils.FastClamp(num6, 0f, 200f);
		this.NoiseCleanup();
		float num7 = this.CalcVolume();
		this.player.Buffs.SetCustomVar("_noiselevel", this.noiseVolume, true, CVarOperation.set, false);
		int num8 = this.sleeperNoiseWaitTicks - 1;
		this.sleeperNoiseWaitTicks = num8;
		if (num8 <= 0)
		{
			this.sleeperNoiseVolume -= 2.5f;
			if (this.sleeperNoiseVolume < 0f)
			{
				this.sleeperNoiseVolume = 0f;
			}
		}
		if (num7 > 0f)
		{
			float num9 = num7 * 0.6f;
			float num10 = EAIManager.CalcSenseScale();
			num9 *= 1f + num10 * 1.6f;
			num9 = Utils.FastMin(num9, 40f + 15f * num10);
			this.player.world.GetEntitiesAround(EntityFlags.AIHearing, this.player.position, num9, PlayerStealth.entityTempList);
			for (int i = 0; i < PlayerStealth.entityTempList.Count; i++)
			{
				EntityAlive entityAlive = (EntityAlive)PlayerStealth.entityTempList[i];
				float distance = this.player.GetDistance(entityAlive);
				float num11 = this.noiseVolume * (1f + num10 * entityAlive.aiManager.feralSense);
				EntityHuman entityHuman = entityAlive as EntityHuman;
				if (entityHuman != null && entityHuman.IsStormEffected())
				{
					num11 *= 2f;
				}
				num11 /= distance * 0.6f + 0.4f;
				num11 *= this.player.DetectUsScale(entityAlive);
				if (num11 >= 1f)
				{
					bool flag = true;
					if (entityAlive.noisePlayer)
					{
						flag = num11 > entityAlive.noisePlayerVolume;
					}
					if (flag)
					{
						entityAlive.noisePlayer = this.player;
						entityAlive.noisePlayerDistance = distance;
						entityAlive.noisePlayerVolume = num11;
					}
				}
			}
			PlayerStealth.entityTempList.Clear();
		}
		this.AttractTickServer();
		if (GamePrefs.GetInt(EnumGamePrefs.AISmellMode) != 0)
		{
			this.SmellTickServer();
		}
		num8 = this.alertEnemiesTicks - 1;
		this.alertEnemiesTicks = num8;
		if (num8 <= 0)
		{
			this.alertEnemiesTicks = 20;
			this.alertEnemy = false;
			this.player.world.GetEntitiesAround(EntityFlags.AIHearing, this.player.position, 12f, PlayerStealth.entityTempList);
			for (int j = 0; j < PlayerStealth.entityTempList.Count; j++)
			{
				if (((EntityAlive)PlayerStealth.entityTempList[j]).IsAlert)
				{
					this.alertEnemy = true;
					break;
				}
			}
			PlayerStealth.entityTempList.Clear();
			this.SetBarColor(this.alertEnemy);
		}
		if (this.player.isEntityRemote)
		{
			if (this.sendTickDelay > 0)
			{
				this.sendTickDelay--;
			}
			if ((this.player.IsCrouching && this.sendTickDelay == 0 && (this.lightLevelSent != (int)this.lightLevel || this.noiseVolumeSent != (int)this.noiseVolume)) || this.alertEnemySent != this.alertEnemy)
			{
				this.sendTickDelay = 16;
				this.lightLevelSent = (int)this.lightLevel;
				this.noiseVolumeSent = (int)this.noiseVolume;
				this.alertEnemySent = this.alertEnemy;
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityStealth>().Setup(this.player, this.lightLevelSent, this.noiseVolumeSent, this.alertEnemySent), false, this.player.entityId, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x00106F59 File Offset: 0x00105159
	public void TickLocalClient()
	{
		if (GamePrefs.GetInt(EnumGamePrefs.AISmellMode) != 0)
		{
			this.SmellTickClient();
		}
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x00106F70 File Offset: 0x00105170
	[PublicizedFrom(EAccessModifier.Private)]
	public void AttractTickServer()
	{
		int num = this.attractEmitTicks - 1;
		this.attractEmitTicks = num;
		if (num <= 0)
		{
			this.attractEmitTicks = 40;
			float customVar = this.player.Buffs.GetCustomVar(ItemClassHeldEntity.CVarEntityStress);
			if (customVar > 0f)
			{
				float num2 = EAIManager.CalcSenseScale();
				float num3 = customVar * (1f + num2);
				this.player.world.GetEntitiesAround(EntityFlags.AIHearing, this.player.position, num3, PlayerStealth.entityTempList);
				for (int i = 0; i < PlayerStealth.entityTempList.Count; i++)
				{
					if (this.player.rand.RandomFloat < 0.2f)
					{
						EntityAlive entityAlive = (EntityAlive)PlayerStealth.entityTempList[i];
						if (this.player.DetectUsScale(entityAlive) >= 1f)
						{
							float distance = this.player.GetDistance(entityAlive);
							bool flag = true;
							if (entityAlive.attractPlayer && entityAlive.attractPlayer != this.player)
							{
								flag = distance < entityAlive.attractPlayerDistance;
							}
							if (flag)
							{
								entityAlive.attractPlayer = this.player;
								entityAlive.attractPlayerDistance = distance;
								entityAlive.attractPlayerTimeoutTicks = 80;
							}
						}
					}
				}
				PlayerStealth.entityTempList.Clear();
			}
		}
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x001070C0 File Offset: 0x001052C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellApplyMode()
	{
		if (GamePrefs.GetInt(EnumGamePrefs.AISmellMode) == 0)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.player.Buffs.HasCustomVar("smell"))
			{
				this.player.Buffs.SetCustomVar("smell", 0f, true, CVarOperation.set, false);
			}
			this.SmellClear();
		}
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x00107120 File Offset: 0x00105320
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellTickServer()
	{
		if (this.player.IsSpectator || this.player.IsGodMode.Value)
		{
			this.SmellClear();
			this.player.Buffs.SetCustomVar("smell", 0f, true, CVarOperation.set, false);
			return;
		}
		this.SmellTickWet();
		int num = this.smellUpdateItemsTicks - 1;
		this.smellUpdateItemsTicks = num;
		if (num <= 0)
		{
			this.smellUpdateItemsTicks = 40;
			if (!this.player.isEntityRemote)
			{
				this.SmellUpdateItemsAndBlood();
			}
		}
		if (this.smellRadius < 1f)
		{
			if (this.smellRadiusTarget < 1f)
			{
				this.smellRadius = 0f;
			}
			else
			{
				this.smellRadius = Utils.FastMoveTowards(this.smellRadius, this.smellRadiusTarget, 0.0045454544f);
				if (this.smellRadius >= 1f)
				{
					this.smellRadius = 10f;
					this.smellUpdateCVarTicks = 0;
				}
			}
		}
		else
		{
			float num2 = 5f;
			if (this.smellRadiusTarget < this.smellRadius)
			{
				num2 = ((!this.smellSheltered) ? 2f : 10f);
			}
			this.smellRadius = Utils.FastMoveTowards(this.smellRadius, this.smellRadiusTarget, num2 * 0.05f);
			if (this.smellRadius < 1f)
			{
				this.smellRadius = 0f;
				this.smellUpdateCVarTicks = 0;
			}
		}
		num = this.smellUpdateCVarTicks - 1;
		this.smellUpdateCVarTicks = num;
		if (num <= 0)
		{
			this.smellUpdateCVarTicks = 20;
			float num3 = ((this.smellRadius < 5f) ? this.smellRadius : ((float)((int)(this.smellRadius + 0.5f))));
			this.player.Buffs.SetCustomVar("smell", num3, true, CVarOperation.set, false);
			BuffValue buff = this.player.Buffs.GetBuff("buffSmellCheck");
			if (buff != null)
			{
				buff.DurationTriggerUpdate();
			}
		}
		if (this.smellRadius >= 5f)
		{
			num = this.smellEmitTicks - 1;
			this.smellEmitTicks = num;
			if (num <= 0)
			{
				this.smellEmitTicks = 40;
				float num4 = EAIManager.CalcSenseScale();
				float num5 = this.smellRadius * (1f + num4);
				this.player.world.GetEntitiesAround(EntityFlags.AISmelling, this.player.position, num5, PlayerStealth.entityTempList);
				for (int i = 0; i < PlayerStealth.entityTempList.Count; i++)
				{
					if (this.player.rand.RandomFloat < 0.2f)
					{
						EntityAlive entityAlive = (EntityAlive)PlayerStealth.entityTempList[i];
						if (this.player.DetectUsScale(entityAlive) >= 1f)
						{
							float distance = this.player.GetDistance(entityAlive);
							bool flag = true;
							if (entityAlive.smellPlayer && entityAlive.smellPlayer != this.player)
							{
								flag = distance < entityAlive.smellPlayerDistance;
							}
							if (flag)
							{
								entityAlive.smellPlayer = this.player;
								entityAlive.smellPlayerDistance = distance;
								entityAlive.smellPlayerTimeoutTicks = 240;
							}
						}
					}
				}
				PlayerStealth.entityTempList.Clear();
			}
		}
		this.SmellTickEat();
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x0010742C File Offset: 0x0010562C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellTickClient()
	{
		this.SmellTickWet();
		int num = this.smellUpdateItemsTicks - 1;
		this.smellUpdateItemsTicks = num;
		if (num <= 0)
		{
			this.smellUpdateItemsTicks = 40;
			this.SmellUpdateItemsAndBlood();
			int num2 = (int)this.smellRadiusTarget;
			if (num2 != this.smellRadiusTargetSent || this.smellSheltered != this.smellShelteredSent)
			{
				this.smellRadiusTargetSent = num2;
				this.smellShelteredSent = this.smellSheltered;
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEntityStealth>().Setup(this.playerLocal, num2, this.smellEatRadius > 0f, this.smellSheltered), false);
			}
		}
		this.SmellTickEat();
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x001074C8 File Offset: 0x001056C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellTickWet()
	{
		this.smellWetRate = this.player.Buffs.GetCustomVar("_wetnessrate");
		if (this.smellWetRate >= 0.01f)
		{
			this.smellWet += this.smellWetRate;
		}
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x00107508 File Offset: 0x00105708
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellTickEat()
	{
		if (this.smellEatTicks > 0)
		{
			if (this.smellEatTicks <= 1640)
			{
				this.smellEatRadius -= 0.0071428576f;
			}
			int num = this.smellEatTicks - 1;
			this.smellEatTicks = num;
			if (num <= 0 || this.smellEatRadius < 1f)
			{
				this.smellEatRadius = 0f;
				this.smellUpdateItemsTicks = 0;
			}
		}
	}

	// Token: 0x060029B0 RID: 10672 RVA: 0x00107570 File Offset: 0x00105770
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellUpdateItemsAndBlood()
	{
		if (this.smellWet >= 3f || this.player.IsDead())
		{
			this.SmellClear();
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEntityStealth>().Setup(this.playerLocal, -1, false, this.smellSheltered), false);
			}
			return;
		}
		if (this.player.Buffs.GetCustomVar(".dysenterySmell") > 0f)
		{
			this.player.Buffs.RemoveCustomVar(".dysenterySmell");
			this.SetSmellEat(35f);
		}
		int num = this.SmellCountItems();
		if (this.smellWetRate >= 0.01f)
		{
			num = 0;
		}
		float num2 = this.SmellCountToRadius((float)num);
		this.smellRadiusTarget = Utils.FastMax(num2, this.smellEatRadius);
		this.smellSheltered = false;
		if (this.playerLocal.shelterPercent > 0f)
		{
			this.smellRadiusTarget *= 0.2f;
			this.smellSheltered = true;
		}
	}

	// Token: 0x060029B1 RID: 10673 RVA: 0x0010766E File Offset: 0x0010586E
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellClear()
	{
		this.smellRadiusTarget = 0f;
		this.smellRadius = 0f;
		this.smellEatRadius = 0f;
		this.smellEatTicks = 0;
		this.smellSheltered = false;
		this.smellWet = 0f;
	}

	// Token: 0x060029B2 RID: 10674 RVA: 0x001076AC File Offset: 0x001058AC
	[PublicizedFrom(EAccessModifier.Private)]
	public int SmellCountItems()
	{
		float num = 0f;
		ItemStack currentStack = this.playerLocal.PlayerUI.xui.DragAndDropWindow.CurrentStack;
		if (!currentStack.IsEmpty())
		{
			num += currentStack.itemValue.ItemClass.Smell * (float)currentStack.count;
		}
		Inventory inventory = this.player.inventory;
		int length = inventory.Length;
		for (int i = 0; i < length; i++)
		{
			ItemStack itemStack = inventory.GetItemStack(i);
			if (itemStack != null && itemStack.count > 0)
			{
				num += itemStack.itemValue.ItemClass.Smell * (float)itemStack.count;
			}
		}
		for (int j = 0; j < this.player.bag.SlotCount; j++)
		{
			ItemStack itemStack2 = this.player.bag.ItemGrid[j];
			if (itemStack2.count > 0)
			{
				ItemClass itemClass = itemStack2.itemValue.ItemClass;
				if (itemClass != null)
				{
					num += itemClass.Smell * (float)itemStack2.count;
				}
			}
		}
		num = Utils.FastMin(num, 50f);
		return (int)num;
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x001077CC File Offset: 0x001059CC
	[PublicizedFrom(EAccessModifier.Private)]
	public float SmellCountToRadius(float _count)
	{
		_count -= 5f;
		float num = 0f;
		if (_count >= 0f)
		{
			num = Utils.FastLerp(10f, 100f, _count / 45f);
		}
		return num;
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x00107808 File Offset: 0x00105A08
	public void SetSmellRadiusTarget(int _radius, bool _eating, bool _sheltered)
	{
		this.smellRadiusTarget = (float)_radius;
		if (_eating)
		{
			this.smellRadius = Utils.FastMax(this.smellRadius, 1f);
		}
		this.smellSheltered = _sheltered;
		if (_radius < 0)
		{
			this.SmellClear();
		}
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x0010783C File Offset: 0x00105A3C
	public void SetSmellEat(float _distance)
	{
		this.smellEatRadius = Utils.FastMin(this.smellEatRadius + _distance, 100f);
		this.smellEatTicks = 1800;
		this.smellRadius = Utils.FastMax(this.smellRadius, 1f);
		this.smellUpdateItemsTicks = 0;
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x0010788C File Offset: 0x00105A8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetBarColor(bool _isAlert)
	{
		this.barColorUI.r = 50;
		this.barColorUI.g = 135;
		if (_isAlert)
		{
			this.barColorUI.r = 180;
			this.barColorUI.g = 180;
		}
	}

	// Token: 0x060029B7 RID: 10679 RVA: 0x001078DC File Offset: 0x00105ADC
	[PublicizedFrom(EAccessModifier.Private)]
	public void NoiseCleanup()
	{
		for (int i = 0; i < this.noises.Count; i++)
		{
			PlayerStealth.NoiseData noiseData = this.noises[i];
			if (noiseData.ticks > 1)
			{
				noiseData.ticks--;
				this.noises[i] = noiseData;
			}
			else
			{
				this.noises.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x060029B8 RID: 10680 RVA: 0x00107940 File Offset: 0x00105B40
	[PublicizedFrom(EAccessModifier.Private)]
	public float CalcVolume()
	{
		float num = 0f;
		float num2 = 1f;
		for (int i = 0; i < this.noises.Count; i++)
		{
			num += this.noises[i].volume * num2;
			num2 *= 0.6f;
		}
		this.noiseVolume = Mathf.Pow(num * 2.35f, 0.86f);
		this.noiseVolume *= 1.5f;
		this.noiseVolume *= EffectManager.GetValue(PassiveEffects.NoiseMultiplier, null, 1f, this.player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		return num;
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x001079EC File Offset: 0x00105BEC
	public bool CanSleeperAttackDetect(EntityAlive _e)
	{
		if (this.player.IsCrouching)
		{
			float num = Utils.FastLerp(3f, 15f, this.lightAttackPercent);
			if (_e.GetDistance(this.player) > num)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x00107A2E File Offset: 0x00105C2E
	public void SetClientLevels(float _lightLevel, float _noiseVolume, bool _isAlert)
	{
		this.lightLevel = _lightLevel;
		this.noiseVolume = _noiseVolume;
		this.alertEnemy = _isAlert;
		this.SetBarColor(_isAlert);
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x00107A4C File Offset: 0x00105C4C
	public bool NotifyNoise(float volume, float duration)
	{
		if (volume <= 0f)
		{
			return false;
		}
		this.AddNoise(this.noises, volume, (int)(duration * 20f));
		if (volume >= 11f)
		{
			this.sleeperNoiseWaitTicks = 20;
		}
		float num = volume;
		if (volume > 60f)
		{
			num = 60f + Mathf.Pow(volume - 60f, 1.4f);
		}
		num *= EffectManager.GetValue(PassiveEffects.NoiseMultiplier, null, 1f, this.player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.sleeperNoiseVolume += num;
		if (this.sleeperNoiseVolume >= 360f)
		{
			this.sleeperNoiseVolume = 360f;
			return true;
		}
		return false;
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x00107AFC File Offset: 0x00105CFC
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddNoise(List<PlayerStealth.NoiseData> list, float volume, int ticks)
	{
		PlayerStealth.NoiseData noiseData = new PlayerStealth.NoiseData(volume, ticks);
		for (int i = 0; i < list.Count; i++)
		{
			PlayerStealth.NoiseData noiseData2 = this.noises[i];
			if (volume >= noiseData2.volume)
			{
				list.Insert(i, noiseData);
				return;
			}
		}
		list.Insert(list.Count, noiseData);
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x00107B50 File Offset: 0x00105D50
	public static PlayerStealth Read(EntityPlayer _player, BinaryReader br)
	{
		int num = br.ReadInt32();
		PlayerStealth playerStealth = default(PlayerStealth);
		playerStealth.Init(_player);
		playerStealth.lightLevel = (float)br.ReadInt32();
		int num2 = br.ReadInt32();
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				br.ReadSingle();
				float num3 = br.ReadSingle();
				int num4 = br.ReadInt32();
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					playerStealth.AddNoise(playerStealth.noises, num3, num4);
				}
			}
		}
		if (num >= 4)
		{
			playerStealth.smellRadiusTarget = br.ReadSingle();
			playerStealth.smellRadius = br.ReadSingle();
			playerStealth.smellEatRadius = br.ReadSingle();
			playerStealth.smellEatTicks = (int)br.ReadInt16();
			playerStealth.smellSheltered = br.ReadBoolean();
		}
		playerStealth.SmellApplyMode();
		return playerStealth;
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x00107C1C File Offset: 0x00105E1C
	public void Write(BinaryWriter bw)
	{
		bw.Write(4);
		bw.Write(this.lightLevel);
		bw.Write((this.noises != null) ? this.noises.Count : 0);
		if (this.noises != null)
		{
			for (int i = 0; i < this.noises.Count; i++)
			{
				PlayerStealth.NoiseData noiseData = this.noises[i];
				bw.Write(0f);
				bw.Write(noiseData.volume);
				bw.Write(noiseData.ticks);
			}
		}
		bw.Write(this.smellRadiusTarget);
		bw.Write(this.smellRadius);
		bw.Write(this.smellEatRadius);
		bw.Write((short)this.smellEatTicks);
		bw.Write(this.smellSheltered);
	}

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x060029BF RID: 10687 RVA: 0x00107CE3 File Offset: 0x00105EE3
	public Color32 ValueColorUI
	{
		get
		{
			return this.barColorUI;
		}
	}

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x060029C0 RID: 10688 RVA: 0x00107CEC File Offset: 0x00105EEC
	public float ValuePercentUI
	{
		get
		{
			float num = this.player.Buffs.GetCustomVar(ItemClassHeldEntity.CVarEntityStress) / 100f;
			float num2 = this.smellRadius / 100f;
			return Utils.FastClamp01((this.lightLevel + this.noiseVolume * 0.5f + (num + num2) * 50f + (float)(this.alertEnemy ? 5 : 0)) * 0.01f + 0.005f);
		}
	}

	// Token: 0x04001FA9 RID: 8105
	public const float cLightLevelMax = 200f;

	// Token: 0x04001FAA RID: 8106
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLightMpyBase = 0.32f;

	// Token: 0x04001FAB RID: 8107
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 4;

	// Token: 0x04001FAC RID: 8108
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cNextSoundPercent = 0.6f;

	// Token: 0x04001FAD RID: 8109
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSleeperNoiseDecay = 50f;

	// Token: 0x04001FAE RID: 8110
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSleeperNoiseHear = 360f;

	// Token: 0x04001FAF RID: 8111
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSleeperNoiseWaitTicks = 20;

	// Token: 0x04001FB0 RID: 8112
	public float lightLevel;

	// Token: 0x04001FB1 RID: 8113
	[PublicizedFrom(EAccessModifier.Private)]
	public float lightAttackPercent;

	// Token: 0x04001FB2 RID: 8114
	public float noiseVolume;

	// Token: 0x04001FB3 RID: 8115
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedAverage;

	// Token: 0x04001FB4 RID: 8116
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer player;

	// Token: 0x04001FB5 RID: 8117
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal playerLocal;

	// Token: 0x04001FB6 RID: 8118
	[PublicizedFrom(EAccessModifier.Private)]
	public int sendTickDelay;

	// Token: 0x04001FB7 RID: 8119
	[PublicizedFrom(EAccessModifier.Private)]
	public int lightLevelSent;

	// Token: 0x04001FB8 RID: 8120
	[PublicizedFrom(EAccessModifier.Private)]
	public int noiseVolumeSent;

	// Token: 0x04001FB9 RID: 8121
	[PublicizedFrom(EAccessModifier.Private)]
	public bool alertEnemySent;

	// Token: 0x04001FBA RID: 8122
	[PublicizedFrom(EAccessModifier.Private)]
	public int sleeperNoiseWaitTicks;

	// Token: 0x04001FBB RID: 8123
	[PublicizedFrom(EAccessModifier.Private)]
	public float sleeperNoiseVolume;

	// Token: 0x04001FBC RID: 8124
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlayerStealth.NoiseData> noises;

	// Token: 0x04001FBD RID: 8125
	[PublicizedFrom(EAccessModifier.Private)]
	public int alertEnemiesTicks;

	// Token: 0x04001FBE RID: 8126
	[PublicizedFrom(EAccessModifier.Private)]
	public bool alertEnemy;

	// Token: 0x04001FBF RID: 8127
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 barColorUI;

	// Token: 0x04001FC0 RID: 8128
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Entity> entityTempList = new List<Entity>();

	// Token: 0x04001FC1 RID: 8129
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cAttractEmitRate = 2;

	// Token: 0x04001FC2 RID: 8130
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAttractEmitChance = 0.2f;

	// Token: 0x04001FC3 RID: 8131
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAttractRadiusMax = 100f;

	// Token: 0x04001FC4 RID: 8132
	[PublicizedFrom(EAccessModifier.Private)]
	public int attractEmitTicks;

	// Token: 0x04001FC5 RID: 8133
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellStartDelay = 11;

	// Token: 0x04001FC6 RID: 8134
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellUpdateItemsTicksMax = 40;

	// Token: 0x04001FC7 RID: 8135
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellCountMin = 5f;

	// Token: 0x04001FC8 RID: 8136
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellCountMax = 50f;

	// Token: 0x04001FC9 RID: 8137
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellRadiusMin = 10f;

	// Token: 0x04001FCA RID: 8138
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellRadiusMax = 100f;

	// Token: 0x04001FCB RID: 8139
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellRadiusPerSecondUp = 5f;

	// Token: 0x04001FCC RID: 8140
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellRadiusPerSecondDown = 2f;

	// Token: 0x04001FCD RID: 8141
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellEatRadiusPerSecondDown = 0.14285715f;

	// Token: 0x04001FCE RID: 8142
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellShelterRadiusPerSecondDown = 10f;

	// Token: 0x04001FCF RID: 8143
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellBleedRadius = 25f;

	// Token: 0x04001FD0 RID: 8144
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellDysenteryRadius = 35f;

	// Token: 0x04001FD1 RID: 8145
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellEmitRate = 2;

	// Token: 0x04001FD2 RID: 8146
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellEmitChance = 0.2f;

	// Token: 0x04001FD3 RID: 8147
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellDuration = 90;

	// Token: 0x04001FD4 RID: 8148
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellWetClear = 3f;

	// Token: 0x04001FD5 RID: 8149
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellWetRateMin = 0.01f;

	// Token: 0x04001FD6 RID: 8150
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellUpdateItemsTicks;

	// Token: 0x04001FD7 RID: 8151
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellUpdateCVarTicks;

	// Token: 0x04001FD8 RID: 8152
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellEmitTicks;

	// Token: 0x04001FD9 RID: 8153
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellRadiusTarget;

	// Token: 0x04001FDA RID: 8154
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellRadius;

	// Token: 0x04001FDB RID: 8155
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellEatRadius;

	// Token: 0x04001FDC RID: 8156
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellEatTicks;

	// Token: 0x04001FDD RID: 8157
	[PublicizedFrom(EAccessModifier.Private)]
	public bool smellSheltered;

	// Token: 0x04001FDE RID: 8158
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellWet;

	// Token: 0x04001FDF RID: 8159
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellWetRate;

	// Token: 0x04001FE0 RID: 8160
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellRadiusTargetSent;

	// Token: 0x04001FE1 RID: 8161
	[PublicizedFrom(EAccessModifier.Private)]
	public bool smellShelteredSent;

	// Token: 0x02000502 RID: 1282
	[PublicizedFrom(EAccessModifier.Private)]
	public struct NoiseData
	{
		// Token: 0x060029C2 RID: 10690 RVA: 0x00107D6A File Offset: 0x00105F6A
		public NoiseData(float _volume, int _ticks)
		{
			this.volume = _volume;
			this.ticks = _ticks;
		}

		// Token: 0x04001FE2 RID: 8162
		public float volume;

		// Token: 0x04001FE3 RID: 8163
		public int ticks;
	}
}
