using System;
using UnityEngine.Scripting;

// Token: 0x020007C3 RID: 1987
[Preserve]
public class NetPackageDebug : NetPackage
{
	// Token: 0x06003AFB RID: 15099 RVA: 0x0017D98E File Offset: 0x0017BB8E
	public NetPackageDebug Setup(NetPackageDebug.Type _type, int _entityId = -1, byte[] _data = null)
	{
		this.type = _type;
		this.entityId = _entityId;
		this.data = _data;
		return this;
	}

	// Token: 0x06003AFC RID: 15100 RVA: 0x0017D9A8 File Offset: 0x0017BBA8
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

	// Token: 0x06003AFD RID: 15101 RVA: 0x0017D9E8 File Offset: 0x0017BBE8
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

	// Token: 0x06003AFE RID: 15102 RVA: 0x0017DA48 File Offset: 0x0017BC48
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

	// Token: 0x06003AFF RID: 15103 RVA: 0x0017DAB8 File Offset: 0x0017BCB8
	public override int GetLength()
	{
		return 10 + ((this.data == null) ? 0 : this.data.Length);
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06003B00 RID: 15104 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x04003032 RID: 12338
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageDebug.Type type;

	// Token: 0x04003033 RID: 12339
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003034 RID: 12340
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x020007C4 RID: 1988
	public enum Type
	{
		// Token: 0x04003036 RID: 12342
		AILatency,
		// Token: 0x04003037 RID: 12343
		AILatencyClientOff,
		// Token: 0x04003038 RID: 12344
		AINameInfo,
		// Token: 0x04003039 RID: 12345
		AINameInfoClientOff,
		// Token: 0x0400303A RID: 12346
		AINameInfoServerToggle
	}
}
