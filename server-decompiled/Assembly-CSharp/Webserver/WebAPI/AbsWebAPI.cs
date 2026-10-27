using System;
using Webserver.Permissions;

namespace Webserver.WebAPI
{
	// Token: 0x02001B29 RID: 6953
	public abstract class AbsWebAPI
	{
		// Token: 0x0600D118 RID: 53528 RVA: 0x004BBC65 File Offset: 0x004B9E65
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsWebAPI(string _name = null)
			: this(null, _name)
		{
		}

		// Token: 0x0600D119 RID: 53529 RVA: 0x004BBC70 File Offset: 0x004B9E70
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsWebAPI(Web _parentWeb, string _name = null)
		{
			this.Name = _name ?? base.GetType().Name;
			this.ParentWeb = _parentWeb;
			this.CachedApiModuleName = "webapi." + this.Name;
			this.RegisterPermissions();
		}

		// Token: 0x0600D11A RID: 53530 RVA: 0x004BBCBC File Offset: 0x004B9EBC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void RegisterPermissions()
		{
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(this.CachedApiModuleName, this.DefaultPermissionLevel(), true));
		}

		// Token: 0x0600D11B RID: 53531
		public abstract void HandleRequest(RequestContext _context);

		// Token: 0x0600D11C RID: 53532 RVA: 0x004BBCDA File Offset: 0x004B9EDA
		public virtual bool Authorized(RequestContext _context)
		{
			return AdminWebModules.Instance.GetModule(this.CachedApiModuleName).LevelGlobal >= _context.PermissionLevel;
		}

		// Token: 0x0600D11D RID: 53533 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int DefaultPermissionLevel()
		{
			return 0;
		}

		// Token: 0x04009E51 RID: 40529
		public readonly string Name;

		// Token: 0x04009E52 RID: 40530
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Web ParentWeb;

		// Token: 0x04009E53 RID: 40531
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string CachedApiModuleName;
	}
}
