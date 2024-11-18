using UnityEngine;

namespace Synthesis.Core
{
    public abstract class AbsEnabledSystemAuthoring : MonoBehaviour
    {
        [SerializeField] protected bool _isSystemEnabled = true;
    }
}