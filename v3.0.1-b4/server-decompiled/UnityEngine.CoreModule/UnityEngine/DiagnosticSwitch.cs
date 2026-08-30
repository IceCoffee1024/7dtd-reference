using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200011B RID: 283
	[NativeClass("DiagnosticSwitch", "struct DiagnosticSwitch;")]
	[NativeHeader("Runtime/Utilities/DiagnosticSwitch.h")]
	[NativeAsStruct]
	[StructLayout(LayoutKind.Sequential)]
	internal class DiagnosticSwitch
	{
		// Token: 0x06000711 RID: 1809 RVA: 0x00009E2F File Offset: 0x0000802F
		private DiagnosticSwitch()
		{
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000712 RID: 1810
		public extern string name
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000713 RID: 1811
		public extern string description
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000714 RID: 1812
		[NativeName("OwningModuleName")]
		public extern string owningModule
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000715 RID: 1813
		public extern DiagnosticSwitch.Flags flags
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00009E39 File Offset: 0x00008039
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x00009E41 File Offset: 0x00008041
		public object value
		{
			get
			{
				return this.GetScriptingValue();
			}
			set
			{
				this.SetScriptingValue(value, false);
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000718 RID: 1816
		[NativeName("ScriptingDefaultValue")]
		public extern object defaultValue
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000719 RID: 1817
		[NativeName("ScriptingMinValue")]
		public extern object minValue
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600071A RID: 1818
		[NativeName("ScriptingMaxValue")]
		public extern object maxValue
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x00009E4C File Offset: 0x0000804C
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x00009E54 File Offset: 0x00008054
		public object persistentValue
		{
			get
			{
				return this.GetScriptingPersistentValue();
			}
			set
			{
				this.SetScriptingValue(value, true);
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600071D RID: 1821
		[NativeName("ScriptingEnumInfo")]
		public extern EnumInfo enumInfo
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600071E RID: 1822
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object GetScriptingValue();

		// Token: 0x0600071F RID: 1823
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object GetScriptingPersistentValue();

		// Token: 0x06000720 RID: 1824
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetScriptingValue(object value, bool setPersistent);

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00009E5F File Offset: 0x0000805F
		public bool isSetToDefault
		{
			get
			{
				return object.Equals(this.persistentValue, this.defaultValue);
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00009E72 File Offset: 0x00008072
		public bool needsRestart
		{
			get
			{
				return !object.Equals(this.value, this.persistentValue);
			}
		}

		// Token: 0x040003A3 RID: 931
		private IntPtr m_Ptr;

		// Token: 0x0200011C RID: 284
		[Flags]
		internal enum Flags
		{
			// Token: 0x040003A5 RID: 933
			None = 0,
			// Token: 0x040003A6 RID: 934
			CanChangeAfterEngineStart = 1,
			// Token: 0x040003A7 RID: 935
			PropagateToAssetImportWorkerProcess = 2
		}
	}
}
