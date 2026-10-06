using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x020016D7 RID: 5847
	[PublicizedFrom(EAccessModifier.Internal)]
	public class WindowsLinkFile : ILinkFile, IDisposable
	{
		// Token: 0x0600B6FF RID: 46847 RVA: 0x0043D3D4 File Offset: 0x0043B5D4
		public unsafe WindowsLinkFile()
		{
			this.memoryMappedFile = MemoryMappedFile.CreateOrOpen("MumbleLink", (long)Marshal.SizeOf<WindowsLinkFile.WindowsLinkMemory>());
			byte* ptr = null;
			this.memoryMappedFile.CreateViewAccessor().SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
			this.ptr = (WindowsLinkFile.WindowsLinkMemory*)ptr;
		}

		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x0600B700 RID: 46848 RVA: 0x0043D41E File Offset: 0x0043B61E
		// (set) Token: 0x0600B701 RID: 46849 RVA: 0x0043D42B File Offset: 0x0043B62B
		public unsafe uint UIVersion
		{
			get
			{
				return this.ptr->uiVersion;
			}
			set
			{
				this.ptr->uiVersion = value;
			}
		}

		// Token: 0x0600B702 RID: 46850 RVA: 0x0043D439 File Offset: 0x0043B639
		public unsafe void Tick()
		{
			this.ptr->uiTick += 1U;
		}

		// Token: 0x170016C1 RID: 5825
		// (set) Token: 0x0600B703 RID: 46851 RVA: 0x0043D44B File Offset: 0x0043B64B
		public unsafe Vector3 AvatarPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarPosition.FixedElementField, value);
			}
		}

		// Token: 0x170016C2 RID: 5826
		// (set) Token: 0x0600B704 RID: 46852 RVA: 0x0043D464 File Offset: 0x0043B664
		public unsafe Vector3 AvatarForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarFront.FixedElementField, value);
			}
		}

		// Token: 0x170016C3 RID: 5827
		// (set) Token: 0x0600B705 RID: 46853 RVA: 0x0043D47D File Offset: 0x0043B67D
		public unsafe Vector3 AvatarTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarTop.FixedElementField, value);
			}
		}

		// Token: 0x170016C4 RID: 5828
		// (set) Token: 0x0600B706 RID: 46854 RVA: 0x0043D496 File Offset: 0x0043B696
		public unsafe string Name
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->name.FixedElementField, value, 256, Encoding.Unicode);
			}
		}

		// Token: 0x170016C5 RID: 5829
		// (set) Token: 0x0600B707 RID: 46855 RVA: 0x0043D4B9 File Offset: 0x0043B6B9
		public unsafe Vector3 CameraPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraPosition.FixedElementField, value);
			}
		}

		// Token: 0x170016C6 RID: 5830
		// (set) Token: 0x0600B708 RID: 46856 RVA: 0x0043D4D2 File Offset: 0x0043B6D2
		public unsafe Vector3 CameraForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraFront.FixedElementField, value);
			}
		}

		// Token: 0x170016C7 RID: 5831
		// (set) Token: 0x0600B709 RID: 46857 RVA: 0x0043D4EB File Offset: 0x0043B6EB
		public unsafe Vector3 CameraTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraTop.FixedElementField, value);
			}
		}

		// Token: 0x170016C8 RID: 5832
		// (set) Token: 0x0600B70A RID: 46858 RVA: 0x0043D504 File Offset: 0x0043B704
		public unsafe string Identity
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->identity.FixedElementField, value, 256, Encoding.Unicode);
			}
		}

		// Token: 0x170016C9 RID: 5833
		// (set) Token: 0x0600B70B RID: 46859 RVA: 0x0043D527 File Offset: 0x0043B727
		public unsafe string Context
		{
			set
			{
				Util.SetContext(&this.ptr->context.FixedElementField, &this.ptr->context_len, value);
			}
		}

		// Token: 0x170016CA RID: 5834
		// (set) Token: 0x0600B70C RID: 46860 RVA: 0x0043D54C File Offset: 0x0043B74C
		public unsafe string Description
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->description.FixedElementField, value, 2048, Encoding.Unicode);
			}
		}

		// Token: 0x0600B70D RID: 46861 RVA: 0x0043D56F File Offset: 0x0043B76F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600B70E RID: 46862 RVA: 0x0043D580 File Offset: 0x0043B780
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~WindowsLinkFile()
		{
			this.Dispose();
		}

		// Token: 0x0600B70F RID: 46863 RVA: 0x0043D5AC File Offset: 0x0043B7AC
		[PublicizedFrom(EAccessModifier.Private)]
		public void Dispose(bool _disposing)
		{
			Log.Out("[MumbleLF] Disposing shm");
			if (!this.disposed)
			{
				if (_disposing)
				{
					this.memoryMappedFile.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x040088B6 RID: 34998
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MemoryMappedFile memoryMappedFile;

		// Token: 0x040088B7 RID: 34999
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe readonly WindowsLinkFile.WindowsLinkMemory* ptr;

		// Token: 0x040088B8 RID: 35000
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;

		// Token: 0x020016D8 RID: 5848
		[PublicizedFrom(EAccessModifier.Private)]
		public struct WindowsLinkMemory
		{
			// Token: 0x040088B9 RID: 35001
			public uint uiVersion;

			// Token: 0x040088BA RID: 35002
			public uint uiTick;

			// Token: 0x040088BB RID: 35003
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarPosition>e__FixedBuffer fAvatarPosition;

			// Token: 0x040088BC RID: 35004
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarFront>e__FixedBuffer fAvatarFront;

			// Token: 0x040088BD RID: 35005
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarTop>e__FixedBuffer fAvatarTop;

			// Token: 0x040088BE RID: 35006
			[FixedBuffer(typeof(ushort), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<name>e__FixedBuffer name;

			// Token: 0x040088BF RID: 35007
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraPosition>e__FixedBuffer fCameraPosition;

			// Token: 0x040088C0 RID: 35008
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraFront>e__FixedBuffer fCameraFront;

			// Token: 0x040088C1 RID: 35009
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraTop>e__FixedBuffer fCameraTop;

			// Token: 0x040088C2 RID: 35010
			[FixedBuffer(typeof(ushort), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<identity>e__FixedBuffer identity;

			// Token: 0x040088C3 RID: 35011
			public uint context_len;

			// Token: 0x040088C4 RID: 35012
			[FixedBuffer(typeof(byte), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<context>e__FixedBuffer context;

			// Token: 0x040088C5 RID: 35013
			[FixedBuffer(typeof(ushort), 2048)]
			public WindowsLinkFile.WindowsLinkMemory.<description>e__FixedBuffer description;

			// Token: 0x020016D9 RID: 5849
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 256)]
			public struct <context>e__FixedBuffer
			{
				// Token: 0x040088C6 RID: 35014
				public byte FixedElementField;
			}

			// Token: 0x020016DA RID: 5850
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 4096)]
			public struct <description>e__FixedBuffer
			{
				// Token: 0x040088C7 RID: 35015
				public ushort FixedElementField;
			}

			// Token: 0x020016DB RID: 5851
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarFront>e__FixedBuffer
			{
				// Token: 0x040088C8 RID: 35016
				public float FixedElementField;
			}

			// Token: 0x020016DC RID: 5852
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarPosition>e__FixedBuffer
			{
				// Token: 0x040088C9 RID: 35017
				public float FixedElementField;
			}

			// Token: 0x020016DD RID: 5853
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarTop>e__FixedBuffer
			{
				// Token: 0x040088CA RID: 35018
				public float FixedElementField;
			}

			// Token: 0x020016DE RID: 5854
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraFront>e__FixedBuffer
			{
				// Token: 0x040088CB RID: 35019
				public float FixedElementField;
			}

			// Token: 0x020016DF RID: 5855
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraPosition>e__FixedBuffer
			{
				// Token: 0x040088CC RID: 35020
				public float FixedElementField;
			}

			// Token: 0x020016E0 RID: 5856
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraTop>e__FixedBuffer
			{
				// Token: 0x040088CD RID: 35021
				public float FixedElementField;
			}

			// Token: 0x020016E1 RID: 5857
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 512)]
			public struct <identity>e__FixedBuffer
			{
				// Token: 0x040088CE RID: 35022
				public ushort FixedElementField;
			}

			// Token: 0x020016E2 RID: 5858
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 512)]
			public struct <name>e__FixedBuffer
			{
				// Token: 0x040088CF RID: 35023
				public ushort FixedElementField;
			}
		}
	}
}
