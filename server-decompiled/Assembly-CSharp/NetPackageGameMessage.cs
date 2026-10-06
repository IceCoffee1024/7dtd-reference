using System;
using UnityEngine.Scripting;

// Token: 0x020007FC RID: 2044
[Preserve]
public class NetPackageGameMessage : NetPackage
{
	// Token: 0x06003C2E RID: 15406 RVA: 0x0018218F File Offset: 0x0018038F
	public NetPackageGameMessage Setup(EnumGameMessages _type, int _mainEntityId, int _secondaryEntityId)
	{
		this.msgType = _type;
		this.mainEntityId = _mainEntityId;
		this.secondaryEntityId = _secondaryEntityId;
		return this;
	}

	// Token: 0x06003C2F RID: 15407 RVA: 0x001821A7 File Offset: 0x001803A7
	public override void read(PooledBinaryReader _br)
	{
		this.msgType = (EnumGameMessages)_br.ReadByte();
		this.mainEntityId = _br.ReadInt32();
		this.secondaryEntityId = _br.ReadInt32();
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x001821CD File Offset: 0x001803CD
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.msgType);
		_bw.Write(this.mainEntityId);
		_bw.Write(this.secondaryEntityId);
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x001821FC File Offset: 0x001803FC
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

	// Token: 0x040030FE RID: 12542
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGameMessages msgType;

	// Token: 0x040030FF RID: 12543
	[PublicizedFrom(EAccessModifier.Private)]
	public int mainEntityId;

	// Token: 0x04003100 RID: 12544
	[PublicizedFrom(EAccessModifier.Private)]
	public int secondaryEntityId;
}
