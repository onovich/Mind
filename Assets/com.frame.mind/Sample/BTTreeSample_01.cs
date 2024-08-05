using MortiseFrame.Mind;
using UnityEngine;
using System;

namespace MortiseFrame.Sample {

    public class BTTreeSample_01 : MonoBehaviour {

        BTTree tree;

        private void Start() {
            Test1();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                tree.Tick();
                Debug.Log("Tick Result =" + BTContext.blackBoardValue);
            }
        }

        public void Test1() {

            this.tree = new BTTree();
            var sequence1 = BTNodeFactory.CreatCompositeNode_Sequence();
            var fallback1 = BTNodeFactory.CreatCompositeNode_Fallback();

            var action1 = BTNodeFactory.CreatElementNode_Action(new Add1_Until10());
            var action2 = BTNodeFactory.CreatElementNode_Action(new Minus100_Until100());
            var action3 = BTNodeFactory.CreatElementNode_Action(new Add1000_AtOnce());

            var condition1 = BTNodeFactory.CreatElementNode_Condition(Condition_SmallerThan10);
            var condition2 = BTNodeFactory.CreatElementNode_Condition(Condition_LagerThan10);

            //判断是否比10大，仅不满足时执行后续节点
            fallback1.Attach(condition2);

            fallback1.Attach(sequence1);

            //加1，直到比10大
            sequence1.Attach(action1);

            //满足{大于等于10}则加1000
            sequence1.Attach(action3);

            //满足{大于1000}则减100，直到比100小
            sequence1.Attach(action2);

            tree.SetRoot(fallback1);
            tree.Activate();

        }

        bool Condition_SmallerThan10() {
            if (BTContext.blackBoardValue < 10) {
                Debug.Log("Condition_SmallerThan10");
                return true;
            }
            return false;
        }

        bool Condition_DoOnce() {
            Debug.Log("Condition_DoOnce");
            return true;
        }

        bool Condition_SmallerThan100() {
            if (BTContext.blackBoardValue < 100) {
                Debug.Log("Condition_SmallerThan100");
                return true;
            }
            return false;
        }

        bool Condition_LagerThan10() {
            if (BTContext.blackBoardValue > 10) {
                Debug.Log("Condition_LagerThan10");
                return true;
            }
            return false;
        }

        // 加1，直到比10大

        static class BTContext {
            public static int blackBoardValue = 0;
        }

        class Add1_Until10 : IBTAction {
            public void OnEnter() { }

            public bool OnExcute() {
                if (BTContext.blackBoardValue > 10) {
                    return true;
                }
                BTContext.blackBoardValue += 1;
                return false;
            }

            public void OnExit() { }

        }

        //减100，直到比100小
        class Minus100_Until100 : IBTAction {
            public void OnEnter() { }

            public bool OnExcute() {
                if (BTContext.blackBoardValue < 100) {
                    return true;
                }
                BTContext.blackBoardValue -= 100;
                return false;
            }

            public void OnExit() { }

        }

        //加1000
        class Add1000_AtOnce : IBTAction {
            public void OnEnter() { }

            public bool OnExcute() {
                if (BTContext.blackBoardValue > 1000) {
                    return true;
                }
                BTContext.blackBoardValue += 1000;
                return false;
            }

            public void OnExit() { }

        }

        // 减1，直到比0小
        class Minus1_Until0 : IBTAction {
            public void OnEnter() { }

            public bool OnExcute() {
                if (BTContext.blackBoardValue <= 0) {
                    return true;
                }
                BTContext.blackBoardValue -= 1;
                return false;
            }

            public void OnExit() { }

        }

        bool Donothing() { return true; }



    }
}
