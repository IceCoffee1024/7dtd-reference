using System;
using UnityEngine;

// Token: 0x02000B97 RID: 2967
public class PropChangeInfo
{
	// Token: 0x0600596C RID: 22892 RVA: 0x00226972 File Offset: 0x00224B72
	public PropChangeInfo(Vector2i chunkPos, int? propId, Vector3? position, Quaternion? rotation, Vector3? scale, BlockValue? blockValue)
	{
		this.ChunkPos = chunkPos;
		this.PropId = propId;
		this.Position = position;
		this.Rotation = rotation;
		this.Scale = scale;
		this.BlockValue = blockValue;
	}

	// Token: 0x0600596D RID: 22893 RVA: 0x002269A8 File Offset: 0x00224BA8
	public static PropChangeInfo Read(PooledBinaryReader br)
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

	// Token: 0x0600596E RID: 22894 RVA: 0x00226A50 File Offset: 0x00224C50
	public void Write(PooledBinaryWriter bw)
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

	// Token: 0x0600596F RID: 22895 RVA: 0x00226B94 File Offset: 0x00224D94
	public override string ToString()
	{
		return string.Format("ChunkId={0}, PropId={1}, Position={2}, Rotation={3}, Scale={4}, BlockValue={5}", new object[] { this.ChunkPos, this.PropId, this.Position, this.Rotation, this.Scale, this.BlockValue });
	}

	// Token: 0x04004596 RID: 17814
	public static readonly PropChangeInfo Empty = new PropChangeInfo.Builder(Vector2i.zero).Build();

	// Token: 0x04004597 RID: 17815
	public readonly Vector2i ChunkPos;

	// Token: 0x04004598 RID: 17816
	public readonly int? PropId;

	// Token: 0x04004599 RID: 17817
	public readonly Vector3? Position;

	// Token: 0x0400459A RID: 17818
	public readonly Quaternion? Rotation;

	// Token: 0x0400459B RID: 17819
	public readonly Vector3? Scale;

	// Token: 0x0400459C RID: 17820
	public readonly BlockValue? BlockValue;

	// Token: 0x02000B98 RID: 2968
	[Flags]
	[PublicizedFrom(EAccessModifier.Private)]
	public enum Flags : byte
	{
		// Token: 0x0400459E RID: 17822
		HasPropId = 1,
		// Token: 0x0400459F RID: 17823
		HasPosition = 2,
		// Token: 0x040045A0 RID: 17824
		HasRotation = 4,
		// Token: 0x040045A1 RID: 17825
		HasScale = 8,
		// Token: 0x040045A2 RID: 17826
		HasBlockValue = 16
	}

	// Token: 0x02000B99 RID: 2969
	public class Builder
	{
		// Token: 0x06005971 RID: 22897 RVA: 0x00226C1B File Offset: 0x00224E1B
		public Builder(int chunkX, int chunkZ)
			: this(new Vector2i(chunkX, chunkZ))
		{
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x00226C2A File Offset: 0x00224E2A
		public Builder(Vector2i chunkPos)
		{
			this.ChunkPos = chunkPos;
		}

		// Token: 0x06005973 RID: 22899 RVA: 0x00226C39 File Offset: 0x00224E39
		public Builder(PropRef propRef)
		{
			this.ChunkPos = propRef.ChunkPos;
			this.PropId = new int?(propRef.PropId);
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x00226C5E File Offset: 0x00224E5E
		public PropChangeInfo Build()
		{
			return new PropChangeInfo(this.ChunkPos, this.PropId, this.Position, this.Rotation, this.Scale, this.BlockValue);
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x00226C89 File Offset: 0x00224E89
		public PropChangeInfo.Builder SetPropId(int propId)
		{
			this.PropId = new int?(propId);
			return this;
		}

		// Token: 0x06005976 RID: 22902 RVA: 0x00226C98 File Offset: 0x00224E98
		public PropChangeInfo.Builder SetPosition(Vector3 position)
		{
			this.Position = new Vector3?(position);
			return this;
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x00226CA7 File Offset: 0x00224EA7
		public PropChangeInfo.Builder SetRotation(Quaternion rotation)
		{
			this.Rotation = new Quaternion?(rotation);
			return this;
		}

		// Token: 0x06005978 RID: 22904 RVA: 0x00226CB6 File Offset: 0x00224EB6
		public PropChangeInfo.Builder SetScale(Vector3 scale)
		{
			this.Scale = new Vector3?(scale);
			return this;
		}

		// Token: 0x06005979 RID: 22905 RVA: 0x00226CC5 File Offset: 0x00224EC5
		public PropChangeInfo.Builder SetBlockValue(BlockValue blockValue)
		{
			this.BlockValue = new BlockValue?(blockValue);
			return this;
		}

		// Token: 0x040045A3 RID: 17827
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Vector2i ChunkPos;

		// Token: 0x040045A4 RID: 17828
		[PublicizedFrom(EAccessModifier.Private)]
		public int? PropId;

		// Token: 0x040045A5 RID: 17829
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3? Position;

		// Token: 0x040045A6 RID: 17830
		[PublicizedFrom(EAccessModifier.Private)]
		public Quaternion? Rotation;

		// Token: 0x040045A7 RID: 17831
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3? Scale;

		// Token: 0x040045A8 RID: 17832
		[PublicizedFrom(EAccessModifier.Private)]
		public BlockValue? BlockValue;
	}
}
