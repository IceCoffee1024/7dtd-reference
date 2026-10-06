using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200012A RID: 298
[Preserve]
public class BlockLight : Block
{
	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060007D6 RID: 2006 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00037860 File Offset: 0x00035A60
	public BlockLight()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x000378D4 File Offset: 0x00035AD4
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("RuntimeSwitch"))
		{
			this.isRuntimeSwitch = StringParsers.ParseBool(base.Properties.Values["RuntimeSwitch"], 0, -1, true);
		}
		if (base.Properties.Values.ContainsKey("Model"))
		{
			DataLoader.PreloadBundle(base.Properties.Values["Model"]);
		}
		base.Properties.ParseBool("IgnoreLightsOff", ref this.ignoreLightsOff);
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0003647C File Offset: 0x0003467C
	public override byte GetLightValue(BlockValue _blockValue)
	{
		if ((_blockValue.meta & 2) == 0)
		{
			return 0;
		}
		return base.GetLightValue(_blockValue);
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00037968 File Offset: 0x00035B68
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.IsEditor())
		{
			return null;
		}
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		if ((_blockValue.meta & 2) != 0)
		{
			return string.Format(Localization.Get("useSwitchLightOff", false, null), text);
		}
		return string.Format(Localization.Get("useSwitchLightOn", false, null), text);
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x000379E8 File Offset: 0x00035BE8
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (!(_commandName == "light"))
		{
			if (!(_commandName == "edit"))
			{
				if (_commandName == "trigger")
				{
					XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, false, true);
				}
			}
			else
			{
				TileEntityLight tileEntityLight = (TileEntityLight)_world.GetTileEntity(_blockPos);
				if (_world.IsEditor())
				{
					XUiC_LightEditor.Open(_player.PlayerUI, tileEntityLight, _blockPos, _world as World, this);
					return true;
				}
			}
		}
		else if (_world.IsEditor() && this.updateLightState(_world, _blockPos, _blockValue, true, false))
		{
			return true;
		}
		return false;
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00037A78 File Offset: 0x00035C78
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateLightState(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bSwitchLight = false, bool _enableState = true)
	{
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return false;
		}
		IChunk chunkSync = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z));
		if (chunkSync == null)
		{
			return false;
		}
		BlockEntityData blockEntity = chunkSync.GetBlockEntity(_blockPos);
		if (blockEntity == null || !blockEntity.bHasTransform)
		{
			return false;
		}
		bool flag = (_blockValue.meta & 2) > 0;
		TileEntityLight tileEntityLight = (TileEntityLight)_world.GetTileEntity(_blockPos);
		if (_bSwitchLight)
		{
			flag = !flag;
			_blockValue.meta = (byte)(((int)_blockValue.meta & -3) | (flag ? 2 : 0));
			_world.SetBlockRPC(_blockPos, _blockValue);
		}
		Transform transform = blockEntity.transform.FindInChildren("MainLight");
		if (transform)
		{
			LightLOD component = transform.GetComponent<LightLOD>();
			if (component)
			{
				component.SwitchOnOff(flag, false);
				component.SetBlockEntityData(blockEntity);
				Light light = component.GetLight();
				if (tileEntityLight != null)
				{
					light.type = tileEntityLight.LightType;
					component.MaxIntensity = tileEntityLight.LightIntensity;
					light.color = tileEntityLight.LightColor;
					light.shadows = tileEntityLight.LightShadows;
					component.LightAngle = tileEntityLight.LightAngle;
					component.LightStateType = tileEntityLight.LightState;
					component.StateRate = tileEntityLight.Rate;
					component.FluxDelay = tileEntityLight.Delay;
					component.SetRange(tileEntityLight.LightRange);
					component.SetEmissiveColor(component.bSwitchedOn);
				}
				else
				{
					GameObject gameObject = DataLoader.LoadAsset<GameObject>(base.Properties.GetValue("Model"), false);
					if (gameObject != null)
					{
						Transform transform2 = gameObject.transform.Find("MainLight");
						if (transform2 != null)
						{
							LightLOD component2 = transform2.GetComponent<LightLOD>();
							Light light2 = component2.GetLight();
							if (light != null && light2 != null)
							{
								light.type = light2.type;
								component.MaxIntensity = light2.intensity;
								light.color = light2.color;
								light.shadows = light2.shadows;
								component.LightAngle = light2.spotAngle;
								component.LightStateType = component2.LightStateType;
								component.StateRate = component2.StateRate;
								component.FluxDelay = component2.FluxDelay;
								component.SetRange(light2.range);
								component.SetEmissiveColor(component.bSwitchedOn);
							}
						}
					}
				}
			}
		}
		transform = blockEntity.transform.Find("Point light");
		if (transform)
		{
			LightLOD component3 = transform.GetComponent<LightLOD>();
			if (component3)
			{
				component3.SwitchOnOff(flag, false);
				component3.SetBlockEntityData(blockEntity);
			}
		}
		return true;
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00037D3F File Offset: 0x00035F3F
	public bool IsLightOn(BlockValue _blockValue)
	{
		return (_blockValue.meta & 2) > 0;
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00037D4D File Offset: 0x00035F4D
	public bool OriginalLightState(BlockValue _blockValue)
	{
		return !this.ignoreLightsOff && (_blockValue.meta & 1) > 0;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00037D65 File Offset: 0x00035F65
	public BlockValue SetLightState(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool isOn)
	{
		_blockValue.meta = (byte)(((int)_blockValue.meta & -3) | (isOn ? 2 : 0));
		return _blockValue;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00037D83 File Offset: 0x00035F83
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		this.updateLightState(_world, _blockPos, _newBlockValue, false, true);
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00037D9F File Offset: 0x00035F9F
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		this.updateLightState(_world, _blockPos, _blockValue, false, true);
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00037DB8 File Offset: 0x00035FB8
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = false;
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache != null)
		{
			IChunk chunkSync = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z));
			if (chunkSync != null)
			{
				BlockEntityData blockEntity = chunkSync.GetBlockEntity(_blockPos);
				if (blockEntity != null && blockEntity.bHasTransform)
				{
					Transform transform = blockEntity.transform.Find("MainLight");
					if (transform != null)
					{
						LightLOD component = transform.GetComponent<LightLOD>();
						if (component != null && component.GetLight() != null)
						{
							flag = true;
						}
					}
				}
			}
		}
		this.cmds[0].enabled = _world.IsEditor() || this.isRuntimeSwitch;
		this.cmds[1].enabled = _world.IsEditor() && flag;
		this.cmds[2].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00037EB4 File Offset: 0x000360B4
	public TileEntityLight CreateTileEntity(Chunk chunk)
	{
		return new TileEntityLight(chunk);
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsTileEntitySavedInPrefab()
	{
		return true;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00037EBC File Offset: 0x000360BC
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (_blockValue.ischild)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			bool flag = this.IsLightOn(_blockValue);
			if (this.OriginalLightState(_blockValue) != flag)
			{
				_blockValue.meta = (byte)(((int)_blockValue.meta & -2) | (flag ? 1 : 0));
				_world.SetBlockRPC(_blockPos, _blockValue);
			}
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00037F28 File Offset: 0x00036128
	public override void OnBlockReset(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (_blockValue.ischild)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			bool flag = this.IsLightOn(_blockValue);
			if (this.OriginalLightState(_blockValue) != flag)
			{
				_blockValue.meta = (byte)(((int)_blockValue.meta & -2) | (flag ? 1 : 0));
				_world.SetBlockRPC(_blockPos, _blockValue);
			}
		}
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00037F87 File Offset: 0x00036187
	public override void OnTriggered(EntityPlayer _player, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		base.OnTriggered(_player, _world, _blockPos, _blockValue, _blockChanges, _triggeredBy);
		_blockValue = this.SetLightState(_world, _blockPos, _blockValue, !this.IsLightOn(_blockValue));
		_blockChanges.Add(new BlockChangeInfo(_blockPos, _blockValue));
	}

	// Token: 0x0400091C RID: 2332
	public const int cMetaOriginalState = 1;

	// Token: 0x0400091D RID: 2333
	public const int cMetaOn = 2;

	// Token: 0x0400091E RID: 2334
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isRuntimeSwitch;

	// Token: 0x0400091F RID: 2335
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreLightsOff;

	// Token: 0x04000920 RID: 2336
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("light", "electric_switch", true, false, null),
		new BlockActivationCommand("edit", "tool", true, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};
}
