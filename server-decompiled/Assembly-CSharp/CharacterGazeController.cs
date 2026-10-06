using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A53 RID: 2643
public class CharacterGazeController : MonoBehaviour
{
	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x06004F97 RID: 20375 RVA: 0x001E456C File Offset: 0x001E276C
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

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x06004F98 RID: 20376 RVA: 0x001E45CB File Offset: 0x001E27CB
	// (set) Token: 0x06004F99 RID: 20377 RVA: 0x001E45D3 File Offset: 0x001E27D3
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

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x06004F9A RID: 20378 RVA: 0x001E45DC File Offset: 0x001E27DC
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

	// Token: 0x06004F9B RID: 20379 RVA: 0x001E460C File Offset: 0x001E280C
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

	// Token: 0x06004F9C RID: 20380 RVA: 0x001E46A9 File Offset: 0x001E28A9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		CharacterGazeController.instances.Add(this);
	}

	// Token: 0x06004F9D RID: 20381 RVA: 0x001E46B6 File Offset: 0x001E28B6
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (CharacterGazeController.instances.Contains(this))
		{
			CharacterGazeController.instances.Remove(this);
		}
	}

	// Token: 0x06004F9E RID: 20382 RVA: 0x001E46D1 File Offset: 0x001E28D1
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

	// Token: 0x06004F9F RID: 20383 RVA: 0x001E470C File Offset: 0x001E290C
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

	// Token: 0x06004FA0 RID: 20384 RVA: 0x001E4760 File Offset: 0x001E2960
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

	// Token: 0x06004FA1 RID: 20385 RVA: 0x001E4A8B File Offset: 0x001E2C8B
	public void SnapNextUpdate()
	{
		this.shouldSnapHeadNextUpdate = true;
		this.shouldSnapEyesNextUpdate = true;
	}

	// Token: 0x06004FA2 RID: 20386 RVA: 0x001E4A9C File Offset: 0x001E2C9C
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

	// Token: 0x06004FA3 RID: 20387 RVA: 0x001E4C78 File Offset: 0x001E2E78
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

	// Token: 0x06004FA4 RID: 20388 RVA: 0x001E4EA6 File Offset: 0x001E30A6
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void Cleanup()
	{
		CharacterGazeController.instances.Clear();
	}

	// Token: 0x04003D45 RID: 15685
	public static List<CharacterGazeController> instances = new List<CharacterGazeController>();

	// Token: 0x04003D46 RID: 15686
	public Transform leftEyeTransform;

	// Token: 0x04003D47 RID: 15687
	public Transform rightEyeTransform;

	// Token: 0x04003D48 RID: 15688
	public Vector3 leftEyeLocalPosition;

	// Token: 0x04003D49 RID: 15689
	public Vector3 rightEyeLocalPosition;

	// Token: 0x04003D4A RID: 15690
	public Transform rootTransform;

	// Token: 0x04003D4B RID: 15691
	public Transform neckTransform;

	// Token: 0x04003D4C RID: 15692
	public Transform headTransform;

	// Token: 0x04003D4D RID: 15693
	[Range(0f, 100f)]
	public float eyeRotationSpeed = 30f;

	// Token: 0x04003D4E RID: 15694
	[Range(0f, 100f)]
	public float headRotationSpeed = 7f;

	// Token: 0x04003D4F RID: 15695
	[Range(0f, 50f)]
	public float twitchSpeed = 25f;

	// Token: 0x04003D50 RID: 15696
	public float eyeLookAtTargetAngle = 35f;

	// Token: 0x04003D51 RID: 15697
	public float headLookAtTargetAngle = 75f;

	// Token: 0x04003D52 RID: 15698
	[Range(0f, 20f)]
	public float maxLookAtDistance = 5f;

	// Token: 0x04003D53 RID: 15699
	public SkinnedMeshRenderer eyeSkinnedMeshRenderer;

	// Token: 0x04003D54 RID: 15700
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material eyeMaterial;

	// Token: 0x04003D55 RID: 15701
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform lookAtTransformOverride;

	// Token: 0x04003D56 RID: 15702
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lookAtCamera;

	// Token: 0x04003D57 RID: 15703
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform lookAtTarget;

	// Token: 0x04003D58 RID: 15704
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameRandom random;

	// Token: 0x04003D59 RID: 15705
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

	// Token: 0x04003D5A RID: 15706
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive entityAlive;

	// Token: 0x04003D5B RID: 15707
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion currentLeftEyeRotation;

	// Token: 0x04003D5C RID: 15708
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion currentRightEyeRotation;

	// Token: 0x04003D5D RID: 15709
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion currentHeadRotation;

	// Token: 0x04003D5E RID: 15710
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float gazeTimer;

	// Token: 0x04003D5F RID: 15711
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int lookatOffsetIndex;

	// Token: 0x04003D60 RID: 15712
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool shouldSnapHeadNextUpdate;

	// Token: 0x04003D61 RID: 15713
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool shouldSnapEyesNextUpdate;

	// Token: 0x04003D62 RID: 15714
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDead;

	// Token: 0x04003D63 RID: 15715
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 lookAtTargetDefaultPosition = new Vector3(0f, 1.7f, 10f);
}
