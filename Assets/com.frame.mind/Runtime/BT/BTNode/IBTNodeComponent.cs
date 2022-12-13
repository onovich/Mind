using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public interface IBTNodeComponent {

        bool Evaluate();
        void Tick();
        void Reset();

    }
}