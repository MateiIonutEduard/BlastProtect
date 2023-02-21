using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	public Vector3 offset;

	public void Start()
	{
		offset = new Vector3(-1, 9, -4);
	}

    private PlayerUnit GetPlayer()
    {
        int id = MyPlayerPrefs.GetEnemyId();

        var player = FindObjectsOfType<PlayerUnit>()
            .FirstOrDefault(u => u.PlayerId == id);

        return player;
    }

    // Update is called once per frame
    public void Update()
	{
		var player = GetPlayer();

		if (player != null)
		{
			var target = player.transform.position + offset;
			var unit = player.gameObject.GetComponent<PlayerUnit>();
			transform.position = Vector3.Lerp(transform.position, target, unit.moveSpeed * Time.deltaTime);
		}
	}
}
