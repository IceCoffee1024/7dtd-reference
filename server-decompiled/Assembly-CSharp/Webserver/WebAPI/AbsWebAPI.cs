using System;
using Webserver.Permissions;

namespace Webserver.WebAPI
{
	// Token: 0x02001AF8 RID: 6904
	public abstract class AbsWebAPI
	{
		// Token: 0x0600D004 RID: 53252 RVA: 0x004B8169 File Offset: 0x004B6369
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsWebAPI(string _name = null)
			: this(null, _name)
		{
		}

		// Token: 0x0600D005 RID: 53253 RVA: 0x004B8174 File Offset: 0x004B6374
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsWebAPI(Web _parentWeb, string _name = null)
		{
			this.Name = _name ?? base.GetType().Name;
			this.ParentWeb = _parentWeb;
			this.CachedApiModuleName = "webapi." + this.Name;
			this.RegisterPermissions();
		}

		// Token: 0x0600D006 RID: 53254 RVA: 0x004B81C0 File Offset: 0x004B63C0
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void RegisterPermissions()
		{
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(this.CachedApiModuleName, this.DefaultPermissionLevel(), true));
		}

		// Token: 0x0600D007 RID: 53255
		public abstract void HandleRequest(RequestContext _context);

		// Token: 0x0600D008 RID: 53256 RVA: 0x004B81DE File Offset: 0x004B63DE
		public virtual bool Authorized(RequestContext _context)
		{
			return AdminWebModules.Instance.GetModule(this.CachedApiModuleName).LevelGlobal >= _context.PermissionLevel;
		}

		// Token: 0x0600D009 RID: 53257 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int DefaultPermissionLevel()
		{
			return 0;
		}

		// Token: 0x04009D68 RID: 40296
		public readonly string Name;

		// Token: 0x04009D69 RID: 40297
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Web ParentWeb;

		// Token: 0x04009D6A RID: 40298
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string CachedApiModuleName;
	}
}
