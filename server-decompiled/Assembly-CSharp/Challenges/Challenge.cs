using System;
using System.Collections.Generic;
using System.IO;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018EB RID: 6379
	[Preserve]
	public class Challenge
	{
		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x0600C4A7 RID: 50343 RVA: 0x00487D52 File Offset: 0x00485F52
		// (set) Token: 0x0600C4A8 RID: 50344 RVA: 0x00487D5A File Offset: 0x00485F5A
		public byte CurrentFileVersion { get; set; }

		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x0600C4A9 RID: 50345 RVA: 0x00487D63 File Offset: 0x00485F63
		public bool IsActive
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Active;
			}
		}

		// Token: 0x14000117 RID: 279
		// (add) Token: 0x0600C4AA RID: 50346 RVA: 0x00487D70 File Offset: 0x00485F70
		// (remove) Token: 0x0600C4AB RID: 50347 RVA: 0x00487DA8 File Offset: 0x00485FA8
		public event ChallengeStateChanged OnChallengeStateChanged;

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x0600C4AC RID: 50348 RVA: 0x00487DDD File Offset: 0x00485FDD
		// (set) Token: 0x0600C4AD RID: 50349 RVA: 0x00487DE5 File Offset: 0x00485FE5
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

		// Token: 0x0600C4AE RID: 50350 RVA: 0x00487E02 File Offset: 0x00486002
		public void SetRequirementGroup(BaseRequirementObjectiveGroup requirementObjectiveGroup)
		{
			requirementObjectiveGroup.Owner = this;
			this.RequirementObjectiveGroup = requirementObjectiveGroup;
		}

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x0600C4AF RID: 50351 RVA: 0x00487E12 File Offset: 0x00486012
		public bool ReadyToComplete
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Completed || (this.ChallengeClass.RedeemAlways && this.ChallengeState == Challenge.ChallengeStates.Active);
			}
		}

		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x0600C4B0 RID: 50352 RVA: 0x00487E38 File Offset: 0x00486038
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

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x0600C4B1 RID: 50353 RVA: 0x00487E83 File Offset: 0x00486083
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

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x0600C4B2 RID: 50354 RVA: 0x00487EA4 File Offset: 0x004860A4
		public bool NeedsUIUpdate
		{
			get
			{
				return this.ChallengeState == Challenge.ChallengeStates.Active && this.ChallengeClass.NeedsConstantUIUpdate;
			}
		}

		// Token: 0x0600C4B3 RID: 50355 RVA: 0x00487EBC File Offset: 0x004860BC
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

		// Token: 0x0600C4B4 RID: 50356 RVA: 0x00487F64 File Offset: 0x00486164
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

		// Token: 0x0600C4B5 RID: 50357 RVA: 0x00487F9F File Offset: 0x0048619F
		[PublicizedFrom(EAccessModifier.Internal)]
		public Recipe GetRecipeFromRequirements()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				return this.RequirementObjectiveGroup.GetItemRecipe();
			}
			return null;
		}

		// Token: 0x0600C4B6 RID: 50358 RVA: 0x00487FB8 File Offset: 0x004861B8
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

		// Token: 0x0600C4B7 RID: 50359 RVA: 0x0048803D File Offset: 0x0048623D
		public bool ResetToChallengeClass()
		{
			return this.ChallengeClass != null && this.ChallengeClass.ResetObjectives(this);
		}

		// Token: 0x0600C4B8 RID: 50360 RVA: 0x00488055 File Offset: 0x00486255
		public List<BaseChallengeObjective> GetObjectiveList()
		{
			if (!this.NeedsPreRequisites)
			{
				return this.ObjectiveList;
			}
			return this.RequirementObjectiveGroup.CurrentObjectiveList;
		}

		// Token: 0x0600C4B9 RID: 50361 RVA: 0x00488074 File Offset: 0x00486274
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

		// Token: 0x0600C4BA RID: 50362 RVA: 0x004880D4 File Offset: 0x004862D4
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

		// Token: 0x0600C4BB RID: 50363 RVA: 0x00488164 File Offset: 0x00486364
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

		// Token: 0x0600C4BC RID: 50364 RVA: 0x004881F0 File Offset: 0x004863F0
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
			ChallengeCompletedEventData challengeCompletedEventData = new ChallengeCompletedEventData();
			challengeCompletedEventData.ServerId = Helper.GetServerId();
			string text;
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				GameManager instance = GameManager.Instance;
				if (instance == null)
				{
					text = null;
				}
				else
				{
					World world = instance.World;
					text = ((world != null) ? world.Guid : null);
				}
			}
			else
			{
				text = GamePrefs.GetString(EnumGamePrefs.GameGuidClient);
			}
			challengeCompletedEventData.SaveId = text;
			challengeCompletedEventData.ChallengeCompletedTimeStamp = DateTime.UtcNow.ToString("O");
			challengeCompletedEventData.ElapsedTime = (uint)(this.Owner.Player.totalTimePlayed * 60f);
			challengeCompletedEventData.PersonalGameStage = this.Owner.Player.gameStage;
			challengeCompletedEventData.PlayerLevel = this.Owner.Player.Progression.Level;
			challengeCompletedEventData.ChallengeName = this.ChallengeClass.Name;
			challengeCompletedEventData.ChallengeGroup = this.ChallengeGroup.Name;
			challengeCompletedEventData.ChallengeCategory = this.ChallengeGroup.Category;
			challengeCompletedEventData.IsForcedCompletion = forceComplete;
			ChallengeCompletedEventData challengeCompletedEventData2 = challengeCompletedEventData;
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(challengeCompletedEventData2);
		}

		// Token: 0x0600C4BD RID: 50365 RVA: 0x004883C0 File Offset: 0x004865C0
		public void Redeem()
		{
			GameEventManager.Current.HandleAction(this.ChallengeClass.RewardEvent, null, this.Owner.Player, false, "", "", false, true, "", null);
			this.Owner.HandleChallengeRedeemed(this);
			this.Owner.HandleChallengeGroupComplete(this.ChallengeGroup);
			ChallengeClaimedEventData challengeClaimedEventData = new ChallengeClaimedEventData();
			challengeClaimedEventData.ServerId = Helper.GetServerId();
			string text;
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				GameManager instance = GameManager.Instance;
				if (instance == null)
				{
					text = null;
				}
				else
				{
					World world = instance.World;
					text = ((world != null) ? world.Guid : null);
				}
			}
			else
			{
				text = GamePrefs.GetString(EnumGamePrefs.GameGuidClient);
			}
			challengeClaimedEventData.SaveId = text;
			challengeClaimedEventData.ChallengeClaimedTimeStamp = DateTime.UtcNow.ToString("O");
			challengeClaimedEventData.ElapsedTime = (uint)(this.Owner.Player.totalTimePlayed * 60f);
			challengeClaimedEventData.PersonalGameStage = this.Owner.Player.gameStage;
			challengeClaimedEventData.PlayerLevel = this.Owner.Player.Progression.Level;
			challengeClaimedEventData.ChallengeName = this.ChallengeClass.Name;
			challengeClaimedEventData.ChallengeGroup = this.ChallengeGroup.Name;
			challengeClaimedEventData.IsChallengeGroupComplete = this.ChallengeGroup.IsComplete;
			challengeClaimedEventData.ChallengeCategory = this.ChallengeGroup.Category;
			challengeClaimedEventData.RewardEvent = this.ChallengeClass.RewardEvent;
			ChallengeClaimedEventData challengeClaimedEventData2 = challengeClaimedEventData;
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(challengeClaimedEventData2);
		}

		// Token: 0x0600C4BE RID: 50366 RVA: 0x00488538 File Offset: 0x00486738
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

		// Token: 0x0600C4BF RID: 50367 RVA: 0x00488580 File Offset: 0x00486780
		public void HandleTrackingEnded()
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				this.ObjectiveList[i].HandleTrackingEnded();
			}
		}

		// Token: 0x0600C4C0 RID: 50368 RVA: 0x004885B4 File Offset: 0x004867B4
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

		// Token: 0x0600C4C1 RID: 50369 RVA: 0x004885E7 File Offset: 0x004867E7
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

		// Token: 0x0600C4C2 RID: 50370 RVA: 0x00488620 File Offset: 0x00486820
		public void RemoveTrackingEntry(TrackingEntry entry)
		{
			if (this.TrackingHandler == null)
			{
				return;
			}
			this.TrackingHandler.RemoveTrackingEntry(entry);
		}

		// Token: 0x0600C4C3 RID: 50371 RVA: 0x00488638 File Offset: 0x00486838
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

		// Token: 0x0600C4C4 RID: 50372 RVA: 0x00488709 File Offset: 0x00486909
		public void RemovePrerequisiteHooks()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				this.RequirementObjectiveGroup.HandleRemoveHooks();
			}
		}

		// Token: 0x0600C4C5 RID: 50373 RVA: 0x00488720 File Offset: 0x00486920
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

		// Token: 0x0600C4C6 RID: 50374 RVA: 0x004887A9 File Offset: 0x004869A9
		public void AddPrerequisiteHooks()
		{
			if (this.RequirementObjectiveGroup != null)
			{
				this.RequirementObjectiveGroup.HandleAddHooks();
			}
			this.CheckPrerequisites();
		}

		// Token: 0x0600C4C7 RID: 50375 RVA: 0x004887C4 File Offset: 0x004869C4
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

		// Token: 0x0600C4C8 RID: 50376 RVA: 0x00488800 File Offset: 0x00486A00
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

		// Token: 0x040094A0 RID: 38048
		public static byte FileVersion = 2;

		// Token: 0x040094A2 RID: 38050
		public Challenge.ChallengeStates ChallengeState;

		// Token: 0x040094A3 RID: 38051
		public bool IsTracked;

		// Token: 0x040094A4 RID: 38052
		public bool UIDirty;

		// Token: 0x040094A5 RID: 38053
		public bool AutoCompleted;

		// Token: 0x040094A6 RID: 38054
		public ChallengeClass ChallengeClass;

		// Token: 0x040094A7 RID: 38055
		public ChallengeGroup ChallengeGroup;

		// Token: 0x040094A8 RID: 38056
		public BaseRequirementObjectiveGroup RequirementObjectiveGroup;

		// Token: 0x040094A9 RID: 38057
		public List<BaseChallengeObjective> ObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x040094AB RID: 38059
		public ChallengeTrackingHandler TrackingHandler;

		// Token: 0x040094AC RID: 38060
		[PublicizedFrom(EAccessModifier.Private)]
		public bool needsPrerequisites;

		// Token: 0x040094AD RID: 38061
		public ChallengeJournal Owner;

		// Token: 0x020018EC RID: 6380
		public enum ChallengeStates : byte
		{
			// Token: 0x040094AF RID: 38063
			Active,
			// Token: 0x040094B0 RID: 38064
			Completed,
			// Token: 0x040094B1 RID: 38065
			Redeemed
		}
	}
}
