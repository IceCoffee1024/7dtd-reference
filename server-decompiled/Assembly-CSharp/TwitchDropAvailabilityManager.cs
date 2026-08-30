using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Platform;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000C93 RID: 3219
public sealed class TwitchDropAvailabilityManager
{
	// Token: 0x17000A44 RID: 2628
	// (get) Token: 0x060062DA RID: 25306 RVA: 0x0026D7B5 File Offset: 0x0026B9B5
	public static TwitchDropAvailabilityManager Instance
	{
		get
		{
			TwitchDropAvailabilityManager twitchDropAvailabilityManager;
			if ((twitchDropAvailabilityManager = TwitchDropAvailabilityManager._instance) == null)
			{
				twitchDropAvailabilityManager = (TwitchDropAvailabilityManager._instance = new TwitchDropAvailabilityManager());
			}
			return twitchDropAvailabilityManager;
		}
	}

	// Token: 0x1400008B RID: 139
	// (add) Token: 0x060062DB RID: 25307 RVA: 0x0026D7CC File Offset: 0x0026B9CC
	// (remove) Token: 0x060062DC RID: 25308 RVA: 0x0026D804 File Offset: 0x0026BA04
	public event Action<TwitchDropAvailabilityManager> Updated;

	// Token: 0x060062DD RID: 25309 RVA: 0x0026D839 File Offset: 0x0026BA39
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchDropAvailabilityManager()
	{
	}

	// Token: 0x060062DE RID: 25310 RVA: 0x0026D84C File Offset: 0x0026BA4C
	public void RegisterSource(string uri)
	{
		if (this._sources.ContainsKey(uri))
		{
			return;
		}
		TwitchDropAvailabilityManager.DropSource dropSource = TwitchDropAvailabilityManager.DropSource.FromUri(this, uri);
		this._sources[uri] = dropSource;
		dropSource.RequestData(false);
	}

	// Token: 0x060062DF RID: 25311 RVA: 0x0026D884 File Offset: 0x0026BA84
	public void UpdateAll(bool force = false)
	{
		foreach (KeyValuePair<string, TwitchDropAvailabilityManager.DropSource> keyValuePair in this._sources)
		{
			keyValuePair.Value.RequestData(force);
		}
	}

	// Token: 0x060062E0 RID: 25312 RVA: 0x0026D8E0 File Offset: 0x0026BAE0
	public void GetEntries(List<string> sourceUris, List<TwitchDropAvailabilityManager.TwitchDropEntry> target)
	{
		target.Clear();
		foreach (string text in sourceUris)
		{
			TwitchDropAvailabilityManager.DropSource dropSource;
			if (this._sources.TryGetValue(text, out dropSource))
			{
				dropSource.GetData(target);
			}
		}
		target.Sort((TwitchDropAvailabilityManager.TwitchDropEntry a, TwitchDropAvailabilityManager.TwitchDropEntry b) => b.Start.CompareTo(a.Start));
	}

	// Token: 0x060062E1 RID: 25313 RVA: 0x0026D96C File Offset: 0x0026BB6C
	public TwitchDropAvailabilityManager.TwitchDropEntry GetLatestForBenefit(string benefitId, List<string> sourceUris)
	{
		List<TwitchDropAvailabilityManager.TwitchDropEntry> list = new List<TwitchDropAvailabilityManager.TwitchDropEntry>();
		this.GetEntries(sourceUris, list);
		for (int i = 0; i < list.Count; i++)
		{
			if (string.Equals(list[i].BenefitId, benefitId, StringComparison.OrdinalIgnoreCase))
			{
				return list[i];
			}
		}
		return null;
	}

	// Token: 0x060062E2 RID: 25314 RVA: 0x0026D9B6 File Offset: 0x0026BBB6
	[PublicizedFrom(EAccessModifier.Internal)]
	public void Notify()
	{
		Action<TwitchDropAvailabilityManager> updated = this.Updated;
		if (updated == null)
		{
			return;
		}
		updated(this);
	}

	// Token: 0x060062E3 RID: 25315 RVA: 0x0026D9CC File Offset: 0x0026BBCC
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<TwitchDropAvailabilityManager.TwitchDropEntry> ParseEntries(byte[] bytes)
	{
		List<TwitchDropAvailabilityManager.TwitchDropEntry> list = new List<TwitchDropAvailabilityManager.TwitchDropEntry>();
		if (bytes == null || bytes.Length == 0)
		{
			return list;
		}
		string @string = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		XDocument xdocument;
		try
		{
			xdocument = XDocument.Parse(@string, LoadOptions.SetLineInfo);
		}
		catch (Exception ex)
		{
			Log.Error("TwitchDrop XML parse failed:");
			Log.Exception(ex);
			return list;
		}
		XElement root = xdocument.Root;
		if (root == null)
		{
			return list;
		}
		if (!string.Equals(root.Name.LocalName, "drops", StringComparison.OrdinalIgnoreCase))
		{
			IXmlLineInfo xmlLineInfo = root;
			Log.Warning(string.Format("TwitchDrop XML unexpected root <{0}> at line {1}, expected <drops>", root.Name.LocalName, xmlLineInfo.LineNumber));
			return list;
		}
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (XElement xelement in root.Elements())
		{
			TwitchDropAvailabilityManager.TwitchDropEntry twitchDropEntry = TwitchDropAvailabilityManager.FromXmlBenefit(xelement);
			if (twitchDropEntry != null)
			{
				if (hashSet.Contains(twitchDropEntry.BenefitId))
				{
					IXmlLineInfo xmlLineInfo2 = xelement;
					Log.Warning(string.Format("TwitchDrop XML duplicate id '{0}' at line {1}; skipping duplicate.", twitchDropEntry.BenefitId, xmlLineInfo2.LineNumber));
				}
				else
				{
					hashSet.Add(twitchDropEntry.BenefitId);
					list.Add(twitchDropEntry);
				}
			}
		}
		return list;
	}

	// Token: 0x060062E4 RID: 25316 RVA: 0x0026DB20 File Offset: 0x0026BD20
	[PublicizedFrom(EAccessModifier.Private)]
	public static TwitchDropAvailabilityManager.TwitchDropEntry FromXmlBenefit(XElement element)
	{
		if (!string.Equals(element.Name.LocalName, "benefit", StringComparison.OrdinalIgnoreCase))
		{
			Log.Warning(string.Format("TwitchDrop XML unknown node <{0}> at line {1}", element.Name.LocalName, ((IXmlLineInfo)element).LineNumber));
			return null;
		}
		XAttribute xattribute = element.Attribute("id");
		XAttribute xattribute2 = element.Attribute("entitlementset");
		XAttribute xattribute3 = element.Attribute("start");
		if (xattribute == null || xattribute3 == null)
		{
			Log.Warning(string.Format("TwitchDrop XML missing required attributes on <benefit> at line {0} (need id,start)", ((IXmlLineInfo)element).LineNumber));
			return null;
		}
		string text = (xattribute.Value ?? string.Empty).Trim();
		if (text.Length == 0)
		{
			Log.Warning(string.Format("TwitchDrop XML empty id at line {0}", ((IXmlLineInfo)element).LineNumber));
			return null;
		}
		DateTime dateTime;
		if (!DateTime.TryParseExact(xattribute3.Value, "u", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out dateTime))
		{
			Log.Warning(string.Format("TwitchDrop XML invalid start '{0}' at line {1}", xattribute3.Value, ((IXmlLineInfo)element).LineNumber));
			return null;
		}
		dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
		EntitlementSetEnum entitlementSetEnum = EntitlementSetEnum.None;
		if (xattribute2 != null)
		{
			EntitlementSetEnum entitlementSetEnum2;
			if (!Enum.TryParse<EntitlementSetEnum>(xattribute2.Value.Trim(), true, out entitlementSetEnum2))
			{
				Log.Warning(string.Format("TwitchDrop XML invalid entitlementset '{0}' for id '{1}' at line {2}", xattribute2.Value, text, ((IXmlLineInfo)element).LineNumber));
				return null;
			}
			entitlementSetEnum = entitlementSetEnum2;
		}
		return new TwitchDropAvailabilityManager.TwitchDropEntry(text, dateTime, entitlementSetEnum);
	}

	// Token: 0x04004CD6 RID: 19670
	[PublicizedFrom(EAccessModifier.Private)]
	public static TwitchDropAvailabilityManager _instance;

	// Token: 0x04004CD7 RID: 19671
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, TwitchDropAvailabilityManager.DropSource> _sources = new CaseInsensitiveStringDictionary<TwitchDropAvailabilityManager.DropSource>();

	// Token: 0x02000C94 RID: 3220
	public sealed class TwitchDropEntry : IEquatable<TwitchDropAvailabilityManager.TwitchDropEntry>
	{
		// Token: 0x060062E5 RID: 25317 RVA: 0x0026DCA1 File Offset: 0x0026BEA1
		public TwitchDropEntry(string benefitId, DateTime startUtc, EntitlementSetEnum entitlementSet)
		{
			this.BenefitId = benefitId;
			this.Start = startUtc;
			this.EntitlementSet = entitlementSet;
		}

		// Token: 0x060062E6 RID: 25318 RVA: 0x0026DCBE File Offset: 0x0026BEBE
		public bool IsAvailable(DateTime localNow)
		{
			return localNow.ToUniversalTime() >= this.Start;
		}

		// Token: 0x060062E7 RID: 25319 RVA: 0x0026DCD4 File Offset: 0x0026BED4
		public bool Equals(TwitchDropAvailabilityManager.TwitchDropEntry other)
		{
			return other != null && (this == other || (string.Equals(this.BenefitId, other.BenefitId, StringComparison.OrdinalIgnoreCase) && this.Start.Equals(other.Start) && this.EntitlementSet == other.EntitlementSet));
		}

		// Token: 0x060062E8 RID: 25320 RVA: 0x0026DD28 File Offset: 0x0026BF28
		public override bool Equals(object obj)
		{
			TwitchDropAvailabilityManager.TwitchDropEntry twitchDropEntry = obj as TwitchDropAvailabilityManager.TwitchDropEntry;
			return twitchDropEntry != null && this.Equals(twitchDropEntry);
		}

		// Token: 0x060062E9 RID: 25321 RVA: 0x0026DD48 File Offset: 0x0026BF48
		public override int GetHashCode()
		{
			return (((StringComparer.OrdinalIgnoreCase.GetHashCode(this.BenefitId ?? string.Empty) * 397) ^ this.Start.GetHashCode()) * 397) ^ (int)this.EntitlementSet;
		}

		// Token: 0x04004CD9 RID: 19673
		public readonly string BenefitId;

		// Token: 0x04004CDA RID: 19674
		public readonly DateTime Start;

		// Token: 0x04004CDB RID: 19675
		public readonly EntitlementSetEnum EntitlementSet;
	}

	// Token: 0x02000C95 RID: 3221
	public abstract class DropSource
	{
		// Token: 0x060062EA RID: 25322 RVA: 0x0026DD90 File Offset: 0x0026BF90
		[PublicizedFrom(EAccessModifier.Protected)]
		public DropSource(TwitchDropAvailabilityManager owner, string uri)
		{
			this.Owner = owner;
			this.OrigUri = uri;
		}

		// Token: 0x060062EB RID: 25323 RVA: 0x0026DDB4 File Offset: 0x0026BFB4
		public void RequestData(bool force)
		{
			if (this._isUpdating)
			{
				return;
			}
			DateTime now = DateTime.Now;
			TwitchDropAvailabilityManager owner = this.Owner;
			lock (owner)
			{
				if (!force && this._entries.Count > 0 && (now - this._lastUpdated).TotalMinutes < 1.0)
				{
					return;
				}
			}
			this._lastUpdated = now;
			this._isUpdating = true;
			ThreadManager.StartCoroutine(this.GetDataCo());
		}

		// Token: 0x060062EC RID: 25324 RVA: 0x0026DE4C File Offset: 0x0026C04C
		public void GetData(List<TwitchDropAvailabilityManager.TwitchDropEntry> target)
		{
			TwitchDropAvailabilityManager owner = this.Owner;
			lock (owner)
			{
				target.AddRange(this._entries);
			}
		}

		// Token: 0x060062ED RID: 25325 RVA: 0x0026DE94 File Offset: 0x0026C094
		[PublicizedFrom(EAccessModifier.Protected)]
		public void ApplyBytes(byte[] bytes)
		{
			this._isUpdating = false;
			List<TwitchDropAvailabilityManager.TwitchDropEntry> list = TwitchDropAvailabilityManager.ParseEntries(bytes);
			TwitchDropAvailabilityManager owner = this.Owner;
			lock (owner)
			{
				this._entries.Clear();
				this._entries.AddRange(list);
			}
			this.Owner.Notify();
		}

		// Token: 0x060062EE RID: 25326
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator GetDataCo();

		// Token: 0x060062EF RID: 25327 RVA: 0x0026DF00 File Offset: 0x0026C100
		public static TwitchDropAvailabilityManager.DropSource FromUri(TwitchDropAvailabilityManager owner, string uri)
		{
			if (uri.StartsWith("rfs://", StringComparison.OrdinalIgnoreCase))
			{
				return new TwitchDropAvailabilityManager.DropSourceRfs(owner, uri);
			}
			return new TwitchDropAvailabilityManager.DropSourceWww(owner, uri);
		}

		// Token: 0x04004CDC RID: 19676
		[PublicizedFrom(EAccessModifier.Protected)]
		public const string RfsScheme = "rfs://";

		// Token: 0x04004CDD RID: 19677
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly TwitchDropAvailabilityManager Owner;

		// Token: 0x04004CDE RID: 19678
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string OrigUri;

		// Token: 0x04004CDF RID: 19679
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool _isUpdating;

		// Token: 0x04004CE0 RID: 19680
		[PublicizedFrom(EAccessModifier.Protected)]
		public DateTime _lastUpdated;

		// Token: 0x04004CE1 RID: 19681
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<TwitchDropAvailabilityManager.TwitchDropEntry> _entries = new List<TwitchDropAvailabilityManager.TwitchDropEntry>();
	}

	// Token: 0x02000C96 RID: 3222
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DropSourceRfs : TwitchDropAvailabilityManager.DropSource
	{
		// Token: 0x060062F0 RID: 25328 RVA: 0x0026DF1F File Offset: 0x0026C11F
		public DropSourceRfs(TwitchDropAvailabilityManager owner, string uri)
			: base(owner, uri)
		{
			this._rfsKey = uri.Substring("rfs://".Length);
		}

		// Token: 0x060062F1 RID: 25329 RVA: 0x0026DF3F File Offset: 0x0026C13F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator GetDataCo()
		{
			IRemoteFileStorage storage = PlatformManager.MultiPlatform.RemoteFileStorage;
			if (storage == null)
			{
				this._isUpdating = false;
				yield break;
			}
			if (PlatformManager.NativePlatform.User.UserStatus != EUserStatus.LoggedIn)
			{
				storage.GetCachedFile(this._rfsKey, new IRemoteFileStorage.FileDownloadCompleteCallback(this.<GetDataCo>g__Callback|2_0));
				this._lastUpdated = DateTime.MinValue;
				yield break;
			}
			bool warned = false;
			float startTime = Time.time;
			while (!storage.IsReady)
			{
				if (storage.Unavailable)
				{
					Log.Warning("Remote Storage is unavailable");
					this._isUpdating = false;
					yield break;
				}
				yield return null;
				if (!warned && Time.time > startTime + 30f)
				{
					warned = true;
					Log.Warning("Waiting for drop XML from remote storage exceeded 30s");
				}
			}
			storage.GetFile(this._rfsKey, new IRemoteFileStorage.FileDownloadCompleteCallback(this.<GetDataCo>g__Callback|2_0));
			yield break;
		}

		// Token: 0x060062F2 RID: 25330 RVA: 0x0026DF50 File Offset: 0x0026C150
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <GetDataCo>g__Callback|2_0(IRemoteFileStorage.EFileDownloadResult result, string error, byte[] data)
		{
			if (result != IRemoteFileStorage.EFileDownloadResult.Ok)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Retrieving drop XML '",
					this._rfsKey,
					"' failed: ",
					result.ToStringCached<IRemoteFileStorage.EFileDownloadResult>(),
					" (",
					error,
					")"
				}));
				base.ApplyBytes(null);
				return;
			}
			base.ApplyBytes(data);
		}

		// Token: 0x04004CE2 RID: 19682
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string _rfsKey;
	}

	// Token: 0x02000C98 RID: 3224
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DropSourceWww : TwitchDropAvailabilityManager.DropSource
	{
		// Token: 0x060062F9 RID: 25337 RVA: 0x0026E0F8 File Offset: 0x0026C2F8
		public DropSourceWww(TwitchDropAvailabilityManager owner, string uri)
			: base(owner, uri)
		{
			string text = uri;
			if (!text.StartsWith("http", StringComparison.Ordinal))
			{
				string text2 = ModManager.PatchModPathString(text);
				if (text2 == null)
				{
					throw new ArgumentException("Drop source '" + uri + "' cannot be retrieved: Not http(s) and not '@modfolder:'");
				}
				text = "file://" + text2;
			}
			text = text.Replace("#", "%23").Replace("+", "%2B");
			this._patchedUri = text;
		}

		// Token: 0x060062FA RID: 25338 RVA: 0x0026E170 File Offset: 0x0026C370
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator GetDataCo()
		{
			UnityWebRequest req = UnityWebRequest.Get(this._patchedUri);
			yield return req.SendWebRequest();
			if (req.result == UnityWebRequest.Result.Success)
			{
				string text = req.downloadHandler.text ?? string.Empty;
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				base.ApplyBytes(bytes);
			}
			else
			{
				Log.Warning("Retrieving drop XML from '" + this.OrigUri + "' failed: " + req.error);
				base.ApplyBytes(null);
			}
			yield break;
		}

		// Token: 0x04004CE9 RID: 19689
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string _patchedUri;
	}
}
