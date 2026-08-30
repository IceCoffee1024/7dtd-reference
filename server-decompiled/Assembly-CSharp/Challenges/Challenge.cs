using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018C5 RID: 6341
	[Preserve]
	public class Challenge
	{
		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x0600C320 RID: 49952 RVA: 0x00481C26 File Offset: 0x0047FE26
		// (set) Token: 0x0600C321 RID: 49953 RVA: 0x00481C2E File Offset: 0x0047FE2E
		public byte CurrentFileVersion { get; set; }

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x0600C322 RID: 49954 RVA: 0x00481C37 File Offset: 0x0047FE37
		public bool IsActive
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Active;
			}
		}

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x0600C323 RID: 49955 RVA: 0x00481C44 File Offset: 0x0047FE44
		// (remove) Token: 0x0600C324 RID: 49956 RVA: 0x00481C7C File Offset: 0x0047FE7C
		public event ChallengeStateChanged OnChallengeStateChanged;

		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x0600C325 RID: 49957 RVA: 0x00481CB1 File Offset: 0x0047FEB1
		// (set) Token: 0x0600C326 RID: 49958 RVA: 0x00481CB9 File Offset: 0x0047FEB9
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

		// Token: 0x0600C327 RID: 49959 RVA: 0x00481CD6 File Offset: 0x0047FED6
		public void SetRequirementGroup(BaseRequirementObjectiveGroup requirementObjectiveGroup)
		{
			requirementObjectiveGroup.Owner = this;
			this.RequirementObjectiveGroup = requirementObjectiveGroup;
		}

		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x0600C328 RID: 49960 RVA: 0x00481CE6 File Offset: 0x0047FEE6
		public bool ReadyToComplete
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Completed || (this.ChallengeClass.RedeemAlways && this.ChallengeState == Challenge.ChallengeStates.Active);
			}
		}

		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x0600C329 RID: 49961 RVA: 0x00481D0C File Offset: 0x0047FF0C
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

		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x0600C32A RID: 49962 RVA: 0x00481D57 File Offset: 0x0047FF57
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

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x0600C32B RID: 49963 RVA: 0x00481D78 File Offset: 0x0047FF78
		public bool NeedsUIUpdate
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Active && this.ChallengeClass.NeedsConstantUIUpdate;
			}
		}

		// Token: 0x0600C32C RID: 49964 RVA: 0x00481D90 File Offset: 0x0047FF90
		public virtual void Read(BinaryReader _br)
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

		// Token: 0x0600C32D RID: 49965 RVA: 0x00481E38 File Offset: 0x00480038
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

		// Token: 0x0600C32E RID: 49966 RVA: 0x00481E73 File Offset: 0x00480073
		[PublicizedFrom(EAccessModifier.Internal)]
		public Recipe GetRecipeFromRequirements()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				return this.RequirementObjectiveGroup.GetItemRecipe();
			}
			return null;
		}

		// Token: 0x0600C32F RID: 49967 RVA: 0x00481E8C File Offset: 0x0048008C
		public virtual void Write(BinaryWriter _bw)
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

		// Token: 0x0600C330 RID: 49968 RVA: 0x00481F11 File Offset: 0x00480111
		public bool ResetToChallengeClass()
		{
			return this.ChallengeClass != null && this.ChallengeClass.ResetObjectives(this);
		}

		// Token: 0x0600C331 RID: 49969 RVA: 0x00481F29 File Offset: 0x00480129
		public List<BaseChallengeObjective> GetObjectiveList()
		{
			if (!this.NeedsPreRequisites)
			{
				return this.ObjectiveList;
			}
			return this.RequirementObjectiveGroup.CurrentObjectiveList;
		}

		// Token: 0x0600C332 RID: 49970 RVA: 0x00481F48 File Offset: 0x00480148
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

		// Token: 0x0600C333 RID: 49971 RVA: 0x00481FA8 File Offset: 0x004801A8
		public void StartChallenge()
		{
			if (!this.ChallengeClass.HandleResourceRequirement())
			{
				for (int i = 0; i < this.ObjectiveList.Count; i++)
				{
					this.ObjectiveList[i].HandleAutoComplete();
				}
				this.HandleComplete(false);
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

		// Token: 0x0600C334 RID: 49972 RVA: 0x00482038 File Offset: 0x00480238
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

		// Token: 0x0600C335 RID: 49973 RVA: 0x004820C4 File Offset: 0x004802C4
		public void HandleComplete(bool showTooltip = true)
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
			if (flag)
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
		}

		// Token: 0x0600C336 RID: 49974 RVA: 0x00482198 File Offset: 0x00480398
		public void Redeem()
		{
			GameEventManager.Current.HandleAction(this.ChallengeClass.RewardEvent, null, this.Owner.Player, false, "", "", false, true, "", null);
			this.Owner.HandleChallengeRedeemed(this);
			this.Owner.HandleChallengeGroupComplete(this.ChallengeGroup);
		}

		// Token: 0x0600C337 RID: 49975 RVA: 0x004821F8 File Offset: 0x004803F8
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

		// Token: 0x0600C338 RID: 49976 RVA: 0x00482240 File Offset: 0x00480440
		public void HandleTrackingEnded()
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				this.ObjectiveList[i].HandleTrackingEnded();
			}
		}

		// Token: 0x0600C339 RID: 49977 RVA: 0x00482274 File Offset: 0x00480474
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

		// Token: 0x0600C33A RID: 49978 RVA: 0x004822A7 File Offset: 0x004804A7
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

		// Token: 0x0600C33B RID: 49979 RVA: 0x004822E0 File Offset: 0x004804E0
		public void RemoveTrackingEntry(TrackingEntry entry)
		{
			if (this.TrackingHandler == null)
			{
				return;
			}
			this.TrackingHandler.RemoveTrackingEntry(entry);
		}

		// Token: 0x0600C33C RID: 49980 RVA: 0x004822F8 File Offset: 0x004804F8
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

		// Token: 0x0600C33D RID: 49981 RVA: 0x004823C9 File Offset: 0x004805C9
		public void RemovePrerequisiteHooks()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				this.RequirementObjectiveGroup.HandleRemoveHooks();
			}
		}

		// Token: 0x0600C33E RID: 49982 RVA: 0x004823E0 File Offset: 0x004805E0
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

		// Token: 0x0600C33F RID: 49983 RVA: 0x00482469 File Offset: 0x00480669
		public void AddPrerequisiteHooks()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				this.RequirementObjectiveGroup.HandleAddHooks();
			}
			this.CheckPrerequisites();
		}

		// Token: 0x0600C340 RID: 49984 RVA: 0x00482484 File Offset: 0x00480684
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

		// Token: 0x0600C341 RID: 49985 RVA: 0x004824C0 File Offset: 0x004806C0
		public void CompleteChallenge(bool forceRedeem = false, bool giveReward = true)
		{
			if (!this.IsActive && (!forceRedeem || this.ChallengeState != Challenge.ChallengeStates.Completed))
			{
				return;
			}
			foreach (BaseChallengeObjective baseChallengeObjective in this.ObjectiveList)
			{
				baseChallengeObjective.CompleteObjective(false);
			}
			this.HandleComplete(false);
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

		// Token: 0x04009377 RID: 37751
		public static byte FileVersion = 2;

		// Token: 0x04009379 RID: 37753
		public Challenge.ChallengeStates ChallengeState;

		// Token: 0x0400937A RID: 37754
		public bool IsTracked;

		// Token: 0x0400937B RID: 37755
		public bool UIDirty;

		// Token: 0x0400937C RID: 37756
		public bool AutoCompleted;

		// Token: 0x0400937D RID: 37757
		public ChallengeClass ChallengeClass;

		// Token: 0x0400937E RID: 37758
		public ChallengeGroup ChallengeGroup;

		// Token: 0x0400937F RID: 37759
		public BaseRequirementObjectiveGroup RequirementObjectiveGroup;

		// Token: 0x04009380 RID: 37760
		public List<BaseChallengeObjective> ObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x04009382 RID: 37762
		public ChallengeTrackingHandler TrackingHandler;

		// Token: 0x04009383 RID: 37763
		[PublicizedFrom(EAccessModifier.Private)]
		public bool needsPrerequisites;

		// Token: 0x04009384 RID: 37764
		public ChallengeJournal Owner;

		// Token: 0x020018C6 RID: 6342
		public enum ChallengeStates : byte
		{
			// Token: 0x04009386 RID: 37766
			Active,
			// Token: 0x04009387 RID: 37767
			Completed,
			// Token: 0x04009388 RID: 37768
			Redeemed
		}
	}
}
