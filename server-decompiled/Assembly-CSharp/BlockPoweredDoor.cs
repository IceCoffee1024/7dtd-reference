using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200013E RID: 318
[Preserve]
public class BlockPoweredDoor : BlockPowered
{
	// Token: 0x0600089C RID: 2204 RVA: 0x0003CCE0 File Offset: 0x0003AEE0
	public BlockPoweredDoor()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0003CD20 File Offset: 0x0003AF20
	public override void Init()
	{
		if (base.Properties.GetValue(Block.PropMultiBlockDim) == null)
		{
			base.Properties.Values[Block.PropMultiBlockDim] = "1,2,1";
		}
		base.Init();
		if (base.Properties.Values.ContainsKey("OpenSound"))
		{
			this.openSound = base.Properties.Values["OpenSound"];
		}
		if (base.Properties.Values.ContainsKey("CloseSound"))
		{
			this.closeSound = base.Properties.Values["CloseSound"];
		}
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0003CDC3 File Offset: 0x0003AFC3
	public static bool IsDoorOpen(byte _metadata)
	{
		return (_metadata & 1) > 0;
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0003CDCC File Offset: 0x0003AFCC
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFace _face)
	{
		if (!this.isMultiBlock || !_blockValue.ischild)
		{
			return !BlockPoweredDoor.IsDoorOpen(_blockValue.meta);
		}
		Vector3i parentPos = this.multiBlockPos.GetParentPos(_blockPos, _blockValue);
		BlockValue block = _world.GetBlock(parentPos);
		if (block.ischild)
		{
			string[] array = new string[5];
			array[0] = "Door on position ";
			int num = 1;
			Vector3i vector3i = parentPos;
			array[num] = vector3i.ToString();
			array[2] = " with value ";
			int num2 = 3;
			BlockValue blockValue = block;
			array[num2] = blockValue.ToString();
			array[4] = " should be a parent but is not! (2)";
			Log.Error(string.Concat(array));
			return true;
		}
		return this.IsMovementBlocked(_world, parentPos, block, _face);
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0003CE78 File Offset: 0x0003B078
	public override bool IsSeeThrough(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (!this.isMultiBlock || !_blockValue.ischild)
		{
			return BlockPoweredDoor.IsDoorOpen(_blockValue.meta);
		}
		Vector3i parentPos = this.multiBlockPos.GetParentPos(_blockPos, _blockValue);
		BlockValue block = _world.GetBlock(parentPos);
		if (block.ischild)
		{
			string[] array = new string[5];
			array[0] = "Door on position ";
			int num = 1;
			Vector3i vector3i = parentPos;
			array[num] = vector3i.ToString();
			array[2] = " with value ";
			int num2 = 3;
			BlockValue blockValue = block;
			array[num2] = blockValue.ToString();
			array[4] = " should be a parent but is not! (1)";
			Log.Error(string.Concat(array));
			return true;
		}
		return this.IsSeeThrough(_world, parentPos, block);
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0003CF1C File Offset: 0x0003B11C
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		if (_blockValue.ischild)
		{
			Vector3i parentPos = _blockValue.Block.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			BlockValue block = _world.GetBlock(parentPos);
			return this.GetBlockActivationCommands(_world, block, parentPos, _entityFocusing);
		}
		TileEntityPoweredBlock tileEntityPoweredBlock = (TileEntityPoweredBlock)_world.GetTileEntity(_blockPos);
		if (tileEntityPoweredBlock != null)
		{
			bool isPowered = tileEntityPoweredBlock.IsPowered;
		}
		this.cmds[0].enabled = flag && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0003CFB0 File Offset: 0x0003B1B0
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_blockValue.ischild)
		{
			Vector3i parentPos = _blockValue.Block.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			BlockValue block = _world.GetBlock(parentPos);
			return this.OnBlockActivated(_commandName, _world, parentPos, block, _player);
		}
		if (_commandName == "take")
		{
			base.takeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay);
			return true;
		}
		return false;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0003D014 File Offset: 0x0003B214
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		if (this.shape is BlockShapeModelEntity && (_oldBlockValue.type != _newBlockValue.type || _oldBlockValue.meta != _newBlockValue.meta) && !_newBlockValue.ischild)
		{
			BlockEntityData blockEntity = ((World)_world).ChunkCache.GetBlockEntity(_blockPos);
			bool flag = BlockPoweredDoor.IsDoorOpen(_newBlockValue.meta);
			bool flag2 = BlockPoweredDoor.IsDoorOpen(_oldBlockValue.meta);
			if (flag != flag2)
			{
				this.updateAnimState(blockEntity, flag);
			}
		}
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0003D09C File Offset: 0x0003B29C
	public override bool OnBlockActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		bool flag = !BlockPoweredDoor.IsDoorOpen(_blockValue.meta);
		this.updateOpenCloseState(flag, _world, _blockPos, _blockValue, false);
		if (_player != null)
		{
			Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, flag ? this.openSound : this.closeSound);
		}
		return true;
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0003D100 File Offset: 0x0003B300
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateAnimState(WorldBase _world, Vector3i _blockPos, bool _bOpen)
	{
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		IChunk chunkSync = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z));
		if (chunkSync == null)
		{
			return;
		}
		BlockEntityData blockEntity = chunkSync.GetBlockEntity(_blockPos);
		if (blockEntity == null || !blockEntity.bHasTransform)
		{
			return;
		}
		this.updateAnimState(blockEntity, _bOpen);
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0003D160 File Offset: 0x0003B360
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateAnimState(BlockEntityData _ebcd, bool _bOpen)
	{
		if (_ebcd != null && _ebcd.bHasTransform)
		{
			Animator[] componentsInChildren = _ebcd.transform.GetComponentsInChildren<Animator>();
			if (componentsInChildren != null)
			{
				for (int i = componentsInChildren.Length - 1; i >= 0; i--)
				{
					Animator animator = componentsInChildren[i];
					animator.enabled = true;
					animator.SetBool(AnimatorDoorState.IsOpenHash, _bOpen);
					animator.SetTrigger(AnimatorDoorState.OpenTriggerHash);
				}
			}
		}
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0003D1B8 File Offset: 0x0003B3B8
	public override bool ActivateBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool isOn, bool isPowered)
	{
		byte meta = _blockValue.meta;
		_blockValue.meta = (byte)(((int)_blockValue.meta & -2) | (isOn ? 1 : 0));
		if (meta != _blockValue.meta)
		{
			_world.SetBlockRPC(_blockPos, _blockValue);
			this.updateAnimState(_world, _blockPos, isOn);
			if (isOn)
			{
				Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, this.openSound);
			}
			else
			{
				Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, this.closeSound);
			}
		}
		return true;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0003D25C File Offset: 0x0003B45C
	public override TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		PowerItem.PowerItemTypes powerItemTypes = PowerItem.PowerItemTypes.Consumer;
		return new TileEntityPoweredBlock(chunk)
		{
			PowerItemType = powerItemTypes
		};
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0003D278 File Offset: 0x0003B478
	public override void ForceAnimationState(BlockValue _blockValue, BlockEntityData _ebcd)
	{
		if (_ebcd != null && _ebcd.bHasTransform)
		{
			Animator[] componentsInChildren = _ebcd.transform.GetComponentsInChildren<Animator>();
			if (componentsInChildren != null)
			{
				bool flag = BlockPoweredDoor.IsDoorOpen(_blockValue.meta);
				for (int i = componentsInChildren.Length - 1; i >= 0; i--)
				{
					Animator animator = componentsInChildren[i];
					animator.enabled = true;
					animator.keepAnimatorStateOnDisable = true;
					animator.SetBool(AnimatorDoorState.IsOpenHash, flag);
					animator.Play(flag ? AnimatorDoorState.OpenHash : AnimatorDoorState.CloseHash, 0, 1f);
				}
			}
		}
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0003D2F4 File Offset: 0x0003B4F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateOpenCloseState(bool _bOpen, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bOnlyLocal)
	{
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		_blockValue.meta = (byte)((_bOpen ? 1 : 0) | ((int)_blockValue.meta & -2));
		if (!_bOnlyLocal)
		{
			_world.SetBlockRPC(_blockPos, _blockValue);
			return;
		}
		chunkCache.SetBlockRaw(_blockPos, _blockValue);
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0003D342 File Offset: 0x0003B542
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (_blockValue.ischild)
		{
			return;
		}
		this.updateOpenCloseState(BlockPoweredDoor.IsDoorOpen(_blockValue.meta), _world, _blockPos, _blockValue, true);
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0003D372 File Offset: 0x0003B572
	public override float GetStepHeight(IBlockAccess world, Vector3i blockPos, BlockValue blockDef, BlockFace stepFace)
	{
		return 0f;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0003D37C File Offset: 0x0003B57C
	public bool IsOpen(IBlockAccess _blockAccess, int _x, int _y, int _z)
	{
		return BlockPoweredDoor.IsDoorOpen(_blockAccess.GetBlock(_x, _y, _z).meta);
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0003D3A0 File Offset: 0x0003B5A0
	public override void RenderDecorations(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, INeighborBlockCache _nBlocks)
	{
		if (!(this.shape is BlockShapeModelEntity) || (_blockValue.meta & 2) == 0)
		{
			this.shape.renderDecorations(_worldPos, _blockValue, _drawPos, _vertices, _lightingAround, _textureFullArray, _meshes, _nBlocks);
		}
	}

	// Token: 0x04000981 RID: 2433
	[PublicizedFrom(EAccessModifier.Private)]
	public string openSound;

	// Token: 0x04000982 RID: 2434
	[PublicizedFrom(EAccessModifier.Private)]
	public string closeSound;

	// Token: 0x04000983 RID: 2435
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockEntityData ebcd;

	// Token: 0x04000984 RID: 2436
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("take", "hand", false, false, null)
	};
}
