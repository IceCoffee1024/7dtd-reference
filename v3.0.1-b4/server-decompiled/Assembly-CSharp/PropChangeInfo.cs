using System;
using System.IO;
using UnityEngine;

// Token: 0x02000B73 RID: 2931
public class PropChangeInfo
{
	// Token: 0x060058CA RID: 22730 RVA: 0x002209CA File Offset: 0x0021EBCA
	public PropChangeInfo(Vector2i chunkPos, int? propId, Vector3? position, Quaternion? rotation, Vector3? scale, BlockValue? blockValue)
	{
		this.ChunkPos = chunkPos;
		this.PropId = propId;
		this.Position = position;
		this.Rotation = rotation;
		this.Scale = scale;
		this.BlockValue = blockValue;
	}

	// Token: 0x060058CB RID: 22731 RVA: 0x00220A00 File Offset: 0x0021EC00
	public static PropChangeInfo Read(BinaryReader br)
	{
		PropChangeInfo.Builder builder = new PropChangeInfo.Builder(br.ReadInt32(), br.ReadInt32());
		byte b = br.ReadByte();
		if ((b & 1) == 1)
		{
			builder.SetPropId(br.ReadInt32());
		}
		if ((b & 2) == 2)
		{
			builder.SetPosition(StreamUtils.ReadVector3(br));
		}
		if ((b & 4) == 4)
		{
			builder.SetRotation(StreamUtils.ReadQuaterion(br));
		}
		if ((b & 8) == 8)
		{
			builder.SetScale(StreamUtils.ReadVector3(br));
		}
		if ((b & 16) == 16)
		{
			builder.SetBlockValue(new BlockValue
			{
				rawData = br.ReadUInt32(),
				damage = (int)br.ReadUInt16()
			});
		}
		return builder.Build();
	}

	// Token: 0x060058CC RID: 22732 RVA: 0x00220AA8 File Offset: 0x0021ECA8
	public void Write(BinaryWriter bw)
	{
		bw.Write(this.ChunkPos.x);
		bw.Write(this.ChunkPos.y);
		PropChangeInfo.Flags flags = ((this.PropId != null) ? PropChangeInfo.Flags.HasPropId : ((PropChangeInfo.Flags)0)) | ((this.Position != null) ? PropChangeInfo.Flags.HasPosition : ((PropChangeInfo.Flags)0)) | ((this.Rotation != null) ? PropChangeInfo.Flags.HasRotation : ((PropChangeInfo.Flags)0)) | ((this.Scale != null) ? PropChangeInfo.Flags.HasScale : ((PropChangeInfo.Flags)0)) | ((this.BlockValue != null) ? PropChangeInfo.Flags.HasBlockValue : ((PropChangeInfo.Flags)0));
		bw.Write((byte)flags);
		if (this.PropId != null)
		{
			bw.Write(this.PropId.Value);
		}
		if (this.Position != null)
		{
			StreamUtils.Write(bw, this.Position.Value);
		}
		if (this.Rotation != null)
		{
			StreamUtils.Write(bw, this.Rotation.Value);
		}
		if (this.Scale != null)
		{
			StreamUtils.Write(bw, this.Scale.Value);
		}
		if (this.BlockValue != null)
		{
			bw.Write(this.BlockValue.Value.rawData);
			bw.Write((ushort)this.BlockValue.Value.damage);
		}
	}

	// Token: 0x060058CD RID: 22733 RVA: 0x00220BEC File Offset: 0x0021EDEC
	public override string ToString()
	{
		return string.Format("ChunkId={0}, PropId={1}, Position={2}, Rotation={3}, Scale={4}, BlockValue={5}", new object[] { this.ChunkPos, this.PropId, this.Position, this.Rotation, this.Scale, this.BlockValue });
	}

	// Token: 0x040044BD RID: 17597
	public static readonly PropChangeInfo Empty = new PropChangeInfo.Builder(Vector2i.zero).Build();

	// Token: 0x040044BE RID: 17598
	public readonly Vector2i ChunkPos;

	// Token: 0x040044BF RID: 17599
	public readonly int? PropId;

	// Token: 0x040044C0 RID: 17600
	public readonly Vector3? Position;

	// Token: 0x040044C1 RID: 17601
	public readonly Quaternion? Rotation;

	// Token: 0x040044C2 RID: 17602
	public readonly Vector3? Scale;

	// Token: 0x040044C3 RID: 17603
	public readonly BlockValue? BlockValue;

	// Token: 0x02000B74 RID: 2932
	[Flags]
	[PublicizedFrom(EAccessModifier.Private)]
	public enum Flags : byte
	{
		// Token: 0x040044C5 RID: 17605
		HasPropId = 1,
		// Token: 0x040044C6 RID: 17606
		HasPosition = 2,
		// Token: 0x040044C7 RID: 17607
		HasRotation = 4,
		// Token: 0x040044C8 RID: 17608
		HasScale = 8,
		// Token: 0x040044C9 RID: 17609
		HasBlockValue = 16
	}

	// Token: 0x02000B75 RID: 2933
	public class Builder
	{
		// Token: 0x060058CF RID: 22735 RVA: 0x00220C73 File Offset: 0x0021EE73
		public Builder(int chunkX, int chunkZ)
			: this(new Vector2i(chunkX, chunkZ))
		{
		}

		// Token: 0x060058D0 RID: 22736 RVA: 0x00220C82 File Offset: 0x0021EE82
		public Builder(Vector2i chunkPos)
		{
			this.ChunkPos = chunkPos;
		}

		// Token: 0x060058D1 RID: 22737 RVA: 0x00220C91 File Offset: 0x0021EE91
		public Builder(PropRef propRef)
		{
			this.ChunkPos = propRef.ChunkPos;
			this.PropId = new int?(propRef.PropId);
		}

		// Token: 0x060058D2 RID: 22738 RVA: 0x00220CB6 File Offset: 0x0021EEB6
		public PropChangeInfo Build()
		{
			return new PropChangeInfo(this.ChunkPos, this.PropId, this.Position, this.Rotation, this.Scale, this.BlockValue);
		}

		// Token: 0x060058D3 RID: 22739 RVA: 0x00220CE1 File Offset: 0x0021EEE1
		public PropChangeInfo.Builder SetPropId(int propId)
		{
			this.PropId = new int?(propId);
			return this;
		}

		// Token: 0x060058D4 RID: 22740 RVA: 0x00220CF0 File Offset: 0x0021EEF0
		public PropChangeInfo.Builder SetPosition(Vector3 position)
		{
			this.Position = new Vector3?(position);
			return this;
		}

		// Token: 0x060058D5 RID: 22741 RVA: 0x00220CFF File Offset: 0x0021EEFF
		public PropChangeInfo.Builder SetRotation(Quaternion rotation)
		{
			this.Rotation = new Quaternion?(rotation);
			return this;
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x00220D0E File Offset: 0x0021EF0E
		public PropChangeInfo.Builder SetScale(Vector3 scale)
		{
			this.Scale = new Vector3?(scale);
			return this;
		}

		// Token: 0x060058D7 RID: 22743 RVA: 0x00220D1D File Offset: 0x0021EF1D
		public PropChangeInfo.Builder SetBlockValue(BlockValue blockValue)
		{
			this.BlockValue = new BlockValue?(blockValue);
			return this;
		}

		// Token: 0x040044CA RID: 17610
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Vector2i ChunkPos;

		// Token: 0x040044CB RID: 17611
		[PublicizedFrom(EAccessModifier.Private)]
		public int? PropId;

		// Token: 0x040044CC RID: 17612
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3? Position;

		// Token: 0x040044CD RID: 17613
		[PublicizedFrom(EAccessModifier.Private)]
		public Quaternion? Rotation;

		// Token: 0x040044CE RID: 17614
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3? Scale;

		// Token: 0x040044CF RID: 17615
		[PublicizedFrom(EAccessModifier.Private)]
		public BlockValue? BlockValue;
	}
}
