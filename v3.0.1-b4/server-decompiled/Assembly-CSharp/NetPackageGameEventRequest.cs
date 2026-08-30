using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007EB RID: 2027
[Preserve]
public class NetPackageGameEventRequest : NetPackage
{
	// Token: 0x06003BC0 RID: 15296 RVA: 0x0017EE9C File Offset: 0x0017D09C
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

	// Token: 0x06003BC1 RID: 15297 RVA: 0x0017EF00 File Offset: 0x0017D100
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

	// Token: 0x06003BC2 RID: 15298 RVA: 0x0017EFB8 File Offset: 0x0017D1B8
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

	// Token: 0x06003BC3 RID: 15299 RVA: 0x0017F098 File Offset: 0x0017D298
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

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06003BC4 RID: 15300 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x00104CB5 File Offset: 0x00102EB5
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x04003062 RID: 12386
	[PublicizedFrom(EAccessModifier.Private)]
	public string eventName;

	// Token: 0x04003063 RID: 12387
	[PublicizedFrom(EAccessModifier.Private)]
	public string extraData;

	// Token: 0x04003064 RID: 12388
	[PublicizedFrom(EAccessModifier.Private)]
	public string tag;

	// Token: 0x04003065 RID: 12389
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityID = -1;

	// Token: 0x04003066 RID: 12390
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTwitchEvent;

	// Token: 0x04003067 RID: 12391
	[PublicizedFrom(EAccessModifier.Private)]
	public bool crateShare;

	// Token: 0x04003068 RID: 12392
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowRefunds = true;

	// Token: 0x04003069 RID: 12393
	[PublicizedFrom(EAccessModifier.Private)]
	public ActionTarget target = ActionTarget.None;

	// Token: 0x0400306A RID: 12394
	[PublicizedFrom(EAccessModifier.Private)]
	public string sequenceLink = "";

	// Token: 0x0400306B RID: 12395
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Tuple<string, string>> variables = new List<Tuple<string, string>>();
}
