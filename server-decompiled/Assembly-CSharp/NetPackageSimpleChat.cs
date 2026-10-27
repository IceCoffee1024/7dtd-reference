using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200085E RID: 2142
[Preserve]
public class NetPackageSimpleChat : NetPackage
{
	// Token: 0x06003E1F RID: 15903 RVA: 0x0018AFE1 File Offset: 0x001891E1
	public NetPackageSimpleChat Setup(string _msg)
	{
		this.msg = (string.IsNullOrEmpty(_msg) ? string.Empty : _msg);
		return this;
	}

	// Token: 0x06003E20 RID: 15904 RVA: 0x0018AFFA File Offset: 0x001891FA
	public NetPackageSimpleChat Setup(string _msg, List<int> _recipientEntityIds)
	{
		this.msg = (string.IsNullOrEmpty(_msg) ? string.Empty : _msg);
		this.recipientEntityIds = _recipientEntityIds;
		return this;
	}

	// Token: 0x06003E21 RID: 15905 RVA: 0x0018B01C File Offset: 0x0018921C
	public override void read(PooledBinaryReader _br)
	{
		this.msg = _br.ReadString();
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

	// Token: 0x06003E22 RID: 15906 RVA: 0x0018B068 File Offset: 0x00189268
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.msg);
		_bw.Write((this.recipientEntityIds != null) ? this.recipientEntityIds.Count : 0);
		if (this.recipientEntityIds != null && this.recipientEntityIds.Count > 0)
		{
			for (int i = 0; i < this.recipientEntityIds.Count; i++)
			{
				_bw.Write(this.recipientEntityIds[i]);
			}
		}
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x0018B0E4 File Offset: 0x001892E4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!_world.IsRemote())
		{
			if (this.recipientEntityIds == null)
			{
				return;
			}
			using (List<int>.Enumerator enumerator = this.recipientEntityIds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num = enumerator.Current;
					EntityPlayerLocal entityPlayerLocal = _world.GetEntity(num) as EntityPlayerLocal;
					if (entityPlayerLocal != null)
					{
						LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
						if (null != uiforPlayer && null != uiforPlayer.windowManager)
						{
							XUiC_ChatOutput.AddMessage(uiforPlayer.xui, EnumGameMessages.PlainTextLocal, this.msg, EChatType.Global, EChatDirection.Inbound, -1, null, null, EMessageSender.Server, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
						}
					}
					else
					{
						ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(num);
						if (clientInfo != null)
						{
							clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageSimpleChat>().Setup(this.msg));
						}
					}
				}
				return;
			}
		}
		List<EntityPlayerLocal> localPlayers = _callbacks.World.GetLocalPlayers();
		for (int i = 0; i < localPlayers.Count; i++)
		{
			LocalPlayerUI uiforPlayer2 = LocalPlayerUI.GetUIForPlayer(localPlayers[i]);
			if (null != uiforPlayer2 && null != uiforPlayer2.windowManager)
			{
				XUiC_ChatOutput.AddMessage(uiforPlayer2.xui, EnumGameMessages.PlainTextLocal, this.msg, EChatType.Global, EChatDirection.Inbound, -1, null, null, EMessageSender.Server, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
			}
		}
	}

	// Token: 0x0400328D RID: 12941
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> recipientEntityIds;

	// Token: 0x0400328E RID: 12942
	[PublicizedFrom(EAccessModifier.Private)]
	public string msg;
}
