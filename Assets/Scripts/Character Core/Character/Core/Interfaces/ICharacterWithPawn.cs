using CharacterFramework.Core;

namespace Core.Traits {

	public interface ICharacterWithPawn<out TPawn> where TPawn : CharacterPawn {

		TPawn Pawn { get; }

	}
}