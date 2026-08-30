using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001195 RID: 4501
public class DebugLines : MonoBehaviour
{
	// Token: 0x06008FF0 RID: 36848 RVA: 0x00361756 File Offset: 0x0035F956
	public static DebugLines Create(string _name, Transform _parentT, Color _color1, Color _color2, float _width1, float _width2, float _duration)
	{
		DebugLines debugLines = DebugLines.Create(_name, _parentT);
		debugLines.duration = _duration;
		LineRenderer lineRenderer = debugLines.line;
		lineRenderer.startColor = _color1;
		lineRenderer.startWidth = _width1;
		lineRenderer.endColor = _color2;
		lineRenderer.endWidth = _width2;
		return debugLines;
	}

	// Token: 0x06008FF1 RID: 36849 RVA: 0x0036178C File Offset: 0x0035F98C
	public static DebugLines Create(string _name, Transform _parentT, Vector3 _pos1, Vector3 _pos2, Color _color1, Color _color2, float _width1, float _width2, float _duration)
	{
		DebugLines debugLines = DebugLines.Create(_name, _parentT);
		debugLines.duration = _duration;
		LineRenderer lineRenderer = debugLines.line;
		lineRenderer.startColor = _color1;
		lineRenderer.startWidth = _width1;
		lineRenderer.endColor = _color2;
		lineRenderer.endWidth = _width2;
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, _pos1 - Origin.position);
		lineRenderer.SetPosition(1, _pos2 - Origin.position);
		return debugLines;
	}

	// Token: 0x06008FF2 RID: 36850 RVA: 0x003617F8 File Offset: 0x0035F9F8
	public static DebugLines CreateAttached(string _name, Transform _parentT, Vector3 _pos1, Vector3 _pos2, Color _color1, Color _color2, float _width1, float _width2, float _duration)
	{
		DebugLines debugLines = DebugLines.Create(_name, _parentT);
		debugLines.duration = _duration;
		LineRenderer lineRenderer = debugLines.line;
		lineRenderer.useWorldSpace = false;
		lineRenderer.startColor = _color1;
		lineRenderer.startWidth = _width1;
		lineRenderer.endColor = _color2;
		lineRenderer.endWidth = _width2;
		lineRenderer.positionCount = 2;
		Vector3 vector = _parentT.InverseTransformPoint(_pos1 - Origin.position);
		lineRenderer.SetPosition(0, vector);
		Vector3 vector2 = _parentT.InverseTransformPoint(_pos2 - Origin.position);
		lineRenderer.SetPosition(1, vector2);
		return debugLines;
	}

	// Token: 0x06008FF3 RID: 36851 RVA: 0x0036187C File Offset: 0x0035FA7C
	[PublicizedFrom(EAccessModifier.Private)]
	public static DebugLines Create(string _name, Transform _parentT)
	{
		DebugLines debugLines = null;
		string text = "DebugLines";
		if (_name != null)
		{
			text += _name;
			DebugLines.lines.TryGetValue(_name, out debugLines);
		}
		if (!debugLines)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>((GameObject)Resources.Load("Prefabs/Debug/DebugLines"), _parentT);
			gameObject.name = text;
			debugLines = gameObject.transform.GetComponent<DebugLines>();
			if (_name != null)
			{
				debugLines.keyName = _name;
				DebugLines.lines[_name] = debugLines;
			}
		}
		else
		{
			debugLines.transform.SetParent(_parentT, false);
		}
		debugLines.line = debugLines.GetComponent<LineRenderer>();
		debugLines.line.positionCount = 0;
		return debugLines;
	}

	// Token: 0x06008FF4 RID: 36852 RVA: 0x00361918 File Offset: 0x0035FB18
	public void AddPoint(Vector3 _pos)
	{
		int positionCount = this.line.positionCount;
		this.line.positionCount = positionCount + 1;
		Vector3 vector = _pos - Origin.position;
		if (!this.line.useWorldSpace)
		{
			vector = base.transform.InverseTransformPoint(vector);
		}
		this.line.SetPosition(positionCount, vector);
	}

	// Token: 0x06008FF5 RID: 36853 RVA: 0x00361974 File Offset: 0x0035FB74
	public void AddCube(Vector3 _cornerPos1, Vector3 _cornerPos2)
	{
		Vector3 vector = _cornerPos1;
		Vector3 vector2 = _cornerPos2 - _cornerPos1;
		this.AddPoint(vector);
		vector.x += vector2.x;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.y -= vector2.y;
		this.AddPoint(vector);
		vector.z += vector2.z;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.y -= vector2.y;
		this.AddPoint(vector);
		vector.x -= vector2.x;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.y -= vector2.y;
		this.AddPoint(vector);
		vector.z -= vector2.z;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.x += vector2.x;
		this.AddPoint(vector);
		vector.z += vector2.z;
		this.AddPoint(vector);
		vector.x -= vector2.x;
		this.AddPoint(vector);
		vector.z -= vector2.z;
		this.AddPoint(vector);
	}

	// Token: 0x06008FF6 RID: 36854 RVA: 0x00361AFA File Offset: 0x0035FCFA
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.duration -= Time.deltaTime;
		if (this.duration <= 0f)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06008FF7 RID: 36855 RVA: 0x00361B26 File Offset: 0x0035FD26
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		if (this.keyName != null)
		{
			DebugLines.lines.Remove(this.keyName);
		}
	}

	// Token: 0x04006A68 RID: 27240
	public static Vector3 InsideOffsetV = new Vector3(0.05f, 0.05f, 0.05f);

	// Token: 0x04006A69 RID: 27241
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cName = "DebugLines";

	// Token: 0x04006A6A RID: 27242
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<string, DebugLines> lines = new Dictionary<string, DebugLines>();

	// Token: 0x04006A6B RID: 27243
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string keyName;

	// Token: 0x04006A6C RID: 27244
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float duration;

	// Token: 0x04006A6D RID: 27245
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LineRenderer line;
}
