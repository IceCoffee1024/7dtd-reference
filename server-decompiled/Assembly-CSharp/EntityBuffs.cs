using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using Twitch;

// Token: 0x0200065E RID: 1630
public class EntityBuffs
{
	// Token: 0x060034D2 RID: 13522 RVA: 0x0015D948 File Offset: 0x0015BB48
	public EntityBuffs(EntityAlive _parent)
	{
		this.parent = _parent;
		this.ActiveBuffs = new List<BuffValue>();
		this.CVars = new CaseInsensitiveStringDictionary<float>();
		this.CVarsLastNetSync = new CaseInsensitiveStringDictionary<float>();
		this.TrackedCVars = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	}

	// Token: 0x060034D3 RID: 13523 RVA: 0x0015D9A0 File Offset: 0x0015BBA0
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

	// Token: 0x060034D4 RID: 13524 RVA: 0x0015DBB4 File Offset: 0x0015BDB4
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

	// Token: 0x060034D5 RID: 13525 RVA: 0x0015DC08 File Offset: 0x0015BE08
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

	// Token: 0x060034D6 RID: 13526 RVA: 0x0015DC60 File Offset: 0x0015BE60
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

	// Token: 0x060034D7 RID: 13527 RVA: 0x0015DCAA File Offset: 0x0015BEAA
	public void FireEvent(MinEventTypes _eventType, BuffClass _buffClass, MinEventParams _params)
	{
		if (_buffClass != null)
		{
			_buffClass.FireEvent(_eventType, _params);
		}
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x0015DCB7 File Offset: 0x0015BEB7
	public EntityBuffs.BuffStatus AddBuff(string _name, int _instigatorId = -1, bool _netSync = true, bool _fromElectrical = false, float _buffDuration = -1f)
	{
		return this.AddBuff(_name, Vector3i.zero, _instigatorId, _netSync, _fromElectrical, _buffDuration);
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x0015DCCC File Offset: 0x0015BECC
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

	// Token: 0x060034DA RID: 13530 RVA: 0x0015DF60 File Offset: 0x0015C160
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

	// Token: 0x060034DB RID: 13531 RVA: 0x0015DFCC File Offset: 0x0015C1CC
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

	// Token: 0x060034DC RID: 13532 RVA: 0x0015E054 File Offset: 0x0015C254
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

	// Token: 0x060034DD RID: 13533 RVA: 0x0015E0C5 File Offset: 0x0015C2C5
	public void SetBuff(string _name, bool _isAdd)
	{
		if (_isAdd)
		{
			this.AddBuff(_name, -1, true, false, -1f);
			return;
		}
		this.RemoveBuff(_name, -1, true);
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x0015E0E4 File Offset: 0x0015C2E4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasBuff(string _name)
	{
		return this.GetBuff(_name) != null;
	}

	// Token: 0x060034DF RID: 13535 RVA: 0x0015E0F0 File Offset: 0x0015C2F0
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

	// Token: 0x060034E0 RID: 13536 RVA: 0x0015E13C File Offset: 0x0015C33C
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

	// Token: 0x060034E1 RID: 13537 RVA: 0x0015E1A0 File Offset: 0x0015C3A0
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

	// Token: 0x060034E2 RID: 13538 RVA: 0x0015E4B0 File Offset: 0x0015C6B0
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

	// Token: 0x060034E3 RID: 13539 RVA: 0x0015E514 File Offset: 0x0015C714
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

	// Token: 0x060034E4 RID: 13540 RVA: 0x0015E582 File Offset: 0x0015C782
	public void AddCustomVar(string _name, float _initialValue)
	{
		this.SetCustomVar(_name, _initialValue, true, CVarOperation.set, false);
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x0015E58F File Offset: 0x0015C78F
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

	// Token: 0x060034E6 RID: 13542 RVA: 0x0015E5D0 File Offset: 0x0015C7D0
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

	// Token: 0x060034E7 RID: 13543 RVA: 0x0015E724 File Offset: 0x0015C924
	public void SetCustomVarNetwork(string _name, float _value, CVarOperation _operation = CVarOperation.set)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageModifyCVar>().Setup(this.parent, _name, _value, _operation), false, -1, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageModifyCVar>().Setup(this.parent, _name, _value, _operation), false);
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x0015E78C File Offset: 0x0015C98C
	public bool HasCustomVar(string _name)
	{
		return this.CVars.ContainsKey(_name);
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x0015E79C File Offset: 0x0015C99C
	public float GetCustomVar(string _name)
	{
		float num;
		if (this.CVars.TryGetValue(_name, out num))
		{
			return num;
		}
		return 0f;
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x000DB224 File Offset: 0x000D9424
	public static int GetCustomVarId(string _name)
	{
		return _name.GetHashCode();
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x0015E7C0 File Offset: 0x0015C9C0
	public void IncrementCustomVar(string _name, float _amount)
	{
		this.SetCustomVar(_name, _amount, true, CVarOperation.add, false);
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x0015E7CD File Offset: 0x0015C9CD
	public int CountCustomVars()
	{
		return this.CVars.Count;
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x0015E7DA File Offset: 0x0015C9DA
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

	// Token: 0x060034EE RID: 13550 RVA: 0x0015E7F8 File Offset: 0x0015C9F8
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

	// Token: 0x060034EF RID: 13551 RVA: 0x0015E880 File Offset: 0x0015CA80
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

	// Token: 0x060034F0 RID: 13552 RVA: 0x0015E92F File Offset: 0x0015CB2F
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeBuff(BuffValue _buffValue)
	{
		_buffValue.Remove = true;
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x0015E938 File Offset: 0x0015CB38
	public void Write(PooledBinaryWriter _bw, bool _netSync = false)
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

	// Token: 0x060034F2 RID: 13554 RVA: 0x0015EAC8 File Offset: 0x0015CCC8
	public void Read(PooledBinaryReader _br)
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

	// Token: 0x060034F3 RID: 13555 RVA: 0x0015EC08 File Offset: 0x0015CE08
	public void UnPauseAll()
	{
		for (int i = 0; i < this.ActiveBuffs.Count; i++)
		{
			this.ActiveBuffs[i].Paused = false;
		}
	}

	// Token: 0x060034F4 RID: 13556 RVA: 0x0015EC40 File Offset: 0x0015CE40
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

	// Token: 0x04002AC7 RID: 10951
	public static byte Version = 3;

	// Token: 0x04002AC8 RID: 10952
	public EntityAlive parent;

	// Token: 0x04002AC9 RID: 10953
	public List<BuffValue> ActiveBuffs;

	// Token: 0x04002ACA RID: 10954
	[PublicizedFrom(EAccessModifier.Private)]
	public CaseInsensitiveStringDictionary<float> CVars;

	// Token: 0x04002ACB RID: 10955
	[PublicizedFrom(EAccessModifier.Private)]
	public CaseInsensitiveStringDictionary<float> CVarsLastNetSync;

	// Token: 0x04002ACC RID: 10956
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<string> TrackedCVars;

	// Token: 0x04002ACD RID: 10957
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> physicalDamageTypes = FastTags<TagGroup.Global>.Parse("piercing,bashing,slashing,crushing,none,corrosive,barbedwire");

	// Token: 0x04002ACE RID: 10958
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> infection = FastTags<TagGroup.Global>.Parse("infection");

	// Token: 0x04002ACF RID: 10959
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> CVarsToSend = new List<string>();

	// Token: 0x0200065F RID: 1631
	public enum BuffStatus
	{
		// Token: 0x04002AD1 RID: 10961
		Added,
		// Token: 0x04002AD2 RID: 10962
		FailedInvalidName,
		// Token: 0x04002AD3 RID: 10963
		FailedImmune,
		// Token: 0x04002AD4 RID: 10964
		FailedFriendlyFire,
		// Token: 0x04002AD5 RID: 10965
		FailedEditor,
		// Token: 0x04002AD6 RID: 10966
		FailedGameStat
	}
}
