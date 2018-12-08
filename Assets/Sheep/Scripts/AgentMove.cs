using UnityEngine;
using System.Collections;

public class AgentMove : MonoBehaviour {

	public Transform targetDestination;

	private const float UPDATE_WAIT_TIME = 0.5f;
	private UnityEngine.AI.NavMeshAgent agent;

	public void Awake () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	public void Start(){
		UpdateAgentDestination();
		StartCoroutine(WaitAndUpdate(UPDATE_WAIT_TIME));
	}

	public void UpdateAgentDestination(){
		agent.destination = targetDestination.position; 
	}

	private IEnumerator WaitAndUpdate(float waitTime){
		while (enabled) {
			yield return new WaitForSeconds(waitTime);
			UpdateAgentDestination();
		}
	}
}
