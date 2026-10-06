using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x020016CB RID: 5835
	[PublicizedFrom(EAccessModifier.Internal)]
	public class UnixLinkFile : ILinkFile, IDisposable
	{
		// Token: 0x0600B6E9 RID: 46825
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int shm_open([MarshalAs(UnmanagedType.LPStr)] string name, int oflag, uint mode);

		// Token: 0x0600B6EA RID: 46826
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern uint getuid();

		// Token: 0x0600B6EB RID: 46827
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int ftruncate(int fd, long length);

		// Token: 0x0600B6EC RID: 46828
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public unsafe static extern void* mmap(void* addr, long length, int prot, int flags, int fd, long off);

		// Token: 0x0600B6ED RID: 46829
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public unsafe static extern void* munmap(void* addr, long length);

		// Token: 0x0600B6EE RID: 46830
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int close(int fd);

		// Token: 0x0600B6EF RID: 46831 RVA: 0x0043D180 File Offset: 0x0043B380
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

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x0600B6F0 RID: 46832 RVA: 0x0043D227 File Offset: 0x0043B427
		// (set) Token: 0x0600B6F1 RID: 46833 RVA: 0x0043D234 File Offset: 0x0043B434
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

		// Token: 0x0600B6F2 RID: 46834 RVA: 0x0043D242 File Offset: 0x0043B442
		public unsafe void Tick()
		{
			this.ptr->uiTick += 1U;
		}

		// Token: 0x170016B6 RID: 5814
		// (set) Token: 0x0600B6F3 RID: 46835 RVA: 0x0043D254 File Offset: 0x0043B454
		public unsafe Vector3 AvatarPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarPosition.FixedElementField, value);
			}
		}

		// Token: 0x170016B7 RID: 5815
		// (set) Token: 0x0600B6F4 RID: 46836 RVA: 0x0043D26D File Offset: 0x0043B46D
		public unsafe Vector3 AvatarForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarFront.FixedElementField, value);
			}
		}

		// Token: 0x170016B8 RID: 5816
		// (set) Token: 0x0600B6F5 RID: 46837 RVA: 0x0043D286 File Offset: 0x0043B486
		public unsafe Vector3 AvatarTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarTop.FixedElementField, value);
			}
		}

		// Token: 0x170016B9 RID: 5817
		// (set) Token: 0x0600B6F6 RID: 46838 RVA: 0x0043D29F File Offset: 0x0043B49F
		public unsafe string Name
		{
			set
			{
				Util.SetString<uint>(&this.ptr->name.FixedElementField, value, 256, Encoding.UTF32);
			}
		}

		// Token: 0x170016BA RID: 5818
		// (set) Token: 0x0600B6F7 RID: 46839 RVA: 0x0043D2C2 File Offset: 0x0043B4C2
		public unsafe Vector3 CameraPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraPosition.FixedElementField, value);
			}
		}

		// Token: 0x170016BB RID: 5819
		// (set) Token: 0x0600B6F8 RID: 46840 RVA: 0x0043D2DB File Offset: 0x0043B4DB
		public unsafe Vector3 CameraForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraFront.FixedElementField, value);
			}
		}

		// Token: 0x170016BC RID: 5820
		// (set) Token: 0x0600B6F9 RID: 46841 RVA: 0x0043D2F4 File Offset: 0x0043B4F4
		public unsafe Vector3 CameraTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraTop.FixedElementField, value);
			}
		}

		// Token: 0x170016BD RID: 5821
		// (set) Token: 0x0600B6FA RID: 46842 RVA: 0x0043D30D File Offset: 0x0043B50D
		public unsafe string Identity
		{
			set
			{
				Util.SetString<uint>(&this.ptr->identity.FixedElementField, value, 256, Encoding.UTF32);
			}
		}

		// Token: 0x170016BE RID: 5822
		// (set) Token: 0x0600B6FB RID: 46843 RVA: 0x0043D330 File Offset: 0x0043B530
		public unsafe string Context
		{
			set
			{
				Util.SetContext(&this.ptr->context.FixedElementField, &this.ptr->context_len, value);
			}
		}

		// Token: 0x170016BF RID: 5823
		// (set) Token: 0x0600B6FC RID: 46844 RVA: 0x0043D355 File Offset: 0x0043B555
		public unsafe string Description
		{
			set
			{
				Util.SetString<uint>(&this.ptr->description.FixedElementField, value, 2048, Encoding.UTF32);
			}
		}

		// Token: 0x0600B6FD RID: 46845 RVA: 0x0043D378 File Offset: 0x0043B578
		public unsafe void Dispose()
		{
			if (!this.disposed)
			{
				UnixLinkFile.munmap((void*)this.ptr, (long)Marshal.SizeOf<UnixLinkFile.LinuxLinkMemory>());
				UnixLinkFile.close(this.fd);
				this.disposed = true;
			}
		}

		// Token: 0x0600B6FE RID: 46846 RVA: 0x0043D3A8 File Offset: 0x0043B5A8
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~UnixLinkFile()
		{
			this.Dispose();
		}

		// Token: 0x0400888F RID: 34959
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_RDONLY = 0;

		// Token: 0x04008890 RID: 34960
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_WRONLY = 1;

		// Token: 0x04008891 RID: 34961
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_RDWR = 2;

		// Token: 0x04008892 RID: 34962
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_CREAT = 64;

		// Token: 0x04008893 RID: 34963
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_EXCL = 128;

		// Token: 0x04008894 RID: 34964
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_TRUNC = 512;

		// Token: 0x04008895 RID: 34965
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_READ = 1;

		// Token: 0x04008896 RID: 34966
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_WRITE = 2;

		// Token: 0x04008897 RID: 34967
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_EXEC = 4;

		// Token: 0x04008898 RID: 34968
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_NONE = 0;

		// Token: 0x04008899 RID: 34969
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_SHARED = 1;

		// Token: 0x0400889A RID: 34970
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_PRIVATE = 2;

		// Token: 0x0400889B RID: 34971
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_SHARED_VALIDATE = 3;

		// Token: 0x0400889C RID: 34972
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;

		// Token: 0x0400889D RID: 34973
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe readonly UnixLinkFile.LinuxLinkMemory* ptr;

		// Token: 0x0400889E RID: 34974
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int fd;

		// Token: 0x020016CC RID: 5836
		[PublicizedFrom(EAccessModifier.Private)]
		public struct LinuxLinkMemory
		{
			// Token: 0x0400889F RID: 34975
			public uint uiVersion;

			// Token: 0x040088A0 RID: 34976
			public uint uiTick;

			// Token: 0x040088A1 RID: 34977
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarPosition>e__FixedBuffer fAvatarPosition;

			// Token: 0x040088A2 RID: 34978
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarFront>e__FixedBuffer fAvatarFront;

			// Token: 0x040088A3 RID: 34979
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarTop>e__FixedBuffer fAvatarTop;

			// Token: 0x040088A4 RID: 34980
			[FixedBuffer(typeof(uint), 256)]
			public UnixLinkFile.LinuxLinkMemory.<name>e__FixedBuffer name;

			// Token: 0x040088A5 RID: 34981
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraPosition>e__FixedBuffer fCameraPosition;

			// Token: 0x040088A6 RID: 34982
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraFront>e__FixedBuffer fCameraFront;

			// Token: 0x040088A7 RID: 34983
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraTop>e__FixedBuffer fCameraTop;

			// Token: 0x040088A8 RID: 34984
			[FixedBuffer(typeof(uint), 256)]
			public UnixLinkFile.LinuxLinkMemory.<identity>e__FixedBuffer identity;

			// Token: 0x040088A9 RID: 34985
			public uint context_len;

			// Token: 0x040088AA RID: 34986
			[FixedBuffer(typeof(byte), 256)]
			public UnixLinkFile.LinuxLinkMemory.<context>e__FixedBuffer context;

			// Token: 0x040088AB RID: 34987
			[FixedBuffer(typeof(uint), 2048)]
			public UnixLinkFile.LinuxLinkMemory.<description>e__FixedBuffer description;

			// Token: 0x020016CD RID: 5837
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 256)]
			public struct <context>e__FixedBuffer
			{
				// Token: 0x040088AC RID: 34988
				public byte FixedElementField;
			}

			// Token: 0x020016CE RID: 5838
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 8192)]
			public struct <description>e__FixedBuffer
			{
				// Token: 0x040088AD RID: 34989
				public uint FixedElementField;
			}

			// Token: 0x020016CF RID: 5839
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarFront>e__FixedBuffer
			{
				// Token: 0x040088AE RID: 34990
				public float FixedElementField;
			}

			// Token: 0x020016D0 RID: 5840
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarPosition>e__FixedBuffer
			{
				// Token: 0x040088AF RID: 34991
				public float FixedElementField;
			}

			// Token: 0x020016D1 RID: 5841
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarTop>e__FixedBuffer
			{
				// Token: 0x040088B0 RID: 34992
				public float FixedElementField;
			}

			// Token: 0x020016D2 RID: 5842
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraFront>e__FixedBuffer
			{
				// Token: 0x040088B1 RID: 34993
				public float FixedElementField;
			}

			// Token: 0x020016D3 RID: 5843
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraPosition>e__FixedBuffer
			{
				// Token: 0x040088B2 RID: 34994
				public float FixedElementField;
			}

			// Token: 0x020016D4 RID: 5844
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraTop>e__FixedBuffer
			{
				// Token: 0x040088B3 RID: 34995
				public float FixedElementField;
			}

			// Token: 0x020016D5 RID: 5845
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1024)]
			public struct <identity>e__FixedBuffer
			{
				// Token: 0x040088B4 RID: 34996
				public uint FixedElementField;
			}

			// Token: 0x020016D6 RID: 5846
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1024)]
			public struct <name>e__FixedBuffer
			{
				// Token: 0x040088B5 RID: 34997
				public uint FixedElementField;
			}
		}
	}
}
