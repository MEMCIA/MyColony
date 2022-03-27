using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game
{
    class Pawn: IPawn
    {
        public Pawn(int owner)
        {
            Owner = owner;
        }

        public int Owner { get; set; }
    }
}
