using System;
using UnityEngine;

namespace Reflex.Scripts.Core
{
    public abstract class AContext : Installer
    {
        [SerializeField] private Installer[] _installers = Array.Empty<Installer>();

        public override void InstallBindings(Context context)
        {
            for (int i = 0, iMax = _installers.Length; i < iMax; i++)
            {
                _installers[i].InstallBindings(context);
            }
        }
    }
}