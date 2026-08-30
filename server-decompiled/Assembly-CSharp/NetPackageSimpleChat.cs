using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000845 RID: 2117
[Preserve]
public class NetPackageSimpleChat : NetPackage
{
	// Token: 0x06003DEA RID: 15850 RVA: 0x0018804F File Offset: 0x0018624F
	public NetPackageSimpleChat Setup(string _msg)
	{
		this.msg = (string.IsNullOrEmpty(_msg) ? string.Empty : _msg);
		return this;
	}

	// Token: 0x06003DEB RID: 15851 RVA: 0x00188068 File Offset: 0x00186268
	public NetPackageSimpleChat Setup(string _msg, List<int> _recipientEntityIds)
	{
		this.msg = (string.IsNullOrEmpty(_msg) ? string.Empty : _msg);
		this.recipientEntityIds = _recipientEntityIds;
		return this;
	}

	// Token: 0x06003DEC RID: 15852 RVA: 0x00188088 File Offset: 0x00186288
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

	// Token: 0x06003DED RID: 15853 RVA: 0x001880D4 File Offset: 0x001862D4
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

	// Token: 0x06003DEE RID: 15854 RVA: 0x00188150 File Offset: 0x00186350
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

	// Token: 0x06003DEF RID: 15855 RVA: 0x00188298 File Offset: 0x00186498
	public override int GetLength()
	{
		return 4 + this.msg.Length;
	}

	// Token: 0x04003203 RID: 12803
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> recipientEntityIds;

	// Token: 0x04003204 RID: 12804
	[PublicizedFrom(EAccessModifier.Private)]
	public string msg;
}
