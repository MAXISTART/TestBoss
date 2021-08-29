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
            // ����Ϊ�����þֲ�����������Ϊ���е���������ڵ���ܷ��ʵ�transform
            BehaviorTree behaviorTree = GetComponent<BehaviorTree>();
            behaviorTree.GetVariable("SnakeBossTF").SetValue(transform);
        }


        #region BossAction
        /// <summary>
        /// ���и���
        /// </summary>
        /// <returns></returns>
        public TaskStatus FlyUp()
        {
            Debug.Log("boss fly up");
            return TaskStatus.Success;
        }

        /// <summary>
        /// �ȴ�
        /// </summary>
        /// <param name="seconds">�ȴ�����</param>
        /// <returns></returns>
        public TaskStatus IdleForSeconds(float seconds) {
            Debug.Log($"boss idle for {seconds}s");
            return TaskStatus.Running;
        }

        /// <summary>
        /// �����ӵ�A
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootBullet_A()
        {
            Debug.Log("boss shoot bullet_A");
            return TaskStatus.Success;
        }

        /// <summary>
        /// �����ӵ�B
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootBullet_B()
        {
            Debug.Log("boss shoot bullet_B");
            return TaskStatus.Success;
        }

        /// <summary>
        /// �����ӵ�C1
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootBullet_C_1()
        {
            Debug.Log("boss shoot bullet_C_1");
            return TaskStatus.Success;
        }

        /// <summary>
        /// �����ӵ�C2
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootBullet_C_2()
        {
            Debug.Log("boss shoot bullet_C_2");
            return TaskStatus.Success;
        }

        /// <summary>
        /// ������A
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootLazer_A()
        {
            Debug.Log("boss shoot lazer_A");
            return TaskStatus.Success;
        }

        /// <summary>
        /// ������B
        /// </summary>
        /// <returns></returns>
        public TaskStatus ShootLazer_B()
        {
            Debug.Log("boss shoot lazer_B");
            return TaskStatus.Success;
        }

        /// <summary>
        /// �ٻ�1
        /// </summary>
        /// <returns></returns>
        public TaskStatus CallMonsters_1()
        {
            Debug.Log("boss callMonsters_1");
            return TaskStatus.Success;
        }

        /// <summary>
        /// �ٻ�2
        /// </summary>
        /// <returns></returns>
        public TaskStatus CallMonsters_2()
        {
            Debug.Log("boss callMonsters_2");
            return TaskStatus.Success;
        }

        /// <summary>
        /// ��ɨ
        /// </summary>
        /// <returns></returns>
        public TaskStatus Swpie()
        {
            Debug.Log("boss swipe");
            return TaskStatus.Success;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public TaskStatus Detach() {
            Debug.Log("boss detach");
            return TaskStatus.Success;
        }

        /// <summary>
        /// ���ܼ���
        /// </summary>
        /// <param name="actions">���ܼ����еļ���</param>
        /// <returns></returns>
        public TaskStatus SkillSet(BehaviorDesigner.Runtime.Tasks.Action[] actions) {
            Debug.Log("boss doing skillSet");
            if (_tmpSkillSets.Count == 0)
            {
                _tmpSkillSets = new List<BehaviorDesigner.Runtime.Tasks.Action>(actions); 
            }
            // ��ʣ��ļ�����ѡȡһ��ִ��
            int idx = UnityEngine.Random.Range(0, _tmpSkillSets.Count);
            var skill = _tmpSkillSets[idx];
            // ��set��ɾ������
            _tmpSkillSets.RemoveAt(idx);

            return skill.OnUpdate();
        }

        /// <summary>
        /// ����·��
        /// </summary>
        /// <param name="sequences">���м���·��</param>
        /// <returns></returns>
        public TaskStatus SkillTrace(BehaviorDesigner.Runtime.Tasks.Sequence[] traces)
        {
            Debug.Log("boss doing skillTrace");
            // ����˳��ִ�����е�sequence��_currentTraceIdx �ڽ���·��ѡ��ʱ�������ã����磺���ڵ�һ�׶Σ��ڵȴ��ϸ������������ϸ��ڵ�����ã�
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
        /// ͳ�Ʊ����ܻ������Ƿ���ڵ���val��c1>=val?��
        /// </summary>
        /// <param name="val">�ܻ�������ֵ</param>
        /// <returns></returns>
        public TaskStatus C1_gte(int val)
        {
            Debug.Log($"boss C1_gte {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// ͳ���ܵ����˺��Ƿ���ڵ���val��c2>=val?��
        /// </summary>
        /// <param name="val">�ܵ����˺�����ֵ</param>
        /// <returns></returns>
        public TaskStatus C2_gte(int val)
        {
            Debug.Log($"boss C2_gte {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// ͳ���ܵ����˺��Ƿ����val��c2>val?��
        /// </summary>
        /// <param name="val">�ܵ����˺�����ֵ</param>
        /// <returns></returns>
        public TaskStatus C2_gt(int val)
        {
            Debug.Log($"boss C2_gt {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// ͳ���ܵ����˺��Ƿ�С�ڵ���val��c2<=val?��
        /// </summary>
        /// <param name="val">�ܵ����˺�����ֵ</param>
        /// <returns></returns>
        public TaskStatus C2_lse(int val)
        {
            Debug.Log($"boss C2_lse {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// ͳ���ܵ����˺��Ƿ�С��val��c2<val?��
        /// </summary>
        /// <param name="val">�ܵ����˺�����ֵ</param>
        /// <returns></returns>
        public TaskStatus C2_ls(int val)
        {
            Debug.Log($"boss C2_ls {val}");
            return TaskStatus.Success;
        }
        /// <summary>
        /// ͳ���ܵ����˺��Ƿ����val��c2==val?��
        /// </summary>
        /// <param name="val">�ܵ����˺�����ֵ</param>
        /// <returns></returns>
        public TaskStatus C2_eq(int val)
        {
            Debug.Log($"boss C2_eq {val}");
            return TaskStatus.Success;
        }


        #endregion

    }
}
