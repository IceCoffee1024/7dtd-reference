using System;
using System.IO;
using Webserver.FileCache;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B55 RID: 6997
	public class StaticHandler : AbsHandler
	{
		// Token: 0x0600D213 RID: 53779 RVA: 0x004C1AC0 File Offset: 0x004BFCC0
		public StaticHandler(string _filePath, AbstractCache _cache, bool _logMissingFiles, string _moduleName = null)
			: base(_moduleName, 0)
		{
			this.datapath = _filePath + ((_filePath[_filePath.Length - 1] == '/') ? "" : "/");
			this.cache = _cache;
			this.logMissingFiles = _logMissingFiles;
		}

		// Token: 0x0600D214 RID: 53780 RVA: 0x004C1B10 File Offset: 0x004BFD10
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

		// Token: 0x04009F66 RID: 40806
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AbstractCache cache;

		// Token: 0x04009F67 RID: 40807
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string datapath;

		// Token: 0x04009F68 RID: 40808
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool logMissingFiles;
	}
}
