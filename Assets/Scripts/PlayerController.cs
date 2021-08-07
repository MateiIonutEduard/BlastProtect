using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public bool canDropBombs = true;
    public bool canMove = true;
    public PlayerUnit player;

    public GameObject bombPrefab;
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    public void Start()
    {
        player = GetComponent<PlayerUnit>();
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        animator = myTransform.Find("PlayerModel").GetComponent<Animator>();
    }

    public void Update()
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        animator.SetBool("Walking", false);
        if (!canMove) return;
        UpdateMovement();
    }

    /// <summary>
    /// Updates Player's movement and facing rotation using the arrow keys and drops bombs.
    /// </summary>
    private void UpdateMovement()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, player.moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidBody.velocity = new Vector3 (-player.moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -player.moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Walking", true);
        }

        if (Input.GetKey (KeyCode.RightArrow))
        {
            rigidBody.velocity = new Vector3(player.moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Walking", true);
        }

        if (canDropBombs && Input.GetKeyDown(KeyCode.Space))
            DropBomb();
    }

    /// <summary>
    /// Drops a bomb beneath the player.
    /// </summary>
    public void DropBomb()
    {
        if (player.bombs > 0 && bombPrefab)
        {
            player.bombs--;

            var obj = Instantiate (bombPrefab,
                new Vector3 (Mathf.RoundToInt(myTransform.position.x), bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)),
                bombPrefab.transform.rotation);

            obj.GetComponent<Bomb>().explode_size = player.explosion_power;
            obj.GetComponent<Bomb>().player = player;

            if(player.canKick) obj.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
