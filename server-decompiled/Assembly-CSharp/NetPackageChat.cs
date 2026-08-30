using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007AD RID: 1965
[Preserve]
public class NetPackageChat : NetPackage
{
	// Token: 0x06003A4A RID: 14922 RVA: 0x0017A154 File Offset: 0x00178354
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

	// Token: 0x06003A4B RID: 14923 RVA: 0x0017A194 File Offset: 0x00178394
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

	// Token: 0x06003A4C RID: 14924 RVA: 0x0017A210 File Offset: 0x00178410
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

	// Token: 0x06003A4D RID: 14925 RVA: 0x0017A2C0 File Offset: 0x001784C0
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

	// Token: 0x06003A4E RID: 14926 RVA: 0x0017A338 File Offset: 0x00178538
	public override int GetLength()
	{
		int num = ((this.recipientEntityIds == null) ? 0 : this.recipientEntityIds.Count);
		return 7 + this.msg.Length + 4 * num;
	}

	// Token: 0x04002F77 RID: 12151
	[PublicizedFrom(EAccessModifier.Private)]
	public EChatType chatType;

	// Token: 0x04002F78 RID: 12152
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityId;

	// Token: 0x04002F79 RID: 12153
	[PublicizedFrom(EAccessModifier.Private)]
	public string msg;

	// Token: 0x04002F7A RID: 12154
	[PublicizedFrom(EAccessModifier.Private)]
	public EMessageSender msgSender;

	// Token: 0x04002F7B RID: 12155
	[PublicizedFrom(EAccessModifier.Private)]
	public GeneratedTextManager.BbCodeSupportMode bbMode;

	// Token: 0x04002F7C RID: 12156
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> recipientEntityIds;
}
