using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Challenges;
using UnityEngine;

namespace Twitch
{
	// Token: 0x0200188D RID: 6285
	public class TwitchViewerData
	{
		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x0600C212 RID: 49682 RVA: 0x00479203 File Offset: 0x00477403
		// (set) Token: 0x0600C213 RID: 49683 RVA: 0x0047920B File Offset: 0x0047740B
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

		// Token: 0x0600C214 RID: 49684 RVA: 0x00479224 File Offset: 0x00477424
		public TwitchViewerData(TwitchManager owner)
		{
			this.Owner = owner;
		}

		// Token: 0x0600C215 RID: 49685 RVA: 0x004792E0 File Offset: 0x004774E0
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

		// Token: 0x0600C216 RID: 49686 RVA: 0x00479300 File Offset: 0x00477500
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

		// Token: 0x0600C217 RID: 49687 RVA: 0x004793B8 File Offset: 0x004775B8
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

		// Token: 0x0600C218 RID: 49688 RVA: 0x004793D8 File Offset: 0x004775D8
		public void SetupLocalization()
		{
			this.chatOutput_AddPPAll = Localization.Get("TwitchChat_AddPPAll", false, null);
			this.chatOutput_AddSPAll = Localization.Get("TwitchChat_AddSPAll", false, null);
			this.chatOutput_ErrorAddingBitCredits = Localization.Get("TwitchChat_ErrorAddingBitCredit", false, null);
			this.chatOutput_ErrorAddingPoints = Localization.Get("TwitchChat_ErrorAddingPoints", false, null);
			this.chatOutput_GiftedSubs = Localization.Get("TwitchChat_GiftedSubs", false, null);
			this.ingameOutput_GiftedSubs = Localization.Get("TwitchInGame_GiftedSubs", false, null);
		}

		// Token: 0x0600C219 RID: 49689 RVA: 0x00479454 File Offset: 0x00477654
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

		// Token: 0x0600C21A RID: 49690 RVA: 0x00479610 File Offset: 0x00477810
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

		// Token: 0x0600C21B RID: 49691 RVA: 0x00479674 File Offset: 0x00477874
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

		// Token: 0x0600C21C RID: 49692 RVA: 0x004796C0 File Offset: 0x004778C0
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

		// Token: 0x0600C21D RID: 49693 RVA: 0x00479860 File Offset: 0x00477A60
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

		// Token: 0x0600C21E RID: 49694 RVA: 0x004799B0 File Offset: 0x00477BB0
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

		// Token: 0x0600C21F RID: 49695 RVA: 0x00479AE8 File Offset: 0x00477CE8
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

		// Token: 0x0600C220 RID: 49696 RVA: 0x00479C20 File Offset: 0x00477E20
		public void WriteExport(string savePath)
		{
			using (StreamWriter streamWriter = SdFile.CreateText(savePath))
			{
				this.WriteExport(streamWriter);
			}
		}

		// Token: 0x0600C221 RID: 49697 RVA: 0x00479C58 File Offset: 0x00477E58
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

		// Token: 0x0600C222 RID: 49698 RVA: 0x00479D24 File Offset: 0x00477F24
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

		// Token: 0x0600C223 RID: 49699 RVA: 0x00479DE4 File Offset: 0x00477FE4
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

		// Token: 0x0600C224 RID: 49700 RVA: 0x00479E68 File Offset: 0x00478068
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

		// Token: 0x0600C225 RID: 49701 RVA: 0x00479EF8 File Offset: 0x004780F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void MoveStandardToSpecialPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].SpecialPoints += this.ViewerEntries[text].StandardPoints;
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C226 RID: 49702 RVA: 0x00479F88 File Offset: 0x00478188
		public void ResetAllStandardPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C227 RID: 49703 RVA: 0x00479FF0 File Offset: 0x004781F0
		public void ResetAllSpecialPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C228 RID: 49704 RVA: 0x0047A058 File Offset: 0x00478258
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

		// Token: 0x0600C229 RID: 49705 RVA: 0x0047A168 File Offset: 0x00478368
		public void ResetAllPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].SpecialPoints = 0f;
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C22A RID: 49706 RVA: 0x0047A1E8 File Offset: 0x004783E8
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

		// Token: 0x0600C22B RID: 49707 RVA: 0x0047A28C File Offset: 0x0047848C
		[PublicizedFrom(EAccessModifier.Private)]
		public void ClearDisplayViewers()
		{
			this.ViewerEntries.Clear();
		}

		// Token: 0x0600C22C RID: 49708 RVA: 0x0047A29C File Offset: 0x0047849C
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

		// Token: 0x0600C22D RID: 49709 RVA: 0x0047A440 File Offset: 0x00478640
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

		// Token: 0x0600C22E RID: 49710 RVA: 0x0047A49C File Offset: 0x0047869C
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

		// Token: 0x0600C22F RID: 49711 RVA: 0x0047A56C File Offset: 0x0047876C
		public bool HasViewerEntry(string name)
		{
			return this.ViewerEntries.ContainsKey(name);
		}

		// Token: 0x0600C230 RID: 49712 RVA: 0x0047A57C File Offset: 0x0047877C
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

		// Token: 0x0600C231 RID: 49713 RVA: 0x0047A5DC File Offset: 0x004787DC
		public bool RemoveViewerEntry(string name)
		{
			if (this.ViewerEntries.ContainsKey(name))
			{
				this.ViewerEntries.Remove(name);
				return true;
			}
			return false;
		}

		// Token: 0x0600C232 RID: 49714 RVA: 0x0047A5FC File Offset: 0x004787FC
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

		// Token: 0x0600C233 RID: 49715 RVA: 0x0047A660 File Offset: 0x00478860
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

		// Token: 0x0600C234 RID: 49716 RVA: 0x0047A764 File Offset: 0x00478964
		public bool HasPointsForAction(string username, TwitchAction action)
		{
			ViewerEntry viewerEntry = this.ViewerEntries[username];
			return (action.SpecialOnly && viewerEntry.SpecialPoints >= (float)action.CurrentCost) || (!action.SpecialOnly && viewerEntry.CombinedPoints >= (float)action.CurrentCost);
		}

		// Token: 0x0600C235 RID: 49717 RVA: 0x0047A7AF File Offset: 0x004789AF
		public bool HandleInitialActionEntrySetup(string username, TwitchAction action, bool isRerun, bool isBitAction, out TwitchActionEntry actionEntry)
		{
			return this.HandleInitialActionEntrySetup(username, action, isRerun, isBitAction, 0, out actionEntry);
		}

		// Token: 0x0600C236 RID: 49718 RVA: 0x0047A7C0 File Offset: 0x004789C0
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

		// Token: 0x0600C237 RID: 49719 RVA: 0x0047A9C0 File Offset: 0x00478BC0
		[PublicizedFrom(EAccessModifier.Internal)]
		public void ReimburseAction(TwitchActionEntry twitchActionEntry)
		{
			ViewerEntry viewerEntry = this.ViewerEntries[twitchActionEntry.UserName];
			viewerEntry.StandardPoints += (float)twitchActionEntry.StandardPointsUsed;
			viewerEntry.SpecialPoints += (float)twitchActionEntry.SpecialPointsUsed;
			viewerEntry.BitCredits += twitchActionEntry.BitsUsed;
			this.Owner.PushBalanceToExtensionQueue(viewerEntry.UserID.ToString(), viewerEntry.BitCredits);
		}

		// Token: 0x0600C238 RID: 49720 RVA: 0x0047AA38 File Offset: 0x00478C38
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

		// Token: 0x0400921C RID: 37404
		public TwitchManager Owner;

		// Token: 0x0400921D RID: 37405
		public static float ChattingAddedTimeAmount = 300f;

		// Token: 0x0400921E RID: 37406
		[PublicizedFrom(EAccessModifier.Private)]
		public float pointRate = 1f;

		// Token: 0x0400921F RID: 37407
		public float PointRateSubs = 2f;

		// Token: 0x04009220 RID: 37408
		public float NextActionTime;

		// Token: 0x04009221 RID: 37409
		public int StartingPoints = 100;

		// Token: 0x04009222 RID: 37410
		public float NonSubPointCap = 1000f;

		// Token: 0x04009223 RID: 37411
		public float SubPointCap = 2000f;

		// Token: 0x04009224 RID: 37412
		public int SubPointAddTier1 = 500;

		// Token: 0x04009225 RID: 37413
		public int SubPointAddTier2 = 1000;

		// Token: 0x04009226 RID: 37414
		public int SubPointAddTier3 = 2500;

		// Token: 0x04009227 RID: 37415
		public int GiftSubPointAddTier1 = 500;

		// Token: 0x04009228 RID: 37416
		public int GiftSubPointAddTier2 = 1000;

		// Token: 0x04009229 RID: 37417
		public int GiftSubPointAddTier3 = 2500;

		// Token: 0x0400922A RID: 37418
		public float ActionSpamDelay = 3f;

		// Token: 0x0400922B RID: 37419
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GiftedSubs;

		// Token: 0x0400922C RID: 37420
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_AddPPAll;

		// Token: 0x0400922D RID: 37421
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_AddSPAll;

		// Token: 0x0400922E RID: 37422
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ErrorAddingBitCredits;

		// Token: 0x0400922F RID: 37423
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ErrorAddingPoints;

		// Token: 0x04009230 RID: 37424
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_GiftedSubs;

		// Token: 0x04009231 RID: 37425
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, ViewerEntry> ViewerEntries = new Dictionary<string, ViewerEntry>();

		// Token: 0x04009232 RID: 37426
		public Dictionary<int, string> IdToUsername = new Dictionary<int, string>();

		// Token: 0x04009233 RID: 37427
		public List<GiftSubEntry> SubEntries = new List<GiftSubEntry>();

		// Token: 0x04009234 RID: 37428
		public static char[] UsernameExcludeCharacters = new char[] { ';', '\\', ':' };
	}
}
