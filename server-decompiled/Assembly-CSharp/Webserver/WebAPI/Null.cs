using System;
using System.Text;
using UnityEngine.Scripting;

namespace Webserver.WebAPI
{
	// Token: 0x02001AA8 RID: 6824
	[Preserve]
	public class Null : AbsWebAPI
	{
		// Token: 0x0600CD37 RID: 52535 RVA: 0x004ADD74 File Offset: 0x004ABF74
		public Null(string _name)
			: base(_name)
		{
		}

		// Token: 0x0600CD38 RID: 52536 RVA: 0x004ADD80 File Offset: 0x004ABF80
		public override void HandleRequest(RequestContext _context)
		{
			_context.Response.ContentLength64 = 0L;
			_context.Response.ContentType = "text/plain";
			_context.Response.ContentEncoding = Encoding.ASCII;
			_context.Response.OutputStream.Write(Array.Empty<byte>(), 0, 0);
		}
	}
}
