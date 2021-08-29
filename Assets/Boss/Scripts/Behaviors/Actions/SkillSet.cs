using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

namespace SnakeBoss
{
    [TaskCategory("SnakeBoss")]
    public class SkillSet : Action
    {
        public SharedTransform snakeBossTF;
        public Action[] Sets;

        private SnakeBoss boss;

        public override void OnAwake()
        {
            boss = snakeBossTF.Value.GetComponent<SnakeBoss>();
        }

        public override TaskStatus OnUpdate()
        {
            return boss.SkillSet(Sets);
        }
    }
}
