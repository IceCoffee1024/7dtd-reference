using System;
using System.IO;

// Token: 0x02000C06 RID: 3078
public class WorldBlockTickerEntry
{
	// Token: 0x06005DD8 RID: 24024 RVA: 0x00247AD4 File Offset: 0x00245CD4
	public WorldBlockTickerEntry(Vector3i _pos, int _id, ulong _scheduledTime)
	{
		long num = WorldBlockTickerEntry.nextTickEntryID;
		WorldBlockTickerEntry.nextTickEntryID = num + 1L;
		this.tickEntryID = num;
		this.worldPos = _pos;
		this.blockID = _id;
		this.scheduledTime = _scheduledTime;
	}

	// Token: 0x06005DD9 RID: 24025 RVA: 0x00247B08 File Offset: 0x00245D08
	public static WorldBlockTickerEntry Read(BinaryReader _br, int _chunkX, int _chunkZ, int _version)
	{
		Vector3i vector3i = new Vector3i((int)_br.ReadByte() + _chunkX * 16, (int)_br.ReadByte(), (int)_br.ReadByte() + _chunkZ * 16);
		int num = (int)_br.ReadUInt16();
		ulong num2 = _br.ReadUInt64();
		_br.ReadUInt16();
		return new WorldBlockTickerEntry(vector3i, num, num2);
	}

	// Token: 0x06005DDA RID: 24026 RVA: 0x00247B54 File Offset: 0x00245D54
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((byte)World.toBlockXZ(this.worldPos.x));
		_bw.Write((byte)this.worldPos.y);
		_bw.Write((byte)World.toBlockXZ(this.worldPos.z));
		_bw.Write((ushort)this.blockID);
		_bw.Write(this.scheduledTime);
		_bw.Write(0);
	}

	// Token: 0x06005DDB RID: 24027 RVA: 0x00247BC4 File Offset: 0x00245DC4
	public override bool Equals(object _obj)
	{
		WorldBlockTickerEntry worldBlockTickerEntry = _obj as WorldBlockTickerEntry;
		return worldBlockTickerEntry != null && (this.worldPos.Equals(worldBlockTickerEntry.worldPos) && this.blockID == worldBlockTickerEntry.blockID) && worldBlockTickerEntry.tickEntryID == this.tickEntryID;
	}

	// Token: 0x06005DDC RID: 24028 RVA: 0x00247C11 File Offset: 0x00245E11
	public override int GetHashCode()
	{
		return WorldBlockTickerEntry.ToHashCode(this.worldPos, this.blockID);
	}

	// Token: 0x06005DDD RID: 24029 RVA: 0x00247C24 File Offset: 0x00245E24
	public static int ToHashCode(Vector3i _pos, int _blockID)
	{
		return (_pos.GetHashCode() * 397) ^ _blockID;
	}

	// Token: 0x06005DDE RID: 24030 RVA: 0x00247C3B File Offset: 0x00245E3B
	public long GetChunkKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.worldPos.x), World.toChunkXZ(this.worldPos.z));
	}

	// Token: 0x040048A3 RID: 18595
	public readonly Vector3i worldPos;

	// Token: 0x040048A4 RID: 18596
	public readonly int blockID;

	// Token: 0x040048A5 RID: 18597
	public readonly ulong scheduledTime;

	// Token: 0x040048A6 RID: 18598
	[PublicizedFrom(EAccessModifier.Private)]
	public static long nextTickEntryID;

	// Token: 0x040048A7 RID: 18599
	public readonly long tickEntryID;
}
