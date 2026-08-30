using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine.Bindings;

namespace Unity.Content
{
	// Token: 0x0200008B RID: 139
	[StaticAccessor("GetContentNamespaceManager()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Misc/ContentNamespace.h")]
	public struct ContentNamespace
	{
		// Token: 0x06000299 RID: 665 RVA: 0x00004C60 File Offset: 0x00002E60
		public string GetName()
		{
			this.ThrowIfInvalidNamespace();
			return ContentNamespace.GetNamespaceName(this);
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00004C84 File Offset: 0x00002E84
		public bool IsValid
		{
			get
			{
				return ContentNamespace.IsNamespaceHandleValid(this);
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00004CA4 File Offset: 0x00002EA4
		public void Delete()
		{
			bool flag = this.Id == ContentNamespace.s_Default.Id;
			if (flag)
			{
				throw new InvalidOperationException("Cannot delete the default namespace.");
			}
			this.ThrowIfInvalidNamespace();
			ContentNamespace.RemoveNamespace(this);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00004CE8 File Offset: 0x00002EE8
		private void ThrowIfInvalidNamespace()
		{
			bool flag = !this.IsValid;
			if (flag)
			{
				throw new InvalidOperationException("The provided namespace is invalid. Did you already delete it?");
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00004D10 File Offset: 0x00002F10
		public static ContentNamespace Default
		{
			get
			{
				bool flag = !ContentNamespace.s_defaultInitialized;
				if (flag)
				{
					ContentNamespace.s_defaultInitialized = true;
					ContentNamespace.s_Default = ContentNamespace.GetOrCreateNamespace("default");
				}
				return ContentNamespace.s_Default;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00004D4C File Offset: 0x00002F4C
		public static ContentNamespace GetOrCreateNamespace(string name)
		{
			bool flag = ContentNamespace.s_ValidName.IsMatch(name);
			if (flag)
			{
				return ContentNamespace.GetOrCreate(name);
			}
			throw new InvalidOperationException("Namespace name can only contain alphanumeric characters and a maximum length of 16 characters.");
		}

		// Token: 0x0600029F RID: 671
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ContentNamespace[] GetAll();

		// Token: 0x060002A0 RID: 672 RVA: 0x00004D80 File Offset: 0x00002F80
		internal static ContentNamespace GetOrCreate(string name)
		{
			ContentNamespace contentNamespace;
			ContentNamespace.GetOrCreate_Injected(name, out contentNamespace);
			return contentNamespace;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00004D96 File Offset: 0x00002F96
		internal static void RemoveNamespace(ContentNamespace ns)
		{
			ContentNamespace.RemoveNamespace_Injected(ref ns);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00004D9F File Offset: 0x00002F9F
		internal static string GetNamespaceName(ContentNamespace ns)
		{
			return ContentNamespace.GetNamespaceName_Injected(ref ns);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00004DA8 File Offset: 0x00002FA8
		internal static bool IsNamespaceHandleValid(ContentNamespace ns)
		{
			return ContentNamespace.IsNamespaceHandleValid_Injected(ref ns);
		}

		// Token: 0x060002A5 RID: 677
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetOrCreate_Injected(string name, out ContentNamespace ret);

		// Token: 0x060002A6 RID: 678
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveNamespace_Injected(ref ContentNamespace ns);

		// Token: 0x060002A7 RID: 679
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetNamespaceName_Injected(ref ContentNamespace ns);

		// Token: 0x060002A8 RID: 680
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsNamespaceHandleValid_Injected(ref ContentNamespace ns);

		// Token: 0x0400020E RID: 526
		internal ulong Id;

		// Token: 0x0400020F RID: 527
		private static bool s_defaultInitialized = false;

		// Token: 0x04000210 RID: 528
		private static ContentNamespace s_Default;

		// Token: 0x04000211 RID: 529
		private static Regex s_ValidName = new Regex("^[a-zA-Z0-9]{1,16}$", RegexOptions.Compiled);
	}
}
