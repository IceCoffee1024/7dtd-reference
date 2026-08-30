using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A38 RID: 2616
public class CharacterGazeController : MonoBehaviour
{
	// Token: 0x1700087A RID: 2170
	// (get) Token: 0x06004F42 RID: 20290 RVA: 0x001E0824 File Offset: 0x001DEA24
	public Transform LookAtTarget
	{
		get
		{
			if (this.lookAtTarget == null)
			{
				this.lookAtTarget = new GameObject("LookAtTarget").transform;
				this.lookAtTarget.parent = this.rootTransform;
				this.lookAtTarget.localPosition = this.lookAtTargetDefaultPosition;
				return this.lookAtTarget;
			}
			return this.lookAtTarget;
		}
	}

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x06004F43 RID: 20291 RVA: 0x001E0883 File Offset: 0x001DEA83
	// (set) Token: 0x06004F44 RID: 20292 RVA: 0x001E088B File Offset: 0x001DEA8B
	public Transform LookAtTransformOverride
	{
		get
		{
			return this.lookAtTransformOverride;
		}
		set
		{
			this.lookAtTransformOverride = value;
		}
	}

	// Token: 0x1700087C RID: 2172
	// (get) Token: 0x06004F45 RID: 20293 RVA: 0x001E0894 File Offset: 0x001DEA94
	public GameRandom Random
	{
		get
		{
			if (this.random != null)
			{
				return this.random;
			}
			return this.random = GameRandomManager.Instance.CreateGameRandom();
		}
	}

	// Token: 0x06004F46 RID: 20294 RVA: 0x001E08C4 File Offset: 0x001DEAC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.entityAlive = base.GetComponentInParent<EntityAlive>();
		this.eyeMaterial = new Material(this.eyeSkinnedMeshRenderer.material);
		if (this.eyeMaterial == null || this.eyeMaterial.shader.name != "Game/SDCS/Eye")
		{
			Debug.LogError("Eye Material is not valid");
			base.enabled = false;
			return;
		}
		this.eyeSkinnedMeshRenderer.material = this.eyeMaterial;
		this.gazeTimer = Time.realtimeSinceStartup + this.Random.RandomRange(0.25f, 2f);
	}

	// Token: 0x06004F47 RID: 20295 RVA: 0x001E0961 File Offset: 0x001DEB61
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		CharacterGazeController.instances.Add(this);
	}

	// Token: 0x06004F48 RID: 20296 RVA: 0x001E096E File Offset: 0x001DEB6E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (CharacterGazeController.instances.Contains(this))
		{
			CharacterGazeController.instances.Remove(this);
		}
	}

	// Token: 0x06004F49 RID: 20297 RVA: 0x001E0989 File Offset: 0x001DEB89
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		if (CharacterGazeController.instances.Contains(this))
		{
			CharacterGazeController.instances.Remove(this);
		}
		if (this.lookAtTarget != null)
		{
			global::UnityEngine.Object.Destroy(this.lookAtTarget.gameObject);
		}
	}

	// Token: 0x06004F4A RID: 20298 RVA: 0x001E09C4 File Offset: 0x001DEBC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.entityAlive != null)
		{
			if (this.entityAlive.emodel.IsRagdollActive)
			{
				return;
			}
			this.isDead = this.entityAlive.IsDead();
		}
		this.UpdateLookAtTarget();
		this.UpdateHeadRotation();
		this.UpdateEyeGaze();
	}

	// Token: 0x06004F4B RID: 20299 RVA: 0x001E0A18 File Offset: 0x001DEC18
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLookAtTarget()
	{
		if (this.lookAtTransformOverride != null)
		{
			this.lookAtCamera = true;
			this.LookAtTarget.position = this.lookAtTransformOverride.position;
			return;
		}
		CharacterGazeController characterGazeController = null;
		float num = float.PositiveInfinity;
		for (int i = CharacterGazeController.instances.Count - 1; i >= 0; i--)
		{
			CharacterGazeController characterGazeController2 = CharacterGazeController.instances[i];
			if (characterGazeController2 == null)
			{
				CharacterGazeController.instances.RemoveAt(i);
			}
			else if (characterGazeController2.enabled && characterGazeController2 != this)
			{
				Vector3 vector = characterGazeController2.headTransform.position - this.headTransform.position;
				float num2 = Vector3.Angle(this.neckTransform.forward, vector);
				float magnitude = vector.magnitude;
				if (num2 < this.eyeLookAtTargetAngle && magnitude <= this.maxLookAtDistance && num2 < num)
				{
					characterGazeController = characterGazeController2;
					num = num2;
				}
			}
		}
		if (characterGazeController != null)
		{
			this.lookAtCamera = true;
			this.LookAtTarget.position = characterGazeController.headTransform.position;
		}
		else
		{
			bool flag = this.entityAlive is EntityPlayerLocal;
			EntityPlayerLocal entityPlayerLocal;
			if (GameManager.Instance != null && GameManager.Instance.World != null)
			{
				entityPlayerLocal = GameManager.Instance.World.GetPrimaryPlayer();
			}
			else
			{
				entityPlayerLocal = null;
			}
			Vector3 vector2 = this.rootTransform.TransformPoint(this.lookAtTargetDefaultPosition);
			if (entityPlayerLocal != null && entityPlayerLocal.cameraTransform != null)
			{
				if (flag)
				{
					if (entityPlayerLocal.bFirstPersonView)
					{
						vector2 = entityPlayerLocal.cameraTransform.position;
					}
					else if (entityPlayerLocal.IsCameraFacingCharacter())
					{
						vector2 = entityPlayerLocal.cameraTransform.position;
					}
					else
					{
						vector2 = entityPlayerLocal.cameraTransform.position + entityPlayerLocal.cameraTransform.forward * 10f;
					}
				}
				else if (entityPlayerLocal.bFirstPersonView)
				{
					vector2 = entityPlayerLocal.cameraTransform.position;
				}
				else
				{
					vector2 = entityPlayerLocal.getHeadPosition();
				}
			}
			else if (Camera.main != null)
			{
				vector2 = Camera.main.transform.position;
			}
			Vector3 vector3 = vector2 - this.headTransform.position;
			float num3 = Vector3.Angle(this.neckTransform.forward, vector3);
			float magnitude2 = vector3.magnitude;
			this.lookAtCamera = false;
			if (num3 < this.headLookAtTargetAngle && magnitude2 <= this.maxLookAtDistance)
			{
				this.lookAtCamera = true;
			}
			else if (entityPlayerLocal != null && flag && !entityPlayerLocal.bFirstPersonView && !entityPlayerLocal.inventory.holdingItem.IsActionRunning(entityPlayerLocal.inventory.holdingItemData))
			{
				this.lookAtCamera = true;
			}
			if (this.lookAtCamera)
			{
				this.LookAtTarget.position = vector2;
			}
			else
			{
				this.LookAtTarget.localPosition = this.lookAtTargetDefaultPosition;
			}
		}
		if (Time.realtimeSinceStartup > this.gazeTimer)
		{
			this.lookatOffsetIndex = this.Random.RandomRange(0, this.lookatOffsets.Count);
			this.gazeTimer = Time.realtimeSinceStartup + this.Random.RandomRange(0.25f, 2f);
		}
	}

	// Token: 0x06004F4C RID: 20300 RVA: 0x001E0D43 File Offset: 0x001DEF43
	public void SnapNextUpdate()
	{
		this.shouldSnapHeadNextUpdate = true;
		this.shouldSnapEyesNextUpdate = true;
	}

	// Token: 0x06004F4D RID: 20301 RVA: 0x001E0D54 File Offset: 0x001DEF54
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateHeadRotation()
	{
		Vector3 normalized = (this.LookAtTarget.position - this.headTransform.position).normalized;
		float num = Vector3.Angle(this.neckTransform.forward, normalized);
		Vector3 normalized2 = this.neckTransform.InverseTransformDirection(normalized).normalized;
		float num2 = Mathf.Atan2(normalized2.x, normalized2.z) * 57.29578f;
		float num3 = Mathf.Atan2(normalized2.y, new Vector2(normalized2.x, normalized2.z).magnitude) * 57.29578f;
		num2 *= 0.5f;
		float num4 = num3 * 0.75f;
		Quaternion quaternion = Quaternion.AngleAxis(num2, this.neckTransform.up);
		Vector3 vector = quaternion * this.neckTransform.forward;
		Vector3 vector2 = quaternion * this.neckTransform.right;
		Vector3 normalized3 = (Quaternion.AngleAxis(-num4, vector2) * vector).normalized;
		if (num < this.headLookAtTargetAngle && !this.isDead)
		{
			Quaternion quaternion2 = Quaternion.FromToRotation(this.neckTransform.forward, normalized3);
			quaternion2 *= Quaternion.Euler(0f, 0f, -(num2 / this.headLookAtTargetAngle) * 15f);
			if (this.shouldSnapHeadNextUpdate)
			{
				this.currentHeadRotation = quaternion2;
				this.shouldSnapHeadNextUpdate = false;
			}
			else
			{
				this.currentHeadRotation = Quaternion.Slerp(this.currentHeadRotation, quaternion2, this.headRotationSpeed * Time.deltaTime);
			}
		}
		else if (this.shouldSnapHeadNextUpdate)
		{
			this.currentHeadRotation = Quaternion.identity;
			this.shouldSnapHeadNextUpdate = false;
		}
		else
		{
			this.currentHeadRotation = Quaternion.Slerp(this.currentHeadRotation, Quaternion.identity, this.headRotationSpeed * Time.deltaTime);
		}
		this.headTransform.rotation = this.currentHeadRotation * this.neckTransform.rotation;
	}

	// Token: 0x06004F4E RID: 20302 RVA: 0x001E0F30 File Offset: 0x001DF130
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEyeGaze()
	{
		Vector3 localPosition = this.LookAtTarget.localPosition;
		if (!this.lookAtCamera)
		{
			this.LookAtTarget.position += this.lookatOffsets[this.lookatOffsetIndex];
		}
		Vector3 vector = this.LookAtTarget.position - this.headTransform.TransformPoint(this.leftEyeLocalPosition);
		Vector3 vector2 = this.LookAtTarget.position - this.headTransform.TransformPoint(this.rightEyeLocalPosition);
		Quaternion quaternion = Quaternion.FromToRotation(this.leftEyeTransform.forward, vector);
		Quaternion quaternion2 = Quaternion.FromToRotation(this.rightEyeTransform.forward, vector2);
		float num = Mathf.Sin(Time.time * this.twitchSpeed) * 0.5f + 0.5f;
		if (this.isDead)
		{
			quaternion = Quaternion.identity;
			quaternion2 = Quaternion.identity;
		}
		if (this.shouldSnapEyesNextUpdate)
		{
			this.currentLeftEyeRotation = quaternion;
			this.currentRightEyeRotation = quaternion2;
			this.shouldSnapEyesNextUpdate = false;
		}
		else
		{
			this.currentLeftEyeRotation = Quaternion.Slerp(this.currentLeftEyeRotation, quaternion, this.eyeRotationSpeed * num * Time.deltaTime);
			this.currentRightEyeRotation = Quaternion.Slerp(this.currentRightEyeRotation, quaternion2, this.eyeRotationSpeed * num * Time.deltaTime);
		}
		this.eyeMaterial.SetVector("_LeftEyeRotation", new Vector4(-this.currentLeftEyeRotation.x, this.currentLeftEyeRotation.y, this.currentLeftEyeRotation.z, this.currentLeftEyeRotation.w));
		this.eyeMaterial.SetVector("_RightEyeRotation", new Vector4(-this.currentRightEyeRotation.x, this.currentRightEyeRotation.y, this.currentRightEyeRotation.z, this.currentRightEyeRotation.w));
		this.eyeMaterial.SetVector("_LeftEyePosition", Vector3.Scale(this.leftEyeLocalPosition, this.leftEyeTransform.lossyScale));
		this.eyeMaterial.SetVector("_RightEyePosition", Vector3.Scale(this.rightEyeLocalPosition, this.rightEyeTransform.lossyScale));
		this.LookAtTarget.localPosition = localPosition;
	}

	// Token: 0x06004F4F RID: 20303 RVA: 0x001E115E File Offset: 0x001DF35E
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void Cleanup()
	{
		CharacterGazeController.instances.Clear();
	}

	// Token: 0x04003CA7 RID: 15527
	public static List<CharacterGazeController> instances = new List<CharacterGazeController>();

	// Token: 0x04003CA8 RID: 15528
	public Transform leftEyeTransform;

	// Token: 0x04003CA9 RID: 15529
	public Transform rightEyeTransform;

	// Token: 0x04003CAA RID: 15530
	public Vector3 leftEyeLocalPosition;

	// Token: 0x04003CAB RID: 15531
	public Vector3 rightEyeLocalPosition;

	// Token: 0x04003CAC RID: 15532
	public Transform rootTransform;

	// Token: 0x04003CAD RID: 15533
	public Transform neckTransform;

	// Token: 0x04003CAE RID: 15534
	public Transform headTransform;

	// Token: 0x04003CAF RID: 15535
	[Range(0f, 100f)]
	public float eyeRotationSpeed = 30f;

	// Token: 0x04003CB0 RID: 15536
	[Range(0f, 100f)]
	public float headRotationSpeed = 7f;

	// Token: 0x04003CB1 RID: 15537
	[Range(0f, 50f)]
	public float twitchSpeed = 25f;

	// Token: 0x04003CB2 RID: 15538
	public float eyeLookAtTargetAngle = 35f;

	// Token: 0x04003CB3 RID: 15539
	public float headLookAtTargetAngle = 75f;

	// Token: 0x04003CB4 RID: 15540
	[Range(0f, 20f)]
	public float maxLookAtDistance = 5f;

	// Token: 0x04003CB5 RID: 15541
	public SkinnedMeshRenderer eyeSkinnedMeshRenderer;

	// Token: 0x04003CB6 RID: 15542
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material eyeMaterial;

	// Token: 0x04003CB7 RID: 15543
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform lookAtTransformOverride;

	// Token: 0x04003CB8 RID: 15544
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lookAtCamera;

	// Token: 0x04003CB9 RID: 15545
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform lookAtTarget;

	// Token: 0x04003CBA RID: 15546
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameRandom random;

	// Token: 0x04003CBB RID: 15547
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> lookatOffsets = new List<Vector3>
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(-1f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(-2f, 0f, 0f),
		new Vector3(2f, 0f, 0f),
		new Vector3(0f, 0f, -1f),
		new Vector3(0f, 0f, 1f),
		new Vector3(0f, 0f, -2f),
		new Vector3(0f, 0f, 2f)
	};

	// Token: 0x04003CBC RID: 15548
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive entityAlive;

	// Token: 0x04003CBD RID: 15549
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion currentLeftEyeRotation;

	// Token: 0x04003CBE RID: 15550
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion currentRightEyeRotation;

	// Token: 0x04003CBF RID: 15551
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion currentHeadRotation;

	// Token: 0x04003CC0 RID: 15552
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float gazeTimer;

	// Token: 0x04003CC1 RID: 15553
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int lookatOffsetIndex;

	// Token: 0x04003CC2 RID: 15554
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool shouldSnapHeadNextUpdate;

	// Token: 0x04003CC3 RID: 15555
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool shouldSnapEyesNextUpdate;

	// Token: 0x04003CC4 RID: 15556
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDead;

	// Token: 0x04003CC5 RID: 15557
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 lookAtTargetDefaultPosition = new Vector3(0f, 1.7f, 10f);
}
