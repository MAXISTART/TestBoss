using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

namespace SnakeBoss
{
    [TaskCategory("SnakeBoss")]
    public class SkillTrace : Action
    {
        public SharedTransform snakeBossTF;
        public Sequence[] Traces;

        private SnakeBoss boss;
        public override void OnAwake()
        {
        }

        public override TaskStatus OnUpdate()
        {
            return snakeBossTF.Value.GetComponent<SnakeBoss>().SkillTrace(Traces);
        }
    }
}
