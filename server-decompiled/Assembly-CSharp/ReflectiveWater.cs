using System;
using System.Collections;
using UnityEngine;

// Token: 0x020012A7 RID: 4775
public class ReflectiveWater : MonoBehaviour
{
	// Token: 0x0600975C RID: 38748 RVA: 0x003969C8 File Offset: 0x00394BC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.gameManager = global::UnityEngine.Object.FindAnyObjectByType<GameManager>();
	}

	// Token: 0x0600975D RID: 38749 RVA: 0x003969D8 File Offset: 0x00394BD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnPreRender()
	{
		if (!GamePrefs.GetBool(EnumGamePrefs.OptionsGfxWaterQuality))
		{
			return;
		}
		Camera current = Camera.current;
		if (!current)
		{
			return;
		}
		if (this.gameManager.World == null || this.gameManager.World.GetPrimaryPlayer() == null)
		{
			return;
		}
		if (ReflectiveWater.s_InsideWater)
		{
			return;
		}
		ReflectiveWater.s_InsideWater = true;
		int waterMode = (int)this.m_WaterMode;
		Camera camera;
		Camera camera2;
		this.CreateWaterObjects(current, out camera, out camera2);
		Vector3 position = this.gameManager.World.GetPrimaryPlayer().transform.position;
		position.y = 60f;
		Vector3 up = Vector3.up;
		int pixelLightCount = QualitySettings.pixelLightCount;
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = 0;
		}
		this.UpdateCameraModes(current, camera);
		if (waterMode >= 1)
		{
			float num = -Vector3.Dot(up, position) - this.m_ClipPlaneOffset;
			Vector4 vector = new Vector4(up.x, up.y, up.z, num);
			Matrix4x4 zero = Matrix4x4.zero;
			ReflectiveWater.CalculateReflectionMatrix(ref zero, vector);
			Vector3 position2 = current.transform.position;
			Vector3 vector2 = zero.MultiplyPoint(position2);
			camera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
			Vector4 vector3 = this.CameraSpacePlane(camera, position, up, 1f);
			Matrix4x4 projectionMatrix = current.projectionMatrix;
			ReflectiveWater.CalculateObliqueMatrix(ref projectionMatrix, vector3);
			camera.projectionMatrix = projectionMatrix;
			camera.cullingMask = -17 & this.m_ReflectLayers.value;
			camera.targetTexture = this.m_ReflectionTexture;
			GL.invertCulling = true;
			camera.transform.position = vector2;
			camera.Render();
			GL.invertCulling = false;
			Shader.SetGlobalTexture("_ReflectionTex", this.m_ReflectionTexture);
		}
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = pixelLightCount;
		}
		ReflectiveWater.s_InsideWater = false;
	}

	// Token: 0x0600975E RID: 38750 RVA: 0x00396B8C File Offset: 0x00394D8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		if (this.m_ReflectionTexture)
		{
			global::UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
			this.m_ReflectionTexture = null;
		}
		if (this.m_RefractionTexture)
		{
			global::UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
			this.m_RefractionTexture = null;
		}
		foreach (object obj in this.m_ReflectionCameras)
		{
			global::UnityEngine.Object.DestroyImmediate(((Camera)((DictionaryEntry)obj).Value).gameObject);
		}
		this.m_ReflectionCameras.Clear();
		foreach (object obj2 in this.m_RefractionCameras)
		{
			global::UnityEngine.Object.DestroyImmediate(((Camera)((DictionaryEntry)obj2).Value).gameObject);
		}
		this.m_RefractionCameras.Clear();
	}

	// Token: 0x0600975F RID: 38751 RVA: 0x00396CA0 File Offset: 0x00394EA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (!this.gameManager.gameStateManager.IsGameStarted() || GameStats.GetInt(EnumGameStats.GameState) != 1)
		{
			return;
		}
		if (MeshDescription.meshes.Length < 1)
		{
			return;
		}
		if (MeshDescription.meshes[1].material == null)
		{
			return;
		}
		if (!MeshDescription.meshes[1].material.HasProperty("WaveSpeed") || !MeshDescription.meshes[1].material.HasProperty("_WaveScale"))
		{
			return;
		}
		Vector4 vector = MeshDescription.meshes[1].material.GetVector("WaveSpeed");
		float @float = MeshDescription.meshes[1].material.GetFloat("_WaveScale");
		Vector4 vector2 = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
		double num = (double)Time.timeSinceLevelLoad / 200.0;
		Vector4 vector3 = new Vector4((float)Math.IEEERemainder((double)(vector.x * vector2.x) * num, 1.0), (float)Math.IEEERemainder((double)(vector.y * vector2.y) * num, 1.0), (float)Math.IEEERemainder((double)(vector.z * vector2.z) * num, 1.0), (float)Math.IEEERemainder((double)(vector.w * vector2.w) * num, 1.0));
		MeshDescription.meshes[1].material.SetVector("_WaveOffset", vector3);
		MeshDescription.meshes[1].material.SetVector("_WaveScale4", vector2);
	}

	// Token: 0x06009760 RID: 38752 RVA: 0x00396E24 File Offset: 0x00395024
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateCameraModes(Camera src, Camera dest)
	{
		if (dest == null)
		{
			return;
		}
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		if (src.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox skybox = src.GetComponent(typeof(Skybox)) as Skybox;
			Skybox skybox2 = dest.GetComponent(typeof(Skybox)) as Skybox;
			if (!skybox || !skybox.material)
			{
				skybox2.enabled = false;
			}
			else
			{
				skybox2.enabled = true;
				skybox2.material = skybox.material;
			}
		}
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
	}

	// Token: 0x06009761 RID: 38753 RVA: 0x00396F04 File Offset: 0x00395104
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
	{
		int waterMode = (int)this.GetWaterMode();
		reflectionCamera = null;
		refractionCamera = null;
		if (waterMode >= 1)
		{
			if (!this.m_ReflectionTexture || this.m_OldReflectionTextureSize != this.m_TextureSize)
			{
				if (this.m_ReflectionTexture)
				{
					global::UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
				}
				this.m_ReflectionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
				this.m_ReflectionTexture.name = "__WaterReflection" + base.GetInstanceID().ToString();
				this.m_ReflectionTexture.isPowerOfTwo = true;
				this.m_ReflectionTexture.hideFlags = HideFlags.DontSave;
				this.m_OldReflectionTextureSize = this.m_TextureSize;
			}
			reflectionCamera = this.m_ReflectionCameras[currentCamera] as Camera;
			if (!reflectionCamera)
			{
				GameObject gameObject = new GameObject("Water Refl Camera id" + base.GetInstanceID().ToString() + " for " + currentCamera.GetInstanceID().ToString(), new Type[]
				{
					typeof(Camera),
					typeof(Skybox)
				});
				reflectionCamera = gameObject.GetComponent<Camera>();
				reflectionCamera.enabled = false;
				reflectionCamera.transform.position = base.transform.position;
				reflectionCamera.transform.rotation = base.transform.rotation;
				reflectionCamera.gameObject.AddComponent<FlareLayer>();
				gameObject.hideFlags = HideFlags.DontSave;
				this.m_ReflectionCameras[currentCamera] = reflectionCamera;
			}
		}
	}

	// Token: 0x06009762 RID: 38754 RVA: 0x00397088 File Offset: 0x00395288
	[PublicizedFrom(EAccessModifier.Private)]
	public ReflectiveWater.WaterMode GetWaterMode()
	{
		if (this.m_HardwareWaterSupport < this.m_WaterMode)
		{
			return this.m_HardwareWaterSupport;
		}
		return this.m_WaterMode;
	}

	// Token: 0x06009763 RID: 38755 RVA: 0x003970A8 File Offset: 0x003952A8
	[PublicizedFrom(EAccessModifier.Private)]
	public ReflectiveWater.WaterMode FindHardwareWaterSupport()
	{
		if (!base.GetComponent<Renderer>())
		{
			return ReflectiveWater.WaterMode.Simple;
		}
		Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
		if (!sharedMaterial)
		{
			return ReflectiveWater.WaterMode.Simple;
		}
		string tag = sharedMaterial.GetTag("WATERMODE", false);
		if (tag == "Refractive")
		{
			return ReflectiveWater.WaterMode.Refractive;
		}
		if (tag == "Reflective")
		{
			return ReflectiveWater.WaterMode.Reflective;
		}
		return ReflectiveWater.WaterMode.Simple;
	}

	// Token: 0x06009764 RID: 38756 RVA: 0x00397106 File Offset: 0x00395306
	[PublicizedFrom(EAccessModifier.Private)]
	public static float sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	// Token: 0x06009765 RID: 38757 RVA: 0x0039712C File Offset: 0x0039532C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 vector = pos + normal * this.m_ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 vector2 = worldToCameraMatrix.MultiplyPoint(vector);
		Vector3 vector3 = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(vector3.x, vector3.y, vector3.z, -Vector3.Dot(vector2, vector3));
	}

	// Token: 0x06009766 RID: 38758 RVA: 0x00397194 File Offset: 0x00395394
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 vector = projection.inverse * new Vector4(ReflectiveWater.sgn(clipPlane.x), ReflectiveWater.sgn(clipPlane.y), 1f, 1f);
		Vector4 vector2 = clipPlane * (2f / Vector4.Dot(clipPlane, vector));
		projection[2] = vector2.x - projection[3];
		projection[6] = vector2.y - projection[7];
		projection[10] = vector2.z - projection[11];
		projection[14] = vector2.w - projection[15];
	}

	// Token: 0x06009767 RID: 38759 RVA: 0x00397240 File Offset: 0x00395440
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
	}

	// Token: 0x04007198 RID: 29080
	public ReflectiveWater.WaterMode m_WaterMode = ReflectiveWater.WaterMode.Refractive;

	// Token: 0x04007199 RID: 29081
	public bool m_DisablePixelLights = true;

	// Token: 0x0400719A RID: 29082
	public int m_TextureSize = 256;

	// Token: 0x0400719B RID: 29083
	public float m_ClipPlaneOffset = 0.07f;

	// Token: 0x0400719C RID: 29084
	public LayerMask m_ReflectLayers = -1;

	// Token: 0x0400719D RID: 29085
	public LayerMask m_RefractLayers = -1;

	// Token: 0x0400719E RID: 29086
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Hashtable m_ReflectionCameras = new Hashtable();

	// Token: 0x0400719F RID: 29087
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Hashtable m_RefractionCameras = new Hashtable();

	// Token: 0x040071A0 RID: 29088
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public RenderTexture m_ReflectionTexture;

	// Token: 0x040071A1 RID: 29089
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public RenderTexture m_RefractionTexture;

	// Token: 0x040071A2 RID: 29090
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ReflectiveWater.WaterMode m_HardwareWaterSupport = ReflectiveWater.WaterMode.Refractive;

	// Token: 0x040071A3 RID: 29091
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_OldReflectionTextureSize;

	// Token: 0x040071A4 RID: 29092
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool s_InsideWater;

	// Token: 0x040071A5 RID: 29093
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameManager gameManager;

	// Token: 0x020012A8 RID: 4776
	public enum WaterMode
	{
		// Token: 0x040071A7 RID: 29095
		Simple,
		// Token: 0x040071A8 RID: 29096
		Reflective,
		// Token: 0x040071A9 RID: 29097
		Refractive
	}
}
