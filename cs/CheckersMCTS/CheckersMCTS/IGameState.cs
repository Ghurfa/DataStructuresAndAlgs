using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersMCTS
{
    internal interface IGameState<T> where T : IGameState<T>
    {
        bool IsWin { get; }
        bool IsTie { get; }
        bool IsLoss { get; }
        bool IsTerminal { get; }
        IReadOnlyList<T> GetChildren();
    }
}
