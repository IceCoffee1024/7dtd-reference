using System;
using System.Text;
using UnityEngine.Scripting;

namespace Webserver.WebAPI
{
	// Token: 0x02001ACF RID: 6863
	[Preserve]
	public class Null : AbsWebAPI
	{
		// Token: 0x0600CEC0 RID: 52928 RVA: 0x004B4168 File Offset: 0x004B2368
		public Null(string _name)
			: base(_name)
		{
		}

		// Token: 0x0600CEC1 RID: 52929 RVA: 0x004B4174 File Offset: 0x004B2374
		public override void HandleRequest(RequestContext _context)
		{
			_context.Response.ContentLength64 = 0L;
			_context.Response.ContentType = "text/plain";
			_context.Response.ContentEncoding = Encoding.ASCII;
			_context.Response.OutputStream.Write(Array.Empty<byte>(), 0, 0);
		}
	}
}
