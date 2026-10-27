using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Platform;
using UnityEngine;

// Token: 0x020012A3 RID: 4771
public class NewsManager
{
	// Token: 0x17001235 RID: 4661
	// (get) Token: 0x06009751 RID: 38737 RVA: 0x003908D4 File Offset: 0x0038EAD4
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
	// (add) Token: 0x06009752 RID: 38738 RVA: 0x003908EC File Offset: 0x0038EAEC
	// (remove) Token: 0x06009753 RID: 38739 RVA: 0x00390924 File Offset: 0x0038EB24
	public event Action<NewsManager> Updated;

	// Token: 0x06009754 RID: 38740 RVA: 0x0039095C File Offset: 0x0038EB5C
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

	// Token: 0x06009755 RID: 38741 RVA: 0x003909BC File Offset: 0x0038EBBC
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

	// Token: 0x06009756 RID: 38742 RVA: 0x003909F4 File Offset: 0x0038EBF4
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

	// Token: 0x06009757 RID: 38743 RVA: 0x00390A60 File Offset: 0x0038EC60
	[PublicizedFrom(EAccessModifier.Private)]
	public void sortNewsByAge(List<NewsManager.NewsEntry> _list)
	{
		_list.Sort((NewsManager.NewsEntry _entryA, NewsManager.NewsEntry _entryB) => _entryB.Date.CompareTo(_entryA.Date));
	}

	// Token: 0x06009758 RID: 38744 RVA: 0x00390A87 File Offset: 0x0038EC87
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

	// Token: 0x0400715A RID: 29018
	[PublicizedFrom(EAccessModifier.Private)]
	public static NewsManager instance;

	// Token: 0x0400715B RID: 29019
	public static readonly NewsManager.NewsEntry EmptyEntry = new NewsManager.NewsEntry(null, null, null, "- No Entries -", null, "", null, DateTime.Now);

	// Token: 0x0400715C RID: 29020
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, NewsManager.NewsSource> sources = new CaseInsensitiveStringDictionary<NewsManager.NewsSource>();

	// Token: 0x020012A4 RID: 4772
	public abstract class NewsSource
	{
		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x0600975B RID: 38747
		public abstract bool IsCustom { get; }

		// Token: 0x0600975C RID: 38748 RVA: 0x00390ACD File Offset: 0x0038ECCD
		[PublicizedFrom(EAccessModifier.Protected)]
		public NewsSource(NewsManager _owner, string _uri)
		{
			this.Owner = _owner;
			this.OrigUri = _uri;
		}

		// Token: 0x0600975D RID: 38749 RVA: 0x00390AF0 File Offset: 0x0038ECF0
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

		// Token: 0x0600975E RID: 38750
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator GetDataCo();

		// Token: 0x0600975F RID: 38751 RVA: 0x00390B88 File Offset: 0x0038ED88
		public void GetData(List<NewsManager.NewsEntry> _target)
		{
			NewsManager owner = this.Owner;
			lock (owner)
			{
				_target.AddRange(this.entries);
			}
		}

		// Token: 0x06009760 RID: 38752
		public abstract void RequestImage(string _imageRelPath, Action<Texture2D> _callback);

		// Token: 0x06009761 RID: 38753 RVA: 0x00390BD0 File Offset: 0x0038EDD0
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

		// Token: 0x06009762 RID: 38754 RVA: 0x00390CF8 File Offset: 0x0038EEF8
		public static NewsManager.NewsSource FromUri(NewsManager _owner, string _uri)
		{
			if (_uri.StartsWith("rfs://"))
			{
				return new NewsManager.NewsSourceRfs(_owner, _uri);
			}
			return new NewsManager.NewsSourceWww(_owner, _uri);
		}

		// Token: 0x0400715E RID: 29022
		[PublicizedFrom(EAccessModifier.Protected)]
		public const string RemoteFileStorageProtocol = "rfs://";

		// Token: 0x0400715F RID: 29023
		public readonly NewsManager Owner;

		// Token: 0x04007160 RID: 29024
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string OrigUri;

		// Token: 0x04007161 RID: 29025
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isUpdating;

		// Token: 0x04007162 RID: 29026
		[PublicizedFrom(EAccessModifier.Protected)]
		public DateTime lastUpdated;

		// Token: 0x04007163 RID: 29027
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<NewsManager.NewsEntry> entries = new List<NewsManager.NewsEntry>();
	}

	// Token: 0x020012A5 RID: 4773
	[PublicizedFrom(EAccessModifier.Private)]
	public class NewsSourceRfs : NewsManager.NewsSource
	{
		// Token: 0x06009763 RID: 38755 RVA: 0x00390D16 File Offset: 0x0038EF16
		public NewsSourceRfs(NewsManager _owner, string _uri)
			: base(_owner, _uri)
		{
			this.rfsFilename = _uri.Substring("rfs://".Length);
		}

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06009764 RID: 38756 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool IsCustom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009765 RID: 38757 RVA: 0x00390D41 File Offset: 0x0038EF41
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

		// Token: 0x06009766 RID: 38758 RVA: 0x00390D50 File Offset: 0x0038EF50
		public override void RequestImage(string _imageRelPath, Action<Texture2D> _callback)
		{
			Queue<ValueTuple<string, Action<Texture2D>>> queue = this.requestedImagesQueue;
			lock (queue)
			{
				this.requestedImagesQueue.Enqueue(new ValueTuple<string, Action<Texture2D>>(_imageRelPath, _callback));
			}
			this.startNextImageRequest();
		}

		// Token: 0x06009767 RID: 38759 RVA: 0x00390DA4 File Offset: 0x0038EFA4
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

		// Token: 0x06009768 RID: 38760 RVA: 0x00390E10 File Offset: 0x0038F010
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

		// Token: 0x06009769 RID: 38761 RVA: 0x00390E60 File Offset: 0x0038F060
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

		// Token: 0x0600976A RID: 38762 RVA: 0x00390E80 File Offset: 0x0038F080
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

		// Token: 0x04007164 RID: 29028
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string rfsFilename;

		// Token: 0x04007165 RID: 29029
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxParallelImageRequests = 3;

		// Token: 0x04007166 RID: 29030
		[TupleElementNames(new string[] { "imageRelPath", "callback" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Queue<ValueTuple<string, Action<Texture2D>>> requestedImagesQueue = new Queue<ValueTuple<string, Action<Texture2D>>>();

		// Token: 0x04007167 RID: 29031
		[PublicizedFrom(EAccessModifier.Private)]
		public int runningImageRequests;
	}

	// Token: 0x020012A9 RID: 4777
	[PublicizedFrom(EAccessModifier.Private)]
	public class NewsSourceWww : NewsManager.NewsSource
	{
		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06009779 RID: 38777 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool IsCustom
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600977A RID: 38778 RVA: 0x003911D4 File Offset: 0x0038F3D4
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

		// Token: 0x0600977B RID: 38779 RVA: 0x0039127F File Offset: 0x0038F47F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator GetDataCo()
		{
			NewsManager.NewsSourceWww.<GetDataCo>d__5 <GetDataCo>d__ = new NewsManager.NewsSourceWww.<GetDataCo>d__5(0);
			<GetDataCo>d__.<>4__this = this;
			return <GetDataCo>d__;
		}

		// Token: 0x0600977C RID: 38780 RVA: 0x0039128E File Offset: 0x0038F48E
		public override void RequestImage(string _imageRelPath, Action<Texture2D> _callback)
		{
			ThreadManager.StartCoroutine(this.requestFromUri(_imageRelPath, _callback));
		}

		// Token: 0x0600977D RID: 38781 RVA: 0x0039129E File Offset: 0x0038F49E
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator requestFromUri(string _imageRelPath, Action<Texture2D> _callback)
		{
			NewsManager.NewsSourceWww.<requestFromUri>d__7 <requestFromUri>d__ = new NewsManager.NewsSourceWww.<requestFromUri>d__7(0);
			<requestFromUri>d__.<>4__this = this;
			<requestFromUri>d__._imageRelPath = _imageRelPath;
			<requestFromUri>d__._callback = _callback;
			return <requestFromUri>d__;
		}

		// Token: 0x04007176 RID: 29046
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string patchedUri;

		// Token: 0x04007177 RID: 29047
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string baseUri;
	}

	// Token: 0x020012AC RID: 4780
	public class NewsEntry : IEquatable<NewsManager.NewsEntry>
	{
		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x0600978C RID: 38796 RVA: 0x003915AC File Offset: 0x0038F7AC
		public bool IsCustom
		{
			get
			{
				return this.owner == null || this.owner.IsCustom;
			}
		}

		// Token: 0x0600978D RID: 38797 RVA: 0x003915C4 File Offset: 0x0038F7C4
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

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x0600978E RID: 38798 RVA: 0x00391614 File Offset: 0x0038F814
		public bool HasImage
		{
			get
			{
				return !string.IsNullOrEmpty(this.imageRelPath);
			}
		}

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x0600978F RID: 38799 RVA: 0x00391624 File Offset: 0x0038F824
		public bool ImageLoaded
		{
			get
			{
				return this.image != null;
			}
		}

		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06009790 RID: 38800 RVA: 0x00391632 File Offset: 0x0038F832
		public Texture2D Image
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x06009791 RID: 38801 RVA: 0x0039163A File Offset: 0x0038F83A
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

		// Token: 0x06009792 RID: 38802 RVA: 0x00391674 File Offset: 0x0038F874
		[PublicizedFrom(EAccessModifier.Private)]
		public void setImage(Texture2D _image)
		{
			this.image = _image;
			this.image.name = "NewsImage_" + this.imageRelPath;
			this.image.Compress(true);
			this.image.Apply(false, true);
			this.owner.Owner.notifyListeners();
		}

		// Token: 0x06009793 RID: 38803 RVA: 0x003916CC File Offset: 0x0038F8CC
		public bool Equals(NewsManager.NewsEntry _other)
		{
			return _other != null && (this == _other || (this.CustomListName == _other.CustomListName && this.imageRelPath == _other.imageRelPath && this.Headline == _other.Headline && this.Headline2 == _other.Headline2 && this.Text == _other.Text && this.Url == _other.Url && this.Date.Equals(_other.Date)));
		}

		// Token: 0x06009794 RID: 38804 RVA: 0x0039176C File Offset: 0x0038F96C
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((NewsManager.NewsEntry)_obj)));
		}

		// Token: 0x06009795 RID: 38805 RVA: 0x0039179C File Offset: 0x0038F99C
		public override int GetHashCode()
		{
			return (((((((((((((this.CustomListName != null) ? this.CustomListName.GetHashCode() : 0) * 397) ^ ((this.imageRelPath != null) ? this.imageRelPath.GetHashCode() : 0)) * 397) ^ ((this.Headline != null) ? this.Headline.GetHashCode() : 0)) * 397) ^ ((this.Headline2 != null) ? this.Headline2.GetHashCode() : 0)) * 397) ^ ((this.Text != null) ? this.Text.GetHashCode() : 0)) * 397) ^ ((this.Url != null) ? this.Url.GetHashCode() : 0)) * 397) ^ this.Date.GetHashCode();
		}

		// Token: 0x06009796 RID: 38806 RVA: 0x00391868 File Offset: 0x0038FA68
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

		// Token: 0x04007183 RID: 29059
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NewsManager.NewsSource owner;

		// Token: 0x04007184 RID: 29060
		public readonly string CustomListName;

		// Token: 0x04007185 RID: 29061
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string imageRelPath;

		// Token: 0x04007186 RID: 29062
		public readonly string Headline;

		// Token: 0x04007187 RID: 29063
		public readonly string Headline2;

		// Token: 0x04007188 RID: 29064
		public readonly string Text;

		// Token: 0x04007189 RID: 29065
		public readonly string Url;

		// Token: 0x0400718A RID: 29066
		public readonly DateTime Date;

		// Token: 0x0400718B RID: 29067
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requestedImage;

		// Token: 0x0400718C RID: 29068
		[PublicizedFrom(EAccessModifier.Private)]
		public Texture2D image;
	}
}
