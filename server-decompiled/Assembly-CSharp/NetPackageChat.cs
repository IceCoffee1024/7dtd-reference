using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007B7 RID: 1975
[Preserve]
public class NetPackageChat : NetPackage
{
	// Token: 0x06003AA2 RID: 15010 RVA: 0x0017C3D4 File Offset: 0x0017A5D4
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

	// Token: 0x06003AA3 RID: 15011 RVA: 0x0017C414 File Offset: 0x0017A614
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

	// Token: 0x06003AA4 RID: 15012 RVA: 0x0017C490 File Offset: 0x0017A690
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

	// Token: 0x06003AA5 RID: 15013 RVA: 0x0017C540 File Offset: 0x0017A740
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

	// Token: 0x06003AA6 RID: 15014 RVA: 0x0017C5B8 File Offset: 0x0017A7B8
	public override int GetLength()
	{
		int num = ((this.recipientEntityIds == null) ? 0 : this.recipientEntityIds.Count);
		return 7 + this.msg.Length + 4 * num;
	}

	// Token: 0x04002FF0 RID: 12272
	[PublicizedFrom(EAccessModifier.Private)]
	public EChatType chatType;

	// Token: 0x04002FF1 RID: 12273
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityId;

	// Token: 0x04002FF2 RID: 12274
	[PublicizedFrom(EAccessModifier.Private)]
	public string msg;

	// Token: 0x04002FF3 RID: 12275
	[PublicizedFrom(EAccessModifier.Private)]
	public EMessageSender msgSender;

	// Token: 0x04002FF4 RID: 12276
	[PublicizedFrom(EAccessModifier.Private)]
	public GeneratedTextManager.BbCodeSupportMode bbMode;

	// Token: 0x04002FF5 RID: 12277
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> recipientEntityIds;
}
