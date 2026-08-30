using System;
using System.Collections.Generic;

namespace GameEvent.GameEventHelpers
{
	// Token: 0x02001950 RID: 6480
	public class HomerunManager
	{
		// Token: 0x0600C798 RID: 51096 RVA: 0x00492B30 File Offset: 0x00490D30
		public void Cleanup()
		{
			for (int i = 0; i < this.HomerunDataList.Count; i++)
			{
				this.HomerunDataList.list[i].Cleanup();
			}
			this.HomerunDataList.Clear();
		}

		// Token: 0x0600C799 RID: 51097 RVA: 0x00492B74 File Offset: 0x00490D74
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

		// Token: 0x0600C79A RID: 51098 RVA: 0x00492BE8 File Offset: 0x00490DE8
		public void AddPlayerToHomerun(EntityPlayer player, List<int> rewardLevels, List<string> rewardEvents, float gameTime, Action completeCallback)
		{
			if (!this.HomerunDataList.dict.ContainsKey(player))
			{
				this.HomerunDataList.Add(player, new HomerunData(player, gameTime, "twitch_homerungoal_red,twitch_homerungoal_blue,twitch_homerungoal_green", rewardLevels, rewardEvents, this, completeCallback));
			}
		}

		// Token: 0x0600C79B RID: 51099 RVA: 0x00492C26 File Offset: 0x00490E26
		public bool HasHomerunActive(EntityPlayer player)
		{
			return this.HomerunDataList.dict.ContainsKey(player);
		}

		// Token: 0x04009645 RID: 38469
		public DictionaryList<EntityPlayer, HomerunData> HomerunDataList = new DictionaryList<EntityPlayer, HomerunData>();
	}
}
