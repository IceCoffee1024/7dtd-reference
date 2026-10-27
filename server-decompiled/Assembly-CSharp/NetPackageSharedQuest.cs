using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000856 RID: 2134
[Preserve]
public class NetPackageSharedQuest : NetPackage
{
	// Token: 0x06003DF7 RID: 15863 RVA: 0x0018A263 File Offset: 0x00188463
	public NetPackageSharedQuest Setup(string _questUniqueId, int _questCode, int _sharedByEntityID)
	{
		this.sharedQuestData.questUniqueId = _questUniqueId;
		this.sharedQuestData.questCode = _questCode;
		this.sharedQuestData.sharedByEntityID = _sharedByEntityID;
		this.sharedQuestData.questEvent = NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.RemoveQuest;
		return this;
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x0018A298 File Offset: 0x00188498
	public NetPackageSharedQuest Setup(string _questUniqueId, int _questCode, int _sharedByEntityID, int _sharedWithEntityID, bool adding)
	{
		this.sharedQuestData.questUniqueId = _questUniqueId;
		this.sharedQuestData.questCode = _questCode;
		this.sharedQuestData.sharedByEntityID = _sharedByEntityID;
		this.sharedQuestData.sharedWithEntityID = _sharedWithEntityID;
		this.sharedQuestData.questEvent = (adding ? NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.AddSharedMember : NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.RemoveSharedMember);
		return this;
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x0018A2EC File Offset: 0x001884EC
	public NetPackageSharedQuest Setup(NetPackageSharedQuest.SharedQuestData sqd)
	{
		this.sharedQuestData.questUniqueId = sqd.questUniqueId;
		this.sharedQuestData.questCode = sqd.questCode;
		this.sharedQuestData.questID = sqd.questID;
		this.sharedQuestData.poiName = sqd.poiName;
		this.sharedQuestData.position = sqd.position;
		this.sharedQuestData.size = sqd.size;
		this.sharedQuestData.returnPos = sqd.returnPos;
		this.sharedQuestData.sharedByEntityID = sqd.sharedByEntityID;
		this.sharedQuestData.sharedWithEntityID = sqd.sharedWithEntityID;
		this.sharedQuestData.questGiverID = sqd.questGiverID;
		this.sharedQuestData.questEvent = NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.ShareQuest;
		return this;
	}

	// Token: 0x06003DFA RID: 15866 RVA: 0x0018A3B0 File Offset: 0x001885B0
	public override void read(PooledBinaryReader _br)
	{
		this.sharedQuestData.read(_br);
	}

	// Token: 0x06003DFB RID: 15867 RVA: 0x0018A3BE File Offset: 0x001885BE
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.sharedQuestData.write(_bw);
	}

	// Token: 0x06003DFC RID: 15868 RVA: 0x0018A3D4 File Offset: 0x001885D4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		int questCode = this.sharedQuestData.questCode;
		int sharedByEntityID = this.sharedQuestData.sharedByEntityID;
		int sharedWithEntityID = this.sharedQuestData.sharedWithEntityID;
		switch (this.sharedQuestData.questEvent)
		{
		case NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.ShareQuest:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				GameManager.Instance.QuestShareServer(this.sharedQuestData);
				return;
			}
			GameManager.Instance.QuestShareClient(this.sharedQuestData, null);
			return;
		case NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.RemoveQuest:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(sharedByEntityID) as EntityPlayer;
				if (entityPlayer != null && entityPlayer.Party != null)
				{
					for (int i = 0; i < entityPlayer.Party.MemberList.Count; i++)
					{
						EntityPlayer entityPlayer2 = entityPlayer.Party.MemberList[i];
						if (entityPlayer2 != entityPlayer)
						{
							if (entityPlayer2 is EntityPlayerLocal)
							{
								entityPlayer2.QuestJournal.RemoveSharedQuestByOwner(questCode);
								entityPlayer2.QuestJournal.RemoveSharedQuestEntry(questCode);
							}
							else
							{
								SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(this.sharedQuestData.questUniqueId, questCode, sharedByEntityID), false, entityPlayer2.entityId, -1, -1, null, 192, false);
							}
						}
					}
					return;
				}
			}
			else
			{
				List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
				if (localPlayers != null && localPlayers.Count > 0)
				{
					EntityPlayerLocal entityPlayerLocal = localPlayers[0];
					entityPlayerLocal.QuestJournal.RemoveSharedQuestByOwner(questCode);
					entityPlayerLocal.QuestJournal.RemoveSharedQuestEntry(this.sharedQuestData.questCode);
					return;
				}
			}
			break;
		case NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.AddSharedMember:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				EntityPlayer entityPlayer3 = GameManager.Instance.World.GetEntity(sharedByEntityID) as EntityPlayer;
				if (entityPlayer3 != null && entityPlayer3.Party != null)
				{
					EntityPlayerLocal entityPlayerLocal2 = entityPlayer3 as EntityPlayerLocal;
					if (entityPlayerLocal2 == null)
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(this.sharedQuestData.questUniqueId, questCode, sharedByEntityID, this.sharedQuestData.sharedWithEntityID, true), false, sharedByEntityID, -1, -1, null, 192, false);
						return;
					}
					EntityPlayer entityPlayer4 = GameManager.Instance.World.GetEntity(this.sharedQuestData.sharedWithEntityID) as EntityPlayer;
					if (entityPlayer4 != null)
					{
						Quest sharedQuest = entityPlayerLocal2.QuestJournal.GetSharedQuest(questCode);
						if (sharedQuest != null)
						{
							sharedQuest.AddSharedWith(entityPlayer4);
							GameManager.ShowTooltip(entityPlayerLocal2, string.Format(Localization.Get("ttQuestSharedAccepted", false, null), sharedQuest.QuestClass.Name, entityPlayer4.PlayerDisplayName), false, false, 0f);
							return;
						}
					}
				}
			}
			else
			{
				EntityPlayer entityPlayer5 = GameManager.Instance.World.GetEntity(sharedByEntityID) as EntityPlayer;
				EntityPlayerLocal entityPlayerLocal3 = entityPlayer5 as EntityPlayerLocal;
				if (entityPlayerLocal3 != null)
				{
					EntityPlayer entityPlayer6 = GameManager.Instance.World.GetEntity(sharedWithEntityID) as EntityPlayer;
					if (entityPlayer6 != null)
					{
						Quest sharedQuest2 = entityPlayer5.QuestJournal.GetSharedQuest(questCode);
						if (sharedQuest2 != null)
						{
							sharedQuest2.AddSharedWith(entityPlayer6);
							GameManager.ShowTooltip(entityPlayerLocal3, string.Format(Localization.Get("ttQuestSharedAccepted", false, null), sharedQuest2.QuestClass.Name, entityPlayer6.PlayerDisplayName), false, false, 0f);
							return;
						}
					}
				}
			}
			break;
		case NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.RemoveSharedMember:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				EntityPlayer entityPlayer7 = GameManager.Instance.World.GetEntity(sharedByEntityID) as EntityPlayer;
				if (entityPlayer7 != null)
				{
					EntityPlayerLocal entityPlayerLocal4 = entityPlayer7 as EntityPlayerLocal;
					if (entityPlayerLocal4 == null)
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(this.sharedQuestData.questUniqueId, questCode, sharedByEntityID, sharedWithEntityID, false), false, sharedByEntityID, -1, -1, null, 192, false);
						return;
					}
					EntityPlayer entityPlayer8 = GameManager.Instance.World.GetEntity(sharedWithEntityID) as EntityPlayer;
					if (entityPlayer8 != null)
					{
						Quest sharedQuest3 = entityPlayerLocal4.QuestJournal.GetSharedQuest(questCode);
						if (sharedQuest3 != null && sharedQuest3.RemoveSharedWith(entityPlayer8))
						{
							GameManager.ShowTooltip(entityPlayerLocal4, string.Format(Localization.Get("ttQuestSharedRemoved", false, null), sharedQuest3.QuestClass.Name, entityPlayer8.PlayerDisplayName), false, false, 0f);
							return;
						}
					}
				}
			}
			else
			{
				EntityPlayer entityPlayer9 = GameManager.Instance.World.GetEntity(sharedByEntityID) as EntityPlayer;
				EntityPlayerLocal entityPlayerLocal5 = entityPlayer9 as EntityPlayerLocal;
				if (entityPlayerLocal5 != null)
				{
					EntityPlayer entityPlayer10 = GameManager.Instance.World.GetEntity(sharedWithEntityID) as EntityPlayer;
					if (entityPlayer10 != null)
					{
						Quest sharedQuest4 = entityPlayer9.QuestJournal.GetSharedQuest(questCode);
						if (sharedQuest4 != null && sharedQuest4.RemoveSharedWith(entityPlayer10))
						{
							GameManager.ShowTooltip(entityPlayerLocal5, string.Format(Localization.Get("ttQuestSharedRemoved", false, null), sharedQuest4.QuestClass.Name, entityPlayer10.PlayerDisplayName), false, false, 0f);
						}
					}
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x04003275 RID: 12917
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageSharedQuest.SharedQuestData sharedQuestData = new NetPackageSharedQuest.SharedQuestData();

	// Token: 0x02000857 RID: 2135
	public class SharedQuestData
	{
		// Token: 0x06003DFE RID: 15870 RVA: 0x0018A8D5 File Offset: 0x00188AD5
		public SharedQuestData()
		{
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x0018A8E4 File Offset: 0x00188AE4
		public SharedQuestData(string _questUniqueId, int _questCode, string _questID, string _poiName, Vector3 _position, Vector3 _size, Vector3 _returnPos, int _sharedByEntityID, int _sharedWithEntityID, int _questGiverID)
		{
			this.questUniqueId = _questUniqueId;
			this.questCode = _questCode;
			this.questID = _questID;
			this.poiName = _poiName;
			this.position = _position;
			this.size = _size;
			this.returnPos = _returnPos;
			this.sharedByEntityID = _sharedByEntityID;
			this.sharedWithEntityID = _sharedWithEntityID;
			this.questGiverID = _questGiverID;
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x0018A94C File Offset: 0x00188B4C
		public SharedQuestData(NetPackageSharedQuest.SharedQuestData other)
		{
			this.questUniqueId = other.questUniqueId;
			this.questCode = other.questCode;
			this.questID = other.questID;
			this.poiName = other.poiName;
			this.position = other.position;
			this.size = other.size;
			this.returnPos = other.returnPos;
			this.sharedByEntityID = other.sharedByEntityID;
			this.sharedWithEntityID = other.sharedWithEntityID;
			this.questGiverID = other.questGiverID;
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x0018A9E0 File Offset: 0x00188BE0
		public void read(PooledBinaryReader _br)
		{
			this.sharedByEntityID = _br.ReadInt32();
			this.questEvent = (NetPackageSharedQuest.SharedQuestData.SharedQuestEvents)_br.ReadByte();
			this.questUniqueId = _br.ReadString();
			if (this.questEvent == NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.ShareQuest)
			{
				this.questCode = _br.ReadInt32();
				this.questID = _br.ReadString();
				this.poiName = _br.ReadString();
				this.position = StreamUtils.ReadVector3(_br);
				this.size = StreamUtils.ReadVector3(_br);
				this.returnPos = StreamUtils.ReadVector3(_br);
				this.questGiverID = _br.ReadInt32();
				this.sharedWithEntityID = _br.ReadInt32();
				return;
			}
			if (this.questEvent == NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.RemoveQuest)
			{
				this.questCode = _br.ReadInt32();
				return;
			}
			this.questCode = _br.ReadInt32();
			this.sharedWithEntityID = _br.ReadInt32();
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0018AAA8 File Offset: 0x00188CA8
		public void write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.sharedByEntityID);
			_bw.Write((byte)this.questEvent);
			_bw.Write(this.questUniqueId);
			if (this.questEvent == NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.ShareQuest)
			{
				_bw.Write(this.questCode);
				_bw.Write(this.questID);
				_bw.Write(this.poiName);
				StreamUtils.Write(_bw, this.position);
				StreamUtils.Write(_bw, this.size);
				StreamUtils.Write(_bw, this.returnPos);
				_bw.Write(this.questGiverID);
				_bw.Write(this.sharedWithEntityID);
				return;
			}
			if (this.questEvent == NetPackageSharedQuest.SharedQuestData.SharedQuestEvents.RemoveQuest)
			{
				_bw.Write(this.questCode);
				return;
			}
			_bw.Write(this.questCode);
			_bw.Write(this.sharedWithEntityID);
		}

		// Token: 0x04003276 RID: 12918
		public string questID;

		// Token: 0x04003277 RID: 12919
		public string poiName;

		// Token: 0x04003278 RID: 12920
		public Vector3 position;

		// Token: 0x04003279 RID: 12921
		public Vector3 size;

		// Token: 0x0400327A RID: 12922
		public Vector3 returnPos;

		// Token: 0x0400327B RID: 12923
		public int sharedByEntityID;

		// Token: 0x0400327C RID: 12924
		public int sharedWithEntityID;

		// Token: 0x0400327D RID: 12925
		public int questGiverID;

		// Token: 0x0400327E RID: 12926
		public string questUniqueId;

		// Token: 0x0400327F RID: 12927
		public int questCode = -1;

		// Token: 0x04003280 RID: 12928
		public NetPackageSharedQuest.SharedQuestData.SharedQuestEvents questEvent;

		// Token: 0x02000858 RID: 2136
		public enum SharedQuestEvents
		{
			// Token: 0x04003282 RID: 12930
			ShareQuest,
			// Token: 0x04003283 RID: 12931
			RemoveQuest,
			// Token: 0x04003284 RID: 12932
			AddSharedMember,
			// Token: 0x04003285 RID: 12933
			RemoveSharedMember
		}
	}
}
