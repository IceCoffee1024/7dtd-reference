using System;

// Token: 0x02000C82 RID: 3202
public class TileEntityPoweredBlock : TileEntityPowered
{
	// Token: 0x06006164 RID: 24932 RVA: 0x0026AB32 File Offset: 0x00268D32
	public TileEntityPoweredBlock(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x06006165 RID: 24933 RVA: 0x0026AB42 File Offset: 0x00268D42
	// (set) Token: 0x06006166 RID: 24934 RVA: 0x0026AB74 File Offset: 0x00268D74
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

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x06006167 RID: 24935 RVA: 0x0026ABC6 File Offset: 0x00268DC6
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

	// Token: 0x06006168 RID: 24936 RVA: 0x0026ABD8 File Offset: 0x00268DD8
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
	}

	// Token: 0x06006169 RID: 24937 RVA: 0x0026ABE4 File Offset: 0x00268DE4
	public override bool Activate(bool activated)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, activated, activated);
	}

	// Token: 0x0600616A RID: 24938 RVA: 0x0026AC24 File Offset: 0x00268E24
	public override bool ActivateOnce()
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlockOnce(world, base.ToWorldPos(), block);
	}

	// Token: 0x0600616B RID: 24939 RVA: 0x0026AC62 File Offset: 0x00268E62
	public override void OnRemove(World world)
	{
		base.OnRemove(world);
		PowerManager.Instance.SetTileEntityUpdate(this, false);
	}

	// Token: 0x0600616C RID: 24940 RVA: 0x0026AC77 File Offset: 0x00268E77
	public override void OnUnload(World world)
	{
		base.OnUnload(world);
		PowerManager.Instance.SetTileEntityUpdate(this, false);
	}

	// Token: 0x0600616D RID: 24941 RVA: 0x0026AC8C File Offset: 0x00268E8C
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

	// Token: 0x0600616E RID: 24942 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClientUpdate()
	{
	}

	// Token: 0x0600616F RID: 24943 RVA: 0x0026AD04 File Offset: 0x00268F04
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

	// Token: 0x06006170 RID: 24944 RVA: 0x0026AD77 File Offset: 0x00268F77
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

	// Token: 0x04004C64 RID: 19556
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C65 RID: 19557
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isToggled = true;

	// Token: 0x04004C66 RID: 19558
	public float DelayTime;

	// Token: 0x04004C67 RID: 19559
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;
}
