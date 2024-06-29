using UnityEngine;
using System.Collections.Generic;

namespace Reflex.Scripts.Core
{
    public abstract class AContext : Installer
    {
        [SerializeField] private List<Installer> _installers = new List<Installer>();

        public override void InstallBindings(Context context)
        {
            foreach (var installer in _installers)
            {
                installer.InstallBindings(context);
            }
        }
    }
}