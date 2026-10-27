using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x020016F6 RID: 5878
	[PublicizedFrom(EAccessModifier.Internal)]
	public class WindowsLinkFile : ILinkFile, IDisposable
	{
		// Token: 0x0600B7D3 RID: 47059 RVA: 0x0043FE78 File Offset: 0x0043E078
		public unsafe WindowsLinkFile()
		{
			this.memoryMappedFile = MemoryMappedFile.CreateOrOpen("MumbleLink", (long)Marshal.SizeOf<WindowsLinkFile.WindowsLinkMemory>());
			byte* ptr = null;
			this.memoryMappedFile.CreateViewAccessor().SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
			this.ptr = (WindowsLinkFile.WindowsLinkMemory*)ptr;
		}

		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x0600B7D4 RID: 47060 RVA: 0x0043FEC2 File Offset: 0x0043E0C2
		// (set) Token: 0x0600B7D5 RID: 47061 RVA: 0x0043FECF File Offset: 0x0043E0CF
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

		// Token: 0x0600B7D6 RID: 47062 RVA: 0x0043FEDD File Offset: 0x0043E0DD
		public unsafe void Tick()
		{
			this.ptr->uiTick += 1U;
		}

		// Token: 0x17001708 RID: 5896
		// (set) Token: 0x0600B7D7 RID: 47063 RVA: 0x0043FEEF File Offset: 0x0043E0EF
		public unsafe Vector3 AvatarPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarPosition.FixedElementField, value);
			}
		}

		// Token: 0x17001709 RID: 5897
		// (set) Token: 0x0600B7D8 RID: 47064 RVA: 0x0043FF08 File Offset: 0x0043E108
		public unsafe Vector3 AvatarForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarFront.FixedElementField, value);
			}
		}

		// Token: 0x1700170A RID: 5898
		// (set) Token: 0x0600B7D9 RID: 47065 RVA: 0x0043FF21 File Offset: 0x0043E121
		public unsafe Vector3 AvatarTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fAvatarTop.FixedElementField, value);
			}
		}

		// Token: 0x1700170B RID: 5899
		// (set) Token: 0x0600B7DA RID: 47066 RVA: 0x0043FF3A File Offset: 0x0043E13A
		public unsafe string Name
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->name.FixedElementField, value, 256, Encoding.Unicode);
			}
		}

		// Token: 0x1700170C RID: 5900
		// (set) Token: 0x0600B7DB RID: 47067 RVA: 0x0043FF5D File Offset: 0x0043E15D
		public unsafe Vector3 CameraPosition
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraPosition.FixedElementField, value);
			}
		}

		// Token: 0x1700170D RID: 5901
		// (set) Token: 0x0600B7DC RID: 47068 RVA: 0x0043FF76 File Offset: 0x0043E176
		public unsafe Vector3 CameraForward
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraFront.FixedElementField, value);
			}
		}

		// Token: 0x1700170E RID: 5902
		// (set) Token: 0x0600B7DD RID: 47069 RVA: 0x0043FF8F File Offset: 0x0043E18F
		public unsafe Vector3 CameraTop
		{
			set
			{
				Util.SetVector3(&this.ptr->fCameraTop.FixedElementField, value);
			}
		}

		// Token: 0x1700170F RID: 5903
		// (set) Token: 0x0600B7DE RID: 47070 RVA: 0x0043FFA8 File Offset: 0x0043E1A8
		public unsafe string Identity
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->identity.FixedElementField, value, 256, Encoding.Unicode);
			}
		}

		// Token: 0x17001710 RID: 5904
		// (set) Token: 0x0600B7DF RID: 47071 RVA: 0x0043FFCB File Offset: 0x0043E1CB
		public unsafe string Context
		{
			set
			{
				Util.SetContext(&this.ptr->context.FixedElementField, &this.ptr->context_len, value);
			}
		}

		// Token: 0x17001711 RID: 5905
		// (set) Token: 0x0600B7E0 RID: 47072 RVA: 0x0043FFF0 File Offset: 0x0043E1F0
		public unsafe string Description
		{
			set
			{
				Util.SetString<ushort>(&this.ptr->description.FixedElementField, value, 2048, Encoding.Unicode);
			}
		}

		// Token: 0x0600B7E1 RID: 47073 RVA: 0x00440013 File Offset: 0x0043E213
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600B7E2 RID: 47074 RVA: 0x00440024 File Offset: 0x0043E224
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~WindowsLinkFile()
		{
			this.Dispose();
		}

		// Token: 0x0600B7E3 RID: 47075 RVA: 0x00440050 File Offset: 0x0043E250
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

		// Token: 0x04008969 RID: 35177
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MemoryMappedFile memoryMappedFile;

		// Token: 0x0400896A RID: 35178
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe readonly WindowsLinkFile.WindowsLinkMemory* ptr;

		// Token: 0x0400896B RID: 35179
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;

		// Token: 0x020016F7 RID: 5879
		[PublicizedFrom(EAccessModifier.Private)]
		public struct WindowsLinkMemory
		{
			// Token: 0x0400896C RID: 35180
			public uint uiVersion;

			// Token: 0x0400896D RID: 35181
			public uint uiTick;

			// Token: 0x0400896E RID: 35182
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarPosition>e__FixedBuffer fAvatarPosition;

			// Token: 0x0400896F RID: 35183
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarFront>e__FixedBuffer fAvatarFront;

			// Token: 0x04008970 RID: 35184
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fAvatarTop>e__FixedBuffer fAvatarTop;

			// Token: 0x04008971 RID: 35185
			[FixedBuffer(typeof(ushort), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<name>e__FixedBuffer name;

			// Token: 0x04008972 RID: 35186
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraPosition>e__FixedBuffer fCameraPosition;

			// Token: 0x04008973 RID: 35187
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraFront>e__FixedBuffer fCameraFront;

			// Token: 0x04008974 RID: 35188
			[FixedBuffer(typeof(float), 3)]
			public WindowsLinkFile.WindowsLinkMemory.<fCameraTop>e__FixedBuffer fCameraTop;

			// Token: 0x04008975 RID: 35189
			[FixedBuffer(typeof(ushort), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<identity>e__FixedBuffer identity;

			// Token: 0x04008976 RID: 35190
			public uint context_len;

			// Token: 0x04008977 RID: 35191
			[FixedBuffer(typeof(byte), 256)]
			public WindowsLinkFile.WindowsLinkMemory.<context>e__FixedBuffer context;

			// Token: 0x04008978 RID: 35192
			[FixedBuffer(typeof(ushort), 2048)]
			public WindowsLinkFile.WindowsLinkMemory.<description>e__FixedBuffer description;

			// Token: 0x020016F8 RID: 5880
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 256)]
			public struct <context>e__FixedBuffer
			{
				// Token: 0x04008979 RID: 35193
				public byte FixedElementField;
			}

			// Token: 0x020016F9 RID: 5881
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 4096)]
			public struct <description>e__FixedBuffer
			{
				// Token: 0x0400897A RID: 35194
				public ushort FixedElementField;
			}

			// Token: 0x020016FA RID: 5882
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarFront>e__FixedBuffer
			{
				// Token: 0x0400897B RID: 35195
				public float FixedElementField;
			}

			// Token: 0x020016FB RID: 5883
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarPosition>e__FixedBuffer
			{
				// Token: 0x0400897C RID: 35196
				public float FixedElementField;
			}

			// Token: 0x020016FC RID: 5884
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fAvatarTop>e__FixedBuffer
			{
				// Token: 0x0400897D RID: 35197
				public float FixedElementField;
			}

			// Token: 0x020016FD RID: 5885
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraFront>e__FixedBuffer
			{
				// Token: 0x0400897E RID: 35198
				public float FixedElementField;
			}

			// Token: 0x020016FE RID: 5886
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraPosition>e__FixedBuffer
			{
				// Token: 0x0400897F RID: 35199
				public float FixedElementField;
			}

			// Token: 0x020016FF RID: 5887
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 12)]
			public struct <fCameraTop>e__FixedBuffer
			{
				// Token: 0x04008980 RID: 35200
				public float FixedElementField;
			}

			// Token: 0x02001700 RID: 5888
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 512)]
			public struct <identity>e__FixedBuffer
			{
				// Token: 0x04008981 RID: 35201
				public ushort FixedElementField;
			}

			// Token: 0x02001701 RID: 5889
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 512)]
			public struct <name>e__FixedBuffer
			{
				// Token: 0x04008982 RID: 35202
				public ushort FixedElementField;
			}
		}
	}
}
