using UnityEngine;

public class Resource : MonoBehaviour
{
	public ItemData itemToGive;
	public int quantityPerHit = 1;
	public int capacity;

	public void Gather(Vector3 hitPoint, Vector3 hitNormal)
	{
		for(int i = 0; i < quantityPerHit; i++)
		{
			if (capacity <= 0) break;
			capacity--;
			Instantiate(itemToGive.dropPrefab, hitPoint + (hitNormal * -0.7f), Quaternion.LookRotation(hitNormal, Vector3.up));
		}
	}
}