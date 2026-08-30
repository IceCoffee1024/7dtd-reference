using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007F6 RID: 2038
[Preserve]
public class NetPackageGameEventRequest : NetPackage
{
	// Token: 0x06003C1F RID: 15391 RVA: 0x00181274 File Offset: 0x0017F474
	public NetPackageGameEventRequest Setup(string _event, int _entityId, bool _isTwitchEvent, ActionTarget _target, List<Tuple<string, string>> _variables, string _extraData = "", string _tag = "", bool _crateShare = false, bool _allowRefunds = true, string _sequenceLink = "")
	{
		this.eventName = _event;
		this.entityID = _entityId;
		this.extraData = _extraData;
		this.tag = _tag;
		this.isTwitchEvent = _isTwitchEvent;
		this.crateShare = _crateShare;
		this.target = _target;
		this.allowRefunds = _allowRefunds;
		this.sequenceLink = _sequenceLink ?? "";
		this.variables = _variables;
		return this;
	}

	// Token: 0x06003C20 RID: 15392 RVA: 0x001812D8 File Offset: 0x0017F4D8
	public override void read(PooledBinaryReader _br)
	{
		this.variables.Clear();
		this.eventName = _br.ReadString();
		this.entityID = _br.ReadInt32();
		this.extraData = _br.ReadString();
		this.tag = _br.ReadString();
		this.isTwitchEvent = _br.ReadBoolean();
		this.crateShare = _br.ReadBoolean();
		this.allowRefunds = _br.ReadBoolean();
		this.sequenceLink = _br.ReadString();
		byte b = _br.ReadByte();
		for (int i = 0; i < (int)b; i++)
		{
			string text = _br.ReadString();
			string text2 = _br.ReadString();
			this.variables.Add(new Tuple<string, string>(text, text2));
		}
		this.target = ActionTarget.Read(_br);
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x00181390 File Offset: 0x0017F590
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.eventName);
		_bw.Write(this.entityID);
		_bw.Write(this.extraData);
		_bw.Write(this.tag);
		_bw.Write(this.isTwitchEvent);
		_bw.Write(this.crateShare);
		_bw.Write(this.allowRefunds);
		_bw.Write(this.sequenceLink);
		int num = 0;
		if (this.variables != null)
		{
			num = this.variables.Count;
			if (num > 255)
			{
				num = 255;
			}
		}
		byte b = (byte)num;
		_bw.Write(b);
		for (int i = 0; i < num; i++)
		{
			Tuple<string, string> tuple = this.variables[i];
			_bw.Write(tuple.Item1);
			_bw.Write(tuple.Item2);
		}
		this.target.Write(_bw);
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x00181470 File Offset: 0x0017F670
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(base.Sender.entityId) as EntityPlayer;
		Entity entity = GameManager.Instance.World.GetEntity(this.entityID);
		EntityPlayer entityPlayer2 = entity as EntityPlayer;
		if (entityPlayer2 == null || entityPlayer == entityPlayer2 || (entityPlayer.Party != null && entityPlayer.Party.ContainsMember(entityPlayer2)))
		{
			if (GameEventManager.Current.HandleAction(this.eventName, entityPlayer, entity, this.isTwitchEvent, this.target, this.extraData, this.tag, this.crateShare, this.allowRefunds, this.sequenceLink, null, this.variables))
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(this.eventName, entity ? entity.entityId : (-1), this.extraData, this.tag, NetPackageGameEventResponse.ResponseTypes.Approved, -1, -1, false, ""), false, base.Sender.entityId, -1, -1, null, 192, false);
				if (this.isTwitchEvent && entityPlayer.Party != null)
				{
					for (int i = 0; i < entityPlayer.Party.MemberList.Count; i++)
					{
						EntityPlayer entityPlayer3 = entityPlayer.Party.MemberList[i];
						if (entityPlayer3 != entityPlayer && entityPlayer3.TwitchEnabled)
						{
							if (entityPlayer3 is EntityPlayerLocal)
							{
								GameEventManager.Current.HandleTwitchPartyGameEventApproved(this.eventName, entity ? entity.entityId : (-1), this.extraData, this.tag);
							}
							else
							{
								SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(this.eventName, entity ? entity.entityId : (-1), this.extraData, this.tag, NetPackageGameEventResponse.ResponseTypes.TwitchPartyActionApproved, -1, -1, false, ""), false, entityPlayer3.entityId, -1, -1, null, 192, false);
							}
						}
					}
					return;
				}
			}
			else
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(this.eventName, entity ? entity.entityId : (-1), this.extraData, this.tag, NetPackageGameEventResponse.ResponseTypes.Denied, -1, -1, false, ""), false, base.Sender.entityId, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06003C23 RID: 15395 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x00105CC1 File Offset: 0x00103EC1
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x040030E1 RID: 12513
	[PublicizedFrom(EAccessModifier.Private)]
	public string eventName;

	// Token: 0x040030E2 RID: 12514
	[PublicizedFrom(EAccessModifier.Private)]
	public string extraData;

	// Token: 0x040030E3 RID: 12515
	[PublicizedFrom(EAccessModifier.Private)]
	public string tag;

	// Token: 0x040030E4 RID: 12516
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityID = -1;

	// Token: 0x040030E5 RID: 12517
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTwitchEvent;

	// Token: 0x040030E6 RID: 12518
	[PublicizedFrom(EAccessModifier.Private)]
	public bool crateShare;

	// Token: 0x040030E7 RID: 12519
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowRefunds = true;

	// Token: 0x040030E8 RID: 12520
	[PublicizedFrom(EAccessModifier.Private)]
	public ActionTarget target = ActionTarget.None;

	// Token: 0x040030E9 RID: 12521
	[PublicizedFrom(EAccessModifier.Private)]
	public string sequenceLink = "";

	// Token: 0x040030EA RID: 12522
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Tuple<string, string>> variables = new List<Tuple<string, string>>();
}
