using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020009EB RID: 2539
public class QuestTraderData
{
	// Token: 0x06004BB9 RID: 19385 RVA: 0x001D671C File Offset: 0x001D491C
	public QuestTraderData()
	{
	}

	// Token: 0x06004BBA RID: 19386 RVA: 0x001D6741 File Offset: 0x001D4941
	public QuestTraderData(Vector2 traderPOI)
	{
		this.TraderPOI = traderPOI;
	}

	// Token: 0x06004BBB RID: 19387 RVA: 0x001D6770 File Offset: 0x001D4970
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

	// Token: 0x06004BBC RID: 19388 RVA: 0x001D67E8 File Offset: 0x001D49E8
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

	// Token: 0x06004BBD RID: 19389 RVA: 0x001D684C File Offset: 0x001D4A4C
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

	// Token: 0x06004BBE RID: 19390 RVA: 0x001D68FA File Offset: 0x001D4AFA
	public List<Vector2> GetTierPOIs(int tier)
	{
		if (this.CompletedPOIByTier.ContainsKey(tier))
		{
			return this.CompletedPOIByTier[tier];
		}
		return null;
	}

	// Token: 0x06004BBF RID: 19391 RVA: 0x001D6918 File Offset: 0x001D4B18
	public void Read(BinaryReader _br, byte version)
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

	// Token: 0x06004BC0 RID: 19392 RVA: 0x001D69D8 File Offset: 0x001D4BD8
	public void Write(BinaryWriter _bw)
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

	// Token: 0x04003B99 RID: 15257
	public QuestJournal Owner;

	// Token: 0x04003B9A RID: 15258
	public Vector2 TraderPOI;

	// Token: 0x04003B9B RID: 15259
	public List<Vector2> TradersSentTo = new List<Vector2>();

	// Token: 0x04003B9C RID: 15260
	public Dictionary<int, List<Vector2>> CompletedPOIByTier = new Dictionary<int, List<Vector2>>();

	// Token: 0x04003B9D RID: 15261
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetDay = -1;

	// Token: 0x04003B9E RID: 15262
	[PublicizedFrom(EAccessModifier.Private)]
	public static int resetStartTier = 4;

	// Token: 0x04003B9F RID: 15263
	[PublicizedFrom(EAccessModifier.Private)]
	public static int fullTierCount = 6;
}
