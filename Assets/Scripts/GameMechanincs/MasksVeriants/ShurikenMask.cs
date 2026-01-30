using TarodevController;
using UnityEngine;

[CreateAssetMenu(menuName = "Masks/Shuriken Mask")]
public class ShurikenMask : Mask
{
    [SerializeField] public ObjectPool ShurikcanPool;

    public override void Activate(GameObject owner)
    {
        CombatController combatController = owner.GetComponent<CombatController>();
        combatController.Amunition = ShurikcanPool;
        combatController._RangeAttack = true;
    }

    public override void Deactivate(GameObject owner)
    {
        CombatController combatController = owner.GetComponent<CombatController>();
        combatController._RangeAttack = false;
    }
}
 