using System;
using System.IO;
using UnityEngine;

// Token: 0x02000B85 RID: 2949
public class PropChangeInfo
{
	// Token: 0x06005952 RID: 22866 RVA: 0x002236E6 File Offset: 0x002218E6
	public PropChangeInfo(Vector2i chunkPos, int? propId, Vector3? position, Quaternion? rotation, Vector3? scale, BlockValue? blockValue)
	{
		this.ChunkPos = chunkPos;
		this.PropId = propId;
		this.Position = position;
		this.Rotation = rotation;
		this.Scale = scale;
		this.BlockValue = blockValue;
	}

	// Token: 0x06005953 RID: 22867 RVA: 0x0022371C File Offset: 0x0022191C
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

	// Token: 0x06005954 RID: 22868 RVA: 0x002237C4 File Offset: 0x002219C4
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

	// Token: 0x06005955 RID: 22869 RVA: 0x00223908 File Offset: 0x00221B08
	public override string ToString()
	{
		return string.Format("ChunkId={0}, PropId={1}, Position={2}, Rotation={3}, Scale={4}, BlockValue={5}", new object[] { this.ChunkPos, this.PropId, this.Position, this.Rotation, this.Scale, this.BlockValue });
	}

	// Token: 0x04004558 RID: 17752
	public static readonly PropChangeInfo Empty = new PropChangeInfo.Builder(Vector2i.zero).Build();

	// Token: 0x04004559 RID: 17753
	public readonly Vector2i ChunkPos;

	// Token: 0x0400455A RID: 17754
	public readonly int? PropId;

	// Token: 0x0400455B RID: 17755
	public readonly Vector3? Position;

	// Token: 0x0400455C RID: 17756
	public readonly Quaternion? Rotation;

	// Token: 0x0400455D RID: 17757
	public readonly Vector3? Scale;

	// Token: 0x0400455E RID: 17758
	public readonly BlockValue? BlockValue;

	// Token: 0x02000B86 RID: 2950
	[Flags]
	[PublicizedFrom(EAccessModifier.Private)]
	public enum Flags : byte
	{
		// Token: 0x04004560 RID: 17760
		HasPropId = 1,
		// Token: 0x04004561 RID: 17761
		HasPosition = 2,
		// Token: 0x04004562 RID: 17762
		HasRotation = 4,
		// Token: 0x04004563 RID: 17763
		HasScale = 8,
		// Token: 0x04004564 RID: 17764
		HasBlockValue = 16
	}

	// Token: 0x02000B87 RID: 2951
	public class Builder
	{
		// Token: 0x06005957 RID: 22871 RVA: 0x0022398F File Offset: 0x00221B8F
		public Builder(int chunkX, int chunkZ)
			: this(new Vector2i(chunkX, chunkZ))
		{
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x0022399E File Offset: 0x00221B9E
		public Builder(Vector2i chunkPos)
		{
			this.ChunkPos = chunkPos;
		}

		// Token: 0x06005959 RID: 22873 RVA: 0x002239AD File Offset: 0x00221BAD
		public Builder(PropRef propRef)
		{
			this.ChunkPos = propRef.ChunkPos;
			this.PropId = new int?(propRef.PropId);
		}

		// Token: 0x0600595A RID: 22874 RVA: 0x002239D2 File Offset: 0x00221BD2
		public PropChangeInfo Build()
		{
			return new PropChangeInfo(this.ChunkPos, this.PropId, this.Position, this.Rotation, this.Scale, this.BlockValue);
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x002239FD File Offset: 0x00221BFD
		public PropChangeInfo.Builder SetPropId(int propId)
		{
			this.PropId = new int?(propId);
			return this;
		}

		// Token: 0x0600595C RID: 22876 RVA: 0x00223A0C File Offset: 0x00221C0C
		public PropChangeInfo.Builder SetPosition(Vector3 position)
		{
			this.Position = new Vector3?(position);
			return this;
		}

		// Token: 0x0600595D RID: 22877 RVA: 0x00223A1B File Offset: 0x00221C1B
		public PropChangeInfo.Builder SetRotation(Quaternion rotation)
		{
			this.Rotation = new Quaternion?(rotation);
			return this;
		}

		// Token: 0x0600595E RID: 22878 RVA: 0x00223A2A File Offset: 0x00221C2A
		public PropChangeInfo.Builder SetScale(Vector3 scale)
		{
			this.Scale = new Vector3?(scale);
			return this;
		}

		// Token: 0x0600595F RID: 22879 RVA: 0x00223A39 File Offset: 0x00221C39
		public PropChangeInfo.Builder SetBlockValue(BlockValue blockValue)
		{
			this.BlockValue = new BlockValue?(blockValue);
			return this;
		}

		// Token: 0x04004565 RID: 17765
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Vector2i ChunkPos;

		// Token: 0x04004566 RID: 17766
		[PublicizedFrom(EAccessModifier.Private)]
		public int? PropId;

		// Token: 0x04004567 RID: 17767
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3? Position;

		// Token: 0x04004568 RID: 17768
		[PublicizedFrom(EAccessModifier.Private)]
		public Quaternion? Rotation;

		// Token: 0x04004569 RID: 17769
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3? Scale;

		// Token: 0x0400456A RID: 17770
		[PublicizedFrom(EAccessModifier.Private)]
		public BlockValue? BlockValue;
	}
}
