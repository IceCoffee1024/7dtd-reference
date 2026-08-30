using System;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001ACD RID: 6861
	public class RewriteHandler : AbsHandler
	{
		// Token: 0x0600CE08 RID: 52744 RVA: 0x004B2CD0 File Offset: 0x004B0ED0
		public RewriteHandler(string _target, bool _fixedTarget = false)
			: base(null, 0)
		{
			this.target = _target;
			this.fixedTarget = _fixedTarget;
		}

		// Token: 0x0600CE09 RID: 52745 RVA: 0x004B2CE8 File Offset: 0x004B0EE8
		public override void HandleRequest(RequestContext _context)
		{
			_context.RequestPath = (this.fixedTarget ? this.target : (this.target + _context.RequestPath.Remove(0, this.urlBasePath.Length)));
			this.parent.ApplyPathHandler(_context);
		}

		// Token: 0x04009C74 RID: 40052
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string target;

		// Token: 0x04009C75 RID: 40053
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool fixedTarget;
	}
}
