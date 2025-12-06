using UnityEngine;

public class ManageNPCDialogueTrigger : MonoBehaviour
{
	private void OnTriggerExit(Collider other)
	{
		GetComponent<BoxCollider>().isTrigger = false;
		GetComponent<BoxCollider>().size = new Vector3(1, 1, 2);
	}
}