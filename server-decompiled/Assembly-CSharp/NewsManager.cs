using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Platform;
using UnityEngine;

// Token: 0x02001291 RID: 4753
public class NewsManager
{
	// Token: 0x17001229 RID: 4649
	// (get) Token: 0x060096F9 RID: 38649 RVA: 0x0038E4CC File Offset: 0x0038C6CC
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
	// (add) Token: 0x060096FA RID: 38650 RVA: 0x0038E4E4 File Offset: 0x0038C6E4
	// (remove) Token: 0x060096FB RID: 38651 RVA: 0x0038E51C File Offset: 0x0038C71C
	public event Action<NewsManager> Updated;

	// Token: 0x060096FC RID: 38652 RVA: 0x0038E554 File Offset: 0x0038C754
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

	// Token: 0x060096FD RID: 38653 RVA: 0x0038E5B4 File Offset: 0x0038C7B4
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

	// Token: 0x060096FE RID: 38654 RVA: 0x0038E5EC File Offset: 0x0038C7EC
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

	// Token: 0x060096FF RID: 38655 RVA: 0x0038E658 File Offset: 0x0038C858
	[PublicizedFrom(EAccessModifier.Private)]
	public void sortNewsByAge(List<NewsManager.NewsEntry> _list)
	{
		_list.Sort((NewsManager.NewsEntry _entryA, NewsManager.NewsEntry _entryB) => _entryB.Date.CompareTo(_entryA.Date));
	}

	// Token: 0x06009700 RID: 38656 RVA: 0x0038E67F File Offset: 0x0038C87F
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

	// Token: 0x04007119 RID: 28953
	[PublicizedFrom(EAccessModifier.Private)]
	public static NewsManager instance;

	// Token: 0x0400711A RID: 28954
	public static readonly NewsManager.NewsEntry EmptyEntry = new NewsManager.NewsEntry(null, null, null, "- No Entries -", null, "", null, DateTime.Now);

	// Token: 0x0400711B RID: 28955
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, NewsManager.NewsSource> sources = new CaseInsensitiveStringDictionary<NewsManager.NewsSource>();

	// Token: 0x02001292 RID: 4754
	public abstract class NewsSource
	{
		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06009703 RID: 38659
		public abstract bool IsCustom { get; }

		// Token: 0x06009704 RID: 38660 RVA: 0x0038E6C5 File Offset: 0x0038C8C5
		[PublicizedFrom(EAccessModifier.Protected)]
		public NewsSource(NewsManager _owner, string _uri)
		{
			this.Owner = _owner;
			this.OrigUri = _uri;
		}

		// Token: 0x06009705 RID: 38661 RVA: 0x0038E6E8 File Offset: 0x0038C8E8
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

		// Token: 0x06009706 RID: 38662
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator GetDataCo();

		// Token: 0x06009707 RID: 38663 RVA: 0x0038E780 File Offset: 0x0038C980
		public void GetData(List<NewsManager.NewsEntry> _target)
		{
			NewsManager owner = this.Owner;
			lock (owner)
			{
				_target.AddRange(this.entries);
			}
		}

		// Token: 0x06009708 RID: 38664
		public abstract void RequestImage(string _imageRelPath, Action<Texture2D> _callback);

		// Token: 0x06009709 RID: 38665 RVA: 0x0038E7C8 File Offset: 0x0038C9C8
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

		// Token: 0x0600970A RID: 38666 RVA: 0x0038E8F0 File Offset: 0x0038CAF0
		public static NewsManager.NewsSource FromUri(NewsManager _owner, string _uri)
		{
			if (_uri.StartsWith("rfs://"))
			{
				return new NewsManager.NewsSourceRfs(_owner, _uri);
			}
			return new NewsManager.NewsSourceWww(_owner, _uri);
		}

		// Token: 0x0400711D RID: 28957
		[PublicizedFrom(EAccessModifier.Protected)]
		public const string RemoteFileStorageProtocol = "rfs://";

		// Token: 0x0400711E RID: 28958
		public readonly NewsManager Owner;

		// Token: 0x0400711F RID: 28959
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string OrigUri;

		// Token: 0x04007120 RID: 28960
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isUpdating;

		// Token: 0x04007121 RID: 28961
		[PublicizedFrom(EAccessModifier.Protected)]
		public DateTime lastUpdated;

		// Token: 0x04007122 RID: 28962
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<NewsManager.NewsEntry> entries = new List<NewsManager.NewsEntry>();
	}

	// Token: 0x02001293 RID: 4755
	[PublicizedFrom(EAccessModifier.Private)]
	public class NewsSourceRfs : NewsManager.NewsSource
	{
		// Token: 0x0600970B RID: 38667 RVA: 0x0038E90E File Offset: 0x0038CB0E
		public NewsSourceRfs(NewsManager _owner, string _uri)
			: base(_owner, _uri)
		{
			this.rfsFilename = _uri.Substring("rfs://".Length);
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x0600970C RID: 38668 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool IsCustom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600970D RID: 38669 RVA: 0x0038E939 File Offset: 0x0038CB39
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

		// Token: 0x0600970E RID: 38670 RVA: 0x0038E948 File Offset: 0x0038CB48
		public override void RequestImage(string _imageRelPath, Action<Texture2D> _callback)
		{
			Queue<ValueTuple<string, Action<Texture2D>>> queue = this.requestedImagesQueue;
			lock (queue)
			{
				this.requestedImagesQueue.Enqueue(new ValueTuple<string, Action<Texture2D>>(_imageRelPath, _callback));
			}
			this.startNextImageRequest();
		}

		// Token: 0x0600970F RID: 38671 RVA: 0x0038E99C File Offset: 0x0038CB9C
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

		// Token: 0x06009710 RID: 38672 RVA: 0x0038EA08 File Offset: 0x0038CC08
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

		// Token: 0x06009711 RID: 38673 RVA: 0x0038EA58 File Offset: 0x0038CC58
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

		// Token: 0x06009712 RID: 38674 RVA: 0x0038EA78 File Offset: 0x0038CC78
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

		// Token: 0x04007123 RID: 28963
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string rfsFilename;

		// Token: 0x04007124 RID: 28964
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxParallelImageRequests = 3;

		// Token: 0x04007125 RID: 28965
		[TupleElementNames(new string[] { "imageRelPath", "callback" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Queue<ValueTuple<string, Action<Texture2D>>> requestedImagesQueue = new Queue<ValueTuple<string, Action<Texture2D>>>();

		// Token: 0x04007126 RID: 28966
		[PublicizedFrom(EAccessModifier.Private)]
		public int runningImageRequests;
	}

	// Token: 0x02001297 RID: 4759
	[PublicizedFrom(EAccessModifier.Private)]
	public class NewsSourceWww : NewsManager.NewsSource
	{
		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06009721 RID: 38689 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool IsCustom
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009722 RID: 38690 RVA: 0x0038EDCC File Offset: 0x0038CFCC
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

		// Token: 0x06009723 RID: 38691 RVA: 0x0038EE77 File Offset: 0x0038D077
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator GetDataCo()
		{
			NewsManager.NewsSourceWww.<GetDataCo>d__5 <GetDataCo>d__ = new NewsManager.NewsSourceWww.<GetDataCo>d__5(0);
			<GetDataCo>d__.<>4__this = this;
			return <GetDataCo>d__;
		}

		// Token: 0x06009724 RID: 38692 RVA: 0x0038EE86 File Offset: 0x0038D086
		public override void RequestImage(string _imageRelPath, Action<Texture2D> _callback)
		{
			ThreadManager.StartCoroutine(this.requestFromUri(_imageRelPath, _callback));
		}

		// Token: 0x06009725 RID: 38693 RVA: 0x0038EE96 File Offset: 0x0038D096
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator requestFromUri(string _imageRelPath, Action<Texture2D> _callback)
		{
			NewsManager.NewsSourceWww.<requestFromUri>d__7 <requestFromUri>d__ = new NewsManager.NewsSourceWww.<requestFromUri>d__7(0);
			<requestFromUri>d__.<>4__this = this;
			<requestFromUri>d__._imageRelPath = _imageRelPath;
			<requestFromUri>d__._callback = _callback;
			return <requestFromUri>d__;
		}

		// Token: 0x04007135 RID: 28981
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string patchedUri;

		// Token: 0x04007136 RID: 28982
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string baseUri;
	}

	// Token: 0x0200129A RID: 4762
	public class NewsEntry : IEquatable<NewsManager.NewsEntry>
	{
		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06009734 RID: 38708 RVA: 0x0038F1A4 File Offset: 0x0038D3A4
		public bool IsCustom
		{
			get
			{
				return this.owner == null || this.owner.IsCustom;
			}
		}

		// Token: 0x06009735 RID: 38709 RVA: 0x0038F1BC File Offset: 0x0038D3BC
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

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06009736 RID: 38710 RVA: 0x0038F20C File Offset: 0x0038D40C
		public bool HasImage
		{
			get
			{
				return !string.IsNullOrEmpty(this.imageRelPath);
			}
		}

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06009737 RID: 38711 RVA: 0x0038F21C File Offset: 0x0038D41C
		public bool ImageLoaded
		{
			get
			{
				return this.image != null;
			}
		}

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06009738 RID: 38712 RVA: 0x0038F22A File Offset: 0x0038D42A
		public Texture2D Image
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x06009739 RID: 38713 RVA: 0x0038F232 File Offset: 0x0038D432
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

		// Token: 0x0600973A RID: 38714 RVA: 0x0038F26C File Offset: 0x0038D46C
		[PublicizedFrom(EAccessModifier.Private)]
		public void setImage(Texture2D _image)
		{
			this.image = _image;
			this.image.name = "NewsImage_" + this.imageRelPath;
			this.image.Compress(true);
			this.image.Apply(false, true);
			this.owner.Owner.notifyListeners();
		}

		// Token: 0x0600973B RID: 38715 RVA: 0x0038F2C4 File Offset: 0x0038D4C4
		public bool Equals(NewsManager.NewsEntry _other)
		{
			return _other != null && (this == _other || (this.CustomListName == _other.CustomListName && this.imageRelPath == _other.imageRelPath && this.Headline == _other.Headline && this.Headline2 == _other.Headline2 && this.Text == _other.Text && this.Url == _other.Url && this.Date.Equals(_other.Date)));
		}

		// Token: 0x0600973C RID: 38716 RVA: 0x0038F364 File Offset: 0x0038D564
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((NewsManager.NewsEntry)_obj)));
		}

		// Token: 0x0600973D RID: 38717 RVA: 0x0038F394 File Offset: 0x0038D594
		public override int GetHashCode()
		{
			return (((((((((((((this.CustomListName != null) ? this.CustomListName.GetHashCode() : 0) * 397) ^ ((this.imageRelPath != null) ? this.imageRelPath.GetHashCode() : 0)) * 397) ^ ((this.Headline != null) ? this.Headline.GetHashCode() : 0)) * 397) ^ ((this.Headline2 != null) ? this.Headline2.GetHashCode() : 0)) * 397) ^ ((this.Text != null) ? this.Text.GetHashCode() : 0)) * 397) ^ ((this.Url != null) ? this.Url.GetHashCode() : 0)) * 397) ^ this.Date.GetHashCode();
		}

		// Token: 0x0600973E RID: 38718 RVA: 0x0038F460 File Offset: 0x0038D660
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

		// Token: 0x04007142 RID: 28994
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NewsManager.NewsSource owner;

		// Token: 0x04007143 RID: 28995
		public readonly string CustomListName;

		// Token: 0x04007144 RID: 28996
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string imageRelPath;

		// Token: 0x04007145 RID: 28997
		public readonly string Headline;

		// Token: 0x04007146 RID: 28998
		public readonly string Headline2;

		// Token: 0x04007147 RID: 28999
		public readonly string Text;

		// Token: 0x04007148 RID: 29000
		public readonly string Url;

		// Token: 0x04007149 RID: 29001
		public readonly DateTime Date;

		// Token: 0x0400714A RID: 29002
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requestedImage;

		// Token: 0x0400714B RID: 29003
		[PublicizedFrom(EAccessModifier.Private)]
		public Texture2D image;
	}
}
