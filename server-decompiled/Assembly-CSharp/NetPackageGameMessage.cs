using System;
using UnityEngine.Scripting;

// Token: 0x02000806 RID: 2054
[Preserve]
public class NetPackageGameMessage : NetPackage
{
	// Token: 0x06003C51 RID: 15441 RVA: 0x00183293 File Offset: 0x00181493
	public NetPackageGameMessage Setup(EnumGameMessages _type, int _mainEntityId, int _secondaryEntityId)
	{
		this.msgType = _type;
		this.mainEntityId = _mainEntityId;
		this.secondaryEntityId = _secondaryEntityId;
		return this;
	}

	// Token: 0x06003C52 RID: 15442 RVA: 0x001832AB File Offset: 0x001814AB
	public override void read(PooledBinaryReader _br)
	{
		this.msgType = (EnumGameMessages)_br.ReadByte();
		this.mainEntityId = _br.ReadInt32();
		this.secondaryEntityId = _br.ReadInt32();
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x001832D1 File Offset: 0x001814D1
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.msgType);
		_bw.Write(this.mainEntityId);
		_bw.Write(this.secondaryEntityId);
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x00183300 File Offset: 0x00181500
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

	// Token: 0x0400311E RID: 12574
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGameMessages msgType;

	// Token: 0x0400311F RID: 12575
	[PublicizedFrom(EAccessModifier.Private)]
	public int mainEntityId;

	// Token: 0x04003120 RID: 12576
	[PublicizedFrom(EAccessModifier.Private)]
	public int secondaryEntityId;
}
