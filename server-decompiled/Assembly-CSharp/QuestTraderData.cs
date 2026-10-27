using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009FF RID: 2559
public class QuestTraderData
{
	// Token: 0x06004BAC RID: 19372 RVA: 0x001D8AA8 File Offset: 0x001D6CA8
	public QuestTraderData()
	{
	}

	// Token: 0x06004BAD RID: 19373 RVA: 0x001D8ACD File Offset: 0x001D6CCD
	public QuestTraderData(Vector2 traderPOI)
	{
		this.TraderPOI = traderPOI;
	}

	// Token: 0x06004BAE RID: 19374 RVA: 0x001D8AFC File Offset: 0x001D6CFC
	public void AddPOI(int tier, Vector2 poiPosition)
	{
		if (!this.CompletedPOIByTier.ContainsKey(tier))
		{
			this.CompletedPOIByTier.Add(tier, new List<Vector2>());
		}
		if (!this.CompletedPOIByTier[tier].Contains(poiPosition))
		{
			this.CompletedPOIByTier[tier].Add(poiPosition);
		}
		if (this.resetDay == -1)
		{
			this.resetDay = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
		}
	}

	// Token: 0x06004BAF RID: 19375 RVA: 0x001D8B74 File Offset: 0x001D6D74
	public void ClearTier(int tier)
	{
		if (tier == -1)
		{
			this.resetDay = -1;
			for (int i = QuestTraderData.resetStartTier; i <= QuestTraderData.fullTierCount; i++)
			{
				if (this.CompletedPOIByTier.ContainsKey(i))
				{
					this.CompletedPOIByTier.Remove(i);
				}
			}
			return;
		}
		if (this.CompletedPOIByTier.ContainsKey(tier))
		{
			this.CompletedPOIByTier.Remove(tier);
		}
	}

	// Token: 0x06004BB0 RID: 19376 RVA: 0x001D8BD8 File Offset: 0x001D6DD8
	public void CheckReset(EntityPlayer player)
	{
		if (this.resetDay != -1 && GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime) - this.resetDay >= 7)
		{
			this.resetDay = -1;
			for (int i = QuestTraderData.resetStartTier; i <= QuestTraderData.fullTierCount; i++)
			{
				if (this.CompletedPOIByTier.ContainsKey(i))
				{
					this.CompletedPOIByTier.Remove(i);
				}
			}
			if (!(player is EntityPlayerLocal))
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNPCQuestList>().SetupClear(player.entityId, this.TraderPOI, -1), false, player.entityId, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x06004BB1 RID: 19377 RVA: 0x001D8C86 File Offset: 0x001D6E86
	public List<Vector2> GetTierPOIs(int tier)
	{
		if (this.CompletedPOIByTier.ContainsKey(tier))
		{
			return this.CompletedPOIByTier[tier];
		}
		return null;
	}

	// Token: 0x06004BB2 RID: 19378 RVA: 0x001D8CA4 File Offset: 0x001D6EA4
	public void Read(PooledBinaryReader _br, byte version)
	{
		this.TraderPOI = StreamUtils.ReadVector2(_br);
		int num = (int)_br.ReadByte();
		this.CompletedPOIByTier.Clear();
		for (int i = 0; i < num; i++)
		{
			int num2 = (int)_br.ReadByte();
			int num3 = _br.ReadInt32();
			if (num3 > 0)
			{
				List<Vector2> list = new List<Vector2>();
				for (int j = 0; j < num3; j++)
				{
					list.Add(StreamUtils.ReadVector2(_br));
				}
				this.CompletedPOIByTier.Add(num2, list);
			}
		}
		int num4 = (int)_br.ReadByte();
		this.TradersSentTo.Clear();
		for (int k = 0; k < num4; k++)
		{
			this.TradersSentTo.Add(StreamUtils.ReadVector2(_br));
		}
		this.resetDay = _br.ReadInt32();
	}

	// Token: 0x06004BB3 RID: 19379 RVA: 0x001D8D64 File Offset: 0x001D6F64
	public void Write(PooledBinaryWriter _bw)
	{
		StreamUtils.Write(_bw, this.TraderPOI);
		_bw.Write((byte)this.CompletedPOIByTier.Count);
		foreach (int num in this.CompletedPOIByTier.Keys)
		{
			_bw.Write((byte)num);
			List<Vector2> list = this.CompletedPOIByTier[num];
			_bw.Write(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				StreamUtils.Write(_bw, list[i]);
			}
		}
		_bw.Write((byte)this.TradersSentTo.Count);
		for (int j = 0; j < this.TradersSentTo.Count; j++)
		{
			StreamUtils.Write(_bw, this.TradersSentTo[j]);
		}
		_bw.Write(this.resetDay);
	}

	// Token: 0x04003BC7 RID: 15303
	public QuestJournal Owner;

	// Token: 0x04003BC8 RID: 15304
	public Vector2 TraderPOI;

	// Token: 0x04003BC9 RID: 15305
	public List<Vector2> TradersSentTo = new List<Vector2>();

	// Token: 0x04003BCA RID: 15306
	public Dictionary<int, List<Vector2>> CompletedPOIByTier = new Dictionary<int, List<Vector2>>();

	// Token: 0x04003BCB RID: 15307
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetDay = -1;

	// Token: 0x04003BCC RID: 15308
	[PublicizedFrom(EAccessModifier.Private)]
	public static int resetStartTier = 4;

	// Token: 0x04003BCD RID: 15309
	[PublicizedFrom(EAccessModifier.Private)]
	public static int fullTierCount = 6;
}
