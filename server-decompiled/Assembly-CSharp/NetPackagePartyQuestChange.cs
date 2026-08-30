using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000816 RID: 2070
[Preserve]
public class NetPackagePartyQuestChange : NetPackage
{
	// Token: 0x06003CC5 RID: 15557 RVA: 0x00182F8A File Offset: 0x0018118A
	public NetPackagePartyQuestChange Setup(int _senderEntityID, byte _objectiveIndex, bool _isComplete, int _questCode)
	{
		this.senderEntityID = _senderEntityID;
		this.objectiveIndex = _objectiveIndex;
		this.isComplete = _isComplete;
		this.questCode = _questCode;
		return this;
	}

	// Token: 0x06003CC6 RID: 15558 RVA: 0x00182FAA File Offset: 0x001811AA
	public override void read(PooledBinaryReader _br)
	{
		this.senderEntityID = _br.ReadInt32();
		this.objectiveIndex = _br.ReadByte();
		this.isComplete = _br.ReadBoolean();
		this.questCode = _br.ReadInt32();
	}

	// Token: 0x06003CC7 RID: 15559 RVA: 0x00182FDC File Offset: 0x001811DC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.senderEntityID);
		_bw.Write(this.objectiveIndex);
		_bw.Write(this.isComplete);
		_bw.Write(this.questCode);
	}

	// Token: 0x06003CC8 RID: 15560 RVA: 0x00183018 File Offset: 0x00181218
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
			this.HandlePlayer(_world, primaryPlayer);
			return;
		}
		EntityPlayer entityPlayer = _world.GetEntity(this.senderEntityID) as EntityPlayer;
		if (entityPlayer == null || entityPlayer.Party == null)
		{
			return;
		}
		for (int i = 0; i < entityPlayer.Party.MemberList.Count; i++)
		{
			EntityPlayer entityPlayer2 = entityPlayer.Party.MemberList[i];
			if (entityPlayer2 != entityPlayer)
			{
				if (entityPlayer2 is EntityPlayerLocal)
				{
					this.HandlePlayer(_world, entityPlayer2 as EntityPlayerLocal);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyQuestChange>().Setup(this.senderEntityID, this.objectiveIndex, this.isComplete, this.questCode), false, entityPlayer2.entityId, -1, -1, null, 192, false);
				}
			}
		}
	}

	// Token: 0x06003CC9 RID: 15561 RVA: 0x00183104 File Offset: 0x00181304
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandlePlayer(World _world, EntityPlayerLocal localPlayer)
	{
		if (localPlayer == null)
		{
			Log.Warning("HandlePlayer NetPackagePartyQuestChange with no active local player");
			return;
		}
		EntityPlayer entityPlayer = _world.GetEntity(this.senderEntityID) as EntityPlayer;
		Quest sharedQuest = localPlayer.QuestJournal.GetSharedQuest(this.questCode);
		if (sharedQuest != null)
		{
			Rect locationRect = sharedQuest.GetLocationRect();
			bool flag;
			if (locationRect != Rect.zero)
			{
				Vector3 position = localPlayer.position;
				position.y = position.z;
				flag = locationRect.Contains(position);
			}
			else
			{
				flag = entityPlayer.GetDistance(localPlayer) < 15f;
			}
			if (flag)
			{
				sharedQuest.Objectives[(int)this.objectiveIndex].ChangeStatus(this.isComplete);
			}
			else
			{
				localPlayer.QuestJournal.RemoveSharedQuestByOwner(this.questCode);
			}
		}
		localPlayer.QuestJournal.RemoveSharedQuestEntry(this.questCode);
	}

	// Token: 0x06003CCA RID: 15562 RVA: 0x001306A9 File Offset: 0x0012E8A9
	public override int GetLength()
	{
		return 9;
	}

	// Token: 0x04003132 RID: 12594
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityID;

	// Token: 0x04003133 RID: 12595
	[PublicizedFrom(EAccessModifier.Private)]
	public byte objectiveIndex;

	// Token: 0x04003134 RID: 12596
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isComplete;

	// Token: 0x04003135 RID: 12597
	[PublicizedFrom(EAccessModifier.Private)]
	public int questCode;
}
