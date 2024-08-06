using System;
using System.Collections.Generic;

namespace game_checkers
{
    public abstract class CharPiece : Piece
    {
        public int BaseAbility { get; }
        public bool WhichAbility { get; }

        protected CharPiece(int id, CharType type, Colour colour, int baseAbility, bool whichAbility)
            : base(id, type, colour)
        {
            BaseAbility = baseAbility;
            WhichAbility = whichAbility;
        }

        public int GetCurrentAbility() => BaseAbility;

        public int GetUpgrade() => BaseAbility + 1;
    }
}