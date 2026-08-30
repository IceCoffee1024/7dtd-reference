using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200083E RID: 2110
[Preserve]
public class NetPackageQuestTreasurePoint : NetPackage
{
	// Token: 0x06003DE0 RID: 15840 RVA: 0x0018830C File Offset: 0x0018650C
	public NetPackageQuestTreasurePoint Setup(int _playerId, float _distance, int _offset, int _questCode, int posX = 0, int posY = -1, int posZ = 0, bool _useNearby = false)
	{
		this.playerId = _playerId;
		this.distance = _distance;
		this.offset = _offset;
		this.questCode = _questCode;
		this.position = new Vector3i(posX, posY, posZ);
		this.useNearby = _useNearby;
		this.treasureOffset = Vector3.zero;
		this.ActionType = NetPackageQuestTreasurePoint.QuestPointActions.GetGotoPoint;
		return this;
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x00188364 File Offset: 0x00186564
	public NetPackageQuestTreasurePoint Setup(int _playerId, int _questCode, int _blocksPerReduction, Vector3i _position, Vector3 _treasureOffset)
	{
		this.playerId = _playerId;
		this.distance = 0f;
		this.offset = 0;
		this.questCode = _questCode;
		this.position = _position;
		this.treasureOffset = _treasureOffset;
		this.blocksPerReduction = _blocksPerReduction;
		this.ActionType = NetPackageQuestTreasurePoint.QuestPointActions.GetTreasurePoint;
		return this;
	}

	// Token: 0x06003DE2 RID: 15842 RVA: 0x001883B0 File Offset: 0x001865B0
	public NetPackageQuestTreasurePoint Setup(int _questCode, float _distance, int _offset, float _treasureRadius, Vector3 _startPosition, int _playerId, bool _useNearby, int _blocksPerReduction)
	{
		this.playerId = _playerId;
		this.distance = _distance;
		this.offset = _offset;
		this.questCode = _questCode;
		this.treasureRadius = _treasureRadius;
		this.position = new Vector3i(_startPosition);
		this.useNearby = _useNearby;
		this.treasureOffset = Vector3.zero;
		this.blocksPerReduction = _blocksPerReduction;
		this.ActionType = NetPackageQuestTreasurePoint.QuestPointActions.GetTreasurePoint;
		return this;
	}

	// Token: 0x06003DE3 RID: 15843 RVA: 0x00188412 File Offset: 0x00186612
	public NetPackageQuestTreasurePoint Setup(int _questCode, Vector3i _updatedPosition)
	{
		this.questCode = _questCode;
		this.position = _updatedPosition;
		this.ActionType = NetPackageQuestTreasurePoint.QuestPointActions.UpdateTreasurePoint;
		return this;
	}

	// Token: 0x06003DE4 RID: 15844 RVA: 0x0018842A File Offset: 0x0018662A
	public NetPackageQuestTreasurePoint Setup(int _questCode, int _blocksPerReduction)
	{
		this.questCode = _questCode;
		this.blocksPerReduction = _blocksPerReduction;
		this.ActionType = NetPackageQuestTreasurePoint.QuestPointActions.UpdateBlocksPerReduction;
		return this;
	}

	// Token: 0x06003DE5 RID: 15845 RVA: 0x00188444 File Offset: 0x00186644
	public override void read(PooledBinaryReader _br)
	{
		this.ActionType = (NetPackageQuestTreasurePoint.QuestPointActions)_br.ReadByte();
		if (this.ActionType == NetPackageQuestTreasurePoint.QuestPointActions.UpdateTreasurePoint)
		{
			this.questCode = _br.ReadInt32();
			this.position = StreamUtils.ReadVector3i(_br);
			return;
		}
		this.playerId = _br.ReadInt32();
		this.distance = _br.ReadSingle();
		this.offset = _br.ReadInt32();
		this.treasureRadius = _br.ReadSingle();
		this.blocksPerReduction = _br.ReadInt32();
		this.questCode = _br.ReadInt32();
		this.position = StreamUtils.ReadVector3i(_br);
		this.treasureOffset = StreamUtils.ReadVector3(_br);
		this.useNearby = _br.ReadBoolean();
	}

	// Token: 0x06003DE6 RID: 15846 RVA: 0x001884EC File Offset: 0x001866EC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.ActionType);
		if (this.ActionType == NetPackageQuestTreasurePoint.QuestPointActions.UpdateTreasurePoint)
		{
			_bw.Write(this.questCode);
			StreamUtils.Write(_bw, this.position);
			return;
		}
		_bw.Write(this.playerId);
		_bw.Write(this.distance);
		_bw.Write(this.offset);
		_bw.Write(this.treasureRadius);
		_bw.Write(this.blocksPerReduction);
		_bw.Write(this.questCode);
		StreamUtils.Write(_bw, this.position);
		StreamUtils.Write(_bw, this.treasureOffset);
		_bw.Write(this.useNearby);
	}

	// Token: 0x06003DE7 RID: 15847 RVA: 0x0018859C File Offset: 0x0018679C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (_world.IsRemote())
		{
			Quest quest = GameManager.Instance.World.GetPrimaryPlayer().QuestJournal.FindActiveQuest(this.questCode);
			if (quest != null)
			{
				for (int i = 0; i < quest.Objectives.Count; i++)
				{
					if (quest.CurrentPhase == quest.Objectives[i].Phase)
					{
						if (quest.Objectives[i] is ObjectiveTreasureChest)
						{
							if (this.ActionType == NetPackageQuestTreasurePoint.QuestPointActions.GetTreasurePoint)
							{
								((ObjectiveTreasureChest)quest.Objectives[i]).FinalizePointFromServer(this.blocksPerReduction, this.position, this.treasureOffset);
							}
							else if (this.ActionType == NetPackageQuestTreasurePoint.QuestPointActions.UpdateBlocksPerReduction)
							{
								((ObjectiveTreasureChest)quest.Objectives[i]).CurrentBlocksPerReduction = this.blocksPerReduction;
							}
						}
						else if (quest.Objectives[i] is ObjectiveRandomGoto)
						{
							((ObjectiveRandomGoto)quest.Objectives[i]).FinalizePoint(this.position.x, this.position.y, this.position.z);
						}
					}
				}
			}
			return;
		}
		if (this.ActionType == NetPackageQuestTreasurePoint.QuestPointActions.UpdateTreasurePoint)
		{
			QuestEventManager.Current.SetTreasureContainerPosition(this.questCode, this.position);
			return;
		}
		if (this.ActionType == NetPackageQuestTreasurePoint.QuestPointActions.UpdateBlocksPerReduction)
		{
			QuestEventManager.Current.UpdateTreasureBlocksPerReduction(this.questCode, this.blocksPerReduction);
			return;
		}
		for (int j = 0; j < 15; j++)
		{
			Vector3i vector3i;
			if (QuestEventManager.Current.GetTreasureContainerPosition(this.questCode, this.distance, this.offset, this.treasureRadius, this.position.ToVector3(), this.playerId, this.useNearby, this.blocksPerReduction, out this.blocksPerReduction, out vector3i, out this.treasureOffset))
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestTreasurePoint>().Setup(this.playerId, this.questCode, this.blocksPerReduction, vector3i, this.treasureOffset), false, this.playerId, -1, -1, null, 192, false);
				return;
			}
		}
	}

	// Token: 0x06003DE8 RID: 15848 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x0400322B RID: 12843
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x0400322C RID: 12844
	[PublicizedFrom(EAccessModifier.Private)]
	public float distance;

	// Token: 0x0400322D RID: 12845
	[PublicizedFrom(EAccessModifier.Private)]
	public int offset;

	// Token: 0x0400322E RID: 12846
	[PublicizedFrom(EAccessModifier.Private)]
	public float treasureRadius;

	// Token: 0x0400322F RID: 12847
	[PublicizedFrom(EAccessModifier.Private)]
	public int questCode;

	// Token: 0x04003230 RID: 12848
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i position;

	// Token: 0x04003231 RID: 12849
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useNearby;

	// Token: 0x04003232 RID: 12850
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 treasureOffset = Vector3.zero;

	// Token: 0x04003233 RID: 12851
	[PublicizedFrom(EAccessModifier.Private)]
	public int blocksPerReduction;

	// Token: 0x04003234 RID: 12852
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageQuestTreasurePoint.QuestPointActions ActionType;

	// Token: 0x0200083F RID: 2111
	[PublicizedFrom(EAccessModifier.Private)]
	public enum QuestPointActions
	{
		// Token: 0x04003236 RID: 12854
		GetGotoPoint,
		// Token: 0x04003237 RID: 12855
		GetTreasurePoint,
		// Token: 0x04003238 RID: 12856
		UpdateTreasurePoint,
		// Token: 0x04003239 RID: 12857
		UpdateBlocksPerReduction
	}
}
