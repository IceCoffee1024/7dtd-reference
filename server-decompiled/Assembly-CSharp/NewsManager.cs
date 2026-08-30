using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Platform;
using UnityEngine;

// Token: 0x02001279 RID: 4729
public class NewsManager
{
	// Token: 0x170011E5 RID: 4581
	// (get) Token: 0x0600966C RID: 38508 RVA: 0x00389C84 File Offset: 0x00387E84
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

	// Token: 0x140000FC RID: 252
	// (add) Token: 0x0600966D RID: 38509 RVA: 0x00389C9C File Offset: 0x00387E9C
	// (remove) Token: 0x0600966E RID: 38510 RVA: 0x00389CD4 File Offset: 0x00387ED4
	public event Action<NewsManager> Updated;

	// Token: 0x0600966F RID: 38511 RVA: 0x00389D0C File Offset: 0x00387F0C
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

	// Token: 0x06009670 RID: 38512 RVA: 0x00389D6C File Offset: 0x00387F6C
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

	// Token: 0x06009671 RID: 38513 RVA: 0x00389DA4 File Offset: 0x00387FA4
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

	// Token: 0x06009672 RID: 38514 RVA: 0x00389E10 File Offset: 0x00388010
	[PublicizedFrom(EAccessModifier.Private)]
	public void sortNewsByAge(List<NewsManager.NewsEntry> _list)
	{
		_list.Sort((NewsManager.NewsEntry _entryA, NewsManager.NewsEntry _entryB) => _entryB.Date.CompareTo(_entryA.Date));
	}

	// Token: 0x06009673 RID: 38515 RVA: 0x00389E37 File Offset: 0x00388037
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

	// Token: 0x040070AF RID: 28847
	[PublicizedFrom(EAccessModifier.Private)]
	public static NewsManager instance;

	// Token: 0x040070B0 RID: 28848
	public static readonly NewsManager.NewsEntry EmptyEntry = new NewsManager.NewsEntry(null, null, null, "- No Entries -", null, "", null, DateTime.Now);

	// Token: 0x040070B1 RID: 28849
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, NewsManager.NewsSource> sources = new CaseInsensitiveStringDictionary<NewsManager.NewsSource>();

	// Token: 0x0200127A RID: 4730
	public abstract class NewsSource
	{
		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x06009676 RID: 38518
		public abstract bool IsCustom { get; }

		// Token: 0x06009677 RID: 38519 RVA: 0x00389E7D File Offset: 0x0038807D
		[PublicizedFrom(EAccessModifier.Protected)]
		public NewsSource(NewsManager _owner, string _uri)
		{
			this.Owner = _owner;
			this.OrigUri = _uri;
		}

		// Token: 0x06009678 RID: 38520 RVA: 0x00389EA0 File Offset: 0x003880A0
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

		// Token: 0x06009679 RID: 38521
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator GetDataCo();

		// Token: 0x0600967A RID: 38522 RVA: 0x00389F38 File Offset: 0x00388138
		public void GetData(List<NewsManager.NewsEntry> _target)
		{
			NewsManager owner = this.Owner;
			lock (owner)
			{
				_target.AddRange(this.entries);
			}
		}

		// Token: 0x0600967B RID: 38523
		public abstract void RequestImage(string _imageRelPath, Action<Texture2D> _callback);

		// Token: 0x0600967C RID: 38524 RVA: 0x00389F80 File Offset: 0x00388180
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

		// Token: 0x0600967D RID: 38525 RVA: 0x0038A0A8 File Offset: 0x003882A8
		public static NewsManager.NewsSource FromUri(NewsManager _owner, string _uri)
		{
			if (_uri.StartsWith("rfs://"))
			{
				return new NewsManager.NewsSourceRfs(_owner, _uri);
			}
			return new NewsManager.NewsSourceWww(_owner, _uri);
		}

		// Token: 0x040070B3 RID: 28851
		[PublicizedFrom(EAccessModifier.Protected)]
		public const string RemoteFileStorageProtocol = "rfs://";

		// Token: 0x040070B4 RID: 28852
		public readonly NewsManager Owner;

		// Token: 0x040070B5 RID: 28853
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string OrigUri;

		// Token: 0x040070B6 RID: 28854
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isUpdating;

		// Token: 0x040070B7 RID: 28855
		[PublicizedFrom(EAccessModifier.Protected)]
		public DateTime lastUpdated;

		// Token: 0x040070B8 RID: 28856
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<NewsManager.NewsEntry> entries = new List<NewsManager.NewsEntry>();
	}

	// Token: 0x0200127B RID: 4731
	[PublicizedFrom(EAccessModifier.Private)]
	public class NewsSourceRfs : NewsManager.NewsSource
	{
		// Token: 0x0600967E RID: 38526 RVA: 0x0038A0C6 File Offset: 0x003882C6
		public NewsSourceRfs(NewsManager _owner, string _uri)
			: base(_owner, _uri)
		{
			this.rfsFilename = _uri.Substring("rfs://".Length);
		}

		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x0600967F RID: 38527 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool IsCustom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009680 RID: 38528 RVA: 0x0038A0F1 File Offset: 0x003882F1
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

		// Token: 0x06009681 RID: 38529 RVA: 0x0038A100 File Offset: 0x00388300
		public override void RequestImage(string _imageRelPath, Action<Texture2D> _callback)
		{
			Queue<ValueTuple<string, Action<Texture2D>>> queue = this.requestedImagesQueue;
			lock (queue)
			{
				this.requestedImagesQueue.Enqueue(new ValueTuple<string, Action<Texture2D>>(_imageRelPath, _callback));
			}
			this.startNextImageRequest();
		}

		// Token: 0x06009682 RID: 38530 RVA: 0x0038A154 File Offset: 0x00388354
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

		// Token: 0x06009683 RID: 38531 RVA: 0x0038A1C0 File Offset: 0x003883C0
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

		// Token: 0x06009684 RID: 38532 RVA: 0x0038A210 File Offset: 0x00388410
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

		// Token: 0x06009685 RID: 38533 RVA: 0x0038A230 File Offset: 0x00388430
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

		// Token: 0x040070B9 RID: 28857
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string rfsFilename;

		// Token: 0x040070BA RID: 28858
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxParallelImageRequests = 3;

		// Token: 0x040070BB RID: 28859
		[TupleElementNames(new string[] { "imageRelPath", "callback" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Queue<ValueTuple<string, Action<Texture2D>>> requestedImagesQueue = new Queue<ValueTuple<string, Action<Texture2D>>>();

		// Token: 0x040070BC RID: 28860
		[PublicizedFrom(EAccessModifier.Private)]
		public int runningImageRequests;
	}

	// Token: 0x0200127F RID: 4735
	[PublicizedFrom(EAccessModifier.Private)]
	public class NewsSourceWww : NewsManager.NewsSource
	{
		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x06009694 RID: 38548 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool IsCustom
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009695 RID: 38549 RVA: 0x0038A584 File Offset: 0x00388784
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

		// Token: 0x06009696 RID: 38550 RVA: 0x0038A62F File Offset: 0x0038882F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator GetDataCo()
		{
			NewsManager.NewsSourceWww.<GetDataCo>d__5 <GetDataCo>d__ = new NewsManager.NewsSourceWww.<GetDataCo>d__5(0);
			<GetDataCo>d__.<>4__this = this;
			return <GetDataCo>d__;
		}

		// Token: 0x06009697 RID: 38551 RVA: 0x0038A63E File Offset: 0x0038883E
		public override void RequestImage(string _imageRelPath, Action<Texture2D> _callback)
		{
			ThreadManager.StartCoroutine(this.requestFromUri(_imageRelPath, _callback));
		}

		// Token: 0x06009698 RID: 38552 RVA: 0x0038A64E File Offset: 0x0038884E
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator requestFromUri(string _imageRelPath, Action<Texture2D> _callback)
		{
			NewsManager.NewsSourceWww.<requestFromUri>d__7 <requestFromUri>d__ = new NewsManager.NewsSourceWww.<requestFromUri>d__7(0);
			<requestFromUri>d__.<>4__this = this;
			<requestFromUri>d__._imageRelPath = _imageRelPath;
			<requestFromUri>d__._callback = _callback;
			return <requestFromUri>d__;
		}

		// Token: 0x040070CB RID: 28875
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string patchedUri;

		// Token: 0x040070CC RID: 28876
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string baseUri;
	}

	// Token: 0x02001282 RID: 4738
	public class NewsEntry : IEquatable<NewsManager.NewsEntry>
	{
		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x060096A7 RID: 38567 RVA: 0x0038A95C File Offset: 0x00388B5C
		public bool IsCustom
		{
			get
			{
				return this.owner == null || this.owner.IsCustom;
			}
		}

		// Token: 0x060096A8 RID: 38568 RVA: 0x0038A974 File Offset: 0x00388B74
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

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x060096A9 RID: 38569 RVA: 0x0038A9C4 File Offset: 0x00388BC4
		public bool HasImage
		{
			get
			{
				return !string.IsNullOrEmpty(this.imageRelPath);
			}
		}

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x060096AA RID: 38570 RVA: 0x0038A9D4 File Offset: 0x00388BD4
		public bool ImageLoaded
		{
			get
			{
				return this.image != null;
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x060096AB RID: 38571 RVA: 0x0038A9E2 File Offset: 0x00388BE2
		public Texture2D Image
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x060096AC RID: 38572 RVA: 0x0038A9EA File Offset: 0x00388BEA
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

		// Token: 0x060096AD RID: 38573 RVA: 0x0038AA24 File Offset: 0x00388C24
		[PublicizedFrom(EAccessModifier.Private)]
		public void setImage(Texture2D _image)
		{
			this.image = _image;
			this.image.name = "NewsImage_" + this.imageRelPath;
			this.image.Compress(true);
			this.image.Apply(false, true);
			this.owner.Owner.notifyListeners();
		}

		// Token: 0x060096AE RID: 38574 RVA: 0x0038AA7C File Offset: 0x00388C7C
		public bool Equals(NewsManager.NewsEntry _other)
		{
			return _other != null && (this == _other || (this.CustomListName == _other.CustomListName && this.imageRelPath == _other.imageRelPath && this.Headline == _other.Headline && this.Headline2 == _other.Headline2 && this.Text == _other.Text && this.Url == _other.Url && this.Date.Equals(_other.Date)));
		}

		// Token: 0x060096AF RID: 38575 RVA: 0x0038AB1C File Offset: 0x00388D1C
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((NewsManager.NewsEntry)_obj)));
		}

		// Token: 0x060096B0 RID: 38576 RVA: 0x0038AB4C File Offset: 0x00388D4C
		public override int GetHashCode()
		{
			return (((((((((((((this.CustomListName != null) ? this.CustomListName.GetHashCode() : 0) * 397) ^ ((this.imageRelPath != null) ? this.imageRelPath.GetHashCode() : 0)) * 397) ^ ((this.Headline != null) ? this.Headline.GetHashCode() : 0)) * 397) ^ ((this.Headline2 != null) ? this.Headline2.GetHashCode() : 0)) * 397) ^ ((this.Text != null) ? this.Text.GetHashCode() : 0)) * 397) ^ ((this.Url != null) ? this.Url.GetHashCode() : 0)) * 397) ^ this.Date.GetHashCode();
		}

		// Token: 0x060096B1 RID: 38577 RVA: 0x0038AC18 File Offset: 0x00388E18
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

		// Token: 0x040070D8 RID: 28888
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NewsManager.NewsSource owner;

		// Token: 0x040070D9 RID: 28889
		public readonly string CustomListName;

		// Token: 0x040070DA RID: 28890
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string imageRelPath;

		// Token: 0x040070DB RID: 28891
		public readonly string Headline;

		// Token: 0x040070DC RID: 28892
		public readonly string Headline2;

		// Token: 0x040070DD RID: 28893
		public readonly string Text;

		// Token: 0x040070DE RID: 28894
		public readonly string Url;

		// Token: 0x040070DF RID: 28895
		public readonly DateTime Date;

		// Token: 0x040070E0 RID: 28896
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requestedImage;

		// Token: 0x040070E1 RID: 28897
		[PublicizedFrom(EAccessModifier.Private)]
		public Texture2D image;
	}
}
