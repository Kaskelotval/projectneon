using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
        {
            player.OnJumpInputDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            player.OnJumpInputUp();
        }

        if (Input.GetButtonDown("Fire1"))
            player.OnAttackInputDown();

        if (Input.GetButtonDown("Sprint"))
            player.OnSprintInputDown();

        if (Input.GetButtonUp("Sprint"))
            player.OnSprintInputUp();
        if (Input.GetButtonDown("Action"))
            player.OnActionInputDown();

        if(Input.GetButtonDown("Restart"))
        {
            player.RestartScene();
        }
            
    }
}
