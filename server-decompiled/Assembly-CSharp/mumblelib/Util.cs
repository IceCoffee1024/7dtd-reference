using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x020016E3 RID: 5859
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class Util
	{
		// Token: 0x0600B710 RID: 46864 RVA: 0x0043D5D5 File Offset: 0x0043B7D5
		public unsafe static void SetVector3(float* _output, Vector3 _input)
		{
			*_output = _input.x;
			_output[1] = _input.y;
			_output[2] = _input.z;
		}

		// Token: 0x0600B711 RID: 46865 RVA: 0x0043D5F8 File Offset: 0x0043B7F8
		public unsafe static void SetString<[IsUnmanaged] T>(T* _output, string _input, int _max, Encoding _encoding) where T : struct, ValueType
		{
			byte[] bytes = _encoding.GetBytes(_input + "\0");
			Marshal.Copy(bytes, 0, new IntPtr((void*)_output), Math.Min(bytes.Length, _max * Marshal.SizeOf<T>()));
		}

		// Token: 0x0600B712 RID: 46866 RVA: 0x0043D634 File Offset: 0x0043B834
		public unsafe static void SetContext(byte* _output, uint* _len, string _input)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(_input);
			*_len = (uint)Math.Min(bytes.Length, 256);
			Marshal.Copy(bytes, 0, new IntPtr((void*)_output), (int)(*_len));
		}
	}
}
