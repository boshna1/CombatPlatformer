using System.Collections.Generic;
using UnityEngine;

public class _ComboValues : MonoBehaviour
{
    [SerializeField] int CountGroundedHorizontal;
    [SerializeField] int CountGroundedUp;
    [SerializeField] int CountGroundedDown;
    [SerializeField] int CountAirborneHorizontal;
    [SerializeField] int CountAirborneUp;
    [SerializeField] int CountAirborneDown;

    public Dictionary<_PlayerBaseClass.Combo, int> _comboDictrionary;

    private void Awake()
    {
        _comboDictrionary = new Dictionary<_PlayerBaseClass.Combo, int>
        {
            [_PlayerBaseClass.Combo.HorizontalGrounded] = CountGroundedHorizontal,
            [_PlayerBaseClass.Combo.HorizontalAirborne] = CountAirborneHorizontal,
            [_PlayerBaseClass.Combo.UpGrounded] = CountGroundedUp,
            [_PlayerBaseClass.Combo.DownGrounded] = CountGroundedDown,
            [_PlayerBaseClass.Combo.UpAirborne] = CountAirborneUp,
            [_PlayerBaseClass.Combo.DownAirborne] = CountAirborneDown,
        };
    }

}
