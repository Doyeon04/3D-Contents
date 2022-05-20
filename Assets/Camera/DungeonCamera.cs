using UnityEngine;
using System.Collections;

public class DungeonCamera : MonoBehaviour {
	public GameObject target;
	public float damping = 1;
	Vector3 offset;

	void Start() {
		offset = transform.position - target.transform.position; // 카메라와 캐릭터 사이의 간격 
	}
	
	void LateUpdate() {
		Vector3 desiredPosition = target.transform.position + offset;
		Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
		transform.position = position;

		transform.LookAt(target.transform.position); // 카메라는 항상 타겟을 바라보게
	}
}
