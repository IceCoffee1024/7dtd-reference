using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000829 RID: 2089
[Preserve]
public class NetPackagePartyQuestChange : NetPackage
{
	// Token: 0x06003D09 RID: 15625 RVA: 0x00186032 File Offset: 0x00184232
	public NetPackagePartyQuestChange Setup(int _senderEntityID, byte _objectiveIndex, bool _isComplete, int _questCode)
	{
		this.senderEntityID = _senderEntityID;
		this.objectiveIndex = _objectiveIndex;
		this.isComplete = _isComplete;
		this.questCode = _questCode;
		return this;
	}

	// Token: 0x06003D0A RID: 15626 RVA: 0x00186052 File Offset: 0x00184252
	public override void read(PooledBinaryReader _br)
	{
		this.senderEntityID = _br.ReadInt32();
		this.objectiveIndex = _br.ReadByte();
		this.isComplete = _br.ReadBoolean();
		this.questCode = _br.ReadInt32();
	}

	// Token: 0x06003D0B RID: 15627 RVA: 0x00186084 File Offset: 0x00184284
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.senderEntityID);
		_bw.Write(this.objectiveIndex);
		_bw.Write(this.isComplete);
		_bw.Write(this.questCode);
	}

	// Token: 0x06003D0C RID: 15628 RVA: 0x001860C0 File Offset: 0x001842C0
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

	// Token: 0x06003D0D RID: 15629 RVA: 0x001861AC File Offset: 0x001843AC
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

	// Token: 0x040031B5 RID: 12725
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityID;

	// Token: 0x040031B6 RID: 12726
	[PublicizedFrom(EAccessModifier.Private)]
	public byte objectiveIndex;

	// Token: 0x040031B7 RID: 12727
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isComplete;

	// Token: 0x040031B8 RID: 12728
	[PublicizedFrom(EAccessModifier.Private)]
	public int questCode;
}
