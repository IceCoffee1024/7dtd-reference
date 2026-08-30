using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x0200169F RID: 5791
	[PublicizedFrom(EAccessModifier.Internal)]
	public class UnixLinkFile : ILinkFile, IDisposable
	{
		// Token: 0x0600B592 RID: 46482
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int shm_open([MarshalAs(UnmanagedType.LPStr)] string name, int oflag, uint mode);

		// Token: 0x0600B593 RID: 46483
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern uint getuid();

		// Token: 0x0600B594 RID: 46484
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int ftruncate(int fd, long length);

		// Token: 0x0600B595 RID: 46485
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public unsafe static extern void* mmap(void* addr, long length, int prot, int flags, int fd, long off);

		// Token: 0x0600B596 RID: 46486
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public unsafe static extern void* munmap(void* addr, long length);

		// Token: 0x0600B597 RID: 46487
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int close(int fd);

		// Token: 0x0600B598 RID: 46488 RVA: 0x00438C60 File Offset: 0x00436E60
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

		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x0600B599 RID: 46489 RVA: 0x00438D07 File Offset: 0x00436F07
		// (set) Token: 0x0600B59A RID: 46490 RVA: 0x00438D14 File Offset: 0x00436F14
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

		// Token: 0x0600B59B RID: 46491 RVA: 0x00438D22 File Offset: 0x00436F22
		public unsafe void Tick()
		{
			this.ptr->uiTick += 1U;
		}

		// Token: 0x17001614 RID: 5652
		// (set) Token: 0x0600B59C RID: 46492 RVA: 0x00438D34 File Offset: 0x00436F34
		public unsafe Vector3 AvatarPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarPosition.FixedElementField, value);
			}
		}

		// Token: 0x17001615 RID: 5653
		// (set) Token: 0x0600B59D RID: 46493 RVA: 0x00438D4D File Offset: 0x00436F4D
		public unsafe Vector3 AvatarForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarFront.FixedElementField, value);
			}
		}

		// Token: 0x17001616 RID: 5654
		// (set) Token: 0x0600B59E RID: 46494 RVA: 0x00438D66 File Offset: 0x00436F66
		public unsafe Vector3 AvatarTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarTop.FixedElementField, value);
			}
		}

		// Token: 0x17001617 RID: 5655
		// (set) Token: 0x0600B59F RID: 46495 RVA: 0x00438D7F File Offset: 0x00436F7F
		public unsafe string Name
		{
			set
			{
				Util.SetString<uint>(&this.ptr->name.FixedElementField, value, 256, Encoding.UTF32);
			}
		}

		// Token: 0x17001618 RID: 5656
		// (set) Token: 0x0600B5A0 RID: 46496 RVA: 0x00438DA2 File Offset: 0x00436FA2
		public unsafe Vector3 CameraPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraPosition.FixedElementField, value);
			}
		}

		// Token: 0x17001619 RID: 5657
		// (set) Token: 0x0600B5A1 RID: 46497 RVA: 0x00438DBB File Offset: 0x00436FBB
		public unsafe Vector3 CameraForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraFront.FixedElementField, value);
			}
		}

		// Token: 0x1700161A RID: 5658
		// (set) Token: 0x0600B5A2 RID: 46498 RVA: 0x00438DD4 File Offset: 0x00436FD4
		public unsafe Vector3 CameraTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraTop.FixedElementField, value);
			}
		}

		// Token: 0x1700161B RID: 5659
		// (set) Token: 0x0600B5A3 RID: 46499 RVA: 0x00438DED File Offset: 0x00436FED
		public unsafe string Identity
		{
			set
			{
				Util.SetString<uint>(&this.ptr->identity.FixedElementField, value, 256, Encoding.UTF32);
			}
		}

		// Token: 0x1700161C RID: 5660
		// (set) Token: 0x0600B5A4 RID: 46500 RVA: 0x00438E10 File Offset: 0x00437010
		public unsafe string Context
		{
			set
			{
				Util.SetContext(&this.ptr->context.FixedElementField, &this.ptr->context_len, value);
			}
		}

		// Token: 0x1700161D RID: 5661
		// (set) Token: 0x0600B5A5 RID: 46501 RVA: 0x00438E35 File Offset: 0x00437035
		public unsafe string Description
		{
			set
			{
				Util.SetString<uint>(&this.ptr->description.FixedElementField, value, 2048, Encoding.UTF32);
			}
		}

		// Token: 0x0600B5A6 RID: 46502 RVA: 0x00438E58 File Offset: 0x00437058
		public unsafe void Dispose()
		{
			if (!this.disposed)
			{
				UnixLinkFile.munmap((void*)this.ptr, (long)Marshal.SizeOf<UnixLinkFile.LinuxLinkMemory>());
				UnixLinkFile.close(this.fd);
				this.disposed = true;
			}
		}

		// Token: 0x0600B5A7 RID: 46503 RVA: 0x00438E88 File Offset: 0x00437088
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~UnixLinkFile()
		{
			this.Dispose();
		}

		// Token: 0x040087CA RID: 34762
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_RDONLY = 0;

		// Token: 0x040087CB RID: 34763
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_WRONLY = 1;

		// Token: 0x040087CC RID: 34764
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_RDWR = 2;

		// Token: 0x040087CD RID: 34765
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_CREAT = 64;

		// Token: 0x040087CE RID: 34766
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_EXCL = 128;

		// Token: 0x040087CF RID: 34767
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_TRUNC = 512;

		// Token: 0x040087D0 RID: 34768
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_READ = 1;

		// Token: 0x040087D1 RID: 34769
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_WRITE = 2;

		// Token: 0x040087D2 RID: 34770
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_EXEC = 4;

		// Token: 0x040087D3 RID: 34771
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_NONE = 0;

		// Token: 0x040087D4 RID: 34772
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_SHARED = 1;

		// Token: 0x040087D5 RID: 34773
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_PRIVATE = 2;

		// Token: 0x040087D6 RID: 34774
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_SHARED_VALIDATE = 3;

		// Token: 0x040087D7 RID: 34775
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;

		// Token: 0x040087D8 RID: 34776
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe readonly UnixLinkFile.LinuxLinkMemory* ptr;

		// Token: 0x040087D9 RID: 34777
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int fd;

		// Token: 0x020016A0 RID: 5792
		[PublicizedFrom(EAccessModifier.Private)]
		public struct LinuxLinkMemory
		{
			// Token: 0x040087DA RID: 34778
			public uint uiVersion;

			// Token: 0x040087DB RID: 34779
			public uint uiTick;

			// Token: 0x040087DC RID: 34780
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarPosition>e__FixedBuffer fAvatarPosition;

			// Token: 0x040087DD RID: 34781
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarFront>e__FixedBuffer fAvatarFront;

			// Token: 0x040087DE RID: 34782
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarTop>e__FixedBuffer fAvatarTop;

			// Token: 0x040087DF RID: 34783
			[FixedBuffer(typeof(uint), 256)]
			public UnixLinkFile.LinuxLinkMemory.<name>e__FixedBuffer name;

			// Token: 0x040087E0 RID: 34784
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraPosition>e__FixedBuffer fCameraPosition;

			// Token: 0x040087E1 RID: 34785
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraFront>e__FixedBuffer fCameraFront;

			// Token: 0x040087E2 RID: 34786
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraTop>e__FixedBuffer fCameraTop;

			// Token: 0x040087E3 RID: 34787
			[FixedBuffer(typeof(uint), 256)]
			public UnixLinkFile.LinuxLinkMemory.<identity>e__FixedBuffer identity;

			// Token: 0x040087E4 RID: 34788
			public uint context_len;

			// Token: 0x040087E5 RID: 34789
			[FixedBuffer(typeof(byte), 256)]
			public UnixLinkFile.LinuxLinkMemory.<context>e__FixedBuffer context;

			// Token: 0x040087E6 RID: 34790
			[FixedBuffer(typeof(uint), 2048)]
			public UnixLinkFile.LinuxLinkMemory.<description>e__FixedBuffer description;

			// Token: 0x020016A1 RID: 5793
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 256)]
			public struct <context>e__FixedBuffer
			{
				// Token: 0x040087E7 RID: 34791
				public byte FixedElementField;
			}

			// Token: 0x020016A2 RID: 5794
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 8192)]
			public struct <description>e__FixedBuffer
			{
				// Token: 0x040087E8 RID: 34792
				public uint FixedElementField;
			}

			// Token: 0x020016A3 RID: 5795
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarFront>e__FixedBuffer
			{
				// Token: 0x040087E9 RID: 34793
				public float FixedElementField;
			}

			// Token: 0x020016A4 RID: 5796
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarPosition>e__FixedBuffer
			{
				// Token: 0x040087EA RID: 34794
				public float FixedElementField;
			}

			// Token: 0x020016A5 RID: 5797
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarTop>e__FixedBuffer
			{
				// Token: 0x040087EB RID: 34795
				public float FixedElementField;
			}

			// Token: 0x020016A6 RID: 5798
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraFront>e__FixedBuffer
			{
				// Token: 0x040087EC RID: 34796
				public float FixedElementField;
			}

			// Token: 0x020016A7 RID: 5799
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraPosition>e__FixedBuffer
			{
				// Token: 0x040087ED RID: 34797
				public float FixedElementField;
			}

			// Token: 0x020016A8 RID: 5800
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraTop>e__FixedBuffer
			{
				// Token: 0x040087EE RID: 34798
				public float FixedElementField;
			}

			// Token: 0x020016A9 RID: 5801
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1024)]
			public struct <identity>e__FixedBuffer
			{
				// Token: 0x040087EF RID: 34799
				public uint FixedElementField;
			}

			// Token: 0x020016AA RID: 5802
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1024)]
			public struct <name>e__FixedBuffer
			{
				// Token: 0x040087F0 RID: 34800
				public uint FixedElementField;
			}
		}
	}
}
