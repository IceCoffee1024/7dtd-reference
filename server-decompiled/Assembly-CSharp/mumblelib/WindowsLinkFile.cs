using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x02001685 RID: 5765
	[PublicizedFrom(EAccessModifier.Internal)]
	public class WindowsLinkFile : ILinkFile, IDisposable
	{
		// Token: 0x0600B425 RID: 46117 RVA: 0x0043313C File Offset: 0x0043133C
		public unsafe WindowsLinkFile()
		{
			this.memoryMappedFile = MemoryMappedFile.CreateOrOpen("MumbleLink", (long)Marshal.SizeOf<WindowsLinkFile.WindowsLinkMemory>());
			byte* ptr = null;
			this.memoryMappedFile.CreateViewAccessor().SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
			this.ptr = (WindowsLinkFile.WindowsLinkMemory*)ptr;
		}

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x0600B426 RID: 46118 RVA: 0x00433186 File Offset: 0x00431386
		// (set) Token: 0x0600B427 RID: 46119 RVA: 0x00433193 File Offset: 0x00431393
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

		// Token: 0x0600B428 RID: 46120 RVA: 0x004331A1 File Offset: 0x004313A1
		public unsafe void Tick()
		{
			this.ptr->uiTick += 1U;
		}

		// Token: 0x170015AD RID: 5549
		// (set) Token: 0x0600B429 RID: 46121 RVA: 0x004331B3 File Offset: 0x004313B3
		public unsafe Vector3 AvatarPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarPosition.FixedElementField, value);
			}
		}

		// Token: 0x170015AE RID: 5550
		// (set) Token: 0x0600B42A RID: 46122 RVA: 0x004331CC File Offset: 0x004313CC
		public unsafe Vector3 AvatarForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarFront.FixedElementField, value);
			}
		}

		// Token: 0x170015AF RID: 5551
		// (set) Token: 0x0600B42B RID: 46123 RVA: 0x004331E5 File Offset: 0x004313E5
		public unsafe Vector3 AvatarTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarTop.FixedElementField, value);
			}
		}

		// Token: 0x170015B0 RID: 5552
		// (set) Token: 0x0600B42C RID: 46124 RVA: 0x004331FE File Offset: 0x004313FE
		public unsafe string Name
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->name.FixedElementField, value, 256, Encoding.Unicode);
			}
		}

		// Token: 0x170015B1 RID: 5553
		// (set) Token: 0x0600B42D RID: 46125 RVA: 0x00433221 File Offset: 0x00431421
		public unsafe Vector3 CameraPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraPosition.FixedElementField, value);
			}
		}

		// Token: 0x170015B2 RID: 5554
		// (set) Token: 0x0600B42E RID: 46126 RVA: 0x0043323A File Offset: 0x0043143A
		public unsafe Vector3 CameraForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraFront.FixedElementField, value);
			}
		}

		// Token: 0x170015B3 RID: 5555
		// (set) Token: 0x0600B42F RID: 46127 RVA: 0x00433253 File Offset: 0x00431453
		public unsafe Vector3 CameraTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraTop.FixedElementField, value);
			}
		}

		// Token: 0x170015B4 RID: 5556
		// (set) Token: 0x0600B430 RID: 46128 RVA: 0x0043326C File Offset: 0x0043146C
		public unsafe string Identity
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->identity.FixedElementField, value, 256, Encoding.Unicode);
			}
		}

		// Token: 0x170015B5 RID: 5557
		// (set) Token: 0x0600B431 RID: 46129 RVA: 0x0043328F File Offset: 0x0043148F
		public unsafe string Context
		{
			set
			{
				Util.SetContext(&this.ptr->context.FixedElementField, &this.ptr->context_len, value);
			}
		}

		// Token: 0x170015B6 RID: 5558
		// (set) Token: 0x0600B432 RID: 46130 RVA: 0x004332B4 File Offset: 0x004314B4
		public unsafe string Description
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->description.FixedElementField, value, 2048, Encoding.Unicode);
			}
		}

		// Token: 0x0600B433 RID: 46131 RVA: 0x004332D7 File Offset: 0x004314D7
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600B434 RID: 46132 RVA: 0x004332E8 File Offset: 0x004314E8
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~WindowsLinkFile()
		{
			this.Dispose();
		}

		// Token: 0x0600B435 RID: 46133 RVA: 0x00433314 File Offset: 0x00431514
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

		// Token: 0x040086D5 RID: 34517
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MemoryMappedFile memoryMappedFile;

		// Token: 0x040086D6 RID: 34518
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe readonly WindowsLinkFile.WindowsLinkMemory* ptr;

		// Token: 0x040086D7 RID: 34519
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;

		// Token: 0x02001686 RID: 5766
		[PublicizedFrom(EAccessModifier.Private)]
		public struct WindowsLinkMemory
		{
			// Token: 0x040086D8 RID: 34520
			public uint uiVersion;

			// Token: 0x040086D9 RID: 34521
			public uint uiTick;

			// Token: 0x040086DA RID: 34522
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarPosition>e__FixedBuffer fAvatarPosition;

			// Token: 0x040086DB RID: 34523
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarFront>e__FixedBuffer fAvatarFront;

			// Token: 0x040086DC RID: 34524
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarTop>e__FixedBuffer fAvatarTop;

			// Token: 0x040086DD RID: 34525
			[FixedBuffer(typeof(ushort), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<name>e__FixedBuffer name;

			// Token: 0x040086DE RID: 34526
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraPosition>e__FixedBuffer fCameraPosition;

			// Token: 0x040086DF RID: 34527
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraFront>e__FixedBuffer fCameraFront;

			// Token: 0x040086E0 RID: 34528
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraTop>e__FixedBuffer fCameraTop;

			// Token: 0x040086E1 RID: 34529
			[FixedBuffer(typeof(ushort), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<identity>e__FixedBuffer identity;

			// Token: 0x040086E2 RID: 34530
			public uint context_len;

			// Token: 0x040086E3 RID: 34531
			[FixedBuffer(typeof(byte), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<context>e__FixedBuffer context;

			// Token: 0x040086E4 RID: 34532
			[FixedBuffer(typeof(ushort), 2048)]
			public WindowsLinkFile.WindowsLinkMemory.<description>e__FixedBuffer description;

			// Token: 0x02001687 RID: 5767
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 256)]
			public struct <context>e__FixedBuffer
			{
				// Token: 0x040086E5 RID: 34533
				public byte FixedElementField;
			}

			// Token: 0x02001688 RID: 5768
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 4096)]
			public struct <description>e__FixedBuffer
			{
				// Token: 0x040086E6 RID: 34534
				public ushort FixedElementField;
			}

			// Token: 0x02001689 RID: 5769
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarFront>e__FixedBuffer
			{
				// Token: 0x040086E7 RID: 34535
				public float FixedElementField;
			}

			// Token: 0x0200168A RID: 5770
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarPosition>e__FixedBuffer
			{
				// Token: 0x040086E8 RID: 34536
				public float FixedElementField;
			}

			// Token: 0x0200168B RID: 5771
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarTop>e__FixedBuffer
			{
				// Token: 0x040086E9 RID: 34537
				public float FixedElementField;
			}

			// Token: 0x0200168C RID: 5772
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraFront>e__FixedBuffer
			{
				// Token: 0x040086EA RID: 34538
				public float FixedElementField;
			}

			// Token: 0x0200168D RID: 5773
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraPosition>e__FixedBuffer
			{
				// Token: 0x040086EB RID: 34539
				public float FixedElementField;
			}

			// Token: 0x0200168E RID: 5774
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraTop>e__FixedBuffer
			{
				// Token: 0x040086EC RID: 34540
				public float FixedElementField;
			}

			// Token: 0x0200168F RID: 5775
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 512)]
			public struct <identity>e__FixedBuffer
			{
				// Token: 0x040086ED RID: 34541
				public ushort FixedElementField;
			}

			// Token: 0x02001690 RID: 5776
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 512)]
			public struct <name>e__FixedBuffer
			{
				// Token: 0x040086EE RID: 34542
				public ushort FixedElementField;
			}
		}
	}
}
