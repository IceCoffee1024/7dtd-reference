using System;
using System.Collections.Generic;
using Audio;
using GameEvent.SequenceActions;
using UnityEngine;

namespace GameEvent.GameEventHelpers
{
	// Token: 0x02001951 RID: 6481
	public class HomerunData
	{
		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x0600C79D RID: 51101 RVA: 0x00492C4C File Offset: 0x00490E4C
		// (set) Token: 0x0600C79E RID: 51102 RVA: 0x00492C54 File Offset: 0x00490E54
		public int Score
		{
			get
			{
				return this.score;
			}
			set
			{
				this.score = value;
				this.currentScoreIndex = this.GetRewardIndex(this.currentScoreIndex, value);
			}
		}

		// Token: 0x0600C79F RID: 51103 RVA: 0x00492C70 File Offset: 0x00490E70
		public HomerunData(EntityPlayer player, float gameTime, string goalEntityNames, List<int> rewardLevels, List<string> rewardEvents, HomerunManager manager, Action completeCallback)
		{
			this.Player = player;
			this.Owner = manager;
			this.rewardLevels = rewardLevels;
			this.rewardEvents = rewardEvents;
			this.CompleteCallback = completeCallback;
			if (player.IsInParty())
			{
				this.BuffedPlayers = new List<EntityPlayer>();
				for (int i = 0; i < player.Party.MemberList.Count; i++)
				{
					EntityPlayer entityPlayer = player.Party.MemberList[i];
					if (!entityPlayer.Buffs.HasBuff("twitch_buffHomeRun"))
					{
						entityPlayer.Buffs.AddBuff("twitch_buffHomeRun", -1, true, false, -1f);
					}
					if (player != entityPlayer)
					{
						this.BuffedPlayers.Add(entityPlayer);
					}
				}
			}
			else if (!player.Buffs.HasBuff("twitch_buffHomeRun"))
			{
				player.Buffs.AddBuff("twitch_buffHomeRun", -1, true, false, -1f);
			}
			this.gr = GameEventManager.Current.Random;
			this.timeRemaining = gameTime;
			this.SetupEntityIDs(goalEntityNames);
			this.world = GameManager.Instance.World;
		}

		// Token: 0x0600C7A0 RID: 51104 RVA: 0x00492DC4 File Offset: 0x00490FC4
		public void SetupEntityIDs(string entityNames)
		{
			string[] array = entityNames.Split(',', StringSplitOptions.None);
			this.entityIDs.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
				{
					if (keyValuePair.Value.entityClassName == array[i])
					{
						this.entityIDs.Add(keyValuePair.Key);
						if (this.entityIDs.Count == array.Length)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600C7A1 RID: 51105 RVA: 0x00492E74 File Offset: 0x00491074
		public bool Update(float deltaTime)
		{
			for (int i = this.ScoreDisplays.Count - 1; i >= 0; i--)
			{
				if (!this.ScoreDisplays[i].Update(deltaTime))
				{
					this.ScoreDisplays.RemoveAt(i);
				}
			}
			if (this.Player.IsDead())
			{
				return false;
			}
			if (this.BuffedPlayers != null)
			{
				for (int j = this.BuffedPlayers.Count - 1; j >= 0; j--)
				{
					if (this.BuffedPlayers[j].IsDead())
					{
						this.BuffedPlayers.RemoveAt(j);
					}
				}
			}
			if (this.timeRemaining > 10f && this.timeRemaining - deltaTime < 10f)
			{
				if (!this.Player.Buffs.HasBuff("twitch_buffHomeRunEnding"))
				{
					this.Player.Buffs.AddBuff("twitch_buffHomeRunEnding", -1, true, false, -1f);
				}
				if (this.BuffedPlayers != null)
				{
					for (int k = 0; k < this.BuffedPlayers.Count; k++)
					{
						if (!this.BuffedPlayers[k].Buffs.HasBuff("twitch_buffHomeRunEnding"))
						{
							this.BuffedPlayers[k].Buffs.AddBuff("twitch_buffHomeRunEnding", -1, true, false, -1f);
						}
					}
				}
			}
			this.timeRemaining -= deltaTime;
			if (this.timeRemaining > 0f)
			{
				if (this.GoalControllers.Count < this.ExpectedCount)
				{
					this.createTime -= deltaTime;
					if (this.createTime <= 0f)
					{
						Vector3 zero = Vector3.zero;
						if (ActionBaseSpawn.FindValidPosition(out zero, this.Player, 6f, 12f, true, 1f, true))
						{
							EntityHomerunGoal entityHomerunGoal = EntityFactory.CreateEntity(this.entityIDs[this.gr.RandomRange(this.entityIDs.Count)], zero, Vector3.zero, this.Player.entityId, "") as EntityHomerunGoal;
							entityHomerunGoal.SetSpawnerSource(EnumSpawnerSource.Dynamic);
							GameManager.Instance.World.SpawnEntityInWorld(entityHomerunGoal);
							entityHomerunGoal.StartPosition = zero;
							entityHomerunGoal.position = zero;
							entityHomerunGoal.direction = (EntityHomerunGoal.Direction)this.gr.RandomRange(5);
							Manager.BroadcastPlayByLocalPlayer(entityHomerunGoal.position, "twitch_balloon_spawn");
							entityHomerunGoal.Owner = this;
							this.GoalControllers.Add(entityHomerunGoal);
							this.createTime = 1f;
						}
					}
				}
				for (int l = this.GoalControllers.Count - 1; l >= 0; l--)
				{
					EntityHomerunGoal entityHomerunGoal2 = this.GoalControllers[l];
					if (this.GoalControllers[l].ReadyForDelete)
					{
						Manager.BroadcastPlayByLocalPlayer(entityHomerunGoal2.position, "twitch_balloon_despawn");
						this.world.RemoveEntity(entityHomerunGoal2.entityId, EnumRemoveEntityReason.Killed);
						this.GoalControllers.RemoveAt(l);
					}
				}
				return true;
			}
			int num = -1;
			for (int m = this.rewardLevels.Count - 1; m >= 0; m--)
			{
				if (this.Score > this.rewardLevels[m])
				{
					num = m;
					break;
				}
			}
			if (num >= 0)
			{
				string text = string.Format(Localization.Get("ttTwitchHomerunScore", false, null), Utils.ColorToHex(QualityInfo.GetTierColor(this.currentScoreIndex)), this.Score);
				GameManager.ShowTooltipMP(this.Player, text, "");
				GameEventManager.Current.HandleAction(this.rewardEvents[num], this.Player, this.Player, false, "", "", false, true, "", null);
				if (this.BuffedPlayers != null)
				{
					for (int n = 0; n < this.BuffedPlayers.Count; n++)
					{
						GameManager.ShowTooltipMP(this.BuffedPlayers[n], text, "");
						GameEventManager.Current.HandleAction(this.rewardEvents[num], this.Player, this.BuffedPlayers[n], false, "", "", false, true, "", null);
					}
				}
			}
			else
			{
				string text2 = Localization.Get("ttTwitchHomerunFailed", false, null);
				GameManager.ShowTooltipMP(this.Player, text2, "");
				if (this.BuffedPlayers != null)
				{
					for (int num2 = 0; num2 < this.BuffedPlayers.Count; num2++)
					{
						GameManager.ShowTooltipMP(this.BuffedPlayers[num2], text2, "");
					}
				}
			}
			return false;
		}

		// Token: 0x0600C7A2 RID: 51106 RVA: 0x004932F4 File Offset: 0x004914F4
		public void Cleanup()
		{
			for (int i = this.ScoreDisplays.Count - 1; i >= 0; i--)
			{
				this.ScoreDisplays[i].Cleanup();
			}
			this.ScoreDisplays.Clear();
			for (int j = 0; j < this.GoalControllers.Count; j++)
			{
				if (this.GoalControllers[j] != null)
				{
					this.world.RemoveEntity(this.GoalControllers[j].entityId, EnumRemoveEntityReason.Killed);
				}
			}
			if (this.Player != null)
			{
				this.Player.Buffs.RemoveBuff("twitch_buffHomeRun", -1, true);
			}
			if (this.BuffedPlayers != null)
			{
				for (int k = 0; k < this.BuffedPlayers.Count; k++)
				{
					this.BuffedPlayers[k].Buffs.RemoveBuff("twitch_buffHomeRun", -1, true);
				}
			}
			this.GoalControllers.Clear();
		}

		// Token: 0x0600C7A3 RID: 51107 RVA: 0x004933E8 File Offset: 0x004915E8
		public void AddScoreDisplay(Vector3 position)
		{
			Color tierColor = QualityInfo.GetTierColor(this.currentScoreIndex);
			if (this.Player.isEntityRemote)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNavObject>().Setup("twitch_score", this.Score.ToString(), position, true, tierColor, false), false, this.Player.entityId, -1, -1, null, 192, false);
			}
			if (this.BuffedPlayers != null)
			{
				for (int i = 0; i < this.BuffedPlayers.Count; i++)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNavObject>().Setup("twitch_score", this.Score.ToString(), position, true, tierColor, false), false, this.BuffedPlayers[i].entityId, -1, -1, null, 192, false);
				}
			}
			this.ScoreDisplays.Add(new HomerunData.ScoreDisplay(this.Score, position, tierColor)
			{
				Owner = this
			});
		}

		// Token: 0x0600C7A4 RID: 51108 RVA: 0x004934E4 File Offset: 0x004916E4
		public void RemoveScoreDisplay(Vector3 position)
		{
			if (this.Player.isEntityRemote)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNavObject>().Setup("twitch_score", "", position, false, false, -1), false, this.Player.entityId, -1, -1, null, 192, false);
			}
			if (this.BuffedPlayers != null)
			{
				for (int i = 0; i < this.BuffedPlayers.Count; i++)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNavObject>().Setup("twitch_score", "", position, false, false, -1), false, this.BuffedPlayers[i].entityId, -1, -1, null, 192, false);
				}
			}
		}

		// Token: 0x0600C7A5 RID: 51109 RVA: 0x004935A0 File Offset: 0x004917A0
		[PublicizedFrom(EAccessModifier.Private)]
		public int GetRewardIndex(int currentIndex, int newScore)
		{
			int num = currentIndex + 1;
			while (num < this.rewardLevels.Count && newScore >= this.rewardLevels[num - 1])
			{
				currentIndex = num;
				num++;
			}
			return currentIndex;
		}

		// Token: 0x04009646 RID: 38470
		public List<EntityHomerunGoal> GoalControllers = new List<EntityHomerunGoal>();

		// Token: 0x04009647 RID: 38471
		public EntityPlayer Player;

		// Token: 0x04009648 RID: 38472
		public List<EntityPlayer> BuffedPlayers;

		// Token: 0x04009649 RID: 38473
		public HomerunManager Owner;

		// Token: 0x0400964A RID: 38474
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> rewardLevels;

		// Token: 0x0400964B RID: 38475
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> rewardEvents;

		// Token: 0x0400964C RID: 38476
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> entityIDs = new List<int>();

		// Token: 0x0400964D RID: 38477
		public float timeRemaining = 120f;

		// Token: 0x0400964E RID: 38478
		public int ExpectedCount = 3;

		// Token: 0x0400964F RID: 38479
		public Action CompleteCallback;

		// Token: 0x04009650 RID: 38480
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentScoreIndex;

		// Token: 0x04009651 RID: 38481
		[PublicizedFrom(EAccessModifier.Private)]
		public int score;

		// Token: 0x04009652 RID: 38482
		public List<HomerunData.ScoreDisplay> ScoreDisplays = new List<HomerunData.ScoreDisplay>();

		// Token: 0x04009653 RID: 38483
		[PublicizedFrom(EAccessModifier.Private)]
		public World world;

		// Token: 0x04009654 RID: 38484
		[PublicizedFrom(EAccessModifier.Private)]
		public float createTime = 1f;

		// Token: 0x04009655 RID: 38485
		[PublicizedFrom(EAccessModifier.Private)]
		public GameRandom gr;

		// Token: 0x02001952 RID: 6482
		public class ScoreDisplay
		{
			// Token: 0x0600C7A6 RID: 51110 RVA: 0x004935DC File Offset: 0x004917DC
			public ScoreDisplay(int score, Vector3 position, Color color)
			{
				this.NavObject = NavObjectManager.Instance.RegisterNavObject("twitch_score", position, "", false, -1, null);
				this.NavObject.IsActive = true;
				this.NavObject.name = score.ToString();
				this.NavObject.UseOverrideFontColor = true;
				this.NavObject.OverrideColor = color;
			}

			// Token: 0x0600C7A7 RID: 51111 RVA: 0x0049364E File Offset: 0x0049184E
			public bool Update(float deltaTime)
			{
				this.TimeRemaining -= deltaTime;
				if (this.TimeRemaining <= 0f)
				{
					this.RemoveNavObject();
					return false;
				}
				return true;
			}

			// Token: 0x0600C7A8 RID: 51112 RVA: 0x00493674 File Offset: 0x00491874
			public void Cleanup()
			{
				if (this.NavObject != null)
				{
					this.RemoveNavObject();
				}
			}

			// Token: 0x0600C7A9 RID: 51113 RVA: 0x00493684 File Offset: 0x00491884
			[PublicizedFrom(EAccessModifier.Private)]
			public void RemoveNavObject()
			{
				this.Owner.RemoveScoreDisplay(this.NavObject.TrackedPosition);
				NavObjectManager.Instance.UnRegisterNavObject(this.NavObject);
				this.NavObject = null;
			}

			// Token: 0x04009656 RID: 38486
			public int Score;

			// Token: 0x04009657 RID: 38487
			public NavObject NavObject;

			// Token: 0x04009658 RID: 38488
			public float TimeRemaining = 3f;

			// Token: 0x04009659 RID: 38489
			public HomerunData Owner;
		}
	}
}
