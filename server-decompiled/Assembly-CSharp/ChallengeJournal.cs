using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Challenges;
using UniLinq;
using UnityEngine;

// Token: 0x0200060A RID: 1546
public class ChallengeJournal
{
	// Token: 0x0600328D RID: 12941 RVA: 0x001486DC File Offset: 0x001468DC
	public void Read(PooledBinaryReader _br)
	{
		this.SetupData();
		if (_br.ReadByte() == 1)
		{
			return;
		}
		int num = _br.ReadInt32();
		this.Challenges.Clear();
		this.ChallengeDictionary.Clear();
		this.CompleteChallengesForMinEvents.Clear();
		for (int i = 0; i < num; i++)
		{
			Challenge challenge = new Challenge();
			challenge.Owner = this;
			challenge.Read(_br);
			if (challenge.ResetToChallengeClass())
			{
				if (challenge.ChallengeState == Challenge.ChallengeStates.Redeemed && this.eventList.ContainsKey(challenge.ChallengeClass.Name))
				{
					this.CompleteChallengesForMinEvents.Add(challenge);
				}
				this.ChallengeDictionary.Add(challenge.ChallengeClass.Name, challenge);
			}
		}
		this.Challenges = this.ChallengeDictionary.Values.OrderBy<Challenge, int>((Challenge c) => c.ChallengeClass.OrderIndex).ToList<Challenge>();
		if (this.ChallengeGroups.Count == 0 && !GameManager.Instance.World.IsEditor() && !GameUtils.IsWorldEditor() && !GameUtils.IsPlaytesting())
		{
			foreach (ChallengeGroup challengeGroup in ChallengeGroup.s_ChallengeGroups.Values)
			{
				ChallengeGroupEntry challengeGroupEntry = new ChallengeGroupEntry(challengeGroup);
				this.ChallengeGroups.Add(challengeGroupEntry);
			}
		}
		num = _br.ReadInt32();
		for (int j = 0; j < num; j++)
		{
			string text = _br.ReadString();
			int num2 = _br.ReadInt32();
			for (int k = 0; k < this.ChallengeGroups.Count; k++)
			{
				if (this.ChallengeGroups[k].ChallengeGroup.Name == text)
				{
					this.ChallengeGroups[k].LastUpdateDay = num2;
				}
			}
		}
		string text2 = _br.ReadString();
		if (text2 != "" && this.ChallengeDictionary.ContainsKey(text2))
		{
			this.ChallengeDictionary[text2].IsTracked = true;
		}
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x001488F4 File Offset: 0x00146AF4
	public void Write(PooledBinaryWriter _bw)
	{
		string text = "";
		_bw.Write(2);
		_bw.Write(this.Challenges.Count);
		for (int i = 0; i < this.Challenges.Count; i++)
		{
			Challenge challenge = this.Challenges[i];
			challenge.Write(_bw);
			if (challenge.IsTracked)
			{
				text = challenge.ChallengeClass.Name;
			}
		}
		int num = 0;
		for (int j = 0; j < this.ChallengeGroups.Count; j++)
		{
			if (this.ChallengeGroups[j].LastUpdateDay != -1)
			{
				num++;
			}
		}
		_bw.Write(num);
		for (int k = 0; k < this.ChallengeGroups.Count; k++)
		{
			if (this.ChallengeGroups[k].LastUpdateDay != -1)
			{
				_bw.Write(this.ChallengeGroups[k].ChallengeGroup.Name);
				_bw.Write(this.ChallengeGroups[k].LastUpdateDay);
			}
		}
		_bw.Write(text);
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x00148A04 File Offset: 0x00146C04
	public ChallengeJournal Clone()
	{
		ChallengeJournal challengeJournal = new ChallengeJournal();
		challengeJournal.Player = this.Player;
		for (int i = 0; i < this.ChallengeGroups.Count; i++)
		{
			challengeJournal.ChallengeGroups.Add(this.ChallengeGroups[i]);
		}
		for (int j = 0; j < this.Challenges.Count; j++)
		{
			Challenge challenge = this.Challenges[j].Clone();
			challengeJournal.ChallengeDictionary.Add(challenge.ChallengeClass.Name, challenge);
			challengeJournal.Challenges.Add(challenge);
		}
		return challengeJournal;
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x00148A9C File Offset: 0x00146C9C
	public void Update(World world)
	{
		int num = GameUtils.WorldTimeToDays(world.worldTime);
		if (this.lastDay < num)
		{
			for (int i = 0; i < this.ChallengeGroups.Count; i++)
			{
				this.ChallengeGroups[i].Update(num, this.Player);
			}
			this.lastDay = num;
		}
		if (Time.time - this.lastUpdateTime >= 1f)
		{
			this.FireEvent(MinEventTypes.onSelfChallengeCompleteUpdate, this.Player.MinEventContext);
			this.lastUpdateTime = Time.time;
		}
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x00148B24 File Offset: 0x00146D24
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupData()
	{
		this.eventList.Clear();
		foreach (ChallengeClass challengeClass in ChallengeClass.s_Challenges.Values)
		{
			if (challengeClass.HasEventsOrPassives())
			{
				this.eventList.Add(challengeClass.Name, challengeClass);
			}
		}
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x00148B9C File Offset: 0x00146D9C
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.eventList == null)
		{
			return;
		}
		for (int i = 0; i < this.CompleteChallengesForMinEvents.Count; i++)
		{
			Challenge challenge = this.CompleteChallengesForMinEvents[i];
			ChallengeClass challengeClass = challenge.ChallengeClass;
			_params.Challenge = challenge;
			challengeClass.FireEvent(_eventType, _params);
		}
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x00148BEC File Offset: 0x00146DEC
	public void ModifyValue(PassiveEffects _effect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> _tags)
	{
		for (int i = 0; i < this.CompleteChallengesForMinEvents.Count; i++)
		{
			Challenge challenge = this.CompleteChallengesForMinEvents[i];
			if (challenge != null)
			{
				ChallengeClass challengeClass = challenge.ChallengeClass;
				if (challengeClass != null)
				{
					MinEffectController effects = challengeClass.Effects;
					if (effects != null)
					{
						HashSet<PassiveEffects> passivesIndex = effects.PassivesIndex;
						if (passivesIndex != null && passivesIndex.Contains(_effect))
						{
							challengeClass.ModifyValue(this.Player, _effect, ref _base_val, ref _perc_val, _tags);
						}
					}
				}
			}
		}
		for (int j = 0; j < this.CompleteChallengeGroupsForMinEvents.Count; j++)
		{
			ChallengeGroup challengeGroup = this.CompleteChallengeGroupsForMinEvents[j];
			if (challengeGroup != null)
			{
				MinEffectController effects2 = challengeGroup.Effects;
				if (effects2 != null)
				{
					HashSet<PassiveEffects> passivesIndex2 = effects2.PassivesIndex;
					if (passivesIndex2 != null && passivesIndex2.Contains(_effect))
					{
						challengeGroup.ModifyValue(this.Player, _effect, ref _base_val, ref _perc_val, _tags);
					}
				}
			}
		}
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x00148CC0 File Offset: 0x00146EC0
	public void StartChallenges(EntityPlayerLocal player)
	{
		if (this.Player == null)
		{
			this.Player = player;
		}
		if (this.Player == null)
		{
			return;
		}
		if (this.ChallengeGroups.Count == 0)
		{
			using (Dictionary<string, ChallengeGroup>.ValueCollection.Enumerator enumerator = ChallengeGroup.s_ChallengeGroups.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChallengeGroup challengeGroup = enumerator.Current;
					ChallengeGroupEntry challengeGroupEntry = new ChallengeGroupEntry(challengeGroup);
					this.ChallengeGroups.Add(challengeGroupEntry);
					challengeGroup.IsComplete = true;
					challengeGroupEntry.CreateChallenges(this.Player);
				}
				goto IL_00FE;
			}
		}
		int num = 0;
		foreach (ChallengeGroup challengeGroup2 in ChallengeGroup.s_ChallengeGroups.Values)
		{
			challengeGroup2.IsComplete = true;
			if (num < this.ChallengeGroups.Count)
			{
				ChallengeGroupEntry challengeGroupEntry2 = this.ChallengeGroups[num];
				if (challengeGroupEntry2.ChallengeGroup == challengeGroup2)
				{
					challengeGroupEntry2.AddAnyMissingChallenges(this.Player);
				}
			}
			num++;
		}
		IL_00FE:
		for (int i = 0; i < this.Challenges.Count; i++)
		{
			Challenge challenge = this.Challenges[i];
			if ((ChallengeJournal.IntroChallengesEnabled || !challenge.ChallengeClass.ChallengeGroup.IsIntro) && challenge.ChallengeClass.ChallengeGroup.IsActive)
			{
				challenge.StartChallenge();
				if (challenge.ChallengeState != Challenge.ChallengeStates.Redeemed)
				{
					challenge.ChallengeGroup.IsComplete = false;
				}
				if (challenge.IsTracked)
				{
					LocalPlayerUI.GetUIForPrimaryPlayer().xui.QuestTracker.TrackedChallenge = challenge;
				}
			}
		}
		foreach (ChallengeGroup challengeGroup3 in ChallengeGroup.s_ChallengeGroups.Values)
		{
			if (challengeGroup3.IsComplete)
			{
				this.CompleteChallengeGroupsForMinEvents.Add(challengeGroup3);
			}
		}
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x00148ED4 File Offset: 0x001470D4
	public void EndChallenges()
	{
		for (int i = 0; i < this.Challenges.Count; i++)
		{
			this.Challenges[i].EndChallenge(false);
		}
	}

	// Token: 0x06003296 RID: 12950 RVA: 0x00148F09 File Offset: 0x00147109
	public void AddChallenge(Challenge challenge)
	{
		this.ChallengeDictionary.Add(challenge.ChallengeClass.Name, challenge);
		this.Challenges.Add(challenge);
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x00148F30 File Offset: 0x00147130
	public void RemoveChallengesForGroup(ChallengeGroup challengeGroup)
	{
		for (int i = this.Challenges.Count - 1; i >= 0; i--)
		{
			Challenge challenge = this.Challenges[i];
			if (challenge.ChallengeGroup == challengeGroup)
			{
				challenge.EndChallenge(false);
				this.ChallengeDictionary.Remove(challenge.ChallengeClass.Name);
				this.Challenges.RemoveAt(i);
			}
		}
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x00148F98 File Offset: 0x00147198
	public void ResetChallenges()
	{
		if (!GameManager.Instance.World.IsEditor() && !GameUtils.IsWorldEditor() && !GameUtils.IsPlaytesting())
		{
			this.Player.Buffs.RemoveCustomVar("StarterQuest");
			this.EndChallenges();
			this.ChallengeDictionary.Clear();
			this.Challenges.Clear();
			this.ChallengeGroups.Clear();
			this.CompleteChallengesForMinEvents.Clear();
			this.CompleteChallengeGroupsForMinEvents.Clear();
			this.StartChallenges(this.Player);
		}
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x00149022 File Offset: 0x00147222
	public void HandleChallengeRedeemed(Challenge challenge)
	{
		if (this.eventList.ContainsKey(challenge.ChallengeClass.Name))
		{
			this.CompleteChallengesForMinEvents.Add(challenge);
		}
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x00149048 File Offset: 0x00147248
	[PublicizedFrom(EAccessModifier.Internal)]
	public void HandleChallengeGroupComplete(ChallengeGroup group)
	{
		for (int i = 0; i < this.Challenges.Count; i++)
		{
			Challenge challenge = this.Challenges[i];
			if (challenge.ChallengeGroup == group && challenge.ChallengeState != Challenge.ChallengeStates.Redeemed)
			{
				group.IsComplete = false;
				Manager.PlayInsidePlayerHead("ui_challenge_redeem", -1, 0f, false, false);
				return;
			}
		}
		if (group.RewardEvent != null)
		{
			GameEventManager.Current.HandleAction(group.RewardEvent, null, this.Player, false, "", "", false, true, "", null);
		}
		Manager.PlayInsidePlayerHead("ui_challenge_complete_row", -1, 0f, false, false);
		group.IsComplete = true;
		if (!this.CompleteChallengeGroupsForMinEvents.Contains(group))
		{
			this.CompleteChallengeGroupsForMinEvents.Add(group);
		}
		GameManager.Instance.StartCoroutine(this.unhideRowLater(group));
	}

	// Token: 0x0600329B RID: 12955 RVA: 0x0014911C File Offset: 0x0014731C
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator unhideRowLater(ChallengeGroup group)
	{
		yield return new WaitForSeconds(1f);
		bool flag = false;
		foreach (ChallengeGroup challengeGroup in ChallengeGroup.s_ChallengeGroups.Values)
		{
			if (challengeGroup.HiddenBy.EqualsCaseInsensitive(group.Name))
			{
				challengeGroup.UIDirty = true;
				flag = true;
			}
		}
		if (flag)
		{
			Manager.PlayInsidePlayerHead("ui_challenge_unhide_row", -1, 0f, false, false);
		}
		yield break;
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x0014912C File Offset: 0x0014732C
	[PublicizedFrom(EAccessModifier.Internal)]
	public Challenge GetNextChallenge(Challenge challenge)
	{
		Challenge challenge2 = null;
		string text = challenge.ChallengeGroup.ChallengeClasses[0].Name;
		if (text != "" && this.ChallengeDictionary.ContainsKey(text))
		{
			challenge2 = this.ChallengeDictionary[text];
		}
		while (challenge2 != null && !challenge2.IsActive)
		{
			text = challenge2.ChallengeClass.GetNextChallengeName();
			if (text != "" && this.ChallengeDictionary.ContainsKey(text))
			{
				challenge2 = this.ChallengeDictionary[text];
			}
			else
			{
				challenge2 = null;
			}
		}
		return challenge2;
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x001491C0 File Offset: 0x001473C0
	[PublicizedFrom(EAccessModifier.Internal)]
	public Challenge GetNextRedeemableChallenge(Challenge challenge)
	{
		Challenge challenge2 = null;
		string text = challenge.ChallengeGroup.ChallengeClasses[0].Name;
		if (text != "" && this.ChallengeDictionary.ContainsKey(text))
		{
			challenge2 = this.ChallengeDictionary[text];
		}
		while (challenge2 != null && !challenge2.ReadyToComplete)
		{
			text = challenge2.ChallengeClass.GetNextChallengeName();
			if (text != "" && this.ChallengeDictionary.ContainsKey(text))
			{
				challenge2 = this.ChallengeDictionary[text];
			}
			else
			{
				challenge2 = null;
			}
		}
		return challenge2;
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x00149254 File Offset: 0x00147454
	public bool HasCompletedChallenges()
	{
		for (int i = 0; i < this.Challenges.Count; i++)
		{
			if (this.Challenges[i].ReadyToComplete)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600329F RID: 12959 RVA: 0x00149290 File Offset: 0x00147490
	public void CompleteIntroChallenges()
	{
		for (int i = 0; i < this.Challenges.Count; i++)
		{
			Challenge challenge = this.Challenges[i];
			if (challenge.ChallengeState != Challenge.ChallengeStates.Redeemed && challenge.ChallengeGroup.IsIntro)
			{
				if (challenge.ChallengeState == Challenge.ChallengeStates.Active)
				{
					challenge.CompleteChallenge(true, false, true);
				}
				else if (challenge.ChallengeState == Challenge.ChallengeStates.Completed)
				{
					challenge.ChallengeState = Challenge.ChallengeStates.Redeemed;
				}
			}
		}
	}

	// Token: 0x040027EF RID: 10223
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCurrentSaveVersion = 2;

	// Token: 0x040027F0 RID: 10224
	public List<ChallengeGroupEntry> ChallengeGroups = new List<ChallengeGroupEntry>();

	// Token: 0x040027F1 RID: 10225
	public Dictionary<string, Challenge> ChallengeDictionary = new Dictionary<string, Challenge>();

	// Token: 0x040027F2 RID: 10226
	public List<Challenge> Challenges = new List<Challenge>();

	// Token: 0x040027F3 RID: 10227
	public List<Challenge> CompleteChallengesForMinEvents = new List<Challenge>();

	// Token: 0x040027F4 RID: 10228
	public List<ChallengeGroup> CompleteChallengeGroupsForMinEvents = new List<ChallengeGroup>();

	// Token: 0x040027F5 RID: 10229
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, ChallengeClass> eventList = new Dictionary<string, ChallengeClass>();

	// Token: 0x040027F6 RID: 10230
	public EntityPlayerLocal Player;

	// Token: 0x040027F7 RID: 10231
	public static bool AllowChallenges = true;

	// Token: 0x040027F8 RID: 10232
	public static bool IntroChallengesEnabled = true;

	// Token: 0x040027F9 RID: 10233
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDay;

	// Token: 0x040027FA RID: 10234
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastUpdateTime;
}
