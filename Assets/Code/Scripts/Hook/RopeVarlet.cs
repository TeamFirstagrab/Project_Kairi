using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using tagName = Globals.TagName;

public class RopeVarlet : MonoBehaviour
{
	[Header("줄")]
	public int segmentCnt = 50;
	public float segmentLength = 0.225f;

	[Header("중력")]
	public Vector2 gravityForce = new Vector2(0f, -2f);   // 로프 중력값
	public float dampingFactor = 0.98f;   // 제동 계수 (과도한 흔들림 제어용)

	[Header("제약 조건")]
	public int constraintRuns = 100;   // 실행 횟수

	private LineRenderer line;  // 실제로 로프를 그리기 위한 라인렌더러
	private List<HookSegment> hookSegments = new List<HookSegment>();

	private Vector3 ropeStartPoint;

	private void Awake()
	{
		line = GetComponent<LineRenderer>();
		line.positionCount = segmentCnt;

		ropeStartPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

		for (int i = 0; i < segmentCnt; i++)
		{
			hookSegments.Add(new HookSegment(ropeStartPoint));
			ropeStartPoint.y -= segmentLength;
		}
	}

	private void Update()
	{
		DrawRope();
	}

	private void FixedUpdate()
	{
		Simulate();

		for (int i = 0; i < segmentCnt; i++)
		{
			ApplyContraints();
		}
	}

	private void DrawRope()
	{
		Vector3[] ropePos = new Vector3[segmentCnt];
		for (int i = 0; i < hookSegments.Count; i++)
		{
			ropePos[i] = hookSegments[i].CurrPos;
		}

		line.SetPositions(ropePos);
	}

	private void Simulate()
	{
		for (int i = 1; i < hookSegments.Count; i++)
		{
			HookSegment segment = hookSegments[i];
			Vector2 velocity = (segment.CurrPos - segment.OldPos) * dampingFactor;

			segment.OldPos = segment.CurrPos;
			segment.CurrPos += velocity;
			segment.CurrPos += gravityForce * Time.fixedDeltaTime;
			hookSegments[i] = segment;  // 현재 세그먼트 리스트에 적용하기
		}
	}

	// 세그먼트 위치 조정하기 (Verlet 적산법 사용)
	private void ApplyContraints()
	{
		HookSegment firstSegment = hookSegments[0];
		firstSegment.CurrPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());   // 첫 번째 세그먼트는 마우스 위치
		hookSegments[0] = firstSegment;     // 현재 세그먼트 리스트에도 반영

		for (int i = 0; i < segmentCnt - 1; i++)
		{
			HookSegment currSeg = hookSegments[i];
			HookSegment nextSeg = hookSegments[i + 1];

			float dist = (currSeg.CurrPos - nextSeg.CurrPos).magnitude;   // 두 세그먼트 사이 거리 계산
			float difference = (dist - segmentLength);  // 세그먼트 길이 차이 계산

			Vector2 changeDir = (currSeg.CurrPos - nextSeg.CurrPos).normalized;     // 변경할 세그먼트 방향 정규화
			Vector2 changeVector = changeDir * difference;                          // 변경할 세그먼트 위치 벡터값 계산

			if (i != 0)  // 첫 번째 세그먼트가 아닐 경우 해당 세그먼트와 다음 세그먼트에 수정값을 분배
			{
				currSeg.CurrPos -= (changeVector * 0.9f);
				nextSeg.CurrPos += (changeVector * 0.9f);
			}
			else    // 첫 번째 세그먼트일 경우 전체 보정값을 다음 세그먼트에 적용
			{
				nextSeg.CurrPos += changeVector;
			}
			hookSegments[i] = currSeg;  // 현재 세그먼트 리스트에 반영
			hookSegments[i + 1] = nextSeg;
		}
	}

	// 세그먼트 구조체 생성
	public struct HookSegment
	{
		public Vector2 CurrPos;     // 현재 세그먼트 위치
		public Vector2 OldPos;      // 이전 세그먼트 위치

		public HookSegment(Vector2 pos)
		{
			CurrPos = pos;
			OldPos = pos;
		}
	}
}
