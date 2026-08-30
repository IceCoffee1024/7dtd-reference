using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Profiling;
using UnityEngine;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AC8 RID: 6856
	public class ItemIconHandler : AbsHandler
	{
		// Token: 0x0600CDE9 RID: 52713 RVA: 0x004B2060 File Offset: 0x004B0260
		public ItemIconHandler(bool _logMissingFiles, string _moduleName = null)
			: base(_moduleName, 0)
		{
			this.logMissingFiles = _logMissingFiles;
			ItemIconHandler.Instance = this;
		}

		// Token: 0x17001917 RID: 6423
		// (get) Token: 0x0600CDEA RID: 52714 RVA: 0x004B20C5 File Offset: 0x004B02C5
		// (set) Token: 0x0600CDEB RID: 52715 RVA: 0x004B20CC File Offset: 0x004B02CC
		public static ItemIconHandler Instance
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = null;

		// Token: 0x0600CDEC RID: 52716 RVA: 0x004B20D4 File Offset: 0x004B02D4
		public override void HandleRequest(RequestContext _context)
		{
			if (!this.loaded)
			{
				_context.Response.StatusCode = 500;
				Log.Out("[Web] IconHandler: Icons not loaded");
				return;
			}
			if (!_context.RequestPath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
			{
				_context.Response.StatusCode = 400;
				return;
			}
			string text = _context.RequestPath.Remove(0, this.urlBasePath.Length);
			int num = text.LastIndexOf('.');
			if (num < 0)
			{
				_context.Response.StatusCode = 400;
				return;
			}
			text = text.Remove(num);
			byte[] array;
			if (!this.icons.TryGetValue(text, out array))
			{
				_context.Response.StatusCode = 404;
				if (this.logMissingFiles)
				{
					Log.Out("[Web] IconHandler: FileNotFound: \"" + _context.RequestPath + "\" ");
				}
				return;
			}
			_context.Response.ContentType = MimeType.GetMimeType(".png");
			_context.Response.ContentLength64 = (long)array.Length;
			_context.Response.OutputStream.Write(array, 0, array.Length);
		}

		// Token: 0x0600CDED RID: 52717 RVA: 0x004B21E2 File Offset: 0x004B03E2
		public IEnumerator LoadIcons()
		{
			Dictionary<string, byte[]> dictionary = this.icons;
			lock (dictionary)
			{
				if (this.loading || this.loaded)
				{
					yield break;
				}
				this.loading = true;
				this.loadingMaxMsPerFrame = (GameManager.IsDedicatedServer ? 100 : 10);
				MicroStopwatch mswPerFrame = new MicroStopwatch(true);
				ItemIconHandler.LoadingStats stats = new ItemIconHandler.LoadingStats();
				ItemIconHandler.LoadingStats loadingStats = stats;
				if (loadingStats != null)
				{
					loadingStats.MswTotal.Start();
				}
				Dictionary<string, List<Color>> tintedIcons = new Dictionary<string, List<Color>>();
				foreach (ItemClass itemClass in ItemClass.list)
				{
					if (itemClass != null)
					{
						Color iconTint = itemClass.GetIconTint(null);
						if (!(iconTint == Color.white))
						{
							string iconName = itemClass.GetIconName();
							List<Color> list2;
							if (!tintedIcons.TryGetValue(iconName, out list2))
							{
								list2 = new List<Color>();
								tintedIcons.Add(iconName, list2);
							}
							list2.Add(iconTint);
						}
					}
				}
				yield return this.loadIconsFromFolder(GameIO.GetGameDir("Data/ItemIcons"), tintedIcons, stats, mswPerFrame);
				foreach (Mod mod in ModManager.GetLoadedMods())
				{
					string text = mod.Path + "/ItemIcons";
					yield return this.loadIconsFromFolder(text, tintedIcons, stats, mswPerFrame);
				}
				List<Mod>.Enumerator enumerator = default(List<Mod>.Enumerator);
				this.loaded = true;
				if (stats == null)
				{
					Log.Out(string.Format("[Web] IconHandler: Loaded {0} icons", this.icons.Count));
				}
				else
				{
					stats.MswTotal.Stop();
					Log.Out(string.Format("[Web] IconHandler: Loaded {0} icons ({1} source images with {2} tints applied)", this.icons.Count, stats.Files, stats.Tints));
					Log.Out(string.Format("[Web] IconHandler: Total time {0} ms, loading files {1} ms, tinting files {2} ms, encoding files {3} ms", new object[]
					{
						stats.MswTotal.ElapsedMilliseconds,
						stats.MswLoading.ElapsedMilliseconds,
						stats.MswTinting.ElapsedMilliseconds,
						stats.MswEncoding.ElapsedMilliseconds
					}));
					int num = 0;
					foreach (KeyValuePair<string, byte[]> keyValuePair in this.icons)
					{
						string text2;
						byte[] array;
						keyValuePair.Deconstruct(out text2, out array);
						byte[] array2 = array;
						num += array2.Length;
					}
					Log.Out(string.Format("[Web] IconHandler: Cached {0} KiB", num / 1024));
				}
				mswPerFrame = null;
				stats = null;
				tintedIcons = null;
			}
			dictionary = null;
			yield break;
			yield break;
		}

		// Token: 0x0600CDEE RID: 52718 RVA: 0x004B21F1 File Offset: 0x004B03F1
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator loadIconsFromFolder(string _path, Dictionary<string, List<Color>> _tintedIcons, ItemIconHandler.LoadingStats _stats, MicroStopwatch _mswPerFrame)
		{
			if (!Directory.Exists(_path))
			{
				yield break;
			}
			_mswPerFrame.ResetAndRestart();
			string[] array = Directory.GetFiles(_path);
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i];
				byte[] array2 = null;
				Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
				try
				{
					using (this.pmSourceFile.Auto())
					{
						if (!text.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
						{
							goto IL_01D0;
						}
						if (_stats != null)
						{
							_stats.MswLoading.Start();
						}
						array2 = File.ReadAllBytes(text);
						if (!tex.LoadImage(array2))
						{
							if (_stats != null)
							{
								_stats.MswLoading.Stop();
							}
							goto IL_01D0;
						}
						if (_stats != null)
						{
							_stats.MswLoading.Stop();
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error("[Web] Failed loading icon from " + _path);
					Log.Exception(ex);
				}
				goto IL_012D;
				IL_01D0:
				i++;
				continue;
				IL_012D:
				if (tex != null)
				{
					if (tex.width > 1)
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
						yield return this.AddIcon(fileNameWithoutExtension, array2, tex, _tintedIcons, _stats, _mswPerFrame);
					}
					global::UnityEngine.Object.Destroy(tex);
				}
				if (_mswPerFrame.ElapsedMilliseconds >= (long)this.loadingMaxMsPerFrame)
				{
					yield return null;
					_mswPerFrame.ResetAndRestart();
				}
				tex = null;
				goto IL_01D0;
			}
			array = null;
			yield break;
		}

		// Token: 0x0600CDEF RID: 52719 RVA: 0x004B221D File Offset: 0x004B041D
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator AddIcon(string _name, byte[] _sourceBytes, Texture2D _tex, Dictionary<string, List<Color>> _tintedIcons, ItemIconHandler.LoadingStats _stats, MicroStopwatch _mswPerFrame)
		{
			ItemIconHandler.<AddIcon>d__18 <AddIcon>d__ = new ItemIconHandler.<AddIcon>d__18(0);
			<AddIcon>d__.<>4__this = this;
			<AddIcon>d__._name = _name;
			<AddIcon>d__._sourceBytes = _sourceBytes;
			<AddIcon>d__._tex = _tex;
			<AddIcon>d__._tintedIcons = _tintedIcons;
			<AddIcon>d__._stats = _stats;
			<AddIcon>d__._mswPerFrame = _mswPerFrame;
			return <AddIcon>d__;
		}

		// Token: 0x04009C46 RID: 40006
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, byte[]> icons = new Dictionary<string, byte[]>();

		// Token: 0x04009C47 RID: 40007
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool logMissingFiles;

		// Token: 0x04009C48 RID: 40008
		[PublicizedFrom(EAccessModifier.Private)]
		public bool loaded;

		// Token: 0x04009C4A RID: 40010
		[PublicizedFrom(EAccessModifier.Private)]
		public int loadingMaxMsPerFrame = 100;

		// Token: 0x04009C4B RID: 40011
		[PublicizedFrom(EAccessModifier.Private)]
		public bool loading;

		// Token: 0x04009C4C RID: 40012
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ProfilerMarker pmSourceFile = new ProfilerMarker(".SourceFile");

		// Token: 0x04009C4D RID: 40013
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ProfilerMarker pmAddIcon = new ProfilerMarker(".AddIcon");

		// Token: 0x04009C4E RID: 40014
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ProfilerMarker pmTint = new ProfilerMarker(".Tint");

		// Token: 0x02001AC9 RID: 6857
		[PublicizedFrom(EAccessModifier.Private)]
		public class LoadingStats
		{
			// Token: 0x04009C4F RID: 40015
			public int Files;

			// Token: 0x04009C50 RID: 40016
			public int Tints;

			// Token: 0x04009C51 RID: 40017
			public readonly MicroStopwatch MswTotal = new MicroStopwatch(false);

			// Token: 0x04009C52 RID: 40018
			public readonly MicroStopwatch MswLoading = new MicroStopwatch(false);

			// Token: 0x04009C53 RID: 40019
			public readonly MicroStopwatch MswEncoding = new MicroStopwatch(false);

			// Token: 0x04009C54 RID: 40020
			public readonly MicroStopwatch MswTinting = new MicroStopwatch(false);
		}
	}
}
