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

// Token: 0x02000C81 RID: 3201
public sealed class TwitchDropAvailabilityManager
{
	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x0600624F RID: 25167 RVA: 0x0026A8DD File Offset: 0x00268ADD
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
	// (add) Token: 0x06006250 RID: 25168 RVA: 0x0026A8F4 File Offset: 0x00268AF4
	// (remove) Token: 0x06006251 RID: 25169 RVA: 0x0026A92C File Offset: 0x00268B2C
	public event Action<TwitchDropAvailabilityManager> Updated;

	// Token: 0x06006252 RID: 25170 RVA: 0x0026A961 File Offset: 0x00268B61
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchDropAvailabilityManager()
	{
	}

	// Token: 0x06006253 RID: 25171 RVA: 0x0026A974 File Offset: 0x00268B74
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

	// Token: 0x06006254 RID: 25172 RVA: 0x0026A9AC File Offset: 0x00268BAC
	public void UpdateAll(bool force = false)
	{
		foreach (KeyValuePair<string, TwitchDropAvailabilityManager.DropSource> keyValuePair in this._sources)
		{
			keyValuePair.Value.RequestData(force);
		}
	}

	// Token: 0x06006255 RID: 25173 RVA: 0x0026AA08 File Offset: 0x00268C08
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

	// Token: 0x06006256 RID: 25174 RVA: 0x0026AA94 File Offset: 0x00268C94
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

	// Token: 0x06006257 RID: 25175 RVA: 0x0026AADE File Offset: 0x00268CDE
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

	// Token: 0x06006258 RID: 25176 RVA: 0x0026AAF4 File Offset: 0x00268CF4
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

	// Token: 0x06006259 RID: 25177 RVA: 0x0026AC48 File Offset: 0x00268E48
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

	// Token: 0x04004C39 RID: 19513
	[PublicizedFrom(EAccessModifier.Private)]
	public static TwitchDropAvailabilityManager _instance;

	// Token: 0x04004C3A RID: 19514
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, TwitchDropAvailabilityManager.DropSource> _sources = new CaseInsensitiveStringDictionary<TwitchDropAvailabilityManager.DropSource>();

	// Token: 0x02000C82 RID: 3202
	public sealed class TwitchDropEntry : IEquatable<TwitchDropAvailabilityManager.TwitchDropEntry>
	{
		// Token: 0x0600625A RID: 25178 RVA: 0x0026ADC9 File Offset: 0x00268FC9
		public TwitchDropEntry(string benefitId, DateTime startUtc, EntitlementSetEnum entitlementSet)
		{
			this.BenefitId = benefitId;
			this.Start = startUtc;
			this.EntitlementSet = entitlementSet;
		}

		// Token: 0x0600625B RID: 25179 RVA: 0x0026ADE6 File Offset: 0x00268FE6
		public bool IsAvailable(DateTime localNow)
		{
			return localNow.ToUniversalTime() >= this.Start;
		}

		// Token: 0x0600625C RID: 25180 RVA: 0x0026ADFC File Offset: 0x00268FFC
		public bool Equals(TwitchDropAvailabilityManager.TwitchDropEntry other)
		{
			return other != null && (this == other || (string.Equals(this.BenefitId, other.BenefitId, StringComparison.OrdinalIgnoreCase) && this.Start.Equals(other.Start) && this.EntitlementSet == other.EntitlementSet));
		}

		// Token: 0x0600625D RID: 25181 RVA: 0x0026AE50 File Offset: 0x00269050
		public override bool Equals(object obj)
		{
			TwitchDropAvailabilityManager.TwitchDropEntry twitchDropEntry = obj as TwitchDropAvailabilityManager.TwitchDropEntry;
			return twitchDropEntry != null && this.Equals(twitchDropEntry);
		}

		// Token: 0x0600625E RID: 25182 RVA: 0x0026AE70 File Offset: 0x00269070
		public override int GetHashCode()
		{
			return (((StringComparer.OrdinalIgnoreCase.GetHashCode(this.BenefitId ?? string.Empty) * 397) ^ this.Start.GetHashCode()) * 397) ^ (int)this.EntitlementSet;
		}

		// Token: 0x04004C3C RID: 19516
		public readonly string BenefitId;

		// Token: 0x04004C3D RID: 19517
		public readonly DateTime Start;

		// Token: 0x04004C3E RID: 19518
		public readonly EntitlementSetEnum EntitlementSet;
	}

	// Token: 0x02000C83 RID: 3203
	public abstract class DropSource
	{
		// Token: 0x0600625F RID: 25183 RVA: 0x0026AEB8 File Offset: 0x002690B8
		[PublicizedFrom(EAccessModifier.Protected)]
		public DropSource(TwitchDropAvailabilityManager owner, string uri)
		{
			this.Owner = owner;
			this.OrigUri = uri;
		}

		// Token: 0x06006260 RID: 25184 RVA: 0x0026AEDC File Offset: 0x002690DC
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

		// Token: 0x06006261 RID: 25185 RVA: 0x0026AF74 File Offset: 0x00269174
		public void GetData(List<TwitchDropAvailabilityManager.TwitchDropEntry> target)
		{
			TwitchDropAvailabilityManager owner = this.Owner;
			lock (owner)
			{
				target.AddRange(this._entries);
			}
		}

		// Token: 0x06006262 RID: 25186 RVA: 0x0026AFBC File Offset: 0x002691BC
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

		// Token: 0x06006263 RID: 25187
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator GetDataCo();

		// Token: 0x06006264 RID: 25188 RVA: 0x0026B028 File Offset: 0x00269228
		public static TwitchDropAvailabilityManager.DropSource FromUri(TwitchDropAvailabilityManager owner, string uri)
		{
			if (uri.StartsWith("rfs://", StringComparison.OrdinalIgnoreCase))
			{
				return new TwitchDropAvailabilityManager.DropSourceRfs(owner, uri);
			}
			return new TwitchDropAvailabilityManager.DropSourceWww(owner, uri);
		}

		// Token: 0x04004C3F RID: 19519
		[PublicizedFrom(EAccessModifier.Protected)]
		public const string RfsScheme = "rfs://";

		// Token: 0x04004C40 RID: 19520
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly TwitchDropAvailabilityManager Owner;

		// Token: 0x04004C41 RID: 19521
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string OrigUri;

		// Token: 0x04004C42 RID: 19522
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool _isUpdating;

		// Token: 0x04004C43 RID: 19523
		[PublicizedFrom(EAccessModifier.Protected)]
		public DateTime _lastUpdated;

		// Token: 0x04004C44 RID: 19524
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<TwitchDropAvailabilityManager.TwitchDropEntry> _entries = new List<TwitchDropAvailabilityManager.TwitchDropEntry>();
	}

	// Token: 0x02000C84 RID: 3204
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DropSourceRfs : TwitchDropAvailabilityManager.DropSource
	{
		// Token: 0x06006265 RID: 25189 RVA: 0x0026B047 File Offset: 0x00269247
		public DropSourceRfs(TwitchDropAvailabilityManager owner, string uri)
			: base(owner, uri)
		{
			this._rfsKey = uri.Substring("rfs://".Length);
		}

		// Token: 0x06006266 RID: 25190 RVA: 0x0026B067 File Offset: 0x00269267
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

		// Token: 0x06006267 RID: 25191 RVA: 0x0026B078 File Offset: 0x00269278
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

		// Token: 0x04004C45 RID: 19525
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string _rfsKey;
	}

	// Token: 0x02000C86 RID: 3206
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DropSourceWww : TwitchDropAvailabilityManager.DropSource
	{
		// Token: 0x0600626E RID: 25198 RVA: 0x0026B220 File Offset: 0x00269420
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

		// Token: 0x0600626F RID: 25199 RVA: 0x0026B298 File Offset: 0x00269498
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

		// Token: 0x04004C4C RID: 19532
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string _patchedUri;
	}
}
