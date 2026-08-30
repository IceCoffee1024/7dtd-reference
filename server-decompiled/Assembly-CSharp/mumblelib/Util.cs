using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x020016B7 RID: 5815
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class Util
	{
		// Token: 0x0600B5B9 RID: 46521 RVA: 0x004390B5 File Offset: 0x004372B5
		public unsafe static void SetVector3(float* _output, Vector3 _input)
		{
			*_output = _input.x;
			_output[1] = _input.y;
			_output[2] = _input.z;
		}

		// Token: 0x0600B5BA RID: 46522 RVA: 0x004390D8 File Offset: 0x004372D8
		public unsafe static void SetString<[IsUnmanaged] T>(T* _output, string _input, int _max, Encoding _encoding) where T : struct, ValueType
		{
			byte[] bytes = _encoding.GetBytes(_input + "\0");
			Marshal.Copy(bytes, 0, new IntPtr((void*)_output), Math.Min(bytes.Length, _max * Marshal.SizeOf<T>()));
		}

		// Token: 0x0600B5BB RID: 46523 RVA: 0x00439114 File Offset: 0x00437314
		public unsafe static void SetContext(byte* _output, uint* _len, string _input)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(_input);
			*_len = (uint)Math.Min(bytes.Length, 256);
			Marshal.Copy(bytes, 0, new IntPtr((void*)_output), (int)(*_len));
		}
	}
}
