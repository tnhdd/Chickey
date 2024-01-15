using UnityEngine;

public class ShovelController : MonoBehaviour
{
    public PlayerAttack playerAttack;

    public bool attackedOrc;
    PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Orc" && playerAttack.isAttacking)
        {
            attackedOrc = true;
        }
    }
}
