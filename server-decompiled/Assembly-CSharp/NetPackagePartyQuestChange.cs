using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200081F RID: 2079
[Preserve]
public class NetPackagePartyQuestChange : NetPackage
{
	// Token: 0x06003CE6 RID: 15590 RVA: 0x00184F2E File Offset: 0x0018312E
	public NetPackagePartyQuestChange Setup(int _senderEntityID, byte _objectiveIndex, bool _isComplete, int _questCode)
	{
		this.senderEntityID = _senderEntityID;
		this.objectiveIndex = _objectiveIndex;
		this.isComplete = _isComplete;
		this.questCode = _questCode;
		return this;
	}

	// Token: 0x06003CE7 RID: 15591 RVA: 0x00184F4E File Offset: 0x0018314E
	public override void read(PooledBinaryReader _br)
	{
		this.senderEntityID = _br.ReadInt32();
		this.objectiveIndex = _br.ReadByte();
		this.isComplete = _br.ReadBoolean();
		this.questCode = _br.ReadInt32();
	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x00184F80 File Offset: 0x00183180
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.senderEntityID);
		_bw.Write(this.objectiveIndex);
		_bw.Write(this.isComplete);
		_bw.Write(this.questCode);
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x00184FBC File Offset: 0x001831BC
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

	// Token: 0x06003CEA RID: 15594 RVA: 0x001850A8 File Offset: 0x001832A8
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

	// Token: 0x04003195 RID: 12693
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityID;

	// Token: 0x04003196 RID: 12694
	[PublicizedFrom(EAccessModifier.Private)]
	public byte objectiveIndex;

	// Token: 0x04003197 RID: 12695
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isComplete;

	// Token: 0x04003198 RID: 12696
	[PublicizedFrom(EAccessModifier.Private)]
	public int questCode;
}
