using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Challenges;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001861 RID: 6241
	public class TwitchViewerData
	{
		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x0600C0BA RID: 49338 RVA: 0x00474CC7 File Offset: 0x00472EC7
		// (set) Token: 0x0600C0BB RID: 49339 RVA: 0x00474CCF File Offset: 0x00472ECF
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

		// Token: 0x0600C0BC RID: 49340 RVA: 0x00474CE8 File Offset: 0x00472EE8
		public TwitchViewerData(TwitchManager owner)
		{
			this.Owner = owner;
		}

		// Token: 0x0600C0BD RID: 49341 RVA: 0x00474DA4 File Offset: 0x00472FA4
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

		// Token: 0x0600C0BE RID: 49342 RVA: 0x00474DC4 File Offset: 0x00472FC4
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

		// Token: 0x0600C0BF RID: 49343 RVA: 0x00474E7C File Offset: 0x0047307C
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

		// Token: 0x0600C0C0 RID: 49344 RVA: 0x00474E9C File Offset: 0x0047309C
		public void SetupLocalization()
		{
			this.chatOutput_AddPPAll = Localization.Get("TwitchChat_AddPPAll", false, null);
			this.chatOutput_AddSPAll = Localization.Get("TwitchChat_AddSPAll", false, null);
			this.chatOutput_ErrorAddingBitCredits = Localization.Get("TwitchChat_ErrorAddingBitCredit", false, null);
			this.chatOutput_ErrorAddingPoints = Localization.Get("TwitchChat_ErrorAddingPoints", false, null);
			this.chatOutput_GiftedSubs = Localization.Get("TwitchChat_GiftedSubs", false, null);
			this.ingameOutput_GiftedSubs = Localization.Get("TwitchInGame_GiftedSubs", false, null);
		}

		// Token: 0x0600C0C1 RID: 49345 RVA: 0x00474F18 File Offset: 0x00473118
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

		// Token: 0x0600C0C2 RID: 49346 RVA: 0x004750D4 File Offset: 0x004732D4
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

		// Token: 0x0600C0C3 RID: 49347 RVA: 0x00475138 File Offset: 0x00473338
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

		// Token: 0x0600C0C4 RID: 49348 RVA: 0x00475184 File Offset: 0x00473384
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

		// Token: 0x0600C0C5 RID: 49349 RVA: 0x00475324 File Offset: 0x00473524
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

		// Token: 0x0600C0C6 RID: 49350 RVA: 0x00475474 File Offset: 0x00473674
		public void Write(BinaryWriter bw)
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

		// Token: 0x0600C0C7 RID: 49351 RVA: 0x004755AC File Offset: 0x004737AC
		public void WriteSpecial(BinaryWriter bw)
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

		// Token: 0x0600C0C8 RID: 49352 RVA: 0x004756E4 File Offset: 0x004738E4
		public void WriteExport(string savePath)
		{
			using (StreamWriter streamWriter = SdFile.CreateText(savePath))
			{
				this.WriteExport(streamWriter);
			}
		}

		// Token: 0x0600C0C9 RID: 49353 RVA: 0x0047571C File Offset: 0x0047391C
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

		// Token: 0x0600C0CA RID: 49354 RVA: 0x004757E8 File Offset: 0x004739E8
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

		// Token: 0x0600C0CB RID: 49355 RVA: 0x004758A8 File Offset: 0x00473AA8
		public void Read(BinaryReader br, byte currentVersion)
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

		// Token: 0x0600C0CC RID: 49356 RVA: 0x0047592C File Offset: 0x00473B2C
		public void ReadSpecial(BinaryReader br, byte currentVersion)
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

		// Token: 0x0600C0CD RID: 49357 RVA: 0x004759BC File Offset: 0x00473BBC
		[PublicizedFrom(EAccessModifier.Private)]
		public void MoveStandardToSpecialPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].SpecialPoints += this.ViewerEntries[text].StandardPoints;
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C0CE RID: 49358 RVA: 0x00475A4C File Offset: 0x00473C4C
		public void ResetAllStandardPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C0CF RID: 49359 RVA: 0x00475AB4 File Offset: 0x00473CB4
		public void ResetAllSpecialPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C0D0 RID: 49360 RVA: 0x00475B1C File Offset: 0x00473D1C
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

		// Token: 0x0600C0D1 RID: 49361 RVA: 0x00475C2C File Offset: 0x00473E2C
		public void ResetAllPoints()
		{
			foreach (string text in this.ViewerEntries.Keys)
			{
				this.ViewerEntries[text].SpecialPoints = 0f;
				this.ViewerEntries[text].StandardPoints = 0f;
			}
		}

		// Token: 0x0600C0D2 RID: 49362 RVA: 0x00475CAC File Offset: 0x00473EAC
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

		// Token: 0x0600C0D3 RID: 49363 RVA: 0x00475D50 File Offset: 0x00473F50
		[PublicizedFrom(EAccessModifier.Private)]
		public void ClearDisplayViewers()
		{
			this.ViewerEntries.Clear();
		}

		// Token: 0x0600C0D4 RID: 49364 RVA: 0x00475D60 File Offset: 0x00473F60
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

		// Token: 0x0600C0D5 RID: 49365 RVA: 0x00475F04 File Offset: 0x00474104
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

		// Token: 0x0600C0D6 RID: 49366 RVA: 0x00475F60 File Offset: 0x00474160
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

		// Token: 0x0600C0D7 RID: 49367 RVA: 0x00476030 File Offset: 0x00474230
		public bool HasViewerEntry(string name)
		{
			return this.ViewerEntries.ContainsKey(name);
		}

		// Token: 0x0600C0D8 RID: 49368 RVA: 0x00476040 File Offset: 0x00474240
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

		// Token: 0x0600C0D9 RID: 49369 RVA: 0x004760A0 File Offset: 0x004742A0
		public bool RemoveViewerEntry(string name)
		{
			if (this.ViewerEntries.ContainsKey(name))
			{
				this.ViewerEntries.Remove(name);
				return true;
			}
			return false;
		}

		// Token: 0x0600C0DA RID: 49370 RVA: 0x004760C0 File Offset: 0x004742C0
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

		// Token: 0x0600C0DB RID: 49371 RVA: 0x00476124 File Offset: 0x00474324
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

		// Token: 0x0600C0DC RID: 49372 RVA: 0x00476228 File Offset: 0x00474428
		public bool HasPointsForAction(string username, TwitchAction action)
		{
			ViewerEntry viewerEntry = this.ViewerEntries[username];
			return (action.SpecialOnly && viewerEntry.SpecialPoints >= (float)action.CurrentCost) || (!action.SpecialOnly && viewerEntry.CombinedPoints >= (float)action.CurrentCost);
		}

		// Token: 0x0600C0DD RID: 49373 RVA: 0x00476273 File Offset: 0x00474473
		public bool HandleInitialActionEntrySetup(string username, TwitchAction action, bool isRerun, bool isBitAction, out TwitchActionEntry actionEntry)
		{
			return this.HandleInitialActionEntrySetup(username, action, isRerun, isBitAction, 0, out actionEntry);
		}

		// Token: 0x0600C0DE RID: 49374 RVA: 0x00476284 File Offset: 0x00474484
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

		// Token: 0x0600C0DF RID: 49375 RVA: 0x00476484 File Offset: 0x00474684
		[PublicizedFrom(EAccessModifier.Internal)]
		public void ReimburseAction(TwitchActionEntry twitchActionEntry)
		{
			ViewerEntry viewerEntry = this.ViewerEntries[twitchActionEntry.UserName];
			viewerEntry.StandardPoints += (float)twitchActionEntry.StandardPointsUsed;
			viewerEntry.SpecialPoints += (float)twitchActionEntry.SpecialPointsUsed;
			viewerEntry.BitCredits += twitchActionEntry.BitsUsed;
			this.Owner.PushBalanceToExtensionQueue(viewerEntry.UserID.ToString(), viewerEntry.BitCredits);
		}

		// Token: 0x0600C0E0 RID: 49376 RVA: 0x004764FC File Offset: 0x004746FC
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

		// Token: 0x04009156 RID: 37206
		public TwitchManager Owner;

		// Token: 0x04009157 RID: 37207
		public static float ChattingAddedTimeAmount = 300f;

		// Token: 0x04009158 RID: 37208
		[PublicizedFrom(EAccessModifier.Private)]
		public float pointRate = 1f;

		// Token: 0x04009159 RID: 37209
		public float PointRateSubs = 2f;

		// Token: 0x0400915A RID: 37210
		public float NextActionTime;

		// Token: 0x0400915B RID: 37211
		public int StartingPoints = 100;

		// Token: 0x0400915C RID: 37212
		public float NonSubPointCap = 1000f;

		// Token: 0x0400915D RID: 37213
		public float SubPointCap = 2000f;

		// Token: 0x0400915E RID: 37214
		public int SubPointAddTier1 = 500;

		// Token: 0x0400915F RID: 37215
		public int SubPointAddTier2 = 1000;

		// Token: 0x04009160 RID: 37216
		public int SubPointAddTier3 = 2500;

		// Token: 0x04009161 RID: 37217
		public int GiftSubPointAddTier1 = 500;

		// Token: 0x04009162 RID: 37218
		public int GiftSubPointAddTier2 = 1000;

		// Token: 0x04009163 RID: 37219
		public int GiftSubPointAddTier3 = 2500;

		// Token: 0x04009164 RID: 37220
		public float ActionSpamDelay = 3f;

		// Token: 0x04009165 RID: 37221
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GiftedSubs;

		// Token: 0x04009166 RID: 37222
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_AddPPAll;

		// Token: 0x04009167 RID: 37223
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_AddSPAll;

		// Token: 0x04009168 RID: 37224
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ErrorAddingBitCredits;

		// Token: 0x04009169 RID: 37225
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ErrorAddingPoints;

		// Token: 0x0400916A RID: 37226
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_GiftedSubs;

		// Token: 0x0400916B RID: 37227
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, ViewerEntry> ViewerEntries = new Dictionary<string, ViewerEntry>();

		// Token: 0x0400916C RID: 37228
		public Dictionary<int, string> IdToUsername = new Dictionary<int, string>();

		// Token: 0x0400916D RID: 37229
		public List<GiftSubEntry> SubEntries = new List<GiftSubEntry>();

		// Token: 0x0400916E RID: 37230
		public static char[] UsernameExcludeCharacters = new char[] { ';', '\\', ':' };
	}
}
