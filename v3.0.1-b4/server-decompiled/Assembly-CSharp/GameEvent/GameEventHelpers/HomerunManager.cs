using System;
using System.Collections.Generic;

namespace GameEvent.GameEventHelpers
{
	// Token: 0x0200192A RID: 6442
	public class HomerunManager
	{
		// Token: 0x0600C611 RID: 50705 RVA: 0x0048C7E4 File Offset: 0x0048A9E4
		public void Cleanup()
		{
			for (int i = 0; i < this.HomerunDataList.Count; i++)
			{
				this.HomerunDataList.list[i].Cleanup();
			}
			this.HomerunDataList.Clear();
		}

		// Token: 0x0600C612 RID: 50706 RVA: 0x0048C828 File Offset: 0x0048AA28
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

		// Token: 0x0600C613 RID: 50707 RVA: 0x0048C89C File Offset: 0x0048AA9C
		public void AddPlayerToHomerun(EntityPlayer player, List<int> rewardLevels, List<string> rewardEvents, float gameTime, Action completeCallback)
		{
			if (!this.HomerunDataList.dict.ContainsKey(player))
			{
				this.HomerunDataList.Add(player, new HomerunData(player, gameTime, "twitch_homerungoal_red,twitch_homerungoal_blue,twitch_homerungoal_green", rewardLevels, rewardEvents, this, completeCallback));
			}
		}

		// Token: 0x0600C614 RID: 50708 RVA: 0x0048C8DA File Offset: 0x0048AADA
		public bool HasHomerunActive(EntityPlayer player)
		{
			return this.HomerunDataList.dict.ContainsKey(player);
		}

		// Token: 0x0400951C RID: 38172
		public DictionaryList<EntityPlayer, HomerunData> HomerunDataList = new DictionaryList<EntityPlayer, HomerunData>();
	}
}
