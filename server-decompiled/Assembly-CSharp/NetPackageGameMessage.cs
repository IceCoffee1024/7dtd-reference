using System;
using UnityEngine.Scripting;

// Token: 0x020007F9 RID: 2041
[Preserve]
public class NetPackageGameMessage : NetPackage
{
	// Token: 0x06003C30 RID: 15408 RVA: 0x00181C7B File Offset: 0x0017FE7B
	public NetPackageGameMessage Setup(EnumGameMessages _type, int _mainEntityId, int _secondaryEntityId)
	{
		this.msgType = _type;
		this.mainEntityId = _mainEntityId;
		this.secondaryEntityId = _secondaryEntityId;
		return this;
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x00181C93 File Offset: 0x0017FE93
	public override void read(PooledBinaryReader _br)
	{
		this.msgType = (EnumGameMessages)_br.ReadByte();
		this.mainEntityId = _br.ReadInt32();
		this.secondaryEntityId = _br.ReadInt32();
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x00181CB9 File Offset: 0x0017FEB9
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.msgType);
		_bw.Write(this.mainEntityId);
		_bw.Write(this.secondaryEntityId);
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x00181CE8 File Offset: 0x0017FEE8
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

	// Token: 0x06003C34 RID: 15412 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003105 RID: 12549
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGameMessages msgType;

	// Token: 0x04003106 RID: 12550
	[PublicizedFrom(EAccessModifier.Private)]
	public int mainEntityId;

	// Token: 0x04003107 RID: 12551
	[PublicizedFrom(EAccessModifier.Private)]
	public int secondaryEntityId;
}
