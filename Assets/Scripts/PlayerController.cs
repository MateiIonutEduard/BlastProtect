using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool canDropBombs = true;
    public bool canMove = true;
    public PlayerUnit player;

    public GameObject bombPrefab;
    private Rigidbody rigidBody;
    private Transform myTransform;

    private DynamicJoystick joystick;
    private ActionJoystick action;

    private Animator animator;
    private List<Bomb> bombs;

    public void Start()
    {
        player = GetComponent<PlayerUnit>();
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;

        animator = myTransform.Find("PlayerModel").GetComponent<Animator>();
        bombs = FindObjectOfType<LoadMap>().BombList;
        action = FindObjectOfType<ActionJoystick>();
        joystick = FindObjectOfType<DynamicJoystick>();
    }

    private bool GetKey(Joypad key)
    {
        if (joystick != null) return joystick.GetKey(key);
        else return false;
    }

    private bool GetActionKey()
    {
        if (action != null) return action.GetActionKey();
        else return false;
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
        if (Input.GetKey (KeyCode.UpArrow) || GetKey(Joypad.UpArrow))
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, player.moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || GetKey(Joypad.LeftArrow))
        {
            rigidBody.velocity = new Vector3 (-player.moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey(KeyCode.DownArrow) || GetKey(Joypad.DownArrow))
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -player.moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Walking", true);
        }

        if (Input.GetKey (KeyCode.RightArrow) || GetKey(Joypad.RightArrow))
        {
            rigidBody.velocity = new Vector3(player.moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Walking", true);
        }

        if (canDropBombs && (Input.GetKeyDown(KeyCode.Space) || GetActionKey()))
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

            var bomb = obj.GetComponent<Bomb>();
            bomb.PlayerId = player.PlayerId;
            bombs.Add(bomb);

            if(player.canKick) obj.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
