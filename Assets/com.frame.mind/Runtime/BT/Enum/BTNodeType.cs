namespace MortiseFrame.Mind {

    public enum BTNodeType : byte {

        None,

        // - Composite Node
        Sequence,
        Fallback,
        Random,
        Parallel,

        // - Decorator Node
        Repeat,
        Delay,

        // - Element Node
        Action,
        Condition,

    }
}
