using TarodevController;
using UnityEngine;

[CreateAssetMenu(menuName = "Masks/Duble Jump Mask")]
public class DubleJumpMask : Mask
{
    public float speedMultiplier = 1.5f;

    public override void Activate(GameObject owner)
    {
        owner.GetComponent<CharacterMovement>()._activateDubleJump = true;
    }

    public override void Deactivate(GameObject owner)
    {
        owner.GetComponent<CharacterMovement>()._activateDubleJump = false;
    }
}
