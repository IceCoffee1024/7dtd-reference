using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000821 RID: 2081
[Preserve]
public class NetPackagePartyQuestChange : NetPackage
{
	// Token: 0x06003D24 RID: 15652 RVA: 0x00185362 File Offset: 0x00183562
	public NetPackagePartyQuestChange Setup(int _senderEntityID, byte _objectiveIndex, bool _isComplete, int _questCode)
	{
		this.senderEntityID = _senderEntityID;
		this.objectiveIndex = _objectiveIndex;
		this.isComplete = _isComplete;
		this.questCode = _questCode;
		return this;
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x00185382 File Offset: 0x00183582
	public override void read(PooledBinaryReader _br)
	{
		this.senderEntityID = _br.ReadInt32();
		this.objectiveIndex = _br.ReadByte();
		this.isComplete = _br.ReadBoolean();
		this.questCode = _br.ReadInt32();
	}

	// Token: 0x06003D26 RID: 15654 RVA: 0x001853B4 File Offset: 0x001835B4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.senderEntityID);
		_bw.Write(this.objectiveIndex);
		_bw.Write(this.isComplete);
		_bw.Write(this.questCode);
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x001853F0 File Offset: 0x001835F0
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

	// Token: 0x06003D28 RID: 15656 RVA: 0x001854DC File Offset: 0x001836DC
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

	// Token: 0x06003D29 RID: 15657 RVA: 0x00131719 File Offset: 0x0012F919
	public override int GetLength()
	{
		return 9;
	}

	// Token: 0x040031B1 RID: 12721
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityID;

	// Token: 0x040031B2 RID: 12722
	[PublicizedFrom(EAccessModifier.Private)]
	public byte objectiveIndex;

	// Token: 0x040031B3 RID: 12723
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isComplete;

	// Token: 0x040031B4 RID: 12724
	[PublicizedFrom(EAccessModifier.Private)]
	public int questCode;
}
