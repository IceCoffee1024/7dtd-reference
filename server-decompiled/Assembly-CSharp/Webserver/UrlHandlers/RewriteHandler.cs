using System;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B51 RID: 6993
	public class RewriteHandler : AbsHandler
	{
		// Token: 0x0600D1FD RID: 53757 RVA: 0x004C108C File Offset: 0x004BF28C
		public RewriteHandler(string _target, bool _fixedTarget = false)
			: base(null, 0)
		{
			this.target = _target;
			this.fixedTarget = _fixedTarget;
		}

		// Token: 0x0600D1FE RID: 53758 RVA: 0x004C10A4 File Offset: 0x004BF2A4
		public override void HandleRequest(RequestContext _context)
		{
			_context.RequestPath = (this.fixedTarget ? this.target : (this.target + _context.RequestPath.Remove(0, this.urlBasePath.Length)));
			this.parent.ApplyPathHandler(_context);
		}

		// Token: 0x04009F54 RID: 40788
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string target;

		// Token: 0x04009F55 RID: 40789
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool fixedTarget;
	}
}
