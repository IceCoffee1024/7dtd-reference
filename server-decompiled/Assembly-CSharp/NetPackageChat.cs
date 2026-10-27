using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007C2 RID: 1986
[Preserve]
public class NetPackageChat : NetPackage
{
	// Token: 0x06003AEC RID: 15084 RVA: 0x0017D9F8 File Offset: 0x0017BBF8
	public NetPackageChat Setup(EChatType _chatType, int _senderEntityId, string _msg, List<int> _recipientEntityIds, EMessageSender _msgSender, GeneratedTextManager.BbCodeSupportMode _bbMode)
	{
		this.chatType = _chatType;
		this.senderEntityId = _senderEntityId;
		this.msg = (string.IsNullOrEmpty(_msg) ? string.Empty : _msg);
		this.msgSender = _msgSender;
		this.bbMode = _bbMode;
		this.recipientEntityIds = _recipientEntityIds;
		return this;
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x0017DA38 File Offset: 0x0017BC38
	public override void read(PooledBinaryReader _br)
	{
		this.chatType = (EChatType)_br.ReadByte();
		this.senderEntityId = _br.ReadInt32();
		this.msg = _br.ReadString();
		this.msgSender = (EMessageSender)_br.ReadByte();
		this.bbMode = (GeneratedTextManager.BbCodeSupportMode)_br.ReadByte();
		int num = _br.ReadInt32();
		if (num > 0)
		{
			this.recipientEntityIds = new List<int>();
			for (int i = 0; i < num; i++)
			{
				this.recipientEntityIds.Add(_br.ReadInt32());
			}
		}
	}

	// Token: 0x06003AEE RID: 15086 RVA: 0x0017DAB4 File Offset: 0x0017BCB4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.chatType);
		_bw.Write(this.senderEntityId);
		_bw.Write(this.msg);
		_bw.Write((byte)this.msgSender);
		_bw.Write((byte)this.bbMode);
		_bw.Write((this.recipientEntityIds != null) ? this.recipientEntityIds.Count : 0);
		if (this.recipientEntityIds != null && this.recipientEntityIds.Count > 0)
		{
			for (int i = 0; i < this.recipientEntityIds.Count; i++)
			{
				_bw.Write(this.recipientEntityIds[i]);
			}
		}
	}

	// Token: 0x06003AEF RID: 15087 RVA: 0x0017DB64 File Offset: 0x0017BD64
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!_world.IsRemote())
		{
			GameManager.Instance.ChatMessageServer(base.Sender, this.chatType, this.senderEntityId, this.msg, this.recipientEntityIds, this.msgSender, this.bbMode);
			return;
		}
		GameManager.Instance.ChatMessageClient(this.chatType, this.senderEntityId, this.msg, null, this.msgSender, this.bbMode);
	}

	// Token: 0x04003006 RID: 12294
	[PublicizedFrom(EAccessModifier.Private)]
	public EChatType chatType;

	// Token: 0x04003007 RID: 12295
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityId;

	// Token: 0x04003008 RID: 12296
	[PublicizedFrom(EAccessModifier.Private)]
	public string msg;

	// Token: 0x04003009 RID: 12297
	[PublicizedFrom(EAccessModifier.Private)]
	public EMessageSender msgSender;

	// Token: 0x0400300A RID: 12298
	[PublicizedFrom(EAccessModifier.Private)]
	public GeneratedTextManager.BbCodeSupportMode bbMode;

	// Token: 0x0400300B RID: 12299
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> recipientEntityIds;
}
