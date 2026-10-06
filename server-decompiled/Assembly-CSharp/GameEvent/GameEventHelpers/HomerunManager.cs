using System;
using System.Collections.Generic;

namespace GameEvent.GameEventHelpers
{
	// Token: 0x0200197C RID: 6524
	public class HomerunManager
	{
		// Token: 0x0600C8F0 RID: 51440 RVA: 0x0049701C File Offset: 0x0049521C
		public void Cleanup()
		{
			for (int i = 0; i < this.HomerunDataList.Count; i++)
			{
				this.HomerunDataList.list[i].Cleanup();
			}
			this.HomerunDataList.Clear();
		}

		// Token: 0x0600C8F1 RID: 51441 RVA: 0x00497060 File Offset: 0x00495260
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

		// Token: 0x0600C8F2 RID: 51442 RVA: 0x004970D4 File Offset: 0x004952D4
		public void AddPlayerToHomerun(EntityPlayer player, List<int> rewardLevels, List<string> rewardEvents, float gameTime, Action completeCallback)
		{
			if (!this.HomerunDataList.dict.ContainsKey(player))
			{
				this.HomerunDataList.Add(player, new HomerunData(player, gameTime, "twitch_homerungoal_red,twitch_homerungoal_blue,twitch_homerungoal_green", rewardLevels, rewardEvents, this, completeCallback));
			}
		}

		// Token: 0x0600C8F3 RID: 51443 RVA: 0x00497112 File Offset: 0x00495312
		public bool HasHomerunActive(EntityPlayer player)
		{
			return this.HomerunDataList.dict.ContainsKey(player);
		}

		// Token: 0x0400970B RID: 38667
		public DictionaryList<EntityPlayer, HomerunData> HomerunDataList = new DictionaryList<EntityPlayer, HomerunData>();
	}
}
