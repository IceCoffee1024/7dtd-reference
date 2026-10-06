using System;
using UnityEngine.Scripting;

// Token: 0x020007C6 RID: 1990
[Preserve]
public class NetPackageDebug : NetPackage
{
	// Token: 0x06003B22 RID: 15138 RVA: 0x0017DFAA File Offset: 0x0017C1AA
	public NetPackageDebug Setup(NetPackageDebug.Type _type, int _entityId = -1, byte[] _data = null)
	{
		this.type = _type;
		this.entityId = _entityId;
		this.data = _data;
		return this;
	}

	// Token: 0x06003B23 RID: 15139 RVA: 0x0017DFC4 File Offset: 0x0017C1C4
	public override void read(PooledBinaryReader _reader)
	{
		this.type = (NetPackageDebug.Type)_reader.ReadInt16();
		this.entityId = _reader.ReadInt32();
		int num = _reader.ReadInt32();
		if (num > 0)
		{
			this.data = _reader.ReadBytes(num);
		}
	}

	// Token: 0x06003B24 RID: 15140 RVA: 0x0017E004 File Offset: 0x0017C204
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.type);
		_writer.Write(this.entityId);
		if (this.data == null || this.data.Length == 0)
		{
			_writer.Write(0);
			return;
		}
		_writer.Write(this.data.Length);
		_writer.Write(this.data);
	}

	// Token: 0x06003B25 RID: 15141 RVA: 0x0017E064 File Offset: 0x0017C264
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		switch (this.type)
		{
		case NetPackageDebug.Type.AILatency:
			AIDirector.DebugReceiveLatency(this.entityId, this.data);
			return;
		case NetPackageDebug.Type.AILatencyClientOff:
			AIDirector.DebugLatencyOff();
			return;
		case NetPackageDebug.Type.AINameInfo:
			AIDirector.DebugReceiveNameInfo(this.entityId, this.data);
			return;
		case NetPackageDebug.Type.AINameInfoClientOff:
			EntityAlive.SetupAllDebugNameHUDs(false);
			return;
		case NetPackageDebug.Type.AINameInfoServerToggle:
			AIDirector.DebugToggleSendNameInfo(base.Sender.entityId);
			return;
		default:
			return;
		}
	}

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06003B26 RID: 15142 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x0400302C RID: 12332
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageDebug.Type type;

	// Token: 0x0400302D RID: 12333
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400302E RID: 12334
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x020007C7 RID: 1991
	public enum Type
	{
		// Token: 0x04003030 RID: 12336
		AILatency,
		// Token: 0x04003031 RID: 12337
		AILatencyClientOff,
		// Token: 0x04003032 RID: 12338
		AINameInfo,
		// Token: 0x04003033 RID: 12339
		AINameInfoClientOff,
		// Token: 0x04003034 RID: 12340
		AINameInfoServerToggle
	}
}
