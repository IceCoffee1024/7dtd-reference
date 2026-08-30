using System;
using System.IO;
using UnityEngine;

// Token: 0x02000166 RID: 358
public struct BlockValueRef : IEquatable<BlockValueRef>
{
	// Token: 0x06000A0F RID: 2575 RVA: 0x0004371F File Offset: 0x0004191F
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValueRef(BlockValueRefType type, Vector3i blockPosition, PropRef propRef)
	{
		this.Type = type;
		this.BlockPosition = blockPosition;
		this.PropReference = propRef;
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00043736 File Offset: 0x00041936
	public BlockValueRef(int x, int y, int z)
	{
		this = new BlockValueRef(new Vector3i(x, y, z));
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x00043748 File Offset: 0x00041948
	public BlockValueRef(Vector3i pos)
	{
		this = new BlockValueRef(BlockValueRefType.Block, pos, default(PropRef));
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x00043766 File Offset: 0x00041966
	public BlockValueRef(PropRef propRef)
	{
		this = new BlockValueRef(BlockValueRefType.Prop, Vector3i.zero, propRef);
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x00043775 File Offset: 0x00041975
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

	// Token: 0x06000A14 RID: 2580 RVA: 0x0004379A File Offset: 0x0004199A
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

	// Token: 0x06000A15 RID: 2581 RVA: 0x000437C0 File Offset: 0x000419C0
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

	// Token: 0x06000A16 RID: 2582 RVA: 0x00043844 File Offset: 0x00041A44
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

	// Token: 0x06000A17 RID: 2583 RVA: 0x000438E0 File Offset: 0x00041AE0
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

	// Token: 0x06000A18 RID: 2584 RVA: 0x0004397B File Offset: 0x00041B7B
	public Vector3i ToBlockPos(WorldBase world)
	{
		return World.worldToBlockPos(this.ToVector3Center(world));
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0004398C File Offset: 0x00041B8C
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

	// Token: 0x06000A1A RID: 2586 RVA: 0x000439F0 File Offset: 0x00041BF0
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

	// Token: 0x06000A1B RID: 2587 RVA: 0x00043A44 File Offset: 0x00041C44
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

	// Token: 0x06000A1C RID: 2588 RVA: 0x00043A98 File Offset: 0x00041C98
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

	// Token: 0x06000A1D RID: 2589 RVA: 0x00043B18 File Offset: 0x00041D18
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

	// Token: 0x06000A1E RID: 2590 RVA: 0x00043B54 File Offset: 0x00041D54
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

	// Token: 0x06000A1F RID: 2591 RVA: 0x00043B8D File Offset: 0x00041D8D
	public static implicit operator BlockValueRef(Vector3i pos)
	{
		return new BlockValueRef(pos);
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x00043B95 File Offset: 0x00041D95
	public static implicit operator BlockValueRef(PropRef propRef)
	{
		return new BlockValueRef(propRef);
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x00043BA0 File Offset: 0x00041DA0
	public bool Equals(BlockValueRef other)
	{
		return this.Type == other.Type && this.BlockPosition.Equals(other.BlockPosition) && this.PropReference.Equals(other.PropReference);
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00043BE8 File Offset: 0x00041DE8
	public override bool Equals(object obj)
	{
		if (obj is BlockValueRef)
		{
			BlockValueRef blockValueRef = (BlockValueRef)obj;
			return this.Equals(blockValueRef);
		}
		return false;
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00043C0D File Offset: 0x00041E0D
	public override int GetHashCode()
	{
		return HashCode.Combine<int, Vector3i, PropRef>((int)this.Type, this.BlockPosition, this.PropReference);
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x00043C26 File Offset: 0x00041E26
	public static bool operator ==(BlockValueRef left, BlockValueRef right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x00043C30 File Offset: 0x00041E30
	public static bool operator !=(BlockValueRef left, BlockValueRef right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000A12 RID: 2578
	public static readonly BlockValueRef None = new BlockValueRef(BlockValueRefType.None, Vector3i.zero, default(PropRef));

	// Token: 0x04000A13 RID: 2579
	public readonly BlockValueRefType Type;

	// Token: 0x04000A14 RID: 2580
	public readonly Vector3i BlockPosition;

	// Token: 0x04000A15 RID: 2581
	public readonly PropRef PropReference;
}
