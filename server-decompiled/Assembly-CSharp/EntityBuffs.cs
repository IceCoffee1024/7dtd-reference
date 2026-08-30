using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Platform;
using Twitch;

// Token: 0x0200065C RID: 1628
public class EntityBuffs
{
	// Token: 0x0600348F RID: 13455 RVA: 0x0015CFE4 File Offset: 0x0015B1E4
	public EntityBuffs(EntityAlive _parent)
	{
		this.parent = _parent;
		this.ActiveBuffs = new List<BuffValue>();
		this.CVars = new CaseInsensitiveStringDictionary<float>();
		this.CVarsLastNetSync = new CaseInsensitiveStringDictionary<float>();
		this.TrackedCVars = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x0015D03C File Offset: 0x0015B23C
	public void Tick()
	{
		int num = this.ActiveBuffs.Count;
		for (int i = 0; i < num; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			if (buffValue.Invalid)
			{
				this.ActiveBuffs.RemoveAt(i);
				i--;
				num--;
			}
			else
			{
				this.parent.MinEventContext.Buff = buffValue;
				if (this.parent.MinEventContext.Other == null)
				{
					this.parent.MinEventContext.Other = this.parent.GetAttackTarget();
				}
				if (buffValue.Finished)
				{
					this.FireEvent(MinEventTypes.onSelfBuffFinish, buffValue.BuffClass, this.parent.MinEventContext);
					buffValue.Remove = true;
				}
				if (buffValue.Remove)
				{
					if (buffValue.BuffClass != null)
					{
						this.FireEvent(MinEventTypes.onSelfBuffRemove, buffValue.BuffClass, this.parent.MinEventContext);
						if (!buffValue.BuffClass.Hidden)
						{
							this.parent.Stats.EntityBuffRemoved(buffValue);
						}
					}
					this.ActiveBuffs.RemoveAt(i);
					i--;
					num--;
				}
				else if (!buffValue.Paused && !this.parent.bDead)
				{
					if (!buffValue.Started)
					{
						this.parent.MinEventContext.Instigator = null;
						if (buffValue.InstigatorId != -1)
						{
							this.parent.MinEventContext.Instigator = GameManager.Instance.World.GetEntity(buffValue.InstigatorId) as EntityAlive;
						}
						this.FireEvent(MinEventTypes.onSelfBuffStart, buffValue.BuffClass, this.parent.MinEventContext);
						buffValue.Started = true;
						if (!buffValue.BuffClass.Hidden)
						{
							this.parent.Stats.EntityBuffAdded(buffValue);
						}
						this.parent.BuffAdded(buffValue);
					}
					buffValue.Tick();
					if (buffValue.Update)
					{
						this.FireEvent(MinEventTypes.onSelfBuffUpdate, buffValue.BuffClass, this.parent.MinEventContext);
						buffValue.Update = false;
					}
				}
			}
		}
		this.parent.MinEventContext.Buff = null;
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x0015D250 File Offset: 0x0015B450
	public void ModifyValue(PassiveEffects _effect, ref float _value, ref float _perc_val, FastTags<TagGroup.Global> _tags)
	{
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			BuffClass buffClass = buffValue.BuffClass;
			if (buffClass != null && !buffValue.Paused)
			{
				buffClass.ModifyValue(this.parent, _effect, buffValue, ref _value, ref _perc_val, _tags);
			}
		}
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x0015D2A4 File Offset: 0x0015B4A4
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, PassiveEffects _effect, ref float _value, ref float _perc_val, FastTags<TagGroup.Global> _tags)
	{
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			BuffClass buffClass = buffValue.BuffClass;
			if (buffClass != null && !buffValue.Paused)
			{
				buffClass.GetModifiedValueData(_modValueSources, _sourceType, this.parent, _effect, buffValue, ref _value, ref _perc_val, _tags);
			}
		}
	}

	// Token: 0x06003493 RID: 13459 RVA: 0x0015D2FC File Offset: 0x0015B4FC
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			BuffClass buffClass = buffValue.BuffClass;
			if (buffClass != null && !buffValue.Paused)
			{
				buffClass.FireEvent(_eventType, _params);
			}
		}
	}

	// Token: 0x06003494 RID: 13460 RVA: 0x0015D346 File Offset: 0x0015B546
	public void FireEvent(MinEventTypes _eventType, BuffClass _buffClass, MinEventParams _params)
	{
		if (_buffClass != null)
		{
			_buffClass.FireEvent(_eventType, _params);
		}
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x0015D353 File Offset: 0x0015B553
	public EntityBuffs.BuffStatus AddBuff(string _name, int _instigatorId = -1, bool _netSync = true, bool _fromElectrical = false, float _buffDuration = -1f)
	{
		return this.AddBuff(_name, Vector3i.zero, _instigatorId, _netSync, _fromElectrical, _buffDuration);
	}

	// Token: 0x06003496 RID: 13462 RVA: 0x0015D368 File Offset: 0x0015B568
	public EntityBuffs.BuffStatus AddBuff(string _name, Vector3i _instigatorPos, int _instigatorId = -1, bool _netSync = true, bool _fromElectrical = false, float _buffDuration = -1f)
	{
		int num = -1;
		if (_fromElectrical)
		{
			num = _instigatorId;
			_instigatorId = -1;
		}
		BuffClass buff = BuffManager.GetBuff(_name);
		if (buff == null)
		{
			return EntityBuffs.BuffStatus.FailedInvalidName;
		}
		if (!buff.AllowInEditor && this.parent.world.IsEditor())
		{
			return EntityBuffs.BuffStatus.FailedEditor;
		}
		if (buff.RequiredGameStat != EnumGameStats.Last && !GameStats.GetBool(buff.RequiredGameStat))
		{
			return EntityBuffs.BuffStatus.FailedGameStat;
		}
		if (_netSync && this.HasImmunity(buff))
		{
			return EntityBuffs.BuffStatus.FailedImmune;
		}
		if (buff.DamageType != EnumDamageTypes.None && _instigatorId != this.parent.entityId && !this.parent.FriendlyFireCheck(GameManager.Instance.World.GetEntity(_instigatorId) as EntityAlive))
		{
			return EntityBuffs.BuffStatus.FailedFriendlyFire;
		}
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			if (buffValue.BuffClass.Name == buff.Name)
			{
				if (_buffDuration >= 0f)
				{
					buffValue.BuffClass.DurationMax = _buffDuration;
				}
				switch (buff.StackType)
				{
				case BuffEffectStackTypes.Ignore:
					if (buffValue.Remove)
					{
						buffValue.Remove = false;
					}
					break;
				case BuffEffectStackTypes.Duration:
				{
					float num2 = _buffDuration - buffValue.DurationInSeconds;
					float num3 = buffValue.BuffClass.InitialDurationMax;
					if (_buffDuration >= 0f)
					{
						num3 = _buffDuration;
					}
					if (num2 > num3)
					{
						num3 = num2;
					}
					buffValue.DurationInTicks = 0U;
					buffValue.BuffClass.DurationMax = num3;
					this.FireEvent(MinEventTypes.onSelfBuffStack, buff, this.parent.MinEventContext);
					break;
				}
				case BuffEffectStackTypes.Effect:
				{
					BuffValue buffValue2 = buffValue;
					int stackEffectMultiplier = buffValue2.StackEffectMultiplier;
					buffValue2.StackEffectMultiplier = stackEffectMultiplier + 1;
					this.FireEvent(MinEventTypes.onSelfBuffStack, buff, this.parent.MinEventContext);
					break;
				}
				case BuffEffectStackTypes.Replace:
					buffValue.DurationInTicks = 0U;
					this.FireEvent(MinEventTypes.onSelfBuffStack, buff, this.parent.MinEventContext);
					break;
				}
				if (_netSync)
				{
					this.AddBuffNetwork(_name, _buffDuration, _instigatorPos, _instigatorId);
				}
				return EntityBuffs.BuffStatus.Added;
			}
		}
		if (!this.parent.isEntityRemote && this.parent.entityType == EntityType.Player && buff.Name.EqualsCaseInsensitive("buffLegBroken"))
		{
			IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
			if (achievementManager != null)
			{
				achievementManager.SetAchievementStat(EnumAchievementDataStat.LegBroken, 1);
			}
		}
		if (_fromElectrical)
		{
			_instigatorId = num;
		}
		BuffValue buffValue3 = new BuffValue(buff.Name, _instigatorPos, _instigatorId, buff);
		if (_buffDuration >= 0f)
		{
			buffValue3.BuffClass.DurationMax = _buffDuration;
		}
		else
		{
			buffValue3.BuffClass.DurationMax = buffValue3.BuffClass.InitialDurationMax;
		}
		this.ActiveBuffs.Add(buffValue3);
		if (_netSync)
		{
			this.AddBuffNetwork(_name, _buffDuration, _instigatorPos, _instigatorId);
		}
		return EntityBuffs.BuffStatus.Added;
	}

	// Token: 0x06003497 RID: 13463 RVA: 0x0015D5FC File Offset: 0x0015B7FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddBuffNetwork(string _name, float _duration, Vector3i _instigatorPos, int _instigatorId = -1)
	{
		NetPackageAddRemoveBuff netPackageAddRemoveBuff = NetPackageManager.GetPackage<NetPackageAddRemoveBuff>().Setup(this.parent.entityId, _name, _duration, true, _instigatorId, _instigatorPos);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAddRemoveBuff, false, -1, -1, this.parent.entityId, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageAddRemoveBuff, false);
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x0015D668 File Offset: 0x0015B868
	public void RemoveBuff(string _name, int _instigatorId = -1, bool _netSync = true)
	{
		BuffClass buff = BuffManager.GetBuff(_name);
		if (buff == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			if (this.ActiveBuffs[i].BuffClass.Name == buff.Name)
			{
				this.ActiveBuffs[i].Remove = true;
				flag = true;
			}
		}
		if (flag && _netSync)
		{
			int num = ((_instigatorId != -1) ? _instigatorId : this.parent.entityId);
			this.RemoveBuffNetwork(_name, num);
		}
	}

	// Token: 0x06003499 RID: 13465 RVA: 0x0015D6F0 File Offset: 0x0015B8F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveBuffNetwork(string _name, int _instigatorId)
	{
		NetPackageAddRemoveBuff netPackageAddRemoveBuff = NetPackageManager.GetPackage<NetPackageAddRemoveBuff>().Setup(this.parent.entityId, _name, -1f, false, _instigatorId, Vector3i.zero);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAddRemoveBuff, false, -1, -1, this.parent.entityId, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageAddRemoveBuff, false);
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x0015D761 File Offset: 0x0015B961
	public void SetBuff(string _name, bool _isAdd)
	{
		if (_isAdd)
		{
			this.AddBuff(_name, -1, true, false, -1f);
			return;
		}
		this.RemoveBuff(_name, -1, true);
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x0015D780 File Offset: 0x0015B980
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasBuff(string _name)
	{
		return this.GetBuff(_name) != null;
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x0015D78C File Offset: 0x0015B98C
	public bool HasBuffByTag(FastTags<TagGroup.Global> _tags)
	{
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			if (buffValue != null && _tags.Test_AnySet(buffValue.BuffClass.Tags))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x0015D7D8 File Offset: 0x0015B9D8
	public BuffValue GetBuff(string _buffName)
	{
		BuffClass buff = BuffManager.GetBuff(_buffName);
		if (buff == null)
		{
			return null;
		}
		int count = this.ActiveBuffs.Count;
		for (int i = 0; i < count; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			if (buffValue != null)
			{
				BuffClass buffClass = buffValue.BuffClass;
				if (buffClass != null && buffClass.Name == buff.Name)
				{
					return buffValue;
				}
			}
		}
		return null;
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x0015D83C File Offset: 0x0015BA3C
	public void OnDeath(EntityAlive _entityThatKilledMe, ItemValue _itemThatKilledMe, bool _blockKilledMe, FastTags<TagGroup.Global> _damageTypeTags)
	{
		if (_entityThatKilledMe != null)
		{
			if (_entityThatKilledMe.entityId == this.parent.entityId)
			{
				this.parent.FireEvent(MinEventTypes.onSelfKilledSelf, true);
			}
			else
			{
				this.parent.MinEventContext.Other = _entityThatKilledMe;
				this.parent.FireEvent(MinEventTypes.onOtherKilledSelf, true);
			}
		}
		else if (_blockKilledMe)
		{
			this.parent.FireEvent(MinEventTypes.onBlockKilledSelf, true);
		}
		this.parent.FireEvent(MinEventTypes.onSelfDied, true);
		List<int> list = new List<int>();
		bool flag = this.parent is EntityPlayer;
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			if (buffValue != null && buffValue.BuffClass != null)
			{
				if (!flag && buffValue.BuffClass.RemoveOnDeath && !buffValue.Paused)
				{
					buffValue.Remove = true;
				}
				if (buffValue.BuffClass.DamageType != EnumDamageTypes.None && !buffValue.Invalid && buffValue.Started && (buffValue.InstigatorId != -1 || !(buffValue.InstigatorPos == Vector3i.zero)) && buffValue.InstigatorId != this.parent.entityId && (!(_entityThatKilledMe != null) || buffValue.InstigatorId != _entityThatKilledMe.entityId))
				{
					if (_entityThatKilledMe != null && buffValue.InstigatorPos != Vector3i.zero)
					{
						_entityThatKilledMe = null;
						this.parent.ClearEntityThatKilledMe();
					}
					if (!list.Contains(buffValue.InstigatorId))
					{
						if (flag)
						{
							EntityAlive entityAlive;
							if (_entityThatKilledMe != null)
							{
								entityAlive = _entityThatKilledMe;
							}
							else
							{
								entityAlive = GameManager.Instance.World.GetEntity(buffValue.InstigatorId) as EntityAlive;
							}
							if (buffValue.BuffClass.DamageType == EnumDamageTypes.BloodLoss || buffValue.BuffClass.DamageType == EnumDamageTypes.Electrical || buffValue.BuffClass.DamageType == EnumDamageTypes.Radiation || buffValue.BuffClass.DamageType == EnumDamageTypes.Heat || buffValue.BuffClass.DamageType == EnumDamageTypes.Cold)
							{
								TwitchManager.Current.CheckKiller(this.parent as EntityPlayer, entityAlive, buffValue.InstigatorPos);
							}
						}
						EntityPlayerLocal entityPlayerLocal = GameManager.Instance.World.GetEntity(buffValue.InstigatorId) as EntityPlayerLocal;
						if (!(entityPlayerLocal == null))
						{
							if (!_damageTypeTags.Test_AnySet(EntityBuffs.physicalDamageTypes))
							{
								if (this.parent.Buffs.GetCustomVar("ETrapHit") == 1f)
								{
									float value = EffectManager.GetValue(PassiveEffects.ElectricalTrapXP, entityPlayerLocal.inventory.holdingItemItemValue, 0f, entityPlayerLocal, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
									if (value > 0f)
									{
										entityPlayerLocal.AddKillXP(this.parent, _itemThatKilledMe, value);
										this.parent.AwardKill(entityPlayerLocal);
									}
								}
								else
								{
									entityPlayerLocal.AddKillXP(this.parent, _itemThatKilledMe, 1f);
									this.parent.AwardKill(entityPlayerLocal);
								}
							}
							list.Add(entityPlayerLocal.entityId);
						}
					}
				}
			}
		}
		this.Tick();
	}

	// Token: 0x0600349F RID: 13471 RVA: 0x0015DB4C File Offset: 0x0015BD4C
	public void RemoveBuffsByTag(FastTags<TagGroup.Global> tags)
	{
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			if (buffValue.BuffClass.Tags.Test_AnySet(tags))
			{
				buffValue.Remove = true;
				this.RemoveBuffNetwork(buffValue.BuffName, this.parent.entityId);
			}
		}
	}

	// Token: 0x060034A0 RID: 13472 RVA: 0x0015DBB0 File Offset: 0x0015BDB0
	public void RemoveDeathBuffs(FastTags<TagGroup.Global> excludeTags)
	{
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			BuffValue buffValue = this.ActiveBuffs[i];
			if (buffValue.BuffClass.RemoveOnDeath && !buffValue.BuffClass.Tags.Test_AnySet(excludeTags))
			{
				buffValue.Remove = true;
				this.RemoveBuffNetwork(buffValue.BuffName, this.parent.entityId);
			}
		}
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x0015DC1E File Offset: 0x0015BE1E
	public void AddCustomVar(string _name, float _initialValue)
	{
		this.SetCustomVar(_name, _initialValue, true, CVarOperation.set, false);
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x0015DC2B File Offset: 0x0015BE2B
	public void RemoveCustomVar(string _name)
	{
		if (this.CVars.ContainsKey(_name))
		{
			this.CVars.Remove(_name);
		}
		if (this.TrackedCVars.Contains(_name))
		{
			Log.Out("CVar " + _name + " was removed.");
		}
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x0015DC6C File Offset: 0x0015BE6C
	public void SetCustomVar(string _name, float _value, bool _netSync = true, CVarOperation _operation = CVarOperation.set, bool _forceSendToClients = false)
	{
		bool flag = true;
		float num;
		bool flag2 = this.CVars.TryGetValue(_name, out num);
		switch (_operation)
		{
		case CVarOperation.set:
		case CVarOperation.setvalue:
			if (!flag2 || num != _value)
			{
				this.CVars[_name] = _value;
			}
			else
			{
				flag = false;
			}
			break;
		case CVarOperation.add:
			this.CVars[_name] = num + _value;
			break;
		case CVarOperation.subtract:
			this.CVars[_name] = num - _value;
			break;
		case CVarOperation.multiply:
			this.CVars[_name] = num * _value;
			break;
		case CVarOperation.divide:
			this.CVars[_name] = num / ((_value == 0f) ? 0.0001f : _value);
			break;
		case CVarOperation.percentadd:
			this.CVars[_name] = num + num * _value;
			break;
		case CVarOperation.percentsubtract:
			this.CVars[_name] = num - num * _value;
			break;
		}
		if (flag || _forceSendToClients)
		{
			if (this.TrackedCVars.Contains(_name))
			{
				Log.Out(string.Format("CVar {0} was set to {1}.", _name, this.CVars[_name]));
			}
			if (_netSync)
			{
				if (!this.parent.isEntityRemote && _name[0] != '%' && !_forceSendToClients)
				{
					return;
				}
				if (_name[0] == '.' || _name[0] == '_')
				{
					return;
				}
				this.SetCustomVarNetwork(_name, _value, _operation);
			}
		}
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x0015DDC0 File Offset: 0x0015BFC0
	public void SetCustomVarNetwork(string _name, float _value, CVarOperation _operation = CVarOperation.set)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageModifyCVar>().Setup(this.parent, _name, _value, _operation), false, -1, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageModifyCVar>().Setup(this.parent, _name, _value, _operation), false);
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x0015DE28 File Offset: 0x0015C028
	public bool HasCustomVar(string _name)
	{
		return this.CVars.ContainsKey(_name);
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x0015DE38 File Offset: 0x0015C038
	public float GetCustomVar(string _name)
	{
		float num;
		if (this.CVars.TryGetValue(_name, out num))
		{
			return num;
		}
		return 0f;
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x000DB27C File Offset: 0x000D947C
	public static int GetCustomVarId(string _name)
	{
		return _name.GetHashCode();
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x0015DE5C File Offset: 0x0015C05C
	public void IncrementCustomVar(string _name, float _amount)
	{
		this.SetCustomVar(_name, _amount, true, CVarOperation.add, false);
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x0015DE69 File Offset: 0x0015C069
	public int CountCustomVars()
	{
		return this.CVars.Count;
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x0015DE76 File Offset: 0x0015C076
	public IEnumerable<KeyValuePair<string, float>> EnumerateCustomVars(string searchString = null, bool startsWith = false)
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.CVars)
		{
			if (string.IsNullOrEmpty(searchString))
			{
				yield return keyValuePair;
			}
			else if (startsWith)
			{
				if (keyValuePair.Key.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
				{
					yield return keyValuePair;
				}
			}
			else if (keyValuePair.Key.Contains(searchString, StringComparison.OrdinalIgnoreCase))
			{
				yield return keyValuePair;
			}
		}
		Dictionary<string, float>.Enumerator enumerator = default(Dictionary<string, float>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x0015DE94 File Offset: 0x0015C094
	public void TrackCustomVar(string _name, bool _isTracked)
	{
		if (!_isTracked)
		{
			this.TrackedCVars.Remove(_name);
			Log.Out("Removed tracking from CVar " + _name + ".");
			return;
		}
		this.TrackedCVars.Add(_name);
		if (this.CVars.ContainsKey(_name))
		{
			Log.Out(string.Format("Tracking CVar {0} with value {1}.", _name, this.CVars[_name]));
			return;
		}
		Log.Out("Tracking CVar " + _name + ", it does not exist yet.");
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x0015DF1C File Offset: 0x0015C11C
	public bool HasImmunity(BuffClass _buffClass)
	{
		if (this.parent.IsDead() && _buffClass.RemoveOnDeath)
		{
			return true;
		}
		if (this.parent.HasImmunity(_buffClass))
		{
			return true;
		}
		float num = Utils.FastClamp01(EffectManager.GetValue(PassiveEffects.BuffResistance, null, 0f, this.parent, null, _buffClass.NameTag, true, true, true, true, true, 1, true, false));
		if (_buffClass.Tags.Test_AnySet(EntityBuffs.infection))
		{
			if (EntityPlayerLocal.InfectionChance == 0f)
			{
				num = 1f;
			}
			else
			{
				num *= 1f + (1f - EntityPlayerLocal.InfectionChance);
			}
		}
		return this.parent.rand.RandomFloat <= num;
	}

	// Token: 0x060034AD RID: 13485 RVA: 0x0015DFCB File Offset: 0x0015C1CB
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeBuff(BuffValue _buffValue)
	{
		_buffValue.Remove = true;
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x0015DFD4 File Offset: 0x0015C1D4
	public void Write(BinaryWriter _bw, bool _netSync = false)
	{
		_bw.Write(EntityBuffs.Version);
		_bw.Write((ushort)this.ActiveBuffs.Count);
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			this.ActiveBuffs[i].Write(_bw);
		}
		this.CVarsToSend.Clear();
		foreach (string text in this.CVars.Keys)
		{
			if ((_netSync || this.CVars[text] != 0f) && text[0] != '.' && (!_netSync || !this.CVarsLastNetSync.ContainsKey(text) || this.CVars[text] != this.CVarsLastNetSync[text]))
			{
				this.CVarsToSend.Add(text);
			}
		}
		if (_netSync)
		{
			this.CVarsLastNetSync.Clear();
		}
		_bw.Write((ushort)this.CVarsToSend.Count);
		for (int j = 0; j < this.CVarsToSend.Count; j++)
		{
			_bw.Write(this.CVarsToSend[j]);
			_bw.Write(this.CVars[this.CVarsToSend[j]]);
			if (_netSync)
			{
				this.CVarsLastNetSync.Add(this.CVarsToSend[j], this.CVars[this.CVarsToSend[j]]);
			}
		}
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x0015E164 File Offset: 0x0015C364
	public void Read(BinaryReader _br)
	{
		int num = (int)_br.ReadByte();
		int num2 = (int)_br.ReadUInt16();
		this.ActiveBuffs.Clear();
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				BuffValue buffValue = new BuffValue();
				buffValue.Read(_br, num);
				if (buffValue.BuffClass != null && (!(buffValue.BuffClass.Name == "god") || this.parent.world.IsEditor() || GameModeCreative.TypeName.Equals(GamePrefs.GetString(EnumGamePrefs.GameMode)) || this.parent.IsGodMode.Value))
				{
					this.ActiveBuffs.Add(buffValue);
					if (!buffValue.BuffClass.Hidden)
					{
						this.parent.Stats.EntityBuffAdded(buffValue);
					}
				}
			}
		}
		if (num < 2)
		{
			int num3 = (int)_br.ReadUInt16();
			Dictionary<int, float> dictionary = new Dictionary<int, float>();
			if (num3 > 0)
			{
				for (int j = 0; j < num3; j++)
				{
					dictionary[_br.ReadInt32()] = _br.ReadSingle();
				}
				return;
			}
		}
		else
		{
			int num4 = (int)_br.ReadUInt16();
			if (num4 > 0)
			{
				for (int k = 0; k < num4; k++)
				{
					this.SetCustomVar(_br.ReadString(), _br.ReadSingle(), false, CVarOperation.set, false);
				}
			}
		}
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x0015E2A4 File Offset: 0x0015C4A4
	public void UnPauseAll()
	{
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			this.ActiveBuffs[i].Paused = false;
		}
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x0015E2DC File Offset: 0x0015C4DC
	public void ClearBuffClassLinks()
	{
		foreach (BuffValue buffValue in this.ActiveBuffs)
		{
			if (buffValue != null)
			{
				buffValue.ClearBuffClassLink();
			}
		}
	}

	// Token: 0x04002ACE RID: 10958
	public static byte Version = 3;

	// Token: 0x04002ACF RID: 10959
	public EntityAlive parent;

	// Token: 0x04002AD0 RID: 10960
	public List<BuffValue> ActiveBuffs;

	// Token: 0x04002AD1 RID: 10961
	[PublicizedFrom(EAccessModifier.Private)]
	public CaseInsensitiveStringDictionary<float> CVars;

	// Token: 0x04002AD2 RID: 10962
	[PublicizedFrom(EAccessModifier.Private)]
	public CaseInsensitiveStringDictionary<float> CVarsLastNetSync;

	// Token: 0x04002AD3 RID: 10963
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<string> TrackedCVars;

	// Token: 0x04002AD4 RID: 10964
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> physicalDamageTypes = FastTags<TagGroup.Global>.Parse("piercing,bashing,slashing,crushing,none,corrosive,barbedwire");

	// Token: 0x04002AD5 RID: 10965
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> infection = FastTags<TagGroup.Global>.Parse("infection");

	// Token: 0x04002AD6 RID: 10966
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> CVarsToSend = new List<string>();

	// Token: 0x0200065D RID: 1629
	public enum BuffStatus
	{
		// Token: 0x04002AD8 RID: 10968
		Added,
		// Token: 0x04002AD9 RID: 10969
		FailedInvalidName,
		// Token: 0x04002ADA RID: 10970
		FailedImmune,
		// Token: 0x04002ADB RID: 10971
		FailedFriendlyFire,
		// Token: 0x04002ADC RID: 10972
		FailedEditor,
		// Token: 0x04002ADD RID: 10973
		FailedGameStat
	}
}
