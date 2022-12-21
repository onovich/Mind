using System;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Mind {

    public class BTNodeFallbackComponent : IBTNodeComponent {

        BTNode node;
        short checkedPtr = 0;
        public void Inject(BTNode node) {
            this.node = node;
        }

        public BTNodeResult Evaluate() {

            // 0. 遍历完子节点，重新遍历
            if (checkedPtr >= node.Children.Count) {
                Reset();
            }

            Debug.Log("FallbackNode.Evaluate()+" + (checkedPtr + 1) + "/" + node.Children.Count);

            // 检测子节点
            var child = node.Children[checkedPtr];
            var childResult = child.Evaluate();

            // 1. 任一子节点返回成功，则返回成功
            if (child.CanEnter() && childResult == BTNodeResult.True) {
                Debug.Log("FallbackNode.Evaluate() return True");
                return BTNodeResult.True;
            }

            // 2. 任一子节点返回失败，则将时钟指针路由到下一个节点
            if ((!child.CanEnter()) || (childResult == BTNodeResult.False)) {
                // 3. 所有子节点返回失败，则返回失败
                if (checkedPtr + 1 >= node.Children.Count) {
                    Debug.Log("FallbackNode.Evaluate() return False");
                    return BTNodeResult.False;
                }
                checkedPtr++;
                Debug.Log("FallbackNode.Evaluate() return Continue1");
                return BTNodeResult.Continue;
            }

            // 4. 任一子节点返回运行中，则返回运行中
            if (childResult == BTNodeResult.Continue) {
                Debug.Log("FallbackNode.Evaluate() return Continue2");
                //child.Reset();
                return BTNodeResult.Continue;
            }

            throw new Exception("FallbackNode.Evaluate() Error");

        }

        public void Tick() {
            //Debug.Log("FallbackNode.Tick()+" + (checkedPtr + 1) + "/" + node.Children.Count);
            node.Children[checkedPtr].Tick();
        }

        public void Reset() {
            Debug.Log("FallbackNode.Reset()");
            checkedPtr = 0;
            node.Children.ForEach(child => child.Reset());
        }

    }
}