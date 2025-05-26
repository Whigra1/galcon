using JetBrains.Annotations;
using UnityEngine;

namespace Dto
{
    [System.Serializable]
    public class LoginResponceDto
    {
        [SerializeField] public string accessToken;
        [SerializeField] public string refreshToken;
        [SerializeField] public int expiresIn;
        [SerializeField] [CanBeNull] public string tokenType;
    }
}