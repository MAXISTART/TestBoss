//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SnakeBoss
{
    using UnityEngine;
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    
    
    [TaskCategory("SnakeBoss")]
    public class ShootBullet_B : Action
    {
        
        public SharedTransform snakebossTF;
        
        public override void OnAwake()
        {
        }
        
        public override TaskStatus OnUpdate()
        {
            return snakebossTF.Value.GetComponent<SnakeBoss> ().ShootBullet_B();
        }
    }
}
