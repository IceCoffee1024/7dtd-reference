using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000150 RID: 336
[Preserve]
public class BlockSpeaker : BlockPowered
{
	// Token: 0x06000948 RID: 2376 RVA: 0x00040B4C File Offset: 0x0003ED4C
	public BlockSpeaker()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x00040B66 File Offset: 0x0003ED66
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("PlaySound"))
		{
			this.playSound = base.Properties.Values["PlaySound"];
		}
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x00040BA0 File Offset: 0x0003EDA0
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

	// Token: 0x0600094B RID: 2379 RVA: 0x00040C3C File Offset: 0x0003EE3C
	public override void OnBlockUnloaded(WorldBase world, Vector3i blockPos, BlockValue blockValue)
	{
		base.OnBlockUnloaded(world, blockPos, blockValue);
		Manager.Stop(blockPos.ToVector3(), this.playSound);
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x00040C59 File Offset: 0x0003EE59
	public override void OnBlockRemoved(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(world, _chunk, _blockPos, _blockValue);
		Manager.Stop(_blockPos.ToVector3(), this.playSound);
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00040C78 File Offset: 0x0003EE78
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

	// Token: 0x0600094E RID: 2382 RVA: 0x00040CD7 File Offset: 0x0003EED7
	public override TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		return new TileEntityPoweredBlock(chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.Consumer
		};
	}

	// Token: 0x040009B7 RID: 2487
	[PublicizedFrom(EAccessModifier.Private)]
	public string playSound;

	// Token: 0x040009B8 RID: 2488
	[PublicizedFrom(EAccessModifier.Private)]
	public float soundDelay = 1f;
}
