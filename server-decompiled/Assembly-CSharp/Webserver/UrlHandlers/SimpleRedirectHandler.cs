using System;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B53 RID: 6995
	public class SimpleRedirectHandler : AbsHandler
	{
		// Token: 0x0600D206 RID: 53766 RVA: 0x004C161C File Offset: 0x004BF81C
		public SimpleRedirectHandler(string _target)
			: base(null, 0)
		{
			this.target = _target;
		}

		// Token: 0x0600D207 RID: 53767 RVA: 0x004C162D File Offset: 0x004BF82D
		public override void HandleRequest(RequestContext _context)
		{
			_context.Response.Redirect(this.target);
		}

		// Token: 0x04009F5E RID: 40798
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string target;
	}
}
