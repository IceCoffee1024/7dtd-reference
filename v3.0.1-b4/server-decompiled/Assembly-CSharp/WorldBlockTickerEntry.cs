using System;
using System.IO;

// Token: 0x02000BF4 RID: 3060
public class WorldBlockTickerEntry
{
	// Token: 0x06005D4E RID: 23886 RVA: 0x00244D30 File Offset: 0x00242F30
	public WorldBlockTickerEntry(Vector3i _pos, int _id, ulong _scheduledTime)
	{
		long num = WorldBlockTickerEntry.nextTickEntryID;
		WorldBlockTickerEntry.nextTickEntryID = num + 1L;
		this.tickEntryID = num;
		this.worldPos = _pos;
		this.blockID = _id;
		this.scheduledTime = _scheduledTime;
	}

	// Token: 0x06005D4F RID: 23887 RVA: 0x00244D64 File Offset: 0x00242F64
	public static WorldBlockTickerEntry Read(BinaryReader _br, int _chunkX, int _chunkZ, int _version)
	{
		Vector3i vector3i = new Vector3i((int)_br.ReadByte() + _chunkX * 16, (int)_br.ReadByte(), (int)_br.ReadByte() + _chunkZ * 16);
		int num = (int)_br.ReadUInt16();
		ulong num2 = _br.ReadUInt64();
		_br.ReadUInt16();
		return new WorldBlockTickerEntry(vector3i, num, num2);
	}

	// Token: 0x06005D50 RID: 23888 RVA: 0x00244DB0 File Offset: 0x00242FB0
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((byte)World.toBlockXZ(this.worldPos.x));
		_bw.Write((byte)this.worldPos.y);
		_bw.Write((byte)World.toBlockXZ(this.worldPos.z));
		_bw.Write((ushort)this.blockID);
		_bw.Write(this.scheduledTime);
		_bw.Write(0);
	}

	// Token: 0x06005D51 RID: 23889 RVA: 0x00244E20 File Offset: 0x00243020
	public override bool Equals(object _obj)
	{
		WorldBlockTickerEntry worldBlockTickerEntry = _obj as WorldBlockTickerEntry;
		return worldBlockTickerEntry != null && (this.worldPos.Equals(worldBlockTickerEntry.worldPos) && this.blockID == worldBlockTickerEntry.blockID) && worldBlockTickerEntry.tickEntryID == this.tickEntryID;
	}

	// Token: 0x06005D52 RID: 23890 RVA: 0x00244E6D File Offset: 0x0024306D
	public override int GetHashCode()
	{
		return WorldBlockTickerEntry.ToHashCode(this.worldPos, this.blockID);
	}

	// Token: 0x06005D53 RID: 23891 RVA: 0x00244E80 File Offset: 0x00243080
	public static int ToHashCode(Vector3i _pos, int _blockID)
	{
		return (_pos.GetHashCode() * 397) ^ _blockID;
	}

	// Token: 0x06005D54 RID: 23892 RVA: 0x00244E97 File Offset: 0x00243097
	public long GetChunkKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.worldPos.x), World.toChunkXZ(this.worldPos.z));
	}

	// Token: 0x04004807 RID: 18439
	public readonly Vector3i worldPos;

	// Token: 0x04004808 RID: 18440
	public readonly int blockID;

	// Token: 0x04004809 RID: 18441
	public readonly ulong scheduledTime;

	// Token: 0x0400480A RID: 18442
	[PublicizedFrom(EAccessModifier.Private)]
	public static long nextTickEntryID;

	// Token: 0x0400480B RID: 18443
	public readonly long tickEntryID;
}
