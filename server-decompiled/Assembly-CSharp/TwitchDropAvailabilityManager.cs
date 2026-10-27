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

// Token: 0x02000CAC RID: 3244
public sealed class TwitchDropAvailabilityManager
{
	// Token: 0x17000A60 RID: 2656
	// (get) Token: 0x060062F2 RID: 25330 RVA: 0x00272121 File Offset: 0x00270321
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
	// (add) Token: 0x060062F3 RID: 25331 RVA: 0x00272138 File Offset: 0x00270338
	// (remove) Token: 0x060062F4 RID: 25332 RVA: 0x00272170 File Offset: 0x00270370
	public event Action<TwitchDropAvailabilityManager> Updated;

	// Token: 0x060062F5 RID: 25333 RVA: 0x002721A5 File Offset: 0x002703A5
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchDropAvailabilityManager()
	{
	}

	// Token: 0x060062F6 RID: 25334 RVA: 0x002721B8 File Offset: 0x002703B8
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

	// Token: 0x060062F7 RID: 25335 RVA: 0x002721F0 File Offset: 0x002703F0
	public void UpdateAll(bool force = false)
	{
		foreach (KeyValuePair<string, TwitchDropAvailabilityManager.DropSource> keyValuePair in this._sources)
		{
			keyValuePair.Value.RequestData(force);
		}
	}

	// Token: 0x060062F8 RID: 25336 RVA: 0x0027224C File Offset: 0x0027044C
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

	// Token: 0x060062F9 RID: 25337 RVA: 0x002722D8 File Offset: 0x002704D8
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

	// Token: 0x060062FA RID: 25338 RVA: 0x00272322 File Offset: 0x00270522
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

	// Token: 0x060062FB RID: 25339 RVA: 0x00272338 File Offset: 0x00270538
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

	// Token: 0x060062FC RID: 25340 RVA: 0x0027248C File Offset: 0x0027068C
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

	// Token: 0x04004D31 RID: 19761
	[PublicizedFrom(EAccessModifier.Private)]
	public static TwitchDropAvailabilityManager _instance;

	// Token: 0x04004D32 RID: 19762
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, TwitchDropAvailabilityManager.DropSource> _sources = new CaseInsensitiveStringDictionary<TwitchDropAvailabilityManager.DropSource>();

	// Token: 0x02000CAD RID: 3245
	public sealed class TwitchDropEntry : IEquatable<TwitchDropAvailabilityManager.TwitchDropEntry>
	{
		// Token: 0x060062FD RID: 25341 RVA: 0x0027260D File Offset: 0x0027080D
		public TwitchDropEntry(string benefitId, DateTime startUtc, EntitlementSetEnum entitlementSet)
		{
			this.BenefitId = benefitId;
			this.Start = startUtc;
			this.EntitlementSet = entitlementSet;
		}

		// Token: 0x060062FE RID: 25342 RVA: 0x0027262A File Offset: 0x0027082A
		public bool IsAvailable(DateTime localNow)
		{
			return localNow.ToUniversalTime() >= this.Start;
		}

		// Token: 0x060062FF RID: 25343 RVA: 0x00272640 File Offset: 0x00270840
		public bool Equals(TwitchDropAvailabilityManager.TwitchDropEntry other)
		{
			return other != null && (this == other || (string.Equals(this.BenefitId, other.BenefitId, StringComparison.OrdinalIgnoreCase) && this.Start.Equals(other.Start) && this.EntitlementSet == other.EntitlementSet));
		}

		// Token: 0x06006300 RID: 25344 RVA: 0x00272694 File Offset: 0x00270894
		public override bool Equals(object obj)
		{
			TwitchDropAvailabilityManager.TwitchDropEntry twitchDropEntry = obj as TwitchDropAvailabilityManager.TwitchDropEntry;
			return twitchDropEntry != null && this.Equals(twitchDropEntry);
		}

		// Token: 0x06006301 RID: 25345 RVA: 0x002726B4 File Offset: 0x002708B4
		public override int GetHashCode()
		{
			return (((StringComparer.OrdinalIgnoreCase.GetHashCode(this.BenefitId ?? string.Empty) * 397) ^ this.Start.GetHashCode()) * 397) ^ (int)this.EntitlementSet;
		}

		// Token: 0x04004D34 RID: 19764
		public readonly string BenefitId;

		// Token: 0x04004D35 RID: 19765
		public readonly DateTime Start;

		// Token: 0x04004D36 RID: 19766
		public readonly EntitlementSetEnum EntitlementSet;
	}

	// Token: 0x02000CAE RID: 3246
	public abstract class DropSource
	{
		// Token: 0x06006302 RID: 25346 RVA: 0x002726FC File Offset: 0x002708FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public DropSource(TwitchDropAvailabilityManager owner, string uri)
		{
			this.Owner = owner;
			this.OrigUri = uri;
		}

		// Token: 0x06006303 RID: 25347 RVA: 0x00272720 File Offset: 0x00270920
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

		// Token: 0x06006304 RID: 25348 RVA: 0x002727B8 File Offset: 0x002709B8
		public void GetData(List<TwitchDropAvailabilityManager.TwitchDropEntry> target)
		{
			TwitchDropAvailabilityManager owner = this.Owner;
			lock (owner)
			{
				target.AddRange(this._entries);
			}
		}

		// Token: 0x06006305 RID: 25349 RVA: 0x00272800 File Offset: 0x00270A00
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

		// Token: 0x06006306 RID: 25350
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator GetDataCo();

		// Token: 0x06006307 RID: 25351 RVA: 0x0027286C File Offset: 0x00270A6C
		public static TwitchDropAvailabilityManager.DropSource FromUri(TwitchDropAvailabilityManager owner, string uri)
		{
			if (uri.StartsWith("rfs://", StringComparison.OrdinalIgnoreCase))
			{
				return new TwitchDropAvailabilityManager.DropSourceRfs(owner, uri);
			}
			return new TwitchDropAvailabilityManager.DropSourceWww(owner, uri);
		}

		// Token: 0x04004D37 RID: 19767
		[PublicizedFrom(EAccessModifier.Protected)]
		public const string RfsScheme = "rfs://";

		// Token: 0x04004D38 RID: 19768
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly TwitchDropAvailabilityManager Owner;

		// Token: 0x04004D39 RID: 19769
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string OrigUri;

		// Token: 0x04004D3A RID: 19770
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool _isUpdating;

		// Token: 0x04004D3B RID: 19771
		[PublicizedFrom(EAccessModifier.Protected)]
		public DateTime _lastUpdated;

		// Token: 0x04004D3C RID: 19772
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<TwitchDropAvailabilityManager.TwitchDropEntry> _entries = new List<TwitchDropAvailabilityManager.TwitchDropEntry>();
	}

	// Token: 0x02000CAF RID: 3247
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DropSourceRfs : TwitchDropAvailabilityManager.DropSource
	{
		// Token: 0x06006308 RID: 25352 RVA: 0x0027288B File Offset: 0x00270A8B
		public DropSourceRfs(TwitchDropAvailabilityManager owner, string uri)
			: base(owner, uri)
		{
			this._rfsKey = uri.Substring("rfs://".Length);
		}

		// Token: 0x06006309 RID: 25353 RVA: 0x002728AB File Offset: 0x00270AAB
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

		// Token: 0x0600630A RID: 25354 RVA: 0x002728BC File Offset: 0x00270ABC
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

		// Token: 0x04004D3D RID: 19773
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string _rfsKey;
	}

	// Token: 0x02000CB1 RID: 3249
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DropSourceWww : TwitchDropAvailabilityManager.DropSource
	{
		// Token: 0x06006311 RID: 25361 RVA: 0x00272A64 File Offset: 0x00270C64
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

		// Token: 0x06006312 RID: 25362 RVA: 0x00272ADC File Offset: 0x00270CDC
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

		// Token: 0x04004D44 RID: 19780
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string _patchedUri;
	}
}
