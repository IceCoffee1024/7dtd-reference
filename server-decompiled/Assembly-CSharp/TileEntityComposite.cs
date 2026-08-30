using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000187 RID: 391
[Preserve]
public class TileEntityComposite : TileEntity
{
	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x0004ABE8 File Offset: 0x00048DE8
	// (set) Token: 0x06000BD7 RID: 3031 RVA: 0x0004ABF0 File Offset: 0x00048DF0
	public IFeatureTriggerCapability.ETriggerRole TriggerRole
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x0004ABF9 File Offset: 0x00048DF9
	// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x0004AC01 File Offset: 0x00048E01
	public bool OverridesPhysicalChecks
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0004AC0A File Offset: 0x00048E0A
	public TileEntityCompositeData TeData
	{
		get
		{
			return this.teData;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0004AC12 File Offset: 0x00048E12
	public bool PlayerPlaced
	{
		get
		{
			return this.Owner != null;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0004AC1D File Offset: 0x00048E1D
	public bool LocalPlayerIsOwner
	{
		get
		{
			return this.Owner != null && this.Owner.Equals(PlatformManager.InternalLocalUserIdentifier);
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0004AC39 File Offset: 0x00048E39
	// (set) Token: 0x06000BDE RID: 3038 RVA: 0x0004AC41 File Offset: 0x00048E41
	public PlatformUserIdentifierAbs Owner
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0004AC4A File Offset: 0x00048E4A
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Composite;
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0004AC4E File Offset: 0x00048E4E
	public TileEntityComposite(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x0004AC57 File Offset: 0x00048E57
	public TileEntityComposite(Chunk _chunk, BlockValue _blockValue)
		: base(_chunk)
	{
		this.InitModulesFromBlock(_blockValue.Block as BlockCompositeTileEntity);
		TileEntityLegacyUtils.MigrateLegacyMetadata(this, _blockValue);
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x0004AC79 File Offset: 0x00048E79
	public TileEntityComposite(Chunk _chunk, BlockCompositeTileEntity _block, TileEntityLegacyUtils.LegacyState _state)
		: base(_chunk)
	{
		this.chunkPos = _state.chunkPos;
		this.heapMapLastTime = _state.heapMapLastTime;
		this.heapMapUpdateTime = _state.heapMapUpdateTime;
		this.InitModulesFromBlock(_block);
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x0004ACAD File Offset: 0x00048EAD
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityComposite(TileEntityComposite _original)
		: base(null)
	{
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x0004ACB8 File Offset: 0x00048EB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitModulesFromBlock(BlockCompositeTileEntity _block)
	{
		if (!TileEntityCompositeData.FeaturesByBlock.TryGetValue(_block, out this.teData))
		{
			throw new Exception("Trying to initialize TileEntityComposite for block " + _block.GetBlockName() + " failed, no feature definitions found");
		}
		this.modulesCustomOrder = new ITileEntityFeature[this.teData.Features.Count];
		this.modulesInternalOrder = new ITileEntityFeature[this.teData.Features.Count];
		for (int i = 0; i < this.teData.Features.Count; i++)
		{
			TileEntityFeatureData tileEntityFeatureData = this.teData.Features[i];
			ITileEntityFeature tileEntityFeature = tileEntityFeatureData.InstantiateModule();
			this.modulesCustomOrder[tileEntityFeatureData.CustomOrder] = tileEntityFeature;
			this.modulesInternalOrder[i] = tileEntityFeature;
		}
		for (int j = 0; j < this.teData.Features.Count; j++)
		{
			TileEntityFeatureData tileEntityFeatureData2 = this.teData.Features[j];
			this.modulesInternalOrder[j].Init(this, tileEntityFeatureData2);
		}
		this.PrecomputeCapabilities();
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x0004ADB8 File Offset: 0x00048FB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void PrecomputeCapabilities()
	{
		this.TriggerRole = IFeatureTriggerCapability.ETriggerRole.None;
		for (int i = 0; i < this.modulesCustomOrder.Length; i++)
		{
			ITileEntityFeature tileEntityFeature = this.modulesCustomOrder[i];
			this.OverridesPhysicalChecks |= tileEntityFeature is IFeaturePhysicalCapabilities;
			IFeatureTriggerCapability featureTriggerCapability = tileEntityFeature as IFeatureTriggerCapability;
			if (featureTriggerCapability != null)
			{
				this.TriggerRole |= featureTriggerCapability.TriggerRole;
			}
		}
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x0004AE1B File Offset: 0x0004901B
	public override TileEntity Clone()
	{
		TileEntityComposite tileEntityComposite = new TileEntityComposite(this);
		tileEntityComposite.CopyFrom(this);
		return tileEntityComposite;
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x0004AE2C File Offset: 0x0004902C
	public override void CopyFrom(TileEntity _other)
	{
		TileEntityComposite tileEntityComposite = _other as TileEntityComposite;
		if (tileEntityComposite == null)
		{
			Log.Warning(string.Format("TEC.CopyFrom with non TEC ({0})", _other));
			return;
		}
		this.InitModulesFromBlock(tileEntityComposite.TeData.Block);
		this.bUserAccessing = _other.IsUserAccessing();
		this.Owner = tileEntityComposite.Owner;
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].CopyFrom(tileEntityComposite);
		}
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x0004AE9C File Offset: 0x0004909C
	public override void UpdateTick(World _world)
	{
		base.UpdateTick(_world);
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateTick(_world);
		}
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x0004AED0 File Offset: 0x000490D0
	public void SetBlockEntityData(BlockEntityData _blockEntityData)
	{
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetBlockEntityData(_blockEntityData);
		}
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x0004AEFC File Offset: 0x000490FC
	public override void OnRemove(World _world)
	{
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnRemove(_world);
		}
		base.OnRemove(_world);
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x0004AF30 File Offset: 0x00049130
	public override void OnLoad()
	{
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnLoad();
		}
		base.OnLoad();
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x0004AF60 File Offset: 0x00049160
	public override void OnUnload(World _world)
	{
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnUnload(_world);
		}
		base.OnUnload(_world);
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x0004AF94 File Offset: 0x00049194
	public override void OnDestroy()
	{
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnDestroy();
		}
		base.OnDestroy();
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x0004AFC4 File Offset: 0x000491C4
	public void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		if (this.TriggerRole == IFeatureTriggerCapability.ETriggerRole.None || this.TriggerRole == IFeatureTriggerCapability.ETriggerRole.Source)
		{
			return;
		}
		for (int i = 0; i < this.modulesCustomOrder.Length; i++)
		{
			IFeatureTriggerCapability featureTriggerCapability = this.modulesCustomOrder[i] as IFeatureTriggerCapability;
			if (featureTriggerCapability != null)
			{
				featureTriggerCapability.OnBlockTriggered(_player, _blockPos, _blockValue, _blockChanges, _triggeredBy);
			}
		}
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x0004B014 File Offset: 0x00049214
	public IEnumerable<IFeaturePhysicalCapabilities> GetOverridesPhysicalChecksModules()
	{
		if (!this.OverridesPhysicalChecks)
		{
			yield break;
		}
		int num;
		for (int i = 0; i < this.modulesCustomOrder.Length; i = num + 1)
		{
			IFeaturePhysicalCapabilities featurePhysicalCapabilities = this.modulesCustomOrder[i] as IFeaturePhysicalCapabilities;
			if (featurePhysicalCapabilities != null)
			{
				yield return featurePhysicalCapabilities;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x0004B024 File Offset: 0x00049224
	public override void UpgradeDowngradeFrom(TileEntity _other)
	{
		base.UpgradeDowngradeFrom(_other);
		TileEntityComposite tileEntityComposite = _other as TileEntityComposite;
		if (tileEntityComposite == null)
		{
			return;
		}
		this.Owner = tileEntityComposite.Owner;
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpgradeDowngradeFrom(tileEntityComposite);
		}
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x0004B070 File Offset: 0x00049270
	public override void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		base.ReplacedBy(_bvOld, _bvNew, _teNew);
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ReplacedBy(_bvOld, _bvNew, _teNew);
		}
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x0004B0A8 File Offset: 0x000492A8
	public override void Reset(FastTags<TagGroup.Global> _questTags)
	{
		base.Reset(_questTags);
		ITileEntityFeature[] array = this.modulesInternalOrder;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Reset(_questTags);
		}
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0004B0DA File Offset: 0x000492DA
	public override bool IsActive(World world)
	{
		return base.IsActive(world);
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x0004B0E3 File Offset: 0x000492E3
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.Owner = _userIdentifier;
		base.SetModified();
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x0004B0F4 File Offset: 0x000492F4
	public string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		ITileEntityFeature[] array = this.modulesCustomOrder;
		for (int i = 0; i < array.Length; i++)
		{
			string activationText = array[i].GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
			if (activationText != null)
			{
				return activationText;
			}
		}
		return null;
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x0004B130 File Offset: 0x00049330
	public BlockActivationCommand[] InitBlockActivationCommands()
	{
		List<ValueTuple<BlockActivationCommand, int>> commands = new List<ValueTuple<BlockActivationCommand, int>>();
		int firsts = 100;
		int regular = 200;
		int last = 300;
		bool flag = false;
		Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> <>9__1;
		foreach (ITileEntityFeature tileEntityFeature in this.modulesCustomOrder)
		{
			ITileEntityFeature tileEntityFeature2 = tileEntityFeature;
			Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(BlockActivationCommand _bac, TileEntityComposite.EBlockCommandOrder _order, TileEntityFeatureData _featureData)
				{
					BlockActivationCommand blockActivationCommand2 = new BlockActivationCommand(_featureData.Name + ":" + _bac.text, _bac.icon, _bac.enabled, _bac.highlighted, null);
					int num3;
					switch (_order)
					{
					case TileEntityComposite.EBlockCommandOrder.First:
					{
						int num2 = firsts;
						firsts = num2 + 1;
						num3 = num2;
						break;
					}
					case TileEntityComposite.EBlockCommandOrder.Normal:
					{
						int num2 = regular;
						regular = num2 + 1;
						num3 = num2;
						break;
					}
					case TileEntityComposite.EBlockCommandOrder.Last:
					{
						int num2 = last;
						last = num2 + 1;
						num3 = num2;
						break;
					}
					default:
						throw new ArgumentOutOfRangeException("_order", _order, null);
					}
					int num4 = num3;
					commands.Add(new ValueTuple<BlockActivationCommand, int>(blockActivationCommand2, num4));
				});
			}
			tileEntityFeature2.InitBlockActivationCommands(action);
			if (!flag)
			{
				IFeatureTriggerCapability featureTriggerCapability = tileEntityFeature as IFeatureTriggerCapability;
				if (featureTriggerCapability != null && featureTriggerCapability.TriggerRole != IFeatureTriggerCapability.ETriggerRole.None)
				{
					flag = true;
					BlockActivationCommand blockActivationCommand = new BlockActivationCommand("IFeatureTriggerCapability:trigger", "wrench", true, false, null);
					int last2 = last;
					last = last2 + 1;
					int num = last2;
					commands.Add(new ValueTuple<BlockActivationCommand, int>(blockActivationCommand, num));
				}
			}
		}
		commands.Sort((ValueTuple<BlockActivationCommand, int> _a, ValueTuple<BlockActivationCommand, int> _b) => _a.Item2.CompareTo(_b.Item2));
		BlockActivationCommand[] array2 = new BlockActivationCommand[commands.Count];
		for (int j = 0; j < commands.Count; j++)
		{
			array2[j] = commands[j].Item1;
		}
		return array2;
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x0004B284 File Offset: 0x00049484
	public bool UpdateBlockActivationCommands(BlockActivationCommand[] _commands, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		int frameCount = Time.frameCount;
		if (frameCount <= this.lastUpdateFrameOfBlockActivationCommands)
		{
			return this.lastUpdateHadEnabledCommands;
		}
		this.lastUpdateHadEnabledCommands = false;
		for (int i = 0; i < _commands.Length; i++)
		{
			_commands[i].enabled = true;
		}
		for (int j = 0; j < _commands.Length; j++)
		{
			ReadOnlyMemory<char> readOnlyMemory;
			ReadOnlyMemory<char> readOnlyMemory2;
			if (!TileEntityComposite.SplitFullCommandName(_commands[j].text, out readOnlyMemory, out readOnlyMemory2))
			{
				_commands[j].enabled = false;
			}
			else if (readOnlyMemory.Span.SequenceEqual<char>("IFeatureTriggerCapability"))
			{
				_commands[j].enabled = _world.IsEditor();
			}
			else
			{
				ITileEntityFeature feature = this.GetFeature(readOnlyMemory);
				foreach (ITileEntityFeature tileEntityFeature in this.modulesInternalOrder)
				{
					_commands[j].enabled = tileEntityFeature.AllowBlockActivationCommand(feature, readOnlyMemory2.Span, _world, _blockPos, _blockValue, _entityFocusing);
					if (!_commands[j].enabled)
					{
						break;
					}
				}
				this.lastUpdateHadEnabledCommands |= _commands[j].enabled;
			}
		}
		this.lastUpdateFrameOfBlockActivationCommands = frameCount;
		return this.lastUpdateHadEnabledCommands;
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x0004B3B4 File Offset: 0x000495B4
	public void OnBlockAdded(Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _owner)
	{
		this.Owner = _owner;
		for (int i = 0; i < this.modulesInternalOrder.Length; i++)
		{
			this.modulesInternalOrder[i].OnAdded(_blockPos, _blockValue);
		}
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x0004B3EC File Offset: 0x000495EC
	public void OnBlockValueChanged(Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		for (int i = 0; i < this.modulesInternalOrder.Length; i++)
		{
			this.modulesInternalOrder[i].OnBlockValueChanged(_blockPos, _oldBlockValue, _newBlockValue);
		}
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x0004B41C File Offset: 0x0004961C
	public void OnBlockReset(Vector3i _blockPos, BlockValue _blockValue)
	{
		for (int i = 0; i < this.modulesInternalOrder.Length; i++)
		{
			this.modulesInternalOrder[i].OnBlockReset(_blockPos, _blockValue);
		}
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0004B44C File Offset: 0x0004964C
	public void OnBlockStartsToFall(Vector3i _blockPos, BlockValue _blockValue)
	{
		for (int i = 0; i < this.modulesInternalOrder.Length; i++)
		{
			this.modulesInternalOrder[i].OnBlockStartsToFall(_blockPos, _blockValue);
		}
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x0004B47C File Offset: 0x0004967C
	public Block.DestroyedResult OnBlockDestroyedBy(Vector3i _blockPos, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool)
	{
		Block.DestroyedResult destroyedResult = Block.DestroyedResult.None;
		for (int i = 0; i < this.modulesCustomOrder.Length; i++)
		{
			Block.DestroyedResult destroyedResult2 = this.modulesCustomOrder[i].OnBlockDestroyedBy(_blockPos, _blockValue, _entityId, _bUseHarvestTool);
			if (destroyedResult == Block.DestroyedResult.None)
			{
				destroyedResult = destroyedResult2;
			}
		}
		return destroyedResult;
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x0004B4B8 File Offset: 0x000496B8
	public Block.DestroyedResult OnBlockDestroyedByExplosion(Vector3i _blockPos, BlockValue _blockValue, int _playerThatStartedExpl)
	{
		Block.DestroyedResult destroyedResult = Block.DestroyedResult.None;
		for (int i = 0; i < this.modulesCustomOrder.Length; i++)
		{
			Block.DestroyedResult destroyedResult2 = this.modulesCustomOrder[i].OnBlockDestroyedByExplosion(_blockPos, _blockValue, _playerThatStartedExpl);
			if (destroyedResult == Block.DestroyedResult.None)
			{
				destroyedResult = destroyedResult2;
			}
		}
		return destroyedResult;
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x0004B4F4 File Offset: 0x000496F4
	public bool OnBlockActivated(BlockActivationCommand[] _commands, string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		ReadOnlyMemory<char> readOnlyMemory;
		ReadOnlyMemory<char> readOnlyMemory2;
		if (!TileEntityComposite.SplitFullCommandName(_commandName, out readOnlyMemory, out readOnlyMemory2))
		{
			return false;
		}
		if (readOnlyMemory.Span.SequenceEqual<char>("IFeatureTriggerCapability"))
		{
			bool flag = this.TriggerRole.HasFlag(IFeatureTriggerCapability.ETriggerRole.Source);
			bool flag2 = this.TriggerRole.HasFlag(IFeatureTriggerCapability.ETriggerRole.TriggeredBy);
			if (flag || flag2)
			{
				XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, flag, flag2);
			}
			return true;
		}
		ITileEntityFeature feature = this.GetFeature(readOnlyMemory);
		return feature != null && feature.OnBlockActivated(readOnlyMemory2.Span, _world, _blockPos, _blockValue, _player);
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x0004B598 File Offset: 0x00049798
	public ITileEntityFeature GetFeature(ReadOnlyMemory<char> _featureName)
	{
		int featureIndex = this.teData.GetFeatureIndex(_featureName);
		if (featureIndex < 0)
		{
			return null;
		}
		return this.modulesInternalOrder[featureIndex];
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x0004B5C0 File Offset: 0x000497C0
	public T GetFeature<T>() where T : class
	{
		int featureIndex = this.teData.GetFeatureIndex<T>();
		if (featureIndex < 0)
		{
			return default(T);
		}
		return (T)((object)this.modulesInternalOrder[featureIndex]);
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x0004B5F4 File Offset: 0x000497F4
	public static bool SplitFullCommandName(string _fullCommandName, out ReadOnlyMemory<char> _moduleName, out ReadOnlyMemory<char> _commandName)
	{
		int num = _fullCommandName.IndexOf(':');
		if (num < 0)
		{
			_moduleName = default(ReadOnlyMemory<char>);
			_commandName = default(ReadOnlyMemory<char>);
			return false;
		}
		_moduleName = _fullCommandName.AsMemory(0, num);
		_commandName = _fullCommandName.AsMemory(num + 1);
		return true;
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x0004B63C File Offset: 0x0004983C
	public override string ToString()
	{
		return string.Format(string.Concat(new string[]
		{
			"[TEC] ",
			this.GetTileEntityType().ToStringCached<TileEntityType>(),
			"/",
			base.ToWorldPos().ToString(),
			"/",
			this.teData.Block.GetBlockName()
		}), Array.Empty<object>());
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x0004B6AE File Offset: 0x000498AE
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		this.read(_br, _eStreamMode, null);
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x0004B6BC File Offset: 0x000498BC
	public void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode, int[] _blockIdMapping)
	{
		base.read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			num = 18;
		}
		else if (base.UseLocalVersioning())
		{
			num = (int)_br.ReadUInt16();
		}
		else
		{
			num = base.GetLegacyForkVersion();
		}
		PooledBinaryReader.StreamReadSizeMarker streamReadSizeMarker = _br.ReadSizeMarker(PooledBinaryWriter.EMarkerSize.UInt32);
		int num2 = _br.ReadInt32();
		int num3 = ((_blockIdMapping == null) ? num2 : _blockIdMapping[num2]);
		BlockCompositeTileEntity blockCompositeTileEntity = Block.list[num3] as BlockCompositeTileEntity;
		if (blockCompositeTileEntity == null)
		{
			string text = "TileEntityComposite.read: Failed reading data, block ";
			Block block = Block.list[num3];
			Log.Error(text + ((block != null) ? block.GetBlockName() : null) + " is not a BlockCompositeTileEntity");
			uint num4;
			_br.ValidateSizeMarker(ref streamReadSizeMarker, out num4, true);
			return;
		}
		try
		{
			this.Owner = PlatformUserIdentifierAbs.FromStream(_br, false, false);
			if (this.modulesInternalOrder == null || this.modulesCustomOrder == null || this.teData == null)
			{
				this.InitModulesFromBlock(blockCompositeTileEntity);
			}
			byte b = _br.ReadByte();
			if (num < 17)
			{
				if ((int)b != this.modulesInternalOrder.Length)
				{
					Log.Warning(string.Format("{0}.{1}: Legacy composite TE (ver {2}) for block \"{3}\" ", new object[]
					{
						"TileEntityComposite",
						"read",
						num,
						blockCompositeTileEntity.GetBlockName()
					}) + string.Format("has {0} features in stream but current definition has {1}. Skipping TE payload.", b, this.modulesInternalOrder.Length));
					uint num4;
					_br.ValidateSizeMarker(ref streamReadSizeMarker, out num4, true);
					return;
				}
				for (int i = 0; i < (int)b; i++)
				{
					ITileEntityFeature tileEntityFeature = this.modulesInternalOrder[i];
					int num5 = _br.ReadInt32();
					if (tileEntityFeature.FeatureData.NameHash != num5)
					{
						Log.Warning(string.Format("{0}.{1}: Legacy composite TE (ver {2}) for block \"{3}\" ", new object[]
						{
							"TileEntityComposite",
							"read",
							num,
							blockCompositeTileEntity.GetBlockName()
						}) + string.Format("hash mismatch at index {0}: stream 0x{1:X8} != expected 0x{2:X8} ({3}). Skipping TE payload.", new object[]
						{
							i,
							num5,
							tileEntityFeature.FeatureData.NameHash,
							tileEntityFeature.FeatureData.Name
						}));
						uint num4;
						_br.ValidateSizeMarker(ref streamReadSizeMarker, out num4, true);
						return;
					}
					tileEntityFeature.Read(_br, _eStreamMode);
				}
				uint num6;
				if (!_br.ValidateSizeMarker(ref streamReadSizeMarker, out num6, true))
				{
					Log.Error("TileEntityComposite.read: Legacy composite TE for block \"" + blockCompositeTileEntity.GetBlockName() + "\" failed size validation: " + string.Format("expected {0} B, read {1} B", streamReadSizeMarker.ExpectedSize, num6));
				}
				return;
			}
			else
			{
				HashSet<int> hashSet = new HashSet<int>((int)b);
				for (int j = 0; j < (int)b; j++)
				{
					int num7 = _br.ReadInt32();
					hashSet.Add(num7);
					PooledBinaryReader.StreamReadSizeMarker streamReadSizeMarker2 = _br.ReadSizeMarker(PooledBinaryWriter.EMarkerSize.UInt32);
					int featureIndex = this.teData.GetFeatureIndex(num7);
					if (featureIndex < 0)
					{
						Log.Warning(string.Format("{0}.{1}: Block \"{2}\" no longer defines feature hash 0x{3:X8}. Skipping payload.", new object[]
						{
							"TileEntityComposite",
							"read",
							blockCompositeTileEntity.GetBlockName(),
							num7
						}));
						uint num4;
						_br.ValidateSizeMarker(ref streamReadSizeMarker2, out num4, true);
					}
					else
					{
						ITileEntityFeature tileEntityFeature2 = this.modulesInternalOrder[featureIndex];
						try
						{
							tileEntityFeature2.Read(_br, _eStreamMode);
						}
						catch (Exception ex)
						{
							Log.Error(string.Format("{0}.{1}: Module read failed for feature \"{2}\" (hash 0x{3:X8}) ", new object[]
							{
								"TileEntityComposite",
								"read",
								tileEntityFeature2.FeatureData.Name,
								num7
							}) + "on block \"" + blockCompositeTileEntity.GetBlockName() + "\": Caught exception:");
							Log.Exception(ex);
						}
						uint num8;
						if (!_br.ValidateSizeMarker(ref streamReadSizeMarker2, out num8, true))
						{
							Log.Error(string.Format("{0}.{1}: Module \"{2}\" failed size validation (hash 0x{3:X8}) ", new object[]
							{
								"TileEntityComposite",
								"read",
								tileEntityFeature2.FeatureData.Name,
								num7
							}) + string.Format("on block \"{0}\": expected {1} B, read {2} B", blockCompositeTileEntity.GetBlockName(), streamReadSizeMarker2.ExpectedSize, num8));
						}
					}
				}
				int num9 = 0;
				for (int k = 0; k < this.teData.Features.Count; k++)
				{
					int nameHash = this.teData.Features[k].NameHash;
					if (!hashSet.Contains(nameHash))
					{
						num9++;
					}
				}
				if (num9 > 0)
				{
					Log.Warning(string.Format("{0}.{1}: Block \"{2}\" has {3} feature(s) in the current definition ", new object[]
					{
						"TileEntityComposite",
						"read",
						blockCompositeTileEntity.GetBlockName(),
						num9
					}) + "that were not present in the saved data. They will use default values.");
				}
			}
		}
		catch (Exception ex2)
		{
			Log.Error("TileEntityComposite.read: Failed reading data for block " + Block.list[num3].GetBlockName() + ": Caught exception:");
			Log.Exception(ex2);
			uint num4;
			_br.ValidateSizeMarker(ref streamReadSizeMarker, out num4, true);
			return;
		}
		uint num10;
		if (!_br.ValidateSizeMarker(ref streamReadSizeMarker, out num10, true))
		{
			Log.Error("TileEntityComposite.read: Failed reading data for block \"" + Block.list[num3].GetBlockName() + "\": " + string.Format("Data received {0} B, read {1} B", streamReadSizeMarker.ExpectedSize, num10));
		}
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x0004BBF4 File Offset: 0x00049DF4
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		PooledBinaryWriter.StreamWriteSizeMarker streamWriteSizeMarker = _bw.ReserveSizeMarker(PooledBinaryWriter.EMarkerSize.UInt32);
		_bw.Write(this.teData.Block.blockID);
		(GameManager.Instance.IsEditMode() ? null : this.Owner).ToStream(_bw, false);
		_bw.Write((byte)this.modulesInternalOrder.Length);
		for (int i = 0; i < this.modulesInternalOrder.Length; i++)
		{
			ITileEntityFeature tileEntityFeature = this.modulesInternalOrder[i];
			_bw.Write(tileEntityFeature.FeatureData.NameHash);
			PooledBinaryWriter.StreamWriteSizeMarker streamWriteSizeMarker2 = _bw.ReserveSizeMarker(PooledBinaryWriter.EMarkerSize.UInt32);
			tileEntityFeature.Write(_bw, _eStreamMode);
			_bw.FinalizeSizeMarker(ref streamWriteSizeMarker2);
		}
		_bw.FinalizeSizeMarker(ref streamWriteSizeMarker);
	}

	// Token: 0x04000AC5 RID: 2757
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04000AC6 RID: 2758
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCompositeData teData;

	// Token: 0x04000AC7 RID: 2759
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileEntityFeature[] modulesCustomOrder;

	// Token: 0x04000AC8 RID: 2760
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileEntityFeature[] modulesInternalOrder;

	// Token: 0x04000ACC RID: 2764
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastUpdateFrameOfBlockActivationCommands;

	// Token: 0x04000ACD RID: 2765
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastUpdateHadEnabledCommands;

	// Token: 0x04000ACE RID: 2766
	[PublicizedFrom(EAccessModifier.Private)]
	public const char ModuleCommandSeparator = ':';

	// Token: 0x04000ACF RID: 2767
	[PublicizedFrom(EAccessModifier.Private)]
	public const string TriggerCommandModule = "IFeatureTriggerCapability";

	// Token: 0x02000188 RID: 392
	public enum EBlockCommandOrder
	{
		// Token: 0x04000AD1 RID: 2769
		First,
		// Token: 0x04000AD2 RID: 2770
		Normal,
		// Token: 0x04000AD3 RID: 2771
		Last
	}
}
