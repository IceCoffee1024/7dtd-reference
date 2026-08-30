using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020004F6 RID: 1270
public struct PlayerStealth
{
	// Token: 0x06002988 RID: 10632 RVA: 0x0010511E File Offset: 0x0010331E
	public void Init(EntityPlayer _player)
	{
		this.player = _player;
		this.playerLocal = _player as EntityPlayerLocal;
		this.noises = new List<PlayerStealth.NoiseData>();
		this.barColorUI = new Color32(0, 0, 0, byte.MaxValue);
	}

	// Token: 0x06002989 RID: 10633 RVA: 0x00105154 File Offset: 0x00103354
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
		this.player.Buffs.SetCustomVar("_lightlevel", num2 * 100f, true, CVarOperation.set);
		num2 *= 1f + this.speedAverage * 0.15f;
		float num5 = EffectManager.GetValue(PassiveEffects.LightMultiplier, null, 1f, this.player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.lightAttackPercent = ((num3 < 0.1f) ? num5 : 1f);
		num5 = 0.32f + 0.68f * num5;
		float num6 = num2 * num5 * 100f;
		this.lightLevel = Utils.FastClamp(num6, 0f, 200f);
		this.NoiseCleanup();
		float num7 = this.CalcVolume();
		this.player.Buffs.SetCustomVar("_noiselevel", this.noiseVolume, true, CVarOperation.set);
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

	// Token: 0x0600298A RID: 10634 RVA: 0x0010561B File Offset: 0x0010381B
	public void TickLocalClient()
	{
		if (GamePrefs.GetInt(EnumGamePrefs.AISmellMode) != 0)
		{
			this.SmellTickClient();
		}
	}

	// Token: 0x0600298B RID: 10635 RVA: 0x00105630 File Offset: 0x00103830
	[PublicizedFrom(EAccessModifier.Private)]
	public void AttractTickServer()
	{
		int num = this.attractEmitTicks - 1;
		this.attractEmitTicks = num;
		if (num <= 0)
		{
			this.attractEmitTicks = 40;
			float num2 = 0f;
			if (num2 > 0f)
			{
				float num3 = EAIManager.CalcSenseScale();
				float num4 = num2 * (1f + num3);
				this.player.world.GetEntitiesAround(EntityFlags.AIHearing, this.player.position, num4, PlayerStealth.entityTempList);
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

	// Token: 0x0600298C RID: 10636 RVA: 0x00105770 File Offset: 0x00103970
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellApplyMode()
	{
		if (GamePrefs.GetInt(EnumGamePrefs.AISmellMode) == 0)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.player.Buffs.HasCustomVar("smell"))
			{
				this.player.Buffs.SetCustomVar("smell", 0f, true, CVarOperation.set);
			}
			this.SmellClear();
		}
	}

	// Token: 0x0600298D RID: 10637 RVA: 0x001057D0 File Offset: 0x001039D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellTickServer()
	{
		if (this.player.IsSpectator || this.player.IsGodMode.Value)
		{
			this.SmellClear();
			this.player.Buffs.SetCustomVar("smell", 0f, true, CVarOperation.set);
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
			this.player.Buffs.SetCustomVar("smell", num3, true, CVarOperation.set);
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

	// Token: 0x0600298E RID: 10638 RVA: 0x00105AD8 File Offset: 0x00103CD8
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

	// Token: 0x0600298F RID: 10639 RVA: 0x00105B74 File Offset: 0x00103D74
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmellTickWet()
	{
		this.smellWetRate = this.player.Buffs.GetCustomVar("_wetnessrate");
		if (this.smellWetRate >= 0.01f)
		{
			this.smellWet += this.smellWetRate;
		}
	}

	// Token: 0x06002990 RID: 10640 RVA: 0x00105BB4 File Offset: 0x00103DB4
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

	// Token: 0x06002991 RID: 10641 RVA: 0x00105C1C File Offset: 0x00103E1C
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

	// Token: 0x06002992 RID: 10642 RVA: 0x00105D1A File Offset: 0x00103F1A
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

	// Token: 0x06002993 RID: 10643 RVA: 0x00105D58 File Offset: 0x00103F58
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
		int slotCount = inventory.GetSlotCount();
		for (int i = 0; i < slotCount; i++)
		{
			ItemStack itemStack = inventory.GetItemStack(i);
			if (itemStack != null && itemStack.count > 0)
			{
				num += itemStack.itemValue.ItemClass.Smell * (float)itemStack.count;
			}
		}
		foreach (ItemStack itemStack2 in this.player.bag.GetSlots())
		{
			if (itemStack2 != null && itemStack2.count > 0)
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

	// Token: 0x06002994 RID: 10644 RVA: 0x00105E70 File Offset: 0x00104070
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

	// Token: 0x06002995 RID: 10645 RVA: 0x00105EAC File Offset: 0x001040AC
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

	// Token: 0x06002996 RID: 10646 RVA: 0x00105EE0 File Offset: 0x001040E0
	public void SetSmellEat(float _distance)
	{
		this.smellEatRadius = Utils.FastMin(this.smellEatRadius + _distance, 100f);
		this.smellEatTicks = 1800;
		this.smellRadius = Utils.FastMax(this.smellRadius, 1f);
		this.smellUpdateItemsTicks = 0;
	}

	// Token: 0x06002997 RID: 10647 RVA: 0x00105F30 File Offset: 0x00104130
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

	// Token: 0x06002998 RID: 10648 RVA: 0x00105F80 File Offset: 0x00104180
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

	// Token: 0x06002999 RID: 10649 RVA: 0x00105FE4 File Offset: 0x001041E4
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

	// Token: 0x0600299A RID: 10650 RVA: 0x00106090 File Offset: 0x00104290
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

	// Token: 0x0600299B RID: 10651 RVA: 0x001060D2 File Offset: 0x001042D2
	public void SetClientLevels(float _lightLevel, float _noiseVolume, bool _isAlert)
	{
		this.lightLevel = _lightLevel;
		this.noiseVolume = _noiseVolume;
		this.alertEnemy = _isAlert;
		this.SetBarColor(_isAlert);
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x001060F0 File Offset: 0x001042F0
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

	// Token: 0x0600299D RID: 10653 RVA: 0x001061A0 File Offset: 0x001043A0
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

	// Token: 0x0600299E RID: 10654 RVA: 0x001061F4 File Offset: 0x001043F4
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

	// Token: 0x0600299F RID: 10655 RVA: 0x001062C0 File Offset: 0x001044C0
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

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x060029A0 RID: 10656 RVA: 0x00106387 File Offset: 0x00104587
	public Color32 ValueColorUI
	{
		get
		{
			return this.barColorUI;
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x060029A1 RID: 10657 RVA: 0x00106390 File Offset: 0x00104590
	public float ValuePercentUI
	{
		get
		{
			float num = 0f;
			float num2 = this.smellRadius / 100f;
			return Utils.FastClamp01((this.lightLevel + this.noiseVolume * 0.5f + (num + num2) * 50f + (float)(this.alertEnemy ? 5 : 0)) * 0.01f + 0.005f);
		}
	}

	// Token: 0x04001F72 RID: 8050
	public const float cLightLevelMax = 200f;

	// Token: 0x04001F73 RID: 8051
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLightMpyBase = 0.32f;

	// Token: 0x04001F74 RID: 8052
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 4;

	// Token: 0x04001F75 RID: 8053
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cNextSoundPercent = 0.6f;

	// Token: 0x04001F76 RID: 8054
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSleeperNoiseDecay = 50f;

	// Token: 0x04001F77 RID: 8055
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSleeperNoiseHear = 360f;

	// Token: 0x04001F78 RID: 8056
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSleeperNoiseWaitTicks = 20;

	// Token: 0x04001F79 RID: 8057
	public float lightLevel;

	// Token: 0x04001F7A RID: 8058
	[PublicizedFrom(EAccessModifier.Private)]
	public float lightAttackPercent;

	// Token: 0x04001F7B RID: 8059
	public float noiseVolume;

	// Token: 0x04001F7C RID: 8060
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedAverage;

	// Token: 0x04001F7D RID: 8061
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer player;

	// Token: 0x04001F7E RID: 8062
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal playerLocal;

	// Token: 0x04001F7F RID: 8063
	[PublicizedFrom(EAccessModifier.Private)]
	public int sendTickDelay;

	// Token: 0x04001F80 RID: 8064
	[PublicizedFrom(EAccessModifier.Private)]
	public int lightLevelSent;

	// Token: 0x04001F81 RID: 8065
	[PublicizedFrom(EAccessModifier.Private)]
	public int noiseVolumeSent;

	// Token: 0x04001F82 RID: 8066
	[PublicizedFrom(EAccessModifier.Private)]
	public bool alertEnemySent;

	// Token: 0x04001F83 RID: 8067
	[PublicizedFrom(EAccessModifier.Private)]
	public int sleeperNoiseWaitTicks;

	// Token: 0x04001F84 RID: 8068
	[PublicizedFrom(EAccessModifier.Private)]
	public float sleeperNoiseVolume;

	// Token: 0x04001F85 RID: 8069
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlayerStealth.NoiseData> noises;

	// Token: 0x04001F86 RID: 8070
	[PublicizedFrom(EAccessModifier.Private)]
	public int alertEnemiesTicks;

	// Token: 0x04001F87 RID: 8071
	[PublicizedFrom(EAccessModifier.Private)]
	public bool alertEnemy;

	// Token: 0x04001F88 RID: 8072
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 barColorUI;

	// Token: 0x04001F89 RID: 8073
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Entity> entityTempList = new List<Entity>();

	// Token: 0x04001F8A RID: 8074
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cAttractEmitRate = 2;

	// Token: 0x04001F8B RID: 8075
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAttractEmitChance = 0.2f;

	// Token: 0x04001F8C RID: 8076
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAttractRadiusMax = 100f;

	// Token: 0x04001F8D RID: 8077
	[PublicizedFrom(EAccessModifier.Private)]
	public int attractEmitTicks;

	// Token: 0x04001F8E RID: 8078
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellStartDelay = 11;

	// Token: 0x04001F8F RID: 8079
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellUpdateItemsTicksMax = 40;

	// Token: 0x04001F90 RID: 8080
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellCountMin = 5f;

	// Token: 0x04001F91 RID: 8081
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellCountMax = 50f;

	// Token: 0x04001F92 RID: 8082
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellRadiusMin = 10f;

	// Token: 0x04001F93 RID: 8083
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellRadiusMax = 100f;

	// Token: 0x04001F94 RID: 8084
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellRadiusPerSecondUp = 5f;

	// Token: 0x04001F95 RID: 8085
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellRadiusPerSecondDown = 2f;

	// Token: 0x04001F96 RID: 8086
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellEatRadiusPerSecondDown = 0.14285715f;

	// Token: 0x04001F97 RID: 8087
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellShelterRadiusPerSecondDown = 10f;

	// Token: 0x04001F98 RID: 8088
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellBleedRadius = 25f;

	// Token: 0x04001F99 RID: 8089
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellDysenteryRadius = 35f;

	// Token: 0x04001F9A RID: 8090
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellEmitRate = 2;

	// Token: 0x04001F9B RID: 8091
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellEmitChance = 0.2f;

	// Token: 0x04001F9C RID: 8092
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellDuration = 90;

	// Token: 0x04001F9D RID: 8093
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellWetClear = 3f;

	// Token: 0x04001F9E RID: 8094
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSmellWetRateMin = 0.01f;

	// Token: 0x04001F9F RID: 8095
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellUpdateItemsTicks;

	// Token: 0x04001FA0 RID: 8096
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellUpdateCVarTicks;

	// Token: 0x04001FA1 RID: 8097
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellEmitTicks;

	// Token: 0x04001FA2 RID: 8098
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellRadiusTarget;

	// Token: 0x04001FA3 RID: 8099
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellRadius;

	// Token: 0x04001FA4 RID: 8100
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellEatRadius;

	// Token: 0x04001FA5 RID: 8101
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellEatTicks;

	// Token: 0x04001FA6 RID: 8102
	[PublicizedFrom(EAccessModifier.Private)]
	public bool smellSheltered;

	// Token: 0x04001FA7 RID: 8103
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellWet;

	// Token: 0x04001FA8 RID: 8104
	[PublicizedFrom(EAccessModifier.Private)]
	public float smellWetRate;

	// Token: 0x04001FA9 RID: 8105
	[PublicizedFrom(EAccessModifier.Private)]
	public int smellRadiusTargetSent;

	// Token: 0x04001FAA RID: 8106
	[PublicizedFrom(EAccessModifier.Private)]
	public bool smellShelteredSent;

	// Token: 0x020004F7 RID: 1271
	[PublicizedFrom(EAccessModifier.Private)]
	public struct NoiseData
	{
		// Token: 0x060029A3 RID: 10659 RVA: 0x001063F8 File Offset: 0x001045F8
		public NoiseData(float _volume, int _ticks)
		{
			this.volume = _volume;
			this.ticks = _ticks;
		}

		// Token: 0x04001FAB RID: 8107
		public float volume;

		// Token: 0x04001FAC RID: 8108
		public int ticks;
	}
}
