using System;
using System.IO;
using UnityEngine;

// Token: 0x02000167 RID: 359
public struct BlockValueRef : IEquatable<BlockValueRef>
{
	// Token: 0x06000A19 RID: 2585 RVA: 0x0004397F File Offset: 0x00041B7F
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValueRef(BlockValueRefType type, Vector3i blockPosition, PropRef propRef)
	{
		this.Type = type;
		this.BlockPosition = blockPosition;
		this.PropReference = propRef;
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x00043996 File Offset: 0x00041B96
	public BlockValueRef(int x, int y, int z)
	{
		this = new BlockValueRef(new Vector3i(x, y, z));
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x000439A8 File Offset: 0x00041BA8
	public BlockValueRef(Vector3i pos)
	{
		this = new BlockValueRef(BlockValueRefType.Block, pos, default(PropRef));
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x000439C6 File Offset: 0x00041BC6
	public BlockValueRef(PropRef propRef)
	{
		this = new BlockValueRef(BlockValueRefType.Prop, Vector3i.zero, propRef);
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x000439D5 File Offset: 0x00041BD5
	public bool TryGetBlockPos(out Vector3i pos)
	{
		if (this.Type != BlockValueRefType.Block)
		{
			pos = Vector3i.zero;
			return false;
		}
		pos = this.BlockPosition;
		return true;
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x000439FA File Offset: 0x00041BFA
	public bool TryGetPropRef(out PropRef propRef)
	{
		if (this.Type != BlockValueRefType.Prop)
		{
			propRef = PropRef.Default;
			return false;
		}
		propRef = this.PropReference;
		return true;
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00043A20 File Offset: 0x00041C20
	public Vector3 ToVector3(WorldBase world)
	{
		Vector3 vector;
		switch (this.Type)
		{
		case BlockValueRefType.None:
			vector = Vector3.zero;
			break;
		case BlockValueRefType.Block:
			vector = this.BlockPosition.ToVector3();
			break;
		case BlockValueRefType.Prop:
		{
			IChunk chunkSync = world.GetChunkSync(this.PropReference);
			vector = ((chunkSync != null) ? chunkSync.GetWorldPos() : Vector3i.zero) + world.GetProp(this.PropReference).position;
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		return vector;
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x00043AA4 File Offset: 0x00041CA4
	public Vector3 ToVector3Center(WorldBase world)
	{
		Vector3 vector;
		switch (this.Type)
		{
		case BlockValueRefType.None:
			vector = Vector3.zero;
			break;
		case BlockValueRefType.Block:
			vector = this.BlockPosition.ToVector3Center();
			break;
		case BlockValueRefType.Prop:
		{
			IChunk chunkSync = world.GetChunkSync(this.PropReference);
			vector = ((chunkSync != null) ? chunkSync.GetWorldPos() : Vector3i.zero) + world.GetProp(this.PropReference).position + new Vector3(0.5f, 0.5f, 0.5f);
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		return vector;
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x00043B40 File Offset: 0x00041D40
	public Vector3 ToVector3CenterXZ(WorldBase world)
	{
		Vector3 vector;
		switch (this.Type)
		{
		case BlockValueRefType.None:
			vector = Vector3.zero;
			break;
		case BlockValueRefType.Block:
			vector = this.BlockPosition.ToVector3CenterXZ();
			break;
		case BlockValueRefType.Prop:
		{
			IChunk chunkSync = world.GetChunkSync(this.PropReference);
			vector = ((chunkSync != null) ? chunkSync.GetWorldPos() : Vector3i.zero) + world.GetProp(this.PropReference).position + new Vector3(0.5f, 0f, 0.5f);
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		return vector;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00043BDB File Offset: 0x00041DDB
	public Vector3i ToBlockPos(WorldBase world)
	{
		return World.worldToBlockPos(this.ToVector3Center(world));
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00043BEC File Offset: 0x00041DEC
	public static BlockValueRef Create(WorldRayHitInfo hitInfo)
	{
		if (!hitInfo.hit.propValue.IsAir)
		{
			return new BlockValueRef(hitInfo.hit.propData.PropRef);
		}
		if (!hitInfo.hit.blockValue.isair)
		{
			return new BlockValueRef(hitInfo.hit.blockPos);
		}
		return BlockValueRef.None;
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x00043C50 File Offset: 0x00041E50
	public static BlockValueRef Read(BinaryReader br)
	{
		BlockValueRef blockValueRef;
		switch (br.ReadByte())
		{
		case 0:
			blockValueRef = BlockValueRef.None;
			break;
		case 1:
			blockValueRef = new BlockValueRef(StreamUtils.ReadVector3i(br));
			break;
		case 2:
			blockValueRef = new BlockValueRef(PropRef.Read(br));
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return blockValueRef;
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x00043CA4 File Offset: 0x00041EA4
	public readonly void Write(BinaryWriter bw)
	{
		bw.Write((byte)this.Type);
		switch (this.Type)
		{
		case BlockValueRefType.None:
			return;
		case BlockValueRefType.Block:
			StreamUtils.Write(bw, this.BlockPosition);
			return;
		case BlockValueRefType.Prop:
			this.PropReference.Write(bw);
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00043CF8 File Offset: 0x00041EF8
	public override string ToString()
	{
		object obj = this.Type;
		string text;
		switch (this.Type)
		{
		case BlockValueRefType.None:
			text = "";
			break;
		case BlockValueRefType.Block:
			text = string.Format(", BlockPos=({0})", this.BlockPosition);
			break;
		case BlockValueRefType.Prop:
			text = string.Format(", PropRef=[{0}]", this.PropReference);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return string.Format("Type={0}{1}", obj, text);
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x00043D78 File Offset: 0x00041F78
	public static implicit operator Vector3i(BlockValueRef bvRef)
	{
		Vector3i vector3i;
		if (bvRef.TryGetBlockPos(out vector3i))
		{
			return vector3i;
		}
		Log.Warning(string.Format("[PROPS] BlockValueRef implicitly converted to Vector3i but type was {0}.", bvRef.Type));
		return Vector3i.zero;
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00043DB4 File Offset: 0x00041FB4
	public static implicit operator PropRef(BlockValueRef bvRef)
	{
		PropRef propRef;
		if (bvRef.TryGetPropRef(out propRef))
		{
			return propRef;
		}
		Log.Warning(string.Format("[PROPS] BlockValueRef implicitly converted to PropRef but type was {0}.", bvRef.Type));
		return PropRef.Default;
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x00043DED File Offset: 0x00041FED
	public static implicit operator BlockValueRef(Vector3i pos)
	{
		return new BlockValueRef(pos);
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00043DF5 File Offset: 0x00041FF5
	public static implicit operator BlockValueRef(PropRef propRef)
	{
		return new BlockValueRef(propRef);
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00043E00 File Offset: 0x00042000
	public bool Equals(BlockValueRef other)
	{
		return this.Type == other.Type && this.BlockPosition.Equals(other.BlockPosition) && this.PropReference.Equals(other.PropReference);
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00043E48 File Offset: 0x00042048
	public override bool Equals(object obj)
	{
		if (obj is BlockValueRef)
		{
			BlockValueRef blockValueRef = (BlockValueRef)obj;
			return this.Equals(blockValueRef);
		}
		return false;
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x00043E6D File Offset: 0x0004206D
	public override int GetHashCode()
	{
		return HashCode.Combine<int, Vector3i, PropRef>((int)this.Type, this.BlockPosition, this.PropReference);
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x00043E86 File Offset: 0x00042086
	public static bool operator ==(BlockValueRef left, BlockValueRef right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00043E90 File Offset: 0x00042090
	public static bool operator !=(BlockValueRef left, BlockValueRef right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000A14 RID: 2580
	public static readonly BlockValueRef None = new BlockValueRef(BlockValueRefType.None, Vector3i.zero, default(PropRef));

	// Token: 0x04000A15 RID: 2581
	public readonly BlockValueRefType Type;

	// Token: 0x04000A16 RID: 2582
	public readonly Vector3i BlockPosition;

	// Token: 0x04000A17 RID: 2583
	public readonly PropRef PropReference;
}
