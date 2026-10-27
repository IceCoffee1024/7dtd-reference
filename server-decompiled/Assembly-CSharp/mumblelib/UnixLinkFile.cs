using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x020016EA RID: 5866
	[PublicizedFrom(EAccessModifier.Internal)]
	public class UnixLinkFile : ILinkFile, IDisposable
	{
		// Token: 0x0600B7BD RID: 47037
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int shm_open([MarshalAs(UnmanagedType.LPStr)] string name, int oflag, uint mode);

		// Token: 0x0600B7BE RID: 47038
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern uint getuid();

		// Token: 0x0600B7BF RID: 47039
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int ftruncate(int fd, long length);

		// Token: 0x0600B7C0 RID: 47040
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public unsafe static extern void* mmap(void* addr, long length, int prot, int flags, int fd, long off);

		// Token: 0x0600B7C1 RID: 47041
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public unsafe static extern void* munmap(void* addr, long length);

		// Token: 0x0600B7C2 RID: 47042
		[PublicizedFrom(EAccessModifier.Private)]
		[DllImport("libc")]
		public static extern int close(int fd);

		// Token: 0x0600B7C3 RID: 47043 RVA: 0x0043FC24 File Offset: 0x0043DE24
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

		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x0600B7C4 RID: 47044 RVA: 0x0043FCCB File Offset: 0x0043DECB
		// (set) Token: 0x0600B7C5 RID: 47045 RVA: 0x0043FCD8 File Offset: 0x0043DED8
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

		// Token: 0x0600B7C6 RID: 47046 RVA: 0x0043FCE6 File Offset: 0x0043DEE6
		public unsafe void Tick()
		{
			this.ptr->uiTick += 1U;
		}

		// Token: 0x170016FD RID: 5885
		// (set) Token: 0x0600B7C7 RID: 47047 RVA: 0x0043FCF8 File Offset: 0x0043DEF8
		public unsafe Vector3 AvatarPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarPosition.FixedElementField, value);
			}
		}

		// Token: 0x170016FE RID: 5886
		// (set) Token: 0x0600B7C8 RID: 47048 RVA: 0x0043FD11 File Offset: 0x0043DF11
		public unsafe Vector3 AvatarForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarFront.FixedElementField, value);
			}
		}

		// Token: 0x170016FF RID: 5887
		// (set) Token: 0x0600B7C9 RID: 47049 RVA: 0x0043FD2A File Offset: 0x0043DF2A
		public unsafe Vector3 AvatarTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarTop.FixedElementField, value);
			}
		}

		// Token: 0x17001700 RID: 5888
		// (set) Token: 0x0600B7CA RID: 47050 RVA: 0x0043FD43 File Offset: 0x0043DF43
		public unsafe string Name
		{
			set
			{
				Util.SetString<uint>(&this.ptr->name.FixedElementField, value, 256, Encoding.UTF32);
			}
		}

		// Token: 0x17001701 RID: 5889
		// (set) Token: 0x0600B7CB RID: 47051 RVA: 0x0043FD66 File Offset: 0x0043DF66
		public unsafe Vector3 CameraPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraPosition.FixedElementField, value);
			}
		}

		// Token: 0x17001702 RID: 5890
		// (set) Token: 0x0600B7CC RID: 47052 RVA: 0x0043FD7F File Offset: 0x0043DF7F
		public unsafe Vector3 CameraForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraFront.FixedElementField, value);
			}
		}

		// Token: 0x17001703 RID: 5891
		// (set) Token: 0x0600B7CD RID: 47053 RVA: 0x0043FD98 File Offset: 0x0043DF98
		public unsafe Vector3 CameraTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraTop.FixedElementField, value);
			}
		}

		// Token: 0x17001704 RID: 5892
		// (set) Token: 0x0600B7CE RID: 47054 RVA: 0x0043FDB1 File Offset: 0x0043DFB1
		public unsafe string Identity
		{
			set
			{
				Util.SetString<uint>(&this.ptr->identity.FixedElementField, value, 256, Encoding.UTF32);
			}
		}

		// Token: 0x17001705 RID: 5893
		// (set) Token: 0x0600B7CF RID: 47055 RVA: 0x0043FDD4 File Offset: 0x0043DFD4
		public unsafe string Context
		{
			set
			{
				Util.SetContext(&this.ptr->context.FixedElementField, &this.ptr->context_len, value);
			}
		}

		// Token: 0x17001706 RID: 5894
		// (set) Token: 0x0600B7D0 RID: 47056 RVA: 0x0043FDF9 File Offset: 0x0043DFF9
		public unsafe string Description
		{
			set
			{
				Util.SetString<uint>(&this.ptr->description.FixedElementField, value, 2048, Encoding.UTF32);
			}
		}

		// Token: 0x0600B7D1 RID: 47057 RVA: 0x0043FE1C File Offset: 0x0043E01C
		public unsafe void Dispose()
		{
			if (!this.disposed)
			{
				UnixLinkFile.munmap((void*)this.ptr, (long)Marshal.SizeOf<UnixLinkFile.LinuxLinkMemory>());
				UnixLinkFile.close(this.fd);
				this.disposed = true;
			}
		}

		// Token: 0x0600B7D2 RID: 47058 RVA: 0x0043FE4C File Offset: 0x0043E04C
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~UnixLinkFile()
		{
			this.Dispose();
		}

		// Token: 0x04008942 RID: 35138
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_RDONLY = 0;

		// Token: 0x04008943 RID: 35139
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_WRONLY = 1;

		// Token: 0x04008944 RID: 35140
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_RDWR = 2;

		// Token: 0x04008945 RID: 35141
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_CREAT = 64;

		// Token: 0x04008946 RID: 35142
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_EXCL = 128;

		// Token: 0x04008947 RID: 35143
		[PublicizedFrom(EAccessModifier.Private)]
		public const int O_TRUNC = 512;

		// Token: 0x04008948 RID: 35144
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_READ = 1;

		// Token: 0x04008949 RID: 35145
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_WRITE = 2;

		// Token: 0x0400894A RID: 35146
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_EXEC = 4;

		// Token: 0x0400894B RID: 35147
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PROT_NONE = 0;

		// Token: 0x0400894C RID: 35148
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_SHARED = 1;

		// Token: 0x0400894D RID: 35149
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_PRIVATE = 2;

		// Token: 0x0400894E RID: 35150
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MAP_SHARED_VALIDATE = 3;

		// Token: 0x0400894F RID: 35151
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;

		// Token: 0x04008950 RID: 35152
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe readonly UnixLinkFile.LinuxLinkMemory* ptr;

		// Token: 0x04008951 RID: 35153
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int fd;

		// Token: 0x020016EB RID: 5867
		[PublicizedFrom(EAccessModifier.Private)]
		public struct LinuxLinkMemory
		{
			// Token: 0x04008952 RID: 35154
			public uint uiVersion;

			// Token: 0x04008953 RID: 35155
			public uint uiTick;

			// Token: 0x04008954 RID: 35156
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarPosition>e__FixedBuffer fAvatarPosition;

			// Token: 0x04008955 RID: 35157
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarFront>e__FixedBuffer fAvatarFront;

			// Token: 0x04008956 RID: 35158
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fAvatarTop>e__FixedBuffer fAvatarTop;

			// Token: 0x04008957 RID: 35159
			[FixedBuffer(typeof(uint), 256)]
			public UnixLinkFile.LinuxLinkMemory.<name>e__FixedBuffer name;

			// Token: 0x04008958 RID: 35160
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraPosition>e__FixedBuffer fCameraPosition;

			// Token: 0x04008959 RID: 35161
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraFront>e__FixedBuffer fCameraFront;

			// Token: 0x0400895A RID: 35162
			[FixedBuffer(typeof(float), 3)]
			public UnixLinkFile.LinuxLinkMemory.<fCameraTop>e__FixedBuffer fCameraTop;

			// Token: 0x0400895B RID: 35163
			[FixedBuffer(typeof(uint), 256)]
			public UnixLinkFile.LinuxLinkMemory.<identity>e__FixedBuffer identity;

			// Token: 0x0400895C RID: 35164
			public uint context_len;

			// Token: 0x0400895D RID: 35165
			[FixedBuffer(typeof(byte), 256)]
			public UnixLinkFile.LinuxLinkMemory.<context>e__FixedBuffer context;

			// Token: 0x0400895E RID: 35166
			[FixedBuffer(typeof(uint), 2048)]
			public UnixLinkFile.LinuxLinkMemory.<description>e__FixedBuffer description;

			// Token: 0x020016EC RID: 5868
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 256)]
			public struct <context>e__FixedBuffer
			{
				// Token: 0x0400895F RID: 35167
				public byte FixedElementField;
			}

			// Token: 0x020016ED RID: 5869
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 8192)]
			public struct <description>e__FixedBuffer
			{
				// Token: 0x04008960 RID: 35168
				public uint FixedElementField;
			}

			// Token: 0x020016EE RID: 5870
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarFront>e__FixedBuffer
			{
				// Token: 0x04008961 RID: 35169
				public float FixedElementField;
			}

			// Token: 0x020016EF RID: 5871
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarPosition>e__FixedBuffer
			{
				// Token: 0x04008962 RID: 35170
				public float FixedElementField;
			}

			// Token: 0x020016F0 RID: 5872
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarTop>e__FixedBuffer
			{
				// Token: 0x04008963 RID: 35171
				public float FixedElementField;
			}

			// Token: 0x020016F1 RID: 5873
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraFront>e__FixedBuffer
			{
				// Token: 0x04008964 RID: 35172
				public float FixedElementField;
			}

			// Token: 0x020016F2 RID: 5874
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraPosition>e__FixedBuffer
			{
				// Token: 0x04008965 RID: 35173
				public float FixedElementField;
			}

			// Token: 0x020016F3 RID: 5875
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraTop>e__FixedBuffer
			{
				// Token: 0x04008966 RID: 35174
				public float FixedElementField;
			}

			// Token: 0x020016F4 RID: 5876
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1024)]
			public struct <identity>e__FixedBuffer
			{
				// Token: 0x04008967 RID: 35175
				public uint FixedElementField;
			}

			// Token: 0x020016F5 RID: 5877
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1024)]
			public struct <name>e__FixedBuffer
			{
				// Token: 0x04008968 RID: 35176
				public uint FixedElementField;
			}
		}
	}
}
