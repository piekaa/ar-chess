using UnityEngine;

[CreateAssetMenu(fileName = "Pieces", menuName = "Piekoszek/Pieces")]
public class Pieces : ScriptableObject
{
    [SerializeField]
    private Pawn pawn;

    [SerializeField]
    private Rook rook;

    [SerializeField]
    private Knight knight;

    [SerializeField]
    private Bishop bishop;

    [SerializeField]
    private Queen queen;
    
    [SerializeField]
    private King king;

    public Piece Pawn => pawn;

    public Piece Rook => rook;

    public Piece Knight => knight;

    public Piece Bishop => bishop;

    public Piece Queen => queen;

    public Piece King => king;
}
