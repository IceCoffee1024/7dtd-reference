using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000832 RID: 2098
[Preserve]
public class NetPackageQuestTreasurePoint : NetPackage
{
	// Token: 0x06003D7B RID: 15739 RVA: 0x001862EC File Offset: 0x001844EC
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

	// Token: 0x06003D7C RID: 15740 RVA: 0x00186344 File Offset: 0x00184544
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

	// Token: 0x06003D7D RID: 15741 RVA: 0x00186390 File Offset: 0x00184590
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

	// Token: 0x06003D7E RID: 15742 RVA: 0x001863F2 File Offset: 0x001845F2
	public NetPackageQuestTreasurePoint Setup(int _questCode, Vector3i _updatedPosition)
	{
		this.questCode = _questCode;
		this.position = _updatedPosition;
		this.ActionType = NetPackageQuestTreasurePoint.QuestPointActions.UpdateTreasurePoint;
		return this;
	}

	// Token: 0x06003D7F RID: 15743 RVA: 0x0018640A File Offset: 0x0018460A
	public NetPackageQuestTreasurePoint Setup(int _questCode, int _blocksPerReduction)
	{
		this.questCode = _questCode;
		this.blocksPerReduction = _blocksPerReduction;
		this.ActionType = NetPackageQuestTreasurePoint.QuestPointActions.UpdateBlocksPerReduction;
		return this;
	}

	// Token: 0x06003D80 RID: 15744 RVA: 0x00186424 File Offset: 0x00184624
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

	// Token: 0x06003D81 RID: 15745 RVA: 0x001864CC File Offset: 0x001846CC
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

	// Token: 0x06003D82 RID: 15746 RVA: 0x0018657C File Offset: 0x0018477C
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

	// Token: 0x06003D83 RID: 15747 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x040031AC RID: 12716
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x040031AD RID: 12717
	[PublicizedFrom(EAccessModifier.Private)]
	public float distance;

	// Token: 0x040031AE RID: 12718
	[PublicizedFrom(EAccessModifier.Private)]
	public int offset;

	// Token: 0x040031AF RID: 12719
	[PublicizedFrom(EAccessModifier.Private)]
	public float treasureRadius;

	// Token: 0x040031B0 RID: 12720
	[PublicizedFrom(EAccessModifier.Private)]
	public int questCode;

	// Token: 0x040031B1 RID: 12721
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i position;

	// Token: 0x040031B2 RID: 12722
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useNearby;

	// Token: 0x040031B3 RID: 12723
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 treasureOffset = Vector3.zero;

	// Token: 0x040031B4 RID: 12724
	[PublicizedFrom(EAccessModifier.Private)]
	public int blocksPerReduction;

	// Token: 0x040031B5 RID: 12725
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageQuestTreasurePoint.QuestPointActions ActionType;

	// Token: 0x02000833 RID: 2099
	[PublicizedFrom(EAccessModifier.Private)]
	public enum QuestPointActions
	{
		// Token: 0x040031B7 RID: 12727
		GetGotoPoint,
		// Token: 0x040031B8 RID: 12728
		GetTreasurePoint,
		// Token: 0x040031B9 RID: 12729
		UpdateTreasurePoint,
		// Token: 0x040031BA RID: 12730
		UpdateBlocksPerReduction
	}
}
