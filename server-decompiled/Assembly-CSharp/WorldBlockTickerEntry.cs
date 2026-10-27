using System;
using System.IO;

// Token: 0x02000C23 RID: 3107
public class WorldBlockTickerEntry
{
	// Token: 0x06005E16 RID: 24086 RVA: 0x0024C2F8 File Offset: 0x0024A4F8
	public WorldBlockTickerEntry(Vector3i _pos, int _id, ulong _scheduledTime)
	{
		long num = WorldBlockTickerEntry.nextTickEntryID;
		WorldBlockTickerEntry.nextTickEntryID = num + 1L;
		this.tickEntryID = num;
		this.worldPos = _pos;
		this.blockID = _id;
		this.scheduledTime = _scheduledTime;
	}

	// Token: 0x06005E17 RID: 24087 RVA: 0x0024C32C File Offset: 0x0024A52C
	public static WorldBlockTickerEntry Read(BinaryReader _br, int _chunkX, int _chunkZ, int _version)
	{
		Vector3i vector3i = new Vector3i((int)_br.ReadByte() + _chunkX * 16, (int)_br.ReadByte(), (int)_br.ReadByte() + _chunkZ * 16);
		int num = (int)_br.ReadUInt16();
		ulong num2 = _br.ReadUInt64();
		_br.ReadUInt16();
		return new WorldBlockTickerEntry(vector3i, num, num2);
	}

	// Token: 0x06005E18 RID: 24088 RVA: 0x0024C378 File Offset: 0x0024A578
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((byte)World.toBlockXZ(this.worldPos.x));
		_bw.Write((byte)this.worldPos.y);
		_bw.Write((byte)World.toBlockXZ(this.worldPos.z));
		_bw.Write((ushort)this.blockID);
		_bw.Write(this.scheduledTime);
		_bw.Write(0);
	}

	// Token: 0x06005E19 RID: 24089 RVA: 0x0024C3E8 File Offset: 0x0024A5E8
	public override bool Equals(object _obj)
	{
		WorldBlockTickerEntry worldBlockTickerEntry = _obj as WorldBlockTickerEntry;
		return worldBlockTickerEntry != null && (this.worldPos.Equals(worldBlockTickerEntry.worldPos) && this.blockID == worldBlockTickerEntry.blockID) && worldBlockTickerEntry.tickEntryID == this.tickEntryID;
	}

	// Token: 0x06005E1A RID: 24090 RVA: 0x0024C435 File Offset: 0x0024A635
	public override int GetHashCode()
	{
		return WorldBlockTickerEntry.ToHashCode(this.worldPos, this.blockID);
	}

	// Token: 0x06005E1B RID: 24091 RVA: 0x0024C448 File Offset: 0x0024A648
	public static int ToHashCode(Vector3i _pos, int _blockID)
	{
		return (_pos.GetHashCode() * 397) ^ _blockID;
	}

	// Token: 0x06005E1C RID: 24092 RVA: 0x0024C45F File Offset: 0x0024A65F
	public long GetChunkKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.worldPos.x), World.toChunkXZ(this.worldPos.z));
	}

	// Token: 0x04004907 RID: 18695
	public readonly Vector3i worldPos;

	// Token: 0x04004908 RID: 18696
	public readonly int blockID;

	// Token: 0x04004909 RID: 18697
	public readonly ulong scheduledTime;

	// Token: 0x0400490A RID: 18698
	[PublicizedFrom(EAccessModifier.Private)]
	public static long nextTickEntryID;

	// Token: 0x0400490B RID: 18699
	public readonly long tickEntryID;
}
