using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Challenges;
using UnityEngine;

namespace Twitch
{
	// Token: 0x020018BE RID: 6334
	public class TwitchViewerData
	{
		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x0600C326 RID: 49958 RVA: 0x0047CBAF File Offset: 0x0047ADAF
		// (set) Token: 0x0600C327 RID: 49959 RVA: 0x0047CBB7 File Offset: 0x0047ADB7
		public float PointRate
		{
			get
			{
				return this.pointRate;
			}
			set
			{
				this.pointRate = value;
				this.PointRateSubs = value * 2f;
			}
		}

		// Token: 0x0600C328 RID: 49960 RVA: 0x0047CBD0 File Offset: 0x0047ADD0
		public TwitchViewerData(TwitchManager owner)
		{
			this.Owner = owner;
		}

		// Token: 0x0600C329 RID: 49961 RVA: 0x0047CC8C File Offset: 0x0047AE8C
		public int GetSubTierPoints(TwitchSubEventEntry.SubTierTypes tier)
		{
			if (tier == TwitchSubEventEntry.SubTierTypes.Tier2)
			{
				return this.SubPointAddTier2;
			}
			if (tier != TwitchSubEventEntry.SubTierTypes.Tier3)
			{
				return this.SubPointAddTier1;
			}
			return this.SubPointAddTier3;
		}

		// Token: 0x0600C32A RID: 49962 RVA: 0x0047CCAC File Offset: 0x0047AEAC
		public string GetRandomActiveViewer()
		{
			string userName = this.Owner.Authentication.userName;
			List<string> list = new List<string>();
			foreach (string text in this.ViewerEntries.Keys)
			{
				if (this.ViewerEntries[text].IsActive && text != userName)
				{
					list.Add(text);
				}
			}
			if (list.Count > 0)
			{
				return list[GameEventManager.Current.Random.RandomRange(list.Count)];
			}
			return "";
		}

		// Token: 0x0600C32B RID: 49963 RVA: 0x0047CD64 File Offset: 0x0047AF64
		public int GetGiftSubTierPoints(TwitchSubEventEntry.SubTierTypes tier)
		{
			if (tier == TwitchSubEventEntry.SubTierTypes.Tier2)
			{
				return this.GiftSubPointAddTier2;
			}
			if (tier != TwitchSubEventEntry.SubTierTypes.Tier3)
			{
				return this.GiftSubPointAddTier1;
			}
			return this.GiftSubPointAddTier3;
		}

		// Token: 0x0600C32C RID: 49964 RVA: 0x0047CD84 File Offset: 0x0047AF84
		public void SetupLocalization()
		{
			this.chatOutput_AddPPAll = Localization.Get("TwitchChat_AddPPAll", false, null);
			this.chatOutput_AddSPAll = Localization.Get("TwitchChat_AddSPAll", false, null);
			this.chatOutput_ErrorAddingBitCredits = Localization.Get("TwitchChat_ErrorAddingBitCredit", false, null);
			this.chatOutput_ErrorAddingPoints = Localization.Get("TwitchChat_ErrorAddingPoints", false, null);
			this.chatOutput_GiftedSubs = Localization.Get("TwitchChat_GiftedSubs", false, null);
			this.ingameOutput_GiftedSubs = Localization.Get("TwitchInGame_GiftedSubs", false, null);
		}

		// Token: 0x0600C32D RID: 49965 RVA: 0x0047CE00 File Offset: 0x0047B000
		public void Update(float deltaTime)
		{
			this.NextActionTime -= deltaTime;
			if (this.NextActionTime <= 0f)
			{
				this.IncrementViewerEntries();
				this.NextActionTime = 10f;
			}
			for (int i = this.SubEntries.Count - 1; i >= 0; i--)
			{
				if (this.SubEntries[i].Update(deltaTime))
				{
					GiftSubEntry giftSubEntry = this.SubEntries[i];
					ViewerEntry viewerEntry = this.GetViewerEntry(giftSubEntry.UserName);
					viewerEntry.UserID = giftSubEntry.UserID;
					int num = this.GetGiftSubTierPoints(giftSubEntry.Tier) * giftSubEntry.SubCount * this.Owner.GiftSubPointModifier;
					if (num > 0)
					{
						viewerEntry.SpecialPoints += (float)num;
						this.Owner.ircClient.SendChannelMessage(string.Format(this.chatOutput_GiftedSubs, new object[]
						{
							giftSubEntry.UserName,
							viewerEntry.CombinedPoints,
							giftSubEntry.SubCount,
							this.Owner.GetTierName(giftSubEntry.Tier),
							num
						}), true);
						this.SubEntries.RemoveAt(i);
						string text = string.Format(this.ingameOutput_GiftedSubs, new object[]
						{
							giftSubEntry.UserName,
							giftSubEntry.SubCount,
							this.Owner.GetTierName(giftSubEntry.Tier),
							num
						});
						XUiC_ChatOutput.AddMessage(this.Owner.LocalPlayerXUi, EnumGameMessages.PlainTextLocal, text, EChatType.Global, EChatDirection.Inbound, -1, null, null, EMessageSender.Server, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
					}
					this.Owner.HandleGiftSubEvent(giftSubEntry.UserName, giftSubEntry.SubCount, giftSubEntry.Tier);
				}
			}
		}

		// Token: 0x0600C32E RID: 49966 RVA: 0x0047CFBC File Offset: 0x0047B1BC
		public void AddGiftSubEntry(string userName, int userID, TwitchSubEventEntry.SubTierTypes tier, int total)
		{
			for (int i = 0; i < this.SubEntries.Count; i++)
			{
				if (this.SubEntries[i].UserName == userName)
				{
					this.SubEntries[i].AddSub();
					return;
				}
			}
			this.SubEntries.Add(new GiftSubEntry(userName, userID, tier, total));
		}

		// Token: 0x0600C32F RID: 49967 RVA: 0x0047D020 File Offset: 0x0047B220
		public ViewerEntry AddCredit(string name, int credit, bool displayNewTotal)
		{
			ViewerEntry viewerEntry = this.AddToViewerEntry(name, credit, TwitchAction.PointTypes.Bits);
			if (viewerEntry == null)
			{
				this.Owner.ircClient.SendChannelMessage(string.Format(this.chatOutput_ErrorAddingBitCredits, name), true);
			}
			else if (displayNewTotal)
			{
				this.Owner.SendChannelCreditOutputMessage(name, viewerEntry);
			}
			return viewerEntry;
		}

		// Token: 0x0600C330 RID: 49968 RVA: 0x0047D06C File Offset: 0x0047B26C
		public void AddPoints(string name, int points, bool isSpecial, bool displayNewTotal)
		{
			if (name == "")
			{
				foreach (string text in this.ViewerEntries.Keys)
				{
					if (this.ViewerEntries[text].IsActive)
					{
						if (isSpecial)
						{
							this.ViewerEntries[text].SpecialPoints += (float)points;
							if (this.ViewerEntries[text].SpecialPoints < 0f)
							{
								this.ViewerEntries[text].SpecialPoints = 0f;
							}
						}
						else
						{
							this.ViewerEntries[text].StandardPoints += (float)points;
							if (this.ViewerEntries[text].StandardPoints < 0f)
							{
								this.ViewerEntries[text].StandardPoints = 0f;
							}
						}
					}
				}
				if (isSpecial)
				{
					this.Owner.ircClient.SendChannelMessage(string.Format(this.chatOutput_AddSPAll, points), true);
					return;
				}
				this.Owner.ircClient.SendChannelMessage(string.Format(this.chatOutput_AddPPAll, points), true);
				return;
			}
			else
			{
				ViewerEntry viewerEntry = this.AddToViewerEntry(name, points, isSpecial ? TwitchAction.PointTypes.SP : TwitchAction.PointTypes.PP);
				if (viewerEntry == null)
				{
					this.Owner.ircClient.SendChannelMessage(string.Format(this.chatOutput_ErrorAddingPoints, name), true);
					return;
				}
				if (displayNewTotal)
				{
					this.Owner.SendChannelPointOutputMessage(name, viewerEntry);
				}
				return;
			}
		}

		// Token: 0x0600C331 RID: 49969 RVA: 0x0047D20C File Offset: 0x0047B40C
		public void AddPointsAll(int standardPoints, int specialPoints, bool announceToChat = true)
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				if (this.ViewerEntries[text].IsActive)
				{
					if (standardPoints != 0)
					{
						this.ViewerEntries[text].StandardPoints += (float)standardPoints;
						if (this.ViewerEntries[text].StandardPoints < 0f)
						{
							this.ViewerEntries[text].StandardPoints = 0f;
						}
					}
					if (specialPoints != 0)
					{
						this.ViewerEntries[text].SpecialPoints += (float)specialPoints;
						if (this.ViewerEntries[text].SpecialPoints < 0f)
						{
							this.ViewerEntries[text].SpecialPoints = 0f;
						}
					}
				}
			}
			if (announceToChat)
			{
				if (standardPoints != 0)
				{
					this.Owner.ircClient.SendChannelMessage(string.Format(this.chatOutput_AddPPAll, standardPoints), true);
				}
				if (specialPoints != 0)
				{
					this.Owner.ircClient.SendChannelMessage(string.Format(this.chatOutput_AddSPAll, specialPoints), true);
				}
			}
		}

		// Token: 0x0600C332 RID: 49970 RVA: 0x0047D35C File Offset: 0x0047B55C
		public void Write(PooledBinaryWriter bw)
		{
			int num = 0;
			foreach (string text in this.ViewerEntries.Keys)
			{
				if (text.IndexOfAny(TwitchViewerData.UsernameExcludeCharacters) == -1 && (this.ViewerEntries[text].StandardPoints > 0f || this.ViewerEntries[text].SpecialPoints > 0f))
				{
					num++;
				}
			}
			bw.Write(num);
			foreach (string text2 in this.ViewerEntries.Keys)
			{
				if (text2.IndexOfAny(TwitchViewerData.UsernameExcludeCharacters) == -1)
				{
					ViewerEntry viewerEntry = this.ViewerEntries[text2];
					if (viewerEntry.StandardPoints > 0f || viewerEntry.SpecialPoints > 0f)
					{
						bw.Write(text2);
						bw.Write(viewerEntry.UserID);
						bw.Write(viewerEntry.StandardPoints);
					}
				}
			}
		}

		// Token: 0x0600C333 RID: 49971 RVA: 0x0047D494 File Offset: 0x0047B694
		public void WriteSpecial(PooledBinaryWriter bw)
		{
			int num = 0;
			foreach (string text in this.ViewerEntries.Keys)
			{
				ViewerEntry viewerEntry = this.ViewerEntries[text];
				if (text.IndexOfAny(TwitchViewerData.UsernameExcludeCharacters) == -1 && (viewerEntry.SpecialPoints > 0f || viewerEntry.BitCredits > 0))
				{
					num++;
				}
			}
			bw.Write(num);
			foreach (string text2 in this.ViewerEntries.Keys)
			{
				if (text2.IndexOfAny(TwitchViewerData.UsernameExcludeCharacters) == -1)
				{
					ViewerEntry viewerEntry2 = this.ViewerEntries[text2];
					if (viewerEntry2.SpecialPoints > 0f || viewerEntry2.BitCredits > 0)
					{
						bw.Write(text2);
						bw.Write(viewerEntry2.UserID);
						bw.Write(viewerEntry2.SpecialPoints);
						bw.Write(viewerEntry2.BitCredits);
					}
				}
			}
		}

		// Token: 0x0600C334 RID: 49972 RVA: 0x0047D5CC File Offset: 0x0047B7CC
		public void WriteExport(string savePath)
		{
			using (StreamWriter streamWriter = SdFile.CreateText(savePath))
			{
				this.WriteExport(streamWriter);
			}
		}

		// Token: 0x0600C335 RID: 49973 RVA: 0x0047D604 File Offset: 0x0047B804
		[PublicizedFrom(EAccessModifier.Private)]
		public void WriteExport(TextWriter tw)
		{
			tw.WriteLine("Name|UserID|PP|SP|Bit Credit");
			foreach (string text in this.ViewerEntries.Keys)
			{
				if (text.IndexOfAny(TwitchViewerData.UsernameExcludeCharacters) == -1)
				{
					ViewerEntry viewerEntry = this.ViewerEntries[text];
					tw.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4}", new object[] { text, viewerEntry.UserID, viewerEntry.StandardPoints, viewerEntry.SpecialPoints, viewerEntry.BitCredits }));
				}
			}
		}

		// Token: 0x0600C336 RID: 49974 RVA: 0x0047D6D0 File Offset: 0x0047B8D0
		public void LoadExport(TextReader tr)
		{
			tr.ReadLine();
			Dictionary<string, ViewerEntry> dictionary = new Dictionary<string, ViewerEntry>();
			while (tr.Peek() >= 0)
			{
				string[] array = tr.ReadLine().Split('|', StringSplitOptions.None);
				if (array.Length == 5)
				{
					ViewerEntry viewerEntry;
					if (this.ViewerEntries.ContainsKey(array[0]))
					{
						viewerEntry = this.ViewerEntries[array[0]];
					}
					else
					{
						viewerEntry = new ViewerEntry();
					}
					viewerEntry.StandardPoints = (float)StringParsers.ParseSInt32(array[2], 0, -1, NumberStyles.Integer);
					viewerEntry.SpecialPoints = (float)StringParsers.ParseSInt32(array[3], 0, -1, NumberStyles.Integer);
					viewerEntry.BitCredits = StringParsers.ParseSInt32(array[4], 0, -1, NumberStyles.Integer);
					dictionary.Add(array[0], viewerEntry);
				}
			}
			this.ViewerEntries.Clear();
			dictionary.CopyTo<string, ViewerEntry>(this.ViewerEntries, true);
		}

		// Token: 0x0600C337 RID: 49975 RVA: 0x0047D790 File Offset: 0x0047B990
		public void Read(PooledBinaryReader br, byte currentVersion)
		{
			this.ViewerEntries.Clear();
			int num = br.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = br.ReadString();
				int num2 = -1;
				if (currentVersion > 14)
				{
					num2 = br.ReadInt32();
				}
				float num3 = br.ReadSingle();
				if (text.IndexOfAny(TwitchViewerData.UsernameExcludeCharacters) == -1)
				{
					if (num2 != -1)
					{
						this.AddToIDLookup(num2, text, false);
					}
					this.ViewerEntries.Add(text, new ViewerEntry
					{
						UserID = num2,
						StandardPoints = num3
					});
				}
			}
		}

		// Token: 0x0600C338 RID: 49976 RVA: 0x0047D814 File Offset: 0x0047BA14
		public void ReadSpecial(PooledBinaryReader br, byte currentVersion)
		{
			int num = br.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = br.ReadString();
				int num2 = -1;
				if (currentVersion > 1)
				{
					num2 = br.ReadInt32();
				}
				float num3 = br.ReadSingle();
				int num4 = 0;
				if (currentVersion > 2)
				{
					num4 = br.ReadInt32();
				}
				if (text.IndexOfAny(TwitchViewerData.UsernameExcludeCharacters) == -1)
				{
					if (num2 != -1)
					{
						this.AddToIDLookup(num2, text, false);
					}
					ViewerEntry viewerEntry = this.GetViewerEntry(text);
					viewerEntry.UserID = num2;
					viewerEntry.SpecialPoints = num3;
					if (currentVersion > 2)
					{
						viewerEntry.BitCredits = num4;
					}
				}
			}
		}

		// Token: 0x0600C339 RID: 49977 RVA: 0x0047D8A4 File Offset: 0x0047BAA4
		[PublicizedFrom(EAccessModifier.Private)]
		public void MoveStandardToSpecialPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].SpecialPoints += this.ViewerEntries[text].StandardPoints;
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C33A RID: 49978 RVA: 0x0047D934 File Offset: 0x0047BB34
		public void ResetAllStandardPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C33B RID: 49979 RVA: 0x0047D99C File Offset: 0x0047BB9C
		public void ResetAllSpecialPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C33C RID: 49980 RVA: 0x0047DA04 File Offset: 0x0047BC04
		public void Cleanup()
		{
			List<string> list = new List<string>();
			foreach (string text in this.ViewerEntries.Keys)
			{
				string text2 = text.ToLower();
				if (text2 != text)
				{
					ViewerEntry viewerEntry = this.ViewerEntries[text];
					if (this.ViewerEntries.ContainsKey(text2))
					{
						ViewerEntry viewerEntry2 = this.ViewerEntries[text2];
						viewerEntry2.StandardPoints += viewerEntry.StandardPoints;
						viewerEntry2.SpecialPoints += viewerEntry.SpecialPoints;
						viewerEntry2.BitCredits += viewerEntry.BitCredits;
					}
					else
					{
						this.ViewerEntries.Add(text2, viewerEntry);
					}
					list.Add(text);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.ViewerEntries.Remove(list[i]);
			}
		}

		// Token: 0x0600C33D RID: 49981 RVA: 0x0047DB14 File Offset: 0x0047BD14
		public void ResetAllPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].SpecialPoints = 0f;
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C33E RID: 49982 RVA: 0x0047DB94 File Offset: 0x0047BD94
		public string GetPointTotals()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (string text in this.ViewerEntries.Keys)
			{
				ViewerEntry viewerEntry = this.ViewerEntries[text];
				num2 += (int)viewerEntry.SpecialPoints;
				num += (int)viewerEntry.StandardPoints;
				num3 += viewerEntry.BitCredits;
			}
			return string.Format("PP: {0} SP: {1} BC: {2}", num, num2, num3);
		}

		// Token: 0x0600C33F RID: 49983 RVA: 0x0047DC38 File Offset: 0x0047BE38
		[PublicizedFrom(EAccessModifier.Private)]
		public void ClearDisplayViewers()
		{
			this.ViewerEntries.Clear();
		}

		// Token: 0x0600C340 RID: 49984 RVA: 0x0047DC48 File Offset: 0x0047BE48
		[PublicizedFrom(EAccessModifier.Private)]
		public void IncrementViewerEntries()
		{
			float value = EffectManager.GetValue(PassiveEffects.TwitchViewerPointRate, null, this.PointRate, TwitchManager.Current.LocalPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			float num = value * 2f;
			float value2 = EffectManager.GetValue(PassiveEffects.TwitchViewerPointRate, null, this.NonSubPointCap, TwitchManager.Current.LocalPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			float value3 = EffectManager.GetValue(PassiveEffects.TwitchViewerPointRate, null, this.SubPointCap, TwitchManager.Current.LocalPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			bool allowPointGeneration = this.Owner.CurrentActionPreset.AllowPointGeneration;
			foreach (string text in this.ViewerEntries.Keys)
			{
				ViewerEntry viewerEntry = this.ViewerEntries[text];
				if (viewerEntry.IsActive)
				{
					this.Owner.HasDataChanges = true;
					if (allowPointGeneration)
					{
						if (viewerEntry.IsSub)
						{
							if (viewerEntry.StandardPoints < value3)
							{
								viewerEntry.StandardPoints += num;
								if (viewerEntry.StandardPoints > value3)
								{
									viewerEntry.StandardPoints = value3;
								}
							}
						}
						else if (viewerEntry.StandardPoints < value2)
						{
							viewerEntry.StandardPoints += value;
							if (viewerEntry.StandardPoints > value2)
							{
								viewerEntry.StandardPoints = value2;
							}
						}
					}
					if (viewerEntry.addPointsUntil < Time.time)
					{
						viewerEntry.IsActive = false;
					}
				}
			}
		}

		// Token: 0x0600C341 RID: 49985 RVA: 0x0047DDEC File Offset: 0x0047BFEC
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddToIDLookup(int viewerID, string viewerName, bool sendNewInChat = false)
		{
			if (this.IdToUsername.ContainsKey(viewerID))
			{
				this.IdToUsername[viewerID] = viewerName;
				return;
			}
			this.IdToUsername.Add(viewerID, viewerName);
			if (sendNewInChat && TwitchManager.Current.extensionManager != null)
			{
				TwitchManager.Current.extensionManager.PushViewerChatState(viewerID.ToString(), true);
			}
		}

		// Token: 0x0600C342 RID: 49986 RVA: 0x0047DE48 File Offset: 0x0047C048
		public ViewerEntry UpdateViewerEntry(int viewerID, string name, string color, bool isSub)
		{
			this.AddToIDLookup(viewerID, name, true);
			if (this.ViewerEntries.ContainsKey(name))
			{
				ViewerEntry viewerEntry = this.ViewerEntries[name];
				viewerEntry.UserColor = color;
				viewerEntry.UserID = viewerID;
				viewerEntry.addPointsUntil = Time.time + TwitchViewerData.ChattingAddedTimeAmount;
				if (!viewerEntry.IsActive)
				{
					this.Owner.PushBalanceToExtensionQueue(viewerID.ToString(), viewerEntry.BitCredits);
				}
				viewerEntry.IsActive = true;
				viewerEntry.IsSub = isSub;
				return viewerEntry;
			}
			ViewerEntry viewerEntry2 = new ViewerEntry
			{
				UserColor = color,
				UserID = viewerID,
				StandardPoints = (float)this.StartingPoints,
				addPointsUntil = Time.time + TwitchViewerData.ChattingAddedTimeAmount,
				IsActive = true,
				IsSub = isSub
			};
			this.ViewerEntries.Add(name, viewerEntry2);
			return viewerEntry2;
		}

		// Token: 0x0600C343 RID: 49987 RVA: 0x0047DF18 File Offset: 0x0047C118
		public bool HasViewerEntry(string name)
		{
			return this.ViewerEntries.ContainsKey(name);
		}

		// Token: 0x0600C344 RID: 49988 RVA: 0x0047DF28 File Offset: 0x0047C128
		public ViewerEntry GetViewerEntry(string name)
		{
			if (this.ViewerEntries.ContainsKey(name))
			{
				return this.ViewerEntries[name];
			}
			ViewerEntry viewerEntry = new ViewerEntry
			{
				StandardPoints = 0f,
				addPointsUntil = 0f,
				IsActive = false,
				IsSub = false
			};
			this.ViewerEntries.Add(name, viewerEntry);
			return viewerEntry;
		}

		// Token: 0x0600C345 RID: 49989 RVA: 0x0047DF88 File Offset: 0x0047C188
		public bool RemoveViewerEntry(string name)
		{
			if (this.ViewerEntries.ContainsKey(name))
			{
				this.ViewerEntries.Remove(name);
				return true;
			}
			return false;
		}

		// Token: 0x0600C346 RID: 49990 RVA: 0x0047DFA8 File Offset: 0x0047C1A8
		public ViewerEntry GetViewerEntry(string name, bool isSub)
		{
			if (this.ViewerEntries.ContainsKey(name))
			{
				return this.ViewerEntries[name];
			}
			ViewerEntry viewerEntry = new ViewerEntry
			{
				StandardPoints = (float)this.StartingPoints,
				addPointsUntil = 0f,
				IsActive = true,
				IsSub = isSub
			};
			this.ViewerEntries.Add(name, viewerEntry);
			return viewerEntry;
		}

		// Token: 0x0600C347 RID: 49991 RVA: 0x0047E00C File Offset: 0x0047C20C
		[PublicizedFrom(EAccessModifier.Private)]
		public ViewerEntry AddToViewerEntry(string name, int points, TwitchAction.PointTypes pointType)
		{
			if (name.StartsWith("@"))
			{
				name = name.Substring(1).ToLower();
			}
			else
			{
				name = name.ToLower();
			}
			if (this.ViewerEntries.ContainsKey(name))
			{
				ViewerEntry viewerEntry = this.ViewerEntries[name];
				switch (pointType)
				{
				case TwitchAction.PointTypes.PP:
					viewerEntry.StandardPoints += (float)points;
					if (viewerEntry.StandardPoints < 0f)
					{
						viewerEntry.StandardPoints = 0f;
					}
					break;
				case TwitchAction.PointTypes.SP:
					viewerEntry.SpecialPoints += (float)points;
					if (viewerEntry.SpecialPoints < 0f)
					{
						viewerEntry.SpecialPoints = 0f;
					}
					break;
				case TwitchAction.PointTypes.Bits:
					viewerEntry.BitCredits += points;
					if (viewerEntry.BitCredits < 0)
					{
						viewerEntry.BitCredits = 0;
					}
					this.Owner.PushBalanceToExtensionQueue(viewerEntry.UserID.ToString(), viewerEntry.BitCredits);
					break;
				}
				return this.ViewerEntries[name];
			}
			return null;
		}

		// Token: 0x0600C348 RID: 49992 RVA: 0x0047E110 File Offset: 0x0047C310
		public bool HasPointsForAction(string username, TwitchAction action)
		{
			ViewerEntry viewerEntry = this.ViewerEntries[username];
			return (action.SpecialOnly && viewerEntry.SpecialPoints >= (float)action.CurrentCost) || (!action.SpecialOnly && viewerEntry.CombinedPoints >= (float)action.CurrentCost);
		}

		// Token: 0x0600C349 RID: 49993 RVA: 0x0047E15B File Offset: 0x0047C35B
		public bool HandleInitialActionEntrySetup(string username, TwitchAction action, bool isRerun, bool isBitAction, out TwitchActionEntry actionEntry)
		{
			return this.HandleInitialActionEntrySetup(username, action, isRerun, isBitAction, 0, out actionEntry);
		}

		// Token: 0x0600C34A RID: 49994 RVA: 0x0047E16C File Offset: 0x0047C36C
		public bool HandleInitialActionEntrySetup(string username, TwitchAction action, bool isRerun, bool isBitAction, int bitsUsed, out TwitchActionEntry actionEntry)
		{
			ViewerEntry viewerEntry = this.ViewerEntries[username];
			bool flag = isRerun || isBitAction;
			if ((flag || viewerEntry.LastAction == -1f || this.ActionSpamDelay == 0f || Time.time - viewerEntry.LastAction > this.ActionSpamDelay) && (flag || (action.SpecialOnly && viewerEntry.SpecialPoints >= (float)action.CurrentCost) || (!action.SpecialOnly && viewerEntry.CombinedPoints >= (float)action.CurrentCost)))
			{
				if (!isRerun && isBitAction && action.PointType == TwitchAction.PointTypes.Bits)
				{
					int currentCost = action.CurrentCost;
					int num = ((ExtensionManager.Version == "2.0.1") ? viewerEntry.BitCredits : TwitchAction.GetAdjustedBitPriceFloor(viewerEntry.BitCredits));
					int num2 = Utils.FastMin(currentCost, num);
					int num3 = Utils.FastMax(0, currentCost - num2);
					if (bitsUsed < num3)
					{
						actionEntry = null;
						return false;
					}
				}
				actionEntry = action.SetupActionEntry();
				actionEntry.UserName = username;
				if (!isRerun)
				{
					viewerEntry.RemovePoints((float)action.CurrentCost, action.PointType, actionEntry);
					if (username != this.Owner.Authentication.userName)
					{
						TwitchLeaderboardStats leaderboardStats = TwitchManager.LeaderboardStats;
						int num4 = ((action.PointType == TwitchAction.PointTypes.Bits) ? 2 : 1);
						if (action.IsPositive)
						{
							leaderboardStats.TotalGood += num4;
							leaderboardStats.CheckTopGood(leaderboardStats.AddGoodActionUsed(username, viewerEntry.UserColor, action.PointType == TwitchAction.PointTypes.Bits));
							QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.GoodAction, action.DisplayCategory.Name);
						}
						else
						{
							leaderboardStats.TotalBad += num4;
							leaderboardStats.CheckTopBad(leaderboardStats.AddBadActionUsed(username, viewerEntry.UserColor, action.PointType == TwitchAction.PointTypes.Bits));
							QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.BadAction, action.DisplayCategory.Name);
						}
						leaderboardStats.TotalActions += num4;
					}
				}
				viewerEntry.LastAction = Time.time;
				return true;
			}
			actionEntry = null;
			return false;
		}

		// Token: 0x0600C34B RID: 49995 RVA: 0x0047E36C File Offset: 0x0047C56C
		[PublicizedFrom(EAccessModifier.Internal)]
		public void ReimburseAction(TwitchActionEntry twitchActionEntry)
		{
			ViewerEntry viewerEntry = this.ViewerEntries[twitchActionEntry.UserName];
			viewerEntry.StandardPoints += (float)twitchActionEntry.StandardPointsUsed;
			viewerEntry.SpecialPoints += (float)twitchActionEntry.SpecialPointsUsed;
			viewerEntry.BitCredits += twitchActionEntry.BitsUsed;
			this.Owner.PushBalanceToExtensionQueue(viewerEntry.UserID.ToString(), viewerEntry.BitCredits);
		}

		// Token: 0x0600C34C RID: 49996 RVA: 0x0047E3E4 File Offset: 0x0047C5E4
		public void ReimburseAction(string userName, int pointsSpent, TwitchAction action)
		{
			ViewerEntry viewerEntry = this.ViewerEntries[userName];
			TwitchAction.PointTypes pointType = action.PointType;
			if (pointType <= TwitchAction.PointTypes.SP)
			{
				viewerEntry.SpecialPoints += (float)pointsSpent;
				return;
			}
			if (pointType != TwitchAction.PointTypes.Bits)
			{
				return;
			}
			viewerEntry.BitCredits += pointsSpent;
			this.Owner.PushBalanceToExtensionQueue(viewerEntry.UserID.ToString(), viewerEntry.BitCredits);
		}

		// Token: 0x04009305 RID: 37637
		public TwitchManager Owner;

		// Token: 0x04009306 RID: 37638
		public static float ChattingAddedTimeAmount = 300f;

		// Token: 0x04009307 RID: 37639
		[PublicizedFrom(EAccessModifier.Private)]
		public float pointRate = 1f;

		// Token: 0x04009308 RID: 37640
		public float PointRateSubs = 2f;

		// Token: 0x04009309 RID: 37641
		public float NextActionTime;

		// Token: 0x0400930A RID: 37642
		public int StartingPoints = 100;

		// Token: 0x0400930B RID: 37643
		public float NonSubPointCap = 1000f;

		// Token: 0x0400930C RID: 37644
		public float SubPointCap = 2000f;

		// Token: 0x0400930D RID: 37645
		public int SubPointAddTier1 = 500;

		// Token: 0x0400930E RID: 37646
		public int SubPointAddTier2 = 1000;

		// Token: 0x0400930F RID: 37647
		public int SubPointAddTier3 = 2500;

		// Token: 0x04009310 RID: 37648
		public int GiftSubPointAddTier1 = 500;

		// Token: 0x04009311 RID: 37649
		public int GiftSubPointAddTier2 = 1000;

		// Token: 0x04009312 RID: 37650
		public int GiftSubPointAddTier3 = 2500;

		// Token: 0x04009313 RID: 37651
		public float ActionSpamDelay = 3f;

		// Token: 0x04009314 RID: 37652
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GiftedSubs;

		// Token: 0x04009315 RID: 37653
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_AddPPAll;

		// Token: 0x04009316 RID: 37654
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_AddSPAll;

		// Token: 0x04009317 RID: 37655
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ErrorAddingBitCredits;

		// Token: 0x04009318 RID: 37656
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ErrorAddingPoints;

		// Token: 0x04009319 RID: 37657
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_GiftedSubs;

		// Token: 0x0400931A RID: 37658
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, ViewerEntry> ViewerEntries = new Dictionary<string, ViewerEntry>();

		// Token: 0x0400931B RID: 37659
		public Dictionary<int, string> IdToUsername = new Dictionary<int, string>();

		// Token: 0x0400931C RID: 37660
		public List<GiftSubEntry> SubEntries = new List<GiftSubEntry>();

		// Token: 0x0400931D RID: 37661
		public static char[] UsernameExcludeCharacters = new char[] { ';', '\\', ':' };
	}
}
