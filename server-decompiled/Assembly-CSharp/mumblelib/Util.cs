using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x02001702 RID: 5890
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class Util
	{
		// Token: 0x0600B7E4 RID: 47076 RVA: 0x00440079 File Offset: 0x0043E279
		public unsafe static void SetVector3(float* _output, Vector3 _input)
		{
			*_output = _input.x;
			_output[1] = _input.y;
			_output[2] = _input.z;
		}

		// Token: 0x0600B7E5 RID: 47077 RVA: 0x0044009C File Offset: 0x0043E29C
		public unsafe static void SetString<[IsUnmanaged] T>(T* _output, string _input, int _max, Encoding _encoding) where T : struct, ValueType
		{
			byte[] bytes = _encoding.GetBytes(_input + "\0");
			Marshal.Copy(bytes, 0, new IntPtr((void*)_output), Math.Min(bytes.Length, _max * Marshal.SizeOf<T>()));
		}

		// Token: 0x0600B7E6 RID: 47078 RVA: 0x004400D8 File Offset: 0x0043E2D8
		public unsafe static void SetContext(byte* _output, uint* _len, string _input)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(_input);
			*_len = (uint)Math.Min(bytes.Length, 256);
			Marshal.Copy(bytes, 0, new IntPtr((void*)_output), (int)(*_len));
		}
	}
}
