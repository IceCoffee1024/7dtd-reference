using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001784 RID: 6020
	[BurstCompile(CompileSynchronously = true)]
	public class StampManager
	{
		// Token: 0x0600BACA RID: 47818 RVA: 0x0044E8A1 File Offset: 0x0044CAA1
		public StampManager(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0600BACB RID: 47819 RVA: 0x0044E8C8 File Offset: 0x0044CAC8
		public void ClearStamps()
		{
			foreach (RawStamp rawStamp in this.AllStamps.Values)
			{
				rawStamp.Clear();
			}
			this.AllStamps.Clear();
		}

		// Token: 0x0600BACC RID: 47820 RVA: 0x0044E928 File Offset: 0x0044CB28
		public void DrawStampGroup(StampGroup _group, ref NativeArray<float> _dest, int size)
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			for (int i = 0; i < _group.Stamps.Count; i++)
			{
				Stamp stamp = _group.Stamps[i];
				if (stamp != null)
				{
					this.DrawStamp(ref _dest, stamp);
					if (this.worldBuilder.IsCanceled)
					{
						break;
					}
				}
			}
			Log.Out("DrawStampGroup '{0}', count {1}, in {2}", new object[]
			{
				_group.Name,
				_group.Stamps.Count,
				(float)microStopwatch.ElapsedMilliseconds * 0.001f
			});
		}

		// Token: 0x0600BACD RID: 47821 RVA: 0x0044E9B8 File Offset: 0x0044CBB8
		public void DrawStampGroup(StampGroup _group, Color32[] _image, int size, float _stampScale = 1f)
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			for (int i = 0; i < _group.Stamps.Count; i++)
			{
				Stamp stamp = _group.Stamps[i];
				if (stamp != null)
				{
					int num = (int)((float)stamp.transform.x * _stampScale);
					int num2 = (int)((float)stamp.transform.y * _stampScale);
					StampManager.DrawStamp(_image, ref stamp.stamp.data, num, num2, size, size, stamp.imageWidth, stamp.imageHeight, stamp.alpha, stamp.scale * _stampScale, stamp.customColor, stamp.alphaCutoff, (float)stamp.transform.rotation, stamp.isWater);
					if (this.worldBuilder.IsCanceled)
					{
						break;
					}
				}
			}
			Log.Out("DrawStampGroup c32 '{0}', count {1}, in {2}", new object[]
			{
				_group.Name,
				_group.Stamps.Count,
				(float)microStopwatch.ElapsedMilliseconds * 0.001f
			});
		}

		// Token: 0x0600BACE RID: 47822 RVA: 0x0044EAB8 File Offset: 0x0044CCB8
		public void DrawWaterStampGroup(StampGroup _group, ref NativeArray<float> _dest, int _destSize)
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			for (int i = 0; i < _group.Stamps.Count; i++)
			{
				Stamp stamp = _group.Stamps[i];
				if (stamp != null)
				{
					StampManager.DrawWaterStamp(stamp, ref _dest, _destSize);
					if (this.worldBuilder.IsCanceled)
					{
						break;
					}
				}
			}
			Log.Out("DrawWaterStampGroup '{0}', count {1}, in {2}", new object[]
			{
				_group.Name,
				_group.Stamps.Count,
				(float)microStopwatch.ElapsedMilliseconds * 0.001f
			});
		}

		// Token: 0x0600BACF RID: 47823 RVA: 0x0044EB48 File Offset: 0x0044CD48
		[PublicizedFrom(EAccessModifier.Private)]
		public static float CalcRotatedValue(float x1, float y1, ref NativeArray<float> src, float sine, float cosine, int width, int height, bool isWater = false)
		{
			int num = width >> 1;
			int num2 = height >> 1;
			float num3 = cosine * (x1 - (float)num) + sine * (y1 - (float)num2) + (float)num;
			int num4 = (int)num3;
			if ((ulong)num4 >= (ulong)((long)width))
			{
				return 0f;
			}
			float num5 = -sine * (x1 - (float)num) + cosine * (y1 - (float)num2) + (float)num2;
			int num6 = (int)num5;
			if ((ulong)num6 >= (ulong)((long)height))
			{
				return 0f;
			}
			int num7 = num4 + num6 * width;
			float num8 = src[num7];
			float num9;
			if (num4 + 1 < width)
			{
				num9 = src[num7 + 1];
			}
			else
			{
				num9 = num8;
			}
			float num10;
			if (num6 + 1 < height)
			{
				num10 = src[num7 + width];
			}
			else
			{
				num10 = num8;
			}
			float num11;
			if (num4 + 1 < width && num6 + 1 < height)
			{
				num11 = src[num7 + width + 1];
			}
			else
			{
				num11 = num8;
			}
			if (isWater && (num8 > 0f || num10 > 0f || num9 > 0f || num11 > 0f))
			{
				return num8;
			}
			num3 -= (float)num4;
			num5 -= (float)num6;
			num8 += (num9 - num8) * num3;
			float num12 = num10 + (num11 - num10) * num3;
			return num8 + (num12 - num8) * num5;
		}

		// Token: 0x0600BAD0 RID: 47824 RVA: 0x0044EC74 File Offset: 0x0044CE74
		[PublicizedFrom(EAccessModifier.Private)]
		public static float CalcRotatedValue(ref StampManager.RotateParams _p, float x1, float y1, ref NativeArray<float> src)
		{
			float num = _p.cosine * (x1 - _p.halfWidth) + _p.sine * (y1 - _p.halfHeight) + _p.halfWidth;
			int num2 = (int)num;
			if ((ulong)num2 >= (ulong)((long)_p.width))
			{
				return 0f;
			}
			float num3 = -_p.sine * (x1 - _p.halfWidth) + _p.cosine * (y1 - _p.halfHeight) + _p.halfHeight;
			int num4 = (int)num3;
			if ((ulong)num4 >= (ulong)((long)_p.height))
			{
				return 0f;
			}
			int num5 = num2 + num4 * _p.width;
			float num6 = src[num5];
			float num7 = num6;
			if (num2 + 1 < _p.width)
			{
				num7 = src[num5 + 1];
			}
			float num8 = num6;
			if (num4 + 1 < _p.height)
			{
				num8 = src[num5 + _p.width];
			}
			float num9 = num6;
			if (num2 + 1 < _p.width && num4 + 1 < _p.height)
			{
				num9 = src[num5 + _p.width + 1];
			}
			if (_p.isWater && (num6 > 0f || num8 > 0f || num7 > 0f || num9 > 0f))
			{
				return num6;
			}
			num -= (float)num2;
			num3 -= (float)num4;
			float num10 = num6 + (num7 - num6) * num;
			float num11 = num8 + (num9 - num8) * num;
			return num10 + (num11 - num10) * num3;
		}

		// Token: 0x0600BAD1 RID: 47825 RVA: 0x0044EDD8 File Offset: 0x0044CFD8
		public void DrawStamp(ref NativeArray<float> _dest, Stamp stamp)
		{
			int x = stamp.transform.x;
			int y = stamp.transform.y;
			int worldSize = this.worldBuilder.WorldSize;
			float num = (float)stamp.transform.rotation;
			StampManager.DrawStamp(ref _dest, ref stamp.stamp.data, x, y, worldSize, worldSize, stamp.alpha, stamp.additive, stamp.scale, ref stamp.customColor, stamp.alphaCutoff, num);
		}

		// Token: 0x0600BAD2 RID: 47826 RVA: 0x0044EE4C File Offset: 0x0044D04C
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void DrawStamp(ref NativeArray<float> _dest, ref RawStamp.Data _src, int _x, int _y, int _destWidth, int _destHeight, float _alphaScale, bool _additive, float _scale, ref Color32 _customColor, float _alphaCutoff, float _angle)
		{
			StampManager.DrawStamp_0000BA7A$BurstDirectCall.Invoke(ref _dest, ref _src, _x, _y, _destWidth, _destHeight, _alphaScale, _additive, _scale, ref _customColor, _alphaCutoff, _angle);
		}

		// Token: 0x0600BAD3 RID: 47827 RVA: 0x0044EE74 File Offset: 0x0044D074
		[PublicizedFrom(EAccessModifier.Private)]
		public static void DrawStamp(Color32[] _dest, ref RawStamp.Data _src, int _x, int _y, int _destWidth, int _destHeight, int _srcWidth, int _srcHeight, float _alpha, float _scale, Color32 _customColor, float _alphaCutoff = 0.1f, float _angle = 0f, bool isWater = false)
		{
			_x -= (int)((float)_srcWidth * _scale) / 2;
			_y -= (int)((float)_srcHeight * _scale) / 2;
			double num = (double)(0.017453292f * _angle);
			float num2 = (float)Math.Sin(num);
			float num3 = (float)Math.Cos(num);
			int num4 = Mathf.FloorToInt((float)(((int)Mathf.Sqrt((float)(_srcWidth * _srcWidth + _srcHeight * _srcHeight)) - _srcWidth) / 2) * _scale);
			int num5 = Mathf.FloorToInt((float)_srcWidth * _scale + (float)num4);
			num4 = -num4;
			int num6 = num4;
			int num7 = _x + num4;
			if (num7 < 0)
			{
				num6 -= num7;
			}
			int num8 = num5;
			num7 = _x + num5;
			if (num7 >= _destWidth)
			{
				num8 -= num7 - _destWidth;
			}
			int num9 = num4;
			int num10 = _y + num4;
			if (num10 < 0)
			{
				num9 -= num10;
			}
			int num11 = num5;
			num10 = _y + num5;
			if (num10 >= _destHeight)
			{
				num11 -= num10 - _destHeight;
			}
			for (int i = num9; i < num11; i++)
			{
				int num12 = (_y + i) * _destWidth;
				float num13 = (float)i / _scale;
				for (int j = num6; j < num8; j++)
				{
					float num14 = _src.alphaConst;
					if (_src.alphaPixels.IsCreated)
					{
						num14 = StampManager.CalcRotatedValue((float)j / _scale, num13, ref _src.alphaPixels, num2, num3, _srcWidth, _srcHeight, isWater);
					}
					if (num14 > _alphaCutoff)
					{
						int num15 = _x + j + num12;
						_dest[num15] = _customColor;
					}
				}
			}
		}

		// Token: 0x0600BAD4 RID: 47828 RVA: 0x0044EFC0 File Offset: 0x0044D1C0
		public static void DrawWaterStamp(Stamp stamp, ref NativeArray<float> _dest, int _destSize)
		{
			if (!stamp.isWater)
			{
				throw new ArgumentException("DrawWaterStamp called with non-water stamp " + stamp.Name);
			}
			NativeArray<float> nativeArray = stamp.stamp.data.waterPixels;
			if (!nativeArray.IsCreated)
			{
				nativeArray = stamp.stamp.data.alphaPixels;
				if (!nativeArray.IsCreated)
				{
					return;
				}
			}
			int x = stamp.transform.x;
			int y = stamp.transform.y;
			float num = (float)stamp.transform.rotation;
			StampManager.DrawWaterStamp(ref _dest, ref nativeArray, x, y, _destSize, _destSize, stamp.imageWidth, stamp.imageHeight, stamp.alpha, stamp.scale, (float)stamp.customColor.b, num);
		}

		// Token: 0x0600BAD5 RID: 47829 RVA: 0x0044F078 File Offset: 0x0044D278
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void DrawWaterStamp(ref NativeArray<float> _dest, ref NativeArray<float> _src, int _x, int _y, int _destWidth, int _destHeight, int _srcWidth, int _srcHeight, float _alpha, float _scale, float _customColor, float _angle)
		{
			StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.Invoke(ref _dest, ref _src, _x, _y, _destWidth, _destHeight, _srcWidth, _srcHeight, _alpha, _scale, _customColor, _angle);
		}

		// Token: 0x0600BAD6 RID: 47830 RVA: 0x0044F0A0 File Offset: 0x0044D2A0
		public static void DrawBiomeStamp(Color32[] _dest, ref NativeArray<float> _src, int _x, int _y, int _destWidth, int _destHeight, int _srcWidth, int _srcHeight, float _scale, Color32 _destColor, float _alphaCutoff = 0.1f, float _angle = 0f)
		{
			_x -= (int)((float)_srcWidth * _scale) / 2;
			_y -= (int)((float)_srcHeight * _scale) / 2;
			double num = (double)(0.017453292f * _angle);
			float num2 = (float)Math.Sin(num);
			float num3 = (float)Math.Cos(num);
			int num4 = Mathf.FloorToInt((float)(((int)Mathf.Sqrt((float)(_srcWidth * _srcWidth + _srcHeight * _srcHeight)) - _srcWidth) / 2) * _scale);
			int num5 = Mathf.FloorToInt((float)_srcWidth * _scale + (float)num4);
			num4 = -num4;
			int num6 = num4;
			int num7 = _x + num4;
			if (num7 < 0)
			{
				num6 -= num7;
			}
			int num8 = num5;
			num7 = _x + num5;
			if (num7 >= _destWidth)
			{
				num8 -= num7 - _destWidth;
			}
			int num9 = num4;
			int num10 = _y + num4;
			if (num10 < 0)
			{
				num9 -= num10;
			}
			int num11 = num5;
			num10 = _y + num5;
			if (num10 >= _destHeight)
			{
				num11 -= num10 - _destHeight;
			}
			for (int i = num9; i < num11; i++)
			{
				int num12 = (_y + i) * _destWidth;
				float num13 = (float)i / _scale;
				for (int j = num6; j < num8; j++)
				{
					int num14 = _x + j + num12;
					if (StampManager.CalcRotatedValue((float)j / _scale, num13, ref _src, num2, num3, _srcWidth, _srcHeight, false) > _alphaCutoff)
					{
						_dest[num14] = _destColor;
					}
				}
			}
		}

		// Token: 0x0600BAD7 RID: 47831 RVA: 0x0044F1CB File Offset: 0x0044D3CB
		public bool TryGetStamp(string terrainTypeName, string comboTypeName, out RawStamp tmp)
		{
			return this.TryGetStamp(comboTypeName, out tmp) || this.TryGetStamp(terrainTypeName, out tmp);
		}

		// Token: 0x0600BAD8 RID: 47832 RVA: 0x0044F1E4 File Offset: 0x0044D3E4
		public bool TryGetStamp(string _baseName, out RawStamp _output)
		{
			return this.TryGetStamp(_baseName, out _output, Rand.Instance);
		}

		// Token: 0x0600BAD9 RID: 47833 RVA: 0x0044F1F4 File Offset: 0x0044D3F4
		public bool TryGetStamp(string _baseName, out RawStamp _output, Rand _rand)
		{
			List<RawStamp> list = this.tempGetStampList;
			bool flag2;
			lock (list)
			{
				foreach (KeyValuePair<string, RawStamp> keyValuePair in this.AllStamps)
				{
					if (keyValuePair.Key.StartsWith(_baseName))
					{
						this.tempGetStampList.Add(keyValuePair.Value);
					}
				}
				if (this.tempGetStampList.Count == 0)
				{
					_output = null;
					flag2 = false;
				}
				else
				{
					if (this.tempGetStampList.Count == 1)
					{
						_output = this.tempGetStampList[0];
					}
					else
					{
						_output = this.tempGetStampList[_rand.Range(this.tempGetStampList.Count)];
					}
					this.tempGetStampList.Clear();
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x0600BADA RID: 47834 RVA: 0x0044F2EC File Offset: 0x0044D4EC
		public RawStamp GetStamp(string _baseName, Rand _rand = null)
		{
			if (_rand == null)
			{
				_rand = Rand.Instance;
			}
			RawStamp rawStamp;
			if (!this.TryGetStamp(_baseName, out rawStamp, _rand))
			{
				return null;
			}
			return rawStamp;
		}

		// Token: 0x0600BADB RID: 47835 RVA: 0x0044F312 File Offset: 0x0044D512
		public IEnumerator LoadStamps()
		{
			if (this.AllStamps.Count > 0)
			{
				yield break;
			}
			MicroStopwatch ms = new MicroStopwatch(true);
			this.AllStamps.Clear();
			List<PathAbstractions.AbstractedLocation> stampMaps = PathAbstractions.RwgStampsSearchPaths.GetAvailablePathsList(null, true, null);
			MicroStopwatch msRestart = new MicroStopwatch(true);
			int num;
			for (int i = 0; i < stampMaps.Count; i = num + 1)
			{
				string extension = stampMaps[i].Extension;
				if (!(extension != ".exr") || !(extension != ".raw"))
				{
					this.LoadStamp(stampMaps[i]);
					if (msRestart.ElapsedMilliseconds > 500L)
					{
						yield return null;
						msRestart.ResetAndRestart();
					}
				}
				num = i;
			}
			for (int i = 0; i < stampMaps.Count; i = num + 1)
			{
				this.LoadStamp(stampMaps[i]);
				if (msRestart.ElapsedMilliseconds > 500L)
				{
					yield return null;
					msRestart.ResetAndRestart();
				}
				num = i;
			}
			Log.Out("LoadStamps in {0}", new object[] { (float)ms.ElapsedMilliseconds * 0.001f });
			yield break;
		}

		// Token: 0x0600BADC RID: 47836 RVA: 0x0044F324 File Offset: 0x0044D524
		[PublicizedFrom(EAccessModifier.Private)]
		public void LoadStamp(PathAbstractions.AbstractedLocation _path)
		{
			string fileNameNoExtension = _path.FileNameNoExtension;
			if (this.AllStamps.ContainsKey(fileNameNoExtension))
			{
				return;
			}
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			Color[] array;
			if (_path.Extension == ".raw")
			{
				array = Utils.LoadRawStampFileArray(_path.FullPath);
			}
			else
			{
				if (_path.Extension == ".exr")
				{
					return;
				}
				Texture2D texture2D = TextureUtils.LoadTexture(_path.FullPath, FilterMode.Point, false, false, null);
				array = texture2D.GetPixels();
				if (Application.isPlaying)
				{
					global::UnityEngine.Object.Destroy(texture2D);
				}
			}
			if (array == null)
			{
				Log.Error("LoadStamp {0} failed", new object[] { _path.FullPath });
				return;
			}
			NativeArray<float> nativeArray = default(NativeArray<float>);
			float num = array[0].r;
			for (int i = 1; i < array.Length; i++)
			{
				if (num != array[i].r)
				{
					nativeArray = new NativeArray<float>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					for (int j = 0; j < array.Length; j++)
					{
						nativeArray[j] = array[j].r * 255f;
					}
					break;
				}
			}
			num *= 255f;
			NativeArray<float> nativeArray2 = default(NativeArray<float>);
			float a = array[0].a;
			for (int k = 1; k < array.Length; k++)
			{
				if (a != array[k].a)
				{
					nativeArray2 = new NativeArray<float>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					for (int l = 0; l < array.Length; l++)
					{
						nativeArray2[l] = array[l].a;
					}
					break;
				}
			}
			NativeArray<float> nativeArray3 = default(NativeArray<float>);
			if (!fileNameNoExtension.Contains("rwg_tile"))
			{
				for (int m = 0; m < array.Length; m++)
				{
					if (array[m].b != 0f)
					{
						nativeArray3 = new NativeArray<float>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
						for (int n = 0; n < array.Length; n++)
						{
							nativeArray3[n] = array[n].b * 255f;
						}
						break;
					}
				}
			}
			int num2 = (int)Mathf.Sqrt((float)array.Length);
			RawStamp rawStamp = new RawStamp
			{
				name = fileNameNoExtension
			};
			rawStamp.data.heightConst = num;
			rawStamp.data.heightPixels = nativeArray;
			rawStamp.data.alphaConst = a;
			rawStamp.data.alphaPixels = nativeArray2;
			rawStamp.data.waterPixels = nativeArray3;
			rawStamp.data.height = num2;
			rawStamp.data.width = num2;
			this.AllStamps[fileNameNoExtension] = rawStamp;
			if (nativeArray2.IsCreated)
			{
				if (fileNameNoExtension.StartsWith("mountains_"))
				{
					RawStamp.SmoothAlpha(ref rawStamp.data, 5);
				}
				if (fileNameNoExtension.StartsWith("desert_mountains_"))
				{
					RawStamp.SmoothAlpha(ref rawStamp.data, 3);
				}
			}
			Log.Out("LoadStamp {0}, size {1}, height {2} ({3}), alpha {4} ({5}), water {6}, in {7}", new object[]
			{
				_path.FullPath,
				num2,
				nativeArray.IsCreated,
				num,
				nativeArray2.IsCreated,
				a,
				nativeArray3.IsCreated,
				(float)microStopwatch.ElapsedMilliseconds * 0.001f
			});
		}

		// Token: 0x0600BADD RID: 47837 RVA: 0x0044F670 File Offset: 0x0044D870
		[PublicizedFrom(EAccessModifier.Private)]
		public static void SaveEXR(string cleanname, Color[] colors, bool _replace = true)
		{
			string text = GameIO.GetGameDir("Data/Stamps") + "/" + cleanname + ".exr";
			if (!_replace && File.Exists(text))
			{
				return;
			}
			int num = (int)Mathf.Sqrt((float)colors.Length);
			Texture2D texture2D = new Texture2D(num, num, TextureFormat.RGBAFloat, false);
			texture2D.SetPixels(colors);
			File.WriteAllBytes(text, texture2D.EncodeToEXR(Texture2D.EXRFlags.None));
			Log.Out("SaveEXR {0}", new object[] { text });
		}

		// Token: 0x0600BADE RID: 47838 RVA: 0x0044F6E0 File Offset: 0x0044D8E0
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawStamp$BurstManaged(ref NativeArray<float> _dest, ref RawStamp.Data _src, int _x, int _y, int _destWidth, int _destHeight, float _alphaScale, bool _additive, float _scale, ref Color32 _customColor, float _alphaCutoff, float _angle)
		{
			_x -= (int)((float)_src.width * _scale) / 2;
			_y -= (int)((float)_src.height * _scale) / 2;
			double num = (double)(0.017453292f * _angle);
			float num2 = (float)Math.Sin(num);
			float num3 = (float)Math.Cos(num);
			int num4 = (int)Math.Floor((double)((float)(((int)Math.Sqrt((double)(_src.width * _src.width + _src.height * _src.height)) - _src.width) / 2) * _scale));
			int num5 = (int)Math.Floor((double)((float)_src.width * _scale + (float)num4));
			num4 = -num4;
			int num6 = num4;
			int num7 = _x + num4;
			if (num7 < 0)
			{
				num6 -= num7;
			}
			int num8 = num5;
			num7 = _x + num5;
			if (num7 >= _destWidth)
			{
				num8 -= num7 - _destWidth;
			}
			int num9 = num4;
			int num10 = _y + num4;
			if (num10 < 0)
			{
				num9 -= num10;
			}
			int num11 = num5;
			num10 = _y + num5;
			if (num10 >= _destHeight)
			{
				num11 -= num10 - _destHeight;
			}
			StampManager.RotateParams rotateParams;
			rotateParams.sine = num2;
			rotateParams.cosine = num3;
			rotateParams.width = _src.width;
			rotateParams.height = _src.height;
			rotateParams.halfWidth = (float)(_src.width / 2);
			rotateParams.halfHeight = (float)(_src.height / 2);
			rotateParams.isWater = false;
			for (int i = num9; i < num11; i++)
			{
				int num12 = (_y + i) * _destWidth;
				float num13 = (float)i / _scale;
				for (int j = num6; j < num8; j++)
				{
					float num14 = (float)j / _scale;
					float num15 = _src.alphaConst;
					if (_src.alphaPixels.IsCreated)
					{
						num15 = StampManager.CalcRotatedValue(ref rotateParams, num14, num13, ref _src.alphaPixels);
					}
					if (num15 >= 1E-05f)
					{
						int num16 = _x + j + num12;
						if (_customColor.a > 0)
						{
							if (num15 > _alphaCutoff)
							{
								_dest[num16] = (float)_customColor.r;
							}
						}
						else
						{
							float num17 = _src.heightConst;
							if (_src.heightPixels.IsCreated)
							{
								num17 = StampManager.CalcRotatedValue(ref rotateParams, num14, num13, ref _src.heightPixels);
							}
							float num18 = num15 * _alphaScale;
							float num19 = _dest[num16];
							if (_additive)
							{
								num19 += num17 * num18;
							}
							else
							{
								num19 += (num17 - num19) * num18;
							}
							_dest[num16] = num19;
						}
					}
				}
			}
		}

		// Token: 0x0600BADF RID: 47839 RVA: 0x0044F928 File Offset: 0x0044DB28
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawWaterStamp$BurstManaged(ref NativeArray<float> _dest, ref NativeArray<float> _src, int _x, int _y, int _destWidth, int _destHeight, int _srcWidth, int _srcHeight, float _alpha, float _scale, float _customColor, float _angle)
		{
			_x -= (int)((float)_srcWidth * _scale) / 2;
			_y -= (int)((float)_srcHeight * _scale) / 2;
			double num = (double)(0.017453292f * _angle);
			float num2 = (float)Math.Sin(num);
			float num3 = (float)Math.Cos(num);
			int num4 = (int)Math.Floor((double)((float)(((int)Math.Sqrt((double)(_srcWidth * _srcWidth + _srcHeight * _srcHeight)) - _srcWidth) / 2) * _scale));
			int num5 = (int)Math.Floor((double)((float)_srcWidth * _scale + (float)num4));
			num4 = -num4;
			int num6 = num4;
			int num7 = _x + num4;
			if (num7 < 0)
			{
				num6 -= num7;
			}
			int num8 = num5;
			num7 = _x + num5;
			if (num7 >= _destWidth)
			{
				num8 -= num7 - _destWidth;
			}
			int num9 = num4;
			int num10 = _y + num4;
			if (num10 < 0)
			{
				num9 -= num10;
			}
			int num11 = num5;
			num10 = _y + num5;
			if (num10 >= _destHeight)
			{
				num11 -= num10 - _destHeight;
			}
			StampManager.RotateParams rotateParams;
			rotateParams.sine = num2;
			rotateParams.cosine = num3;
			rotateParams.width = _srcWidth;
			rotateParams.height = _srcHeight;
			rotateParams.halfWidth = (float)(_srcWidth / 2);
			rotateParams.halfHeight = (float)(_srcHeight / 2);
			rotateParams.isWater = true;
			for (int i = num9; i < num11; i++)
			{
				int num12 = (_y + i) * _destWidth;
				float num13 = (float)i / _scale;
				for (int j = num6; j < num8; j++)
				{
					int num14 = _x + j + num12;
					if (_dest[num14] <= 0f && StampManager.CalcRotatedValue(ref rotateParams, (float)j / _scale, num13, ref _src) >= 1E-05f)
					{
						_dest[num14] = _customColor;
					}
				}
			}
		}

		// Token: 0x04008B76 RID: 35702
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cAlphaCutoff = 1E-05f;

		// Token: 0x04008B77 RID: 35703
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008B78 RID: 35704
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, RawStamp> AllStamps = new Dictionary<string, RawStamp>();

		// Token: 0x04008B79 RID: 35705
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<RawStamp> tempGetStampList = new List<RawStamp>();

		// Token: 0x02001785 RID: 6021
		[PublicizedFrom(EAccessModifier.Private)]
		public struct RotateParams
		{
			// Token: 0x04008B7A RID: 35706
			public float sine;

			// Token: 0x04008B7B RID: 35707
			public float cosine;

			// Token: 0x04008B7C RID: 35708
			public int width;

			// Token: 0x04008B7D RID: 35709
			public int height;

			// Token: 0x04008B7E RID: 35710
			public float halfWidth;

			// Token: 0x04008B7F RID: 35711
			public float halfHeight;

			// Token: 0x04008B80 RID: 35712
			public bool isWater;
		}

		// Token: 0x02001787 RID: 6023
		// (Invoke) Token: 0x0600BAE7 RID: 47847
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void DrawStamp_0000BA7A$PostfixBurstDelegate(ref NativeArray<float> _dest, ref RawStamp.Data _src, int _x, int _y, int _destWidth, int _destHeight, float _alphaScale, bool _additive, float _scale, ref Color32 _customColor, float _alphaCutoff, float _angle);

		// Token: 0x02001788 RID: 6024
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class DrawStamp_0000BA7A$BurstDirectCall
		{
			// Token: 0x0600BAEA RID: 47850 RVA: 0x0044FC9A File Offset: 0x0044DE9A
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StampManager.DrawStamp_0000BA7A$BurstDirectCall.Pointer == 0)
				{
					StampManager.DrawStamp_0000BA7A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StampManager.DrawStamp_0000BA7A$BurstDirectCall.DeferredCompilation, methodof(StampManager.DrawStamp$BurstManaged(ref NativeArray<float>, ref RawStamp.Data, int, int, int, int, float, bool, float, ref Color32, float, float)).MethodHandle, typeof(StampManager.DrawStamp_0000BA7A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StampManager.DrawStamp_0000BA7A$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BAEB RID: 47851 RVA: 0x0044FCC8 File Offset: 0x0044DEC8
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StampManager.DrawStamp_0000BA7A$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BAEC RID: 47852 RVA: 0x0044FCE0 File Offset: 0x0044DEE0
			public static void Constructor()
			{
				StampManager.DrawStamp_0000BA7A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StampManager.DrawStamp(ref NativeArray<float>, ref RawStamp.Data, int, int, int, int, float, bool, float, ref Color32, float, float)).MethodHandle);
			}

			// Token: 0x0600BAED RID: 47853 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BAEE RID: 47854 RVA: 0x0044FCF1 File Offset: 0x0044DEF1
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static DrawStamp_0000BA7A$BurstDirectCall()
			{
				StampManager.DrawStamp_0000BA7A$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BAEF RID: 47855 RVA: 0x0044FCF8 File Offset: 0x0044DEF8
			public static void Invoke(ref NativeArray<float> _dest, ref RawStamp.Data _src, int _x, int _y, int _destWidth, int _destHeight, float _alphaScale, bool _additive, float _scale, ref Color32 _customColor, float _alphaCutoff, float _angle)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StampManager.DrawStamp_0000BA7A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeArray`1<System.Single>&,WorldGenerationEngineFinal.RawStamp/Data&,System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Boolean,System.Single,UnityEngine.Color32&,System.Single,System.Single), ref _dest, ref _src, _x, _y, _destWidth, _destHeight, _alphaScale, _additive, _scale, ref _customColor, _alphaCutoff, _angle, functionPointer);
						return;
					}
				}
				StampManager.DrawStamp$BurstManaged(ref _dest, ref _src, _x, _y, _destWidth, _destHeight, _alphaScale, _additive, _scale, ref _customColor, _alphaCutoff, _angle);
			}

			// Token: 0x04008B88 RID: 35720
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008B89 RID: 35721
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001789 RID: 6025
		// (Invoke) Token: 0x0600BAF1 RID: 47857
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void DrawWaterStamp_0000BA7D$PostfixBurstDelegate(ref NativeArray<float> _dest, ref NativeArray<float> _src, int _x, int _y, int _destWidth, int _destHeight, int _srcWidth, int _srcHeight, float _alpha, float _scale, float _customColor, float _angle);

		// Token: 0x0200178A RID: 6026
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class DrawWaterStamp_0000BA7D$BurstDirectCall
		{
			// Token: 0x0600BAF4 RID: 47860 RVA: 0x0044FD4F File Offset: 0x0044DF4F
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.Pointer == 0)
				{
					StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.DeferredCompilation, methodof(StampManager.DrawWaterStamp$BurstManaged(ref NativeArray<float>, ref NativeArray<float>, int, int, int, int, int, int, float, float, float, float)).MethodHandle, typeof(StampManager.DrawWaterStamp_0000BA7D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BAF5 RID: 47861 RVA: 0x0044FD7C File Offset: 0x0044DF7C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BAF6 RID: 47862 RVA: 0x0044FD94 File Offset: 0x0044DF94
			public static void Constructor()
			{
				StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StampManager.DrawWaterStamp(ref NativeArray<float>, ref NativeArray<float>, int, int, int, int, int, int, float, float, float, float)).MethodHandle);
			}

			// Token: 0x0600BAF7 RID: 47863 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BAF8 RID: 47864 RVA: 0x0044FDA5 File Offset: 0x0044DFA5
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static DrawWaterStamp_0000BA7D$BurstDirectCall()
			{
				StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BAF9 RID: 47865 RVA: 0x0044FDAC File Offset: 0x0044DFAC
			public static void Invoke(ref NativeArray<float> _dest, ref NativeArray<float> _src, int _x, int _y, int _destWidth, int _destHeight, int _srcWidth, int _srcHeight, float _alpha, float _scale, float _customColor, float _angle)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeArray`1<System.Single>&,Unity.Collections.NativeArray`1<System.Single>&,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single), ref _dest, ref _src, _x, _y, _destWidth, _destHeight, _srcWidth, _srcHeight, _alpha, _scale, _customColor, _angle, functionPointer);
						return;
					}
				}
				StampManager.DrawWaterStamp$BurstManaged(ref _dest, ref _src, _x, _y, _destWidth, _destHeight, _srcWidth, _srcHeight, _alpha, _scale, _customColor, _angle);
			}

			// Token: 0x04008B8A RID: 35722
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008B8B RID: 35723
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
