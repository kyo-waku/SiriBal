using System.Collections;
using UnityEngine;

namespace Generic
{
    // GameScenes
    public enum GameScenes{
        None = 0,
        Top,
        SeriousBalloon,
        Result,
        ScoreBoard,
        Login,
        UserRegistered,
    }


    public enum DefinedErrors{

        Pass = 0, // success.
        E_Fail, // generic fails. Not grouped.
        E_InvalidArg, // invalid argument.
    }
}