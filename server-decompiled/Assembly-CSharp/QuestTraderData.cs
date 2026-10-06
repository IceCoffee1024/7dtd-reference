using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009F5 RID: 2549
public class QuestTraderData
{
	// Token: 0x06004B88 RID: 19336 RVA: 0x001D79C0 File Offset: 0x001D5BC0
	public QuestTraderData()
	{
	}

	// Token: 0x06004B89 RID: 19337 RVA: 0x001D79E5 File Offset: 0x001D5BE5
	public QuestTraderData(Vector2 traderPOI)
	{
		this.TraderPOI = traderPOI;
	}

	// Token: 0x06004B8A RID: 19338 RVA: 0x001D7A14 File Offset: 0x001D5C14
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

	// Token: 0x06004B8B RID: 19339 RVA: 0x001D7A8C File Offset: 0x001D5C8C
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

	// Token: 0x06004B8C RID: 19340 RVA: 0x001D7AF0 File Offset: 0x001D5CF0
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

	// Token: 0x06004B8D RID: 19341 RVA: 0x001D7B9E File Offset: 0x001D5D9E
	public List<Vector2> GetTierPOIs(int tier)
	{
		if (this.CompletedPOIByTier.ContainsKey(tier))
		{
			return this.CompletedPOIByTier[tier];
		}
		return null;
	}

	// Token: 0x06004B8E RID: 19342 RVA: 0x001D7BBC File Offset: 0x001D5DBC
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

	// Token: 0x06004B8F RID: 19343 RVA: 0x001D7C7C File Offset: 0x001D5E7C
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

	// Token: 0x04003BA6 RID: 15270
	public QuestJournal Owner;

	// Token: 0x04003BA7 RID: 15271
	public Vector2 TraderPOI;

	// Token: 0x04003BA8 RID: 15272
	public List<Vector2> TradersSentTo = new List<Vector2>();

	// Token: 0x04003BA9 RID: 15273
	public Dictionary<int, List<Vector2>> CompletedPOIByTier = new Dictionary<int, List<Vector2>>();

	// Token: 0x04003BAA RID: 15274
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetDay = -1;

	// Token: 0x04003BAB RID: 15275
	[PublicizedFrom(EAccessModifier.Private)]
	public static int resetStartTier = 4;

	// Token: 0x04003BAC RID: 15276
	[PublicizedFrom(EAccessModifier.Private)]
	public static int fullTierCount = 6;
}
