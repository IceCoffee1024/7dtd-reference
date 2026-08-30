using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000102 RID: 258
	[RequireComponent(typeof(Transform))]
	[UsedByNativeCode]
	[NativeHeader("Runtime/Misc/GameObjectUtility.h")]
	[NativeHeader("Runtime/Graphics/RenderTexture.h")]
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	[NativeHeader("Runtime/Camera/RenderManager.h")]
	[NativeHeader("Runtime/Camera/Camera.h")]
	[NativeHeader("Runtime/Graphics/CommandBuffer/RenderingCommandBuffer.h")]
	[NativeHeader("Runtime/Shaders/Shader.h")]
	public sealed class Camera : Behaviour
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000536 RID: 1334
		// (set) Token: 0x06000537 RID: 1335
		[NativeProperty("Near")]
		public extern float nearClipPlane
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000538 RID: 1336
		// (set) Token: 0x06000539 RID: 1337
		[NativeProperty("Far")]
		public extern float farClipPlane
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600053A RID: 1338
		// (set) Token: 0x0600053B RID: 1339
		[NativeProperty("VerticalFieldOfView")]
		public extern float fieldOfView
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600053C RID: 1340
		// (set) Token: 0x0600053D RID: 1341
		public extern RenderingPath renderingPath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600053E RID: 1342
		public extern RenderingPath actualRenderingPath
		{
			[NativeName("CalculateRenderingPath")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600053F RID: 1343
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Reset();

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000540 RID: 1344
		// (set) Token: 0x06000541 RID: 1345
		public extern bool allowHDR
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000542 RID: 1346
		// (set) Token: 0x06000543 RID: 1347
		public extern bool allowMSAA
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000544 RID: 1348
		// (set) Token: 0x06000545 RID: 1349
		public extern bool allowDynamicResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000546 RID: 1350
		// (set) Token: 0x06000547 RID: 1351
		[NativeProperty("ForceIntoRT")]
		public extern bool forceIntoRenderTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000548 RID: 1352
		// (set) Token: 0x06000549 RID: 1353
		public extern float orthographicSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600054A RID: 1354
		// (set) Token: 0x0600054B RID: 1355
		public extern bool orthographic
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600054C RID: 1356
		// (set) Token: 0x0600054D RID: 1357
		public extern OpaqueSortMode opaqueSortMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600054E RID: 1358
		// (set) Token: 0x0600054F RID: 1359
		public extern TransparencySortMode transparencySortMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00008620 File Offset: 0x00006820
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x00008636 File Offset: 0x00006836
		public Vector3 transparencySortAxis
		{
			get
			{
				Vector3 vector;
				this.get_transparencySortAxis_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_transparencySortAxis_Injected(ref value);
			}
		}

		// Token: 0x06000552 RID: 1362
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetTransparencySortSettings();

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000553 RID: 1363
		// (set) Token: 0x06000554 RID: 1364
		public extern float depth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000555 RID: 1365
		// (set) Token: 0x06000556 RID: 1366
		public extern float aspect
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000557 RID: 1367
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetAspect();

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00008640 File Offset: 0x00006840
		public Vector3 velocity
		{
			get
			{
				Vector3 vector;
				this.get_velocity_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000559 RID: 1369
		// (set) Token: 0x0600055A RID: 1370
		public extern int cullingMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600055B RID: 1371
		// (set) Token: 0x0600055C RID: 1372
		public extern int eventMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600055D RID: 1373
		// (set) Token: 0x0600055E RID: 1374
		public extern bool layerCullSpherical
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600055F RID: 1375
		// (set) Token: 0x06000560 RID: 1376
		public extern CameraType cameraType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000561 RID: 1377
		internal extern Material skyboxMaterial
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000562 RID: 1378
		// (set) Token: 0x06000563 RID: 1379
		[NativeConditional("UNITY_EDITOR")]
		public extern ulong overrideSceneCullingMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000564 RID: 1380
		[NativeConditional("UNITY_EDITOR")]
		internal extern ulong sceneCullingMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000565 RID: 1381
		[FreeFunction("CameraScripting::GetLayerCullDistances", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float[] GetLayerCullDistances();

		// Token: 0x06000566 RID: 1382
		[FreeFunction("CameraScripting::SetLayerCullDistances", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLayerCullDistances([NotNull("ArgumentNullException")] float[] d);

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x00008658 File Offset: 0x00006858
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x00008670 File Offset: 0x00006870
		public float[] layerCullDistances
		{
			get
			{
				return this.GetLayerCullDistances();
			}
			set
			{
				bool flag = value.Length != 32;
				if (flag)
				{
					throw new UnityException("Array needs to contain exactly 32 floats for layerCullDistances.");
				}
				this.SetLayerCullDistances(value);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x000086A0 File Offset: 0x000068A0
		[Obsolete("PreviewCullingLayer is obsolete. Use scene culling masks instead.", false)]
		internal static int PreviewCullingLayer
		{
			get
			{
				return 31;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600056A RID: 1386
		// (set) Token: 0x0600056B RID: 1387
		public extern bool useOcclusionCulling
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x000086B4 File Offset: 0x000068B4
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x000086CA File Offset: 0x000068CA
		public Matrix4x4 cullingMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_cullingMatrix_Injected(out matrix4x);
				return matrix4x;
			}
			set
			{
				this.set_cullingMatrix_Injected(ref value);
			}
		}

		// Token: 0x0600056E RID: 1390
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetCullingMatrix();

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x000086D4 File Offset: 0x000068D4
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x000086EA File Offset: 0x000068EA
		public Color backgroundColor
		{
			get
			{
				Color color;
				this.get_backgroundColor_Injected(out color);
				return color;
			}
			set
			{
				this.set_backgroundColor_Injected(ref value);
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000571 RID: 1393
		// (set) Token: 0x06000572 RID: 1394
		public extern CameraClearFlags clearFlags
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000573 RID: 1395
		// (set) Token: 0x06000574 RID: 1396
		public extern DepthTextureMode depthTextureMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000575 RID: 1397
		// (set) Token: 0x06000576 RID: 1398
		public extern bool clearStencilAfterLightingPass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000577 RID: 1399
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetReplacementShader(Shader shader, string replacementTag);

		// Token: 0x06000578 RID: 1400
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetReplacementShader();

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000579 RID: 1401
		internal extern Camera.ProjectionMatrixMode projectionMatrixMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600057A RID: 1402
		// (set) Token: 0x0600057B RID: 1403
		public extern bool usePhysicalProperties
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600057C RID: 1404
		// (set) Token: 0x0600057D RID: 1405
		public extern int iso
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600057E RID: 1406
		// (set) Token: 0x0600057F RID: 1407
		public extern float shutterSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000580 RID: 1408
		// (set) Token: 0x06000581 RID: 1409
		public extern float aperture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000582 RID: 1410
		// (set) Token: 0x06000583 RID: 1411
		public extern float focusDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000584 RID: 1412
		// (set) Token: 0x06000585 RID: 1413
		public extern float focalLength
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000586 RID: 1414
		// (set) Token: 0x06000587 RID: 1415
		public extern int bladeCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x000086F4 File Offset: 0x000068F4
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0000870A File Offset: 0x0000690A
		public Vector2 curvature
		{
			get
			{
				Vector2 vector;
				this.get_curvature_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_curvature_Injected(ref value);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600058A RID: 1418
		// (set) Token: 0x0600058B RID: 1419
		public extern float barrelClipping
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600058C RID: 1420
		// (set) Token: 0x0600058D RID: 1421
		public extern float anamorphism
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00008714 File Offset: 0x00006914
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x0000872A File Offset: 0x0000692A
		public Vector2 sensorSize
		{
			get
			{
				Vector2 vector;
				this.get_sensorSize_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_sensorSize_Injected(ref value);
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00008734 File Offset: 0x00006934
		// (set) Token: 0x06000591 RID: 1425 RVA: 0x0000874A File Offset: 0x0000694A
		public Vector2 lensShift
		{
			get
			{
				Vector2 vector;
				this.get_lensShift_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_lensShift_Injected(ref value);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000592 RID: 1426
		// (set) Token: 0x06000593 RID: 1427
		public extern Camera.GateFitMode gateFit
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000594 RID: 1428
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetGateFittedFieldOfView();

		// Token: 0x06000595 RID: 1429 RVA: 0x00008754 File Offset: 0x00006954
		public Vector2 GetGateFittedLensShift()
		{
			Vector2 vector;
			this.GetGateFittedLensShift_Injected(out vector);
			return vector;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0000876C File Offset: 0x0000696C
		internal Vector3 GetLocalSpaceAim()
		{
			Vector3 vector;
			this.GetLocalSpaceAim_Injected(out vector);
			return vector;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00008784 File Offset: 0x00006984
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0000879A File Offset: 0x0000699A
		[NativeProperty("NormalizedViewportRect")]
		public Rect rect
		{
			get
			{
				Rect rect;
				this.get_rect_Injected(out rect);
				return rect;
			}
			set
			{
				this.set_rect_Injected(ref value);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x000087A4 File Offset: 0x000069A4
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x000087BA File Offset: 0x000069BA
		[NativeProperty("ScreenViewportRect")]
		public Rect pixelRect
		{
			get
			{
				Rect rect;
				this.get_pixelRect_Injected(out rect);
				return rect;
			}
			set
			{
				this.set_pixelRect_Injected(ref value);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600059B RID: 1435
		public extern int pixelWidth
		{
			[FreeFunction("CameraScripting::GetPixelWidth", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600059C RID: 1436
		public extern int pixelHeight
		{
			[FreeFunction("CameraScripting::GetPixelHeight", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600059D RID: 1437
		public extern int scaledPixelWidth
		{
			[FreeFunction("CameraScripting::GetScaledPixelWidth", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600059E RID: 1438
		public extern int scaledPixelHeight
		{
			[FreeFunction("CameraScripting::GetScaledPixelHeight", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600059F RID: 1439
		// (set) Token: 0x060005A0 RID: 1440
		public extern RenderTexture targetTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060005A1 RID: 1441
		public extern RenderTexture activeTexture
		{
			[NativeName("GetCurrentTargetTexture")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060005A2 RID: 1442
		// (set) Token: 0x060005A3 RID: 1443
		public extern int targetDisplay
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000087C4 File Offset: 0x000069C4
		[FreeFunction("CameraScripting::SetTargetBuffers", HasExplicitThis = true)]
		private void SetTargetBuffersImpl(RenderBuffer color, RenderBuffer depth)
		{
			this.SetTargetBuffersImpl_Injected(ref color, ref depth);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x000087D0 File Offset: 0x000069D0
		public void SetTargetBuffers(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
		{
			this.SetTargetBuffersImpl(colorBuffer, depthBuffer);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x000087DC File Offset: 0x000069DC
		[FreeFunction("CameraScripting::SetTargetBuffers", HasExplicitThis = true)]
		private void SetTargetBuffersMRTImpl(RenderBuffer[] color, RenderBuffer depth)
		{
			this.SetTargetBuffersMRTImpl_Injected(color, ref depth);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000087E7 File Offset: 0x000069E7
		public void SetTargetBuffers(RenderBuffer[] colorBuffer, RenderBuffer depthBuffer)
		{
			this.SetTargetBuffersMRTImpl(colorBuffer, depthBuffer);
		}

		// Token: 0x060005A8 RID: 1448
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string[] GetCameraBufferWarnings();

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x000087F4 File Offset: 0x000069F4
		public Matrix4x4 cameraToWorldMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_cameraToWorldMatrix_Injected(out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0000880C File Offset: 0x00006A0C
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x00008822 File Offset: 0x00006A22
		public Matrix4x4 worldToCameraMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_worldToCameraMatrix_Injected(out matrix4x);
				return matrix4x;
			}
			set
			{
				this.set_worldToCameraMatrix_Injected(ref value);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0000882C File Offset: 0x00006A2C
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x00008842 File Offset: 0x00006A42
		public Matrix4x4 projectionMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_projectionMatrix_Injected(out matrix4x);
				return matrix4x;
			}
			set
			{
				this.set_projectionMatrix_Injected(ref value);
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0000884C File Offset: 0x00006A4C
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x00008862 File Offset: 0x00006A62
		public Matrix4x4 nonJitteredProjectionMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_nonJitteredProjectionMatrix_Injected(out matrix4x);
				return matrix4x;
			}
			set
			{
				this.set_nonJitteredProjectionMatrix_Injected(ref value);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060005B0 RID: 1456
		// (set) Token: 0x060005B1 RID: 1457
		[NativeProperty("UseJitteredProjectionMatrixForTransparent")]
		public extern bool useJitteredProjectionMatrixForTransparentRendering
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0000886C File Offset: 0x00006A6C
		public Matrix4x4 previousViewProjectionMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_previousViewProjectionMatrix_Injected(out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x060005B3 RID: 1459
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetWorldToCameraMatrix();

		// Token: 0x060005B4 RID: 1460
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetProjectionMatrix();

		// Token: 0x060005B5 RID: 1461 RVA: 0x00008884 File Offset: 0x00006A84
		[FreeFunction("CameraScripting::CalculateObliqueMatrix", HasExplicitThis = true)]
		public Matrix4x4 CalculateObliqueMatrix(Vector4 clipPlane)
		{
			Matrix4x4 matrix4x;
			this.CalculateObliqueMatrix_Injected(ref clipPlane, out matrix4x);
			return matrix4x;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0000889C File Offset: 0x00006A9C
		public Vector3 WorldToScreenPoint(Vector3 position, Camera.MonoOrStereoscopicEye eye)
		{
			Vector3 vector;
			this.WorldToScreenPoint_Injected(ref position, eye, out vector);
			return vector;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x000088B8 File Offset: 0x00006AB8
		public Vector3 WorldToViewportPoint(Vector3 position, Camera.MonoOrStereoscopicEye eye)
		{
			Vector3 vector;
			this.WorldToViewportPoint_Injected(ref position, eye, out vector);
			return vector;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x000088D4 File Offset: 0x00006AD4
		public Vector3 ViewportToWorldPoint(Vector3 position, Camera.MonoOrStereoscopicEye eye)
		{
			Vector3 vector;
			this.ViewportToWorldPoint_Injected(ref position, eye, out vector);
			return vector;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000088F0 File Offset: 0x00006AF0
		public Vector3 ScreenToWorldPoint(Vector3 position, Camera.MonoOrStereoscopicEye eye)
		{
			Vector3 vector;
			this.ScreenToWorldPoint_Injected(ref position, eye, out vector);
			return vector;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0000890C File Offset: 0x00006B0C
		public Vector3 WorldToScreenPoint(Vector3 position)
		{
			return this.WorldToScreenPoint(position, Camera.MonoOrStereoscopicEye.Mono);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00008928 File Offset: 0x00006B28
		public Vector3 WorldToViewportPoint(Vector3 position)
		{
			return this.WorldToViewportPoint(position, Camera.MonoOrStereoscopicEye.Mono);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00008944 File Offset: 0x00006B44
		public Vector3 ViewportToWorldPoint(Vector3 position)
		{
			return this.ViewportToWorldPoint(position, Camera.MonoOrStereoscopicEye.Mono);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00008960 File Offset: 0x00006B60
		public Vector3 ScreenToWorldPoint(Vector3 position)
		{
			return this.ScreenToWorldPoint(position, Camera.MonoOrStereoscopicEye.Mono);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0000897C File Offset: 0x00006B7C
		public Vector3 ScreenToViewportPoint(Vector3 position)
		{
			Vector3 vector;
			this.ScreenToViewportPoint_Injected(ref position, out vector);
			return vector;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00008994 File Offset: 0x00006B94
		public Vector3 ViewportToScreenPoint(Vector3 position)
		{
			Vector3 vector;
			this.ViewportToScreenPoint_Injected(ref position, out vector);
			return vector;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000089AC File Offset: 0x00006BAC
		internal Vector2 GetFrustumPlaneSizeAt(float distance)
		{
			Vector2 vector;
			this.GetFrustumPlaneSizeAt_Injected(distance, out vector);
			return vector;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000089C4 File Offset: 0x00006BC4
		private Ray ViewportPointToRay(Vector2 pos, Camera.MonoOrStereoscopicEye eye)
		{
			Ray ray;
			this.ViewportPointToRay_Injected(ref pos, eye, out ray);
			return ray;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000089E0 File Offset: 0x00006BE0
		public Ray ViewportPointToRay(Vector3 pos, Camera.MonoOrStereoscopicEye eye)
		{
			return this.ViewportPointToRay(pos, eye);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00008A00 File Offset: 0x00006C00
		public Ray ViewportPointToRay(Vector3 pos)
		{
			return this.ViewportPointToRay(pos, Camera.MonoOrStereoscopicEye.Mono);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00008A1C File Offset: 0x00006C1C
		private Ray ScreenPointToRay(Vector2 pos, Camera.MonoOrStereoscopicEye eye)
		{
			Ray ray;
			this.ScreenPointToRay_Injected(ref pos, eye, out ray);
			return ray;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00008A38 File Offset: 0x00006C38
		public Ray ScreenPointToRay(Vector3 pos, Camera.MonoOrStereoscopicEye eye)
		{
			return this.ScreenPointToRay(pos, eye);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00008A58 File Offset: 0x00006C58
		public Ray ScreenPointToRay(Vector3 pos)
		{
			return this.ScreenPointToRay(pos, Camera.MonoOrStereoscopicEye.Mono);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00008A72 File Offset: 0x00006C72
		[FreeFunction("CameraScripting::CalculateViewportRayVectors", HasExplicitThis = true)]
		private void CalculateFrustumCornersInternal(Rect viewport, float z, Camera.MonoOrStereoscopicEye eye, [Out] Vector3[] outCorners)
		{
			this.CalculateFrustumCornersInternal_Injected(ref viewport, z, eye, outCorners);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00008A80 File Offset: 0x00006C80
		public void CalculateFrustumCorners(Rect viewport, float z, Camera.MonoOrStereoscopicEye eye, Vector3[] outCorners)
		{
			bool flag = outCorners == null;
			if (flag)
			{
				throw new ArgumentNullException("outCorners");
			}
			bool flag2 = outCorners.Length < 4;
			if (flag2)
			{
				throw new ArgumentException("outCorners minimum size is 4", "outCorners");
			}
			this.CalculateFrustumCornersInternal(viewport, z, eye, outCorners);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00008AC9 File Offset: 0x00006CC9
		[NativeName("CalculateProjectionMatrixFromPhysicalProperties")]
		private static void CalculateProjectionMatrixFromPhysicalPropertiesInternal(out Matrix4x4 output, float focalLength, Vector2 sensorSize, Vector2 lensShift, float nearClip, float farClip, float gateAspect, Camera.GateFitMode gateFitMode)
		{
			Camera.CalculateProjectionMatrixFromPhysicalPropertiesInternal_Injected(out output, focalLength, ref sensorSize, ref lensShift, nearClip, farClip, gateAspect, gateFitMode);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00008ADE File Offset: 0x00006CDE
		public static void CalculateProjectionMatrixFromPhysicalProperties(out Matrix4x4 output, float focalLength, Vector2 sensorSize, Vector2 lensShift, float nearClip, float farClip, Camera.GateFitParameters gateFitParameters = default(Camera.GateFitParameters))
		{
			Camera.CalculateProjectionMatrixFromPhysicalPropertiesInternal(out output, focalLength, sensorSize, lensShift, nearClip, farClip, gateFitParameters.aspect, gateFitParameters.mode);
		}

		// Token: 0x060005CB RID: 1483
		[NativeName("FocalLengthToFieldOfView_Safe")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float FocalLengthToFieldOfView(float focalLength, float sensorSize);

		// Token: 0x060005CC RID: 1484
		[NativeName("FieldOfViewToFocalLength_Safe")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float FieldOfViewToFocalLength(float fieldOfView, float sensorSize);

		// Token: 0x060005CD RID: 1485
		[NativeName("HorizontalToVerticalFieldOfView_Safe")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float HorizontalToVerticalFieldOfView(float horizontalFieldOfView, float aspectRatio);

		// Token: 0x060005CE RID: 1486
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float VerticalToHorizontalFieldOfView(float verticalFieldOfView, float aspectRatio);

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060005CF RID: 1487
		public static extern Camera main
		{
			[FreeFunction("FindMainCamera")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060005D0 RID: 1488
		public static extern Camera current
		{
			[FreeFunction("GetCurrentCameraPPtr")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00008B00 File Offset: 0x00006D00
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x00008B16 File Offset: 0x00006D16
		public Scene scene
		{
			[FreeFunction("CameraScripting::GetScene", HasExplicitThis = true)]
			get
			{
				Scene scene;
				this.get_scene_Injected(out scene);
				return scene;
			}
			[FreeFunction("CameraScripting::SetScene", HasExplicitThis = true)]
			set
			{
				this.set_scene_Injected(ref value);
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060005D3 RID: 1491
		public extern bool stereoEnabled
		{
			[NativeMethod("GetStereoEnabledForBuiltInOrSRP")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060005D4 RID: 1492
		// (set) Token: 0x060005D5 RID: 1493
		public extern float stereoSeparation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060005D6 RID: 1494
		// (set) Token: 0x060005D7 RID: 1495
		public extern float stereoConvergence
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060005D8 RID: 1496
		public extern bool areVRStereoViewMatricesWithinSingleCullTolerance
		{
			[NativeName("AreVRStereoViewMatricesWithinSingleCullTolerance")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060005D9 RID: 1497
		// (set) Token: 0x060005DA RID: 1498
		public extern StereoTargetEyeMask stereoTargetEye
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060005DB RID: 1499
		public extern Camera.MonoOrStereoscopicEye stereoActiveEye
		{
			[FreeFunction("CameraScripting::GetStereoActiveEye", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00008B20 File Offset: 0x00006D20
		public Matrix4x4 GetStereoNonJitteredProjectionMatrix(Camera.StereoscopicEye eye)
		{
			Matrix4x4 matrix4x;
			this.GetStereoNonJitteredProjectionMatrix_Injected(eye, out matrix4x);
			return matrix4x;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00008B38 File Offset: 0x00006D38
		[FreeFunction("CameraScripting::GetStereoViewMatrix", HasExplicitThis = true)]
		public Matrix4x4 GetStereoViewMatrix(Camera.StereoscopicEye eye)
		{
			Matrix4x4 matrix4x;
			this.GetStereoViewMatrix_Injected(eye, out matrix4x);
			return matrix4x;
		}

		// Token: 0x060005DE RID: 1502
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CopyStereoDeviceProjectionMatrixToNonJittered(Camera.StereoscopicEye eye);

		// Token: 0x060005DF RID: 1503 RVA: 0x00008B50 File Offset: 0x00006D50
		[FreeFunction("CameraScripting::GetStereoProjectionMatrix", HasExplicitThis = true)]
		public Matrix4x4 GetStereoProjectionMatrix(Camera.StereoscopicEye eye)
		{
			Matrix4x4 matrix4x;
			this.GetStereoProjectionMatrix_Injected(eye, out matrix4x);
			return matrix4x;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00008B67 File Offset: 0x00006D67
		public void SetStereoProjectionMatrix(Camera.StereoscopicEye eye, Matrix4x4 matrix)
		{
			this.SetStereoProjectionMatrix_Injected(eye, ref matrix);
		}

		// Token: 0x060005E1 RID: 1505
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetStereoProjectionMatrices();

		// Token: 0x060005E2 RID: 1506 RVA: 0x00008B72 File Offset: 0x00006D72
		public void SetStereoViewMatrix(Camera.StereoscopicEye eye, Matrix4x4 matrix)
		{
			this.SetStereoViewMatrix_Injected(eye, ref matrix);
		}

		// Token: 0x060005E3 RID: 1507
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetStereoViewMatrices();

		// Token: 0x060005E4 RID: 1508
		[FreeFunction("CameraScripting::GetAllCamerasCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetAllCamerasCount();

		// Token: 0x060005E5 RID: 1509
		[FreeFunction("CameraScripting::GetAllCameras")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetAllCamerasImpl([NotNull("ArgumentNullException")] [Out] Camera[] cam);

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00008B80 File Offset: 0x00006D80
		public static int allCamerasCount
		{
			get
			{
				return Camera.GetAllCamerasCount();
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00008B98 File Offset: 0x00006D98
		public static Camera[] allCameras
		{
			get
			{
				Camera[] array = new Camera[Camera.allCamerasCount];
				Camera.GetAllCamerasImpl(array);
				return array;
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00008BC0 File Offset: 0x00006DC0
		public static int GetAllCameras(Camera[] cameras)
		{
			bool flag = cameras == null;
			if (flag)
			{
				throw new NullReferenceException();
			}
			bool flag2 = cameras.Length < Camera.allCamerasCount;
			if (flag2)
			{
				throw new ArgumentException("Passed in array to fill with cameras is to small to hold the number of cameras. Use Camera.allCamerasCount to get the needed size.");
			}
			return Camera.GetAllCamerasImpl(cameras);
		}

		// Token: 0x060005E9 RID: 1513
		[FreeFunction("CameraScripting::RenderToCubemap", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool RenderToCubemapImpl(Texture tex, [DefaultValue("63")] int faceMask);

		// Token: 0x060005EA RID: 1514 RVA: 0x00008C00 File Offset: 0x00006E00
		public bool RenderToCubemap(Cubemap cubemap, int faceMask)
		{
			return this.RenderToCubemapImpl(cubemap, faceMask);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00008C1C File Offset: 0x00006E1C
		public bool RenderToCubemap(Cubemap cubemap)
		{
			return this.RenderToCubemapImpl(cubemap, 63);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00008C38 File Offset: 0x00006E38
		public bool RenderToCubemap(RenderTexture cubemap, int faceMask)
		{
			return this.RenderToCubemapImpl(cubemap, faceMask);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00008C54 File Offset: 0x00006E54
		public bool RenderToCubemap(RenderTexture cubemap)
		{
			return this.RenderToCubemapImpl(cubemap, 63);
		}

		// Token: 0x060005EE RID: 1518
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetFilterMode();

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x00008C70 File Offset: 0x00006E70
		[NativeConditional("UNITY_EDITOR")]
		public Camera.SceneViewFilterMode sceneViewFilterMode
		{
			get
			{
				return (Camera.SceneViewFilterMode)this.GetFilterMode();
			}
		}

		// Token: 0x060005F0 RID: 1520
		[NativeName("RenderToCubemap")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool RenderToCubemapEyeImpl(RenderTexture cubemap, int faceMask, Camera.MonoOrStereoscopicEye stereoEye);

		// Token: 0x060005F1 RID: 1521 RVA: 0x00008C88 File Offset: 0x00006E88
		public bool RenderToCubemap(RenderTexture cubemap, int faceMask, Camera.MonoOrStereoscopicEye stereoEye)
		{
			return this.RenderToCubemapEyeImpl(cubemap, faceMask, stereoEye);
		}

		// Token: 0x060005F2 RID: 1522
		[FreeFunction("CameraScripting::Render", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Render();

		// Token: 0x060005F3 RID: 1523
		[FreeFunction("CameraScripting::RenderWithShader", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RenderWithShader(Shader shader, string replacementTag);

		// Token: 0x060005F4 RID: 1524
		[FreeFunction("CameraScripting::RenderDontRestore", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RenderDontRestore();

		// Token: 0x060005F5 RID: 1525 RVA: 0x00008CA4 File Offset: 0x00006EA4
		[Obsolete("SubmitRenderRequests is obsolete, use SubmitRenderRequest with RequestData of supported types such as RenderPipeline.StandardRequest", true)]
		public void SubmitRenderRequests(List<Camera.RenderRequest> renderRequests)
		{
			bool flag = renderRequests == null || renderRequests.Count == 0;
			if (flag)
			{
				throw new ArgumentException("SubmitRenderRequests has been invoked with invalid renderRequests");
			}
			bool flag2 = GraphicsSettings.currentRenderPipeline == null;
			if (flag2)
			{
				Debug.LogWarning("Trying to invoke 'SubmitRenderRequests' when no SRP is set. A scriptable render pipeline is needed for this function call");
			}
			else
			{
				this.SubmitRenderRequestsInternal(renderRequests);
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00008CF8 File Offset: 0x00006EF8
		public void SubmitRenderRequest<RequestData>(RequestData renderRequest)
		{
			bool flag = renderRequest == null;
			if (flag)
			{
				throw new ArgumentException("SubmitRenderRequests is invoked with invalid renderRequests");
			}
			ObjectIdRequest objectIdRequest = renderRequest as ObjectIdRequest;
			bool flag2 = objectIdRequest != null;
			if (flag2)
			{
				bool flag3 = objectIdRequest.destination.depthStencilFormat == GraphicsFormat.None;
				if (flag3)
				{
					Debug.LogWarning("ObjectId Render Request submitted without a depth stencil, which can produce results that are not depth tested correctly");
				}
				bool flag4 = GraphicsSettings.currentRenderPipeline == null || !RenderPipelineManager.currentPipeline.IsRenderRequestSupported<ObjectIdRequest>(this, objectIdRequest);
				if (flag4)
				{
					throw new ArgumentException((GraphicsSettings.currentRenderPipeline == null) ? "The Built-In Render Pipeline does not support ObjectIdRequest outside of the editor." : "The current render pipeline does not support ObjectIdRequest, and the fallback implementation of the Built-In Render Pipeline is not available outside of the editor.");
				}
			}
			bool flag5 = GraphicsSettings.currentRenderPipeline == null;
			if (flag5)
			{
				Debug.LogWarning("Trying to invoke 'SubmitRenderRequest' when no SRP is set. A scriptable render pipeline is needed for this function call");
			}
			else
			{
				this.SubmitRenderRequestsInternal(renderRequest);
			}
		}

		// Token: 0x060005F7 RID: 1527
		[FreeFunction("CameraScripting::SubmitRenderRequests", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SubmitRenderRequestsInternal(object requests);

		// Token: 0x060005F8 RID: 1528
		[NativeConditional("UNITY_EDITOR")]
		[FreeFunction("CameraScripting::SubmitBuiltInObjectIDRenderRequest", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Object[] SubmitBuiltInObjectIDRenderRequest(RenderTexture target, int mipLevel, CubemapFace cubemapFace, int depthSlice);

		// Token: 0x060005F9 RID: 1529
		[FreeFunction("CameraScripting::SetupCurrent")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetupCurrent(Camera cur);

		// Token: 0x060005FA RID: 1530
		[FreeFunction("CameraScripting::CopyFrom", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CopyFrom(Camera other);

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060005FB RID: 1531
		public extern int commandBufferCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060005FC RID: 1532
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveCommandBuffers(CameraEvent evt);

		// Token: 0x060005FD RID: 1533
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveAllCommandBuffers();

		// Token: 0x060005FE RID: 1534
		[NativeName("AddCommandBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddCommandBufferImpl(CameraEvent evt, [NotNull("ArgumentNullException")] CommandBuffer buffer);

		// Token: 0x060005FF RID: 1535
		[NativeName("AddCommandBufferAsync")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddCommandBufferAsyncImpl(CameraEvent evt, [NotNull("ArgumentNullException")] CommandBuffer buffer, ComputeQueueType queueType);

		// Token: 0x06000600 RID: 1536
		[NativeName("RemoveCommandBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RemoveCommandBufferImpl(CameraEvent evt, [NotNull("ArgumentNullException")] CommandBuffer buffer);

		// Token: 0x06000601 RID: 1537 RVA: 0x00008DC4 File Offset: 0x00006FC4
		public void AddCommandBuffer(CameraEvent evt, CommandBuffer buffer)
		{
			bool flag = !CameraEventUtils.IsValid(evt);
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid CameraEvent value \"{0}\".", (int)evt), "evt");
			}
			bool flag2 = buffer == null;
			if (flag2)
			{
				throw new NullReferenceException("buffer is null");
			}
			this.AddCommandBufferImpl(evt, buffer);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00008E18 File Offset: 0x00007018
		public void AddCommandBufferAsync(CameraEvent evt, CommandBuffer buffer, ComputeQueueType queueType)
		{
			bool flag = !CameraEventUtils.IsValid(evt);
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid CameraEvent value \"{0}\".", (int)evt), "evt");
			}
			bool flag2 = buffer == null;
			if (flag2)
			{
				throw new NullReferenceException("buffer is null");
			}
			this.AddCommandBufferAsyncImpl(evt, buffer, queueType);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00008E6C File Offset: 0x0000706C
		public void RemoveCommandBuffer(CameraEvent evt, CommandBuffer buffer)
		{
			bool flag = !CameraEventUtils.IsValid(evt);
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid CameraEvent value \"{0}\".", (int)evt), "evt");
			}
			bool flag2 = buffer == null;
			if (flag2)
			{
				throw new NullReferenceException("buffer is null");
			}
			this.RemoveCommandBufferImpl(evt, buffer);
		}

		// Token: 0x06000604 RID: 1540
		[FreeFunction("CameraScripting::GetCommandBuffers", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern CommandBuffer[] GetCommandBuffers(CameraEvent evt);

		// Token: 0x06000605 RID: 1541 RVA: 0x00008EC0 File Offset: 0x000070C0
		[RequiredByNativeCode]
		private static void FireOnPreCull(Camera cam)
		{
			bool flag = Camera.onPreCull != null;
			if (flag)
			{
				Camera.onPreCull(cam);
			}
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00008EE8 File Offset: 0x000070E8
		[RequiredByNativeCode]
		private static void FireOnPreRender(Camera cam)
		{
			bool flag = Camera.onPreRender != null;
			if (flag)
			{
				Camera.onPreRender(cam);
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00008F10 File Offset: 0x00007110
		[RequiredByNativeCode]
		private static void FireOnPostRender(Camera cam)
		{
			bool flag = Camera.onPostRender != null;
			if (flag)
			{
				Camera.onPostRender(cam);
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00002669 File Offset: 0x00000869
		internal void OnlyUsedForTesting1()
		{
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00002669 File Offset: 0x00000869
		internal void OnlyUsedForTesting2()
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00008F38 File Offset: 0x00007138
		public unsafe bool TryGetCullingParameters(out ScriptableCullingParameters cullingParameters)
		{
			return Camera.GetCullingParameters_Internal(this, false, out cullingParameters, sizeof(ScriptableCullingParameters));
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00008F58 File Offset: 0x00007158
		public unsafe bool TryGetCullingParameters(bool stereoAware, out ScriptableCullingParameters cullingParameters)
		{
			return Camera.GetCullingParameters_Internal(this, stereoAware, out cullingParameters, sizeof(ScriptableCullingParameters));
		}

		// Token: 0x0600060C RID: 1548
		[FreeFunction("ScriptableRenderPipeline_Bindings::GetCullingParameters_Internal")]
		[NativeHeader("Runtime/Export/RenderPipeline/ScriptableRenderPipeline.bindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetCullingParameters_Internal(Camera camera, bool stereoAware, out ScriptableCullingParameters cullingParameters, int managedCullingParametersSize);

		// Token: 0x0600060D RID: 1549
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_transparencySortAxis_Injected(out Vector3 ret);

		// Token: 0x0600060E RID: 1550
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_transparencySortAxis_Injected(ref Vector3 value);

		// Token: 0x0600060F RID: 1551
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_velocity_Injected(out Vector3 ret);

		// Token: 0x06000610 RID: 1552
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_cullingMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06000611 RID: 1553
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_cullingMatrix_Injected(ref Matrix4x4 value);

		// Token: 0x06000612 RID: 1554
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_backgroundColor_Injected(out Color ret);

		// Token: 0x06000613 RID: 1555
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_backgroundColor_Injected(ref Color value);

		// Token: 0x06000614 RID: 1556
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_curvature_Injected(out Vector2 ret);

		// Token: 0x06000615 RID: 1557
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_curvature_Injected(ref Vector2 value);

		// Token: 0x06000616 RID: 1558
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_sensorSize_Injected(out Vector2 ret);

		// Token: 0x06000617 RID: 1559
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_sensorSize_Injected(ref Vector2 value);

		// Token: 0x06000618 RID: 1560
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_lensShift_Injected(out Vector2 ret);

		// Token: 0x06000619 RID: 1561
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_lensShift_Injected(ref Vector2 value);

		// Token: 0x0600061A RID: 1562
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetGateFittedLensShift_Injected(out Vector2 ret);

		// Token: 0x0600061B RID: 1563
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetLocalSpaceAim_Injected(out Vector3 ret);

		// Token: 0x0600061C RID: 1564
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_rect_Injected(out Rect ret);

		// Token: 0x0600061D RID: 1565
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_rect_Injected(ref Rect value);

		// Token: 0x0600061E RID: 1566
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_pixelRect_Injected(out Rect ret);

		// Token: 0x0600061F RID: 1567
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_pixelRect_Injected(ref Rect value);

		// Token: 0x06000620 RID: 1568
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTargetBuffersImpl_Injected(ref RenderBuffer color, ref RenderBuffer depth);

		// Token: 0x06000621 RID: 1569
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTargetBuffersMRTImpl_Injected(RenderBuffer[] color, ref RenderBuffer depth);

		// Token: 0x06000622 RID: 1570
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_cameraToWorldMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06000623 RID: 1571
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_worldToCameraMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06000624 RID: 1572
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_worldToCameraMatrix_Injected(ref Matrix4x4 value);

		// Token: 0x06000625 RID: 1573
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_projectionMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06000626 RID: 1574
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_projectionMatrix_Injected(ref Matrix4x4 value);

		// Token: 0x06000627 RID: 1575
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_nonJitteredProjectionMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06000628 RID: 1576
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_nonJitteredProjectionMatrix_Injected(ref Matrix4x4 value);

		// Token: 0x06000629 RID: 1577
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_previousViewProjectionMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x0600062A RID: 1578
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CalculateObliqueMatrix_Injected(ref Vector4 clipPlane, out Matrix4x4 ret);

		// Token: 0x0600062B RID: 1579
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void WorldToScreenPoint_Injected(ref Vector3 position, Camera.MonoOrStereoscopicEye eye, out Vector3 ret);

		// Token: 0x0600062C RID: 1580
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void WorldToViewportPoint_Injected(ref Vector3 position, Camera.MonoOrStereoscopicEye eye, out Vector3 ret);

		// Token: 0x0600062D RID: 1581
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ViewportToWorldPoint_Injected(ref Vector3 position, Camera.MonoOrStereoscopicEye eye, out Vector3 ret);

		// Token: 0x0600062E RID: 1582
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ScreenToWorldPoint_Injected(ref Vector3 position, Camera.MonoOrStereoscopicEye eye, out Vector3 ret);

		// Token: 0x0600062F RID: 1583
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ScreenToViewportPoint_Injected(ref Vector3 position, out Vector3 ret);

		// Token: 0x06000630 RID: 1584
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ViewportToScreenPoint_Injected(ref Vector3 position, out Vector3 ret);

		// Token: 0x06000631 RID: 1585
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetFrustumPlaneSizeAt_Injected(float distance, out Vector2 ret);

		// Token: 0x06000632 RID: 1586
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ViewportPointToRay_Injected(ref Vector2 pos, Camera.MonoOrStereoscopicEye eye, out Ray ret);

		// Token: 0x06000633 RID: 1587
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ScreenPointToRay_Injected(ref Vector2 pos, Camera.MonoOrStereoscopicEye eye, out Ray ret);

		// Token: 0x06000634 RID: 1588
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CalculateFrustumCornersInternal_Injected(ref Rect viewport, float z, Camera.MonoOrStereoscopicEye eye, [Out] Vector3[] outCorners);

		// Token: 0x06000635 RID: 1589
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CalculateProjectionMatrixFromPhysicalPropertiesInternal_Injected(out Matrix4x4 output, float focalLength, ref Vector2 sensorSize, ref Vector2 lensShift, float nearClip, float farClip, float gateAspect, Camera.GateFitMode gateFitMode);

		// Token: 0x06000636 RID: 1590
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_scene_Injected(out Scene ret);

		// Token: 0x06000637 RID: 1591
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_scene_Injected(ref Scene value);

		// Token: 0x06000638 RID: 1592
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetStereoNonJitteredProjectionMatrix_Injected(Camera.StereoscopicEye eye, out Matrix4x4 ret);

		// Token: 0x06000639 RID: 1593
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetStereoViewMatrix_Injected(Camera.StereoscopicEye eye, out Matrix4x4 ret);

		// Token: 0x0600063A RID: 1594
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetStereoProjectionMatrix_Injected(Camera.StereoscopicEye eye, out Matrix4x4 ret);

		// Token: 0x0600063B RID: 1595
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetStereoProjectionMatrix_Injected(Camera.StereoscopicEye eye, ref Matrix4x4 matrix);

		// Token: 0x0600063C RID: 1596
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetStereoViewMatrix_Injected(Camera.StereoscopicEye eye, ref Matrix4x4 matrix);

		// Token: 0x0400034A RID: 842
		public const float kMinAperture = 0.7f;

		// Token: 0x0400034B RID: 843
		public const float kMaxAperture = 32f;

		// Token: 0x0400034C RID: 844
		public const int kMinBladeCount = 3;

		// Token: 0x0400034D RID: 845
		public const int kMaxBladeCount = 11;

		// Token: 0x0400034E RID: 846
		public static Camera.CameraCallback onPreCull;

		// Token: 0x0400034F RID: 847
		public static Camera.CameraCallback onPreRender;

		// Token: 0x04000350 RID: 848
		public static Camera.CameraCallback onPostRender;

		// Token: 0x02000103 RID: 259
		internal enum ProjectionMatrixMode
		{
			// Token: 0x04000352 RID: 850
			Explicit,
			// Token: 0x04000353 RID: 851
			Implicit,
			// Token: 0x04000354 RID: 852
			PhysicalPropertiesBased
		}

		// Token: 0x02000104 RID: 260
		public enum GateFitMode
		{
			// Token: 0x04000356 RID: 854
			Vertical = 1,
			// Token: 0x04000357 RID: 855
			Horizontal,
			// Token: 0x04000358 RID: 856
			Fill,
			// Token: 0x04000359 RID: 857
			Overscan,
			// Token: 0x0400035A RID: 858
			None = 0
		}

		// Token: 0x02000105 RID: 261
		public enum FieldOfViewAxis
		{
			// Token: 0x0400035C RID: 860
			Vertical,
			// Token: 0x0400035D RID: 861
			Horizontal
		}

		// Token: 0x02000106 RID: 262
		public struct GateFitParameters
		{
			// Token: 0x17000147 RID: 327
			// (get) Token: 0x0600063D RID: 1597 RVA: 0x00008F78 File Offset: 0x00007178
			// (set) Token: 0x0600063E RID: 1598 RVA: 0x00008F80 File Offset: 0x00007180
			public Camera.GateFitMode mode { readonly get; set; }

			// Token: 0x17000148 RID: 328
			// (get) Token: 0x0600063F RID: 1599 RVA: 0x00008F89 File Offset: 0x00007189
			// (set) Token: 0x06000640 RID: 1600 RVA: 0x00008F91 File Offset: 0x00007191
			public float aspect { readonly get; set; }

			// Token: 0x06000641 RID: 1601 RVA: 0x00008F9A File Offset: 0x0000719A
			public GateFitParameters(Camera.GateFitMode mode, float aspect)
			{
				this.mode = mode;
				this.aspect = aspect;
			}
		}

		// Token: 0x02000107 RID: 263
		public enum StereoscopicEye
		{
			// Token: 0x04000361 RID: 865
			Left,
			// Token: 0x04000362 RID: 866
			Right
		}

		// Token: 0x02000108 RID: 264
		public enum MonoOrStereoscopicEye
		{
			// Token: 0x04000364 RID: 868
			Left,
			// Token: 0x04000365 RID: 869
			Right,
			// Token: 0x04000366 RID: 870
			Mono
		}

		// Token: 0x02000109 RID: 265
		public enum SceneViewFilterMode
		{
			// Token: 0x04000368 RID: 872
			Off,
			// Token: 0x04000369 RID: 873
			ShowFiltered
		}

		// Token: 0x0200010A RID: 266
		[Obsolete("The RenderRequest struct is obsolete, use the function overload with RequestData of supported types such as RenderPipeline.StandardRequest", true)]
		public enum RenderRequestMode
		{
			// Token: 0x0400036B RID: 875
			None,
			// Token: 0x0400036C RID: 876
			ObjectId,
			// Token: 0x0400036D RID: 877
			Depth,
			// Token: 0x0400036E RID: 878
			VertexNormal,
			// Token: 0x0400036F RID: 879
			WorldPosition,
			// Token: 0x04000370 RID: 880
			EntityId,
			// Token: 0x04000371 RID: 881
			BaseColor,
			// Token: 0x04000372 RID: 882
			SpecularColor,
			// Token: 0x04000373 RID: 883
			Metallic,
			// Token: 0x04000374 RID: 884
			Emission,
			// Token: 0x04000375 RID: 885
			Normal,
			// Token: 0x04000376 RID: 886
			Smoothness,
			// Token: 0x04000377 RID: 887
			Occlusion,
			// Token: 0x04000378 RID: 888
			DiffuseColor
		}

		// Token: 0x0200010B RID: 267
		[Obsolete("The RenderRequest struct is obsolete, use the function overload with RequestData of supported types such as RenderPipeline.StandardRequest", true)]
		public enum RenderRequestOutputSpace
		{
			// Token: 0x0400037A RID: 890
			ScreenSpace = -1,
			// Token: 0x0400037B RID: 891
			UV0,
			// Token: 0x0400037C RID: 892
			UV1,
			// Token: 0x0400037D RID: 893
			UV2,
			// Token: 0x0400037E RID: 894
			UV3,
			// Token: 0x0400037F RID: 895
			UV4,
			// Token: 0x04000380 RID: 896
			UV5,
			// Token: 0x04000381 RID: 897
			UV6,
			// Token: 0x04000382 RID: 898
			UV7,
			// Token: 0x04000383 RID: 899
			UV8
		}

		// Token: 0x0200010C RID: 268
		[Obsolete("The RenderRequest struct is obsolete, use the function overload with RequestData of supported types such as RenderPipeline.StandardRequest", true)]
		public struct RenderRequest
		{
			// Token: 0x06000642 RID: 1602 RVA: 0x00008FAD File Offset: 0x000071AD
			public RenderRequest(Camera.RenderRequestMode mode, RenderTexture rt)
			{
				this.m_CameraRenderMode = mode;
				this.m_ResultRT = rt;
				this.m_OutputSpace = Camera.RenderRequestOutputSpace.ScreenSpace;
			}

			// Token: 0x06000643 RID: 1603 RVA: 0x00008FC5 File Offset: 0x000071C5
			public RenderRequest(Camera.RenderRequestMode mode, Camera.RenderRequestOutputSpace space, RenderTexture rt)
			{
				this.m_CameraRenderMode = mode;
				this.m_ResultRT = rt;
				this.m_OutputSpace = space;
			}

			// Token: 0x17000149 RID: 329
			// (get) Token: 0x06000644 RID: 1604 RVA: 0x00008FDD File Offset: 0x000071DD
			public bool isValid
			{
				get
				{
					return this.m_CameraRenderMode != Camera.RenderRequestMode.None && this.m_ResultRT != null;
				}
			}

			// Token: 0x1700014A RID: 330
			// (get) Token: 0x06000645 RID: 1605 RVA: 0x00008FF6 File Offset: 0x000071F6
			public Camera.RenderRequestMode mode
			{
				get
				{
					return this.m_CameraRenderMode;
				}
			}

			// Token: 0x1700014B RID: 331
			// (get) Token: 0x06000646 RID: 1606 RVA: 0x00008FFE File Offset: 0x000071FE
			public RenderTexture result
			{
				get
				{
					return this.m_ResultRT;
				}
			}

			// Token: 0x1700014C RID: 332
			// (get) Token: 0x06000647 RID: 1607 RVA: 0x00009006 File Offset: 0x00007206
			public Camera.RenderRequestOutputSpace outputSpace
			{
				get
				{
					return this.m_OutputSpace;
				}
			}

			// Token: 0x04000384 RID: 900
			private readonly Camera.RenderRequestMode m_CameraRenderMode;

			// Token: 0x04000385 RID: 901
			private readonly RenderTexture m_ResultRT;

			// Token: 0x04000386 RID: 902
			private readonly Camera.RenderRequestOutputSpace m_OutputSpace;
		}

		// Token: 0x0200010D RID: 269
		// (Invoke) Token: 0x06000649 RID: 1609
		public delegate void CameraCallback(Camera cam);
	}
}
