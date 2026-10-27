using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000803 RID: 2051
[Preserve]
public class NetPackageGameEventRequest : NetPackage
{
	// Token: 0x06003C42 RID: 15426 RVA: 0x0018288C File Offset: 0x00180A8C
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

	// Token: 0x06003C43 RID: 15427 RVA: 0x001828F0 File Offset: 0x00180AF0
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

	// Token: 0x06003C44 RID: 15428 RVA: 0x001829A8 File Offset: 0x00180BA8
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

	// Token: 0x06003C45 RID: 15429 RVA: 0x00182A88 File Offset: 0x00180C88
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

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06003C46 RID: 15430 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x040030FA RID: 12538
	[PublicizedFrom(EAccessModifier.Private)]
	public string eventName;

	// Token: 0x040030FB RID: 12539
	[PublicizedFrom(EAccessModifier.Private)]
	public string extraData;

	// Token: 0x040030FC RID: 12540
	[PublicizedFrom(EAccessModifier.Private)]
	public string tag;

	// Token: 0x040030FD RID: 12541
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityID = -1;

	// Token: 0x040030FE RID: 12542
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTwitchEvent;

	// Token: 0x040030FF RID: 12543
	[PublicizedFrom(EAccessModifier.Private)]
	public bool crateShare;

	// Token: 0x04003100 RID: 12544
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowRefunds = true;

	// Token: 0x04003101 RID: 12545
	[PublicizedFrom(EAccessModifier.Private)]
	public ActionTarget target = ActionTarget.None;

	// Token: 0x04003102 RID: 12546
	[PublicizedFrom(EAccessModifier.Private)]
	public string sequenceLink = "";

	// Token: 0x04003103 RID: 12547
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Tuple<string, string>> variables = new List<Tuple<string, string>>();
}
