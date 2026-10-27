using System;
using System.Collections.Generic;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001948 RID: 6472
	[Preserve]
	public class Challenge
	{
		// Token: 0x1700192B RID: 6443
		// (get) Token: 0x0600C713 RID: 50963 RVA: 0x0048FC46 File Offset: 0x0048DE46
		// (set) Token: 0x0600C714 RID: 50964 RVA: 0x0048FC4E File Offset: 0x0048DE4E
		public byte CurrentFileVersion { get; set; }

		// Token: 0x1700192C RID: 6444
		// (get) Token: 0x0600C715 RID: 50965 RVA: 0x0048FC57 File Offset: 0x0048DE57
		public bool IsActive
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Active;
			}
		}

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x0600C716 RID: 50966 RVA: 0x0048FC64 File Offset: 0x0048DE64
		// (remove) Token: 0x0600C717 RID: 50967 RVA: 0x0048FC9C File Offset: 0x0048DE9C
		public event ChallengeStateChanged OnChallengeStateChanged;

		// Token: 0x1700192D RID: 6445
		// (get) Token: 0x0600C718 RID: 50968 RVA: 0x0048FCD1 File Offset: 0x0048DED1
		// (set) Token: 0x0600C719 RID: 50969 RVA: 0x0048FCD9 File Offset: 0x0048DED9
		public bool NeedsPreRequisites
		{
			get
			{
				return this.needsPrerequisites;
			}
			set
			{
				this.needsPrerequisites = value;
				if (this.OnChallengeStateChanged != null)
				{
					this.OnChallengeStateChanged(this);
				}
			}
		}

		// Token: 0x0600C71A RID: 50970 RVA: 0x0048FCF6 File Offset: 0x0048DEF6
		public void SetRequirementGroup(BaseRequirementObjectiveGroup requirementObjectiveGroup)
		{
			requirementObjectiveGroup.Owner = this;
			this.RequirementObjectiveGroup = requirementObjectiveGroup;
		}

		// Token: 0x1700192E RID: 6446
		// (get) Token: 0x0600C71B RID: 50971 RVA: 0x0048FD06 File Offset: 0x0048DF06
		public bool ReadyToComplete
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Completed || (this.ChallengeClass.RedeemAlways && this.ChallengeState == Challenge.ChallengeStates.Active);
			}
		}

		// Token: 0x1700192F RID: 6447
		// (get) Token: 0x0600C71C RID: 50972 RVA: 0x0048FD2C File Offset: 0x0048DF2C
		public float FillAmount
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.ObjectiveList.Count; i++)
				{
					num += this.ObjectiveList[i].FillAmount;
				}
				return num / (float)this.ObjectiveList.Count;
			}
		}

		// Token: 0x17001930 RID: 6448
		// (get) Token: 0x0600C71D RID: 50973 RVA: 0x0048FD77 File Offset: 0x0048DF77
		public int ActiveObjectives
		{
			get
			{
				if (!this.NeedsPreRequisites)
				{
					return this.ObjectiveList.Count;
				}
				return this.RequirementObjectiveGroup.Count;
			}
		}

		// Token: 0x17001931 RID: 6449
		// (get) Token: 0x0600C71E RID: 50974 RVA: 0x0048FD98 File Offset: 0x0048DF98
		public bool NeedsUIUpdate
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Active && this.ChallengeClass.NeedsConstantUIUpdate;
			}
		}

		// Token: 0x0600C71F RID: 50975 RVA: 0x0048FDB0 File Offset: 0x0048DFB0
		public virtual void Read(PooledBinaryReader _br)
		{
			this.CurrentFileVersion = _br.ReadByte();
			string text = _br.ReadString();
			this.ChallengeState = (Challenge.ChallengeStates)_br.ReadByte();
			if (this.CurrentFileVersion >= 2)
			{
				this.AutoCompleted = _br.ReadBoolean();
			}
			byte b = _br.ReadByte();
			int num = _br.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				ChallengeObjectiveType challengeObjectiveType = (ChallengeObjectiveType)_br.ReadByte();
				this.ObjectiveList.Add(BaseChallengeObjective.ReadObjective(b, challengeObjectiveType, _br));
			}
			if (ChallengeClass.s_Challenges.ContainsKey(text))
			{
				this.ChallengeClass = ChallengeClass.s_Challenges[text];
				this.ChallengeGroup = this.ChallengeClass.ChallengeGroup;
			}
		}

		// Token: 0x0600C720 RID: 50976 RVA: 0x0048FE58 File Offset: 0x0048E058
		public ChallengeObjectiveChallengeComplete GetChallengeCompleteObjective()
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				ChallengeObjectiveChallengeComplete challengeObjectiveChallengeComplete = this.ObjectiveList[i] as ChallengeObjectiveChallengeComplete;
				if (challengeObjectiveChallengeComplete != null)
				{
					return challengeObjectiveChallengeComplete;
				}
			}
			return null;
		}

		// Token: 0x0600C721 RID: 50977 RVA: 0x0048FE93 File Offset: 0x0048E093
		[PublicizedFrom(EAccessModifier.Internal)]
		public Recipe GetRecipeFromRequirements()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				return this.RequirementObjectiveGroup.GetItemRecipe();
			}
			return null;
		}

		// Token: 0x0600C722 RID: 50978 RVA: 0x0048FEAC File Offset: 0x0048E0AC
		public virtual void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(Challenge.FileVersion);
			_bw.Write(this.ChallengeClass.Name);
			_bw.Write((byte)this.ChallengeState);
			_bw.Write(this.AutoCompleted);
			_bw.Write(BaseChallengeObjective.FileVersion);
			_bw.Write(this.ObjectiveList.Count);
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				this.ObjectiveList[i].WriteObjective(_bw);
			}
		}

		// Token: 0x0600C723 RID: 50979 RVA: 0x0048FF31 File Offset: 0x0048E131
		public bool ResetToChallengeClass()
		{
			return this.ChallengeClass != null && this.ChallengeClass.ResetObjectives(this);
		}

		// Token: 0x0600C724 RID: 50980 RVA: 0x0048FF49 File Offset: 0x0048E149
		public List<BaseChallengeObjective> GetObjectiveList()
		{
			if (!this.NeedsPreRequisites)
			{
				return this.ObjectiveList;
			}
			return this.RequirementObjectiveGroup.CurrentObjectiveList;
		}

		// Token: 0x0600C725 RID: 50981 RVA: 0x0048FF68 File Offset: 0x0048E168
		public List<Recipe> CraftedRecipes()
		{
			List<Recipe> list = null;
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				if (!this.ObjectiveList[i].Complete)
				{
					Recipe[] recipeItems = this.ObjectiveList[i].GetRecipeItems();
					if (recipeItems != null)
					{
						if (list == null)
						{
							list = new List<Recipe>();
						}
						list.AddRange(recipeItems);
					}
				}
			}
			return list;
		}

		// Token: 0x0600C726 RID: 50982 RVA: 0x0048FFC8 File Offset: 0x0048E1C8
		public void StartChallenge()
		{
			if (!this.ChallengeClass.HandleResourceRequirement())
			{
				for (int i = 0; i < this.ObjectiveList.Count; i++)
				{
					this.ObjectiveList[i].HandleAutoComplete();
				}
				this.HandleComplete(false, true);
				if (this.ChallengeState == Challenge.ChallengeStates.Completed)
				{
					this.AutoCompleted = true;
					this.ChallengeState = Challenge.ChallengeStates.Redeemed;
				}
			}
			if (this.IsActive)
			{
				for (int j = 0; j < this.ObjectiveList.Count; j++)
				{
					this.ObjectiveList[j].HandleAddHooks();
				}
			}
		}

		// Token: 0x0600C727 RID: 50983 RVA: 0x00490058 File Offset: 0x0048E258
		public void EndChallenge(bool isCompleted)
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				this.ObjectiveList[i].HandleRemoveHooks();
			}
			if (this.RequirementObjectiveGroup != null)
			{
				this.RequirementObjectiveGroup.HandleRemoveHooks();
			}
			if (isCompleted)
			{
				LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Player);
				if (uiforPlayer.xui.QuestTracker.TrackedChallenge == this)
				{
					uiforPlayer.xui.QuestTracker.TrackedChallenge = this.Owner.GetNextChallenge(this);
				}
			}
		}

		// Token: 0x0600C728 RID: 50984 RVA: 0x004900E4 File Offset: 0x0048E2E4
		public void HandleComplete(bool showTooltip = true, bool forceComplete = false)
		{
			bool flag = false;
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				if (!this.ObjectiveList[i].Complete)
				{
					flag = true;
					break;
				}
			}
			if (flag || this.ChallengeState == Challenge.ChallengeStates.Completed)
			{
				if (this.IsTracked)
				{
					this.CheckPrerequisites();
				}
				return;
			}
			if (this.ChallengeState == Challenge.ChallengeStates.Active)
			{
				this.ChallengeState = Challenge.ChallengeStates.Completed;
			}
			this.EndChallenge(true);
			QuestEventManager.Current.ChallengeCompleted(this.ChallengeClass, false);
			if (this.ChallengeClass.ChallengeGroup.IsVisible(this.Owner.Player) && showTooltip)
			{
				GameManager.ShowTooltip(this.Owner.Player, string.Format(Localization.Get("challengeMessageComplete", false, null), this.ChallengeClass.Title), "", "ui_challenge_complete", null, false, false, 0f);
			}
			ChallengeCompletedEventData challengeCompletedEventData = new ChallengeCompletedEventData
			{
				ServerId = Helper.GetServerId(),
				SaveId = Helper.GetSaveId(),
				ChallengeCompletedTimeStamp = DateTime.UtcNow.ToString("O"),
				ElapsedTime = (uint)(this.Owner.Player.totalTimePlayed * 60f),
				PersonalGameStage = this.Owner.Player.gameStage,
				PlayerLevel = this.Owner.Player.Progression.Level,
				ChallengeName = this.ChallengeClass.Name,
				ChallengeGroup = this.ChallengeGroup.Name,
				ChallengeCategory = this.ChallengeGroup.Category,
				IsForcedCompletion = forceComplete,
				PlayerLocation = this.Owner.Player.GetBlockPosition().ToString()
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(challengeCompletedEventData);
		}

		// Token: 0x0600C729 RID: 50985 RVA: 0x004902B0 File Offset: 0x0048E4B0
		public void Redeem()
		{
			GameEventManager.Current.HandleAction(this.ChallengeClass.RewardEvent, null, this.Owner.Player, false, "", "", false, true, "", null);
			this.Owner.HandleChallengeRedeemed(this);
			this.Owner.HandleChallengeGroupComplete(this.ChallengeGroup);
			ChallengeClaimedEventData challengeClaimedEventData = new ChallengeClaimedEventData
			{
				ServerId = Helper.GetServerId(),
				SaveId = Helper.GetSaveId(),
				ChallengeClaimedTimeStamp = DateTime.UtcNow.ToString("O"),
				ElapsedTime = (uint)(this.Owner.Player.totalTimePlayed * 60f),
				PersonalGameStage = this.Owner.Player.gameStage,
				PlayerLevel = this.Owner.Player.Progression.Level,
				ChallengeName = this.ChallengeClass.Name,
				ChallengeGroup = this.ChallengeGroup.Name,
				IsChallengeGroupComplete = this.ChallengeGroup.IsComplete,
				ChallengeCategory = this.ChallengeGroup.Category,
				RewardEvent = this.ChallengeClass.RewardEvent,
				PlayerLocation = this.Owner.Player.GetBlockPosition().ToString()
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(challengeClaimedEventData);
		}

		// Token: 0x0600C72A RID: 50986 RVA: 0x0049041C File Offset: 0x0048E61C
		public void HandleTrackingStarted()
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				if (!this.ObjectiveList[i].Complete)
				{
					this.ObjectiveList[i].HandleTrackingStarted();
				}
			}
		}

		// Token: 0x0600C72B RID: 50987 RVA: 0x00490464 File Offset: 0x0048E664
		public void HandleTrackingEnded()
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				this.ObjectiveList[i].HandleTrackingEnded();
			}
		}

		// Token: 0x0600C72C RID: 50988 RVA: 0x00490498 File Offset: 0x0048E698
		public ChallengeTrackingHandler GetTrackingHelper()
		{
			if (this.TrackingHandler == null)
			{
				this.TrackingHandler = new ChallengeTrackingHandler
				{
					Owner = this,
					LocalPlayer = this.Owner.Player
				};
			}
			return this.TrackingHandler;
		}

		// Token: 0x0600C72D RID: 50989 RVA: 0x004904CB File Offset: 0x0048E6CB
		public void AddTrackingEntry(TrackingEntry entry)
		{
			if (this.TrackingHandler == null)
			{
				this.TrackingHandler = new ChallengeTrackingHandler
				{
					Owner = this,
					LocalPlayer = this.Owner.Player
				};
			}
			this.TrackingHandler.AddTrackingEntry(entry);
		}

		// Token: 0x0600C72E RID: 50990 RVA: 0x00490504 File Offset: 0x0048E704
		public void RemoveTrackingEntry(TrackingEntry entry)
		{
			if (this.TrackingHandler == null)
			{
				return;
			}
			this.TrackingHandler.RemoveTrackingEntry(entry);
		}

		// Token: 0x0600C72F RID: 50991 RVA: 0x0049051C File Offset: 0x0048E71C
		public Challenge Clone()
		{
			Challenge challenge = new Challenge();
			challenge.ChallengeClass = this.ChallengeClass;
			challenge.ChallengeState = this.ChallengeState;
			challenge.IsTracked = this.IsTracked;
			challenge.AutoCompleted = this.AutoCompleted;
			if (this.RequirementObjectiveGroup != null)
			{
				BaseRequirementObjectiveGroup requirementObjectiveGroup = this.RequirementObjectiveGroup;
				BaseRequirementObjectiveGroup baseRequirementObjectiveGroup = requirementObjectiveGroup.Clone();
				baseRequirementObjectiveGroup.Owner = this;
				baseRequirementObjectiveGroup.ClonePhases(requirementObjectiveGroup);
				challenge.RequirementObjectiveGroup = baseRequirementObjectiveGroup;
			}
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				BaseChallengeObjective baseChallengeObjective = this.ObjectiveList[i].Clone();
				baseChallengeObjective.CopyValues(this.ObjectiveList[i], this.ChallengeClass.ObjectiveList[i]);
				baseChallengeObjective.Owner = challenge;
				challenge.ObjectiveList.Add(baseChallengeObjective);
			}
			return challenge;
		}

		// Token: 0x0600C730 RID: 50992 RVA: 0x004905ED File Offset: 0x0048E7ED
		public void RemovePrerequisiteHooks()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				this.RequirementObjectiveGroup.HandleRemoveHooks();
			}
		}

		// Token: 0x0600C731 RID: 50993 RVA: 0x00490604 File Offset: 0x0048E804
		public void CheckPrerequisites()
		{
			bool flag = false;
			if (this.RequirementObjectiveGroup != null && this.RequirementObjectiveGroup.HasPrerequisiteCondition())
			{
				if (this.RequirementObjectiveGroup.HandleCheckStatus())
				{
					flag = true;
					this.UIDirty = true;
				}
				this.RequirementObjectiveGroup.UpdateStatus();
			}
			if (this.NeedsPreRequisites != flag)
			{
				this.NeedsPreRequisites = flag;
				LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Player);
				if (uiforPlayer.xui.QuestTracker.TrackedChallenge == this)
				{
					uiforPlayer.xui.QuestTracker.HandleTrackedChallengeChanged();
				}
			}
		}

		// Token: 0x0600C732 RID: 50994 RVA: 0x0049068D File Offset: 0x0048E88D
		public void AddPrerequisiteHooks()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				this.RequirementObjectiveGroup.HandleAddHooks();
			}
			this.CheckPrerequisites();
		}

		// Token: 0x0600C733 RID: 50995 RVA: 0x004906A8 File Offset: 0x0048E8A8
		public BaseChallengeObjective GetNavObjective()
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				BaseChallengeObjective baseChallengeObjective = this.ObjectiveList[i];
				if (baseChallengeObjective.NavType != ChallengeClass.UINavTypes.None)
				{
					return baseChallengeObjective;
				}
			}
			return null;
		}

		// Token: 0x0600C734 RID: 50996 RVA: 0x004906E4 File Offset: 0x0048E8E4
		public void CompleteChallenge(bool forceRedeem = false, bool giveReward = true, bool forceComplete = false)
		{
			if (!this.IsActive && (!forceRedeem || this.ChallengeState != Challenge.ChallengeStates.Completed))
			{
				return;
			}
			foreach (BaseChallengeObjective baseChallengeObjective in this.ObjectiveList)
			{
				baseChallengeObjective.CompleteObjective(false);
			}
			this.HandleComplete(false, forceComplete);
			if (!forceRedeem)
			{
				return;
			}
			this.ChallengeState = Challenge.ChallengeStates.Redeemed;
			if (giveReward)
			{
				this.Redeem();
				QuestEventManager.Current.ChallengeCompleted(this.ChallengeClass, true);
			}
		}

		// Token: 0x0400964F RID: 38479
		public static byte FileVersion = 2;

		// Token: 0x04009651 RID: 38481
		public Challenge.ChallengeStates ChallengeState;

		// Token: 0x04009652 RID: 38482
		public bool IsTracked;

		// Token: 0x04009653 RID: 38483
		public bool UIDirty;

		// Token: 0x04009654 RID: 38484
		public bool AutoCompleted;

		// Token: 0x04009655 RID: 38485
		public ChallengeClass ChallengeClass;

		// Token: 0x04009656 RID: 38486
		public ChallengeGroup ChallengeGroup;

		// Token: 0x04009657 RID: 38487
		public BaseRequirementObjectiveGroup RequirementObjectiveGroup;

		// Token: 0x04009658 RID: 38488
		public List<BaseChallengeObjective> ObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x0400965A RID: 38490
		public ChallengeTrackingHandler TrackingHandler;

		// Token: 0x0400965B RID: 38491
		[PublicizedFrom(EAccessModifier.Private)]
		public bool needsPrerequisites;

		// Token: 0x0400965C RID: 38492
		public ChallengeJournal Owner;

		// Token: 0x02001949 RID: 6473
		public enum ChallengeStates : byte
		{
			// Token: 0x0400965E RID: 38494
			Active,
			// Token: 0x0400965F RID: 38495
			Completed,
			// Token: 0x04009660 RID: 38496
			Redeemed
		}
	}
}
