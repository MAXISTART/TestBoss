using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeBoss {
    public class SnakeBoss : MonoBehaviour
    {
        private List<BehaviorDesigner.Runtime.Tasks.Action> _tmpSkillSets;
        private int _currentTraceIdx = 0;

        void Awake()
        {
            BehaviorTreeInit();
            _tmpSkillSets = new List<BehaviorDesigner.Runtime.Tasks.Action>();
        }

        void BehaviorTreeInit()
        {
            // 给行为树设置局部变量，让行为树中的所有任务节点均能访问到transform
            BehaviorTree behaviorTree = GetComponent<BehaviorTree>();
            behaviorTree.GetVariable("SnakeBossTF").SetValue(transform);
        }


        #region BossAction
        /// <summary>
        /// 飞行浮动
        /// </summary>
        /// <returns></returns>
        public TaskStatus FlyUp()
        {
            Debug.Log("boss fly up");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 等待
        /// </summary>
        /// <param name="seconds">等待几秒</param>
        /// <returns></returns>
        public TaskStatus IdleForSeconds(float seconds) {
            Debug.Log($"boss idle for {seconds}s");
            return TaskStatus.Running;
        }

        /// <summary>
        /// 发射子弹A
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootBullet_A()
        {
            Debug.Log("boss shoot bullet_A");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 发射子弹B
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootBullet_B()
        {
            Debug.Log("boss shoot bullet_B");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 发射子弹C1
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootBullet_C_1()
        {
            Debug.Log("boss shoot bullet_C_1");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 发射子弹C2
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootBullet_C_2()
        {
            Debug.Log("boss shoot bullet_C_2");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 红外线A
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootLazer_A()
        {
            Debug.Log("boss shoot lazer_A");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 红外线B
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootLazer_B()
        {
            Debug.Log("boss shoot lazer_B");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 召唤1
        /// </summary>
        /// <returns></returns>
        public TaskStatus CallMonsters_1()
        {
            Debug.Log("boss callMonsters_1");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 召唤2
        /// </summary>
        /// <returns></returns>
        public TaskStatus CallMonsters_2()
        {
            Debug.Log("boss callMonsters_2");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 横扫
        /// </summary>
        /// <returns></returns>
        public TaskStatus Swpie()
        {
            Debug.Log("boss swipe");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 分离
        /// </summary>
        /// <returns></returns>
        public TaskStatus Detach() {
            Debug.Log("boss detach");
            return TaskStatus.Success;
        }

        /// <summary>
        /// 技能集合
        /// </summary>
        /// <param name="actions">技能集合中的技能</param>
        /// <returns></returns>
        public TaskStatus SkillSet(BehaviorDesigner.Runtime.Tasks.Action[] actions) {
            Debug.Log("boss doing skillSet");
            if (_tmpSkillSets.Count == 0)
            {
                _tmpSkillSets = new List<BehaviorDesigner.Runtime.Tasks.Action>(actions); 
            }
            // 从剩余的技能中选取一个执行
            int idx = UnityEngine.Random.Range(0, _tmpSkillSets.Count);
            var skill = _tmpSkillSets[idx];
            // 从set中删除技能
            _tmpSkillSets.RemoveAt(idx);

            return skill.OnUpdate();
        }

        /// <summary>
        /// 技能路径
        /// </summary>
        /// <param name="sequences">所有技能路径</param>
        /// <returns></returns>
        public TaskStatus SkillTrace(BehaviorDesigner.Runtime.Tasks.Sequence[] traces)
        {
            Debug.Log("boss doing skillTrace");
            // 按照顺序执行所有的sequence，_currentTraceIdx 在进入路径选择时会先重置（比如：对于第一阶段，在等待上个攻击结束的上个节点就重置）
            int previousIdx = _currentTraceIdx;
            _currentTraceIdx = (previousIdx + 1) % traces.Length;
            return traces[previousIdx].OnUpdate();
        }
        public TaskStatus ResetTraceIdx() {
            _currentTraceIdx = 0;
            return TaskStatus.Success;
        }




        #endregion

        #region BossCondition

        public TaskStatus GetHurt()
        {
            Debug.Log("boss get hurt");
            return TaskStatus.Success;
        }
        /// <summary>
        /// 统计背部受击次数是否大于等于val（c1>=val?）
        /// </summary>
        /// <param name="val">受击次数阈值</param>
        /// <returns></returns>
        public TaskStatus C1_gte(int val)
        {
            Debug.Log($"boss C1_gte {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// 统计受到的伤害是否大于等于val（c2>=val?）
        /// </summary>
        /// <param name="val">受到的伤害的阈值</param>
        /// <returns></returns>
        public TaskStatus C2_gte(int val)
        {
            Debug.Log($"boss C2_gte {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// 统计受到的伤害是否大于val（c2>val?）
        /// </summary>
        /// <param name="val">受到的伤害的阈值</param>
        /// <returns></returns>
        public TaskStatus C2_gt(int val)
        {
            Debug.Log($"boss C2_gt {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// 统计受到的伤害是否小于等于val（c2<=val?）
        /// </summary>
        /// <param name="val">受到的伤害的阈值</param>
        /// <returns></returns>
        public TaskStatus C2_lse(int val)
        {
            Debug.Log($"boss C2_lse {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// 统计受到的伤害是否小于val（c2<val?）
        /// </summary>
        /// <param name="val">受到的伤害的阈值</param>
        /// <returns></returns>
        public TaskStatus C2_ls(int val)
        {
            Debug.Log($"boss C2_ls {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// 统计受到的伤害是否等于val（c2==val?）
        /// </summary>
        /// <param name="val">受到的伤害的阈值</param>
        /// <returns></returns>
        public TaskStatus C2_eq(int val)
        {
            Debug.Log($"boss C2_eq {val}");
            return TaskStatus.Success;
        }


        #endregion

    }
}
