using System;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B22 RID: 6946
	public class SimpleRedirectHandler : AbsHandler
	{
		// Token: 0x0600D0F2 RID: 53490 RVA: 0x004BDB20 File Offset: 0x004BBD20
		public SimpleRedirectHandler(string _target)
			: base(null, 0)
		{
			this.target = _target;
		}

		// Token: 0x0600D0F3 RID: 53491 RVA: 0x004BDB31 File Offset: 0x004BBD31
		public override void HandleRequest(RequestContext _context)
		{
			_context.Response.Redirect(this.target);
		}

		// Token: 0x04009E75 RID: 40565
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string target;
	}
}
