using System;

// Token: 0x02000C8D RID: 3213
public class TileEntityPoweredBlock : TileEntityPowered
{
	// Token: 0x06006189 RID: 24969 RVA: 0x0026C05A File Offset: 0x0026A25A
	public TileEntityPoweredBlock(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x0600618A RID: 24970 RVA: 0x0026C06A File Offset: 0x0026A26A
	// (set) Token: 0x0600618B RID: 24971 RVA: 0x0026C09C File Offset: 0x0026A29C
	public bool IsToggled
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.PowerItem is PowerConsumerToggle)
			{
				return (this.PowerItem as PowerConsumerToggle).IsToggled;
			}
			return this.isToggled;
		}
		set
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				if (this.PowerItem is PowerConsumerToggle)
				{
					(this.PowerItem as PowerConsumerToggle).IsToggled = value;
				}
				this.isToggled = value;
				base.SetModified();
				return;
			}
			this.isToggled = value;
			base.SetModified();
		}
	}

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x0600618C RID: 24972 RVA: 0x0026C0EE File Offset: 0x0026A2EE
	public override int PowerUsed
	{
		get
		{
			if (this.IsToggled)
			{
				return base.PowerUsed;
			}
			return 0;
		}
	}

	// Token: 0x0600618D RID: 24973 RVA: 0x0026C100 File Offset: 0x0026A300
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
	}

	// Token: 0x0600618E RID: 24974 RVA: 0x0026C10C File Offset: 0x0026A30C
	public override bool Activate(bool activated)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, activated, activated);
	}

	// Token: 0x0600618F RID: 24975 RVA: 0x0026C14C File Offset: 0x0026A34C
	public override bool ActivateOnce()
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlockOnce(world, base.ToWorldPos(), block);
	}

	// Token: 0x06006190 RID: 24976 RVA: 0x0026C18A File Offset: 0x0026A38A
	public override void OnRemove(World world)
	{
		base.OnRemove(world);
		PowerManager.Instance.SetTileEntityUpdate(this, false);
	}

	// Token: 0x06006191 RID: 24977 RVA: 0x0026C19F File Offset: 0x0026A39F
	public override void OnUnload(World world)
	{
		base.OnUnload(world);
		PowerManager.Instance.SetTileEntityUpdate(this, false);
	}

	// Token: 0x06006192 RID: 24978 RVA: 0x0026C1B4 File Offset: 0x0026A3B4
	public override void OnSetLocalChunkPosition()
	{
		base.OnSetLocalChunkPosition();
		if (GameManager.Instance == null)
		{
			return;
		}
		World world = GameManager.Instance.World;
		if (this.chunk != null)
		{
			BlockValue block = this.chunk.GetBlock(base.localChunkPos);
			Block block2 = block.Block;
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				block2.ActivateBlock(world, base.ToWorldPos(), block, base.IsPowered, base.IsPowered);
			}
		}
	}

	// Token: 0x06006193 RID: 24979 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClientUpdate()
	{
	}

	// Token: 0x06006194 RID: 24980 RVA: 0x0026C22C File Offset: 0x0026A42C
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == StreamModeRead.Persistency)
		{
			if (base.UseLocalVersioning())
			{
				_br.ReadUInt16();
			}
			else
			{
				base.GetLegacyForkVersion();
			}
		}
		if (_eStreamMode != StreamModeRead.Persistency)
		{
			if (_eStreamMode == StreamModeRead.FromClient)
			{
				this.isToggled = _br.ReadBoolean();
				if (this.PowerItem is PowerConsumerToggle)
				{
					(this.PowerItem as PowerConsumerToggle).IsToggled = this.isToggled;
					return;
				}
			}
			else
			{
				this.isToggled = _br.ReadBoolean();
			}
		}
	}

	// Token: 0x06006195 RID: 24981 RVA: 0x0026C29F File Offset: 0x0026A49F
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		if (_eStreamMode != StreamModeWrite.Persistency)
		{
			_bw.Write(this.IsToggled);
		}
	}

	// Token: 0x04004C89 RID: 19593
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C8A RID: 19594
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isToggled = true;

	// Token: 0x04004C8B RID: 19595
	public float DelayTime;

	// Token: 0x04004C8C RID: 19596
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;
}
