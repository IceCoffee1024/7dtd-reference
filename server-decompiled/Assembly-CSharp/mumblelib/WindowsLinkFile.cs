using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x020016AB RID: 5803
	[PublicizedFrom(EAccessModifier.Internal)]
	public class WindowsLinkFile : ILinkFile, IDisposable
	{
		// Token: 0x0600B5A8 RID: 46504 RVA: 0x00438EB4 File Offset: 0x004370B4
		public unsafe WindowsLinkFile()
		{
			this.memoryMappedFile = MemoryMappedFile.CreateOrOpen("MumbleLink", (long)Marshal.SizeOf<WindowsLinkFile.WindowsLinkMemory>());
			byte* ptr = null;
			this.memoryMappedFile.CreateViewAccessor().SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
			this.ptr = (WindowsLinkFile.WindowsLinkMemory*)ptr;
		}

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x0600B5A9 RID: 46505 RVA: 0x00438EFE File Offset: 0x004370FE
		// (set) Token: 0x0600B5AA RID: 46506 RVA: 0x00438F0B File Offset: 0x0043710B
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

		// Token: 0x0600B5AB RID: 46507 RVA: 0x00438F19 File Offset: 0x00437119
		public unsafe void Tick()
		{
			this.ptr->uiTick += 1U;
		}

		// Token: 0x1700161F RID: 5663
		// (set) Token: 0x0600B5AC RID: 46508 RVA: 0x00438F2B File Offset: 0x0043712B
		public unsafe Vector3 AvatarPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarPosition.FixedElementField, value);
			}
		}

		// Token: 0x17001620 RID: 5664
		// (set) Token: 0x0600B5AD RID: 46509 RVA: 0x00438F44 File Offset: 0x00437144
		public unsafe Vector3 AvatarForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarFront.FixedElementField, value);
			}
		}

		// Token: 0x17001621 RID: 5665
		// (set) Token: 0x0600B5AE RID: 46510 RVA: 0x00438F5D File Offset: 0x0043715D
		public unsafe Vector3 AvatarTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarTop.FixedElementField, value);
			}
		}

		// Token: 0x17001622 RID: 5666
		// (set) Token: 0x0600B5AF RID: 46511 RVA: 0x00438F76 File Offset: 0x00437176
		public unsafe string Name
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->name.FixedElementField, value, 256, Encoding.Unicode);
			}
		}

		// Token: 0x17001623 RID: 5667
		// (set) Token: 0x0600B5B0 RID: 46512 RVA: 0x00438F99 File Offset: 0x00437199
		public unsafe Vector3 CameraPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraPosition.FixedElementField, value);
			}
		}

		// Token: 0x17001624 RID: 5668
		// (set) Token: 0x0600B5B1 RID: 46513 RVA: 0x00438FB2 File Offset: 0x004371B2
		public unsafe Vector3 CameraForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraFront.FixedElementField, value);
			}
		}

		// Token: 0x17001625 RID: 5669
		// (set) Token: 0x0600B5B2 RID: 46514 RVA: 0x00438FCB File Offset: 0x004371CB
		public unsafe Vector3 CameraTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraTop.FixedElementField, value);
			}
		}

		// Token: 0x17001626 RID: 5670
		// (set) Token: 0x0600B5B3 RID: 46515 RVA: 0x00438FE4 File Offset: 0x004371E4
		public unsafe string Identity
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->identity.FixedElementField, value, 256, Encoding.Unicode);
			}
		}

		// Token: 0x17001627 RID: 5671
		// (set) Token: 0x0600B5B4 RID: 46516 RVA: 0x00439007 File Offset: 0x00437207
		public unsafe string Context
		{
			set
			{
				Util.SetContext(&this.ptr->context.FixedElementField, &this.ptr->context_len, value);
			}
		}

		// Token: 0x17001628 RID: 5672
		// (set) Token: 0x0600B5B5 RID: 46517 RVA: 0x0043902C File Offset: 0x0043722C
		public unsafe string Description
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->description.FixedElementField, value, 2048, Encoding.Unicode);
			}
		}

		// Token: 0x0600B5B6 RID: 46518 RVA: 0x0043904F File Offset: 0x0043724F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600B5B7 RID: 46519 RVA: 0x00439060 File Offset: 0x00437260
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~WindowsLinkFile()
		{
			this.Dispose();
		}

		// Token: 0x0600B5B8 RID: 46520 RVA: 0x0043908C File Offset: 0x0043728C
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

		// Token: 0x040087F1 RID: 34801
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MemoryMappedFile memoryMappedFile;

		// Token: 0x040087F2 RID: 34802
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe readonly WindowsLinkFile.WindowsLinkMemory* ptr;

		// Token: 0x040087F3 RID: 34803
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;

		// Token: 0x020016AC RID: 5804
		[PublicizedFrom(EAccessModifier.Private)]
		public struct WindowsLinkMemory
		{
			// Token: 0x040087F4 RID: 34804
			public uint uiVersion;

			// Token: 0x040087F5 RID: 34805
			public uint uiTick;

			// Token: 0x040087F6 RID: 34806
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarPosition>e__FixedBuffer fAvatarPosition;

			// Token: 0x040087F7 RID: 34807
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarFront>e__FixedBuffer fAvatarFront;

			// Token: 0x040087F8 RID: 34808
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarTop>e__FixedBuffer fAvatarTop;

			// Token: 0x040087F9 RID: 34809
			[FixedBuffer(typeof(ushort), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<name>e__FixedBuffer name;

			// Token: 0x040087FA RID: 34810
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraPosition>e__FixedBuffer fCameraPosition;

			// Token: 0x040087FB RID: 34811
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraFront>e__FixedBuffer fCameraFront;

			// Token: 0x040087FC RID: 34812
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraTop>e__FixedBuffer fCameraTop;

			// Token: 0x040087FD RID: 34813
			[FixedBuffer(typeof(ushort), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<identity>e__FixedBuffer identity;

			// Token: 0x040087FE RID: 34814
			public uint context_len;

			// Token: 0x040087FF RID: 34815
			[FixedBuffer(typeof(byte), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<context>e__FixedBuffer context;

			// Token: 0x04008800 RID: 34816
			[FixedBuffer(typeof(ushort), 2048)]
			public WindowsLinkFile.WindowsLinkMemory.<description>e__FixedBuffer description;

			// Token: 0x020016AD RID: 5805
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 256)]
			public struct <context>e__FixedBuffer
			{
				// Token: 0x04008801 RID: 34817
				public byte FixedElementField;
			}

			// Token: 0x020016AE RID: 5806
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 4096)]
			public struct <description>e__FixedBuffer
			{
				// Token: 0x04008802 RID: 34818
				public ushort FixedElementField;
			}

			// Token: 0x020016AF RID: 5807
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarFront>e__FixedBuffer
			{
				// Token: 0x04008803 RID: 34819
				public float FixedElementField;
			}

			// Token: 0x020016B0 RID: 5808
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarPosition>e__FixedBuffer
			{
				// Token: 0x04008804 RID: 34820
				public float FixedElementField;
			}

			// Token: 0x020016B1 RID: 5809
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarTop>e__FixedBuffer
			{
				// Token: 0x04008805 RID: 34821
				public float FixedElementField;
			}

			// Token: 0x020016B2 RID: 5810
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraFront>e__FixedBuffer
			{
				// Token: 0x04008806 RID: 34822
				public float FixedElementField;
			}

			// Token: 0x020016B3 RID: 5811
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraPosition>e__FixedBuffer
			{
				// Token: 0x04008807 RID: 34823
				public float FixedElementField;
			}

			// Token: 0x020016B4 RID: 5812
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraTop>e__FixedBuffer
			{
				// Token: 0x04008808 RID: 34824
				public float FixedElementField;
			}

			// Token: 0x020016B5 RID: 5813
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 512)]
			public struct <identity>e__FixedBuffer
			{
				// Token: 0x04008809 RID: 34825
				public ushort FixedElementField;
			}

			// Token: 0x020016B6 RID: 5814
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 512)]
			public struct <name>e__FixedBuffer
			{
				// Token: 0x0400880A RID: 34826
				public ushort FixedElementField;
			}
		}
	}
}
