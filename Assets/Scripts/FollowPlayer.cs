using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	public PlayerController player;
	public Vector3 offset;

	public void Start()
	{
		if (player != null)
		{
			player = FindObjectOfType<PlayerController>();
			offset = new Vector3(-1, 9, -4);
		}
	}

	// Update is called once per frame
	public void Update()
	{
		if (player != null)
		{
			var target = player.transform.position + offset;
			var unit = player.gameObject.GetComponent<PlayerUnit>();
			transform.position = Vector3.Lerp(transform.position, target, unit.moveSpeed * Time.deltaTime);
		}
	}
}
