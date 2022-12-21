using System;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Mind {

    public class BTNodeParallelComponent : IBTNodeComponent {

        BTNode node;
        bool isAnd;

        public BTNodeParallelComponent(bool isAnd) {
            this.isAnd = isAnd;
        }

        public void Inject(BTNode node) {
            this.node = node;
        }

        public BTNodeResult Evaluate() {
            if (isAnd) {
                return node.Result = EvaluateAnd();
            }
            return node.Result = EvaluateOr();
        }

        public BTNodeResult EvaluateOr() {

            // 检测子节点
            for (int i = 0; i < node.Children.Count; i++) {
                Debug.Log("ParallelNode.Evaluate()+" + (i + 1) + "/" + node.Children.Count);

                var child = node.Children[i];
                var childResult = child.Evaluate();

                // 1. 任一子节点返回成功，则返回成功
                if (child.CanEnter() && childResult == BTNodeResult.True) {
                    return BTNodeResult.True;
                }

                // 2. 所有子节点返回失败，则返回失败
                if ((!child.CanEnter()) || (childResult == BTNodeResult.False)) {
                    if (i + 1 >= node.Children.Count) {
                        return BTNodeResult.True;
                    }
                    continue;
                }

                // 4. 其他情况，则返回运行中
                return BTNodeResult.Continue;

            }
            throw new Exception("ParallelNode.Evaluate() Error");

        }

        public BTNodeResult EvaluateAnd() {

            // 检测子节点
            for (int i = 0; i < node.Children.Count; i++) {
                Debug.Log("ParallelNode.Evaluate()+" + (i + 1) + "/" + node.Children.Count);

                var child = node.Children[i];
                var childResult = child.Evaluate();

                // 1. 任一子节点返回失败，则返回失败
                if (child.CanEnter() || childResult == BTNodeResult.True) {
                    return node.Result = BTNodeResult.True;
                }

                // 2. 所有子节点返回成功，则返回成功
                if ((childResult == BTNodeResult.True)) {
                    if (i + 1 >= node.Children.Count) {
                        return node.Result = BTNodeResult.True;
                    }
                    continue;
                }

                // 4. 其他情况，则返回运行中
                return node.Result = BTNodeResult.Continue;

            }
            throw new Exception("ParallelNode.Evaluate() Error");

        }

        public void Tick() {
            //Debug.Log("ParallelNode.Tick()");
            node.Children.ForEach(child => child.Tick());
        }

        public void Reset() {
            node.Children.ForEach(child => child.Reset());
        }

    }
}