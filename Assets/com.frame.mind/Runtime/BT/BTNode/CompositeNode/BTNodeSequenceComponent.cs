using System;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Mind {

    public class BTNodeSequenceComponent : IBTNodeComponent {

        BTNode node;
        short checkedPtr = 0;
        public void Inject(BTNode node) {
            this.node = node;
        }

        public BTNodeResult Evaluate() {

            // 0. 遍历完子节点，重新遍历
            if (checkedPtr >= node.Children.Count) {
                Debug.Log("复位");
                Reset();
            }

            Debug.Log("SequenceNode.Evaluate()+" + (checkedPtr + 1) + "/" + node.Children.Count);

            // 检测子节点
            var child = node.Children[checkedPtr];
            var childResult = child.Evaluate();

            // 1. 任一子节点返回失败，则返回失败
            if ((!child.CanEnter()) || childResult == BTNodeResult.False) {
                Debug.Log("SequenceNode.Evaluate() return False");
                return node.Result = BTNodeResult.False;
            }

            // 2. 任一子节点返回成功，则将时钟指针路由到下一个节点，同时返回运行中
            if (childResult == BTNodeResult.True) {
                // 3. 所有子节点返回成功，则返回成功
                if (checkedPtr + 1 >= node.Children.Count) {
                    Debug.Log("SequenceNode.Evaluate() return True");
                    Reset();
                    return node.Result = BTNodeResult.True;
                }
                Debug.Log("步进");
                checkedPtr++;
                Debug.Log("SequenceNode.Evaluate() return Continue");
                return node.Result = BTNodeResult.Continue;
            }

            // 4. 任一子节点返回运行中，则返回运行中
            if (childResult == BTNodeResult.Continue) {
                Debug.Log("SequenceNode.Evaluate() return Continue");
                //child.Reset();
                return node.Result = BTNodeResult.Continue;
            }

            throw new Exception("SequenceNode.Evaluate() Error"+childResult);

        }

        public void Tick() {
            //Debug.Log("SequenceNode.Tick()+" + (checkedPtr + 1) + "/" + node.Children.Count);
            node.Children[checkedPtr].Tick();
        }

        public void Reset() {
            Debug.Log("SequenceNode.Reset()");
            checkedPtr = 0;
            node.Children.ForEach(child => child.Reset());
        }

    }
}