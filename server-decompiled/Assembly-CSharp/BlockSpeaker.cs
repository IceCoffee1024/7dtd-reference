using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000152 RID: 338
[Preserve]
public class BlockSpeaker : BlockPowered
{
	// Token: 0x0600094E RID: 2382 RVA: 0x00040C2C File Offset: 0x0003EE2C
	public BlockSpeaker()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00040C46 File Offset: 0x0003EE46
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("PlaySound"))
		{
			this.playSound = base.Properties.Values["PlaySound"];
		}
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00040C80 File Offset: 0x0003EE80
	public override bool ActivateBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool isOn, bool isPowered)
	{
		byte b = (byte)(((int)_blockValue.meta & -3) | (isOn ? 2 : 0));
		if (_blockValue.meta != b)
		{
			_blockValue.meta = (byte)(((int)_blockValue.meta & -3) | (isOn ? 2 : 0));
			_world.SetBlockRPC(_blockPos, _blockValue);
			try
			{
				if (isOn)
				{
					Manager.BroadcastPlay(_blockPos.ToVector3(), this.playSound, 0f);
				}
				else
				{
					Manager.BroadcastStop(_blockPos.ToVector3(), this.playSound);
				}
			}
			catch (Exception)
			{
			}
		}
		return true;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x00040D1C File Offset: 0x0003EF1C
	public override void OnBlockUnloaded(WorldBase world, Vector3i blockPos, BlockValue blockValue)
	{
		base.OnBlockUnloaded(world, blockPos, blockValue);
		Manager.Stop(blockPos.ToVector3(), this.playSound);
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x00040D39 File Offset: 0x0003EF39
	public override void OnBlockRemoved(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(world, _chunk, _blockPos, _blockValue);
		Manager.Stop(_blockPos.ToVector3(), this.playSound);
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x00040D58 File Offset: 0x0003EF58
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		if ((TileEntityPoweredBlock)_world.GetTileEntity(_blockPos) != null)
		{
			if ((_blockValue.meta & 2) > 0)
			{
				Manager.Play(_blockPos.ToVector3(), this.playSound, -1, false, 1f);
				return;
			}
			Manager.Stop(_blockPos.ToVector3(), this.playSound);
		}
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x00040DB7 File Offset: 0x0003EFB7
	public override TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		return new TileEntityPoweredBlock(chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.Consumer
		};
	}

	// Token: 0x040009BA RID: 2490
	[PublicizedFrom(EAccessModifier.Private)]
	public string playSound;

	// Token: 0x040009BB RID: 2491
	[PublicizedFrom(EAccessModifier.Private)]
	public float soundDelay = 1f;
}
