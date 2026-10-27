using System;
using System.Collections.Generic;

namespace GameEvent.GameEventHelpers
{
	// Token: 0x020019AD RID: 6573
	public class HomerunManager
	{
		// Token: 0x0600CA04 RID: 51716 RVA: 0x0049AA10 File Offset: 0x00498C10
		public void Cleanup()
		{
			for (int i = 0; i < this.HomerunDataList.Count; i++)
			{
				this.HomerunDataList.list[i].Cleanup();
			}
			this.HomerunDataList.Clear();
		}

		// Token: 0x0600CA05 RID: 51717 RVA: 0x0049AA54 File Offset: 0x00498C54
		public void Update(float deltaTime)
		{
			for (int i = this.HomerunDataList.Count - 1; i >= 0; i--)
			{
				if (!this.HomerunDataList.list[i].Update(deltaTime))
				{
					HomerunData homerunData = this.HomerunDataList.list[i];
					homerunData.CompleteCallback();
					this.HomerunDataList.Remove(homerunData.Player);
					homerunData.Cleanup();
				}
			}
		}

		// Token: 0x0600CA06 RID: 51718 RVA: 0x0049AAC8 File Offset: 0x00498CC8
		public void AddPlayerToHomerun(EntityPlayer player, List<int> rewardLevels, List<string> rewardEvents, float gameTime, Action completeCallback)
		{
			if (!this.HomerunDataList.dict.ContainsKey(player))
			{
				this.HomerunDataList.Add(player, new HomerunData(player, gameTime, "twitch_homerungoal_red,twitch_homerungoal_blue,twitch_homerungoal_green", rewardLevels, rewardEvents, this, completeCallback));
			}
		}

		// Token: 0x0600CA07 RID: 51719 RVA: 0x0049AB06 File Offset: 0x00498D06
		public bool HasHomerunActive(EntityPlayer player)
		{
			return this.HomerunDataList.dict.ContainsKey(player);
		}

		// Token: 0x040097F4 RID: 38900
		public DictionaryList<EntityPlayer, HomerunData> HomerunDataList = new DictionaryList<EntityPlayer, HomerunData>();
	}
}
