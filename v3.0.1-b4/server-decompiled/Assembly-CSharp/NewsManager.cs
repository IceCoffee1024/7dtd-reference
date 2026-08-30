using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Platform;
using UnityEngine;

// Token: 0x0200125F RID: 4703
public class NewsManager
{
	// Token: 0x170011AC RID: 4524
	// (get) Token: 0x06009571 RID: 38257 RVA: 0x00384B28 File Offset: 0x00382D28
	public static NewsManager Instance
	{
		get
		{
			NewsManager newsManager;
			if ((newsManager = NewsManager.instance) == null)
			{
				newsManager = (NewsManager.instance = new NewsManager());
			}
			return newsManager;
		}
	}

	// Token: 0x140000FB RID: 251
	// (add) Token: 0x06009572 RID: 38258 RVA: 0x00384B40 File Offset: 0x00382D40
	// (remove) Token: 0x06009573 RID: 38259 RVA: 0x00384B78 File Offset: 0x00382D78
	public event Action<NewsManager> Updated;

	// Token: 0x06009574 RID: 38260 RVA: 0x00384BB0 File Offset: 0x00382DB0
	public void UpdateNews(bool _force = false)
	{
		foreach (KeyValuePair<string, NewsManager.NewsSource> keyValuePair in this.sources)
		{
			string text;
			NewsManager.NewsSource newsSource;
			keyValuePair.Deconstruct(out text, out newsSource);
			newsSource.RequestData(_force);
		}
	}

	// Token: 0x06009575 RID: 38261 RVA: 0x00384C10 File Offset: 0x00382E10
	public void RegisterNewsSource(string _uri)
	{
		if (this.sources.ContainsKey(_uri))
		{
			return;
		}
		NewsManager.NewsSource newsSource = NewsManager.NewsSource.FromUri(this, _uri);
		this.sources[_uri] = newsSource;
		newsSource.RequestData(false);
	}

	// Token: 0x06009576 RID: 38262 RVA: 0x00384C48 File Offset: 0x00382E48
	public void GetNewsData(List<string> _sources, List<NewsManager.NewsEntry> _target)
	{
		_target.Clear();
		foreach (string text in _sources)
		{
			NewsManager.NewsSource newsSource;
			if (this.sources.TryGetValue(text, out newsSource))
			{
				newsSource.GetData(_target);
			}
		}
		this.sortNewsByAge(_target);
	}

	// Token: 0x06009577 RID: 38263 RVA: 0x00384CB4 File Offset: 0x00382EB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void sortNewsByAge(List<NewsManager.NewsEntry> _list)
	{
		_list.Sort((NewsManager.NewsEntry _entryA, NewsManager.NewsEntry _entryB) => _entryB.Date.CompareTo(_entryA.Date));
	}

	// Token: 0x06009578 RID: 38264 RVA: 0x00384CDB File Offset: 0x00382EDB
	[PublicizedFrom(EAccessModifier.Private)]
	public void notifyListeners()
	{
		Action<NewsManager> updated = this.Updated;
		if (updated == null)
		{
			return;
		}
		updated(this);
	}

	// Token: 0x04006FDA RID: 28634
	[PublicizedFrom(EAccessModifier.Private)]
	public static NewsManager instance;

	// Token: 0x04006FDB RID: 28635
	public static readonly NewsManager.NewsEntry EmptyEntry = new NewsManager.NewsEntry(null, null, null, "- No Entries -", null, "", null, DateTime.Now);

	// Token: 0x04006FDC RID: 28636
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, NewsManager.NewsSource> sources = new CaseInsensitiveStringDictionary<NewsManager.NewsSource>();

	// Token: 0x02001260 RID: 4704
	public abstract class NewsSource
	{
		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x0600957B RID: 38267
		public abstract bool IsCustom { get; }

		// Token: 0x0600957C RID: 38268 RVA: 0x00384D21 File Offset: 0x00382F21
		[PublicizedFrom(EAccessModifier.Protected)]
		public NewsSource(NewsManager _owner, string _uri)
		{
			this.Owner = _owner;
			this.OrigUri = _uri;
		}

		// Token: 0x0600957D RID: 38269 RVA: 0x00384D44 File Offset: 0x00382F44
		public void RequestData(bool _force)
		{
			if (this.isUpdating)
			{
				return;
			}
			DateTime now = DateTime.Now;
			NewsManager owner = this.Owner;
			lock (owner)
			{
				if (!_force && this.entries.Count > 0 && (now - this.lastUpdated).TotalMinutes < 1.0)
				{
					return;
				}
			}
			this.lastUpdated = now;
			this.isUpdating = true;
			ThreadManager.StartCoroutine(this.GetDataCo());
		}

		// Token: 0x0600957E RID: 38270
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator GetDataCo();

		// Token: 0x0600957F RID: 38271 RVA: 0x00384DDC File Offset: 0x00382FDC
		public void GetData(List<NewsManager.NewsEntry> _target)
		{
			NewsManager owner = this.Owner;
			lock (owner)
			{
				_target.AddRange(this.entries);
			}
		}

		// Token: 0x06009580 RID: 38272
		public abstract void RequestImage(string _imageRelPath, Action<Texture2D> _callback);

		// Token: 0x06009581 RID: 38273 RVA: 0x00384E24 File Offset: 0x00383024
		[PublicizedFrom(EAccessModifier.Protected)]
		public void LoadXml(XmlFile _xml)
		{
			this.isUpdating = false;
			NewsManager newsManager = this.Owner;
			lock (newsManager)
			{
				this.entries.Clear();
			}
			XElement xelement = ((_xml != null) ? _xml.XmlDoc.Root : null);
			if (xelement == null)
			{
				this.Owner.notifyListeners();
				return;
			}
			string text = xelement.GetAttribute("name").Trim();
			if (text == "")
			{
				text = null;
			}
			newsManager = this.Owner;
			lock (newsManager)
			{
				foreach (XElement xelement2 in xelement.Elements("entry"))
				{
					NewsManager.NewsEntry newsEntry = NewsManager.NewsEntry.FromXml(this, text, xelement2);
					if (newsEntry != null)
					{
						this.entries.Add(newsEntry);
					}
				}
			}
			this.Owner.notifyListeners();
		}

		// Token: 0x06009582 RID: 38274 RVA: 0x00384F4C File Offset: 0x0038314C
		public static NewsManager.NewsSource FromUri(NewsManager _owner, string _uri)
		{
			if (_uri.StartsWith("rfs://"))
			{
				return new NewsManager.NewsSourceRfs(_owner, _uri);
			}
			return new NewsManager.NewsSourceWww(_owner, _uri);
		}

		// Token: 0x04006FDE RID: 28638
		[PublicizedFrom(EAccessModifier.Protected)]
		public const string RemoteFileStorageProtocol = "rfs://";

		// Token: 0x04006FDF RID: 28639
		public readonly NewsManager Owner;

		// Token: 0x04006FE0 RID: 28640
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string OrigUri;

		// Token: 0x04006FE1 RID: 28641
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isUpdating;

		// Token: 0x04006FE2 RID: 28642
		[PublicizedFrom(EAccessModifier.Protected)]
		public DateTime lastUpdated;

		// Token: 0x04006FE3 RID: 28643
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<NewsManager.NewsEntry> entries = new List<NewsManager.NewsEntry>();
	}

	// Token: 0x02001261 RID: 4705
	[PublicizedFrom(EAccessModifier.Private)]
	public class NewsSourceRfs : NewsManager.NewsSource
	{
		// Token: 0x06009583 RID: 38275 RVA: 0x00384F6A File Offset: 0x0038316A
		public NewsSourceRfs(NewsManager _owner, string _uri)
			: base(_owner, _uri)
		{
			this.rfsFilename = _uri.Substring("rfs://".Length);
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x06009584 RID: 38276 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool IsCustom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009585 RID: 38277 RVA: 0x00384F95 File Offset: 0x00383195
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator GetDataCo()
		{
			IRemoteFileStorage storage = PlatformManager.MultiPlatform.RemoteFileStorage;
			if (storage == null)
			{
				this.isUpdating = false;
				yield break;
			}
			if (PlatformManager.NativePlatform.User.UserStatus != EUserStatus.LoggedIn)
			{
				storage.GetCachedFile(this.rfsFilename, new IRemoteFileStorage.FileDownloadCompleteCallback(this.<GetDataCo>g__fileDownloadedCallback|4_0));
				this.lastUpdated = DateTime.MinValue;
				yield break;
			}
			bool loggedSlow = false;
			float startTime = Time.time;
			while (!storage.IsReady)
			{
				if (storage.Unavailable)
				{
					Log.Warning("Remote Storage is unavailable");
					this.isUpdating = false;
					yield break;
				}
				yield return null;
				if (!loggedSlow && Time.time > startTime + 30f)
				{
					loggedSlow = true;
					Log.Warning("Waiting for news from remote storage exceeded 30s");
				}
			}
			storage.GetFile(this.rfsFilename, new IRemoteFileStorage.FileDownloadCompleteCallback(this.<GetDataCo>g__fileDownloadedCallback|4_0));
			yield break;
		}

		// Token: 0x06009586 RID: 38278 RVA: 0x00384FA4 File Offset: 0x003831A4
		public override void RequestImage(string _imageRelPath, Action<Texture2D> _callback)
		{
			Queue<ValueTuple<string, Action<Texture2D>>> queue = this.requestedImagesQueue;
			lock (queue)
			{
				this.requestedImagesQueue.Enqueue(new ValueTuple<string, Action<Texture2D>>(_imageRelPath, _callback));
			}
			this.startNextImageRequest();
		}

		// Token: 0x06009587 RID: 38279 RVA: 0x00384FF8 File Offset: 0x003831F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void startNextImageRequest()
		{
			Queue<ValueTuple<string, Action<Texture2D>>> queue = this.requestedImagesQueue;
			lock (queue)
			{
				ValueTuple<string, Action<Texture2D>> valueTuple;
				if (this.runningImageRequests < 3 && this.requestedImagesQueue.TryDequeue(out valueTuple))
				{
					ThreadManager.StartCoroutine(this.requestFromRemoteStorage(valueTuple.Item1, valueTuple.Item2));
				}
			}
		}

		// Token: 0x06009588 RID: 38280 RVA: 0x00385064 File Offset: 0x00383264
		[PublicizedFrom(EAccessModifier.Private)]
		public void imageRequestCompleted()
		{
			Queue<ValueTuple<string, Action<Texture2D>>> queue = this.requestedImagesQueue;
			lock (queue)
			{
				this.runningImageRequests--;
				this.startNextImageRequest();
			}
		}

		// Token: 0x06009589 RID: 38281 RVA: 0x003850B4 File Offset: 0x003832B4
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator requestFromRemoteStorage(string _imageRelPath, Action<Texture2D> _callback)
		{
			NewsManager.NewsSourceRfs.<>c__DisplayClass11_0 CS$<>8__locals1 = new NewsManager.NewsSourceRfs.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1._imageRelPath = _imageRelPath;
			CS$<>8__locals1._callback = _callback;
			IRemoteFileStorage remoteFileStorage = PlatformManager.MultiPlatform.RemoteFileStorage;
			if (remoteFileStorage == null)
			{
				yield break;
			}
			Queue<ValueTuple<string, Action<Texture2D>>> queue = this.requestedImagesQueue;
			lock (queue)
			{
				this.runningImageRequests++;
			}
			if (remoteFileStorage.Unavailable)
			{
				remoteFileStorage.GetCachedFile(CS$<>8__locals1._imageRelPath, new IRemoteFileStorage.FileDownloadCompleteCallback(CS$<>8__locals1.<requestFromRemoteStorage>g__imageDownloadedCallback|0));
			}
			else
			{
				remoteFileStorage.GetFile(CS$<>8__locals1._imageRelPath, new IRemoteFileStorage.FileDownloadCompleteCallback(CS$<>8__locals1.<requestFromRemoteStorage>g__imageDownloadedCallback|0));
			}
			yield break;
		}

		// Token: 0x0600958A RID: 38282 RVA: 0x003850D4 File Offset: 0x003832D4
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <GetDataCo>g__fileDownloadedCallback|4_0(IRemoteFileStorage.EFileDownloadResult _result, string _errorDetails, byte[] _data)
		{
			if (_result != IRemoteFileStorage.EFileDownloadResult.Ok)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Retrieving remote news file '",
					this.rfsFilename,
					"' failed: ",
					_result.ToStringCached<IRemoteFileStorage.EFileDownloadResult>(),
					" (",
					_errorDetails,
					")"
				}));
				base.LoadXml(null);
				return;
			}
			XmlFile xmlFile = null;
			if (_data != null && _data.Length != 0)
			{
				try
				{
					xmlFile = new XmlFile(_data, true);
				}
				catch (Exception ex)
				{
					Log.Error("Failed loading news XML:");
					Log.Exception(ex);
					return;
				}
			}
			base.LoadXml(xmlFile);
		}

		// Token: 0x04006FE4 RID: 28644
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string rfsFilename;

		// Token: 0x04006FE5 RID: 28645
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxParallelImageRequests = 3;

		// Token: 0x04006FE6 RID: 28646
		[TupleElementNames(new string[] { "imageRelPath", "callback" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Queue<ValueTuple<string, Action<Texture2D>>> requestedImagesQueue = new Queue<ValueTuple<string, Action<Texture2D>>>();

		// Token: 0x04006FE7 RID: 28647
		[PublicizedFrom(EAccessModifier.Private)]
		public int runningImageRequests;
	}

	// Token: 0x02001265 RID: 4709
	[PublicizedFrom(EAccessModifier.Private)]
	public class NewsSourceWww : NewsManager.NewsSource
	{
		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x06009599 RID: 38297 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool IsCustom
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600959A RID: 38298 RVA: 0x00385428 File Offset: 0x00383628
		public NewsSourceWww(NewsManager _owner, string _uri)
			: base(_owner, _uri)
		{
			string text = _uri;
			if (!text.StartsWith("http", StringComparison.Ordinal))
			{
				string text2 = ModManager.PatchModPathString(text);
				if (text2 == null)
				{
					throw new ArgumentException("WWW news source '" + _uri + "' can not be retrieved: Neither is a 'http(s)://' URI nor a '@modfolder:' reference.");
				}
				text = "file://" + text2;
			}
			text = text.Replace("#", "%23").Replace("+", "%2B");
			this.patchedUri = text;
			int num = text.LastIndexOf('/');
			if (num < 0)
			{
				throw new ArgumentException("WWW news source '" + _uri + "' does not have a valid path");
			}
			this.baseUri = text.Substring(0, num + 1);
		}

		// Token: 0x0600959B RID: 38299 RVA: 0x003854D3 File Offset: 0x003836D3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator GetDataCo()
		{
			NewsManager.NewsSourceWww.<GetDataCo>d__5 <GetDataCo>d__ = new NewsManager.NewsSourceWww.<GetDataCo>d__5(0);
			<GetDataCo>d__.<>4__this = this;
			return <GetDataCo>d__;
		}

		// Token: 0x0600959C RID: 38300 RVA: 0x003854E2 File Offset: 0x003836E2
		public override void RequestImage(string _imageRelPath, Action<Texture2D> _callback)
		{
			ThreadManager.StartCoroutine(this.requestFromUri(_imageRelPath, _callback));
		}

		// Token: 0x0600959D RID: 38301 RVA: 0x003854F2 File Offset: 0x003836F2
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator requestFromUri(string _imageRelPath, Action<Texture2D> _callback)
		{
			NewsManager.NewsSourceWww.<requestFromUri>d__7 <requestFromUri>d__ = new NewsManager.NewsSourceWww.<requestFromUri>d__7(0);
			<requestFromUri>d__.<>4__this = this;
			<requestFromUri>d__._imageRelPath = _imageRelPath;
			<requestFromUri>d__._callback = _callback;
			return <requestFromUri>d__;
		}

		// Token: 0x04006FF6 RID: 28662
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string patchedUri;

		// Token: 0x04006FF7 RID: 28663
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string baseUri;
	}

	// Token: 0x02001268 RID: 4712
	public class NewsEntry : IEquatable<NewsManager.NewsEntry>
	{
		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x060095AC RID: 38316 RVA: 0x00385800 File Offset: 0x00383A00
		public bool IsCustom
		{
			get
			{
				return this.owner == null || this.owner.IsCustom;
			}
		}

		// Token: 0x060095AD RID: 38317 RVA: 0x00385818 File Offset: 0x00383A18
		public NewsEntry(NewsManager.NewsSource _owner, string _customListName, string _imageRelPath, string _headline, string _headline2, string _text, string _url, DateTime _date)
		{
			this.owner = _owner;
			this.CustomListName = _customListName;
			this.imageRelPath = _imageRelPath;
			this.Headline = _headline;
			this.Headline2 = _headline2;
			this.Text = _text;
			this.Url = _url;
			this.Date = _date;
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x060095AE RID: 38318 RVA: 0x00385868 File Offset: 0x00383A68
		public bool HasImage
		{
			get
			{
				return !string.IsNullOrEmpty(this.imageRelPath);
			}
		}

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x060095AF RID: 38319 RVA: 0x00385878 File Offset: 0x00383A78
		public bool ImageLoaded
		{
			get
			{
				return this.image != null;
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x060095B0 RID: 38320 RVA: 0x00385886 File Offset: 0x00383A86
		public Texture2D Image
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x060095B1 RID: 38321 RVA: 0x0038588E File Offset: 0x00383A8E
		public void RequestImage()
		{
			if (!this.HasImage)
			{
				return;
			}
			if (this.requestedImage)
			{
				return;
			}
			this.requestedImage = true;
			this.owner.RequestImage(this.imageRelPath, new Action<Texture2D>(this.setImage));
		}

		// Token: 0x060095B2 RID: 38322 RVA: 0x003858C8 File Offset: 0x00383AC8
		[PublicizedFrom(EAccessModifier.Private)]
		public void setImage(Texture2D _image)
		{
			this.image = _image;
			this.image.name = "NewsImage_" + this.imageRelPath;
			this.image.Compress(true);
			this.image.Apply(false, true);
			this.owner.Owner.notifyListeners();
		}

		// Token: 0x060095B3 RID: 38323 RVA: 0x00385920 File Offset: 0x00383B20
		public bool Equals(NewsManager.NewsEntry _other)
		{
			return _other != null && (this == _other || (this.CustomListName == _other.CustomListName && this.imageRelPath == _other.imageRelPath && this.Headline == _other.Headline && this.Headline2 == _other.Headline2 && this.Text == _other.Text && this.Url == _other.Url && this.Date.Equals(_other.Date)));
		}

		// Token: 0x060095B4 RID: 38324 RVA: 0x003859C0 File Offset: 0x00383BC0
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((NewsManager.NewsEntry)_obj)));
		}

		// Token: 0x060095B5 RID: 38325 RVA: 0x003859F0 File Offset: 0x00383BF0
		public override int GetHashCode()
		{
			return (((((((((((((this.CustomListName != null) ? this.CustomListName.GetHashCode() : 0) * 397) ^ ((this.imageRelPath != null) ? this.imageRelPath.GetHashCode() : 0)) * 397) ^ ((this.Headline != null) ? this.Headline.GetHashCode() : 0)) * 397) ^ ((this.Headline2 != null) ? this.Headline2.GetHashCode() : 0)) * 397) ^ ((this.Text != null) ? this.Text.GetHashCode() : 0)) * 397) ^ ((this.Url != null) ? this.Url.GetHashCode() : 0)) * 397) ^ this.Date.GetHashCode();
		}

		// Token: 0x060095B6 RID: 38326 RVA: 0x00385ABC File Offset: 0x00383CBC
		public static NewsManager.NewsEntry FromXml(NewsManager.NewsSource _owner, string _customListName, XElement _element)
		{
			string text = null;
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = null;
			DateTime minValue = DateTime.MinValue;
			DateTime maxValue = DateTime.MaxValue;
			bool flag = true;
			foreach (XElement xelement in _element.Elements())
			{
				string localName = xelement.Name.LocalName;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(localName);
				if (num <= 1990630727U)
				{
					if (num <= 561879464U)
					{
						if (num != 232457833U)
						{
							if (num == 561879464U)
							{
								if (localName == "devicetypes")
								{
									bool flag2 = false;
									string[] array = xelement.Value.Split(',', StringSplitOptions.None);
									for (int i = 0; i < array.Length; i++)
									{
										string text6 = array[i].Trim();
										if (!string.IsNullOrEmpty(text6))
										{
											EDeviceType edeviceType;
											if (!EnumUtils.TryParse<EDeviceType>(text6, out edeviceType, true))
											{
												Log.Warning(string.Format("News XML has an entry with an invalid 'devicetypes' element '{0}', devicetype '{1}' unknown at line {2}", xelement.Value, text6, ((IXmlLineInfo)_element).LineNumber));
												return null;
											}
											if (edeviceType == PlatformManager.DeviceType)
											{
												flag2 = true;
												break;
											}
										}
									}
									if (!flag2)
									{
										return null;
									}
									continue;
								}
							}
						}
						else if (localName == "link")
						{
							text5 = xelement.Value.Trim();
							continue;
						}
					}
					else if (num != 589056993U)
					{
						if (num != 1406002643U)
						{
							if (num == 1990630727U)
							{
								if (localName == "showbefore")
								{
									if (!bool.TryParse(xelement.Value, out flag))
									{
										Log.Warning(string.Format("News XML has an entry with an invalid 'showbefore' element '{0}' at line {1}", xelement.Value, ((IXmlLineInfo)_element).LineNumber));
										return null;
									}
									continue;
								}
							}
						}
						else if (localName == "platforms")
						{
							bool flag3 = false;
							string[] array = xelement.Value.Split(',', StringSplitOptions.None);
							for (int i = 0; i < array.Length; i++)
							{
								string text7 = array[i].Trim();
								if (!string.IsNullOrEmpty(text7))
								{
									EPlatformIdentifier eplatformIdentifier;
									if (!EnumUtils.TryParse<EPlatformIdentifier>(text7, out eplatformIdentifier, true))
									{
										Log.Warning(string.Format("News XML has an entry with an invalid 'platforms' element '{0}', platform '{1}' unknown at line {2}", xelement.Value, text7, ((IXmlLineInfo)_element).LineNumber));
										return null;
									}
									if (eplatformIdentifier != PlatformManager.NativePlatform.PlatformIdentifier)
									{
										EPlatformIdentifier eplatformIdentifier2 = eplatformIdentifier;
										IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
										EPlatformIdentifier? eplatformIdentifier3 = ((crossplatformPlatform != null) ? new EPlatformIdentifier?(crossplatformPlatform.PlatformIdentifier) : null);
										if (!((eplatformIdentifier2 == eplatformIdentifier3.GetValueOrDefault()) & (eplatformIdentifier3 != null)))
										{
											goto IL_03A0;
										}
									}
									flag3 = true;
									break;
								}
								IL_03A0:;
							}
							if (!flag3)
							{
								return null;
							}
							continue;
						}
					}
					else if (localName == "title2")
					{
						text3 = xelement.Value;
						continue;
					}
				}
				else if (num <= 2556802313U)
				{
					if (num != 2428985098U)
					{
						if (num == 2556802313U)
						{
							if (localName == "title")
							{
								text2 = xelement.Value;
								continue;
							}
						}
					}
					else if (localName == "imagerelpath")
					{
						text = xelement.Value;
						continue;
					}
				}
				else if (num != 3185987134U)
				{
					if (num != 3564297305U)
					{
						if (num == 3808839532U)
						{
							if (localName == "showuntil")
							{
								if (!DateTime.TryParseExact(xelement.Value, "u", null, DateTimeStyles.AssumeUniversal, out maxValue))
								{
									Log.Warning(string.Format("News XML has an entry with an invalid 'showuntil' element '{0}' at line {1}", xelement.Value, ((IXmlLineInfo)_element).LineNumber));
									return null;
								}
								continue;
							}
						}
					}
					else if (localName == "date")
					{
						if (!DateTime.TryParseExact(xelement.Value, "u", null, DateTimeStyles.AssumeUniversal, out minValue))
						{
							Log.Warning(string.Format("News XML has an entry with an invalid 'date' element '{0}' at line {1}", xelement.Value, ((IXmlLineInfo)_element).LineNumber));
							return null;
						}
						continue;
					}
				}
				else if (localName == "text")
				{
					text4 = xelement.Value;
					continue;
				}
				Log.Warning(string.Format("News XML has an entry with an unknown element '{0}' at line {1}", xelement.Name.LocalName, ((IXmlLineInfo)_element).LineNumber));
			}
			if (minValue == DateTime.MinValue)
			{
				Log.Warning(string.Format("News XML has an entry without a date element at line {0}", ((IXmlLineInfo)_element).LineNumber));
				return null;
			}
			DateTime now = DateTime.Now;
			if (!flag && minValue > now)
			{
				return null;
			}
			if (maxValue < now)
			{
				return null;
			}
			return new NewsManager.NewsEntry(_owner, _customListName, text, text2, text3, text4, text5, minValue);
		}

		// Token: 0x04007003 RID: 28675
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NewsManager.NewsSource owner;

		// Token: 0x04007004 RID: 28676
		public readonly string CustomListName;

		// Token: 0x04007005 RID: 28677
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string imageRelPath;

		// Token: 0x04007006 RID: 28678
		public readonly string Headline;

		// Token: 0x04007007 RID: 28679
		public readonly string Headline2;

		// Token: 0x04007008 RID: 28680
		public readonly string Text;

		// Token: 0x04007009 RID: 28681
		public readonly string Url;

		// Token: 0x0400700A RID: 28682
		public readonly DateTime Date;

		// Token: 0x0400700B RID: 28683
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requestedImage;

		// Token: 0x0400700C RID: 28684
		[PublicizedFrom(EAccessModifier.Private)]
		public Texture2D image;
	}
}
