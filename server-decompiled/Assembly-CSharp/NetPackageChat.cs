using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007BA RID: 1978
[Preserve]
public class NetPackageChat : NetPackage
{
	// Token: 0x06003AD5 RID: 15061 RVA: 0x0017CA60 File Offset: 0x0017AC60
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

	// Token: 0x06003AD6 RID: 15062 RVA: 0x0017CAA0 File Offset: 0x0017ACA0
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

	// Token: 0x06003AD7 RID: 15063 RVA: 0x0017CB1C File Offset: 0x0017AD1C
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

	// Token: 0x06003AD8 RID: 15064 RVA: 0x0017CBCC File Offset: 0x0017ADCC
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

	// Token: 0x04002FEA RID: 12266
	[PublicizedFrom(EAccessModifier.Private)]
	public EChatType chatType;

	// Token: 0x04002FEB RID: 12267
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityId;

	// Token: 0x04002FEC RID: 12268
	[PublicizedFrom(EAccessModifier.Private)]
	public string msg;

	// Token: 0x04002FED RID: 12269
	[PublicizedFrom(EAccessModifier.Private)]
	public EMessageSender msgSender;

	// Token: 0x04002FEE RID: 12270
	[PublicizedFrom(EAccessModifier.Private)]
	public GeneratedTextManager.BbCodeSupportMode bbMode;

	// Token: 0x04002FEF RID: 12271
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> recipientEntityIds;
}
