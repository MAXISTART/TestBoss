using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace SnakeBoss
{
    [TaskCategory("SnakeBoss")]
    public class ResetTraceIdx : Action
    {
        public SharedTransform snakeBossTF;

        public override void OnAwake()
        {

        }

        public override TaskStatus OnUpdate()
        {
            return snakeBossTF.Value.GetComponent<SnakeBoss>().ResetTraceIdx();
        }
    }
}