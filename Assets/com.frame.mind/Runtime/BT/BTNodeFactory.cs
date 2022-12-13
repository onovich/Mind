using System;

namespace MortiseFrame.Mind {

    public abstract class BTNodeFactory<T> {

        static ushort NodeIDRecord;
        public BTNodeFactory() {
            NodeIDRecord = 0;
        }

        public ushort GetNodeID() {
            return NodeIDRecord++;
        }

        // - Composite Node

        public BTNode CreatCompositeNode_Sequence(Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Sequence;
            var sequenceComponent = new BTNodeSequenceComponent();
            sequenceComponent.Inject(node);

            node.SequenceComponent = sequenceComponent;
            node.ID = GetNodeID();

            return node;

        }

        public BTNode CreatCompositeNode_Fallback(Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Fallback;
            node.Precondition = precondition;
            var fallbackComponent = new BTNodeFallbackComponent();
            fallbackComponent.Inject(node);

            node.FallbackComponent = fallbackComponent;
            node.ID = GetNodeID();

            return node;

        }

        public BTNode CreatCompositeNode_Parallel(Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Parallel;
            node.Precondition = precondition;
            var parallelComponent = new BTNodeParallelComponent();
            parallelComponent.Inject(node);

            node.ParallelComponent = parallelComponent;
            node.ID = GetNodeID();

            return node;

        }

        // - Decorator Node

        public BTNode CreatDecoratorNode_Delay(float delay, float dt, Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Delay;
            node.Precondition = precondition;
            var delayComponent = new BTNodeDelayComponent();
            delayComponent.DelayTime = delay;
            delayComponent.dt = dt;
            delayComponent.Inject(node);

            node.DelayComponent = delayComponent;
            node.ID = GetNodeID();

            return node;

        }

        public BTNode CreatDecoratorNode_Loop(ushort repeatCount, Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Repeat;
            node.Precondition = precondition;
            var loopComponent = new BTNodeLoopComponent();
            loopComponent.RepeatCount = repeatCount;
            loopComponent.Inject(node);

            node.LoopComponent = loopComponent;
            node.ID = GetNodeID();

            return node;

        }

        // - Element Node

        public BTNode CreatElementNode_Action(Action OnStart, Func<bool> OnExcute, Action OnExit, Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Action;
            node.Precondition = precondition;
            var actionComponent = new BTNodeActionComponent();
            actionComponent.OnStart = OnStart;
            actionComponent.OnExcute = OnExcute;
            actionComponent.OnExit = OnExit;
            actionComponent.Inject(node);

            node.ActionComponent = actionComponent;
            node.ID = GetNodeID();

            return node;

        }

        public BTNode CreatElementNode_Condition(Func<bool> predicate, Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Condition;
            node.Precondition = precondition;
            var conditionComponent = new BTNodeConditionComponent();
            conditionComponent.Condition = predicate;
            node.ConditionComponent = conditionComponent;
            conditionComponent.Inject(node);
            node.ID = GetNodeID();

            return node;

        }

    }
}
