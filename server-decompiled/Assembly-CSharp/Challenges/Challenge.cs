using System;
using System.Collections.Generic;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001917 RID: 6423
	[Preserve]
	public class Challenge
	{
		// Token: 0x170018E3 RID: 6371
		// (get) Token: 0x0600C5FF RID: 50687 RVA: 0x0048C29A File Offset: 0x0048A49A
		// (set) Token: 0x0600C600 RID: 50688 RVA: 0x0048C2A2 File Offset: 0x0048A4A2
		public byte CurrentFileVersion { get; set; }

		// Token: 0x170018E4 RID: 6372
		// (get) Token: 0x0600C601 RID: 50689 RVA: 0x0048C2AB File Offset: 0x0048A4AB
		public bool IsActive
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Active;
			}
		}

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x0600C602 RID: 50690 RVA: 0x0048C2B8 File Offset: 0x0048A4B8
		// (remove) Token: 0x0600C603 RID: 50691 RVA: 0x0048C2F0 File Offset: 0x0048A4F0
		public event ChallengeStateChanged OnChallengeStateChanged;

		// Token: 0x170018E5 RID: 6373
		// (get) Token: 0x0600C604 RID: 50692 RVA: 0x0048C325 File Offset: 0x0048A525
		// (set) Token: 0x0600C605 RID: 50693 RVA: 0x0048C32D File Offset: 0x0048A52D
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

		// Token: 0x0600C606 RID: 50694 RVA: 0x0048C34A File Offset: 0x0048A54A
		public void SetRequirementGroup(BaseRequirementObjectiveGroup requirementObjectiveGroup)
		{
			requirementObjectiveGroup.Owner = this;
			this.RequirementObjectiveGroup = requirementObjectiveGroup;
		}

		// Token: 0x170018E6 RID: 6374
		// (get) Token: 0x0600C607 RID: 50695 RVA: 0x0048C35A File Offset: 0x0048A55A
		public bool ReadyToComplete
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Completed || (this.ChallengeClass.RedeemAlways && this.ChallengeState == Challenge.ChallengeStates.Active);
			}
		}

		// Token: 0x170018E7 RID: 6375
		// (get) Token: 0x0600C608 RID: 50696 RVA: 0x0048C380 File Offset: 0x0048A580
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

		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x0600C609 RID: 50697 RVA: 0x0048C3CB File Offset: 0x0048A5CB
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

		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x0600C60A RID: 50698 RVA: 0x0048C3EC File Offset: 0x0048A5EC
		public bool NeedsUIUpdate
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Active && this.ChallengeClass.NeedsConstantUIUpdate;
			}
		}

		// Token: 0x0600C60B RID: 50699 RVA: 0x0048C404 File Offset: 0x0048A604
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

		// Token: 0x0600C60C RID: 50700 RVA: 0x0048C4AC File Offset: 0x0048A6AC
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

		// Token: 0x0600C60D RID: 50701 RVA: 0x0048C4E7 File Offset: 0x0048A6E7
		[PublicizedFrom(EAccessModifier.Internal)]
		public Recipe GetRecipeFromRequirements()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				return this.RequirementObjectiveGroup.GetItemRecipe();
			}
			return null;
		}

		// Token: 0x0600C60E RID: 50702 RVA: 0x0048C500 File Offset: 0x0048A700
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

		// Token: 0x0600C60F RID: 50703 RVA: 0x0048C585 File Offset: 0x0048A785
		public bool ResetToChallengeClass()
		{
			return this.ChallengeClass != null && this.ChallengeClass.ResetObjectives(this);
		}

		// Token: 0x0600C610 RID: 50704 RVA: 0x0048C59D File Offset: 0x0048A79D
		public List<BaseChallengeObjective> GetObjectiveList()
		{
			if (!this.NeedsPreRequisites)
			{
				return this.ObjectiveList;
			}
			return this.RequirementObjectiveGroup.CurrentObjectiveList;
		}

		// Token: 0x0600C611 RID: 50705 RVA: 0x0048C5BC File Offset: 0x0048A7BC
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

		// Token: 0x0600C612 RID: 50706 RVA: 0x0048C61C File Offset: 0x0048A81C
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

		// Token: 0x0600C613 RID: 50707 RVA: 0x0048C6AC File Offset: 0x0048A8AC
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

		// Token: 0x0600C614 RID: 50708 RVA: 0x0048C738 File Offset: 0x0048A938
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
				IsForcedCompletion = forceComplete
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(challengeCompletedEventData);
		}

		// Token: 0x0600C615 RID: 50709 RVA: 0x0048C8E0 File Offset: 0x0048AAE0
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
				RewardEvent = this.ChallengeClass.RewardEvent
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(challengeClaimedEventData);
		}

		// Token: 0x0600C616 RID: 50710 RVA: 0x0048CA28 File Offset: 0x0048AC28
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

		// Token: 0x0600C617 RID: 50711 RVA: 0x0048CA70 File Offset: 0x0048AC70
		public void HandleTrackingEnded()
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				this.ObjectiveList[i].HandleTrackingEnded();
			}
		}

		// Token: 0x0600C618 RID: 50712 RVA: 0x0048CAA4 File Offset: 0x0048ACA4
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

		// Token: 0x0600C619 RID: 50713 RVA: 0x0048CAD7 File Offset: 0x0048ACD7
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

		// Token: 0x0600C61A RID: 50714 RVA: 0x0048CB10 File Offset: 0x0048AD10
		public void RemoveTrackingEntry(TrackingEntry entry)
		{
			if (this.TrackingHandler == null)
			{
				return;
			}
			this.TrackingHandler.RemoveTrackingEntry(entry);
		}

		// Token: 0x0600C61B RID: 50715 RVA: 0x0048CB28 File Offset: 0x0048AD28
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

		// Token: 0x0600C61C RID: 50716 RVA: 0x0048CBF9 File Offset: 0x0048ADF9
		public void RemovePrerequisiteHooks()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				this.RequirementObjectiveGroup.HandleRemoveHooks();
			}
		}

		// Token: 0x0600C61D RID: 50717 RVA: 0x0048CC10 File Offset: 0x0048AE10
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

		// Token: 0x0600C61E RID: 50718 RVA: 0x0048CC99 File Offset: 0x0048AE99
		public void AddPrerequisiteHooks()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				this.RequirementObjectiveGroup.HandleAddHooks();
			}
			this.CheckPrerequisites();
		}

		// Token: 0x0600C61F RID: 50719 RVA: 0x0048CCB4 File Offset: 0x0048AEB4
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

		// Token: 0x0600C620 RID: 50720 RVA: 0x0048CCF0 File Offset: 0x0048AEF0
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

		// Token: 0x04009566 RID: 38246
		public static byte FileVersion = 2;

		// Token: 0x04009568 RID: 38248
		public Challenge.ChallengeStates ChallengeState;

		// Token: 0x04009569 RID: 38249
		public bool IsTracked;

		// Token: 0x0400956A RID: 38250
		public bool UIDirty;

		// Token: 0x0400956B RID: 38251
		public bool AutoCompleted;

		// Token: 0x0400956C RID: 38252
		public ChallengeClass ChallengeClass;

		// Token: 0x0400956D RID: 38253
		public ChallengeGroup ChallengeGroup;

		// Token: 0x0400956E RID: 38254
		public BaseRequirementObjectiveGroup RequirementObjectiveGroup;

		// Token: 0x0400956F RID: 38255
		public List<BaseChallengeObjective> ObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x04009571 RID: 38257
		public ChallengeTrackingHandler TrackingHandler;

		// Token: 0x04009572 RID: 38258
		[PublicizedFrom(EAccessModifier.Private)]
		public bool needsPrerequisites;

		// Token: 0x04009573 RID: 38259
		public ChallengeJournal Owner;

		// Token: 0x02001918 RID: 6424
		public enum ChallengeStates : byte
		{
			// Token: 0x04009575 RID: 38261
			Active,
			// Token: 0x04009576 RID: 38262
			Completed,
			// Token: 0x04009577 RID: 38263
			Redeemed
		}
	}
}
