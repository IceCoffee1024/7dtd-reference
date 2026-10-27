using System;
using UnityEngine;

// Token: 0x02000BA2 RID: 2978
public class PropChangeInfo
{
	// Token: 0x06005991 RID: 22929 RVA: 0x00227ECE File Offset: 0x002260CE
	public PropChangeInfo(Vector2i chunkPos, int? propId, Vector3? position, Quaternion? rotation, Vector3? scale, BlockValue? blockValue)
	{
		this.ChunkPos = chunkPos;
		this.PropId = propId;
		this.Position = position;
		this.Rotation = rotation;
		this.Scale = scale;
		this.BlockValue = blockValue;
	}

	// Token: 0x06005992 RID: 22930 RVA: 0x00227F04 File Offset: 0x00226104
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

	// Token: 0x06005993 RID: 22931 RVA: 0x00227FAC File Offset: 0x002261AC
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

	// Token: 0x06005994 RID: 22932 RVA: 0x002280F0 File Offset: 0x002262F0
	public override string ToString()
	{
		return string.Format("ChunkId={0}, PropId={1}, Position={2}, Rotation={3}, Scale={4}, BlockValue={5}", new object[] { this.ChunkPos, this.PropId, this.Position, this.Rotation, this.Scale, this.BlockValue });
	}

	// Token: 0x040045BB RID: 17851
	public static readonly PropChangeInfo Empty = new PropChangeInfo.Builder(Vector2i.zero).Build();

	// Token: 0x040045BC RID: 17852
	public readonly Vector2i ChunkPos;

	// Token: 0x040045BD RID: 17853
	public readonly int? PropId;

	// Token: 0x040045BE RID: 17854
	public readonly Vector3? Position;

	// Token: 0x040045BF RID: 17855
	public readonly Quaternion? Rotation;

	// Token: 0x040045C0 RID: 17856
	public readonly Vector3? Scale;

	// Token: 0x040045C1 RID: 17857
	public readonly BlockValue? BlockValue;

	// Token: 0x02000BA3 RID: 2979
	[Flags]
	[PublicizedFrom(EAccessModifier.Private)]
	public enum Flags : byte
	{
		// Token: 0x040045C3 RID: 17859
		HasPropId = 1,
		// Token: 0x040045C4 RID: 17860
		HasPosition = 2,
		// Token: 0x040045C5 RID: 17861
		HasRotation = 4,
		// Token: 0x040045C6 RID: 17862
		HasScale = 8,
		// Token: 0x040045C7 RID: 17863
		HasBlockValue = 16
	}

	// Token: 0x02000BA4 RID: 2980
	public class Builder
	{
		// Token: 0x06005996 RID: 22934 RVA: 0x00228177 File Offset: 0x00226377
		public Builder(int chunkX, int chunkZ)
			: this(new Vector2i(chunkX, chunkZ))
		{
		}

		// Token: 0x06005997 RID: 22935 RVA: 0x00228186 File Offset: 0x00226386
		public Builder(Vector2i chunkPos)
		{
			this.ChunkPos = chunkPos;
		}

		// Token: 0x06005998 RID: 22936 RVA: 0x00228195 File Offset: 0x00226395
		public Builder(PropRef propRef)
		{
			this.ChunkPos = propRef.ChunkPos;
			this.PropId = new int?(propRef.PropId);
		}

		// Token: 0x06005999 RID: 22937 RVA: 0x002281BA File Offset: 0x002263BA
		public PropChangeInfo Build()
		{
			return new PropChangeInfo(this.ChunkPos, this.PropId, this.Position, this.Rotation, this.Scale, this.BlockValue);
		}

		// Token: 0x0600599A RID: 22938 RVA: 0x002281E5 File Offset: 0x002263E5
		public PropChangeInfo.Builder SetPropId(int propId)
		{
			this.PropId = new int?(propId);
			return this;
		}

		// Token: 0x0600599B RID: 22939 RVA: 0x002281F4 File Offset: 0x002263F4
		public PropChangeInfo.Builder SetPosition(Vector3 position)
		{
			this.Position = new Vector3?(position);
			return this;
		}

		// Token: 0x0600599C RID: 22940 RVA: 0x00228203 File Offset: 0x00226403
		public PropChangeInfo.Builder SetRotation(Quaternion rotation)
		{
			this.Rotation = new Quaternion?(rotation);
			return this;
		}

		// Token: 0x0600599D RID: 22941 RVA: 0x00228212 File Offset: 0x00226412
		public PropChangeInfo.Builder SetScale(Vector3 scale)
		{
			this.Scale = new Vector3?(scale);
			return this;
		}

		// Token: 0x0600599E RID: 22942 RVA: 0x00228221 File Offset: 0x00226421
		public PropChangeInfo.Builder SetBlockValue(BlockValue blockValue)
		{
			this.BlockValue = new BlockValue?(blockValue);
			return this;
		}

		// Token: 0x040045C8 RID: 17864
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Vector2i ChunkPos;

		// Token: 0x040045C9 RID: 17865
		[PublicizedFrom(EAccessModifier.Private)]
		public int? PropId;

		// Token: 0x040045CA RID: 17866
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3? Position;

		// Token: 0x040045CB RID: 17867
		[PublicizedFrom(EAccessModifier.Private)]
		public Quaternion? Rotation;

		// Token: 0x040045CC RID: 17868
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3? Scale;

		// Token: 0x040045CD RID: 17869
		[PublicizedFrom(EAccessModifier.Private)]
		public BlockValue? BlockValue;
	}
}
