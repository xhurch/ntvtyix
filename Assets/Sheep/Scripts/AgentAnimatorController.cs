using UnityEngine;
using System.Collections;

public class AgentAnimatorController : MonoBehaviour {
	
	public Animator animator;

	private UnityEngine.AI.NavMeshAgent agent;
	private int movingParameterHash;

	void Awake () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		movingParameterHash = Animator.StringToHash("isMoving");
	}

	void Update () {
		if (animator == null) return;

		bool isMoving = (agent.velocity != Vector3.zero);
		animator.SetBool(movingParameterHash, isMoving);
	}
}
