using TarodevController;
using UnityEngine;

[CreateAssetMenu(menuName = "Masks/Dash Mask")]
public class DashMask : Mask
{
    public override void Activate(GameObject owner)
    {
        CharacterMovement controller = owner.GetComponent<CharacterMovement>();
        controller._activateDash = true;
    }

    public override void Deactivate(GameObject owner)
    {
        CharacterMovement controller = owner.GetComponent<CharacterMovement>();
        controller._activateDash = false;
    }
}
 