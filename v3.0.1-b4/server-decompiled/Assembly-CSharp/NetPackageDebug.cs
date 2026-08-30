using System;
using UnityEngine.Scripting;

// Token: 0x020007B8 RID: 1976
[Preserve]
public class NetPackageDebug : NetPackage
{
	// Token: 0x06003A9C RID: 15004 RVA: 0x0017B5B7 File Offset: 0x001797B7
	public NetPackageDebug Setup(NetPackageDebug.Type _type, int _entityId = -1, byte[] _data = null)
	{
		this.type = _type;
		this.entityId = _entityId;
		this.data = _data;
		return this;
	}

	// Token: 0x06003A9D RID: 15005 RVA: 0x0017B5D0 File Offset: 0x001797D0
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

	// Token: 0x06003A9E RID: 15006 RVA: 0x0017B610 File Offset: 0x00179810
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

	// Token: 0x06003A9F RID: 15007 RVA: 0x0017B670 File Offset: 0x00179870
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

	// Token: 0x06003AA0 RID: 15008 RVA: 0x0017B6E0 File Offset: 0x001798E0
	public override int GetLength()
	{
		return 10 + ((this.data == null) ? 0 : this.data.Length);
	}

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x06003AA1 RID: 15009 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x04002FB3 RID: 12211
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageDebug.Type type;

	// Token: 0x04002FB4 RID: 12212
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04002FB5 RID: 12213
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x020007B9 RID: 1977
	public enum Type
	{
		// Token: 0x04002FB7 RID: 12215
		AILatency,
		// Token: 0x04002FB8 RID: 12216
		AILatencyClientOff,
		// Token: 0x04002FB9 RID: 12217
		AINameInfo,
		// Token: 0x04002FBA RID: 12218
		AINameInfoClientOff,
		// Token: 0x04002FBB RID: 12219
		AINameInfoServerToggle
	}
}
