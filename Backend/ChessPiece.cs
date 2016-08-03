using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public enum TeamColor { WHITE, BLACK }

    public class ChessPiece
    {
        public TeamColor Color { get; private set; }
    }
}
