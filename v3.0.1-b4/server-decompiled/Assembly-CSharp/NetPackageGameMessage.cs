using System;
using UnityEngine.Scripting;

// Token: 0x020007EE RID: 2030
[Preserve]
public class NetPackageGameMessage : NetPackage
{
	// Token: 0x06003BD1 RID: 15313 RVA: 0x0017F8A3 File Offset: 0x0017DAA3
	public NetPackageGameMessage Setup(EnumGameMessages _type, int _mainEntityId, int _secondaryEntityId)
	{
		this.msgType = _type;
		this.mainEntityId = _mainEntityId;
		this.secondaryEntityId = _secondaryEntityId;
		return this;
	}

	// Token: 0x06003BD2 RID: 15314 RVA: 0x0017F8BB File Offset: 0x0017DABB
	public override void read(PooledBinaryReader _br)
	{
		this.msgType = (EnumGameMessages)_br.ReadByte();
		this.mainEntityId = _br.ReadInt32();
		this.secondaryEntityId = _br.ReadInt32();
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x0017F8E1 File Offset: 0x0017DAE1
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.msgType);
		_bw.Write(this.mainEntityId);
		_bw.Write(this.secondaryEntityId);
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x0017F910 File Offset: 0x0017DB10
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!_world.IsRemote())
		{
			GameManager.Instance.GameMessageServer(base.Sender, this.msgType, this.mainEntityId, this.secondaryEntityId);
			return;
		}
		GameManager.Instance.DisplayGameMessage(this.msgType, this.mainEntityId, this.secondaryEntityId, true);
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003086 RID: 12422
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGameMessages msgType;

	// Token: 0x04003087 RID: 12423
	[PublicizedFrom(EAccessModifier.Private)]
	public int mainEntityId;

	// Token: 0x04003088 RID: 12424
	[PublicizedFrom(EAccessModifier.Private)]
	public int secondaryEntityId;
}
