using System;
using System.IO;
using Webserver.FileCache;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AF8 RID: 6904
	public class StaticHandler : AbsHandler
	{
		// Token: 0x0600CFA7 RID: 53159 RVA: 0x004B9AF8 File Offset: 0x004B7CF8
		public StaticHandler(string _filePath, AbstractCache _cache, bool _logMissingFiles, string _moduleName = null)
			: base(_moduleName, 0)
		{
			this.datapath = _filePath + ((_filePath[_filePath.Length - 1] == '/') ? "" : "/");
			this.cache = _cache;
			this.logMissingFiles = _logMissingFiles;
		}

		// Token: 0x0600CFA8 RID: 53160 RVA: 0x004B9B48 File Offset: 0x004B7D48
		public override void HandleRequest(RequestContext _context)
		{
			string text = _context.RequestPath.Remove(0, this.urlBasePath.Length);
			byte[] fileContent = this.cache.GetFileContent(this.datapath + text);
			if (fileContent != null)
			{
				_context.Response.ContentType = MimeType.GetMimeType(Path.GetExtension(text));
				_context.Response.ContentLength64 = (long)fileContent.Length;
				_context.Response.OutputStream.Write(fileContent, 0, fileContent.Length);
				return;
			}
			_context.Response.StatusCode = 404;
			if (this.logMissingFiles)
			{
				Log.Warning(string.Concat(new string[] { "[Web] Static: FileNotFound: \"", _context.RequestPath, "\" @ \"", this.datapath, text, "\"" }));
			}
		}

		// Token: 0x04009DB7 RID: 40375
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AbstractCache cache;

		// Token: 0x04009DB8 RID: 40376
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string datapath;

		// Token: 0x04009DB9 RID: 40377
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool logMissingFiles;
	}
}
