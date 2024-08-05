using MortiseFrame.Mind;
using UnityEngine;
using System;

namespace MortiseFrame.Sample {

    public class EnemySample : MonoBehaviour {

        public int AttackRange;
        public int ChaseRange;
        BTTree tree;
        public PlayerSample player;

        Transform chaseTarget;
        public Transform ChaseTarget => chaseTarget;
        public void SetChaseTarget(Transform value) => chaseTarget = value;
        Transform attackTarget;
        public Transform AttackTarget => attackTarget;
        public void SetAttackTarget(Transform value) => attackTarget = value;
        public Transform OriginHome;
        Transform home;
        public Transform Home => home;
        public void SetHome(Transform value) => home = value;

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, ChaseRange);
            if (chaseTarget != null) {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, chaseTarget.position);
            }
            if (attackTarget != null) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, attackTarget.position);
            }
            if (home != null) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, Home.position);
            }
        }

        public Transform SearchForChaseTarget() {

            Debug.Log("计算追逐对象距离");
            var playerPos = player.transform.position;
            var enemyPos = transform.position;
            var distance = Vector2.Distance(playerPos, enemyPos);
            if (distance < ChaseRange) {
                return player.transform;
            }
            return null;

        }
        public void ReleaseChaseTarget() {
            if (chaseTarget != null) {
                chaseTarget = null;
            }
        }

        public Transform SearchForAttackTarget() {

            var playerPos = player.transform.position;
            var enemyPos = transform.position;
            var distance = Vector2.Distance(playerPos, enemyPos);
            if (distance < AttackRange) {
                return player.transform;
            }
            return null;

        }
        public void ReleaseAttackTarget() {
            if (attackTarget != null) {
                attackTarget = null;
            }
        }

        Transform SearchForEscapeTarget() {

            var playerPos = player.transform.position;
            var enemyPos = transform.position;
            var distance = Vector2.Distance(playerPos, enemyPos);
            if (distance < AttackRange) {
                return player.transform;
            }
            return null;

        }
        public void ReleaseEscapeTarget() {
            if (home != null) {
                home = null;
            }
        }

        public void Chase() {
            if (chaseTarget != null) {
                Debug.Log("追逐");
                transform.position = Vector2.MoveTowards(transform.position, chaseTarget.position, Time.deltaTime);
            }
        }

        public void Attack() {
            if (attackTarget != null) {
                Debug.Log("攻击");
            }
        }

        public void Escape() {
            if (Home != null) {
                transform.position = Vector2.MoveTowards(transform.position, Home.position, Time.deltaTime);
            }
        }

        void CheckPlayerStatus() {

        }

        private void Start() {
            this.tree = new BTTree();
            //var sequence1 = BTNodeFactory.CreatCompositeNode_Sequence();
            var fallback0 = BTNodeFactory.CreatCompositeNode_Fallback();
            var playerStatus = new CheckPlayerStatus();
            playerStatus.Inject(this);

            var fallback1 = BTNodeFactory.CreatCompositeNode_Fallback();
            var chase = new EnemyChaseAction();
            chase.Inject(this);
            var playerInChaseRange = new PlayerInChaseRange();
            playerInChaseRange.Inject(this);
            var chaseAction = BTNodeFactory.CreatElementNode_Action(chase, playerInChaseRange);
            var attack = new EnemyAttackAction();
            attack.Inject(this);
            var playerInAttackRange = new PlayerInAttackRange();
            playerInAttackRange.Inject(this);
            var attackAction = BTNodeFactory.CreatElementNode_Action(attack, playerInAttackRange);
            var escape = new EnemyEscapeAction();
            escape.Inject(this);
            var escapeAction = BTNodeFactory.CreatElementNode_Action(escape, playerStatus);

            fallback0.Attach(escapeAction);
            fallback0.Attach(fallback1);

            fallback1.Attach(attackAction);
            fallback1.Attach(chaseAction);

            tree.SetRoot(fallback0);
            tree.Activate();
        }

        private void Update() {
            tree.Tick();
        }

    }

    public class EnemyEscapeAction : IBTAction {

        EnemySample owner;

        public void Inject(EnemySample owner) {
            this.owner = owner;
        }

        public void OnEnter() { }

        public bool OnExcute() {
            owner.SetHome(owner.OriginHome);
            var homePos = owner.Home.position;
            var enemyPos = owner.transform.position;
            var distance = Vector2.Distance(homePos, enemyPos);
            if (distance < 0.5f) {
                owner.Escape();
                return true;
            }

            owner.Escape();

            return false;
        }

        public void OnExit() {
            owner.ReleaseEscapeTarget();
        }

    }

    public class EnemyChaseAction : IBTAction {

        EnemySample owner;

        public void Inject(EnemySample owner) {
            this.owner = owner;
        }

        public void OnEnter() { }

        public bool OnExcute() {
            var target = owner.SearchForChaseTarget();
            if (target != null) {
                Debug.Log("获取到追逐目标");
                owner.SetChaseTarget(target);
                owner.Chase();
                if (Vector3.Distance(owner.transform.position, target.position) <= owner.AttackRange) {
                    owner.SetAttackTarget(target);
                    return true;
                } else {
                    owner.ReleaseAttackTarget();
                }
            } else {
                owner.ReleaseChaseTarget();
            }
            return false;
        }

        public void OnExit() {
            owner.ReleaseChaseTarget();
        }

    }

    public class PlayerInChaseRange : IBTPrecondition {

        EnemySample owner;

        public void Inject(EnemySample owner) {
            this.owner = owner;
        }

        public bool CanEnter() {
            var target = owner.SearchForChaseTarget();
            if (target != null) {
                return true;
            }
            return false;
        }

    }

    public class PlayerInAttackRange : IBTPrecondition {

        EnemySample owner;

        public void Inject(EnemySample owner) {
            this.owner = owner;
        }

        public bool CanEnter() {
            var target = owner.SearchForAttackTarget();
            if (target != null) {
                return true;
            }
            return false;
        }

    }

    public class CheckPlayerStatus : IBTPrecondition {

        EnemySample owner;

        public void Inject(EnemySample owner) {
            this.owner = owner;
        }

        public bool CanEnter() {
            var status = owner.player.status;
            if (status == 1) {
                return true;
            }
            return false;
        }

    }

    public class EnemyAttackAction : IBTAction {

        EnemySample owner;

        public void Inject(EnemySample owner) {
            this.owner = owner;
        }

        public void OnEnter() { }

        public bool OnExcute() {
            if (owner == null) {
                Debug.LogError("owner is null");
            }
            var target = owner.SearchForAttackTarget();
            if (target != null) {
                Debug.Log("获取到攻击目标");
                owner.SetAttackTarget(target);
                //owner.SetAttackTarget(target);
                owner.Attack();
                return true;
            } else {
                //owner.ReleaseAttackTarget();
            }
            owner.ReleaseAttackTarget();
            return false;
        }

        public void OnExit() {
            owner.ReleaseAttackTarget();
        }

    }

}