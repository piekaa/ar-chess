using System;
using System.Reflection.Emit;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        var message = Piekson.FromJson<LichessMessage>("{\n  \"t\": \"move\",\n  \"v\": 13,\n  \"d\": {\n    \"uci\": \"e1h1\",\n    \"san\": \"O-O\",\n    \"castle\": {\n      \"king\": [\n        \"e1\",\n        \"g1\"\n      ],\n      \"rook\": [\n        \"h1\",\n        \"f1\"\n      ],\n      \"color\": \"white\"\n    },\n    \"fen\": \"rnbq2k1/pppp1rpp/3bpp1n/8/3P3P/3BP3/PPPN1PP1/RNBQ1RK1\",\n    \"ply\": 13,\n    \"dests\": {\n      \"b8\": \"a6c6\",\n      \"f7\": \"f8e7\",\n      \"f6\": \"f5\",\n      \"a7\": \"a6a5\",\n      \"c7\": \"c6c5\",\n      \"d6\": \"e7f8c5b4a3e5f4g3h2\",\n      \"e6\": \"e5\",\n      \"d8\": \"e8f8e7\",\n      \"g7\": \"g6g5\",\n      \"b7\": \"b6b5\",\n      \"h6\": \"g4f5\",\n      \"g8\": \"f8h8\"\n    }\n  }\n}");
        
        Debug.Log(message);
        
    }
}
