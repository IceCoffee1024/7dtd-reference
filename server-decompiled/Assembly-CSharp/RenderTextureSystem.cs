using System;
using UnityEngine;

// Token: 0x020014C5 RID: 5317
public class RenderTextureSystem
{
	// Token: 0x0600A64E RID: 42574 RVA: 0x003DEF24 File Offset: 0x003DD124
	public void Create(string _name, GameObject _target, Vector3 _targetRelPos, Vector3 _lightSourceRelPos, Vector2i _renderTexSize, bool _isAA, bool _orthographic = false, float _orthoSize = 1f)
	{
		GameObject gameObject = GameObject.Find("3D RenderTextures");
		if (gameObject == null)
		{
			gameObject = new GameObject("3D RenderTextures");
			gameObject.layer = 11;
		}
		this.ParentGO = new GameObject(_name);
		Transform transform = this.ParentGO.transform;
		transform.SetParent(gameObject.transform);
		this.ParentGO.layer = 11;
		this.TargetGO = _target;
		this.TargetGO.transform.SetParent(transform);
		this.TargetGO.transform.localPosition = _targetRelPos;
		this.TargetGO.layer = 11;
		this.CameraGO = new GameObject("Camera");
		this.CameraGO.transform.SetParent(transform);
		this.CameraGO.transform.LookAt(_target.transform);
		this.CameraGO.layer = 11;
		this.cam = this.CameraGO.AddComponent<Camera>();
		this.cam.nearClipPlane = 0.01f;
		this.cam.farClipPlane = 20f;
		this.cam.cullingMask = 2048;
		this.cam.renderingPath = RenderingPath.Forward;
		this.cam.clearFlags = CameraClearFlags.Color;
		if (_orthographic)
		{
			this.cam.orthographic = _orthographic;
			this.cam.orthographicSize = _orthoSize;
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
			foreach (Renderer renderer in this.TargetGO.GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(renderer.bounds);
			}
			this.TargetGO.transform.localPosition = _targetRelPos - bounds.center / 2f;
			if (bounds.size.y > bounds.size.x && bounds.size.y > bounds.size.z)
			{
				this.cam.orthographicSize = bounds.size.magnitude / 2f * _orthoSize;
			}
			else
			{
				this.cam.orthographicSize = bounds.size.magnitude / 3f * _orthoSize;
			}
		}
		int num = (_isAA ? 2 : 1);
		this.RenderTex = new RenderTexture(_renderTexSize.x * num, _renderTexSize.y * num, 24);
		this.RenderTex.autoGenerateMips = _isAA;
		this.RenderTex.useMipMap = _isAA;
		this.cam.targetTexture = this.RenderTex;
		this.LightGO = new GameObject("Light");
		this.LightGO.transform.SetParent(transform);
		this.LightGO.transform.localPosition = _lightSourceRelPos;
		this.LightGO.layer = 11;
		Light light = this.LightGO.AddComponent<Light>();
		light.type = LightType.Point;
		light.intensity = 1.5f;
		light.bounceIntensity = 0f;
		light.range = _targetRelPos.magnitude * 10f;
		light.cullingMask = 2048;
	}

	// Token: 0x0600A64F RID: 42575 RVA: 0x003DF240 File Offset: 0x003DD440
	public void SetTarget(GameObject _target, Vector3 _targetRelPos, bool _orthographic, float _orthoSize = 1f)
	{
		if (this.TargetGO != null)
		{
			global::UnityEngine.Object.Destroy(this.TargetGO);
		}
		this.TargetGO = global::UnityEngine.Object.Instantiate<GameObject>(_target);
		this.TargetGO.transform.parent = this.ParentGO.transform;
		this.TargetGO.transform.localPosition = Vector3.zero;
		this.recursiveLayerSetup(this.TargetGO, 11);
		if (_orthographic)
		{
			this.cam.orthographic = _orthographic;
			this.cam.orthographicSize = _orthoSize;
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
			foreach (Renderer renderer in this.TargetGO.GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(renderer.bounds);
			}
			this.TargetGO.transform.localPosition = _targetRelPos - bounds.center / 2f;
			if (bounds.size.y > bounds.size.x && bounds.size.y > bounds.size.z)
			{
				this.cam.orthographicSize = bounds.size.magnitude / 2f * _orthoSize;
				return;
			}
			this.cam.orthographicSize = bounds.size.magnitude / 3f * _orthoSize;
		}
	}

	// Token: 0x0600A650 RID: 42576 RVA: 0x003DF3B0 File Offset: 0x003DD5B0
	public void SetTargetNoCopy(GameObject _target, Vector3 _targetRelPos, bool _orthographic, float _orthoSize = 1f)
	{
		if (this.TargetGO != null)
		{
			global::UnityEngine.Object.Destroy(this.TargetGO);
		}
		Light component = this.LightGO.GetComponent<Light>();
		component.range = 14f;
		component.intensity = 2f;
		this.TargetGO = _target;
		this.TargetGO.transform.parent = this.ParentGO.transform;
		this.TargetGO.transform.localPosition = Vector3.zero;
		this.recursiveLayerSetup(_target, 11);
		if (_orthographic)
		{
			this.cam.orthographic = _orthographic;
			this.cam.orthographicSize = _orthoSize;
			this.cam.backgroundColor = new Color32(0, 0, 0, 0);
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
			foreach (Renderer renderer in this.TargetGO.GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(renderer.bounds);
			}
			this.TargetGO.transform.localPosition = _targetRelPos - bounds.center / 2f;
		}
	}

	// Token: 0x0600A651 RID: 42577 RVA: 0x003DF4D1 File Offset: 0x003DD6D1
	public void SetOrtho(bool enabled, float _orthoSize)
	{
		this.cam.orthographic = enabled;
		this.cam.orthographicSize = _orthoSize;
	}

	// Token: 0x0600A652 RID: 42578 RVA: 0x003DF4EC File Offset: 0x003DD6EC
	public void RotateTarget(float _amount)
	{
		if (this.TargetGO != null)
		{
			Vector3 eulerAngles = this.TargetGO.transform.localRotation.eulerAngles;
			this.TargetGO.transform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y + _amount, eulerAngles.z);
		}
	}

	// Token: 0x0600A653 RID: 42579 RVA: 0x003DF54C File Offset: 0x003DD74C
	[PublicizedFrom(EAccessModifier.Private)]
	public void recursiveLayerSetup(GameObject _go, int _layer)
	{
		Transform transform = _go.transform;
		transform.gameObject.layer = _layer;
		foreach (object obj in transform)
		{
			Transform transform2 = (Transform)obj;
			transform2.gameObject.layer = _layer;
			this.recursiveLayerSetup(transform2.gameObject, _layer);
		}
	}

	// Token: 0x0600A654 RID: 42580 RVA: 0x003DF5C4 File Offset: 0x003DD7C4
	public void Create(string _name, Camera _existingCamera, Vector2i _renderTexSize)
	{
		GameObject gameObject = GameObject.Find("3D RenderTextures");
		if (gameObject == null)
		{
			gameObject = new GameObject("3D RenderTextures");
			gameObject.layer = 11;
		}
		this.ParentGO = new GameObject(_name);
		this.ParentGO.transform.parent = gameObject.transform;
		this.ParentGO.layer = 11;
		this.RenderTex = new RenderTexture(_renderTexSize.x, _renderTexSize.y, 24);
		_existingCamera.targetTexture = this.RenderTex;
	}

	// Token: 0x0600A655 RID: 42581 RVA: 0x003DF64C File Offset: 0x003DD84C
	public void SetEnabled(bool _b)
	{
		this.ParentGO.SetActive(_b);
	}

	// Token: 0x0600A656 RID: 42582 RVA: 0x003DF65A File Offset: 0x003DD85A
	public void Cleanup()
	{
		if (this.ParentGO != null)
		{
			global::UnityEngine.Object.Destroy(this.ParentGO);
		}
		if (this.RenderTex != null)
		{
			global::UnityEngine.Object.Destroy(this.RenderTex);
		}
	}

	// Token: 0x04007BDC RID: 31708
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cSceneParentName = "3D RenderTextures";

	// Token: 0x04007BDD RID: 31709
	public GameObject ParentGO;

	// Token: 0x04007BDE RID: 31710
	public GameObject CameraGO;

	// Token: 0x04007BDF RID: 31711
	public GameObject TargetGO;

	// Token: 0x04007BE0 RID: 31712
	public GameObject LightGO;

	// Token: 0x04007BE1 RID: 31713
	public RenderTexture RenderTex;

	// Token: 0x04007BE2 RID: 31714
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera cam;
}
