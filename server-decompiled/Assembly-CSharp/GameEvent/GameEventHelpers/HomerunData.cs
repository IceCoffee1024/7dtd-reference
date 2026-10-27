using System;
using System.Collections.Generic;
using Audio;
using GameEvent.SequenceActions;
using UnityEngine;

namespace GameEvent.GameEventHelpers
{
	// Token: 0x020019AE RID: 6574
	public class HomerunData
	{
		// Token: 0x17001986 RID: 6534
		// (get) Token: 0x0600CA09 RID: 51721 RVA: 0x0049AB2C File Offset: 0x00498D2C
		// (set) Token: 0x0600CA0A RID: 51722 RVA: 0x0049AB34 File Offset: 0x00498D34
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

		// Token: 0x0600CA0B RID: 51723 RVA: 0x0049AB50 File Offset: 0x00498D50
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

		// Token: 0x0600CA0C RID: 51724 RVA: 0x0049ACA4 File Offset: 0x00498EA4
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

		// Token: 0x0600CA0D RID: 51725 RVA: 0x0049AD54 File Offset: 0x00498F54
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

		// Token: 0x0600CA0E RID: 51726 RVA: 0x0049B1D4 File Offset: 0x004993D4
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

		// Token: 0x0600CA0F RID: 51727 RVA: 0x0049B2C8 File Offset: 0x004994C8
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

		// Token: 0x0600CA10 RID: 51728 RVA: 0x0049B3C4 File Offset: 0x004995C4
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

		// Token: 0x0600CA11 RID: 51729 RVA: 0x0049B480 File Offset: 0x00499680
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

		// Token: 0x040097F5 RID: 38901
		public List<EntityHomerunGoal> GoalControllers = new List<EntityHomerunGoal>();

		// Token: 0x040097F6 RID: 38902
		public EntityPlayer Player;

		// Token: 0x040097F7 RID: 38903
		public List<EntityPlayer> BuffedPlayers;

		// Token: 0x040097F8 RID: 38904
		public HomerunManager Owner;

		// Token: 0x040097F9 RID: 38905
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> rewardLevels;

		// Token: 0x040097FA RID: 38906
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> rewardEvents;

		// Token: 0x040097FB RID: 38907
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> entityIDs = new List<int>();

		// Token: 0x040097FC RID: 38908
		public float timeRemaining = 120f;

		// Token: 0x040097FD RID: 38909
		public int ExpectedCount = 3;

		// Token: 0x040097FE RID: 38910
		public Action CompleteCallback;

		// Token: 0x040097FF RID: 38911
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentScoreIndex;

		// Token: 0x04009800 RID: 38912
		[PublicizedFrom(EAccessModifier.Private)]
		public int score;

		// Token: 0x04009801 RID: 38913
		public List<HomerunData.ScoreDisplay> ScoreDisplays = new List<HomerunData.ScoreDisplay>();

		// Token: 0x04009802 RID: 38914
		[PublicizedFrom(EAccessModifier.Private)]
		public World world;

		// Token: 0x04009803 RID: 38915
		[PublicizedFrom(EAccessModifier.Private)]
		public float createTime = 1f;

		// Token: 0x04009804 RID: 38916
		[PublicizedFrom(EAccessModifier.Private)]
		public GameRandom gr;

		// Token: 0x020019AF RID: 6575
		public class ScoreDisplay
		{
			// Token: 0x0600CA12 RID: 51730 RVA: 0x0049B4BC File Offset: 0x004996BC
			public ScoreDisplay(int score, Vector3 position, Color color)
			{
				this.NavObject = NavObjectManager.Instance.RegisterNavObject("twitch_score", position, "", false, -1, null);
				this.NavObject.IsActive = true;
				this.NavObject.name = score.ToString();
				this.NavObject.UseOverrideFontColor = true;
				this.NavObject.OverrideColor = color;
			}

			// Token: 0x0600CA13 RID: 51731 RVA: 0x0049B52E File Offset: 0x0049972E
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

			// Token: 0x0600CA14 RID: 51732 RVA: 0x0049B554 File Offset: 0x00499754
			public void Cleanup()
			{
				if (this.NavObject != null)
				{
					this.RemoveNavObject();
				}
			}

			// Token: 0x0600CA15 RID: 51733 RVA: 0x0049B564 File Offset: 0x00499764
			[PublicizedFrom(EAccessModifier.Private)]
			public void RemoveNavObject()
			{
				this.Owner.RemoveScoreDisplay(this.NavObject.TrackedPosition);
				NavObjectManager.Instance.UnRegisterNavObject(this.NavObject);
				this.NavObject = null;
			}

			// Token: 0x04009805 RID: 38917
			public int Score;

			// Token: 0x04009806 RID: 38918
			public NavObject NavObject;

			// Token: 0x04009807 RID: 38919
			public float TimeRemaining = 3f;

			// Token: 0x04009808 RID: 38920
			public HomerunData Owner;
		}
	}
}
