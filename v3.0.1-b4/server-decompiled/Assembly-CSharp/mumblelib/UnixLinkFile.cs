using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x02001679 RID: 5753
	[PublicizedFrom(EAccessModifier.Internal)]
	public class UnixLinkFile : ILinkFile, IDisposable
	{
		// Token: 0x0600B40F RID: 46095
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int shm_open([MarshalAs(UnmanagedType.LPStr)] string name, int oflag, uint mode);

		// Token: 0x0600B410 RID: 46096
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern uint getuid();

		// Token: 0x0600B411 RID: 46097
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int ftruncate(int fd, long length);

		// Token: 0x0600B412 RID: 46098
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public unsafe static extern void* mmap(void* addr, long length, int prot, int flags, int fd, long off);

		// Token: 0x0600B413 RID: 46099
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public unsafe static extern void* munmap(void* addr, long length);

		// Token: 0x0600B414 RID: 46100
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int close(int fd);

		// Token: 0x0600B415 RID: 46101 RVA: 0x00432EE8 File Offset: 0x004310E8
		public unsafe UnixLinkFile()
		{
			this.fd = UnixLinkFile.shm_open("/MumbleLink." + UnixLinkFile.getuid().ToString(), 66, 384U);
			if (this.fd < 0)
			{
				throw new Exception("[MumbleLF] Failed to open shm");
			}
			Log.Out("[MumbleLF] FD opened");
			int num = Marshal.SizeOf<UnixLinkFile.LinuxLinkMemory>();
			if (UnixLinkFile.ftruncate(this.fd, (long)num) != 0)
			{
				Log.Error("[MumbleLF] Failed resizing shm");
				return;
			}
			Log.Out("[MumbleLF] Resized");
			this.ptr = (UnixLinkFile.LinuxLinkMemory*)UnixLinkFile.mmap(null, (long)num, 3, 1, this.fd, 0L);
			Log.Out("[MumbleLF] MemMapped");
		}

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x0600B416 RID: 46102 RVA: 0x00432F8F File Offset: 0x0043118F
		// (set) Token: 0x0600B417 RID: 46103 RVA: 0x00432F9C File Offset: 0x0043119C
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

		// Token: 0x0600B418 RID: 46104 RVA: 0x00432FAA File Offset: 0x004311AA
		public unsafe void Tick()
		{
			this.ptr->uiTick += 1U;
		}

		// Token: 0x170015A2 RID: 5538
		// (set) Token: 0x0600B419 RID: 46105 RVA: 0x00432FBC File Offset: 0x004311BC
		public unsafe Vector3 AvatarPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarPosition.FixedElementField, value);
			}
		}

		// Token: 0x170015A3 RID: 5539
		// (set) Token: 0x0600B41A RID: 46106 RVA: 0x00432FD5 File Offset: 0x004311D5
		public unsafe Vector3 AvatarForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarFront.FixedElementField, value);
			}
		}

		// Token: 0x170015A4 RID: 5540
		// (set) Token: 0x0600B41B RID: 46107 RVA: 0x00432FEE File Offset: 0x004311EE
		public unsafe Vector3 AvatarTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarTop.FixedElementField, value);
			}
		}

		// Token: 0x170015A5 RID: 5541
		// (set) Token: 0x0600B41C RID: 46108 RVA: 0x00433007 File Offset: 0x00431207
		public unsafe string Name
		{
			set
			{
				Util.SetString<uint>(&this.ptr->name.FixedElementField, value, 256, Encoding.UTF32);
			}
		}

		// Token: 0x170015A6 RID: 5542
		// (set) Token: 0x0600B41D RID: 46109 RVA: 0x0043302A File Offset: 0x0043122A
		public unsafe Vector3 CameraPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraPosition.FixedElementField, value);
			}
		}

		// Token: 0x170015A7 RID: 5543
		// (set) Token: 0x0600B41E RID: 46110 RVA: 0x00433043 File Offset: 0x00431243
		public unsafe Vector3 CameraForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraFront.FixedElementField, value);
			}
		}

		// Token: 0x170015A8 RID: 5544
		// (set) Token: 0x0600B41F RID: 46111 RVA: 0x0043305C File Offset: 0x0043125C
		public unsafe Vector3 CameraTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraTop.FixedElementField, value);
			}
		}

		// Token: 0x170015A9 RID: 5545
		// (set) Token: 0x0600B420 RID: 46112 RVA: 0x00433075 File Offset: 0x00431275
		public unsafe string Identity
		{
			set
			{
				Util.SetString<uint>(&this.ptr->identity.FixedElementField, value, 256, Encoding.UTF32);
			}
		}

		// Token: 0x170015AA RID: 5546
		// (set) Token: 0x0600B421 RID: 46113 RVA: 0x00433098 File Offset: 0x00431298
		public unsafe string Context
		{
			set
			{
				Util.SetContext(&this.ptr->context.FixedElementField, &this.ptr->context_len, value);
			}
		}

		// Token: 0x170015AB RID: 5547
		// (set) Token: 0x0600B422 RID: 46114 RVA: 0x004330BD File Offset: 0x004312BD
		public unsafe string Description
		{
			set
			{
				Util.SetString<uint>(&this.ptr->description.FixedElementField, value, 2048, Encoding.UTF32);
			}
		}

		// Token: 0x0600B423 RID: 46115 RVA: 0x004330E0 File Offset: 0x004312E0
		public unsafe void Dispose()
		{
			if (!this.disposed)
			{
				UnixLinkFile.munmap((void*)this.ptr, (long)Marshal.SizeOf<UnixLinkFile.LinuxLinkMemory>());
				UnixLinkFile.close(this.fd);
				this.disposed = true;
			}
		}

		// Token: 0x0600B424 RID: 46116 RVA: 0x00433110 File Offset: 0x00431310
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~UnixLinkFile()
		{
			this.Dispose();
		}

		// Token: 0x040086AE RID: 34478
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_RDONLY = 0;

		// Token: 0x040086AF RID: 34479
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_WRONLY = 1;

		// Token: 0x040086B0 RID: 34480
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_RDWR = 2;

		// Token: 0x040086B1 RID: 34481
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_CREAT = 64;

		// Token: 0x040086B2 RID: 34482
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_EXCL = 128;

		// Token: 0x040086B3 RID: 34483
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_TRUNC = 512;

		// Token: 0x040086B4 RID: 34484
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_READ = 1;

		// Token: 0x040086B5 RID: 34485
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_WRITE = 2;

		// Token: 0x040086B6 RID: 34486
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_EXEC = 4;

		// Token: 0x040086B7 RID: 34487
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_NONE = 0;

		// Token: 0x040086B8 RID: 34488
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_SHARED = 1;

		// Token: 0x040086B9 RID: 34489
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_PRIVATE = 2;

		// Token: 0x040086BA RID: 34490
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_SHARED_VALIDATE = 3;

		// Token: 0x040086BB RID: 34491
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;

		// Token: 0x040086BC RID: 34492
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe readonly UnixLinkFile.LinuxLinkMemory* ptr;

		// Token: 0x040086BD RID: 34493
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int fd;

		// Token: 0x0200167A RID: 5754
		[PublicizedFrom(EAccessModifier.Private)]
		public struct LinuxLinkMemory
		{
			// Token: 0x040086BE RID: 34494
			public uint uiVersion;

			// Token: 0x040086BF RID: 34495
			public uint uiTick;

			// Token: 0x040086C0 RID: 34496
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarPosition>e__FixedBuffer fAvatarPosition;

			// Token: 0x040086C1 RID: 34497
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarFront>e__FixedBuffer fAvatarFront;

			// Token: 0x040086C2 RID: 34498
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarTop>e__FixedBuffer fAvatarTop;

			// Token: 0x040086C3 RID: 34499
			[FixedBuffer(typeof(uint), 256)]
			public UnixLinkFile.LinuxLinkMemory.<name>e__FixedBuffer name;

			// Token: 0x040086C4 RID: 34500
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraPosition>e__FixedBuffer fCameraPosition;

			// Token: 0x040086C5 RID: 34501
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraFront>e__FixedBuffer fCameraFront;

			// Token: 0x040086C6 RID: 34502
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraTop>e__FixedBuffer fCameraTop;

			// Token: 0x040086C7 RID: 34503
			[FixedBuffer(typeof(uint), 256)]
			public UnixLinkFile.LinuxLinkMemory.<identity>e__FixedBuffer identity;

			// Token: 0x040086C8 RID: 34504
			public uint context_len;

			// Token: 0x040086C9 RID: 34505
			[FixedBuffer(typeof(byte), 256)]
			public UnixLinkFile.LinuxLinkMemory.<context>e__FixedBuffer context;

			// Token: 0x040086CA RID: 34506
			[FixedBuffer(typeof(uint), 2048)]
			public UnixLinkFile.LinuxLinkMemory.<description>e__FixedBuffer description;

			// Token: 0x0200167B RID: 5755
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 256)]
			public struct <context>e__FixedBuffer
			{
				// Token: 0x040086CB RID: 34507
				public byte FixedElementField;
			}

			// Token: 0x0200167C RID: 5756
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 8192)]
			public struct <description>e__FixedBuffer
			{
				// Token: 0x040086CC RID: 34508
				public uint FixedElementField;
			}

			// Token: 0x0200167D RID: 5757
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarFront>e__FixedBuffer
			{
				// Token: 0x040086CD RID: 34509
				public float FixedElementField;
			}

			// Token: 0x0200167E RID: 5758
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarPosition>e__FixedBuffer
			{
				// Token: 0x040086CE RID: 34510
				public float FixedElementField;
			}

			// Token: 0x0200167F RID: 5759
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarTop>e__FixedBuffer
			{
				// Token: 0x040086CF RID: 34511
				public float FixedElementField;
			}

			// Token: 0x02001680 RID: 5760
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraFront>e__FixedBuffer
			{
				// Token: 0x040086D0 RID: 34512
				public float FixedElementField;
			}

			// Token: 0x02001681 RID: 5761
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraPosition>e__FixedBuffer
			{
				// Token: 0x040086D1 RID: 34513
				public float FixedElementField;
			}

			// Token: 0x02001682 RID: 5762
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraTop>e__FixedBuffer
			{
				// Token: 0x040086D2 RID: 34514
				public float FixedElementField;
			}

			// Token: 0x02001683 RID: 5763
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1024)]
			public struct <identity>e__FixedBuffer
			{
				// Token: 0x040086D3 RID: 34515
				public uint FixedElementField;
			}

			// Token: 0x02001684 RID: 5764
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1024)]
			public struct <name>e__FixedBuffer
			{
				// Token: 0x040086D4 RID: 34516
				public uint FixedElementField;
			}
		}
	}
}
