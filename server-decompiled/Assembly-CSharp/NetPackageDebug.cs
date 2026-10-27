using System;
using UnityEngine.Scripting;

// Token: 0x020007CE RID: 1998
[Preserve]
public class NetPackageDebug : NetPackage
{
	// Token: 0x06003B39 RID: 15161 RVA: 0x0017EF42 File Offset: 0x0017D142
	public NetPackageDebug Setup(NetPackageDebug.Type _type, int _entityId = -1, byte[] _data = null)
	{
		this.type = _type;
		this.entityId = _entityId;
		this.data = _data;
		return this;
	}

	// Token: 0x06003B3A RID: 15162 RVA: 0x0017EF5C File Offset: 0x0017D15C
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

	// Token: 0x06003B3B RID: 15163 RVA: 0x0017EF9C File Offset: 0x0017D19C
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

	// Token: 0x06003B3C RID: 15164 RVA: 0x0017EFFC File Offset: 0x0017D1FC
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

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x06003B3D RID: 15165 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x04003048 RID: 12360
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageDebug.Type type;

	// Token: 0x04003049 RID: 12361
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400304A RID: 12362
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x020007CF RID: 1999
	public enum Type
	{
		// Token: 0x0400304C RID: 12364
		AILatency,
		// Token: 0x0400304D RID: 12365
		AILatencyClientOff,
		// Token: 0x0400304E RID: 12366
		AINameInfo,
		// Token: 0x0400304F RID: 12367
		AINameInfoClientOff,
		// Token: 0x04003050 RID: 12368
		AINameInfoServerToggle
	}
}
